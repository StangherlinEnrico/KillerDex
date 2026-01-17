using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RealmConfiguration : IEntityTypeConfiguration<Realm>
{
    public void Configure(EntityTypeBuilder<Realm> builder)
    {
        builder.ToTable("Realms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(r => r.Slug)
            .IsUnique();

        builder.Property(r => r.Description)
            .HasMaxLength(2000);

        builder.Property(r => r.ImageUrl)
            .HasMaxLength(500);

        builder.Property(r => r.GameVersion)
            .HasMaxLength(20);

        builder.HasOne(r => r.Killer)
            .WithMany()
            .HasForeignKey(r => r.KillerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(r => r.Maps)
            .WithOne(m => m.Realm)
            .HasForeignKey(m => m.RealmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MapConfiguration : IEntityTypeConfiguration<Map>
{
    public void Configure(EntityTypeBuilder<Map> builder)
    {
        builder.ToTable("Maps");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(m => m.Slug)
            .IsUnique();

        builder.Property(m => m.Description)
            .HasMaxLength(2000);

        builder.Property(m => m.ImageUrl)
            .HasMaxLength(500);

        builder.Property(m => m.GameVersion)
            .HasMaxLength(20);

        builder.HasIndex(m => m.RealmId);
    }
}
