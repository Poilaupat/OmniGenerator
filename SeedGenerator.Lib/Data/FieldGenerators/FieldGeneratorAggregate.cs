namespace SeedGenerator.Lib.Data.FieldGenerators
{
    internal class FieldGeneratorAggregate : AbstractFieldGeneratorOneFieldDependant
    {
        public enum EFFieldAggregateType
        {
            Count,
            Sum,
        }

        public enum EScope
        {
            Overall,
            DirectParent,
        }

        public EFFieldAggregateType AggregateType { get; set; }
        public EScope Scope { get; set; }
        public string TargetDocument { get; set; }
        public Group? Group { get; set; }

        public FieldGeneratorAggregate(string name, string dependantUpon, EFFieldAggregateType aggregateType, EScope scope, string targetDocument)
            : base(name, dependantUpon)
        {
            AggregateType = aggregateType;
            Scope = scope;
            TargetDocument = targetDocument;
        }

        public override string NextValue()
        {
            return AggregateType switch
            {
                EFFieldAggregateType.Count => ComputeCountAggregate(),
                EFFieldAggregateType.Sum => ComputeSumAggregate(),
                _ => string.Empty,
            };
        }

        private string ComputeSumAggregate()
        {
            if (Group is null)
            {
                throw new NullReferenceException($"{nameof(Group)} property must be set before generating value");
            }

            return Group
                .GetDocuments(TargetDocument, Scope == EScope.Overall)
                .Sum(x => Convert.ToDouble(x.Fields[Dependances.Single().Key].Value))
                .ToString();
        }

        private string ComputeCountAggregate()
        {
            if (Group is null)
            {
                throw new NullReferenceException($"{nameof(Group)} property must be set before generating value");
            }

            return Group
                .GetDocuments(TargetDocument, Scope == EScope.Overall)
                .Count()
                .ToString();
        }
    }
}
