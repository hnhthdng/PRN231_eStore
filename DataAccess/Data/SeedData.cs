using BusinessObject.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, StoreDbContext context)
        {
            context.Database.Migrate(); // Đảm bảo rằng cơ sở dữ liệu đã được tạo ra

            // Kiểm tra xem dữ liệu đã được tạo chưa
            if (context.Members.Any() || context.Categories.Any() || context.Products.Any() || context.Orders.Any() || context.OrderDetails.Any())
            {
                return; // Nếu có dữ liệu rồi thì không làm gì
            }

            // Thêm dữ liệu mẫu cho các bảng
            AddCategories(context);
            AddProducts(context);
            AddMembers(context);
            AddOrders(context);  // Đảm bảo thêm Orders trước
            AddOrderDetails(context); // Thêm OrderDetails sau Orders

            //context.SaveChanges(); // Lưu các thay đổi
        }

        private static void AddCategories(StoreDbContext context)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Categories ON");

            var categories = new[]
            {
                new Category {CategoryId = 1, CategoryName = "Electronics" },
                new Category {CategoryId = 2, CategoryName = "Clothing" },
                new Category {CategoryId = 3, CategoryName = "Food" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges(); // Lưu các thay đổi
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Categories OFF");

        }

        private static void AddProducts(StoreDbContext context)
        {
            var products = new[]
            {
                new Product { ProductName = "Laptop", UnitPrice = 1000, Weight = 2, UnitsInStock = 50, CategoryId = 1 },
                new Product { ProductName = "Smartphone", UnitPrice = 500, Weight = 5, UnitsInStock = 200, CategoryId = 1 },
                new Product { ProductName = "T-Shirt", UnitPrice = 20, Weight = 2, UnitsInStock = 100, CategoryId = 2 },
                new Product { ProductName = "Jeans", UnitPrice = 40, Weight = 5, UnitsInStock = 150, CategoryId = 2 },
                new Product { ProductName = "Apple", UnitPrice = 2, Weight = 1, UnitsInStock = 300, CategoryId = 3 }
            };

            context.Products.AddRange(products);
            context.SaveChanges(); // Lưu các thay đổi

        }

        private static void AddMembers(StoreDbContext context)
        {
            var members = new[]
            {
                new Member { Email = "john.doe@example.com", CompanyName = "TechCorp", City = "New York", Country = "USA", Password = "password123" },
                new Member { Email = "jane.smith@example.com", CompanyName = "FashionInc", City = "London", Country = "UK", Password = "password123" }
            };

            context.Members.AddRange(members);
            context.SaveChanges(); // Lưu các thay đổi

        }

        private static void AddOrders(StoreDbContext context)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Orders ON");
            var orders = new[]
            {
                new Order { OrderId = 1, MemberId = 1, OrderDate = DateTime.Now, RequiredDate = DateTime.Now.AddDays(5), ShippedDate = DateTime.Now.AddDays(2), Freight = "10" },
                new Order {OrderId = 2, MemberId = 2, OrderDate = DateTime.Now, RequiredDate = DateTime.Now.AddDays(7), ShippedDate = DateTime.Now.AddDays(3), Freight = "15" }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges(); // Lưu các thay đổi
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Orders OFF");

        }

        private static void AddOrderDetails(StoreDbContext context)
        {
            var orderDetails = new[]
            {
                // Cập nhật OrderId để phù hợp với các OrderId đã có trong bảng Orders
                new OrderDetail { OrderId = 1, ProductId = 1, UnitPrice = 1000, Quantity = 1, Discount = 0 },
                new OrderDetail { OrderId = 1, ProductId = 2, UnitPrice = 500, Quantity = 2, Discount = 0 },
                new OrderDetail { OrderId = 2, ProductId = 3, UnitPrice = 20, Quantity = 3, Discount = 10 }
            };

            context.OrderDetails.AddRange(orderDetails);
            context.SaveChanges(); // Lưu các thay đổi

        }
    }
}
