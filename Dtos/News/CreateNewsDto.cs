namespace GameStoreApi.Dtos.News;

public class CreateNewsDto
{
    public string Title { get; set; } = string.Empty;
    public string CoverURL { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}