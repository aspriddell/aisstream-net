// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

namespace AISStream.Coordinates;

/// <summary>
/// Represents a geographic coordinate point (corner of a bounding box)
/// </summary>
public readonly struct BoundingBoxCorner : IEquatable<BoundingBoxCorner>
{
    /// <summary>
    /// Represents a geographic coordinate point (corner of a bounding box)
    /// </summary>
    public BoundingBoxCorner(double latitude, double longitude)
    {
        if (latitude is < -90.0 or > 90.0)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees");

        if (longitude is < -180.0 or > 180.0)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees");

        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }

    public bool Equals(BoundingBoxCorner other)
    {
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    public override bool Equals(object? obj)
    {
        return obj is BoundingBoxCorner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }

    public static bool operator ==(BoundingBoxCorner left, BoundingBoxCorner right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BoundingBoxCorner left, BoundingBoxCorner right)
    {
        return !left.Equals(right);
    }

    public static explicit operator double[](BoundingBoxCorner corner)
    {
        return [corner.Latitude, corner.Longitude];
    }
}