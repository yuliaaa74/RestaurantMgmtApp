using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class RestaurantRepository : RepositoryBase, IRestaurantRepository
    {
        public RestaurantRepository(IDbContext db) : base(db) { }

        public async Task<long> CreateAsync(Restaurant r, long? userId = null)
        {
            using var cmd = CreateCommand("sp_Restaurant_Create");
            cmd.Parameters.AddWithValue("@Name", r.Name);
            cmd.Parameters.AddWithValue("@Address", (object)r.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)r.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object)userId ?? DBNull.Value);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task UpdateAsync(Restaurant r, long? userId = null)
        {
            using var cmd = CreateCommand("sp_Restaurant_Update");
            cmd.Parameters.AddWithValue("@RestaurantId", r.RestaurantId);
            cmd.Parameters.AddWithValue("@Name", r.Name);
            cmd.Parameters.AddWithValue("@Address", (object)r.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)r.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedBy", (object)userId ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SoftDeleteAsync(long restaurantId, long? userId = null)
        {
            using var cmd = CreateCommand("sp_Restaurant_SoftDelete");
            cmd.Parameters.AddWithValue("@RestaurantId", restaurantId);
            cmd.Parameters.AddWithValue("@ModifiedBy", (object)userId ?? DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Restaurant> GetByIdAsync(long restaurantId)
        {
            using var cmd = CreateCommand("sp_Restaurant_GetById");
            cmd.Parameters.AddWithValue("@RestaurantId", restaurantId);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var r = new Restaurant
                {
                    RestaurantId = (long)reader["RestaurantId"],
                    Name = reader["Name"] as string,
                    Address = reader["Address"] as string,
                    Phone = reader["Phone"] as string,
                    IsDeleted = (bool)reader["IsDeleted"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : (long?)reader["CreatedBy"]
                };
                return r;
            }
            return null;
        }
    }
}
