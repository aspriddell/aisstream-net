// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Collections.Immutable;

namespace AISStream.Coordinates;

/// <summary>
/// Represents a geographic bounding box defined by corners
/// </summary>
public record BoundingBox
{
    /// <summary>
    /// Represents the entire world as a bounding box
    /// </summary>
    public static readonly BoundingBox World = new(new BoundingBoxCorner(-90.0, -180.0), new BoundingBoxCorner(90.0, 180.0));

    private readonly ImmutableArray<BoundingBoxCorner> _corners;

    public BoundingBox(params BoundingBoxCorner[] corners)
    {
        if (corners.Length is not 2 and not 4)
            throw new ArgumentException("Bounding box must have exactly 2 or 4 corners", nameof(corners));

        // copy to own array to ensure immutability
        _corners = [..corners];
    }

    /// <summary>
    /// The corners of the bounding box, in no particular order
    /// </summary>
    public IReadOnlyList<BoundingBoxCorner> Corners => _corners;

    /// <summary>
    /// Converts the bounding box to a jagged array (used in serialization)
    /// </summary>
    internal double[][] ToArray()
    {
        return _corners.Select(x => (double[])x).ToArray();
    }

    /// <summary>
    /// Creates a rectangular bounding box from coordinate bounds
    /// </summary>
    public static BoundingBox FromBounds(double minLatitude, double minLongitude, double maxLatitude, double maxLongitude)
    {
        return new BoundingBox(new BoundingBoxCorner(minLatitude, minLongitude), new BoundingBoxCorner(maxLatitude, maxLongitude));
    }
}