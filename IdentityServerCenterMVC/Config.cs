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

                new ApiResource("api","My Api")
            };
        }

        public static IEnumerable<Client> GeClients() {
            return new List<Client>()
            {
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
                #region MyRegion
                //new Client
                //{
                //    ClientId = "mvc",
                //    ClientName = "MVC Client",
                //    AllowedGrantTypes = GrantTypes.Implicit,

                //    // where to redirect to after login
                //    RedirectUris = { "http://localhost:5002/signin-oidc" },

                //    // where to redirect to after logout
                //    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                //    AllowedScopes = new List<string>
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile
                //    }
                //},
	            #endregion
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Hybrid Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                        ,"api"
                    },
                    AllowOfflineAccess = true
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
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                //,"api"
            };
        }
    }
}
