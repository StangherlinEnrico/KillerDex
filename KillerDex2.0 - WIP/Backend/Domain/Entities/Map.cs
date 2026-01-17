namespace Domain.Entities;

public class Map : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid RealmId { get; private set; }
    public Realm Realm { get; private set; } = null!;

    private Map() { }

    public Map(string name, Guid realmId, string? description = null, string? gameVersion = null)
    {
        ValidateName(name, "Map");
        ValidateDescription(description, "Map");

        Name = name;
        Slug = GenerateSlug(name);
        RealmId = realmId;
        Description = description;
        GameVersion = gameVersion;
    }

    public void Update(string? name = null, string? description = null)
    {
        if (name is not null)
        {
            ValidateName(name, "Map");
            Name = name;
            Slug = GenerateSlug(name);
        }

        if (description is not null)
        {
            ValidateDescription(description, "Map");
            Description = description;
        }

        MarkAsUpdated();
    }
}
