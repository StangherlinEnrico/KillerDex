using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class KillerConfiguration : IEntityTypeConfiguration<Killer>
{
    public void Configure(EntityTypeBuilder<Killer> builder)
    {
        builder.ToTable("Killers");

        builder.HasKey(k => k.Id);

        builder.Property(k => k.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(k => k.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(k => k.Slug)
            .IsUnique();

        builder.Property(k => k.RealName)
            .HasMaxLength(100);

        builder.Property(k => k.Overview)
            .HasMaxLength(2000);

        builder.Property(k => k.Backstory)
            .HasMaxLength(4000);

        builder.Property(k => k.ImageUrl)
            .HasMaxLength(500);

        builder.Property(k => k.GameVersion)
            .HasMaxLength(20);

        builder.Property(k => k.MovementSpeed)
            .HasPrecision(4, 2);

        builder.OwnsOne(k => k.Power, power =>
        {
            power.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("PowerName");

            power.Property(p => p.Description)
                .HasMaxLength(2000)
                .HasColumnName("PowerDescription");
        });

        builder.HasOne(k => k.Chapter)
            .WithMany(c => c.Killers)
            .HasForeignKey(k => k.ChapterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(k => k.Perks)
            .WithOne(p => p.Killer)
            .HasForeignKey(p => p.KillerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(k => k.Addons)
            .WithOne(a => a.Killer)
            .HasForeignKey(a => a.KillerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
