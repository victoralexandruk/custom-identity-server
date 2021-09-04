using Dapper;
using Identity.Helpers;
using Identity.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Identity.Data.Repositories
{
    public class ClientRepository : BaseRepository
    {
        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(CustomClient model)
        {
            var old = FindByClientId(model.ClientId);
            if (old == null)
            {
                if (string.IsNullOrWhiteSpace(model.ClientSecret))
                    model.ClientSecret = RandomStringHelper.RandomString(48);

                InsertSql("Client", "ClientId, ClientName, ClientSecret, LogoUri, RequireClientSecret", model);
                UpdateAllowedUris(model);
            }
            else
            {
                UpdateSql("Client", "ClientName, LogoUri, RequireClientSecret", model, "ClientId = @ClientId");
                UpdateAllowedUris(model);
            }
        }

        public void Delete(string clientId)
        {
            using (var db = GetConn())
            {
                db.Execute("DELETE FROM Client WHERE ClientId = @clientId", new { clientId });
            }
        }

        public CustomClient FindByClientId(string clientId)
        {
            using (var db = GetConn())
            {
                return db.Query<CustomClient, string, CustomClient>("SELECT * FROM Client WHERE ClientId = @clientId", (client, allowedUris) =>
                {
                    if (!string.IsNullOrWhiteSpace(allowedUris))
                        client.AllowedUris = JsonSerializer.Deserialize<ICollection<string>>(allowedUris);
                    return client;
                }, new { clientId }, splitOn: "AllowedUris").FirstOrDefault();
            }
        }

        public IEnumerable<CustomClient> GetAll()
        {
            using (var db = GetConn())
            {
                return db.Query<CustomClient>("SELECT ClientId, ClientName, LogoUri FROM Client");
            }
        }

        private void UpdateAllowedUris(CustomClient model)
        {
            UpdateSql("Client", "AllowedUris", new
            {
                ClientId = model.ClientId,
                AllowedUris = JsonSerializer.Serialize(model.AllowedUris)
            }, "ClientId = @ClientId");
        }
    }
}
