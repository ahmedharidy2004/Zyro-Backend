using GameStoreApi.Data;
using GameStoreApi.Dtos.Carts;
using GameStoreApi.Dtos.CartItems;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public CartController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<CartDto>> GetCart(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if(user is null)
        {
            return NotFound(new { message = "User Not Found!"});
        }

        var cart = await _context.Carts
                                    .Where(cart => cart.UserId == userId)
                                    .Select(cart => new CartDto
                                    {
                                        Id = cart.Id,
                                        UserId = cart.UserId,

                                        Items = cart.Items.Select(item => new CartItemDto
                                        {
                                            Id = item.Id,
                                            GameId = item.GameId,
                                            GameName = item.Game.Name,
                                            Price = item.Game.Price,
                                            Quantity = item.Quantity
                                        }).ToList()
                                    })
                                    .FirstOrDefaultAsync();

        if(cart is null)
        {
             return NotFound(new { message = "Cart Not Found!"});
        }

        return Ok(cart);
    }

    [HttpPost("{userId}/items")]
    public async Task<ActionResult> AddCartItem(
        Guid userId,
        AddCartItemDto dto)
    {
        // check quantity
        if (dto.Quantity <= 0)
        {
            return BadRequest(new { message = "Wrong Quantity!" });
        }

        // check game exists
        var game = await _context.Games
            .FirstOrDefaultAsync(game => game.Id == dto.GameId);

        if (game is null)
        {
            return NotFound(new { message = "Game Not Found" });
        }

        // check cart exists
        var cart = await _context.Carts
            .FirstOrDefaultAsync(cart => cart.UserId == userId);

        if (cart is null)
        {
            return NotFound(new { message = "Cart Not Found" });
        }

        // check if this game already exists in the cart
        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(cartItem =>
                cartItem.CartId == cart.Id &&
                cartItem.GameId == dto.GameId);

        if (existingItem is not null)
        {
            return BadRequest(new { message = "Item Already Found" });
        }

        // create new cart item
        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cart.Id,
            GameId = dto.GameId,
            Quantity = dto.Quantity
        };

        _context.CartItems.Add(cartItem);

        await _context.SaveChangesAsync();

        return Ok(cartItem);
    }

    [HttpPut("{userId}/items/{itemId}")]
    public async Task<ActionResult> UpdateCartItem(
        Guid userId,
        Guid itemId,
        UpdateCartItemDto dto)
    {
        if(dto.Quantity <= 0)
        {
            return BadRequest(new { message = "Invalid Quantity"});
        }

        var cart = await _context.Carts.FirstOrDefaultAsync(
            cart => cart.UserId == userId
        );

        if(cart is null)
        {
            return NotFound(new { message = "Cart Not Found" });
        }

        var item = await _context.CartItems.FirstOrDefaultAsync(
            cartItem => cartItem.CartId == cart.Id
            && cartItem.Id == itemId
        );

        if(item is null)
        {
            return NotFound(new { message = "Cart Item Not Found" });
        }

        item.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{userId}/items/{itemId}")]
    public async Task<ActionResult> DeleteCartItem(
        Guid userId,
        Guid itemId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(
            cart => cart.UserId == userId
        );

        if(cart is null)
        {
            return NotFound(new { message = "Cart Not Found" });
        }

        var item = await _context.CartItems.FirstOrDefaultAsync(
            cartItem => cartItem.CartId == cart.Id
            && cartItem.Id == itemId
        );

        if(item is null)
        {
            return NotFound(new { message = "Cart Item Not Found" });
        }

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}