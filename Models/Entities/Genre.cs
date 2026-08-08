namespace GameStoreApi.Models.Entities;

public class Genre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Game> Games { get; set; } = new List<Game>();
}