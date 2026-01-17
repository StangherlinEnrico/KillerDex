using Domain.Enums;

namespace Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public ItemType Type { get; private set; }
    public Rarity Rarity { get; private set; }

    private readonly List<SurvivorAddon> _addons = [];
    public IReadOnlyList<SurvivorAddon> Addons => _addons.AsReadOnly();

    private Item() { }

    public Item(string name, ItemType type, Rarity rarity, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Item");
        ValidateDescription(description, "Item");

        Name = name;
        Slug = GenerateSlug(name);
        Type = type;
        Rarity = rarity;
        Description = description;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null, Rarity? rarity = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Item");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Item");
            Description = description;
        }

        if (rarity.HasValue)
            Rarity = rarity.Value;

        MarkAsUpdated();
    }
}
