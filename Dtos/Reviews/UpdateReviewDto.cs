namespace GameStoreApi.Dtos.Reviews;

public class UpdateReviewDto
{
	public decimal Rating { get; set; }
	public string Comment { get; set; } = string.Empty;
}
