namespace Domain.Entities;

/// <summary>
/// Represents a realm in Dead by Daylight.
/// A realm can optionally be associated with a specific killer (e.g., The MacMillan Estate for The Trapper).
/// Realms without a killer association are shared/generic realms.
/// </summary>
public class Realm : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid? KillerId { get; private set; }
    public Killer? Killer { get; private set; }

    private readonly List<Map> _maps = [];
    public IReadOnlyList<Map> Maps => _maps.AsReadOnly();

    private Realm() { }

    public Realm(string name, string? description = null, Guid? killerId = null, string? gameVersion = null)
    {
        ValidateName(name, "Realm");
        ValidateDescription(description, "Realm");

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        KillerId = killerId;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Realm");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Realm");
            Description = description;
        }

        MarkAsUpdated();
    }

    public void AssignToKiller(Guid killerId)
    {
        KillerId = killerId;
        MarkAsUpdated();
    }

    public void RemoveKillerAssociation()
    {
        KillerId = null;
        MarkAsUpdated();
    }
}
