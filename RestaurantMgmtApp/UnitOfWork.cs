using RestaurantMgmtApp.Models;
using System;
using System.Threading.Tasks;

namespace RestaurantMgmtApp.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbContext _db;
        private bool _disposed = false;

        public IRestaurantRepository Restaurants { get; private set; }
        public IBranchRepository Branches { get; private set; }
        public IMenuRepository Menus { get; private set; }
        public IMenuItemRepository MenuItems { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IOrderItemRepository OrderItems { get; private set; }

        public UnitOfWork(IDbContext db)
        {
            _db = db;

            Restaurants = new RestaurantRepository(_db);
            Branches = new BranchRepository(_db);
            Menus = new MenuRepository(_db);
            MenuItems = new MenuItemRepository(_db);
            Customers = new CustomerRepository(_db);
            Orders = new OrderRepository(_db);
            OrderItems = new OrderItemRepository(_db);
        }

        public void BeginTransaction()
        {
            _db.Open();
            _db.Transaction = _db.Connection.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            try
            {
                _db.Transaction?.Commit();
                await Task.CompletedTask;
            }
            catch
            {
                _db.Transaction?.Rollback();
                throw;
            }
            finally
            {
                _db.Transaction = null;
                _db.Close();
            }
        }

        public void Rollback()
        {
            _db.Transaction?.Rollback();
            _db.Transaction = null;
            _db.Close();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _db?.Dispose();
                _disposed = true;
            }
        }
    }
}
