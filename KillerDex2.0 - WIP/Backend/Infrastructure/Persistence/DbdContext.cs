using Domain.Entities;
using Domain.Entities.Translations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class DbdContext : DbContext
{
    public DbdContext(DbContextOptions<DbdContext> options) : base(options)
    {
    }

    // Main entities
    public DbSet<Killer> Killers => Set<Killer>();
    public DbSet<Survivor> Survivors => Set<Survivor>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Perk> Perks => Set<Perk>();
    public DbSet<KillerAddon> KillerAddons => Set<KillerAddon>();
    public DbSet<SurvivorAddon> SurvivorAddons => Set<SurvivorAddon>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Offering> Offerings => Set<Offering>();
    public DbSet<Realm> Realms => Set<Realm>();
    public DbSet<Map> Maps => Set<Map>();
    public DbSet<StatusEffect> StatusEffects => Set<StatusEffect>();

    // Translations
    public DbSet<KillerTranslation> KillerTranslations => Set<KillerTranslation>();
    public DbSet<SurvivorTranslation> SurvivorTranslations => Set<SurvivorTranslation>();
    public DbSet<ChapterTranslation> ChapterTranslations => Set<ChapterTranslation>();
    public DbSet<PerkTranslation> PerkTranslations => Set<PerkTranslation>();
    public DbSet<KillerAddonTranslation> KillerAddonTranslations => Set<KillerAddonTranslation>();
    public DbSet<SurvivorAddonTranslation> SurvivorAddonTranslations => Set<SurvivorAddonTranslation>();
    public DbSet<ItemTranslation> ItemTranslations => Set<ItemTranslation>();
    public DbSet<OfferingTranslation> OfferingTranslations => Set<OfferingTranslation>();
    public DbSet<RealmTranslation> RealmTranslations => Set<RealmTranslation>();
    public DbSet<MapTranslation> MapTranslations => Set<MapTranslation>();
    public DbSet<StatusEffectTranslation> StatusEffectTranslations => Set<StatusEffectTranslation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Ignore DomainEvents - they are not persisted to the database
        modelBuilder.Ignore<Domain.Events.DomainEvent>();

        // Ignore DomainEvents property on all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var domainEventsProperty = entityType.FindProperty("DomainEvents");
            if (domainEventsProperty != null)
            {
                entityType.RemoveProperty(domainEventsProperty);
            }
        }
    }
}
