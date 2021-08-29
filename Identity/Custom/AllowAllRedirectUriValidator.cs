using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace Identity.Custom
{
    // allows arbitrary redirect URIs. NEVER USE IN PRODUCTION
    public class AllowAllRedirectUriValidator : IRedirectUriValidator
    {
        public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(true);
        }
    }
}
