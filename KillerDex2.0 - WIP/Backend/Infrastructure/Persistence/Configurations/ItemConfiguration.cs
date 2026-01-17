using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(i => i.Slug)
            .IsUnique();

        builder.Property(i => i.Description)
            .HasMaxLength(2000);

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.GameVersion)
            .HasMaxLength(20);

        builder.Property(i => i.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(i => i.Rarity)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(i => i.Type);

        builder.Ignore(i => i.Addons);
    }
}
