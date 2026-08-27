using GameStoreApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreApi.Data.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
	public void Configure(EntityTypeBuilder<News> builder)
	{
		builder.HasKey(n => n.Id);

		builder.Property(n => n.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(n => n.CoverURL)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(n => n.Content)
			.IsRequired()
			.HasMaxLength(4000);

		builder.Property(n => n.CreatedAt)
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.Property(n => n.UpdatedAt)
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.Property(n => n.PublishedAt);

		builder.HasOne<User>()
			.WithMany(u => u.News)
			.HasForeignKey(n => n.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
