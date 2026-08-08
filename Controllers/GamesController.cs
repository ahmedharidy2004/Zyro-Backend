using GameStoreApi.Data;
using GameStoreApi.Dtos.Games;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public GamesController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
    {
        var games = await _context.Games
                        .Select(game => new GameDto
                        {
                            Id = game.Id,
                            Name = game.Name,
                            Price = game.Price,
                            GenreId = game.GenreId,
                            ReleaseDate = game.ReleaseDate
                        }).ToListAsync();

        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetGameById(Guid id)
    {
        var game = await _context.Games
            .Where(game => game.Id == id)
            .Select(game => new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price,
                GenreId = game.GenreId,
                ReleaseDate = game.ReleaseDate
            })
            .FirstOrDefaultAsync();

        if (game is null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> CreateGame([FromBody] CreateGameDto dto)
    {
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Price = dto.Price,
            GenreId = dto.GenreId,
            ReleaseDate = dto.ReleaseDate
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetGameById),
            new { id = game.Id },
            game
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGame(Guid id,[FromBody] UpdateGameDto dto)
    {
        var game = await _context.Games
            .FirstOrDefaultAsync(game => game.Id == id);

        if (game is null)
        {
            return NotFound();
        }

        game.Name = dto.Name;
        game.Price = dto.Price;
        game.GenreId = dto.GenreId;
        game.ReleaseDate = dto.ReleaseDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(Guid id)
    {
        var game = await _context.Games
                        .FirstOrDefaultAsync(game => game.Id == id);

        if(game is null)
        {
            return NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}