using AutoMapper;
using OmniGenerator.Lib.Data.FieldGenerators;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Param.FieldParams;

namespace OmniGenerator.Lib.AutoMapper
{
    internal class FieldAutoMapperProfile : Profile
    {
        /// <summary>
        /// The mapings between field configuration objects and field generators
        /// </summary>
        public FieldAutoMapperProfile()
        {
            // Base Types
            CreateMap(typeof(FieldParamBase), typeof(AbstractFieldGenerator<>));

            CreateMap<FieldParamBase, IFieldGenerator>()
                .Include<FieldParamRegex, FieldGeneratorRegex>()
                .Include<FieldParamList, FieldGeneratorList>()
                .Include<FieldParamConstant, FieldGeneratorConstant>()
                .Include<FieldParamNumeric, FieldGeneratorNumeric>()
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
            CreateMap<FieldParamNumeric, FieldGeneratorNumeric>();
            CreateMap<FieldParamDate, FieldGeneratorDate>();

            ////Derived types based upon FieldParamDependantBase
            CreateMap<FieldParamKeyCalculator, FieldGeneratorKeyCalculator>();
            CreateMap<FieldParamComposite, FieldGeneratorComposite>();
            CreateMap<FieldParamAggregate, FieldGeneratorAggregate>();
        }
    }
}
