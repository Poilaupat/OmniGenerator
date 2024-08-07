using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using OmniGenerator.Lib.Configuration.Fields;

namespace OmniGenerator.Lib.Configuration.Serialization
{
    internal class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            jsonTypeInfo.PolymorphismOptions = jsonTypeInfo.Type switch
            {
                Type t when t == typeof(AbstractFieldConfigurationBase) => ResolveFieldConfigurationDerivedTypes(),
                Type t when t == typeof(ElementConfiguration) => ResolveElementConfigurationDerivedTypes(),
                _ => null,
            };

            return jsonTypeInfo;
        }

        private JsonPolymorphismOptions ResolveFieldConfigurationDerivedTypes()
        {
            return new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(FieldConfigurationRegex), "regex"),
                    new JsonDerivedType(typeof(FieldConfigurationConstant), "constant"),
                    new JsonDerivedType(typeof(FieldConfigurationEquiprobableList), "list-e"),
                    new JsonDerivedType(typeof(FieldConfigurationProbabilityDensityList), "list-dp"),
                    new JsonDerivedType(typeof(FieldConfigurationKeyCalculator), "key"),
                    new JsonDerivedType(typeof(FieldConfigurationComposite), "composite"),
                    new JsonDerivedType(typeof(FieldConfigurationNumeric), "numeric"),
                    new JsonDerivedType(typeof(FieldConfigurationDate), "date"),
                    new JsonDerivedType(typeof(FieldConfigurationAggregate), "aggregate"),
                }
            };
        }

        private JsonPolymorphismOptions ResolveElementConfigurationDerivedTypes()
        {
            return new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(DocumentConfiguration), "document"),
                    new JsonDerivedType(typeof(GroupConfiguration), "group"),
                }
            };
        }
    }
}
