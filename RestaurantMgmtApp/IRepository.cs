using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IRepository<T> where T : class
    {
        // Методи залежать від сутності; тут лише загальні
    }
}
