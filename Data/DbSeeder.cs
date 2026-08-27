using GameStoreApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Data;

public static class DbSeeder
{
    public static void Initialize(GameStoreDbContext context)
    {
        context.Database.Migrate();

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

        var games = new List<Game>
        {
            // ---- Action ----
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "DOOM Eternal",
                Price = 39.99m,
                HasDiscount = true,
                DiscountRate = 0.75m,
                GenreId = genres.First(g => g.Name == "Action").Id,
                ReleaseDate = new DateOnly(2020, 3, 20),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/782330/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/_J2YcaVqjCQ",
                Description = "DOOM Eternal is a fast-paced first-person shooter where you play as the Doom Slayer, battling hordes of demons across Earth and beyond. Armed with brutal weapons and powerful abilities, you must fight relentlessly to stop the forces of Hell and save humanity.\n\nFight. Kill. Survive. Become the Slayer."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Sekiro: Shadows Die Twice",
                Price = 59.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "Action").Id,
                ReleaseDate = new DateOnly(2019, 3, 22),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/814380/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/rXMX4YJ7Lks",
                Description = "Sekiro: Shadows Die Twice is a challenging action-adventure game set in a dark, reimagined version of feudal Japan. You play as a shinobi tasked with protecting his young master and seeking revenge against those who betrayed him. Master precise sword combat, use stealth and prosthetic tools, and face deadly enemies in a brutal fight for survival.\n\nHesitation is defeat."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Devil May Cry 5",
                Price = 29.99m,
                HasDiscount = true,
                DiscountRate = 0.50m,
                GenreId = genres.First(g => g.Name == "Action").Id,
                ReleaseDate = new DateOnly(2019, 3, 8),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/601150/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/dG6_CAdiLPM",
                Description = "Devil May Cry 5 is a stylish action game that follows Nero, Dante, and the mysterious V as they battle powerful demons threatening the world. Switch between unique combat styles, master devastating weapons and abilities, and unleash spectacular combos as you fight through a demon-infested city.\n\nStyle. Skill. Demon-slaying."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Metal Gear Solid V: The Phantom Pain",
                Price = 19.99m,
                HasDiscount = true,
                DiscountRate = 0.80m,
                GenreId = genres.First(g => g.Name == "Action").Id,
                ReleaseDate = new DateOnly(2015, 9, 1),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/287700/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/C19ap2M7DDE",
                Description = "Metal Gear Solid V: The Phantom Pain is an open-world stealth action game that follows Venom Snake on a mission of revenge after the destruction of his forces. Infiltrate enemy bases, rescue prisoners, recruit soldiers, manage your private army, and choose how to approach each mission in a vast and dangerous world.\n\nTactical espionage. One soldier. One mission."
            },

            // ---- RPG ----
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Elden Ring",
                Price = 59.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "RPG").Id,
                ReleaseDate = new DateOnly(2022, 2, 25),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1245620/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/E3Huy2cdih0",
                Description = "Elden Ring is an expansive action RPG set in the mysterious and brutal world of the Lands Between. As a Tarnished, explore vast landscapes, uncover ancient secrets, battle terrifying creatures and powerful demigods, and forge your own path toward becoming the Elden Lord.\n\nRise, Tarnished. Become the Elden Lord."
            },

            new Game
            {
                Id = Guid.NewGuid(),
                Name = "The Witcher 3: Wild Hunt",
                Price = 39.99m,
                HasDiscount = true,
                DiscountRate = 0.70m,
                GenreId = genres.First(g => g.Name == "RPG").Id,
                ReleaseDate = new DateOnly(2015, 5, 19),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/292030/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/XHrskkHf958",
                Description="The Witcher 3: Wild Hunt is an open-world action RPG that follows Geralt of Rivia, a monster hunter searching for his adopted daughter while a supernatural force known as the Wild Hunt closes in. Explore a vast fantasy world, hunt dangerous monsters, make difficult choices, and shape the story through your actions.\n\nDestiny awaits. The hunt begins."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Cyberpunk 2077",
                Price = 59.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "RPG").Id,
                ReleaseDate = new DateOnly(2020, 12, 10),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1091500/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/8X2kIfS6fb8",
                Description = "Cyberpunk 2077 is an open-world action RPG set in the futuristic metropolis of Night City. Play as V, a mercenary chasing a unique cybernetic implant that holds the key to immortality. Customize your abilities, explore a dangerous world of corporations and gangs, and make choices that shape your journey.\n\nLive fast. Die legendary."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Persona 5 Royal",
                Price = 59.99m,
                HasDiscount = true,
                DiscountRate = 0.30m,
                GenreId = genres.First(g => g.Name == "RPG").Id,
                ReleaseDate = new DateOnly(2022, 10, 21),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1687950/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/SKpSpvFCZRw",
                Description = "Persona 5 Royal is a stylish turn-based RPG that follows the Phantom Thieves, a group of high school students who use mysterious powers to change the hearts of corrupt adults. Balance school life, friendships, and daily activities while exploring supernatural palaces and battling enemies in a hidden world.\n\nTake your time. Steal their hearts."
            },

            // ---- Strategy ----
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Sid Meier's Civilization VI",
                Price = 59.99m,
                HasDiscount = true,
                DiscountRate = 0.75m,
                GenreId = genres.First(g => g.Name == "Strategy").Id,
                ReleaseDate = new DateOnly(2016, 10, 21),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/289070/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/5KdE0p2joJw",
                Description = "Sid Meier's Civilization VI is a turn-based strategy game where you build and lead a civilization from the Stone Age into the modern era and beyond. Explore the world, develop cities, research technologies, build armies, form alliances, and compete with rival civilizations to achieve victory.\n\nBuild your empire. Shape history."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Age of Empires IV",
                Price = 59.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "Strategy").Id,
                ReleaseDate = new DateOnly(2021, 10, 28),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1466860/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/5TnynE3PuDE",
                Description = "Age of Empires IV is a real-time strategy game that lets you build and command powerful civilizations across centuries of history. Gather resources, develop your economy, train armies, conquer territories, and lead your civilization through epic historical battles.\n\nBuild. Conquer. Make history."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "XCOM 2",
                Price = 59.99m,
                HasDiscount = true,
                DiscountRate = 0.85m,
                GenreId = genres.First(g => g.Name == "Strategy").Id,
                ReleaseDate = new DateOnly(2016, 2, 5),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/268500/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/ZlF4_o3qALo",
                Description = "XCOM 2 is a turn-based tactical strategy game where humanity fights back against an alien occupation of Earth.\n Lead a resistance force, manage your base, customize your soldiers, research advanced technology, and make critical decisions on the battlefield where every mistake can be fatal.\n\nResist. Adapt. Take back Earth."
            },

            // ---- Adventure ----
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Red Dead Redemption 2",
                Price = 59.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "Adventure").Id,
                ReleaseDate = new DateOnly(2019, 11, 5),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1174180/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/gmA6MrX81z4",
                Description = "Red Dead Redemption 2 is an open-world action-adventure game set in the American frontier during the decline of the Wild West. Play as Arthur Morgan, an outlaw and member of the Van der Linde gang, as he struggles with loyalty, survival, and the consequences of a changing world.\n\nOutlaws for life. The end of an era."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Death Stranding Director's Cut",
                Price = 39.99m,
                HasDiscount = true,
                DiscountRate = 0.60m,
                GenreId = genres.First(g => g.Name == "Adventure").Id,
                ReleaseDate = new DateOnly(2022, 3, 30),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1850570/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/QlLEmu8c-Vk",
                Description = "Death Stranding Director's Cut is a unique open-world action game set in a fractured future where humanity is isolated and mysterious supernatural phenomena threaten the world. Play as Sam Porter Bridges, delivering vital supplies across a dangerous landscape while reconnecting isolated communities and uncovering the truth behind the Death Stranding.\n\nReconnect the world. Rebuild the future."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "A Plague Tale: Requiem",
                Price = 49.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "Adventure").Id,
                ReleaseDate = new DateOnly(2022, 10, 18),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1182900/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/NAuzkT5A4xM",
                Description = "A Plague Tale: Requiem is a dark action-adventure game that follows Amicia and her younger brother Hugo as they journey through a war-torn medieval world plagued by deadly rats and dangerous enemies. Use stealth, strategy, and powerful abilities to survive while searching for a cure and protecting the people you love.\n\nSurvive the darkness. Protect your brother."
            },

            // ---- Puzzle ----
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Portal 2",
                Price = 9.99m,
                HasDiscount = true,
                DiscountRate = 0.90m,
                GenreId = genres.First(g => g.Name == "Puzzle").Id,
                ReleaseDate = new DateOnly(2011, 4, 18),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/620/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/tax4e4hBBZc",
                Description = "Portal 2 is a first-person puzzle game where you use the Portal Gun to create linked portals and manipulate space to overcome increasingly challenging tests. Explore the mysterious Aperture Science facility, solve clever puzzles, and uncover a darkly humorous story alongside memorable characters.\n\nThink with portals."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Tetris Effect: Connected",
                Price = 39.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "Puzzle").Id,
                ReleaseDate = new DateOnly(2020, 7, 23),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1003590/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/Mwcf-vC6q5s",
                Description = "Tetris Effect: Connected is a mesmerizing take on the classic puzzle game, combining iconic Tetris gameplay with immersive visuals, music, and rhythmic effects. Play solo or with others across multiple modes, including cooperative and competitive challenges.\n\nTetris like you've never seen it before."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "The Witness",
                Price = 39.99m,
                HasDiscount = true,
                DiscountRate = 0.65m,
                GenreId = genres.First(g => g.Name == "Puzzle").Id,
                ReleaseDate = new DateOnly(2016, 1, 26),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/210970/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/ul7kNFD6noU",
                Description = "The Witness is a first-person puzzle adventure set on a mysterious, beautifully designed island filled with hundreds of puzzles. Explore the environment, discover hidden connections, and learn the rules of each puzzle through observation and experimentation.\n\nExplore. Observe. Understand."
            },

            // ---- Sports ----
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Tony Hawk's Pro Skater 1 + 2",
                Price = 39.99m,
                HasDiscount = true,
                DiscountRate = 0.50m,
                GenreId = genres.First(g => g.Name == "Sports").Id,
                ReleaseDate = new DateOnly(2020, 9, 4),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/1888160/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/4paYDD0WIVY",
                Description = "Tony Hawk's Pro Skater 1 + 2 brings the first two legendary skateboarding games together in a fully remastered package. Skate as iconic pros, land massive tricks and combos, complete challenging goals, and customize your skater across classic and modern modes.\n\nSkate. Trick. Repeat."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "NBA 2K24",
                Price = 69.99m,
                HasDiscount = false,
                DiscountRate = 0m,
                GenreId = genres.First(g => g.Name == "Sports").Id,
                ReleaseDate = new DateOnly(2023, 9, 8),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/2338770/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/GITzbGIiNKg",
                Description = "NBA 2K24 is a basketball simulation game featuring realistic gameplay, competitive online modes, and iconic NBA experiences. Build your own player, compete in MyCAREER, manage a team in MyNBA, and relive legendary moments from the career of Kobe Bryant.\n\nThe game is yours."
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Football Manager 2024",
                Price = 59.99m,
                HasDiscount = true,
                DiscountRate = 0.25m,
                GenreId = genres.First(g => g.Name == "Sports").Id,
                ReleaseDate = new DateOnly(2023, 11, 6),
                imageURL = "https://cdn.akamai.steamstatic.com/steam/apps/2252570/library_600x900.jpg",
                TrailerURL = "https://www.youtube.com/embed/QvyMqBtiJDg",
                Description = "Football Manager 2024 is a detailed football management simulation where you take control of a club and build it into a winning team. Scout and sign players, create tactics, manage your squad, develop young talent, and make crucial decisions on and off the pitch.\n\nYour club. Your tactics. Your legacy."
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

        // Seed News
        var admin = users.First(u => u.Role == "Admin");

        var newsList = new List<News>
        {
            new News
            {
                Id = Guid.NewGuid(),
                Title = "Welcome to GameStore!",
                Content = "We're excited to launch our brand new digital storefront. Browse our curated catalog of Action, RPG, Strategy, Adventure, Puzzle, and Sports titles, all at great prices. Thanks for being one of our first players!",
                CoverURL = "https://i.pinimg.com/736x/34/ce/01/34ce0171b6e064b8373b1e04feb5f7e6.jpg",
                UserId = admin.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow
            },
            new News
            {
                Id = Guid.NewGuid(),
                Title = "Summer Sale is Live",
                Content = "Our biggest sale of the season has arrived. Titles like DOOM Eternal, Metal Gear Solid V, and The Witcher 3 are discounted for a limited time. Check your favorite genres and grab your next adventure before the sale ends.",
                CoverURL = "https://i.pinimg.com/736x/4b/9a/fb/4b9afbb8721bd9678d6d80d1ffffb466.jpg",
                UserId = admin.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow
            },
            new News
            {
                Id = Guid.NewGuid(),
                Title = "Elden Ring Community Spotlight",
                Content = "Elden Ring continues to be one of our most-reviewed titles. Thank you to everyone who left feedback and ratings. Stay tuned for more RPG additions to the catalog in the coming weeks.",
                UserId = admin.Id,
                CoverURL = "https://i.pinimg.com/736x/4b/9a/fb/4b9afbb8721bd9678d6d80d1ffffb466.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow
            },
            new News
            {
                Id = Guid.NewGuid(),
                Title = "New Sports Lineup Added",
                Content = "We've expanded our Sports section with the latest titles including NBA 2K24 and Football Manager 2024. Whether you're on the court or managing from the sidelines, there's something new to play.",
                UserId = admin.Id,
                CoverURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSHozZrEt8eom6w7QXLdXYZudcqUQm7fN_R6KxWtP6XGA&s=10",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow
            },
            new News
            {
                Id = Guid.NewGuid(),
                Title = "Puzzle Lovers, Rejoice",
                Content = "Tetris Effect: Connected and The Witness are now featured in our Puzzle category. Both titles offer unique takes on the genre, from rhythmic block-stacking to serene island exploration.",
                UserId = admin.Id,
                CoverURL = "https://i.pinimg.com/736x/4e/60/c5/4e60c59a1cd4e9aed25abf2c995527d0.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow
            },
            new News
            {
                Id = Guid.NewGuid(),
                Title = "Platform Update: Reviews & Ratings",
                Content = "You can now leave ratings and comments on any game you've purchased. Help other players discover great titles by sharing your honest feedback on the store.",
                UserId = admin.Id,
                CoverURL = "https://i.pinimg.com/736x/de/7b/bd/de7bbd4003e147accefaed9954ef419d.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow
            }
        };
        context.News.AddRange(newsList);
        context.SaveChanges();
    }
}