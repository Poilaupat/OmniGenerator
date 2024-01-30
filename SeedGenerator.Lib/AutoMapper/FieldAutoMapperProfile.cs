using AutoMapper;
using SeedGenerator.Lib.Data.FieldGenerators;
using SeedGenerator.Lib.Param.FieldParams;

namespace SeedGenerator.Lib.AutoMapper
{
    internal class FieldAutoMapperProfile : Profile
    {
        public FieldAutoMapperProfile()
        {
            // Base Types
            CreateMap<FieldParamBase, AbstractFieldGenerator>();

            CreateMap<FieldParamDependantBase, AbstractFieldGeneratorDependant>()
                .IncludeBase<FieldParamBase, AbstractFieldGenerator>();


            //Derived types based upon FieldParamBase
            CreateMap<FieldParamRegex, FieldGeneratorRegex>()
                .IncludeBase<FieldParamBase, AbstractFieldGenerator>();

            CreateMap<FieldParamList, FieldGeneratorList>()
                .IncludeBase<FieldParamBase, AbstractFieldGenerator>();

            CreateMap<FieldParamConstant, FieldGeneratorConstant>()
                .IncludeBase<FieldParamBase, AbstractFieldGenerator>();

            CreateMap<FieldParamAmount, FieldGeneratorAmount>()
                .IncludeBase<FieldParamBase, AbstractFieldGenerator>();

            CreateMap<FieldParamDate, FieldGeneratorDate>()
                .IncludeBase<FieldParamBase, AbstractFieldGenerator>();

            //Derived types based upon FieldParamDependantBase
            CreateMap<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>()
                .IncludeBase<FieldParamDependantBase, AbstractFieldGeneratorDependant>();

            CreateMap<FieldParamComposite, FieldGeneratorComposite>()
                .IncludeBase<FieldParamDependantBase, AbstractFieldGeneratorDependant>();

            CreateMap<FieldParamAggregate, FieldGeneratorAggregate>()
                .IncludeBase<FieldParamDependantBase, AbstractFieldGeneratorDependant>();
        }
    }
}
