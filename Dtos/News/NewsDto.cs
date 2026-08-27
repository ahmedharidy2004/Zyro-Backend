namespace GameStoreApi.Dtos.News;

public class NewsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverURL { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}