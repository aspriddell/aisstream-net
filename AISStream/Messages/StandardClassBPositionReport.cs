// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;

namespace AISStream.Messages;

public class StandardClassBPositionReport : PositionReportBase
{
    [JsonPropertyName("Spare1")]
    public int Spare1 { get; set; }

    [JsonPropertyName("Spare2")]
    public int Spare2 { get; set; }

    [JsonPropertyName("ClassBUnit")]
    public bool ClassBUnit { get; set; }

    [JsonPropertyName("ClassBDisplay")]
    public bool ClassBDisplay { get; set; }

    [JsonPropertyName("ClassBDsc")]
    public bool ClassBDsc { get; set; }

    [JsonPropertyName("ClassBBand")]
    public bool ClassBBand { get; set; }

    [JsonPropertyName("ClassBMsg22")]
    public bool ClassBMsg22 { get; set; }

    [JsonPropertyName("AssignedMode")]
    public bool AssignedMode { get; set; }

    [JsonPropertyName("CommunicationStateIsItdma")]
    public bool CommunicationStateIsItdma { get; set; }
}