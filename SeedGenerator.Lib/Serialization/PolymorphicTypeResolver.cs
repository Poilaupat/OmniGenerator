using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using SeedGenerator.Lib.MetaData;

namespace SeedGenerator.Lib.Serialization
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            jsonTypeInfo.PolymorphismOptions = jsonTypeInfo.Type switch
            {
                Type mdgb when mdgb == typeof(MetaDataGeneratorBase) => ResolveMetaDataGeneratorDerivedTypes(),
                _ => null,
            };

            return jsonTypeInfo;
        }

        private JsonPolymorphismOptions ResolveMetaDataGeneratorDerivedTypes()
        {
            return new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(MetaDataGeneratorRegex), "regex"),
                    new JsonDerivedType(typeof(MetaDataGeneratorFixedValue), "fixed"),
                    new JsonDerivedType(typeof(MetaDataGeneratorList), "list"),
                    new JsonDerivedType(typeof(MetaDataGeneratorKeyRlmc), "rlmc"),
                }
            };
        }
    }
}
