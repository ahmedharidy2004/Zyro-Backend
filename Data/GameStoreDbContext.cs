using GameStoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Data;

public class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(
        DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}