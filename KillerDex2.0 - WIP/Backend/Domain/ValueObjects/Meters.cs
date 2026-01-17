namespace Domain.ValueObjects;

/// <summary>
/// Represents a measurement in meters, commonly used for terror radius and other distances.
/// </summary>
public sealed class Meters : IEquatable<Meters>, IComparable<Meters>
{
    public int Value { get; }

    public Meters(int value)
    {
        if (value < 0)
            throw new ArgumentException("Meters cannot be negative.", nameof(value));

        Value = value;
    }

    public static Meters Zero => new(0);

    public static Meters FromInt(int value) => new(value);

    public bool Equals(Meters? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => obj is Meters other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(Meters? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    public override string ToString() => $"{Value}m";

    public static bool operator ==(Meters? left, Meters? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Meters? left, Meters? right) => !(left == right);

    public static bool operator <(Meters left, Meters right) => left.CompareTo(right) < 0;
    public static bool operator >(Meters left, Meters right) => left.CompareTo(right) > 0;
    public static bool operator <=(Meters left, Meters right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Meters left, Meters right) => left.CompareTo(right) >= 0;

    public static implicit operator int(Meters meters) => meters.Value;
}
