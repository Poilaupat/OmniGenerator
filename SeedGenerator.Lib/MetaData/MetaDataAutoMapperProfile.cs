using AutoMapper;
using SeedGenerator.Lib.MetaData.Generators;
using SeedGenerator.Lib.Param.MetaData;

namespace SeedGenerator.Lib.MetaData
{
    internal class MetaDataAutoMapperProfile : Profile
    {
        public MetaDataAutoMapperProfile() 
        {
            CreateMap<MetaDataParamBase, MetaDataGeneratorBase>()
                .Include<MetaDataParamRegex, MetaDataGeneratorRegex>()
                .Include<MetaDataParamList, MetaDataGeneratorList>()
                .Include<MetaDataParamFixedValue, MetaDataGeneratorFixedValue>()
                .Include<MetaDataParamKeyCalculator, MetaDataGeneratorKeyCalculator>();

            CreateMap<MetaDataParamRegex, MetaDataGeneratorRegex>();
            CreateMap<MetaDataParamList, MetaDataGeneratorList>();
            CreateMap<MetaDataParamFixedValue, MetaDataGeneratorFixedValue>();
            CreateMap<MetaDataParamKeyCalculator, MetaDataGeneratorKeyCalculator>();
        }
    }
}
