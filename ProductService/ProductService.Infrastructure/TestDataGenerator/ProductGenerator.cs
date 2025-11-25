using ProductService.Infrastructure.Entities;

namespace ProductService.Infrastructure.TestDataGenerator
{
    internal static class ProductGenerator
    {
        public static Product[] GetTestData()
        {
            return
            [
                new Product { Id = 1, Name = "Laptop", Price = 3999.99m },
                new Product { Id = 2, Name = "Programming Book From Cracow University of Technology", Price = 59.99m },
                new Product { Id = 3, Name = "Wireless Mouse", Price = 129.99m },
                new Product { Id = 4, Name = "Regular Keyboard", Price = 449.99m },
                new Product { Id = 5, Name = "USB-C Cable", Price = 29.99m },
                new Product { Id = 6, Name = "Monitor 27inch", Price = 1299.99m },
                new Product { Id = 7, Name = "Headphones", Price = 299.99m },
                new Product { Id = 8, Name = "Webcam HD", Price = 199.99m },
                new Product { Id = 9, Name = "Monitor 24inch", Price = 599.99m },
                new Product { Id = 10, Name = "Gaming Laptop", Price = 3999.99m }
            ];
        }
    }
}
