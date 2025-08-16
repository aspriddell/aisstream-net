// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Messages.Shared;

namespace AISStream.Messages;

public class StaticDataReport : AISMessage
{
    [JsonPropertyName("Reserved")]
    public int Reserved { get; set; }

    [JsonPropertyName("PartNumber")]
    public bool PartNumber { get; set; }

    [JsonPropertyName("ReportA")]
    public StaticDataReportA ReportA { get; set; } = null!;

    [JsonPropertyName("ReportB")]
    public StaticDataReportB ReportB { get; set; } = null!;
}

public record StaticDataReportA([property: JsonPropertyName("Name")] string Name)
{
    [JsonPropertyName("Valid")]
    public bool Valid { get; set; }
}

public class StaticDataReportB
{
    [JsonPropertyName("CallSign")]
    public string CallSign { get; set; } = null!;

    [JsonPropertyName("Dimension")]
    public ShipDimensions Dimension { get; set; } = null!;

    [JsonPropertyName("FixType")]
    public PositionFixingDeviceType PositionFixingDevice { get; set; }

    [JsonPropertyName("ShipType")]
    public int ShipAndCargoType { get; set; }

    [JsonPropertyName("Spare")]
    public int Spare { get; set; }

    [JsonPropertyName("Valid")]
    public bool Valid { get; set; }

    [JsonPropertyName("VendorIDModel")]
    public int VendorIdModel { get; set; }

    [JsonPropertyName("VendorIDSerial")]
    public int VendorIdSerial { get; set; }

    [JsonPropertyName("VendorIDName")]
    public string VendorIdName { get; set; } = null!;
}
