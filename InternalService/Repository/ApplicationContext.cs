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
}