using Domain.Enums;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Killer : BaseEntity
{
    public const int MaxOverviewLength = 2000;
    public const int MaxBackstoryLength = 8000;

    public string Name { get; private set; } = null!;
    public string? RealName { get; private set; }
    public string? Overview { get; private set; }
    public string? Backstory { get; private set; }
    public Power Power { get; private set; } = null!;
    public decimal MovementSpeed { get; private set; }
    public int TerrorRadius { get; private set; }
    public KillerHeight Height { get; private set; }
    public Guid? ChapterId { get; private set; }
    public Chapter? Chapter { get; private set; }

    private readonly List<Perk> _perks = [];
    public IReadOnlyList<Perk> Perks => _perks.AsReadOnly();

    private readonly List<KillerAddon> _addons = [];
    public IReadOnlyList<KillerAddon> Addons => _addons.AsReadOnly();

    private Killer() { }

    public Killer(
        string name,
        Power power,
        decimal movementSpeed,
        int terrorRadius,
        KillerHeight height,
        string? realName = null,
        string? overview = null,
        string? backstory = null,
        Guid? chapterId = null,
        string? gameVersion = null)
    {
        ValidateName(name, "Killer");
        ValidateOverview(overview);
        ValidateBackstory(backstory);

        if (movementSpeed <= 0)
            throw new ArgumentException("Movement speed must be positive.", nameof(movementSpeed));

        if (terrorRadius < 0)
            throw new ArgumentException("Terror radius cannot be negative.", nameof(terrorRadius));

        Name = name;
        Slug = GenerateSlug(name);
        RealName = realName;
        Overview = overview;
        Backstory = backstory;
        Power = power ?? throw new ArgumentNullException(nameof(power));
        MovementSpeed = movementSpeed;
        TerrorRadius = terrorRadius;
        Height = height;
        ChapterId = chapterId;
        GameVersion = gameVersion;

        AddDomainEvent(new KillerCreatedEvent(Id, name));
    }

    public void AssignToChapter(Guid chapterId)
    {
        ChapterId = chapterId;
        MarkAsUpdated();
        AddDomainEvent(new KillerAssignedToChapterEvent(Id, chapterId));
    }

    public void Update(
        string? name = null,
        string? realName = null,
        string? overview = null,
        string? backstory = null,
        Power? power = null,
        decimal? movementSpeed = null,
        int? terrorRadius = null,
        KillerHeight? height = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Killer");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (realName is not null) RealName = realName;

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

        if (power is not null) Power = power;

        if (movementSpeed.HasValue)
        {
            if (movementSpeed.Value <= 0)
                throw new ArgumentException("Movement speed must be positive.", nameof(movementSpeed));
            MovementSpeed = movementSpeed.Value;
        }

        if (terrorRadius.HasValue)
        {
            if (terrorRadius.Value < 0)
                throw new ArgumentException("Terror radius cannot be negative.", nameof(terrorRadius));
            TerrorRadius = terrorRadius.Value;
        }

        if (height.HasValue) Height = height.Value;

        MarkAsUpdated();
        AddDomainEvent(new KillerUpdatedEvent(Id));
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
