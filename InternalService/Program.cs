using System.Data.Common;
using System.Reflection;
using System.Text.Json.Serialization;
using InternalService.Config;
using InternalService.Models;
using InternalService.Repository;
using InternalService.Repository.Dish;
using InternalService.Repository.Order;
using InternalService.Service;
using InternalService.Service.Argument;
using InternalService.Service.DishService;
using InternalService.Service.OrderService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



var connectionString = GetConnectionString(builder.Environment.EnvironmentName, builder);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => $"{app.Environment.EnvironmentName}");

app.Run();

                                            
/*  todo
 
 * global logging
 * global exception handler
 * authorization & authentication
 * ~automapper~
 * ~Data Access Layer(?) Unity repo?~
 
 
*/

string? GetConnectionString(string stage, WebApplicationBuilder hostBuilder)
{
    switch (stage)
    {
        case "Development":
            var dataBaseOptions =
                hostBuilder.Configuration.GetSection(InternalService.Config.DataBaseOptions.OptionName)
                    .Get<DataBaseOptions>();
            if (dataBaseOptions is null)
                throw new ArgumentNullException(nameof(dataBaseOptions));
            DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
            connectionStringBuilder.Add("Host", dataBaseOptions.Server);
            connectionStringBuilder.Add("Port", dataBaseOptions.Port);
            connectionStringBuilder.Add("Username", dataBaseOptions.UserName);
            connectionStringBuilder.Add("Database", dataBaseOptions.DatabaseName);
            connectionStringBuilder.Add("Password", dataBaseOptions.Password);
            return connectionStringBuilder.ConnectionString;
            break;
        default:
            return null;

    }
}

public partial class Program {  }
