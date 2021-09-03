using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Linq;

namespace Identity.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected string _connectionString;

        protected IDbConnection GetConn()
        {
            return new SqliteConnection(_connectionString);
        }

        protected void InsertSql(string table, string columns, object model)
        {
            using (var db = GetConn())
            {
                var cols = columns.Split(",").Select(x => x.Trim());
                db.Execute($"INSERT INTO {table} ({string.Join(",", cols)}) VALUES (@{string.Join(",@", cols)})", model);
            }
        }

        protected void UpdateSql(string table, string columns, object model, string where)
        {
            using (var db = GetConn())
            {
                if (!string.IsNullOrWhiteSpace(where))
                    where = $"WHERE {where}";
                var cols = columns.Split(",").Select(x => x.Trim()).Select(x => $"{x} = @{x}");
                db.Execute($"UPDATE {table} SET {string.Join(",", cols)} {where}", model);
            }
        }
    }
}
