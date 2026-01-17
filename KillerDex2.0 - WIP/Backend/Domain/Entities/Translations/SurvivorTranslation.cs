using Domain.Enums;

namespace Domain.Entities.Translations;

public class SurvivorTranslation : BaseTranslation
{
    public const int MaxOverviewLength = 2000;
    public const int MaxBackstoryLength = 8000;

    public Guid SurvivorId { get; private set; }
    public Survivor Survivor { get; private set; } = null!;
    public string? Overview { get; private set; }
    public string? Backstory { get; private set; }

    private SurvivorTranslation() { }

    public SurvivorTranslation(
        Guid survivorId,
        Language language,
        string? name = null,
        string? overview = null,
        string? backstory = null)
    {
        ValidateName(name);
        ValidateOverview(overview);
        ValidateBackstory(backstory);

        SurvivorId = survivorId;
        Language = language;
        Name = name;
        Overview = overview;
        Backstory = backstory;
    }

    public void Update(
        string? name = null,
        string? overview = null,
        string? backstory = null)
    {
        if (name is not null)
        {
            ValidateName(name);
            Name = name;
        }

        if (overview is not null)
        {
            ValidateOverview(overview);
            Overview = overview;
        }

        if (backstory is not null)
        {
            ValidateBackstory(backstory);
            Backstory = backstory;
        }

        MarkAsUpdated();
    }

    private static void ValidateOverview(string? overview)
    {
        if (overview is not null && overview.Length > MaxOverviewLength)
            throw new ArgumentException($"Overview cannot exceed {MaxOverviewLength} characters.", nameof(overview));
    }

    private static void ValidateBackstory(string? backstory)
    {
        if (backstory is not null && backstory.Length > MaxBackstoryLength)
            throw new ArgumentException($"Backstory cannot exceed {MaxBackstoryLength} characters.", nameof(backstory));
    }
}
