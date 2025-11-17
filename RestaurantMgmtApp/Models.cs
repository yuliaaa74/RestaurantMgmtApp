using System;

namespace RestaurantMgmtApp.Models
{
    public class Restaurant
    {
        public long RestaurantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedBy { get; set; }
    }

    public class Branch
    {
        public long BranchId { get; set; }
        public long RestaurantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Menu
    {
        public long MenuId { get; set; }
        public long BranchId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class MenuItem
    {
        public long MenuItemId { get; set; }
        public long MenuId { get; set; }
        public long? CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Customer
    {
        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; } 
        public string Email { get; set; }
    }

    public class Order
    {
        public long OrderId { get; set; }
        public long BranchId { get; set; }
        public long? CustomerId { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string BranchName { get; set; }
    }

    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        public long MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
