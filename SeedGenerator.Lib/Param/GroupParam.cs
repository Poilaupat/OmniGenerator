using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class GroupParam : ElementParam
    {
        [JsonPropertyName("elements")]
        public List<ElementParam> Elements { get; set; } = new List<ElementParam>();

        public IEnumerable<DocumentParam> GetAllDocuments()
        {
            foreach (var doc in Elements.Where(x => x is DocumentParam))
            {
                yield return (DocumentParam)doc;
            }

            foreach (var doc in Elements
                .Where(x => x is GroupParam)
                .Cast<GroupParam>()
                .SelectMany(x => x.GetAllDocuments()))
            {
                yield return doc;
            }
        }
    }
}
