namespace GameStoreApi.Dtos.Reviews;

public class CreateReviewDto
{
	public decimal Rating { get; set; }
	public string Comment { get; set; } = string.Empty;
	public Guid GameId { get; set; }
}
