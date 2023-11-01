using AutoMapper;
using SeedGenerator.Lib.Data.Fields.Generators;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib.Data.Fields
{
    internal class FieldAutoMapperProfile : Profile
    {
        public FieldAutoMapperProfile()
        {
            CreateMap<FieldParamBase, FieldGeneratorBase>()
                .Include<FieldParamRegex, FieldGeneratorRegex>()
                .Include<FieldParamList, FieldGeneratorList>()
                .Include<FieldParamFixedValue, FieldGeneratorFixedValue>()
                .Include<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>()
                .Include<FieldParamComposite, FieldGeneratorComposite>()
                .Include<FieldParamAmount, FieldGeneratorAmount>();

            CreateMap<FieldParamRegex, FieldGeneratorRegex>();
            CreateMap<FieldParamList, FieldGeneratorList>();
            CreateMap<FieldParamFixedValue, FieldGeneratorFixedValue>();
            CreateMap<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>();
            CreateMap<FieldParamComposite, FieldGeneratorComposite>();
            CreateMap<FieldParamAmount, FieldGeneratorAmount>();

        }
    }
}
