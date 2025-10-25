using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class MenuItemRepository : RepositoryBase, IMenuItemRepository
    {
        public MenuItemRepository(IDbContext db) : base(db) { }

        public async Task<long> CreateAsync(MenuItem m)
        {
            using var cmd = CreateCommand("sp_MenuItem_Create");
            cmd.Parameters.AddWithValue("@MenuId", m.MenuId);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Price", m.Price);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task UpdateAsync(MenuItem m)
        {
            using var cmd = CreateCommand("sp_MenuItem_Update");
            cmd.Parameters.AddWithValue("@MenuItemId", m.MenuItemId);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Price", m.Price);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SoftDeleteAsync(long menuItemId)
        {
            using var cmd = CreateCommand("sp_MenuItem_SoftDelete");
            cmd.Parameters.AddWithValue("@MenuItemId", menuItemId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<MenuItem> GetByIdAsync(long menuItemId)
        {
            using var cmd = CreateCommand("sp_MenuItem_GetById");
            cmd.Parameters.AddWithValue("@MenuItemId", menuItemId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new MenuItem
                {
                    MenuItemId = (long)reader["MenuItemId"],
                    MenuId = (long)reader["MenuId"],
                    Name = reader["Name"] as string,
                    Price = (decimal)reader["Price"]
                };
            }
            return null;
        }
    }
}
