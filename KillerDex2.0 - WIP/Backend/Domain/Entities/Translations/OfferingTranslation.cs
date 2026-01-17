using Domain.Enums;

namespace Domain.Entities.Translations;

public class OfferingTranslation : BaseTranslation
{
    public Guid OfferingId { get; private set; }
    public Offering Offering { get; private set; } = null!;

    private OfferingTranslation() { }

    public OfferingTranslation(Guid offeringId, Language language, string? name = null, string? description = null)
    {
        OfferingId = offeringId;
        Language = language;
        Name = name;
        Description = description;
    }
}
