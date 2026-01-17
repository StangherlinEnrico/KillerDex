using Domain.Enums;

namespace Domain.Entities.Translations;

public class PerkTranslation : BaseTranslation
{
    public Guid PerkId { get; private set; }
    public Perk Perk { get; private set; } = null!;

    private PerkTranslation() { }

    public PerkTranslation(Guid perkId, Language language, string? name = null, string? description = null)
    {
        PerkId = perkId;
        Language = language;
        Name = name;
        Description = description;
    }
}
