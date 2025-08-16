// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

namespace AISStream.Interfaces;

public interface IHasPosition
{
    int UserId { get; }

    /// <summary>
    /// Determines whether the position report refers to the first party (i.e. a ship sending out its own location).
    /// </summary>
    bool IsFirstParty { get; }

    /// <summary>
    /// If <c>true</c>, the reported position is within 10 meters of the actual position.
    /// </summary>
    bool PositionHighAccuracy { get; }

    double Latitude { get; }
    double Longitude { get; }

    /// <summary>
    /// If <c>true</c>, the RAIM device is currently in use.
    /// </summary>
    bool RAIM { get; }
}