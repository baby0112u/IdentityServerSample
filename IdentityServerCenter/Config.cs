﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Models;

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
                }
            };
        }
    }
}
