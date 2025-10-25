using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace RestaurantMgmtApp.Data
{
    public class SqlDbContext : IDbContext, IDisposable
    {
        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; set; }

        private readonly string _connectionString;

        public SqlDbContext(string connectionString)
        {
            _connectionString = connectionString;
            Connection = new SqlConnection(_connectionString);
        }

        public void Open()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        public void Close()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Close();
            Connection?.Dispose();
        }
    }
}
