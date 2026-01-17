using Domain.Enums;

namespace Domain.Entities.Translations;

public class ItemTranslation : BaseTranslation
{
    public Guid ItemId { get; private set; }
    public Item Item { get; private set; } = null!;

    private ItemTranslation() { }

    public ItemTranslation(Guid itemId, Language language, string? name = null, string? description = null)
    {
        ItemId = itemId;
        Language = language;
        Name = name;
        Description = description;
    }
}
