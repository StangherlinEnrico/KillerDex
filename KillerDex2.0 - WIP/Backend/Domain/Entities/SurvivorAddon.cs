using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a survivor addon (item addon) in Dead by Daylight.
/// Survivor addons are categorized by ItemType and can be used with items of that type.
/// </summary>
public class SurvivorAddon : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Rarity Rarity { get; private set; }
    public ItemType ItemType { get; private set; }

    private SurvivorAddon() { }

    public SurvivorAddon(string name, Rarity rarity, ItemType itemType, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Survivor addon");
        ValidateDescription(description, "Survivor addon");

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Rarity = rarity;
        ItemType = itemType;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null, Rarity? rarity = null, ItemType? itemType = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Survivor addon");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Survivor addon");
            Description = description;
        }

        if (rarity.HasValue)
            Rarity = rarity.Value;

        if (itemType.HasValue)
            ItemType = itemType.Value;

        MarkAsUpdated();
    }
}
