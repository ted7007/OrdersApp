using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelotConfig.json");
builder.Services.AddOcelot();

var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
var authenticationProviderKey = "IdentityApiKey";

builder.Services.AddAuthentication()
    .AddJwtBearer(authenticationProviderKey, x =>
    {
        x.Authority = identityUrl;
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudiences = new[] { "orders", "dishes" }
        };
    });


var app = builder.Build();


await app.UseOcelot();

app.Run();