using RestaurantMgmtApp.Models;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IMenuItemRepository
    {
        Task<long> CreateAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task SoftDeleteAsync(long menuItemId);
        Task<MenuItem> GetByIdAsync(long menuItemId);
    }
}
