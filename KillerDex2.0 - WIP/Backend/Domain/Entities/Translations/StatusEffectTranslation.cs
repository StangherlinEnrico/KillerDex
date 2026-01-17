using Domain.Enums;

namespace Domain.Entities.Translations;

public class StatusEffectTranslation : BaseTranslation
{
    public Guid StatusEffectId { get; private set; }
    public StatusEffect StatusEffect { get; private set; } = null!;

    private StatusEffectTranslation() { }

    public StatusEffectTranslation(Guid statusEffectId, Language language, string? name = null, string? description = null)
    {
        StatusEffectId = statusEffectId;
        Language = language;
        Name = name;
        Description = description;
    }
}
