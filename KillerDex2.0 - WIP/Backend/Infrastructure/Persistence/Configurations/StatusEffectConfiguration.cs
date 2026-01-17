using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StatusEffectConfiguration : IEntityTypeConfiguration<StatusEffect>
{
    public void Configure(EntityTypeBuilder<StatusEffect> builder)
    {
        builder.ToTable("StatusEffects");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Slug)
            .IsUnique();

        builder.Property(s => s.Description)
            .HasMaxLength(2000);

        builder.Property(s => s.ImageUrl)
            .HasMaxLength(500);

        builder.Property(s => s.GameVersion)
            .HasMaxLength(20);

        builder.Property(s => s.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.AppliesTo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(s => s.Type);
        builder.HasIndex(s => s.AppliesTo);
    }
}
