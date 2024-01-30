namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorAggregate : AbstractFieldGeneratorOneFieldDependant
    {
        public EFFieldAggregateType AggregateType { get; set; }
        public EScope Scope { get; set; }
        public string TargetElement { get; set; }
        public Group? Group { get; set; }

        public FieldGeneratorAggregate(string name, string dependentUpon, EFFieldAggregateType aggregateType, EScope scope, string targetElement)
            : base(name, dependentUpon)
        {
            AggregateType = aggregateType;
            Scope = scope;
            TargetElement = targetElement;
        }

        protected override object NextValue()
        {
            if (Group is null)
            {
                throw new NullReferenceException($"{nameof(Group)} property must be set before generating value");
            }

            return AggregateType switch
            {
                EFFieldAggregateType.Count => ComputeCountAggregate(),
                EFFieldAggregateType.Sum => ComputeSumAggregate(),
                _ => string.Empty,
            };
        }

        private string ComputeSumAggregate()
        {
            return Group
                ?.GetElements(TargetElement, Scope == EScope.Overall)
                .Sum(x => Convert.ToDouble(x.Fields[DependenceNames.Single()].Value))
                .ToString() ?? string.Empty;
        }

        private string ComputeCountAggregate()
        {
            return Group
                ?.GetElements(TargetElement, Scope == EScope.Overall)
                .Count()
                .ToString() ?? string.Empty;
        }
    }
}
