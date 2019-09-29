using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ThirdPartyClientMVC.AuthPolicy;

namespace ThirdPartyClientMVC {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region MyRegion

            //services.Configure<CookiePolicyOptions>(options => {
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = "Cookies";
            //        options.DefaultChallengeScheme = "oidc";
            //    })
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options => {
            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;

            //        options.ClientId = "mvc";
            //        options.SaveTokens = true;
            //    });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = "Cookies";
            //        options.DefaultChallengeScheme = "oidc";
            //    })
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.SignInScheme = "Cookies";
            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;
            //        options.ClientId = "mvc";
            //        options.ClientSecret = "secret";
            //        options.ResponseType = "code id_token";
            //        options.SaveTokens = true;
            //        options.GetClaimsFromUserInfoEndpoint = true;
            //        //options.Scope.Add("api");
            //        options.Scope.Add("api.full_access");
            //        options.Scope.Add("api.read_only");
            //        options.Scope.Add("offline_access");
            //        options.ClaimActions.MapJsonKey("website", "website");
            //    });

            #endregion

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // 注释掉后，默认的名称都改变了
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "hybrid client";
                    options.ClientSecret = "hybrid secret";
                    options.SaveTokens = true;
                    options.ResponseType = "code id_token";

                    options.Scope.Clear();
                    options.GetClaimsFromUserInfoEndpoint = true;   // 从UserInfoEndpoint获取Claims
                    

                    options.Scope.Add("api");
                    options.Scope.Add(OidcConstants.StandardScopes.Address);
                    options.Scope.Add(OidcConstants.StandardScopes.Email);
                    options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                    options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                    options.Scope.Add(OidcConstants.StandardScopes.Phone);
                    options.Scope.Add(OidcConstants.StandardScopes.Profile);
                    options.Scope.Add("roles");
                    options.Scope.Add("locations");

                    options.ClaimActions.MapUniqueJsonKey("role", "role");
                    options.ClaimActions.MapUniqueJsonKey("location", "location");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };

                    

                    // options.ClaimActions.Add("api");
                    // ClaimActions 默认过滤掉的Claims都在这个集合里，如果不需要默认过滤掉，则需要把它从这个集合移除掉
                    // 如果需要过滤掉某个Claims 则只需要把对应的Claims添加到这个集合就可以了

                    //options.ClaimActions.Remove("arm");
                    //options.ClaimActions.Remove("nbf");
                    //options.ClaimActions.Remove("exp");

                });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("SmithInSomewhere", builder =>
                //{
                //    builder.RequireAuthenticatedUser();
                //    builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith");
                //    builder.RequireClaim("location", "somewhere");
                //});
                options.AddPolicy("SmithInSomewhere", builder =>
                    {
                        builder.AddRequirements(new SmithInSomeWhereRequirement());
                    });
            });
            services.AddSingleton<IAuthorizationHandler, SmithInSomeWhereHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
