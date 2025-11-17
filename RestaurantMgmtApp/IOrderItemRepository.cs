using RestaurantMgmtApp.Models;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IOrderItemRepository
    {
        Task<long> AddAsync(OrderItem orderItem, long? createdByUserId = null);
        Task RemoveAsync(long orderItemId);
    }
}
