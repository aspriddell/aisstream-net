// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

namespace AISStream.Enums;

public enum PositionFixingDeviceType
{
    Undefined = 0,
    GPS = 1,
    GLONASS = 2,
    CombinedGPSGLONASS = 3,
    LoranC = 4,
    Chayka = 5,
    IntegratedNavigationSystem = 6,
    Surveyed = 7,
    Galileo = 8,
    InternalGNSS = 15
}