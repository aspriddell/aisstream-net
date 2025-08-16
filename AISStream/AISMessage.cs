// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

global using ApplicationId = AISStream.Messages.Shared.ApplicationId;

using System.Text.Json.Serialization;
using AISStream.Messages;

namespace AISStream;

/// <summary>
/// The base type for all AIS messages.
/// </summary>
[JsonDerivedType(typeof(PositionReport), 1)]
[JsonDerivedType(typeof(AssignedScheduledPositionReport), 2)]
[JsonDerivedType(typeof(SpecialPositionReport), 3)]
[JsonDerivedType(typeof(BaseStationReport), 4)]
[JsonDerivedType(typeof(ShipStaticData), 5)]
[JsonDerivedType(typeof(AddressedBinaryMessage), 6)]
[JsonDerivedType(typeof(BinaryBroadcastMessage), 8)]
[JsonDerivedType(typeof(StandardSearchAndRescueAircraftReport), 9)]
[JsonDerivedType(typeof(BaseStationUtcResponse), 11)]
[JsonDerivedType(typeof(AddressedSafetyMessage), 12)]
[JsonDerivedType(typeof(SafetyBroadcastMessage), 14)]
[JsonDerivedType(typeof(GnssBroadcastBinaryMessage), 17)]
[JsonDerivedType(typeof(StandardClassBPositionReport), 18)]
[JsonDerivedType(typeof(ExtendedClassBPositionReport), 19)]
[JsonDerivedType(typeof(AidsToNavigationReport), 21)]
[JsonDerivedType(typeof(ChannelManagement), 22)]
[JsonDerivedType(typeof(StaticDataReport), 24)]
[JsonDerivedType(typeof(SingleSlotBinaryMessage), 25)]
[JsonDerivedType(typeof(MultiSlotBinaryMessage), 26)]
[JsonDerivedType(typeof(LongRangeAisBroadcastMessage), 27)]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "MessageID", IgnoreUnrecognizedTypeDiscriminators = true)]
public class AISMessage
{
    [JsonPropertyName("RepeatIndicator")]
    public int RepeatIndicator { get; set; }

    /// <summary>
    /// The ship MMSI number or base station ID.
    /// </summary>
    [JsonPropertyName("UserID")]
    public int UserId { get; set; }

    [JsonPropertyName("Valid")]
    public bool Valid { get; set; }

    [JsonIgnore]
    internal string? MessageTypeName { get; set; }
}