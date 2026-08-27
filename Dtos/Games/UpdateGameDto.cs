namespace GameStoreApi.Dtos.Games;

public class UpdateGameDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string TrailerURL { get; set; } = string.Empty;
    public bool HasDiscount { get; set; }
    public decimal DiscountRate { get; set; }
    public Guid GenreId { get; set; }
    public DateOnly ReleaseDate { get; set; }
}