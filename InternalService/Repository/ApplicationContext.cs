using InternalService.Models;
using Microsoft.EntityFrameworkCore;
namespace InternalService.Service;

public class ApplicationContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<Dish> Dishes { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Order>()
            .Property(o => o.Status)
            .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));
        modelBuilder
            .Entity<Order>()
            .Property(o => o.Type)
            .HasConversion(
                v => v.ToString(),
                v => (OrderType)Enum.Parse(typeof(OrderType), v));

        modelBuilder
            .Entity<Dish>()
            .HasData(
                new Dish[]
                {
                    new Dish()
                    {
                        Id = Guid.NewGuid(),
                        Calory = 200f,
                        Price = 400,
                        Title = "Pizza",
                        Orders = new List<Order>()
                    }
                });
        modelBuilder
            .Entity<Dish>()
            .HasMany(dish => dish.Orders)
            .WithMany(order => order.Dishes)
            .UsingEntity(j => j.ToTable("DishOrder"));
        
        
        base.OnModelCreating(modelBuilder);
        
        
    }
}