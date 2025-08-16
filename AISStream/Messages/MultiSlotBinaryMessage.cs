// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class MultiSlotBinaryMessage : SingleSlotBinaryMessage
{
    [JsonPropertyName("CommunicationState")]
    public int CommunicationState { get; set; }

    [JsonPropertyName("CommunicationStateIsItdma")]
    public bool CommunicationStateIsItdma { get; set; }

    [JsonPropertyName("Spare1")]
    public int Spare1 { get; set; }

    [JsonPropertyName("Spare2")]
    public int Spare2 { get; set; }
}