using IdentityServer4.Models;
using System.Collections;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IDServerPrototype.Login.Core.Configuration
{
    internal class Scopes
    {
        public static IEnumerable Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = StandardScopes.OpenId,
                    DisplayName = "OpenId",
                    Description = "OpenId scope",
                },
                new Scope
                {
                    Name = StandardScopes.Profile,
                    DisplayName = "Profile",
                    Description = "Profile scope",
                },
                new Scope
                {
                    Name = "api",
                    DisplayName = "Bank API",
                    Description = "Bank API scope",
                }
            };
        }
    }
}
