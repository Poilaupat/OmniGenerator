using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class GroupParam : ElementParam
    {
        [JsonPropertyName("elements")]
        public List<ElementParam> Elements { get; set; } = new List<ElementParam>();

        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase> { };

        public IEnumerable<DocumentParam> GetDocumentParams(bool recursive)
        {
            foreach (var doc in Elements.Where(x => x is DocumentParam))
            {
                yield return (DocumentParam)doc;
            }

            if (recursive)
            {
                foreach (var doc in Elements
                    .Where(x => x is GroupParam)
                    .Cast<GroupParam>()
                    .SelectMany(x => x.GetDocumentParams(recursive)))
                {
                    yield return doc;
                }
            }
        }

        public IEnumerable<GroupParam> GetGroupParams(bool recursive)
        {
            foreach (var grp in Elements.Where(x => x is GroupParam))
            {
                yield return (GroupParam)grp;
            }

            if (recursive)
            {
                foreach (var grp in Elements
                    .Where(x => x is GroupParam)
                    .Cast<GroupParam>()
                    .SelectMany(x => x.GetGroupParams(true)))
                {
                    yield return grp;
                }
            }
        }
    }
}
