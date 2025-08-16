// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Interfaces;

namespace AISStream.Messages;

public class AidsToNavigationReport : AISMessage, IHasPosition
{
    [JsonPropertyName("Type")]
    public NavigationalAidsType AidType { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("PositionAccuracy")]
    public bool PositionHighAccuracy { get; set; }

    [JsonPropertyName("Latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("Longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("Dimension")]
    public AidsToNavigationReportDimension Dimensions { get; set; } = null!;

    [JsonPropertyName("Fixtype")]
    public PositionFixingDeviceType PositionFixingType { get; set; }

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

    [JsonPropertyName("OffPosition")]
    public bool OffPosition { get; set; }

    [JsonPropertyName("AtoN")]
    public int AtoN { get; set; }

    [JsonPropertyName("Raim")]
    public bool RAIM { get; set; }

    [JsonPropertyName("VirtualAtoN")]
    public bool VirtualAtoN { get; set; }

    [JsonPropertyName("AssignedMode")]
    public bool AssignedMode { get; set; }

    [JsonPropertyName("Spare")]
    public bool Spare { get; set; }

    [JsonPropertyName("NameExtension")]
    public string NameExtension { get; set; } = null!;

    bool IHasPosition.IsFirstParty => false;
}

public class AidsToNavigationReportDimension
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public int D { get; set; }
}
