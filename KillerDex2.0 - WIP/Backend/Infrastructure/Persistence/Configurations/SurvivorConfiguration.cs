using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SurvivorConfiguration : IEntityTypeConfiguration<Survivor>
{
    public void Configure(EntityTypeBuilder<Survivor> builder)
    {
        builder.ToTable("Survivors");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Slug)
            .IsUnique();

        builder.Property(s => s.Overview)
            .HasMaxLength(2000);

        builder.Property(s => s.Backstory)
            .HasMaxLength(4000);

        builder.Property(s => s.ImageUrl)
            .HasMaxLength(500);

        builder.Property(s => s.GameVersion)
            .HasMaxLength(20);

        builder.HasOne(s => s.Chapter)
            .WithMany(c => c.Survivors)
            .HasForeignKey(s => s.ChapterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Perks)
            .WithOne(p => p.Survivor)
            .HasForeignKey(p => p.SurvivorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
