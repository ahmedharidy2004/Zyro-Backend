using System.Security.Claims;
using GameStoreApi.Data;
using GameStoreApi.Dtos.Reviews;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public ReviewController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
    {
        var reviews = await _context.Reviews
            .AsNoTracking()
            .Select(review => new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                UserId = review.UserId,
                Username = review.User.Username,
                GameId = review.GameId,
                GameName = review.Game.Name,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            })
            .ToListAsync();

        return Ok(reviews);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReviewDto>> GetReviewById(Guid id)
    {
        var review = await _context.Reviews
            .AsNoTracking()
            .Where(review => review.Id == id)
            .Select(review => new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                UserId = review.UserId,
                Username = review.User.Username,
                GameId = review.GameId,
                GameName = review.Game.Name,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (review is null)
        {
            return NotFound(new { message = "Review Not Found!" });
        }

        return Ok(review);
    }

    [HttpGet("game/{gameId:guid}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewByGameId(Guid gameId)
    {
        var reviews = await _context.Reviews
            .AsNoTracking()
            .Where(review => review.GameId == gameId)
            .Select(review => new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                UserId = review.UserId,
                Username = review.User.Username,
                GameId = review.GameId,
                GameName = review.Game.Name,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            })
            .OrderByDescending(review => review.CreatedAt)
            .ToListAsync();

        if (reviews.Count == 0)
        {
            return NotFound(new { message = "No reviews found for this game!" });
        }

        return Ok(reviews);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReviewDto>> CreateReview([FromBody] CreateReviewDto dto)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
        {
            return BadRequest(new { message = "Rating must be between 1 and 5." });
        }

        if (string.IsNullOrWhiteSpace(dto.Comment))
        {
            return BadRequest(new { message = "Comment is required." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return Unauthorized();
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == userGuid);

        if (user is null)
        {
            return Unauthorized();
        }

        var game = await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(game => game.Id == dto.GameId);

        if (game is null)
        {
            return NotFound(new { message = "Game Not Found!" });
        }

        var existingReview =await _context.Reviews.FirstOrDefaultAsync(review => review.UserId == userGuid
                                                                         && review.GameId == dto.GameId);

        if(existingReview is not null)
        {
            return BadRequest(new { message = "You already created a review on this game!"});
        }                                                             

        var now = DateTime.UtcNow;
        var review = new Review
        {
            Id = Guid.NewGuid(),
            Rating = dto.Rating,
            Comment = dto.Comment.Trim(),
            GameId = dto.GameId,
            UserId = userGuid,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            UserId = review.UserId,
            Username = user.Username,
            GameId = review.GameId,
            GameName = game.Name,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };

        return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, reviewDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> UpdateReview(Guid id, [FromBody] UpdateReviewDto dto)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
        {
            return BadRequest(new { message = "Rating must be between 1 and 5." });
        }

        if (string.IsNullOrWhiteSpace(dto.Comment))
        {
            return BadRequest(new { message = "Comment is required." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return Unauthorized();
        }

        var review = await _context.Reviews
            .FirstOrDefaultAsync(review => review.Id == id && review.UserId == userGuid);

        if (review is null)
        {
            return NotFound(new { message = "Review Not Found!" });
        }

        review.Rating = dto.Rating;
        review.Comment = dto.Comment.Trim();
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteReview(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return Unauthorized();
        }

        var review = await _context.Reviews
            .FirstOrDefaultAsync(review => review.Id == id && review.UserId == userGuid);

        if (review is null)
        {
            return NotFound(new { message = "Review Not Found!" });
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("admin/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteReviewAsAdmin(Guid id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(review => review.Id == id);

        if (review is null)
        {
            return NotFound(new { message = "Review Not Found!" });
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}