using Microsoft.EntityFrameworkCore;
using Server.Entity;

namespace Server.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(
            new List<Product> {
                new Product {ProductId=1,
                Name="Galaxy S23 256 GB",
                Description="S23 description",
                ImageUrl="s23.jpg",
                IsActive=true,
                Price=35000,
                Stock=5000},

                new Product {ProductId=2,
                Name="IPhone 15 Pro 512 GB",
                Description="15 Pro description",
                ImageUrl="15pro.jpg",
                IsActive=true,
                Price=75000,
                Stock=2000},

                new Product {ProductId=3,
                Name="Star Wars Darth Vader Lego Set",
                Description="Lego set description",
                ImageUrl="lego.jpg",
                IsActive=true,
                Price=3000,
                Stock=4200},

                new Product {ProductId=4,
                Name="Logitech G Pro X Superlight",
                Description="G Pro X Superlight description",
                ImageUrl="gprox.jpg",
                IsActive=false,
                Price=5500,
                Stock=30},

                new Product {ProductId=5,
                Name="Galaxy Buds FE",
                Description="Buds FE description",
                ImageUrl="budsfe.jpg",
                IsActive=true,
                Price=2000,
                Stock=0},
            }
        );
    }

}
