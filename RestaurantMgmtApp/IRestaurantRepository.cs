using RestaurantMgmtApp.Models;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IRestaurantRepository
    {
        Task<long> CreateAsync(Restaurant r, long? userId = null);
        Task UpdateAsync(Restaurant r, long? userId = null);
        Task SoftDeleteAsync(long restaurantId, long? userId = null);
        Task<Restaurant> GetByIdAsync(long restaurantId);
    }
}
