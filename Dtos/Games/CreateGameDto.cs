namespace GameStoreApi.Dtos.Games;

public class CreateGameDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid GenreId { get; set; }
    public DateOnly ReleaseDate { get; set; }
}