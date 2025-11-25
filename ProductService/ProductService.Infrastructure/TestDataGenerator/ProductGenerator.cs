using ProductService.Infrastructure.Entities;

namespace ProductService.Infrastructure.TestDataGenerator
{
    internal static class ProductGenerator
    {
        public static Product[] GetTestData()
        {
            return
            [
                new Product { Id = 1, Name = "Laptop", Price = 3999.99m, Category = ProductCategory.Electronics, Quantity = 10 },
                new Product { Id = 2, Name = "CSharp Book", Price = 59.99m, Category = ProductCategory.Books, Quantity = 50 },
                new Product { Id = 3, Name = "Wireless Mouse", Price = 129.99m, Category = ProductCategory.Electronics, Quantity = 25 },
                new Product { Id = 4, Name = "Regular Keyboard", Price = 449.99m, Category = ProductCategory.Electronics, Quantity = 15 },
                new Product { Id = 5, Name = "T-Shirt", Price = 89.99m, Category = ProductCategory.Clothing, Quantity = 100 },
                new Product { Id = 6, Name = "Monitor 27inch", Price = 1299.99m, Category = ProductCategory.Electronics, Quantity = 8 },
                new Product { Id = 7, Name = "Headphones", Price = 299.99m, Category = ProductCategory.Electronics, Quantity = 20 },
                new Product { Id = 8, Name = "Webcam HD", Price = 199.99m, Category = ProductCategory.Electronics, Quantity = 12 },
                new Product { Id = 9, Name = "Cracow University of Technology Campus Guide", Price = 199.99m, Category = ProductCategory.Books, Quantity = 30 },
                new Product { Id = 10, Name = "SQL Book", Price = 49.99m, Category = ProductCategory.Books, Quantity = 40 }
            ];
        }
    }
}
