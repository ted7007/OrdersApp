using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));;

var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
var authenticationProviderKey = "IdentityApiKey";

builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options = >);

// builder.Services.AddAuthentication()
//     .AddJwtBearer(authenticationProviderKey, x =>
//     {
//         x.Authority = identityUrl;
//         x.RequireHttpsMetadata = false;
//         x.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidAudiences = new[] { "orders", "dishes" }
//         };
//     });
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("InternalPolicy", policy =>
//         policy.RequireAuthenticatedUser()
//             .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
// });

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();


app.Run();