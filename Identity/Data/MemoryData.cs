using Identity.Custom.Constants;
using Identity.Models;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.Data
{
    public static class MemoryData
    {
        public static List<User> Users = new List<User>();

        public static List<CustomClient> Clients = new List<CustomClient>
        {
            new CustomClient
            {
                ClientId = "client",
                ClientName = "Demo Client",
                LogoUri = "",
                ClientSecret = new Secret("secret".Sha256()),
                AllowedUris = { "https://localhost:5005" },
                FrontChannelLogoutUri = "https://localhost:5005/Home/Signout"
            }
        };

        public static List<ApiScope> ApiScopes = new List<ApiScope>
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

        public static List<ApiResource> Apis = new List<ApiResource>
        {
            new ApiResource("api", "Custom Identity API")
            {
                ApiSecrets = { new Secret("secret".Sha256()) },
                Scopes = { CustomScopes.CustomIdentityApi, "api.scope1", "api.scope2" }
            }
        };
    }
}
