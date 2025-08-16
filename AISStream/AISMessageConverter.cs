// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using FastEnumUtility;

namespace AISStream;

public class AISMessageConverter : JsonConverter<AISMessage>
{
    public override AISMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token for AisMessageBase.");
        }

        string? messageTypeName = null;
        var initialDepth = reader.CurrentDepth;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName && reader.CurrentDepth == initialDepth + 1)
            {
                var propertyName = reader.GetString();
                if (FastEnum.TryParse<AISMessageType, AISMessageTypeEnumBooster>(propertyName, out _))
                {
                    messageTypeName = propertyName;
                    break;
                }
            }

            // break when the end of the current object is reached
            if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == initialDepth)
            {
                break;
            }
        }

        // message type not supported
        if (messageTypeName == null)
        {
            return null;
        }

        var message = JsonSerializer.Deserialize<AISMessage>(ref reader, options);
        if (message == null)
        {
            throw new JsonException("Failed to deserialize AISMessage.");
        }

        // restore reader to the initial depth ready for next read.
        while (reader.CurrentDepth > initialDepth)
        {
            if (!reader.Read())
            {
                throw new JsonException("Unexpected end of JSON while reading input.");
            }
        }

        message.MessageTypeName = messageTypeName;
        return message;
    }

    public override void Write(Utf8JsonWriter writer, AISMessage value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "AISMessage cannot be null.");
        }

        writer.WriteStartObject();

        writer.WriteStringValue(value.MessageTypeName);
        JsonSerializer.Serialize(writer, value, options);

        writer.WriteEndObject();
    }
}