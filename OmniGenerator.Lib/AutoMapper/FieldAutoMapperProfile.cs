using AutoMapper;
using OmniGenerator.Lib.Generators.Fields;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using OmniGenerator.Lib.Tools;
using System.Collections.Generic;

namespace OmniGenerator.Lib.AutoMapper
{
    internal sealed class FieldAutoMapperProfile : Profile
    {
        /// <summary>
        /// The mapings between field configuration and field generators
        /// </summary>
        public FieldAutoMapperProfile()
        {
            // Base Types
            CreateMap(typeof(AbstractFieldConfigurationBase), typeof(AbstractFieldGenerator<>));

            CreateMap<AbstractFieldConfigurationBase, IFieldGenerator>()
                .Include<FieldConfigurationRegex, FieldGeneratorRegex>()
                .Include<FieldConfigurationConstant, FieldGeneratorConstant>()
                .Include<FieldConfigurationNumeric, FieldGeneratorNumeric>()
                .Include<FieldConfigurationDate, FieldGeneratorDate>()
                .Include<FieldConfigurationIncrement, FieldGeneratorIncrement>();

            CreateMap(typeof(AbstractFieldConfigurationDependantBase), typeof(AbstractFieldGeneratorDependant<>));

            CreateMap<AbstractFieldConfigurationDependantBase, IFieldGeneratorDependent>()
                .IncludeBase<AbstractFieldConfigurationBase, IFieldGenerator>()
                .Include<FieldConfigurationKeyCalculator, FieldGeneratorKeyCalculator>()
                .Include<FieldConfigurationComposite, FieldGeneratorComposite>()
                .Include<FieldConfigurationAggregate, FieldGeneratorAggregate>();

            CreateMap(typeof(AbstractFieldConfigurationCollectionBase), typeof(AbstractFieldGeneratorFromListBase<,>));

            CreateMap(typeof(AbstractFieldConfigurationCollectionBase), typeof(IFieldGeneratorFromList<>))
                .IncludeBase(typeof(AbstractFieldConfigurationBase), typeof(IFieldGenerator))
                .Include(typeof(FieldConfigurationWeightedList), typeof(FieldGeneratorWeightedList));

            ////Derived types based upon FieldConfigurationBase
            CreateMap<FieldConfigurationRegex, FieldGeneratorRegex>();
            CreateMap<FieldConfigurationConstant, FieldGeneratorConstant>();
            CreateMap<FieldConfigurationNumeric, FieldGeneratorNumeric>();
            CreateMap<FieldConfigurationDate, FieldGeneratorDate>();
            CreateMap<FieldConfigurationIncrement, FieldGeneratorIncrement>();

            ////Derived types based upon FieldConfigurationDependantBase
            CreateMap<FieldConfigurationKeyCalculator, FieldGeneratorKeyCalculator>();
            CreateMap<FieldConfigurationComposite, FieldGeneratorComposite>();
            CreateMap<FieldConfigurationAggregate, FieldGeneratorAggregate>();

            ////Derived types based upon FieldConfigurationCollectionBase
            CreateMap<FieldConfigurationWeightedList, FieldGeneratorWeightedList>()
                .ConstructUsing((src, ctx) =>
                {
                    var list = ctx.Mapper.Map<IEnumerable<WeightedValue>>(src.List);
                    return new FieldGeneratorWeightedList(src.Name, list);
                })
                .ForMember(dest => dest.List, opt => opt.MapFrom<WeightedListResolver>());
        }
    }
}
