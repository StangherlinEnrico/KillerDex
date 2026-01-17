using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a status effect in Dead by Daylight.
/// Status effects can apply to Killers, Survivors, or All (both roles).
/// Unlike Perks, status effects can legitimately apply to all roles.
/// </summary>
public class StatusEffect : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public StatusEffectType Type { get; private set; }
    public Role AppliesTo { get; private set; }

    private StatusEffect() { }

    public StatusEffect(string name, StatusEffectType type, Role appliesTo, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Status effect");
        ValidateDescription(description, "Status effect");

        Name = name;
        Slug = GenerateSlug(name);
        Type = type;
        AppliesTo = appliesTo;
        Description = description;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null, StatusEffectType? type = null, Role? appliesTo = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Status effect");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Status effect");
            Description = description;
        }

        if (type.HasValue)
            Type = type.Value;

        if (appliesTo.HasValue)
            AppliesTo = appliesTo.Value;

        MarkAsUpdated();
    }
}
