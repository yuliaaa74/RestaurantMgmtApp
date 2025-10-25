using RestaurantMgmtApp.Models;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IOrderItemRepository
    {
        Task<long> AddAsync(OrderItem orderItem);
        Task RemoveAsync(long orderItemId);
    }
}
