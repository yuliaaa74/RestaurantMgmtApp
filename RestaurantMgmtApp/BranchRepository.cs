using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System.Data;

namespace RestaurantMgmtApp.Data
{
    public interface IBranchRepository
    {
        Task<long> CreateAsync(Branch b, long? userId = null);
        Task UpdateAsync(Branch b, long? userId = null);
        Task SoftDeleteAsync(long branchId, long? userId = null);
        Task<Branch> GetByIdAsync(long branchId);
    }

    public class BranchRepository : RepositoryBase, IBranchRepository
    {
        public BranchRepository(IDbContext db) : base(db) { }

        public async Task<long> CreateAsync(Branch b, long? userId = null)
        {
            using var cmd = CreateCommand("sp_Branch_Create");
            cmd.Parameters.AddWithValue("@RestaurantId", b.RestaurantId);
            cmd.Parameters.AddWithValue("@Name", b.Name);
            cmd.Parameters.AddWithValue("@Address", (object)b.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)b.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object)userId ?? DBNull.Value);
            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);
            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task UpdateAsync(Branch b, long? userId = null)
        {
            using var cmd = CreateCommand("sp_Branch_Update");
            cmd.Parameters.AddWithValue("@BranchId", b.BranchId);
            cmd.Parameters.AddWithValue("@Name", b.Name);
            cmd.Parameters.AddWithValue("@Address", (object)b.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)b.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedBy", (object)userId ?? DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SoftDeleteAsync(long branchId, long? userId = null)
        {
            using var cmd = CreateCommand("sp_Branch_SoftDelete");
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            cmd.Parameters.AddWithValue("@ModifiedBy", (object)userId ?? DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Branch> GetByIdAsync(long branchId)
        {
            using var cmd = CreateCommand("sp_Branch_GetById");
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Branch
                {
                    BranchId = (long)reader["BranchId"],
                    RestaurantId = (long)reader["RestaurantId"],
                    Name = reader["Name"] as string,
                    Address = reader["Address"] as string,
                    Phone = reader["Phone"] as string,
                    IsDeleted = (bool)reader["IsDeleted"]
                };
            }
            return null;
        }
    }

}