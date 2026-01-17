using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class KillerAddonConfiguration : IEntityTypeConfiguration<KillerAddon>
{
    public void Configure(EntityTypeBuilder<KillerAddon> builder)
    {
        builder.ToTable("KillerAddons");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.Slug);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.ImageUrl)
            .HasMaxLength(500);

        builder.Property(a => a.GameVersion)
            .HasMaxLength(20);

        builder.Property(a => a.Rarity)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(a => a.KillerId);
    }
}

public class SurvivorAddonConfiguration : IEntityTypeConfiguration<SurvivorAddon>
{
    public void Configure(EntityTypeBuilder<SurvivorAddon> builder)
    {
        builder.ToTable("SurvivorAddons");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.Slug);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.ImageUrl)
            .HasMaxLength(500);

        builder.Property(a => a.GameVersion)
            .HasMaxLength(20);

        builder.Property(a => a.Rarity)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.ItemType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(a => a.ItemType);
    }
}
