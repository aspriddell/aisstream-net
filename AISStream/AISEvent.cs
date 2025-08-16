// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using FastEnumUtility;

namespace AISStream;

public class AISEvent
{
    [JsonPropertyName("Message")]
    [JsonConverter(typeof(AISMessageConverter))]
    public AISMessage? Message { get; set; }

    [JsonPropertyName("MessageType")]
    public required string MessageType { get; set; }

    [JsonIgnore]
    public bool IsSupported => FastEnum.TryParse<AISMessageType, AISMessageTypeEnumBooster>(MessageType, out _);
}