// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Interfaces;

namespace AISStream.Messages;

public class LongRangeAisBroadcastMessage : AISMessage, IHasPosition
{
    [JsonPropertyName("PositionAccuracy")]
    public bool PositionHighAccuracy { get; set; }

    [JsonPropertyName("Raim")]
    public bool RAIM { get; set; }

    [JsonPropertyName("NavigationalStatus")]
    public NavigationalStatus NavigationalStatus { get; set; }

    [JsonPropertyName("Latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("Longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("Sog")]
    public double SpeedOverGround { get; set; }

    [JsonPropertyName("Cog")]
    public double CourseOverGround { get; set; }

    /// <summary>
    /// If true, the reported position latency is >= 5 seconds.
    /// </summary>
    [JsonPropertyName("PositionLatency")]
    public bool PositionLatency { get; set; }

    bool IHasPosition.IsFirstParty => true;
}