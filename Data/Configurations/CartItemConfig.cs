using GameStoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreApi.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Game)
            .WithMany(g => g.CartItems)
            .HasForeignKey(ci => ci.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ci => new { ci.CartId, ci.GameId })
            .IsUnique();
    }
}