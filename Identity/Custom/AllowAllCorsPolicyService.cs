using IdentityServer4.Services;
using System.Threading.Tasks;

namespace Identity.Custom
{
    // allows arbitrary CORS origins. NEVER USE IN PRODUCTION
    public class AllowAllCorsPolicyService : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(true);
        }
    }
}
