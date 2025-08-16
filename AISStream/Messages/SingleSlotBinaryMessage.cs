// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class SingleSlotBinaryMessage : AISMessage
{
    [JsonPropertyName("ApplicationID")]
    public ApplicationId ApplicationId { get; set; } = null!;

    [JsonPropertyName("DestinationID")]
    public int DestinationId { get; set; }

    [JsonPropertyName("ApplicationIDValid")]
    public bool ApplicationIdValid { get; set; }

    [JsonPropertyName("DestinationIDValid")]
    public bool DestinationIdValid { get; set; }

    [JsonPropertyName("Payload")]
    public string Payload { get; set; } = null!;
}