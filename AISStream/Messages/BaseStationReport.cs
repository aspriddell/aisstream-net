// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Interfaces;

namespace AISStream.Messages;

public class BaseStationReport : AISMessage, IHasPosition
{
    [JsonPropertyName("UtcYear")]
    public int UtcYear { get; set; }

    [JsonPropertyName("UtcMonth")]
    public int UtcMonth { get; set; }

    [JsonPropertyName("UtcDay")]
    public int UtcDay { get; set; }

    [JsonPropertyName("UtcHour")]
    public int UtcHour { get; set; }

    [JsonPropertyName("UtcMinute")]
    public int UtcMinute { get; set; }

    [JsonPropertyName("UtcSecond")]
    public int UtcSecond { get; set; }

    [JsonIgnore]
    public DateTime UtcDateTime => new(UtcYear, UtcMonth, UtcDay, UtcHour, UtcMinute, UtcSecond, DateTimeKind.Utc);

    [JsonPropertyName("PositionAccuracy")]
    public bool PositionHighAccuracy { get; set; }

    [JsonPropertyName("Latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("Longitude")]
    public double Longitude { get; set; }

    // todo check values are correct
    [JsonPropertyName("FixType")]
    public PositionFixingDeviceType PositionFixingType { get; set; }

    [JsonPropertyName("LongRangeEnable")]
    public bool SupportsLongRange { get; set; }

    [JsonPropertyName("Spare")]
    public int Spare { get; set; }

    [JsonPropertyName("Raim")]
    public bool RAIM { get; set; }

    // todo enum values
    [JsonPropertyName("CommunicationState")]
    public int CommunicationState { get; set; }

    bool IHasPosition.IsFirstParty => false;
}