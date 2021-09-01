using Identity.Custom.Constants;
using IdentityServer4.Models;
using System;

namespace Identity.Models
{
    public class CustomApiResource : ApiResource
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Secret { get; set; } = Guid.NewGuid().ToString().Sha256();

        public ApiResource ToIdentityApiResource()
        {
            var identityApiResource = new ApiResource(this.Name, this.DisplayName)
            {
                ApiSecrets =
                {
                    new Secret(this.Secret)
                },
                Scopes =
                {
                    CustomScopes.CustomIdentityApi
                }
            };
            return identityApiResource;
        }
    }
}
