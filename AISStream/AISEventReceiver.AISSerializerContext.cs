// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json;
using System.Text.Json.Serialization;

namespace AISStream;

public partial class AISEventReceiver
{
    [JsonSerializable(typeof(AISEvent))]
    [JsonSerializable(typeof(AISMessage))]
    [JsonSerializable(typeof(AISSubscriptionRequestBody))]
    [JsonSourceGenerationOptions(JsonSerializerDefaults.Web, AllowOutOfOrderMetadataProperties = true)]
    internal partial class AISSerializerContext : JsonSerializerContext;
}