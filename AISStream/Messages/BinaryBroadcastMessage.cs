// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class BinaryBroadcastMessage : AISMessage
{
    [JsonPropertyName("Spare")]
    public int Spare { get; set; }

    [JsonPropertyName("ApplicationID")]
    public ApplicationId ApplicationId { get; set; } = null!;

    [JsonPropertyName("BinaryData")]
    public string BinaryData { get; set; }
}