namespace Domain.ValueObjects;

/// <summary>
/// Represents a percentage value, commonly used for movement speed (e.g., 115% = 4.6 m/s base speed).
/// </summary>
public sealed class Percentage : IEquatable<Percentage>, IComparable<Percentage>
{
    public decimal Value { get; }

    public Percentage(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Percentage cannot be negative.", nameof(value));

        Value = value;
    }

    public static Percentage FromDecimal(decimal value) => new(value);
    public static Percentage FromInt(int value) => new(value);

    /// <summary>
    /// Creates a percentage from a movement speed value (e.g., 4.6 m/s = 115%).
    /// Base survivor speed is 4.0 m/s = 100%.
    /// </summary>
    public static Percentage FromMovementSpeed(decimal metersPerSecond)
    {
        const decimal baseSurvivorSpeed = 4.0m;
        return new Percentage(metersPerSecond / baseSurvivorSpeed * 100);
    }

    /// <summary>
    /// Converts the percentage to movement speed in meters per second.
    /// </summary>
    public decimal ToMovementSpeed()
    {
        const decimal baseSurvivorSpeed = 4.0m;
        return Value / 100 * baseSurvivorSpeed;
    }

    public bool Equals(Percentage? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => obj is Percentage other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(Percentage? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    public override string ToString() => $"{Value}%";

    public static bool operator ==(Percentage? left, Percentage? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Percentage? left, Percentage? right) => !(left == right);

    public static bool operator <(Percentage left, Percentage right) => left.CompareTo(right) < 0;
    public static bool operator >(Percentage left, Percentage right) => left.CompareTo(right) > 0;
    public static bool operator <=(Percentage left, Percentage right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Percentage left, Percentage right) => left.CompareTo(right) >= 0;

    public static implicit operator decimal(Percentage percentage) => percentage.Value;
}
