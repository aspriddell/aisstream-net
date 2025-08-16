// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class ChannelManagement : AISMessage
{
    [JsonPropertyName("Spare1")]
    public int Spare1 { get; set; }

    [JsonPropertyName("ChannelA")]
    public int ChannelA { get; set; }

    [JsonPropertyName("ChannelB")]
    public int ChannelB { get; set; }

    [JsonPropertyName("TxRxMode")]
    public int TxRxMode { get; set; }

    [JsonPropertyName("LowPower")]
    public bool LowPower { get; set; }

    [JsonPropertyName("Area")]
    public ChannelManagementArea Area { get; set; } = null!;

    [JsonPropertyName("Unicast")]
    public ChannelManagementUnicast Unicast { get; set; } = null!;

    [JsonPropertyName("Addressed")]
    public bool IsAddressed { get; set; }

    [JsonPropertyName("BwA")]
    public bool BwA { get; set; }

    [JsonPropertyName("BwB")]
    public bool BwB { get; set; }

    [JsonPropertyName("TransitionalZoneSize")]
    public int TransitionalZoneSize { get; set; }

    [JsonPropertyName("Spare4")]
    public int Spare4 { get; set; }
}

public class ChannelManagementUnicast
{
    [JsonPropertyName("AddressStation1")]
    public int AddressStation1 { get; set; }

    [JsonPropertyName("AddressStation2")]
    public int AddressStation2 { get; set; }

    [JsonPropertyName("Spare2")]
    public int Spare2 { get; set; }

    [JsonPropertyName("Spare3")]
    public int Spare3 { get; set; }
}

public record ChannelManagementArea(
    [property: JsonPropertyName("Latitude1")] double Latitude1,
    [property: JsonPropertyName("Latitude2")] double Latitude2,
    [property: JsonPropertyName("Longitude1")] double Longitude1,
    [property: JsonPropertyName("Longitude2")] double Longitude2
);