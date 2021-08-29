using Identity.Custom.Constants;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
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

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api", "Custom Identity API")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },
                    Scopes = { CustomScopes.CustomIdentityApi, "api.scope1", "api.scope2" }
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
    }
}
