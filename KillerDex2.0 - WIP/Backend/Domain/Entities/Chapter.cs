namespace Domain.Entities;

public class Chapter : BaseEntity
{
    public string Name { get; private set; } = null!;
    public int Number { get; private set; }
    public DateOnly? ReleaseDate { get; private set; }

    private readonly List<Killer> _killers = [];
    public IReadOnlyList<Killer> Killers => _killers.AsReadOnly();

    private readonly List<Survivor> _survivors = [];
    public IReadOnlyList<Survivor> Survivors => _survivors.AsReadOnly();

    private Chapter() { }

    public Chapter(string name, int number, DateOnly? releaseDate = null, string? gameVersion = null)
    {
        ValidateName(name, "Chapter");

        if (number < 0)
            throw new ArgumentException("Chapter number cannot be negative.", nameof(number));

        Name = name;
        Slug = GenerateSlug(name);
        Number = number;
        ReleaseDate = releaseDate;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, int? number = null, DateOnly? releaseDate = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Chapter");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (number.HasValue)
        {
            if (number.Value < 0)
                throw new ArgumentException("Chapter number cannot be negative.", nameof(number));
            Number = number.Value;
        }

        if (releaseDate.HasValue)
            ReleaseDate = releaseDate;

        MarkAsUpdated();
    }
}
