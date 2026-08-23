namespace GameStoreApi.Dtos.Reviews;

public class ReviewDto
{
	public Guid Id { get; set; }
	public decimal Rating { get; set; }
	public string Comment { get; set; } = string.Empty;
	public Guid UserId { get; set; }
	public string Username { get; set; } = string.Empty;
	public Guid GameId { get; set; }
	public string GameName { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
}
