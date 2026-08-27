using GameStoreApi.Data;
using GameStoreApi.Dtos.News;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public NewsController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NewsDto>>> GetNews()
    {
        var news = await _context.News
            .AsNoTracking()
            .Select(item => new NewsDto
            {
                Id = item.Id,
                Title = item.Title,
                CoverURL = item.CoverURL,
                Content = item.Content,
                UserId = item.UserId,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                PublishedAt = item.PublishedAt
            })
            .ToListAsync();

        return Ok(news);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NewsDto>> GetNewsById(Guid id)
    {
        var news = await _context.News
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new NewsDto
            {
                Id = item.Id,
                Title = item.Title,
                CoverURL = item.CoverURL,
                Content = item.Content,
                UserId = item.UserId,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                PublishedAt = item.PublishedAt
            })
            .FirstOrDefaultAsync();

        if (news is null)
        {
            return NotFound(new { message = "News Not Found!" });
        }

        return Ok(news);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<NewsDto>> CreateNews([FromBody] CreateNewsDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.CoverURL) || string.IsNullOrWhiteSpace(dto.Content))
        {
            return BadRequest(new { message = "Title, cover URL, and content are required." });
        }

        if (!await _context.Users.AnyAsync(user => user.Id == dto.UserId))
        {
            return BadRequest(new { message = "User Not Found!" });
        }

        var news = new News
        {
            Id = Guid.NewGuid(),
            Title = dto.Title.Trim(),
            CoverURL = dto.CoverURL.Trim(),
            Content = dto.Content.Trim(),
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PublishedAt = dto.PublishedAt
        };

        _context.News.Add(news);
        await _context.SaveChangesAsync();

        var newsDto = new NewsDto
        {
            Id = news.Id,
            Title = news.Title,
            CoverURL = news.CoverURL,
            Content = news.Content,
            UserId = news.UserId,
            CreatedAt = news.CreatedAt,
            UpdatedAt = news.UpdatedAt,
            PublishedAt = news.PublishedAt
        };

        return CreatedAtAction(nameof(GetNewsById), new { id = news.Id }, newsDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateNews(Guid id, [FromBody] UpdateNewsDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.CoverURL) || string.IsNullOrWhiteSpace(dto.Content))
        {
            return BadRequest(new { message = "Title, cover URL, and content are required." });
        }

        var news = await _context.News.FirstOrDefaultAsync(item => item.Id == id);
        if (news is null)
        {
            return NotFound(new { message = "News Not Found!" });
        }

        if (!await _context.Users.AnyAsync(user => user.Id == dto.UserId))
        {
            return BadRequest(new { message = "User Not Found!" });
        }

        news.Title = dto.Title.Trim();
        news.CoverURL = dto.CoverURL.Trim();
        news.Content = dto.Content.Trim();
        news.UserId = dto.UserId;
        news.UpdatedAt = DateTime.UtcNow;
        news.PublishedAt = dto.PublishedAt;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteNews(Guid id)
    {
        var news = await _context.News.FirstOrDefaultAsync(item => item.Id == id);
        if (news is null)
        {
            return NotFound(new { message = "News Not Found!" });
        }

        _context.News.Remove(news);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}