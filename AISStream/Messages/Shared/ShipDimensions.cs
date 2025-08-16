// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages.Shared;

public record ShipDimensions(
    [property: JsonPropertyName("A")] int ToBow,
    [property: JsonPropertyName("B")] int ToStern,
    [property: JsonPropertyName("C")] int ToPort,
    [property: JsonPropertyName("D")] int ToStarboard
);
