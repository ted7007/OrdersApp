using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using InternalService.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InternalServiceIntegrationTests;

public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class 
    where TDbContext : DbContext
{
    private readonly PostgreSqlTestcontainer _container;

    public IntegrationTestFactory()
    {
        _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "test_db",
                Username = "root",
                Password = "password"
            })
            .WithImage("postgres:13.3")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove AppDbContext
            var descriptor = services.SingleOrDefault
                (d => d.ServiceType == typeof(DbContextOptions<TDbContext>));
            if (descriptor != null) 
                services.Remove(descriptor);
            
            // Add DB context pointing to test container
            services.AddDbContext<TDbContext>
                (options => { options.UseNpgsql(_container.ConnectionString); });
            
            // Ensure schema gets created
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<TDbContext>();
            context.Database.EnsureCreated();
        })
            .UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}