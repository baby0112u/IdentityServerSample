﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerCenter {
    public class Config {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return  new List<ApiResource>()
            {
                new ApiResource("api","My Api")
            };
        }

        public static IEnumerable<Client> GeClients()
        {
            return  new List<Client>()
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
                }
            };
        }
        public static List<TestUser> GetUsers() {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "tanzb",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}
