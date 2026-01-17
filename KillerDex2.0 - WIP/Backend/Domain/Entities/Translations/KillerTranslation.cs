using Domain.Enums;

namespace Domain.Entities.Translations;

public class KillerTranslation : BaseTranslation
{
    public const int MaxOverviewLength = 2000;
    public const int MaxBackstoryLength = 8000;

    public Guid KillerId { get; private set; }
    public Killer Killer { get; private set; } = null!;
    public string? Overview { get; private set; }
    public string? Backstory { get; private set; }
    public string? PowerName { get; private set; }
    public string? PowerDescription { get; private set; }

    private KillerTranslation() { }

    public KillerTranslation(
        Guid killerId,
        Language language,
        string? name = null,
        string? overview = null,
        string? backstory = null,
        string? powerName = null,
        string? powerDescription = null)
    {
        ValidateName(name);
        ValidateOverview(overview);
        ValidateBackstory(backstory);

        KillerId = killerId;
        Language = language;
        Name = name;
        Overview = overview;
        Backstory = backstory;
        PowerName = powerName;
        PowerDescription = powerDescription;
    }

    public void Update(
        string? name = null,
        string? overview = null,
        string? backstory = null,
        string? powerName = null,
        string? powerDescription = null)
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

        if (powerName is not null) PowerName = powerName;
        if (powerDescription is not null) PowerDescription = powerDescription;

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
