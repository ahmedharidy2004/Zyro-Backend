using GameStoreApi.Dtos.OrderItems;
using GameStoreApi.Dtos.Users;

namespace GameStoreApi.Dtos.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}