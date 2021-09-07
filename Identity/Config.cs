using Identity.Custom.Constants;
using Identity.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<Role> Roles =>
            new List<Role>
            {
                new Role
                {
                    Name = "identity.admin",
                    DisplayName = "Admin"
                },
                new Role
                {
                    Name = "identity.user",
                    DisplayName = "User"
                }
            };

        public static IEnumerable<User> Users =>
            new List<User>
            {
                new User
                {
                    UserName = "admin",
                    Password = "admin",
                    FullName = "Admin User",
                    Email = "admin@localhost.local",
                    Role = "identity.admin"
                },
                new User
                {
                    UserName = "demo",
                    Password = "demo",
                    FullName = "Demo User",
                    Email = "demo@localhost.local",
                    Role = "identity.user"
                }
            };

        public static IEnumerable<CustomClient> Clients =>
            new List<CustomClient>
            {
                new CustomClient
                {
                    ClientId = "demo",
                    ClientName = "Demo Client",
                    LogoUri = "",
                    ClientSecret = "demo",
                    AllowedUris = { "https://localhost:5005" },
                    FrontChannelLogoutUri = "https://localhost:5005/Home/Signout",
                    Permissions = new List<ClientPermission>
                    {
                        new ClientPermission
                        {
                            Name = "demo.admin",
                            DisplayName = "Admin",
                            Description = "All permissions"
                        },
                        new ClientPermission
                        {
                            Name = "demo.edit",
                            DisplayName = "Edit",
                            Description = "Edit permission"
                        },
                        new ClientPermission
                        {
                            Name = "demo.read",
                            DisplayName = "Read",
                            Description = "Read permission"
                        }
                    }
                }
            };

        public static IEnumerable<CustomApiResource> Apis =>
            new List<CustomApiResource>
            {
                new CustomApiResource
                {
                    Name = "api",
                    DisplayName = "Custom Identity API"
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
    }
}
