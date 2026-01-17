using Domain.Enums;

namespace Domain.Entities.Translations;

public class RealmTranslation : BaseTranslation
{
    public Guid RealmId { get; private set; }
    public Realm Realm { get; private set; } = null!;

    private RealmTranslation() { }

    public RealmTranslation(Guid realmId, Language language, string? name = null, string? description = null)
    {
        RealmId = realmId;
        Language = language;
        Name = name;
        Description = description;
    }
}

public class MapTranslation : BaseTranslation
{
    public Guid MapId { get; private set; }
    public Map Map { get; private set; } = null!;

    private MapTranslation() { }

    public MapTranslation(Guid mapId, Language language, string? name = null, string? description = null)
    {
        MapId = mapId;
        Language = language;
        Name = name;
        Description = description;
    }
}
