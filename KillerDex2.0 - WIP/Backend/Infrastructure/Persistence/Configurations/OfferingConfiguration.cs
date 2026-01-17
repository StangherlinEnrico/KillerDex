using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OfferingConfiguration : IEntityTypeConfiguration<Offering>
{
    public void Configure(EntityTypeBuilder<Offering> builder)
    {
        builder.ToTable("Offerings");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(o => o.Slug)
            .IsUnique();

        builder.Property(o => o.Description)
            .HasMaxLength(2000);

        builder.Property(o => o.ImageUrl)
            .HasMaxLength(500);

        builder.Property(o => o.GameVersion)
            .HasMaxLength(20);

        builder.Property(o => o.Rarity)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(o => o.Role);
    }
}
