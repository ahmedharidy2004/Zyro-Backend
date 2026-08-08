namespace GameStoreApi.Dtos.OrderItems;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid GameId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}