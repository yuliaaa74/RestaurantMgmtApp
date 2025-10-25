using Microsoft.Data.SqlClient;
using System.Data;

namespace RestaurantMgmtApp.Data
{
    public interface IDbContext
    {
        SqlConnection Connection { get; }
        SqlTransaction Transaction { get; set; }

        void Open();
        void Close();
        void Dispose();
    }
}
