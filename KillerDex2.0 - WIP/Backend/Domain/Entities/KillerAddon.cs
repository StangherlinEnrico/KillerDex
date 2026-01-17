using Domain.Enums;

namespace Domain.Entities;

public class KillerAddon : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Rarity Rarity { get; private set; }
    public Guid KillerId { get; private set; }
    public Killer Killer { get; private set; } = null!;

    private KillerAddon() { }

    public KillerAddon(string name, Rarity rarity, Guid killerId, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Killer addon");
        ValidateDescription(description, "Killer addon");

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Rarity = rarity;
        KillerId = killerId;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null, Rarity? rarity = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Killer addon");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Killer addon");
            Description = description;
        }

        if (rarity.HasValue)
            Rarity = rarity.Value;

        MarkAsUpdated();
    }
}
