// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json.Serialization;
using FastEnumUtility;

namespace AISStream;

/// <summary>
/// Utility class for improving performance of <see cref="AISMessageType"/>-related enum operations.
/// </summary>
[FastEnum<AISMessageType>]
internal partial class AISMessageTypeEnumBooster;

[JsonConverter(typeof(JsonStringEnumConverter<AISMessageType>))]
public enum AISMessageType
{
    PositionReport,
    // UnknownMessage,
    AddressedSafetyMessage,
    AddressedBinaryMessage,
    AidsToNavigationReport,
    // AssignedModeCommand,
    BaseStationReport,
    // BinaryAcknowledge,
    BinaryBroadcastMessage,
    ChannelManagement,
    // CoordinatedUTCInquiry,
    // DataLinkManagementMessage,
    // DataLinkManagementMessageData,
    ExtendedClassBPositionReport,
    // GroupAssignmentCommand,
    GnssBroadcastBinaryMessage,
    // Interrogation,
    LongRangeAisBroadcastMessage,
    MultiSlotBinaryMessage,
    SafetyBroadcastMessage,
    ShipStaticData,
    SingleSlotBinaryMessage,
    StandardClassBPositionReport,
    StandardSearchAndRescueAircraftReport,
    StaticDataReport
}
