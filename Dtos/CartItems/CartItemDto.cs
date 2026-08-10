namespace GameStoreApi.Dtos.CartItems;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}