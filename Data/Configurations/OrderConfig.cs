using GameStoreApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreApi.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.PaymentMethod)
            .IsRequired();

        builder.Property(o => o.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}