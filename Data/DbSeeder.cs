using GameStoreApi.Models.Entities;

namespace GameStoreApi.Data;

public static class DbSeeder
{
    public static void Initialize(GameStoreDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // If data already exists, skip seeding
        if (context.Genres.Any() || context.Games.Any() || context.Users.Any())
        {
            return;
        }

        // Seed Genres
        var genres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Action" },
            new Genre { Id = Guid.NewGuid(), Name = "RPG" },
            new Genre { Id = Guid.NewGuid(), Name = "Strategy" },
            new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
            new Genre { Id = Guid.NewGuid(), Name = "Puzzle" },
            new Genre { Id = Guid.NewGuid(), Name = "Sports" }
        };
        context.Genres.AddRange(genres);
        context.SaveChanges();

        // Seed Games
        var games = new List<Game>
        {
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "The Legend of Zelda: Breath of the Wild",
                Price = 59.99m,
                GenreId = genres.First(g => g.Name == "Adventure").Id,
                ReleaseDate = new DateOnly(2017, 3, 3),
                imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQjpMgXIem2QtsglcBBBNAVqwbeNuLTd6CVOZ-NZqSzgg&s=10"
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Elden Ring",
                Price = 59.99m,
                GenreId = genres.First(g => g.Name == "RPG").Id,
                ReleaseDate = new DateOnly(2022, 2, 25),
                imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSirwsHzzCShA3Be6M9sLeqTawjqFof-XEMa79bc6y-VA&s=10"
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Chess Ultra",
                Price = 14.99m,
                GenreId = genres.First(g => g.Name == "Strategy").Id,
                ReleaseDate = new DateOnly(2019, 7, 30),
                imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5RmIUgo6ZCFR9fAzSkbky18OGWdlGamaDlcYNvntubQ&s=10"
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Call of Duty: Modern Warfare",
                Price = 69.99m,
                GenreId = genres.First(g => g.Name == "Action").Id,
                ReleaseDate = new DateOnly(2019, 10, 25),
                imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQXY3WZqPsxy8_5lYY-aE0CWyAWHMouuPdnUUUCVnnBKZp8hrW8ew6H5hfm&s=10"
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Portal 2",
                Price = 19.99m,
                GenreId = genres.First(g => g.Name == "Puzzle").Id,
                ReleaseDate = new DateOnly(2011, 4, 18),
                imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSe1WnAd3ir7dIQqItaw84kJs04hS2o7grwG1DlSfnbDw&s=10"
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "FIFA 24",
                Price = 59.99m,
                GenreId = genres.First(g => g.Name == "Sports").Id,
                ReleaseDate = new DateOnly(2023, 9, 29),
                imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT6upNO06KTImeHJUphwtnLaiJB-uz2xb_WSusQ5m62kg&s=10"
            }
        };
        context.Games.AddRange(games);
        context.SaveChanges();

        // Seed Users
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@gamestore.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Hash password
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "john_doe",
                Email = "john@gamestore.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "jane_smith",
                Email = "jane@gamestore.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // Seed Carts
        foreach (var user in users)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = user.Id
            };
            context.Carts.Add(cart);
        }
        context.SaveChanges();

        // Seed Reviews
        var reviews = new List<Review>
        {
            new Review
            {
                Id = Guid.NewGuid(),
                Rating = 5.00m,
                Comment = "Amazing game! Absolutely loved it.",
                UserId = users[1].Id,
                GameId = games[0].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Review
            {
                Id = Guid.NewGuid(),
                Rating = 4.50m,
                Comment = "Great experience with challenging gameplay.",
                UserId = users[2].Id,
                GameId = games[1].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Review
            {
                Id = Guid.NewGuid(),
                Rating = 4.00m,
                Comment = "Fun and entertaining, worth the price.",
                UserId = users[1].Id,
                GameId = games[4].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        context.Reviews.AddRange(reviews);
        context.SaveChanges();
    }
}
