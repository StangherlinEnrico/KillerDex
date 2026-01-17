using Domain.Entities;
using Domain.Entities.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Translations;

public class KillerTranslationConfiguration : IEntityTypeConfiguration<KillerTranslation>
{
    public void Configure(EntityTypeBuilder<KillerTranslation> builder)
    {
        builder.ToTable("KillerTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);
        builder.Property(t => t.Overview).HasMaxLength(2000);
        builder.Property(t => t.Backstory).HasMaxLength(4000);
        builder.Property(t => t.PowerName).HasMaxLength(100);
        builder.Property(t => t.PowerDescription).HasMaxLength(2000);

        builder.HasOne(t => t.Killer).WithMany().HasForeignKey(t => t.KillerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.KillerId, t.Language }).IsUnique();
    }
}

public class SurvivorTranslationConfiguration : IEntityTypeConfiguration<SurvivorTranslation>
{
    public void Configure(EntityTypeBuilder<SurvivorTranslation> builder)
    {
        builder.ToTable("SurvivorTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);
        builder.Property(t => t.Overview).HasMaxLength(2000);
        builder.Property(t => t.Backstory).HasMaxLength(4000);

        builder.HasOne(t => t.Survivor).WithMany().HasForeignKey(t => t.SurvivorId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.SurvivorId, t.Language }).IsUnique();
    }
}

public class ChapterTranslationConfiguration : IEntityTypeConfiguration<ChapterTranslation>
{
    public void Configure(EntityTypeBuilder<ChapterTranslation> builder)
    {
        builder.ToTable("ChapterTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.Chapter).WithMany().HasForeignKey(t => t.ChapterId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.ChapterId, t.Language }).IsUnique();
    }
}

public class PerkTranslationConfiguration : IEntityTypeConfiguration<PerkTranslation>
{
    public void Configure(EntityTypeBuilder<PerkTranslation> builder)
    {
        builder.ToTable("PerkTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.Perk).WithMany().HasForeignKey(t => t.PerkId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.PerkId, t.Language }).IsUnique();
    }
}

public class KillerAddonTranslationConfiguration : IEntityTypeConfiguration<KillerAddonTranslation>
{
    public void Configure(EntityTypeBuilder<KillerAddonTranslation> builder)
    {
        builder.ToTable("KillerAddonTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.KillerAddon).WithMany().HasForeignKey(t => t.KillerAddonId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.KillerAddonId, t.Language }).IsUnique();
    }
}

public class SurvivorAddonTranslationConfiguration : IEntityTypeConfiguration<SurvivorAddonTranslation>
{
    public void Configure(EntityTypeBuilder<SurvivorAddonTranslation> builder)
    {
        builder.ToTable("SurvivorAddonTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.SurvivorAddon).WithMany().HasForeignKey(t => t.SurvivorAddonId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.SurvivorAddonId, t.Language }).IsUnique();
    }
}

public class ItemTranslationConfiguration : IEntityTypeConfiguration<ItemTranslation>
{
    public void Configure(EntityTypeBuilder<ItemTranslation> builder)
    {
        builder.ToTable("ItemTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.ItemId, t.Language }).IsUnique();
    }
}

public class OfferingTranslationConfiguration : IEntityTypeConfiguration<OfferingTranslation>
{
    public void Configure(EntityTypeBuilder<OfferingTranslation> builder)
    {
        builder.ToTable("OfferingTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.Offering).WithMany().HasForeignKey(t => t.OfferingId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.OfferingId, t.Language }).IsUnique();
    }
}

public class RealmTranslationConfiguration : IEntityTypeConfiguration<RealmTranslation>
{
    public void Configure(EntityTypeBuilder<RealmTranslation> builder)
    {
        builder.ToTable("RealmTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.Realm).WithMany().HasForeignKey(t => t.RealmId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.RealmId, t.Language }).IsUnique();
    }
}

public class MapTranslationConfiguration : IEntityTypeConfiguration<MapTranslation>
{
    public void Configure(EntityTypeBuilder<MapTranslation> builder)
    {
        builder.ToTable("MapTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.Map).WithMany().HasForeignKey(t => t.MapId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.MapId, t.Language }).IsUnique();
    }
}

public class StatusEffectTranslationConfiguration : IEntityTypeConfiguration<StatusEffectTranslation>
{
    public void Configure(EntityTypeBuilder<StatusEffectTranslation> builder)
    {
        builder.ToTable("StatusEffectTranslations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Language).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder.HasOne(t => t.StatusEffect).WithMany().HasForeignKey(t => t.StatusEffectId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.StatusEffectId, t.Language }).IsUnique();
    }
}
