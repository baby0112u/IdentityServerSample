using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServerCenterMVC {
    public class Startup {
        
        public  IConfiguration Configuration { get; }
        public  IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region MyRegion

            //services.AddMvcCore()
            //    .AddAuthorization()
            //    .AddJsonFormatters();

            //services.AddMvcCore().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            ////services.Configure<IISOptions>(iis =>
            ////{
            ////    iis.AuthenticationDisplayName = "Windows";
            ////    iis.AutomaticAuthentication = false;
            ////});

            //var builder = services.AddIdentityServer(option =>
            //    {
            //        option.Events.RaiseErrorEvents = true;
            //        option.Events.RaiseInformationEvents = true;
            //        option.Events.RaiseFailureEvents = true;
            //        option.Events.RaiseSuccessEvents = true;
            //    })
            //    .AddInMemoryApiResources(Config.GetApiResources())
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //    .AddInMemoryClients(Config.GeClients())
            //    .AddTestUsers(Config.GetUsers());

            //if (Environment.IsDevelopment())
            //{
            //    builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    throw new Exception("need to configure key material");
            //}


            #endregion

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddSigningCredential(new RsaSecurityKey(RSA.Create("baby0112")))
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                .AddInMemoryIdentityResources(Config.GetIdentityResources()); ;

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }
    }
}
