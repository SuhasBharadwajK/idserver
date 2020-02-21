using IdentityServer4.Models;
using System.Collections;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IDServerPrototype.Login.Core.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "ResourceOwnerClient",
                    ClientId = "roclient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("roclientsecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        "api"
                    },
                },
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Example Client Credentials Client Application",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("superSecretPassword".Sha256())
                    },
                    AllowedScopes = new List<string> { "customAPI.read", "https://localhost:44350/resources" },
                }
            };
        }
    }
}
