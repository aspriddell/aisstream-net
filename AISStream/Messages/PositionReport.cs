// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;

namespace AISStream.Messages;

public class PositionReport : PositionReportBase
{
    [JsonPropertyName("NavigationalStatus")]
    public NavigationalStatus NavigationalStatus { get; set; }

    [JsonPropertyName("RateOfTurn")]
    public int RateOfTurn { get; set; }

    [JsonPropertyName("SpecialManoeuvreIndicator")]
    public SpecialManoeuvreIndicator SpecialManoeuvreIndicator { get; set; }
}

public class AssignedScheduledPositionReport : PositionReport;

/// <summary>
/// Special position report, as a response to an interrogation request.
/// </summary>
public class SpecialPositionReport : PositionReport;
