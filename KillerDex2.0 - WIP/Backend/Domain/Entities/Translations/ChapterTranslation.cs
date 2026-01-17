using Domain.Enums;

namespace Domain.Entities.Translations;

public class ChapterTranslation : BaseTranslation
{
    public Guid ChapterId { get; private set; }
    public Chapter Chapter { get; private set; } = null!;

    private ChapterTranslation() { }

    public ChapterTranslation(Guid chapterId, Language language, string? name = null)
    {
        ChapterId = chapterId;
        Language = language;
        Name = name;
    }
}
