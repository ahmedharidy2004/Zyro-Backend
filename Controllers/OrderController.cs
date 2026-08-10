using GameStoreApi.Data;
using GameStoreApi.Dtos.Orders;
using GameStoreApi.Dtos.OrderItems;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public OrderController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _context.Orders
            .Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,

                Items = order.Items
                    .Select(orderItem => new OrderItemDto
                    {
                        Id = orderItem.Id,
                        GameId = orderItem.GameId,
                        Quantity = orderItem.Quantity,
                        UnitPrice = orderItem.UnitPrice
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
    {
        var order = await _context.Orders
            .Where(user => user.Id == id)
            .Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,

                Items = order.Items
                    .Select(orderItem => new OrderItemDto
                    {
                        Id = orderItem.Id,
                        GameId = orderItem.GameId,
                        Quantity = orderItem.Quantity,
                        UnitPrice = orderItem.UnitPrice
                    })
                    .ToList()
            })
            .ToListAsync();

        if (order is null)
        {
            return NotFound(new { message = "Order Not Found" });
        }

        return Ok(order);
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult<OrderDto>> CreateOrder(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if(user is null) return NotFound(new { message = "User Not Found!"});

        var cart = await _context.Carts
                    .Include(cart => cart.Items)
                    .ThenInclude(item => item.Game)
                    .FirstOrDefaultAsync(cart => cart.UserId == userId);
                    
        if(cart is null) return NotFound(new { message = "Cart Not Found!"});

        if (cart.Items.Count == 0)
        {
            return BadRequest(new { message = "Cart is Empty" });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            TotalPrice = 0
        };

        foreach(var item in cart.Items)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                GameId = item.GameId,
                Quantity = item.Quantity,
                UnitPrice = item.Game.Price
            };

            order.Items.Add(orderItem);
            order.TotalPrice += orderItem.Quantity * orderItem.UnitPrice;
        }

        _context.Orders.Add(order);
        

        foreach(var item in cart.Items)
        {
            _context.CartItems.Remove(item);
        }

        await _context.SaveChangesAsync();

        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        if(user is null) return NotFound(new { message = "User Not Found"});

        var orders = await _context.Orders
            .Where(user => user.Id == userId)
            .Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,

                Items = order.Items
                    .Select(orderItem => new OrderItemDto
                    {
                        Id = orderItem.Id,
                        GameId = orderItem.GameId,
                        Quantity = orderItem.Quantity,
                        UnitPrice = orderItem.UnitPrice
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(order => order.Id == id);

        if (order is null)
        {
            return NotFound(new { message = "Order Not Found" });
        }

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
        return NoContent();
    }
}