using System.Data.Common;
using System.Text;
using AuthService;
using AuthService.Model;
using InternalService.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = GetConnectionString(builder.Environment.EnvironmentName, builder);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["BearerOptions:Audience"],
            ValidIssuer = builder.Configuration["BearerOptions:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["BearerOptions:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();
app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();



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
