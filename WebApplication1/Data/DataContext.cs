using Microsoft.EntityFrameworkCore;
using WebApplication1.Entity;

namespace WebApplication1.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new List<Product> {
                new() {
                Id = 1, Name = "Galaxy S23", Description = "S23 Açıklaması",
                ImageUrl = "s23.jpg",IsActive = true,Price = 35000, Stock = 200},
                new() { Id = 2,
                Name = "Galaxy S24 FE",Description = "S24 FE Açıklaması",ImageUrl = "s24fe.jpg",IsActive = true,Price = 32000, Stock = 300},
                new() { Id = 3, Name = "Xiamoi 14T",   Description = "14T Açıklaması", ImageUrl = "14t.jpg", IsActive = true,
                Price = 28000, Stock = 100},
                new() { Id = 4, Name = "IPhone 14 Pro", Description = "14 Pro Açıklaması", ImageUrl = "14pro.jpg", IsActive = true,
                Price = 80000, Stock = 250}
            }
        );
    }
}