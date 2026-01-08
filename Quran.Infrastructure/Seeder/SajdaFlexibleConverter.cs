// Converters/SajdaFlexibleConverter.cs
using Quran.Infrastructure.Seeder;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SajdaFlexibleConverter : JsonConverter<SajdaDto>
{
    public override SajdaDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.False:
                // sajda: false
                return new SajdaDto { Id = null, Recommended = false, Obligatory = false };

            case JsonTokenType.True:
                // sajda: true (rare case)
                return new SajdaDto { Id = 1, Recommended = true, Obligatory = false };

            case JsonTokenType.Number:
                // sajda: 1 (just a number)
                var num = reader.GetInt32();
                return new SajdaDto
                {
                    Id = num > 0 ? num : null,
                    Recommended = num > 0,
                    Obligatory = false
                };

            case JsonTokenType.StartObject:
                // sajda: { "id": 1, "recommended": true, "obligatory": false }
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    var root = doc.RootElement;

                    return new SajdaDto
                    {
                        Id = root.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.Number
                            ? idProp.GetInt32()
                            : null,
                        Recommended = root.TryGetProperty("recommended", out var recProp) && recProp.ValueKind == JsonValueKind.True,
                        Obligatory = root.TryGetProperty("obligatory", out var oblProp) && oblProp.ValueKind == JsonValueKind.True
                    };
                }

            default:
                // Any other case, no sajda
                return new SajdaDto { Id = null, Recommended = false, Obligatory = false };
        }
    }

    public override void Write(Utf8JsonWriter writer, SajdaDto value, JsonSerializerOptions options)
    {
        if (value == null || !value.HasSajda)
        {
            writer.WriteBooleanValue(false);
        }
        else
        {
            writer.WriteStartObject();
            writer.WriteNumber("id", value.Id ?? 0);
            writer.WriteBoolean("recommended", value.Recommended);
            writer.WriteBoolean("obligatory", value.Obligatory);
            writer.WriteEndObject();
        }
    }
}