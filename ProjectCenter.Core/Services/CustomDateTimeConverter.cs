using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectCenter.Core.Services
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string[] _formats = new[]
        {
            "dd.MM.yyyy",
            "dd.MM.yyyy HH:mm:ss",
            "yyyy-MM-dd",
            "yyyy-MM-ddTHH:mm:ss",
            "dd/MM/yyyy",
            "dd/MM/yyyy HH:mm:ss"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            foreach (var format in _formats)
            {
                if (DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out var date))
                    return date;
            }

            throw new JsonException($"Некорректный формат даты. Поддерживаемые форматы: {string.Join(", ", _formats)}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
        }
    }
}
