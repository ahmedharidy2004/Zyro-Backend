namespace GameStoreApi.Dtos.CartItems;

public class AddCartItemDto
{
    public Guid GameId { get; set; }
    public int Quantity { get; set; }
}