using Dapper;
using Identity.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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
            }
            else
            {
                UpdateSql("Role", "Name, DisplayName", model, "Id = @Id");
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
                return db.QueryFirstOrDefault<Role>("SELECT * FROM Role WHERE Id = @id", new { id });
            }
        }

        public Role FindByName(string name)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefault<Role>("SELECT * FROM Role WHERE Name = @name", new { name });
            }
        }

        public IEnumerable<Role> GetAll()
        {
            using (var db = GetConn())
            {
                return db.Query<Role>("SELECT * FROM Role");
            }
        }
    }
}
