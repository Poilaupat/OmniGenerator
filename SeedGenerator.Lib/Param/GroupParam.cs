using Microsoft.ProgramSynthesis;
using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    internal class GroupParam : ElementParam
    {
        [JsonPropertyName("elements")]
        public List<ElementParam> Elements { get; set; } = new List<ElementParam>();

        [JsonPropertyName("fields")]
        public List<FieldParamBase> FieldParams { get; set; } = new List<FieldParamBase> { };

        public IEnumerable<DocumentParam> GetAllDocumentParams()
        {
            foreach (var doc in Elements.Where(x => x is DocumentParam))
            {
                yield return (DocumentParam)doc;
            }

            foreach (var doc in Elements
                .Where(x => x is GroupParam)
                .Cast<GroupParam>()
                .SelectMany(x => x.GetAllDocumentParams()))
            {
                yield return doc;
            }
        }

        public IEnumerable<GroupParam> GetAllGroupParams()
        {
            foreach(var grp in Elements.Where(x => x is GroupParam))
            {
                yield return (GroupParam)grp;
            }

            foreach (var grp in Elements
                .Where(x => x is GroupParam)
                .Cast<GroupParam>()
                .SelectMany(x => x.GetAllGroupParams()))
            {
                yield return grp;
            }
        }
    }
}
