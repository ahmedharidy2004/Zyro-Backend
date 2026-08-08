namespace GameStoreApi.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid GameId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public Order Order { get; set; } = null!;

    public Game Game { get; set; } = null!;
}