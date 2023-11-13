using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib.Param.Serialization
{
    internal class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            jsonTypeInfo.PolymorphismOptions = jsonTypeInfo.Type switch
            {
                Type t when t == typeof(FieldParamBase) => ResolveFieldParamDerivedTypes(),
                Type t when t == typeof(ElementParam) => ResolveElementParamDerivedTypes(),
                _ => null,
            };

            return jsonTypeInfo;
        }

        private JsonPolymorphismOptions ResolveFieldParamDerivedTypes()
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
                    new JsonDerivedType(typeof(FieldParamComposite), "composite"),
                    new JsonDerivedType(typeof(FieldParamAmount), "amount"),
                    new JsonDerivedType(typeof(FieldParamDate), "date"),
                }
            };
        }

        private JsonPolymorphismOptions ResolveElementParamDerivedTypes()
        {
            return new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(DocumentParam), "document"),
                    new JsonDerivedType(typeof(GroupParam), "group"),
                }
            };
        }
    }
}
