using GameStoreApi.Models.Entities;

namespace GameStoreApi.Dtos.Games;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageURL { get; set; } = string.Empty;
    public Guid GenreId { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Genre { get; set; } = string.Empty;
}