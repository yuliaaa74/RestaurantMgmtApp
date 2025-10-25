using RestaurantMgmtApp.Models;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IMenuRepository
    {
        Task<long> CreateAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task SoftDeleteAsync(long menuId);
        Task<Menu> GetByIdAsync(long menuId);
    }
}
