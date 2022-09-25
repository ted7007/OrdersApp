using System.Data.Common;
using System.Reflection;
using InternalService.Config;
using InternalService.Models;
using InternalService.Repository;
using InternalService.Repository.Argument;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
                       //todo comments

#region mysql connection string  build
var MySQLOptions =
    builder.Configuration.GetSection(InternalService.Config.MySQLOptions.OptionName)
                         .Get<MySQLOptions>();
DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
connectionStringBuilder.Add("Server", MySQLOptions.Server);
connectionStringBuilder.Add("Port", MySQLOptions.Port);
connectionStringBuilder.Add("User", MySQLOptions.UserName);
connectionStringBuilder.Add("Database", MySQLOptions.DatabaseName);
connectionStringBuilder.Add("Password", MySQLOptions.Password);


#endregion

var connectionString =connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped
    <IOrderRepository, OrderRepository>();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => $"{app.Environment.EnvironmentName}");

app.Run();
                                            
/*  todo
 
 * global logging
 * global exception handler
 * authorization & authentification
 * automapper
 * Data Access Layer(?) Unity repo?
 
 
*/

