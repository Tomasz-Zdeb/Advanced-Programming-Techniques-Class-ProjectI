using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Entities;
using ProductService.Infrastructure.TestDataGenerator;

namespace ProductService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        //Proces seedowania danymi testowymi wzorowałem na dokumentacji Microsoft:
        //https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#model-seed-data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(ProductGenerator.GetTestData());
        }
    }
}
