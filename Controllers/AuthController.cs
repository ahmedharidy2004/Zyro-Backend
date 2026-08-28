using GameStoreApi.Data;
using GameStoreApi.Dtos.Auth;
using GameStoreApi.Dtos.Users;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using GameStoreApi.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GameStoreDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthController(
        GameStoreDbContext context,
        IConfiguration configuration,
        IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
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

        var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = newUser.Id
            };

        _context.Add(newUser);
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();

        var tokenString = GenerateJwtToken(newUser);

        return Ok(new
        {
            token = tokenString,
            user = new
            {
                newUser.Id,
                newUser.Username,
                newUser.Email,
                newUser.Role
            }
        });
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
        
        var tokenString = GenerateJwtToken(user);

        return Ok(new
                {
                    token = tokenString,
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        user.Role
                    }
                });
    }

    private string GenerateJwtToken(User user)
    {
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

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return Unauthorized(new {message = "You are not authorized :( "});
        }

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userGuid);
        if(user is null) return NotFound(new { message = "Sth wrong happened!"});
      
        // check if the password match the old one
        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            return Unauthorized(new { message = "Current Password is incorrect" });
        }

        // check if passwords match.
        Console.WriteLine(dto.NewPassword);
        Console.WriteLine(dto.ConfirmPassword);
        if(dto.NewPassword != dto.ConfirmPassword)
        {
            return BadRequest("Passwords do not match!");
        }

        var passwordHash =  BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.PasswordHash = passwordHash;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Password Changed successfully"});
    }

    [HttpPost("forget-password")]
    public async Task<ActionResult> ForgetPassword(ForgetPasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
        if(user is null) 
            return Ok("If an account with this email exists, a password reset link has been sent.");

        byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);

        string resetToken = WebEncoders.Base64UrlEncode(tokenBytes);

        var hashedToken = BCrypt.Net.BCrypt.HashPassword(resetToken);

        user.ResetToken = hashedToken;
        user.ResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);
        await _context.SaveChangesAsync();

        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
        string body = "Please use this specific link to reset your password: \n"
        + $"link: {frontendUrl.TrimEnd('/')}/reset-password/{user.Id}/{resetToken}";

        await _emailService.SendEmailAsync(dto.Email, "Reset Password Confirmation", body);

        return Ok("If an account with this email exists, a password reset link has been sent.");
    }

    [HttpPost("reset-password/{userId}/{token}")]
    public async Task<ActionResult> ResetPassword(
        Guid userId,
        string token,
        ResetPasswordDto dto)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user is null || string.IsNullOrEmpty(user.ResetToken))
        {
            return Unauthorized("Invalid or expired token. Please try again.");
        }

        if (user.ResetTokenExpiresAt <= DateTime.UtcNow)
        {
            return Unauthorized("Invalid or expired token. Please try again.");
        }

        if (!BCrypt.Net.BCrypt.Verify(token, user.ResetToken))
        {
            return Unauthorized("Invalid or expired token. Please try again.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        user.ResetToken = string.Empty;
        user.ResetTokenExpiresAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok("Password changed successfully. Please try to log in with your new password.");
    }
}