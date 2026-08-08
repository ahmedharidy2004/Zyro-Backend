namespace GameStoreApi.Dtos.CartItems;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid GameId { get; set; }
    public int Quantity { get; set; }
}