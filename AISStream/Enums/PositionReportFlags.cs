// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

namespace AISStream.Enums;

public enum PositionReportFlags
{
    None,
    HasValidTimestamp,
    TimestampNotAvailable,
    PositioningSystemInManualMode,
    PositioningSystemEstimated,
    PositioningSystemInoperative,
}