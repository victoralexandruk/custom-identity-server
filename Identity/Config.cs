using Identity.Custom.Constants;
using Identity.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<User> Users =>
            new List<User>
            {
                new User
                {
                    Id = "1",
                    UserName = "admin",
                    Password = "admin",
                    FullName = "Admin User",
                    Email = "admin@localhost.local",
                    Roles = new List<IdentityUserRole<string>> {
                        new IdentityUserRole<string>
                        {
                            RoleId = "identity.admin"
                        },
                        new IdentityUserRole<string>
                        {
                            RoleId = "identity.user"
                        }
                    }
                },
                new User
                {
                    Id = "2",
                    UserName = "demo",
                    Password = "demo",
                    FullName = "Demo User",
                    Email = "demo@localhost.local",
                    Roles = new List<IdentityUserRole<string>> {
                        new IdentityUserRole<string>
                        {
                            RoleId = "identity.user"
                        }
                    }
                }
            };

        public static IEnumerable<CustomClient> Clients =>
            new List<CustomClient>
            {
                new CustomClient
                {
                    ClientId = "local",
                    ClientName = "Local Client",
                    RequireClientSecret = false
                },
                new CustomClient
                {
                    ClientId = "demo",
                    ClientName = "Demo Client",
                    LogoUri = "",
                    ClientSecret = "demo".Sha256(),
                    AllowedUris = { "https://localhost:5005" },
                    FrontChannelLogoutUri = "https://localhost:5005/Home/Signout"
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(CustomScopes.CustomIdentityApi, "Custom Identity API"),
            
                // more formal
                new ApiScope("api.scope1"),
                new ApiScope("api.scope2"),
            
                // scope without a resource
                new ApiScope("scope2"),
            
                // policyserver
                new ApiScope("policyserver.runtime"),
                new ApiScope("policyserver.management")
            };

        public static IEnumerable<CustomApiResource> Apis =>
            new List<CustomApiResource>
            {
                new CustomApiResource
                {
                    Name = "api",
                    DisplayName = "Custom Identity API",
                    Secret = "secret".Sha256()
                }
            };
    }
}
