using RestaurantMgmtApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestaurantMgmtApp.Data
{
    public interface IOrderRepository
    {
        Task<long> CreateAsync(Order order, long? createdByUserId = null);
        Task UpdateStatusAsync(long orderId, string status);
        Task SoftDeleteAsync(long orderId);
        Task<Order> GetByIdAsync(long orderId);
        Task<IEnumerable<Order>> GetActiveAsync();
    }
}
