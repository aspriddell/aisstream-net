// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using AISStream.Enums;
using AISStream.Messages.Shared;

namespace AISStream.Messages;

public class ShipStaticData : AISMessage
{
    [JsonPropertyName("AisVersion")] public int AISVersion { get; set; }

    [JsonPropertyName("ImoNumber")] public int IMONumber { get; set; }

    [JsonPropertyName("CallSign")] public string CallSign { get; set; } = null!;

    [JsonPropertyName("Name")] public string Name { get; set; } = null!;

    [JsonPropertyName("Type")] public int ShipAndCargoType { get; set; }

    [JsonPropertyName("Dimension")] public ShipDimensions Dimension { get; set; } = null!;

    [JsonPropertyName("FixType")] public PositionFixingDeviceType PositionFixingType { get; set; }

    [JsonPropertyName("Eta")]
    public ShipStaticDataEta ETA { get; set; } = null!;

    [JsonPropertyName("MaximumStaticDraught")]
    public double MaximumStaticDraught { get; set; }

    [JsonPropertyName("Destination")] public string Destination { get; set; } = null!;

    /// <summary>
    /// Whether the ship's DTE (Data Terminal Equipment) is ready to receive messages.
    /// </summary>
    [JsonPropertyName("Dte")]
    public bool DTEReady { get; set; }

    [JsonPropertyName("Spare")] public bool Spare { get; set; }
}

public record ShipStaticDataEta(
    [property: JsonPropertyName("Day")] int Day,
    [property: JsonPropertyName("Hour")] int Hour,
    [property: JsonPropertyName("Minute")] int Minute,
    [property: JsonPropertyName("Month")] int Month
)
{
    public bool IsDateReliable => Day is > 0 and < 31 && Month is > 0 and < 13;
    public bool IsTimeReliable => Hour is >= 0 and < 24;
    
    public DateTime? ToDateTime()
    {
        if (!IsDateReliable && !IsTimeReliable)
        {
            return null;
        }

        DateOnly date;

        if (IsDateReliable)
        {
            var now = DateTime.UtcNow;
            var correctedYear = Month < now.Month ? now.Year + 1 : now.Year;

            date = new DateOnly(correctedYear, Month, Math.Min(Day, DateTime.DaysInMonth(correctedYear, Month)));
        }
        else
        {
            date = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        return date.ToDateTime(IsTimeReliable ? new TimeOnly(Hour, Math.Min(59, Minute)) : new TimeOnly(0, 0), DateTimeKind.Utc);
    }

    public static implicit operator DateTime?(ShipStaticDataEta eta) => eta.ToDateTime();
}