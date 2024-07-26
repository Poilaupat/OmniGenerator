using AutoMapper;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Configuration.Fields;

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
            CreateMap(typeof(FieldConfigurationBase), typeof(AbstractFieldGenerator<>));

            CreateMap<FieldConfigurationBase, IFieldGenerator>()
                .Include<FieldConfigurationRegex, FieldGeneratorRegex>()
                .Include<FieldConfigurationList, FieldGeneratorList>()
                .Include<FieldConfigurationConstant, FieldGeneratorConstant>()
                .Include<FieldConfigurationNumeric, FieldGeneratorNumeric>()
                .Include<FieldConfigurationDate, FieldGeneratorDate>();

            CreateMap(typeof(FieldConfigurationDependantBase), typeof(AbstractFieldGeneratorDependant<>));

            CreateMap<FieldConfigurationDependantBase, IFieldGeneratorDependent>()
                .IncludeBase<FieldConfigurationBase, IFieldGenerator>()
                .Include<FieldConfigurationKeyCalculator, FieldGeneratorKeyCalculator>()
                .Include<FieldConfigurationComposite, FieldGeneratorComposite>()
                .Include<FieldConfigurationAggregate, FieldGeneratorAggregate>();

            ////Derived types based upon FieldConfigurationBase
            CreateMap<FieldConfigurationRegex, FieldGeneratorRegex>();
            CreateMap<FieldConfigurationList, FieldGeneratorList>();
            CreateMap<FieldConfigurationConstant, FieldGeneratorConstant>();
            CreateMap<FieldConfigurationNumeric, FieldGeneratorNumeric>();
            CreateMap<FieldConfigurationDate, FieldGeneratorDate>();

            ////Derived types based upon FieldConfigurationDependantBase
            CreateMap<FieldConfigurationKeyCalculator, FieldGeneratorKeyCalculator>();
            CreateMap<FieldConfigurationComposite, FieldGeneratorComposite>();
            CreateMap<FieldConfigurationAggregate, FieldGeneratorAggregate>();
        }
    }
}
