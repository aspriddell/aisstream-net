// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Messages.Shared;

namespace AISStream.Messages;

public class ExtendedClassBPositionReport : PositionReportBase
{
    [JsonPropertyName("Spare1")]
    public int Spare1 { get; set; }

    [JsonPropertyName("Spare2")]
    public int Spare2 { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Type")]
    public int ShipAndCargoType { get; set; }

    [JsonPropertyName("Dimension")]
    public ShipDimensions Dimension { get; set; } = null!;

    [JsonPropertyName("FixType")]
    public PositionFixingDeviceType PositionFixingType { get; set; }

    [JsonPropertyName("Dte")]
    public bool DTEReady { get; set; }

    [JsonPropertyName("AssignedMode")]
    public bool AssignedMode { get; set; }

    [JsonPropertyName("Spare3")]
    public int Spare3 { get; set; }
}