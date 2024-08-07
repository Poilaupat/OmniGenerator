using AutoMapper;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Interfaces.FieldGenerators;

namespace OmniGenerator.Lib.AutoMapper
{
    internal sealed class FieldAutoMapperProfile : Profile
    {
        /// <summary>
        /// The mapings between field configuration objects and field generators
        /// </summary>
        public FieldAutoMapperProfile()
        {
            // Base Types
            CreateMap(typeof(AbstractFieldConfigurationBase), typeof(AbstractFieldGenerator<>));

            CreateMap<AbstractFieldConfigurationBase, IFieldGenerator>()
                .Include<FieldConfigurationRegex, FieldGeneratorRegex>()
                .Include<FieldConfigurationConstant, FieldGeneratorConstant>()
                .Include<FieldConfigurationNumeric, FieldGeneratorNumeric>()
                .Include<FieldConfigurationDate, FieldGeneratorDate>();

            CreateMap(typeof(AbstractFieldConfigurationDependantBase), typeof(AbstractFieldGeneratorDependant<>));

            CreateMap<AbstractFieldConfigurationDependantBase, IFieldGeneratorDependent>()
                .IncludeBase<AbstractFieldConfigurationBase, IFieldGenerator>()
                .Include<FieldConfigurationKeyCalculator, FieldGeneratorKeyCalculator>()
                .Include<FieldConfigurationComposite, FieldGeneratorComposite>()
                .Include<FieldConfigurationAggregate, FieldGeneratorAggregate>();

            CreateMap(typeof(AbstractFieldConfigurationCollectionBase), typeof(AbstractFieldGeneratorCollectionBase<,>));

            CreateMap(typeof(AbstractFieldConfigurationCollectionBase), typeof(IFieldGeneratorCollection<>))
                .IncludeBase(typeof(AbstractFieldConfigurationBase), typeof(IFieldGenerator))
                .Include(typeof(FieldConfigurationEquiprobableList), typeof(FieldGeneratorEquiprobableList))
                .Include(typeof(FieldConfigurationProbabilityDensityList), typeof(FieldGeneratorProbabilityDensityList));


            ////Derived types based upon FieldConfigurationBase
            CreateMap<FieldConfigurationRegex, FieldGeneratorRegex>();
            CreateMap<FieldConfigurationEquiprobableList, FieldGeneratorEquiprobableList>();
            CreateMap<FieldConfigurationConstant, FieldGeneratorConstant>();
            CreateMap<FieldConfigurationNumeric, FieldGeneratorNumeric>();
            CreateMap<FieldConfigurationDate, FieldGeneratorDate>();

            ////Derived types based upon FieldConfigurationDependantBase
            CreateMap<FieldConfigurationKeyCalculator, FieldGeneratorKeyCalculator>();
            CreateMap<FieldConfigurationComposite, FieldGeneratorComposite>();
            CreateMap<FieldConfigurationAggregate, FieldGeneratorAggregate>();

            ////Derived types based upon FieldConfigurationCollectionBase
            CreateMap<FieldConfigurationEquiprobableList, FieldGeneratorEquiprobableList>();
            CreateMap<FieldConfigurationProbabilityDensityList, FieldGeneratorProbabilityDensityList>();
        }
    }
}
