using InternalService.Models;
using Microsoft.EntityFrameworkCore;
namespace InternalService.Repository;

public class ApplicationContext : DbContext
{
    public DbSet<Models.Order> Orders { get; set; }
    
    public DbSet<Models.Dish> Dishes { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Models.Order>()
            .Property(o => o.Status)
            .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));
        modelBuilder
            .Entity<Models.Order>()
            .Property(o => o.Type)
            .HasConversion(
                v => v.ToString(),
                v => (OrderType)Enum.Parse(typeof(OrderType), v));

        modelBuilder
            .Entity<Models.Order>()
            .HasMany(o => o.Dishes);

        modelBuilder
            .Entity<Models.Dish>()
            .HasData(
                new Models.Dish[]
                {
                    new Models.Dish()
                    {
                        Id = Guid.NewGuid(),
                        Calory = 200f,
                        Price = 400,
                        Title = "Pizza"
                    }
                });


        base.OnModelCreating(modelBuilder);
        
        
    }
}