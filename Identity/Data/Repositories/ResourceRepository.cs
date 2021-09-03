using Dapper;
using Identity.Models;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data.Repositories
{
    public class ResourceRepository : BaseRepository
    {
        // Replce this with your ApiScope persistence.
        private List<ApiScope> _apiScopes = MemoryData.ApiScopes;

        // Replce this with your IdentityResource persistence.
        private IEnumerable<IdentityResource> _identityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public ResourceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region ApiResource
        public void SaveApiResource(CustomApiResource model)
        {
            var old = FindApiResourceById(model.Id);
            if (old == null)
            {
                InsertSql("Api", "Id, Name, Secret, DisplayName, Enabled", model);
            }
            else
            {
                UpdateSql("Api", "Name, DisplayName, Enabled", model, "Id = @Id");
            }
        }

        public void DeleteApiResource(string id)
        {
            using (var db = GetConn())
            {
                db.Execute("DELETE FROM Api WHERE Id = @id", new { id });
            }
        }

        public IEnumerable<CustomApiResource> GetAllApiResources()
        {
            using (var db = GetConn())
            {
                return db.Query<CustomApiResource>("SELECT * FROM Api");
            }
        }

        public CustomApiResource FindApiResourceById(string id)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefault<CustomApiResource>("SELECT * FROM Api WHERE Id = @id", new { id });
            }
        }

        public CustomApiResource FindApiResourceByName(string name)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefault<CustomApiResource>("SELECT * FROM Api WHERE Name = @name", new { name });
            }
        }

        public Task<IEnumerable<CustomApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            using (var db = GetConn())
            {
                return db.QueryAsync<CustomApiResource>("SELECT * FROM Api WHERE Name IN @apiResourceNames", new { apiResourceNames });
            }
        }

        public Task<IEnumerable<CustomApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = GetAllApiResources().Where(x => x.ToIdentityApiResource().Scopes.Any(s => scopeNames.Contains(s)));
            return Task.FromResult(resources);
        }
        #endregion

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
            var resources = new Resources(_identityResources, GetAllApiResources(), _apiScopes);
            return Task.FromResult(resources);
        }
    }
}
