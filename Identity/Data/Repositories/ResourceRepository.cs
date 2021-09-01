using Identity.Models;
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
        private List<CustomApiResource> _apis = MemoryData.Apis;

        // Replce this with your IdentityResource persistence.
        private IEnumerable<IdentityResource> _identityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public IEnumerable<CustomApiResource> GetAllApiResources()
        {
            return _apis;
        }

        public CustomApiResource FindApiResourceById(string id)
        {
            return _apis.FirstOrDefault(x => x.Id == id);
        }

        public Task<IEnumerable<CustomApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var resources = _apis.Where(x => apiResourceNames.Contains(x.Name));
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<CustomApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
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
