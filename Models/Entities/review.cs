namespace GameStoreApi.Models.Entities;

public class Review
{
    public Guid Id { get; set; }
    public decimal Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}