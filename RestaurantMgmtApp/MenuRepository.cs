using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class MenuRepository : RepositoryBase, IMenuRepository
    {
        public MenuRepository(IDbContext db) : base(db) { }

        public async Task<long> CreateAsync(Menu m)
        {
            using var cmd = CreateCommand("sp_Menu_Create");
            cmd.Parameters.AddWithValue("@BranchId", m.BranchId);
            cmd.Parameters.AddWithValue("@Name", m.Name);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task UpdateAsync(Menu m)
        {
            using var cmd = CreateCommand("sp_Menu_Update");
            cmd.Parameters.AddWithValue("@MenuId", m.MenuId);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@IsActive", m.IsActive);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SoftDeleteAsync(long menuId)
        {
            using var cmd = CreateCommand("sp_Menu_SoftDelete");
            cmd.Parameters.AddWithValue("@MenuId", menuId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Menu> GetByIdAsync(long menuId)
        {
            using var cmd = CreateCommand("sp_Menu_GetById");
            cmd.Parameters.AddWithValue("@MenuId", menuId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Menu
                {
                    MenuId = (long)reader["MenuId"],
                    BranchId = (long)reader["BranchId"],
                    Name = reader["Name"] as string,
                    IsActive = (bool)reader["IsActive"]
                };
            }
            return null;
        }
    }
}
