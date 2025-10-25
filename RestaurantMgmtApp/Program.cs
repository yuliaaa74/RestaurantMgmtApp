using RestaurantMgmtApp.Data;
using RestaurantMgmtApp.Models;
using System;
using System.Threading.Tasks;

namespace RestaurantMgmtApp
{
    class Program
    {
        private const string ConnectionString = "Server=DESKTOP-G44M1FL\\SQLEXPRESS;Database=RestaurantMgmt;Trusted_Connection=True;TrustServerCertificate=True;";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting full demo...");

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            try
            {
                uow.BeginTransaction();

                // --- 1) Restaurant ---
                var restaurant = new Restaurant
                {
                    Name = "Demo Restaurant",
                    Address = "Main St 1",
                    Phone = "+380001234567"
                };
                long restaurantId = await uow.Restaurants.CreateAsync(restaurant, 1);
                Console.WriteLine($"Created Restaurant Id={restaurantId}");

                // --- 2) Branch ---
                var branch = new Branch
                {
                    RestaurantId = restaurantId,
                    Name = "Demo Branch",
                    Address = "Branch Address"
                };
                long branchId = await uow.Branches.CreateAsync(branch, 1);
                Console.WriteLine($"Created Branch Id={branchId}");

                // --- 3) Menu ---
                var menu = new Menu
                {
                    BranchId = branchId,
                    Name = "Main Menu",
                    IsActive = true
                };
                long menuId = await uow.Menus.CreateAsync(menu);
                Console.WriteLine($"Created Menu Id={menuId}");

                // --- 4) Customer ---
                var customer = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe"
                };
                long customerId = await uow.Customers.CreateAsync(customer);
                Console.WriteLine($"Created Customer Id={customerId}");

                // --- 5) Order ---
                var order = new Order
                {
                    BranchId = branchId,
                    CustomerId = customerId,
                    Status = "New"
                };
                long orderId = await uow.Orders.CreateAsync(order);
                Console.WriteLine($"Created Order Id={orderId}");

                await uow.CommitAsync();
                Console.WriteLine("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                uow.Rollback();
                Console.WriteLine("Transaction rolled back.");
            }

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
