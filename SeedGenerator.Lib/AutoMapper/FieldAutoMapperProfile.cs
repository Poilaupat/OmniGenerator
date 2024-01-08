using AutoMapper;
using Microsoft.ML;
using SeedGenerator.Lib.DataGenerators;
using SeedGenerator.Lib.Param.FieldParams;
using System.Text.RegularExpressions;

namespace SeedGenerator.Lib.AutoMapper
{
    internal class FieldAutoMapperProfile : Profile
    {
        public FieldAutoMapperProfile()
        {
            // Base Types
            CreateMap<FieldParamBase, FieldGeneratorBase>();

            CreateMap<FieldParamDependantBase, FieldGeneratorDependantBase>()
                .IncludeBase<FieldParamBase, FieldGeneratorBase>();


            //Derived types based upon FieldParamBase
            CreateMap<FieldParamRegex, FieldGeneratorRegex>()
                .IncludeBase<FieldParamBase, FieldGeneratorBase>();

            CreateMap<FieldParamList, FieldGeneratorList>()
                .IncludeBase<FieldParamBase, FieldGeneratorBase>();

            CreateMap<FieldParamFixedValue, FieldGeneratorFixedValue>()
                .IncludeBase<FieldParamBase, FieldGeneratorBase>();

            CreateMap<FieldParamAmount, FieldGeneratorAmount>()
                .IncludeBase<FieldParamBase, FieldGeneratorBase>();

            CreateMap<FieldParamDate, FieldGeneratorDate>()
                .IncludeBase<FieldParamBase, FieldGeneratorBase>();

            //Derived types based upon FieldParamDependantBase
            CreateMap<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>()
                .IncludeBase<FieldParamDependantBase, FieldGeneratorDependantBase>();

            CreateMap<FieldParamComposite, FieldGeneratorComposite>()
                .IncludeBase<FieldParamDependantBase, FieldGeneratorDependantBase>();
        }
    }
}
