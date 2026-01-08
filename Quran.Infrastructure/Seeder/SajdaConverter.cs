// Converters/SajdaConverter.cs
using System.Text.Json;
using System.Text.Json.Serialization;

public class SajdaConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.True:
                return 1; // Default sajda number

            case JsonTokenType.False:
                return null; // No sajda

            case JsonTokenType.Number:
                var num = reader.GetInt32();
                return num > 0 ? num : null;

            case JsonTokenType.String:
                var stringValue = reader.GetString();

                if (string.IsNullOrWhiteSpace(stringValue))
                    return null;

                if (int.TryParse(stringValue, out int intResult))
                    return intResult > 0 ? intResult : null;

                if (bool.TryParse(stringValue, out bool boolResult))
                    return boolResult ? 1 : null;

                return null;

            default:
                return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value.HasValue && value.Value > 0)
            writer.WriteNumberValue(value.Value);
        else
            writer.WriteBooleanValue(false);
    }
}