using Domain.Enums;

namespace Domain.Entities.Translations;

public abstract class BaseTranslation
{
    public const int MaxNameLength = 200;
    public const int MaxDescriptionLength = 4000;

    public Guid Id { get; protected set; }
    public Language Language { get; protected set; }
    public string? Name { get; protected set; }
    public string? Description { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected BaseTranslation()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public virtual void Update(string? name = null, string? description = null)
    {
        if (name is not null)
        {
            ValidateName(name);
            Name = name;
        }

        if (description is not null)
        {
            ValidateDescription(description);
            Description = description;
        }

        MarkAsUpdated();
    }

    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    protected static void ValidateName(string? name)
    {
        if (name is not null && name.Length > MaxNameLength)
            throw new ArgumentException($"Name cannot exceed {MaxNameLength} characters.", nameof(name));
    }

    protected static void ValidateDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters.", nameof(description));
    }
}
