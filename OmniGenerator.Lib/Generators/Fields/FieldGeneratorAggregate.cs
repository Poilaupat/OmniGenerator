using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The <see cref="FieldGeneratorAggregate"/> is a kind of generator that computes aggregates on <see cref="Group"/> content
    /// </summary>
    internal class FieldGeneratorAggregate : AbstractFieldGeneratorSingleFieldDependant<int>
    {
        /// <summary>
        /// The aggregate type. Its configures how the aggregate is computed (count, sum, ...)
        /// <see cref="EFFieldAggregateType"/> for complete list 
        /// </summary>
        public EFFieldAggregateType AggregateType { get; set; }

        /// <summary>
        /// The scope determines the perimeter of the aggregate
        /// Can be limited to direct children or all children recursively
        /// </summary>
        public EScope Scope { get; set; }

        /// <summary>
        /// The target of the aggregate. Can be the name of a <see cref="Document"/> or a <see cref="Group"/>
        /// </summary>
        public string TargetElement { get; set; }

        /// <summary>
        /// The group to compute the aggregate on
        /// </summary>
        public Group? Group { get; set; }

        /// <summary>
        /// Creates a new <see cref="FieldGeneratorAggregate"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="dependentUpon">If aggregate type is set to Sum, name of the numeric field to sum. String.Empty either</param>
        /// <param name="aggregateType">The aggregate type. Either Sum or Count</param>
        /// <param name="scope">The scope. Either DirectChildren or Overall</param>
        /// <param name="targetElement">The name of the target element. Either a <see cref="Document"/> name or a <see cref="Group"/> name</param>
        public FieldGeneratorAggregate(string name, string dependentUpon, EFFieldAggregateType aggregateType, EScope scope, string targetElement)
            : base(name, dependentUpon)
        {
            AggregateType = aggregateType;
            Scope = scope;
            TargetElement = targetElement;
        }

        /// <summary>
        /// Generates a new value
        /// If the Group property is not set, throws a  <see cref="NullReferenceException"/>
        /// </summary>
        /// <returns>The aggregate result</returns>
        /// <exception cref="NullReferenceException"></exception>
        protected override int GenerateValue()
        {
            if (Group is null)
            {
                throw new NullReferenceException($"{nameof(Group)} property must be set before generating value");
            }

            return AggregateType switch
            {
                EFFieldAggregateType.Count => ComputeCountAggregate(),
                EFFieldAggregateType.Sum => ComputeSumAggregate(),
                _ => 0,
            };
        }

        /// <summary>
        /// Computes the aggregate result when Aggregate type is set to Sum
        /// </summary>
        /// <returns>The sum of the field of the target element</returns>
        private int ComputeSumAggregate()
        {
            return Group
                ?.GetElements(TargetElement, Scope == EScope.Overall)
                .Sum(x => Convert.ToInt32(x.Fields[DependenceNames.Single()].Value))
                ?? 0;
        }

        /// <summary>
        /// Computes the aggregate result when the aggregate field type is set to Count
        /// </summary>
        /// <returns>The target element count</returns>
        private int ComputeCountAggregate()
        {
            return Group
                ?.GetElements(TargetElement, Scope == EScope.Overall)
                .Count() ?? 0;
        }
    }
}
