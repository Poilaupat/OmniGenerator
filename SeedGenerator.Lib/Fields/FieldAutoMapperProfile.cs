using AutoMapper;
using SeedGenerator.Lib.Fields.Generators;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib.Fields
{
    internal class FieldAutoMapperProfile : Profile
    {
        public FieldAutoMapperProfile() 
        {
            CreateMap<FieldParamBase, FieldGeneratorBase>()
                .Include<FieldParamRegex, FieldGeneratorRegex>()
                .Include<FieldParamList, FieldGeneratorList>()
                .Include<FieldParamFixedValue, FieldGeneratorFixedValue>()
                .Include<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>();

            CreateMap<FieldParamRegex, FieldGeneratorRegex>();
            CreateMap<FieldParamList, FieldGeneratorList>();
            CreateMap<FieldParamFixedValue, FieldGeneratorFixedValue>();
            CreateMap<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>();
        }
    }
}
