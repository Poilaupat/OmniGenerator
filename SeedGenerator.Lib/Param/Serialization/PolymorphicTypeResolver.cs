using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib.Param.Serialization
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            jsonTypeInfo.PolymorphismOptions = jsonTypeInfo.Type switch
            {
                Type t when t == typeof(FieldParamBase) => ResolveMetaDataParamDerivedTypes(),
                _ => null,
            };

            return jsonTypeInfo;
        }

        private JsonPolymorphismOptions ResolveMetaDataParamDerivedTypes()
        {
            return new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(FieldParamRegex), "regex"),
                    new JsonDerivedType(typeof(FieldParamFixedValue), "fixed"),
                    new JsonDerivedType(typeof(FieldParamList), "list"),
                    new JsonDerivedType(typeof(FieldParamKeyCalculator), "key"),
                }
            };
        }
    }
}
