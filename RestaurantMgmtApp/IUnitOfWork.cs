using System;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRestaurantRepository Restaurants { get; }
        IBranchRepository Branches { get; }
       

        void BeginTransaction();
        Task CommitAsync();
        void Rollback();
    }
}
