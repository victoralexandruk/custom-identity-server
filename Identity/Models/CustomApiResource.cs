using Identity.Custom.Constants;
using IdentityServer4.Models;
using System;

namespace Identity.Models
{
    public class CustomApiResource : ApiResource
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public ApiResource ToIdentityApiResource()
        {
            var identityApiResource = new ApiResource(this.Name, this.DisplayName)
            {
                Scopes =
                {
                    CustomScopes.CustomIdentityApi
                }
            };
            return identityApiResource;
        }
    }
}
