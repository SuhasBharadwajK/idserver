using IdentityServer4;
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
                    AllowedScopes = new List<string> { "customAPI.read", "https://localhost:44350/resources", "http://localhost:5100/resources" },
                },
                new Client {
                    ClientId = "openIdConnectClient",
                    ClientName = "Example Implicit Client Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI.write"
                    },
                    RedirectUris = new List<string> {"http://localhost:5100/signin-oidc", "https://localhost:44350/signin-oidc"},
                    PostLogoutRedirectUris = new List<string> {"http://localhost:5100", "https://localhost:44350"}
                },
                new Client {
                    ClientId = "1b7449f9-c6e4-44c3-bde3-7001b4db1b76",
                    ClientName = "Example Implicit Client Application",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI.write"
                    },
                    RedirectUris = new List<string> {"https://localhost:44350/Account/ExternalLoginCallback"},
                    PostLogoutRedirectUris = new List<string> {"http://localhost:5100", "https://localhost:44350"}
                }
            };
        }
    }
}
