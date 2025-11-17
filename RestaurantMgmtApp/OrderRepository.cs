using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IDbContext db) : base(db) { }

        public async Task<long> CreateAsync(Order o, long? createdByUserId = null)
        {
            using var cmd = CreateCommand("sp_Order_Create");
            cmd.Parameters.AddWithValue("@BranchId", o.BranchId);
            cmd.Parameters.AddWithValue("@CustomerId", (object)o.CustomerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", o.Status ?? "New");
            cmd.Parameters.AddWithValue("@CreatedBy", (object)createdByUserId ?? DBNull.Value);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task UpdateStatusAsync(long orderId, string status)
        {
            using var cmd = CreateCommand("sp_Order_UpdateStatus");
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@Status", status);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SoftDeleteAsync(long orderId)
        {
            using var cmd = CreateCommand("sp_Order_SoftDelete");
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Order> GetByIdAsync(long orderId)
        {
         
            using var cmd = CreateCommand("sp_Order_GetById");
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Order
                {
                    OrderId = (long)reader["OrderId"],
                    BranchId = (long)reader["BranchId"],
                    CustomerId = reader["CustomerId"] as long?,
                    Status = reader["Status"] as string
                    
                };
            }
            return null;
        }

        
        public async Task<IEnumerable<Order>> GetActiveAsync()
        {
            var orders = new List<Order>();
            using var cmd = CreateCommand("sp_Orders_GetActive"); 

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    OrderId = (long)reader["OrderId"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    Status = reader["Status"] as string,
                    TotalAmount = (decimal)reader["TotalAmount"],
                    BranchName = reader["BranchName"] as string
                });
            }
            return orders;
        }
    }
}
