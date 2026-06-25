using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapiCourse.models;

namespace webapiCourse.Data
{
    public class DataDBContext : IdentityDbContext<ApplicationUser>
    {
        public DataDBContext(DbContextOptions<DataDBContext> options)
           : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 10,
                    Name = "Electronics",
                    Description = "Electronic devices and gadgets" 
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop Dell",
                    Description = "High performance laptop",
                    quantity = 10,
                    price = 15000m,
                    CategoryId = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "Smartphone",
                    Description = "Latest model smartphone",
                    quantity = 25,
                    price = 8000m,
                    CategoryId = 10
                }
            );
        }
    }
}