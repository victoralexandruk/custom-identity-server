using Dapper;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly string _connectionString;

        public PersistedGrantStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected IDbConnection GetConn()
        {
            return new SqliteConnection(_connectionString);
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            using (var db = GetConn())
            {
                return db.QueryAsync<PersistedGrant>(@"SELECT * FROM PersistedGrant WHERE 
                    (SubjectId = @SubjectId OR @SubjectId IS NULL)
                    AND (SessionId = @SessionId OR @SessionId IS NULL)
                    AND (ClientId = @ClientId OR @ClientId IS NULL)
                    AND (Type = @Type OR @Type IS NULL)", filter);
            }
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefaultAsync<PersistedGrant>("SELECT * FROM PersistedGrant WHERE Key = @key", new { key });
            }
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            using (var db = GetConn())
            {
                return db.ExecuteAsync(@"DELETE FROM PersistedGrant WHERE
                    (SubjectId = @SubjectId OR @SubjectId IS NULL)
                    AND (SessionId = @SessionId OR @SessionId IS NULL)
                    AND (ClientId = @ClientId OR @ClientId IS NULL)
                    AND (Type = @Type OR @Type IS NULL)", filter);
            }
        }

        public Task RemoveAsync(string key)
        {
            using (var db = GetConn())
            {
                return db.ExecuteAsync("DELETE FROM PersistedGrant WHERE Key = @key", new { key });
            }
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            //await RemoveAsync(grant.Key);
            using (var db = GetConn())
            {
                var cols = "Key, Type, SubjectId, SessionId, ClientId, Description, CreationTime, Expiration, ConsumedTime, Data".Split(",").Select(x => x.Trim());
                return db.ExecuteAsync($"INSERT INTO PersistedGrant ({string.Join(",", cols)}) VALUES (@{string.Join(",@", cols)})", grant);
            }
        }
    }
}
