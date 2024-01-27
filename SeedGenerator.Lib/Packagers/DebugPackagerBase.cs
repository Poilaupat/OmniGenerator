using SeedGenerator.Lib.Data;

namespace SeedGenerator.Lib.Packagers
{
    public class DebugPackagerBase
    {
        protected virtual IEnumerable<string> GetTxtFileContent(Root root)
        {
            yield return $"00 {DateTime.Now:yyyyMMddHHmmss} {root.Fields["numlot"].Value}";

            foreach (var document in root.GetDocuments(true))
            {
                if (document.Fields is not null)
                {
                    yield return document.Name switch
                    {
                        "slip" => $"{document.Fields["encline"].Value} {document.Id}",
                        "coupon" => $"{document.Fields["encline"].Value} {document.Id}",
                        "cheque" => $"{document.Fields["encline"].Value} {document.Id}",
                        _ => throw new Exception("Unexpected document type"),
                    };
                }
            }
        }
    }
}
