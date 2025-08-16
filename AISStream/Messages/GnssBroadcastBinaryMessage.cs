// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Interfaces;

namespace AISStream.Messages;

public class GnssBroadcastBinaryMessage : AISMessage, IHasPosition
{
    [JsonPropertyName("Spare1")]
    public int Spare1 { get; set; }

    [JsonPropertyName("Latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("Longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("Spare2")]
    public int Spare2 { get; set; }

    [JsonPropertyName("Data")]
    public string Data { get; set; } = null!;

    bool IHasPosition.IsFirstParty => true;
    bool IHasPosition.PositionHighAccuracy => false;
    bool IHasPosition.RAIM => false;
}