using AutoMapper;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Tools;
using System.Collections.Generic;

namespace OmniGenerator.Lib.AutoMapper
{
    /// <summary>
    /// Resolves a list of WeightedValue from configuration and/or file
    /// </summary>
    internal class WeightedListResolver : IValueResolver<FieldConfigurationWeightedList, object, IEnumerable<WeightedValue>>
    {
        private readonly WeightedValueFileLoader _fileLoader = new();

        public IEnumerable<WeightedValue> Resolve(
            FieldConfigurationWeightedList source,
            object destination,
            IEnumerable<WeightedValue> destMember,
            ResolutionContext context)
        {
            return ListResolver.ResolveList(source.List, source.ListFilePath, _fileLoader);
        }
    }
}
