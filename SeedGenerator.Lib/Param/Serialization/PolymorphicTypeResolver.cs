using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using SeedGenerator.Lib.MetaData;
using SeedGenerator.Lib.Param.MetaData;

namespace SeedGenerator.Lib.Param.Serialization
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            jsonTypeInfo.PolymorphismOptions = jsonTypeInfo.Type switch
            {
                Type t when t == typeof(MetaDataParamBase) => ResolveMetaDataParamDerivedTypes(),
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
                    new JsonDerivedType(typeof(MetaDataParamRegex), "regex"),
                    new JsonDerivedType(typeof(MetaDataParamFixedValue), "fixed"),
                    new JsonDerivedType(typeof(MetaDataParamList), "list"),
                    new JsonDerivedType(typeof(MetaDataParamKeyCalculator), "key"),
                }
            };
        }
    }
}
