using GameStoreApi.Dtos.OrderItems;
using GameStoreApi.Models.Entities;

namespace GameStoreApi.Dtos.Orders;

public class CreateOrderDto
{
    public Guid UserId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}