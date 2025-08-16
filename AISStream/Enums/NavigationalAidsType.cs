// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

namespace AISStream.Enums;

public enum NavigationalAidsType
{
    Unspecified = 0,
    ReferencePoint = 1,
    RACON = 2,
    OffshoreFixedStructure = 3,
    EmergencyWreckMarkingBuoy = 4,

    FixedAtoNLightWithoutSectors = 5,
    FixedAtoNLightWithSectors = 6,

    FixedAtoNLeadingLightFront = 7,
    FixedAtoNLeadingLightRear = 8,

    FixedAtoNBeaconCardinalNorth = 9,
    FixedAtoNBeaconCardinalEast = 10,
    FixedAtoNBeaconCardinalSouth = 11,
    FixedAtoNBeaconCardinalWest = 12,

    FixedAtoNBeaconPortHand = 13,
    FixedAtoNBeaconStarboardHand = 14,
    FixedAtoNBeaconPreferredChannelPortHand = 15,
    FixedAtoNBeaconPreferredChannelStarboardHand = 16,
    FixedAtoNBeaconIsolatedDanger = 17,
    FixedAtoNBeaconSafeWater = 18,
    FixedAtoNBeaconSpecialMark = 19,

    FloatingAtoNCardinalMarkNorth = 20,
    FloatingAtoNCardinalMarkEast = 21,
    FloatingAtoNCardinalMarkSouth = 22,
    FloatingAtoNCardinalMarkWest = 23,

    FloatingAtoNPortHand = 24,
    FloatingAtoNStarboardHand = 25,

    FloatingAtoNPreferredChannelPortHand = 26,
    FloatingAtoNPreferredChannelStarboardHand = 27,

    FloatingAtoNIsolatedDanger = 28,
    FloatingAtoNSafeWater = 29,
    FloatingAtoNSpecialMark = 30,
    FloatingAtoNLightWithoutSectors = 31,
}