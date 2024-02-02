using SeedGenerator.Lib.Param.FieldParams;
using System.Text.Json.Serialization;

namespace SeedGenerator.Lib.Param
{
    public class GroupParam : ElementParam
    {
        [JsonPropertyName("elements")]
        public List<ElementParam> Elements { get; set; } = new List<ElementParam>();

        private IEnumerable<T> GetElementParams<T>(bool recursive) 
            where T : ElementParam 
        {
            foreach (var element in Elements.Where(x => x is T))
            {
                yield return (T)element;
            }

            if (recursive)
            {
                foreach (var element in Elements
                    .Where(x => x is GroupParam)
                    .Cast<GroupParam>()
                    .SelectMany(x => x.GetElementParams<T>(recursive)))
                {
                    yield return element;
                }
            }
        }

        public IEnumerable<DocumentParam> GetDocumentParams(bool recursive)
        {
            return GetElementParams<DocumentParam>(recursive);
        }

        public IEnumerable<GroupParam> GetGroupParams(bool recursive)
        {
            return GetElementParams<GroupParam>(recursive);
        }

        public IEnumerable<GroupParam> GetGroupParamsAndSelf(bool recursive)
        {
            yield return this;

            foreach(var subgroup in GetElementParams<GroupParam>(recursive))
            {
                yield return subgroup;
            }
        }

        public IEnumerable<ElementParam> GetElementParams(bool recursive)
        {
            return GetElementParams<ElementParam>(recursive); 
        }
    }
}
