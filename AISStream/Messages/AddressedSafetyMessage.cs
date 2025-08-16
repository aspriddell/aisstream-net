// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class AddressedSafetyMessage : AISMessage
{
    [JsonPropertyName("Spare")]
    public bool Spare { get; set; }

    [JsonPropertyName("DestinationID")]
    public int DestinationId { get; set; }

    [JsonPropertyName("Sequenceinteger")]
    public int SequenceInteger { get; set; }

    [JsonPropertyName("Retransmission")]
    public bool Retransmission { get; set; }

    [JsonPropertyName("Text")]
    public string Text { get; set; } = null!;
}