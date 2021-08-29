using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data.Repositories
{
    public class ResourceRepository
    {
        // Replce this with your ApiScope persistence.
        private List<ApiScope> _apiScopes = MemoryData.ApiScopes;

        // Replce this with your ApiResource persistence.
        private List<ApiResource> _apis = MemoryData.Apis;

        // Replce this with your IdentityResource persistence.
        private IEnumerable<IdentityResource> _identityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var resources = _apis.Where(x => apiResourceNames.Contains(x.Name));
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = _apis.Where(x => x.Scopes.Any(s => scopeNames.Contains(s)));
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = _apiScopes.Where(x => scopeNames.Contains(x.Name));
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = _identityResources.Where(x => scopeNames.Contains(x.Name));
            return Task.FromResult(resources);
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var resources = new Resources(_identityResources, _apis, _apiScopes);
            return Task.FromResult(resources);
        }
    }
}
