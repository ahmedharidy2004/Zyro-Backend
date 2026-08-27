namespace GameStoreApi.Dtos.OrderItems;

public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid GameId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}