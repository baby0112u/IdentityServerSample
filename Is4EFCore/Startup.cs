﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using IdentityServer4.Quickstart.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using IdentityServer4;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;

namespace Is4EFCore {
    public class Startup {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration config, IHostingEnvironment env) {
            Configuration = config;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.Configure<IISOptions>(options => {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            #region Sqlite

            //var identityServer = services.AddIdentityServer(options =>
            //{
            //    options.Events.RaiseErrorEvents = true;
            //    options.Events.RaiseInformationEvents = true;
            //    options.Events.RaiseFailureEvents = true;
            //    options.Events.RaiseSuccessEvents = true;
            //})
            //    .AddTestUsers(TestUsers.Users)
            //    // this adds the config data from DB (clients, resources, CORS)
            //    .AddConfigurationStore(options =>
            //    {
            //        options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
            //    })
            //    // this adds the operational data from DB (codes, tokens, consents)
            //    .AddOperationalStore(options =>
            //    {
            //        options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);

            //        // this enables automatic token cleanup. this is optional.
            //        options.EnableTokenCleanup = true;
            //    });

            #endregion

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var identityServer = services.AddIdentityServer()
                .AddTestUsers(TestUsers.Users)
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options => {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options => {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                });


            services.AddAuthentication()
                .AddGoogle(options => {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });

            if (Environment.IsDevelopment()) {
                identityServer.AddDeveloperSigningCredential();
            } else {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app) {

            InitializeDatabase(app);

            if (Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
        private void InitializeDatabase(IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>
                ().CreateScope()) {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().
                    Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService
                    <ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any()) {
                    foreach (var client in Config.GetClients()) {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.IdentityResources.Any()) {
                    foreach (var resource in Config.GetIdentityResources()) {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.ApiResources.Any()) {
                    foreach (var resource in Config.GetApis()) {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
