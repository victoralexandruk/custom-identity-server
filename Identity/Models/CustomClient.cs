using Identity.Custom.Constants;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Models
{
    public class CustomClient : Client
    {
        public new string ClientId { get; set; } = Guid.NewGuid().ToString();

        public string ClientSecret { get; set; }

        public ICollection<string> AllowedUris { get; set; } = new List<string>();

        public Client ToIdentityClient()
        {
            var identityClient = new Client
            {
                ClientId = this.ClientId,
                ClientName = this.ClientName,
                LogoUri = this.LogoUri,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                ClientSecrets =
                {
                    new Secret(this.ClientSecret.Sha256())
                },

                // where to redirect to after login
                RedirectUris = this.AllowedUris.SelectMany(x => new string[] { x, $"{x}/signin-oidc" }).ToList(),

                // where to redirect to after logout
                PostLogoutRedirectUris = this.AllowedUris.SelectMany(x => new string[] { x, $"{x}/signout-callback-oidc" }).ToList(),

                AllowedCorsOrigins = this.AllowedUris,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    CustomScopes.CustomIdentityApi
                },

                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
                RequirePkce = false,
                RequireConsent = false
            };
            return identityClient;
        }
    }
}
