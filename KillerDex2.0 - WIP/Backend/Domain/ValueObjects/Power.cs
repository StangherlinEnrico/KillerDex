namespace Domain.ValueObjects;

/// <summary>
/// Represents a killer's unique power in Dead by Daylight.
/// Powers are immutable value objects that define what makes each killer unique.
/// </summary>
public sealed class Power : IEquatable<Power>
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 2000;

    public string Name { get; }
    public string Description { get; }

    public Power(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Power name cannot be empty.", nameof(name));

        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Power name cannot exceed {MaxNameLength} characters.", nameof(name));

        if (description is not null && description.Length > MaxDescriptionLength)
            throw new ArgumentException($"Power description cannot exceed {MaxDescriptionLength} characters.", nameof(description));

        Name = name;
        Description = description ?? string.Empty;
    }

    public bool Equals(Power? other)
    {
        if (other is null) return false;
        return Name == other.Name && Description == other.Description;
    }

    public override bool Equals(object? obj) => obj is Power other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Name, Description);

    public override string ToString() => Name;

    public static bool operator ==(Power? left, Power? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Power? left, Power? right) => !(left == right);
}
