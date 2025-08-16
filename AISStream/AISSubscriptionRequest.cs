// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using Riok.Mapperly.Abstractions;

namespace AISStream;

public class AISSubscriptionRequest
{
    [JsonPropertyName("BoundingBoxes")]
    public List<List<List<double>>> BoundingBoxes => [[[-90.0, -180.0], [90.0, 180.0]]];

    [JsonPropertyName("FiltersShipMMSI")]
    public IList<string>? FiltersShipMMSI { get; set; }

    [JsonPropertyName("FilterMessageTypes")]
    public IList<AISMessageType>? FiltersMessageType { get; set; }
}

/// <summary>
/// Authenticated variant of <see cref="AISSubscriptionRequest"/>
/// </summary>
[Mapper]
internal partial class AISAuthenticatedSubscriptionRequest : AISSubscriptionRequest
{
    [JsonPropertyName("APIKey")]
    public required string ApiKey { get; set; }

    public static partial AISAuthenticatedSubscriptionRequest CreateAuthenticatedRequest(AISSubscriptionRequest req, string apiKey);
}
