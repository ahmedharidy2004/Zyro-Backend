using GameStoreApi.Data;
using GameStoreApi.Dtos.Genres;
using GameStoreApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly GameStoreDbContext _context;

    public GenresController(GameStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
    {
        var genres = await _context.Genres.Select(genre => new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        }).ToListAsync();

        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetGenreById(Guid id)
    {
        var genre = await _context.Genres
                            .Where(genre => genre.Id == id)
                            .Select(genre => new GenreDto
                            {
                                Id = genre.Id,
                                Name = genre.Name
                            }).FirstOrDefaultAsync();

        if(genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> CreateGenre(CreateGenreDto dto)
    {
        var CreatedGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        _context.Genres.Add(CreatedGenre);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGenreById), new { id = CreatedGenre.Id }, CreatedGenre);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGenre(Guid id, UpdateGenreDto dto)
    {
        var UpdatedGenre = await _context.Genres.FirstOrDefaultAsync(genre => genre.Id == id);

        if(UpdatedGenre is null)
        {
            return NotFound();
        } 


        UpdatedGenre.Name = dto.Name;
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(Guid id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(genre => genre.Id == id);

        if(genre is null)
        {
            return NotFound();
        } 

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}