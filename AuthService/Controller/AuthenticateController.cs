using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Model;
using AuthService.ViewModel;
using InternalService.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
        {
            return Unauthorized();
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole  in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = GetToken(authClaims);

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    [Route("users")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userManager.Users.ToArrayAsync());
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
            return BadRequest();
        var userExists = await _userManager.FindByNameAsync(model.UserName);
        if (userExists != null)
            return Conflict();
        User user = new()
        {
            UserName = model.UserName,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return Problem();
        return Ok();
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult PingAuth()
    {
        return Ok("U're authorized bro");
    }

    private JwtSecurityToken GetToken(List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["BearerOptions:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["BearerOptions:Issuer"],
            audience: _configuration["BearerOptions:Audience"],
            expires: DateTime.Now.Add(
                TimeSpan.FromMinutes(
                    Int32.Parse(_configuration["BearerOptions:LifeTime"]))),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    
}