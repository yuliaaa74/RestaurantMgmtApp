using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public abstract class RepositoryBase
    {
        protected readonly IDbContext _db;

        protected RepositoryBase(IDbContext db)
        {
            _db = db;
        }

        protected SqlCommand CreateCommand(string storedProcName)
        {
            var cmd = _db.Connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            if (_db.Transaction != null)
                cmd.Transaction = _db.Transaction;
            return (SqlCommand)cmd;
        }
    }
}
