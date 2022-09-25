using InternalService.Models;
using Microsoft.EntityFrameworkCore;
namespace InternalService.Repository;

public class ApplicationContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

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
        base.OnModelCreating(modelBuilder);
    }
}