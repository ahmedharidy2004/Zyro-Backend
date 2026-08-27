namespace GameStoreApi.Models.Entities;

public class Game
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public string imageURL { get; set; } = string.Empty;

    public string TrailerURL { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool HasDiscount { get; set; }

    public decimal DiscountRate { get; set; }

    public Guid GenreId { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public Genre Genre { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}