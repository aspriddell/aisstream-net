// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class AddressedBinaryMessage : AISMessage
{
    [JsonPropertyName("Sequenceinteger")]
    public int SequenceNumber { get; set; }

    [JsonPropertyName("DestinationId")]
    public int DestinationId { get; set; }

    [JsonPropertyName("Retransmission ")]
    public bool IsRetransmission { get; set; }

    /// <summary>
    /// Reserved for future use, should always be set to <c>false</c>
    /// </summary>
    [JsonPropertyName("Spare")]
    public bool Spare { get; set; }

    [JsonPropertyName("ApplicationID")]
    public ApplicationId ApplicationId { get; set; } = null!;

    [JsonPropertyName("BinaryData")]
    public string BinaryData { get; set; } = null!;
}