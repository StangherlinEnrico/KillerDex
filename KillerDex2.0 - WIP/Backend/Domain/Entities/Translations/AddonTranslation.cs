using Domain.Enums;

namespace Domain.Entities.Translations;

public class KillerAddonTranslation : BaseTranslation
{
    public Guid KillerAddonId { get; private set; }
    public KillerAddon KillerAddon { get; private set; } = null!;

    private KillerAddonTranslation() { }

    public KillerAddonTranslation(Guid killerAddonId, Language language, string? name = null, string? description = null)
    {
        KillerAddonId = killerAddonId;
        Language = language;
        Name = name;
        Description = description;
    }
}

public class SurvivorAddonTranslation : BaseTranslation
{
    public Guid SurvivorAddonId { get; private set; }
    public SurvivorAddon SurvivorAddon { get; private set; } = null!;

    private SurvivorAddonTranslation() { }

    public SurvivorAddonTranslation(Guid survivorAddonId, Language language, string? name = null, string? description = null)
    {
        SurvivorAddonId = survivorAddonId;
        Language = language;
        Name = name;
        Description = description;
    }
}
