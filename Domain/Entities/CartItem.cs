namespace GameStoreApi.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }

    public Guid GameId { get; set; }

    public int Quantity { get; set; }

    public Cart Cart { get; set; } = null!;

    public Game Game { get; set; } = null!;
}