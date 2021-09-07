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

                InsertSql("Client", "ClientId, ClientName, ClientSecret, LogoUri, Enabled", model);
                UpdateAllowedUris(model);
                UpdatePermissions(model);
            }
            else
            {
                UpdateSql("Client", "ClientName, LogoUri, Enabled", model, "ClientId = @ClientId");
                UpdateAllowedUris(model);
                UpdatePermissions(model);
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
                return db.Query<CustomClient, string, string, CustomClient>("SELECT * FROM Client WHERE ClientId = @clientId", (client, allowedUris, permissions) =>
                {
                    if (!string.IsNullOrWhiteSpace(allowedUris))
                        client.AllowedUris = JsonSerializer.Deserialize<ICollection<string>>(allowedUris);
                    
                    if (!string.IsNullOrWhiteSpace(permissions))
                        client.Permissions = JsonSerializer.Deserialize<IEnumerable<ClientPermission>>(permissions);
                    
                    return client;
                }, new { clientId }, splitOn: "AllowedUris,Permissions").FirstOrDefault();
            }
        }

        public IEnumerable<CustomClient> GetAll(bool withPermissions = false)
        {
            using (var db = GetConn())
            {
                if (withPermissions)
                {
                    return db.Query<CustomClient, string, CustomClient>("SELECT ClientId, ClientName, LogoUri, Enabled, Permissions FROM Client", (client, permissions) =>
                    {

                        if (!string.IsNullOrWhiteSpace(permissions))
                            client.Permissions = JsonSerializer.Deserialize<IEnumerable<ClientPermission>>(permissions);

                        return client;
                    }, splitOn: "Permissions");
                }
                return db.Query<CustomClient>("SELECT ClientId, ClientName, LogoUri, Enabled FROM Client");
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

        private void UpdatePermissions(CustomClient model)
        {
            UpdateSql("Client", "Permissions", new
            {
                ClientId = model.ClientId,
                Permissions = JsonSerializer.Serialize(model.Permissions)
            }, "ClientId = @ClientId");
        }
    }
}
