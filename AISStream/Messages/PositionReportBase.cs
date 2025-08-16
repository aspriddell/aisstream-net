// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Interfaces;

namespace AISStream.Messages;

public class PositionReportBase : AISMessage, IHasPosition
{
    /// <summary>
    /// Speed over ground in knots (0-102.2 knots).
    /// If set to 102.3, the speed is not available.
    /// </summary>
    [JsonPropertyName("Sog")]
    public double SpeedOverGround { get; set; }

    [JsonPropertyName("Longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("Latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// If <c>true</c>, reported position is within 10 meters of the actual position.
    /// </summary>
    [JsonPropertyName("PositionAccuracy")]
    public bool PositionHighAccuracy { get; set; }

    [JsonPropertyName("Cog")]
    public double CourseOverGround { get; set; }

    /// <summary>
    /// Degrees (0-359) relative to true north. If set to 511, the true heading is not available.
    /// </summary>
    [JsonPropertyName("TrueHeading")]
    public int TrueHeading { get; set; }

    /// <summary>
    /// Seconds part of the UTC timestamp for when the report was generated.
    /// This value should only be used if <see cref="PositionReport.ReportFlags"/> returns <see cref="PositionReportFlags.HasValidTimestamp"/>
    /// </summary>
    [JsonPropertyName("Timestamp")]
    public int Timestamp { get; set; }

    /// <summary>
    /// Additional flags reported as part of the <see cref="Timestamp"/>
    /// </summary>
    [JsonIgnore]
    public PositionReportFlags ReportFlags => Timestamp switch
    {
        >= 0 and <= 59 => PositionReportFlags.HasValidTimestamp,

        60 => PositionReportFlags.TimestampNotAvailable,
        61 => PositionReportFlags.PositioningSystemInManualMode,
        62 => PositionReportFlags.PositioningSystemEstimated,
        63 => PositionReportFlags.PositioningSystemInoperative,

        _ => PositionReportFlags.None
    };

    [JsonPropertyName("Raim")]
    public bool RAIM { get; set; }

    [JsonPropertyName("CommunicationState")]
    public int CommunicationState { get; set; }

    bool IHasPosition.IsFirstParty => true;
}