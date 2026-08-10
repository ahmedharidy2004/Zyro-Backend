using GameStoreApi.Data;
using GameStoreApi.Dtos.Users;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public UserController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users
                            .Select(user => new UserDto
                            {
                               Id = user.Id,
                               Username = user.Username,
                               Email = user.Email
                            }).ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await _context.Users
                            .Where(user => user.Id == id)
                            .Select(user => new UserDto
                            {
                               Id = user.Id,
                               Username = user.Username,
                               Email = user.Email
                            }).FirstOrDefaultAsync();

        if(user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        var CreatedUser = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email
        };

        var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = CreatedUser.Id
            };
        
        _context.Users.Add(CreatedUser);
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
            {
                Id = CreatedUser.Id,
                Username = CreatedUser.Username,
                Email = CreatedUser.Email
            };

        return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(Guid id, UpdateUserDto dto)
    {
        var UpdatedUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if(UpdatedUser is null)
        {
            return NotFound();
        } 


        UpdatedUser.Username = dto.Username;
        UpdatedUser.Email = dto.Email;
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if(user is null)
        {
            return NotFound();
        } 

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}