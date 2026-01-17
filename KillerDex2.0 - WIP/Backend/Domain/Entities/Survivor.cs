using Domain.Events;

namespace Domain.Entities;

public class Survivor : BaseEntity
{
    public const int MaxOverviewLength = 2000;
    public const int MaxBackstoryLength = 8000;

    public string Name { get; private set; } = null!;
    public string? Overview { get; private set; }
    public string? Backstory { get; private set; }
    public Guid? ChapterId { get; private set; }
    public Chapter? Chapter { get; private set; }

    private readonly List<Perk> _perks = [];
    public IReadOnlyList<Perk> Perks => _perks.AsReadOnly();

    private Survivor() { }

    public Survivor(
        string name,
        string? overview = null,
        string? backstory = null,
        Guid? chapterId = null,
        string? gameVersion = null)
    {
        ValidateName(name, "Survivor");
        ValidateOverview(overview);
        ValidateBackstory(backstory);

        Name = name;
        Slug = GenerateSlug(name);
        Overview = overview;
        Backstory = backstory;
        ChapterId = chapterId;
        GameVersion = gameVersion;

        AddDomainEvent(new SurvivorCreatedEvent(Id, name));
    }

    public void AssignToChapter(Guid chapterId)
    {
        ChapterId = chapterId;
        MarkAsUpdated();
        AddDomainEvent(new SurvivorAssignedToChapterEvent(Id, chapterId));
    }

    public void Update(
        string? name = null,
        string? overview = null,
        string? backstory = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Survivor");
            Name = name;
            Slug = GenerateSlug(name);
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
        AddDomainEvent(new SurvivorUpdatedEvent(Id));
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
