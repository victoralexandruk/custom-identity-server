using Identity.Custom.Constants;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Models
{
    public class CustomClient : Client
    {
        //public string ClientId { get; set; }
        //public string ClientName { get; set; }
        //public string LogoUri { get; set; }
        //public string FrontChannelLogoutUri { get; set; }

        public Secret ClientSecret { get; set; }

        public ICollection<string> AllowedUris { get; set; } = new List<string>();

        public Client ToIdentityClient()
        {
            var identityClient = new Client
            {
                ClientId = ClientId,
                ClientName = ClientName,
                LogoUri = LogoUri,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                ClientSecrets =
                {
                    ClientSecret
                },

                // where to redirect to after login
                RedirectUris = AllowedUris.SelectMany(x => new string[] { x, $"{x}/signin-oidc" }).ToList(),

                // where to redirect to after logout
                PostLogoutRedirectUris = AllowedUris.SelectMany(x => new string[] { x, $"{x}/signout-callback-oidc" }).ToList(),

                AllowedCorsOrigins = AllowedUris,
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
