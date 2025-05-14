using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using OmniGenerator.Lib.Configuration.Fields;

namespace OmniGenerator.Lib.Configuration.Serialization
{
    /// <summary>
    /// Provides custom polymorphic type resolution for JSON serialization and deserialization.
    /// This resolver configures how derived types of specific configuration base classes are handled,
    /// enabling correct (de)serialization of polymorphic objects using type discriminators.
    /// </summary>
    internal class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        /// <summary>
        /// Returns the <see cref="JsonTypeInfo"/> for the specified type, configuring polymorphism options
        /// for supported base types.
        /// </summary>
        /// <param name="type">The type for which to get serialization metadata.</param>
        /// <param name="options">The serializer options in use.</param>
        /// <returns>
        /// A <see cref="JsonTypeInfo"/> instance with custom polymorphism options if the type is supported;
        /// otherwise, the default type info.
        /// </returns>
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

        /// <summary>
        /// Configures polymorphism options for types derived from <see cref="AbstractFieldConfigurationBase"/>.
        /// Sets up type discriminators and derived type mappings for field configuration classes.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonPolymorphismOptions"/> instance describing the supported derived types.
        /// </returns>
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
                        new JsonDerivedType(typeof(FieldConfigurationEquiprobableList), "equiprobable-list"),
                        new JsonDerivedType(typeof(FieldConfigurationProbabilityDensityList), "probadensity-list"),
                        new JsonDerivedType(typeof(FieldConfigurationKeyCalculator), "key"),
                        new JsonDerivedType(typeof(FieldConfigurationComposite), "composite"),
                        new JsonDerivedType(typeof(FieldConfigurationNumeric), "numeric"),
                        new JsonDerivedType(typeof(FieldConfigurationDate), "date"),
                        new JsonDerivedType(typeof(FieldConfigurationAggregate), "aggregate"),
                        new JsonDerivedType(typeof(FieldConfigurationIncrement), "increment"),
                    }
            };
        }

        /// <summary>
        /// Configures polymorphism options for types derived from <see cref="ElementConfiguration"/>.
        /// Sets up type discriminators and derived type mappings for element configuration classes.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonPolymorphismOptions"/> instance describing the supported derived types.
        /// </returns>
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
