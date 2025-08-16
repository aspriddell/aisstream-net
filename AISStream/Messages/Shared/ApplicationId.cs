// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages.Shared;

public record ApplicationId(
    [property: JsonPropertyName("DesignatedAreaCode")] int DesignatedAreaCode,
    [property: JsonPropertyName("FunctionIdentifier")] int FunctionIdentifier)
{
    [JsonPropertyName("Valid")] public bool Valid { get; set; }
}