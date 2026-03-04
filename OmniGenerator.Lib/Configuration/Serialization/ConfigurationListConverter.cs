using OmniGenerator.Lib.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration.Serialization
{
    internal class ConfigurationListConverter : JsonConverter<IEnumerable<WeightedValue>>
    {
        public override IEnumerable<WeightedValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<WeightedValue> result = new();

            // Array format : ["A", "B", "C"] the weight is 1.0 by default
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                        break;

                    if (reader.TokenType != JsonTokenType.String)
                        throw new JsonException("Expected string element in array.");

                    string? key = reader.GetString();

                    if (!string.IsNullOrWhiteSpace(key))
                        result.Add(new WeightedValue(key, 1.0));
                }
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                // Dictionary format : { "A":1.0, "B":2.0 }
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                        break;

                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException("Expected property name.");

                    string? key = reader.GetString();

                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        reader.Read(); // move to value

                        if (reader.TokenType != JsonTokenType.Number)
                            throw new JsonException("Expected number value.");

                        double value = reader.GetDouble();
                        result.Add(new WeightedValue(key, value));
                    }
                }
            }
            else
            {
                throw new JsonException("Invalid JSON token for list property.");
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<WeightedValue> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}