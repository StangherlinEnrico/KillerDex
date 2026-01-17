using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents an offering in Dead by Daylight.
/// Offerings can be used by Killers, Survivors, or All (both roles).
/// Unlike Perks, offerings can legitimately be available to all roles.
/// </summary>
public class Offering : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Rarity Rarity { get; private set; }
    public Role Role { get; private set; }

    private Offering() { }

    public Offering(string name, Rarity rarity, Role role, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Offering");
        ValidateDescription(description, "Offering");

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Rarity = rarity;
        Role = role;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null, Rarity? rarity = null, Role? role = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Offering");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Offering");
            Description = description;
        }

        if (rarity.HasValue)
            Rarity = rarity.Value;

        if (role.HasValue)
            Role = role.Value;

        MarkAsUpdated();
    }
}
