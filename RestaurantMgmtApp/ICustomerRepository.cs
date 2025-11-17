using RestaurantMgmtApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface ICustomerRepository
    {
        Task<long> CreateAsync(Customer customer, long? createdByUserId = null);
        Task UpdateAsync(Customer customer);
        Task SoftDeleteAsync(long customerId);
        Task<Customer> GetByIdAsync(long customerId);
        Task<IEnumerable<Customer>> GetAllActiveAsync();
    }
}
