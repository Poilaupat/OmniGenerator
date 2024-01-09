using SeedGenerator.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Plugins.Packagers
{
    public class DebugPackagerBase
    {
        protected virtual IEnumerable<string> GetTxtFileContent(PacketData packet)
        {
            yield return $"00 {DateTime.Now:yyyyMMddHHmmss} {packet.Fields["numlot"].Value}";

            foreach (var document in packet.Documents)
            {
                if (document.Fields is not null)
                {
                    yield return document.Name switch
                    {
                        "slip" => $"{document.Fields["encline"].Value} {document.Fields["numremise"].Value}",
                        "coupon" => $"{document.Fields["encline"].Value} {document.Fields["numcoupon"].Value}",
                        "cheque" => $"{document.Fields["encline"].Value} {document.Fields["cmc7"].Value}",
                        _ => throw new Exception("Unexpected document type"),
                    };
                }
            }
        }
    }
}
