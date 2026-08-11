using GameStoreApi.Data;
using GameStoreApi.Dtos.Auth;
using GameStoreApi.Dtos.Users;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    private readonly IConfiguration _configuration;

    public AuthController(
        GameStoreDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        if(dto.Password != dto.ConfirmPassword)
        {
            return BadRequest(new { message = "Passwords don't match" });
        }

        var user = await _context.Users
                        .FirstOrDefaultAsync(user => user.Email == dto.Email);

        if (user is not null)
            {
                return BadRequest(new { message = "Email already exists!" });
            }

        var passwordHash =  BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Add(newUser);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = newUser.Id,
            Username = newUser.Username,
            Email = newUser.Email,
            Role = newUser.Role
        };

        return Ok(userDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
                    .FirstOrDefaultAsync(user => user.Email == dto.Email);

        if(user is null) return Unauthorized(new { message = "Invalid email or password" });

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid Email or password" });
        }
        
         var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email
        };

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1440),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler()
                            .WriteToken(token);

        return Ok(new { token = tokenString });
    }
}