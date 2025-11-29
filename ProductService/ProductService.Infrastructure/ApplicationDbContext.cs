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
        public DbSet<ForbiddenWord> ForbiddenWords { get; set; }

        //Proces seedowania danymi testowymi wzorowałem na dokumentacji Microsoft:
        //https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#model-seed-data
        //Mapowanie wartośći decimal z kolei na artykule:
        //https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwith-nrt#precision-and-scale
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().HasIndex(p => p.Name);
            modelBuilder.Entity<Product>().HasData(ProductGenerator.GetTestData());

            modelBuilder.Entity<ForbiddenWord>().HasIndex(fw => fw.Word).IsUnique();
            modelBuilder.Entity<ForbiddenWord>().HasData(ForbiddenWordGenerator.GetTestData());
        }


        
    }
}
