using RestaurantMgmtApp.Data;
using RestaurantMgmtApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace RestaurantMgmtApp
{
    class Program
    {
        private const string ConnectionString = "Server=DESKTOP-G44M1FL\\SQLEXPRESS;Database=RestaurantMgmt;Trusted_Connection=True;TrustServerCertificate=True;";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool isRunning = true;
            while (isRunning)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                isRunning = await HandleMenuChoiceAsync(choice);
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n--- Меню Керування Рестораном ---");
            Console.WriteLine("1. Переглянути клієнтів");
            Console.WriteLine("2. Додати нового клієнта");
            Console.WriteLine("3. Переглянути активні замовлення");
            Console.WriteLine("4. Створити нове замовлення");
            Console.WriteLine("5. Пошук страви в меню");
            Console.WriteLine("6. Скасувати замовлення"); 
            Console.WriteLine("7. Вийти");
            Console.Write("Виберіть дію: ");
        }

        private static async Task<bool> HandleMenuChoiceAsync(string choice)
        {
            try
            {
                switch (choice)
                {
                    case "1":
                        await ViewCustomersAsync();
                        break;
                    case "2":
                        await AddNewCustomerAsync();
                        break;
                    case "3":
                        await ViewActiveOrdersAsync();
                        break;
                    case "4":
                        await CreateNewOrderAsync();
                        break;
                    case "5":
                        await SearchMenuItemsAsync();
                        break;
                    case "6":
                        await CancelOrderAsync(); 
                        break;
                    case "7":
                        Console.WriteLine("Вихід...");
                        return false;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n!!! Сталася помилка: {ex.Message} !!!");
                Console.ResetColor();
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
            return true;
        }

       
        #region Методи 1-5 (без змін)
        private static async Task ViewCustomersAsync()
        {
            Console.WriteLine("--- Список Активних Клієнтів ---");

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            dbContext.Open();

            var customers = await uow.Customers.GetAllActiveAsync();

            if (!customers.Any())
            {
                Console.WriteLine("Клієнтів не знайдено.");
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.CustomerId}, ПІБ: {customer.FirstName} {customer.LastName}, Телефон: {customer.Phone}");
            }
        }

        private static async Task AddNewCustomerAsync()
        {
            Console.WriteLine("--- Додавання Нового Клієнта ---");
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine();
            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email
            };

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            uow.BeginTransaction();
            try
            {
                long newId = await uow.Customers.CreateAsync(customer, createdByUserId: 1);
                await uow.CommitAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Успішно створено клієнта з ID: {newId}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка створення клієнта: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static async Task ViewActiveOrdersAsync()
        {
            Console.WriteLine("--- Список Активних Замовлень ('New', 'Preparing') ---");

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            dbContext.Open();

            var orders = await uow.Orders.GetActiveAsync();

            if (!orders.Any())
            {
                Console.WriteLine("Активних замовлень не знайдено.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.OrderId}, Статус: {order.Status}, Сума: {order.TotalAmount:C}, Філія: {order.BranchName}");
            }
        }

        private static async Task CreateNewOrderAsync()
        {
            Console.WriteLine("--- Створення Нового Замовлення ---");

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            try
            {
                Console.Write("Введіть ID Філії (BranchId): ");
                long branchId = long.Parse(Console.ReadLine());

                Console.Write("Введіть ID Клієнта (CustomerId): ");
                long customerId = long.Parse(Console.ReadLine());

                var order = new Order
                {
                    BranchId = branchId,
                    CustomerId = customerId,
                    Status = "New"
                };

                uow.BeginTransaction();

                long orderId = await uow.Orders.CreateAsync(order, createdByUserId: 1);
                Console.WriteLine($"Створено заголовок замовлення ID: {orderId}");

                Console.WriteLine("Додаємо позицію до замовлення...");
                Console.Write("Введіть ID Страви (MenuItemId): ");
                long menuItemId = long.Parse(Console.ReadLine());
                Console.Write("Введіть Ціну (UnitPrice): ");
                decimal unitPrice = decimal.Parse(Console.ReadLine());
                Console.Write("Введіть Кількість (Quantity): ");
                int quantity = int.Parse(Console.ReadLine());

                var item = new OrderItem
                {
                    OrderId = orderId,
                    MenuItemId = menuItemId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                await uow.OrderItems.AddAsync(item, createdByUserId: 1);
                Console.WriteLine("Позицію успішно додано.");

                await uow.CommitAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Успішно створено замовлення ID: {orderId}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка створення замовлення: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static async Task SearchMenuItemsAsync()
        {
            Console.WriteLine("--- Пошук Страви ---");
            Console.Write("Введіть назву (або Enter для показу всіх): ");
            string name = Console.ReadLine();

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            dbContext.Open();

            var items = await uow.MenuItems.SearchAsync(string.IsNullOrEmpty(name) ? null : name);

            if (!items.Any())
            {
                Console.WriteLine("Страв не знайдено.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.MenuItemId}, Назва: {item.Name}, Ціна: {item.Price:C}");
            }
        }
        #endregion

        
        private static async Task CancelOrderAsync()
        {
            Console.WriteLine("--- Скасування Замовлення ---");

            using var dbContext = new SqlDbContext(ConnectionString);
            using var uow = new UnitOfWork(dbContext);

            try
            {
                
                Console.Write("Введіть ID Замовлення (OrderId) для скасування: ");
                long orderId = long.Parse(Console.ReadLine());

                
                uow.BeginTransaction();

                
                await uow.Orders.SoftDeleteAsync(orderId);

                await uow.CommitAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Замовлення ID: {orderId} було успішно скасовано.");
               
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка скасування замовлення: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
