using GameStoreApi.Dtos.OrderItems;
using GameStoreApi.Dtos.Users;

namespace GameStoreApi.Dtos.Orders;

public class CreateOrderDto
{
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}