using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class OrderItemRepository : RepositoryBase, IOrderItemRepository
    {
        public OrderItemRepository(IDbContext db) : base(db) { }

        public async Task<long> AddAsync(OrderItem orderItem, long? createdByUserId = null)
        {
            using var cmd = CreateCommand("sp_OrderItem_Add");
            cmd.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
            cmd.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItemId);
            cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            cmd.Parameters.AddWithValue("@UnitPrice", orderItem.UnitPrice); 
            cmd.Parameters.AddWithValue("@CreatedBy", (object)createdByUserId ?? DBNull.Value);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task RemoveAsync(long orderItemId)
        {
            using var cmd = CreateCommand("sp_OrderItem_Remove");
            cmd.Parameters.AddWithValue("@OrderItemId", orderItemId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
