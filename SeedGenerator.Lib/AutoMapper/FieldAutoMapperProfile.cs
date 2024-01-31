using AutoMapper;
using SeedGenerator.Lib.Data.FieldGenerators;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib.AutoMapper
{
    internal class FieldAutoMapperProfile : Profile
    {
        public FieldAutoMapperProfile()
        {
            // Base Types
            CreateMap(typeof(FieldParamBase), typeof(AbstractFieldGenerator<>));

            CreateMap<FieldParamBase, IFieldGenerator>()
                .Include<FieldParamRegex, FieldGeneratorRegex>()
                .Include<FieldParamList, FieldGeneratorList>()
                .Include<FieldParamConstant, FieldGeneratorConstant>()
                .Include<FieldParamAmount, FieldGeneratorAmount>()
                .Include<FieldParamDate, FieldGeneratorDate>();

            CreateMap(typeof(FieldParamDependantBase), typeof(AbstractFieldGeneratorDependant<>));

            CreateMap<FieldParamDependantBase, IFieldGeneratorDependent>()
                .IncludeBase<FieldParamBase, IFieldGenerator>()
                .Include<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>()
                .Include<FieldParamComposite, FieldGeneratorComposite>()
                .Include<FieldParamAggregate, FieldGeneratorAggregate>();

            ////Derived types based upon FieldParamBase
            CreateMap<FieldParamRegex, FieldGeneratorRegex>();
            CreateMap<FieldParamList, FieldGeneratorList>();
            CreateMap<FieldParamConstant, FieldGeneratorConstant>();
            CreateMap<FieldParamAmount, FieldGeneratorAmount>();
            CreateMap<FieldParamDate, FieldGeneratorDate>();

            ////Derived types based upon FieldParamDependantBase
            CreateMap<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>();
            CreateMap<FieldParamComposite, FieldGeneratorComposite>();
            CreateMap<FieldParamAggregate, FieldGeneratorAggregate>();
        }
    }
}
