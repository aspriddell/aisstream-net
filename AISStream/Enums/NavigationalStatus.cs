// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

namespace AISStream.Enums;

public enum NavigationalStatus
{
    MovingUsingEngine = 0,
    AtAnchor = 1,
    NotUnderCommand = 2,
    RestrictedManeuverability = 3,
    ConstrainedByDraught = 4,
    Moored = 5,
    Aground = 6,
    EngagedInFishing = 7,
    UnderwaySailing = 8,

    PoweredVesselTowingAstern = 11,
    PoweredVesselPushingAhead = 12,

    AISSART = 14,
    Undefined = 15
}