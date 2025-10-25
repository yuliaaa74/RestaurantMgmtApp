using RestaurantMgmtApp.Models;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface ICustomerRepository
    {
        Task<long> CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task SoftDeleteAsync(long customerId);
        Task<Customer> GetByIdAsync(long customerId);
    }
}
