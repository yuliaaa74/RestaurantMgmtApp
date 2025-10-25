using Microsoft.Data.SqlClient;
using RestaurantMgmtApp.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        public CustomerRepository(IDbContext db) : base(db) { }

        public async Task<long> CreateAsync(Customer c)
        {
            using var cmd = CreateCommand("sp_Customer_Create");
            cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
            cmd.Parameters.AddWithValue("@LastName", c.LastName);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();
            return (long)outParam.Value;
        }

        public async Task UpdateAsync(Customer c)
        {
            using var cmd = CreateCommand("sp_Customer_Update");
            cmd.Parameters.AddWithValue("@CustomerId", c.CustomerId);
            cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
            cmd.Parameters.AddWithValue("@LastName", c.LastName);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SoftDeleteAsync(long customerId)
        {
            using var cmd = CreateCommand("sp_Customer_SoftDelete");
            cmd.Parameters.AddWithValue("@CustomerId", customerId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Customer> GetByIdAsync(long customerId)
        {
            using var cmd = CreateCommand("sp_Customer_GetById");
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Customer
                {
                    CustomerId = (long)reader["CustomerId"],
                    FirstName = reader["FirstName"] as string,
                    LastName = reader["LastName"] as string
                };
            }
            return null;
        }
    }
}
