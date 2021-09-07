using Dapper;
using Identity.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Identity.Data.Repositories
{
    public class RoleRepository : BaseRepository
    {
        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Role model)
        {
            var old = FindById(model.Id);
            if (old == null)
            {
                InsertSql("Role", "Id, Name, DisplayName", model);
                UpdatePermissions(model);
            }
            else
            {
                UpdateSql("Role", "Name, DisplayName", model, "Id = @Id");
                UpdatePermissions(model);
            }
        }

        public void Delete(string id)
        {
            using (var db = GetConn())
            {
                db.Execute("DELETE FROM Role WHERE Id = @id", new { id });
            }
        }

        public Role FindById(string id)
        {
            using (var db = GetConn())
            {
                return db.Query<Role, string, Role>("SELECT * FROM Role WHERE Id = @id", (role, permissions) =>
                {

                    if (!string.IsNullOrWhiteSpace(permissions))
                        role.Permissions = JsonSerializer.Deserialize<IEnumerable<RolePermission>>(permissions);

                    return role;
                }, new { id }, splitOn: "Permissions").FirstOrDefault();
            }
        }

        public Role FindByName(string name)
        {
            using (var db = GetConn())
            {
                return db.Query<Role, string, Role>("SELECT * FROM Role WHERE Name = @name", (role, permissions) =>
                {

                    if (!string.IsNullOrWhiteSpace(permissions))
                        role.Permissions = JsonSerializer.Deserialize<IEnumerable<RolePermission>>(permissions);

                    return role;
                }, new { name }, splitOn: "Permissions").FirstOrDefault();
            }
        }

        public IEnumerable<Role> GetAll()
        {
            using (var db = GetConn())
            {
                return db.Query<Role>("SELECT Id, Name, DisplayName FROM Role");
            }
        }

        private void UpdatePermissions(Role model)
        {
            UpdateSql("Role", "Permissions", new
            {
                Id = model.Id,
                Permissions = JsonSerializer.Serialize(model.Permissions)
            }, "Id = @Id");
        }
    }
}
