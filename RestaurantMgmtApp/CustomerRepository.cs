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
        public async Task<long> CreateAsync(Customer c, long? createdByUserId = null)
        {
            using var cmd = CreateCommand("sp_Customer_Create");
            cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
            cmd.Parameters.AddWithValue("@LastName", c.LastName);
            cmd.Parameters.AddWithValue("@Phone", (object)c.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)c.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object)createdByUserId ?? DBNull.Value);

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
            cmd.Parameters.AddWithValue("@Phone", (object)c.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)c.Email ?? DBNull.Value);
           
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
                    LastName = reader["LastName"] as string,
                    Phone = reader["Phone"] as string,
                    Email = reader["Email"] as string
                };
            }
            return null;
        }

        
        public async Task<IEnumerable<Customer>> GetAllActiveAsync()
        {
            var customers = new List<Customer>();
            using var cmd = CreateCommand("sp_Customer_GetAllActive"); 

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                customers.Add(new Customer
                {
                    CustomerId = (long)reader["CustomerId"],
                    FirstName = reader["FirstName"] as string,
                    LastName = reader["LastName"] as string,
                    Phone = reader["Phone"] as string,
                    Email = reader["Email"] as string
                });
            }
            return customers;
        }
    
    }
}
