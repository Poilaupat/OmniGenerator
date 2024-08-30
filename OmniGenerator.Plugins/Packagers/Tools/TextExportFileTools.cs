using OmniGenerator.Lib.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Packagers.Tools
{
    public static class TextExportFileTools
    {
        /// <summary>
        /// Procuces a human readable file containing data of the documents
        /// </summary>
        /// <param name="root">The root containing the documents</param>
        /// <returns>The lines of the file</returns>
        /// <exception cref="NotSupportedException">Thrown if an unknown document is found</exception>
        public static IEnumerable<string> GetDefaultTextFileContent(Root root)
        {
            yield return $"00 {DateTime.Now:yyyyMMddHHmmss} {root.Fields["numlot"].Value}";

            foreach (var document in root.GetDocuments(true))
            {
                if (document.Fields is not null)
                {
                    yield return document.Name switch
                    {
                        "slip" => $"{document.Fields["encline"].Value} {document.Id}",
                        "talon-optique" => $"{document.Fields["encline"].Value} {document.Id}",
                        "cheque" => $"{document.Fields["encline"].Value} {document.Id}",
                        _ => throw new NotSupportedException("{document.Name} was an unexpected document type"),
                    };
                }
            }
        }
    }
}
