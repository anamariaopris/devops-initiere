using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShopApi.Common;

public class JavaDateConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.Number => DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).UtcDateTime,
            JsonTokenType.String => ParseString(reader.GetString()!),
            _ => throw new JsonException("Invalid date value.")
        };
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var utc = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        writer.WriteStringValue(utc.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'+00:00'", CultureInfo.InvariantCulture));
    }

    private static DateTime ParseString(string text)
    {
        if (DateOnly.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        }

        if (DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
        {
            return parsed.UtcDateTime;
        }

        throw new JsonException($"Cannot parse date: {text}");
    }
}
