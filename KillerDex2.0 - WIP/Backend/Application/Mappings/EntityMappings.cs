using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappings;

public static class EntityMappings
{
    // Killer mappings
    public static KillerDto ToDto(this Killer killer) => new(
        killer.Id,
        killer.Slug,
        killer.Name,
        killer.RealName,
        killer.Overview,
        killer.Backstory,
        killer.ImageUrl,
        killer.GameVersion,
        new PowerDto(killer.Power.Name, killer.Power.Description),
        killer.MovementSpeed,
        killer.TerrorRadius,
        killer.Height.ToString(),
        killer.Chapter?.ToSummaryDto(),
        killer.CreatedAt,
        killer.UpdatedAt
    );

    public static KillerSummaryDto ToSummaryDto(this Killer killer) => new(
        killer.Id,
        killer.Slug,
        killer.Name,
        killer.ImageUrl,
        killer.Power.Name
    );

    // Survivor mappings
    public static SurvivorDto ToDto(this Survivor survivor) => new(
        survivor.Id,
        survivor.Slug,
        survivor.Name,
        survivor.Overview,
        survivor.Backstory,
        survivor.ImageUrl,
        survivor.GameVersion,
        survivor.Chapter?.ToSummaryDto(),
        survivor.CreatedAt,
        survivor.UpdatedAt
    );

    public static SurvivorSummaryDto ToSummaryDto(this Survivor survivor) => new(
        survivor.Id,
        survivor.Slug,
        survivor.Name,
        survivor.ImageUrl
    );

    // Chapter mappings
    public static ChapterDto ToDto(this Chapter chapter, IEnumerable<Killer> killers, IEnumerable<Survivor> survivors) => new(
        chapter.Id,
        chapter.Slug,
        chapter.Name,
        chapter.Number,
        chapter.ReleaseDate,
        chapter.ImageUrl,
        chapter.GameVersion,
        killers.Select(k => k.ToSummaryDto()),
        survivors.Select(s => s.ToSummaryDto()),
        chapter.CreatedAt,
        chapter.UpdatedAt
    );

    public static ChapterSummaryDto ToSummaryDto(this Chapter chapter) => new(
        chapter.Id,
        chapter.Slug,
        chapter.Name,
        chapter.Number
    );

    // Perk mappings
    public static PerkDto ToDto(this Perk perk) => new(
        perk.Id,
        perk.Slug,
        perk.Name,
        perk.Description,
        perk.ImageUrl,
        perk.GameVersion,
        perk.Role.ToString(),
        perk.GetOwnerSummary(),
        perk.CreatedAt,
        perk.UpdatedAt
    );

    public static PerkSummaryDto ToSummaryDto(this Perk perk) => new(
        perk.Id,
        perk.Slug,
        perk.Name,
        perk.ImageUrl,
        perk.Role.ToString()
    );

    private static OwnerSummaryDto? GetOwnerSummary(this Perk perk)
    {
        if (perk.Killer is not null)
            return new OwnerSummaryDto(perk.Killer.Id, perk.Killer.Slug, perk.Killer.Name, "Killer");
        if (perk.Survivor is not null)
            return new OwnerSummaryDto(perk.Survivor.Id, perk.Survivor.Slug, perk.Survivor.Name, "Survivor");
        return null;
    }

    // KillerAddon mappings
    public static KillerAddonDto ToDto(this KillerAddon addon) => new(
        addon.Id,
        addon.Slug,
        addon.Name,
        addon.Description,
        addon.ImageUrl,
        addon.GameVersion,
        addon.Rarity.ToString(),
        addon.Killer.ToSummaryDto(),
        addon.CreatedAt,
        addon.UpdatedAt
    );

    public static AddonSummaryDto ToSummaryDto(this KillerAddon addon) => new(
        addon.Id,
        addon.Slug,
        addon.Name,
        addon.ImageUrl,
        addon.Rarity.ToString()
    );

    // SurvivorAddon mappings
    public static SurvivorAddonDto ToDto(this SurvivorAddon addon) => new(
        addon.Id,
        addon.Slug,
        addon.Name,
        addon.Description,
        addon.ImageUrl,
        addon.GameVersion,
        addon.Rarity.ToString(),
        addon.ItemType.ToString(),
        addon.CreatedAt,
        addon.UpdatedAt
    );

    public static AddonSummaryDto ToSummaryDto(this SurvivorAddon addon) => new(
        addon.Id,
        addon.Slug,
        addon.Name,
        addon.ImageUrl,
        addon.Rarity.ToString()
    );

    // Item mappings
    public static ItemDto ToDto(this Item item) => new(
        item.Id,
        item.Slug,
        item.Name,
        item.Description,
        item.ImageUrl,
        item.GameVersion,
        item.Type.ToString(),
        item.Rarity.ToString(),
        item.CreatedAt,
        item.UpdatedAt
    );

    public static ItemSummaryDto ToSummaryDto(this Item item) => new(
        item.Id,
        item.Slug,
        item.Name,
        item.ImageUrl,
        item.Type.ToString(),
        item.Rarity.ToString()
    );

    // Offering mappings
    public static OfferingDto ToDto(this Offering offering) => new(
        offering.Id,
        offering.Slug,
        offering.Name,
        offering.Description,
        offering.ImageUrl,
        offering.GameVersion,
        offering.Rarity.ToString(),
        offering.Role.ToString(),
        offering.CreatedAt,
        offering.UpdatedAt
    );

    public static OfferingSummaryDto ToSummaryDto(this Offering offering) => new(
        offering.Id,
        offering.Slug,
        offering.Name,
        offering.ImageUrl,
        offering.Rarity.ToString(),
        offering.Role.ToString()
    );

    // Realm mappings
    public static RealmDto ToDto(this Realm realm, IEnumerable<Map> maps) => new(
        realm.Id,
        realm.Slug,
        realm.Name,
        realm.Description,
        realm.ImageUrl,
        realm.GameVersion,
        realm.Killer?.ToSummaryDto(),
        maps.Select(m => m.ToSummaryDto()),
        realm.CreatedAt,
        realm.UpdatedAt
    );

    public static RealmSummaryDto ToSummaryDto(this Realm realm) => new(
        realm.Id,
        realm.Slug,
        realm.Name,
        realm.ImageUrl
    );

    // Map mappings
    public static MapDto ToDto(this Map map) => new(
        map.Id,
        map.Slug,
        map.Name,
        map.Description,
        map.ImageUrl,
        map.GameVersion,
        map.Realm.ToSummaryDto(),
        map.CreatedAt,
        map.UpdatedAt
    );

    public static MapSummaryDto ToSummaryDto(this Map map) => new(
        map.Id,
        map.Slug,
        map.Name,
        map.ImageUrl
    );

    // StatusEffect mappings
    public static StatusEffectDto ToDto(this StatusEffect statusEffect) => new(
        statusEffect.Id,
        statusEffect.Slug,
        statusEffect.Name,
        statusEffect.Description,
        statusEffect.ImageUrl,
        statusEffect.GameVersion,
        statusEffect.Type.ToString(),
        statusEffect.AppliesTo.ToString(),
        statusEffect.CreatedAt,
        statusEffect.UpdatedAt
    );

    public static StatusEffectSummaryDto ToSummaryDto(this StatusEffect statusEffect) => new(
        statusEffect.Id,
        statusEffect.Slug,
        statusEffect.Name,
        statusEffect.ImageUrl,
        statusEffect.Type.ToString()
    );
}
