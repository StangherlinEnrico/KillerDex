using System.Text.RegularExpressions;
using Domain.Events;

namespace Domain.Entities;

public abstract class BaseEntity
{
    public const int MaxNameLength = 200;
    public const int MaxDescriptionLength = 4000;
    public const int MaxSlugLength = 250;

    public Guid Id { get; protected set; }
    public string Slug { get; protected set; } = null!;
    public string? ImageUrl { get; protected set; }
    public string? GameVersion { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void SetSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty.", nameof(slug));

        if (slug.Length > MaxSlugLength)
            throw new ArgumentException($"Slug cannot exceed {MaxSlugLength} characters.", nameof(slug));

        Slug = slug.ToLowerInvariant().Trim();
    }

    public void SetImageUrl(string? imageUrl)
    {
        if (imageUrl is not null && !Uri.TryCreate(imageUrl, UriKind.RelativeOrAbsolute, out _))
            throw new ArgumentException("Invalid URL format.", nameof(imageUrl));

        ImageUrl = imageUrl;
    }

    public void SetGameVersion(string? gameVersion)
    {
        GameVersion = gameVersion;
    }

    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected static string GenerateSlug(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty for slug generation.", nameof(name));

        var slug = name
            .ToLowerInvariant()
            .Trim();

        // Replace spaces and special characters
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        slug = slug.Trim('-');

        if (string.IsNullOrEmpty(slug))
            throw new ArgumentException("Name must contain at least one alphanumeric character.", nameof(name));

        if (slug.Length > MaxSlugLength)
            slug = slug[..MaxSlugLength].TrimEnd('-');

        return slug;
    }

    protected static void ValidateName(string name, string entityName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{entityName} name cannot be empty.", nameof(name));

        if (name.Length > MaxNameLength)
            throw new ArgumentException($"{entityName} name cannot exceed {MaxNameLength} characters.", nameof(name));
    }

    protected static void ValidateDescription(string? description, string entityName)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
            throw new ArgumentException($"{entityName} description cannot exceed {MaxDescriptionLength} characters.", nameof(description));
    }
}
