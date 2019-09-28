using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerCenterMVC {
    public class Config {
        public static IEnumerable<ApiResource> GetApiResources() {
            return new List<ApiResource>()
            {

                #region MyRegion    为什么这样就不行


                //new ApiResource
                //{
                //    Name = "api1",
                //    Description = "Api Resource"
                //    ////Scopes are not a function of the user, only the client the user is using.
                //    //// 暂时还不知道api里定义不同Scopes怎么用
                //    //Scopes =
                //    //{
                //    //    new Scope()
                //    //    {
                //    //        Name = "api.full_access",
                //    //        DisplayName = "Full access to API"
                //    //    },
                //    //    new Scope
                //    //    {
                //    //        Name = "api.read_only",
                //    //        DisplayName = "Read only access to API"
                //    //    }
                //    //}
                //}
                
                #endregion

                new ApiResource("api","My Api Resource")

            };
        }

        public static IEnumerable<Client> GetClients() {
            return new List<Client>()
            {
                #region MyRegion
                /*
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api"}
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "pwdClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api" }
                },
               
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                */
	            #endregion

                new Client
                {
                    ClientId = "hybrid client",
                    ClientName = "Hybrid Client Demo",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret("hybrid secret".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    // AlwaysIncludeUserClaimsInIdToken = true, // 根据情况，是否把用户的Claims 添加到IdToken里面

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId
                        ,IdentityServerConstants.StandardScopes.Phone
                        ,IdentityServerConstants.StandardScopes.Address
                        ,IdentityServerConstants.StandardScopes.Email
                        ,IdentityServerConstants.StandardScopes.Profile
                        ,"api"
                    },
                    AllowOfflineAccess = true // refresh token
                }

            };
        }
        public static List<TestUser> GetUsers() {
            return new List<TestUser>
            {
                // 登录用户名唯一
                //new TestUser
                //{
                //    SubjectId = "1",
                //    Username = "tanzb",
                //    Password = "password"
                //},
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob2",
                    Password = "password"
                },
                new TestUser{
                    SubjectId = "818727",
                    Username = "alice",
                    Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }
            },
                new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    }
                },
                new TestUser{SubjectId = "88421114", Username = "tanzb", Password = "tanzb",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Tan Zhenbiao"),
                        new Claim(JwtClaimTypes.GivenName, "BoTanb"),
                        new Claim(JwtClaimTypes.FamilyName, "Zhenbiao"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId()
                ,new IdentityResources.Phone()
                ,new IdentityResources.Address()
                ,new IdentityResources.Email()
                ,new IdentityResources.Profile()
                //,"api" 
                ,new IdentityResource("roles", "角色", new List<string>{ "role" })
            };
        }
    }
}
