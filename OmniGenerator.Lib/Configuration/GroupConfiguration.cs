using OmniGenerator.Lib.Configuration.Fields;
using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Configuration
{
    public class GroupConfiguration : ElementConfiguration
    {
        [JsonPropertyName("elements")]
        public List<ElementConfiguration> Elements { get; set; } = new List<ElementConfiguration>();

        private IEnumerable<T> GetElementsConfiguration<T>(bool recursive)
            where T : ElementConfiguration
        {
            foreach (var element in Elements.Where(x => x is T))
            {
                yield return (T)element;
            }

            if (recursive)
            {
                foreach (var element in Elements
                    .Where(x => x is GroupConfiguration)
                    .Cast<GroupConfiguration>()
                    .SelectMany(x => x.GetElementsConfiguration<T>(recursive)))
                {
                    yield return element;
                }
            }
        }

        public IEnumerable<ElementConfiguration> GetElementsConfiguration(bool recursive)
        {
            return GetElementsConfiguration<ElementConfiguration>(recursive);
        }

        public IEnumerable<DocumentConfiguration> GetDocumentsConfiguration(bool recursive)
        {
            return GetElementsConfiguration<DocumentConfiguration>(recursive);
        }

        public IEnumerable<GroupConfiguration> GetGroupsConfiguration(bool recursive)
        {
            return GetElementsConfiguration<GroupConfiguration>(recursive);
        }

        public IEnumerable<GroupConfiguration> GetGroupsAndSelfConfiguration(bool recursive)
        {
            yield return this;

            foreach (var subgroup in GetElementsConfiguration<GroupConfiguration>(recursive))
            {
                yield return subgroup;
            }
        }
    }
}
