// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Coordinates;

namespace AISStream;

public class AISSubscriptionRequestOptions
{
    /// <summary>
    /// List of bounding boxes to filter AIS messages by geographic region
    /// </summary>
    /// <remarks>
    /// If not set, AISStream.NET will submit <see cref="BoundingBox.World"/> as the selected area.
    /// </remarks>
    public IReadOnlyList<BoundingBox>? BoundingBoxes { get; set; }

    /// <summary>
    /// Optional list of ship MMSI numbers to request messages from
    /// </summary>
    public IReadOnlyList<long>? FiltersShipMMSI { get; set; }

    /// <summary>
    /// Optional list of <see cref="AISMessageType"/>s to request
    /// </summary>
    public IReadOnlyList<AISMessageType>? FiltersMessageType { get; set; }

    internal AISSubscriptionRequestBody CreateRequest(string apiKey) => new()
    {
        ApiKey = apiKey,
        BoundingBoxesJson = (BoundingBoxes ?? [BoundingBox.World]).Select(bb => bb.ToArray()).ToArray(),
        FiltersShipMMSI = FiltersShipMMSI?.Select(mmsi => mmsi.ToString()).ToArray(),
        FiltersMessageType = FiltersMessageType
    };
}

internal record AISSubscriptionRequestBody
{
    [JsonPropertyName("APIKey")]
    public required string ApiKey { get; init; }

    [JsonPropertyName("BoundingBoxes")]
    public required double[][][] BoundingBoxesJson { get; init; }

    [JsonPropertyName("FiltersShipMMSI")]
    public IReadOnlyList<string>? FiltersShipMMSI { get; init; }

    [JsonPropertyName("FilterMessageTypes")]
    public IReadOnlyList<AISMessageType>? FiltersMessageType { get; init; }
}