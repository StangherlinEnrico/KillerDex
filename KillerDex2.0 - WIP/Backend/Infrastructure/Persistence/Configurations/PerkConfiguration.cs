using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PerkConfiguration : IEntityTypeConfiguration<Perk>
{
    public void Configure(EntityTypeBuilder<Perk> builder)
    {
        builder.ToTable("Perks");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Slug)
            .IsUnique();

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        builder.Property(p => p.GameVersion)
            .HasMaxLength(20);

        builder.Property(p => p.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(p => p.Role);
    }
}
