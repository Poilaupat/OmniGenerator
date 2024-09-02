using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    public class ProgressReport
    {
        public string? Topic { get; set; }

        public long TotalGroups { get; set; } 
        public long TotalDocuments { get; set; } 
        public long ProcessedGroups { get; set; } 
        public long ProcessedDocuments { get; set; }

        public override string ToString()
        {
            return $"{Topic} Groups {ProcessedGroups}/{TotalGroups} Documents {ProcessedDocuments}/{TotalDocuments}";
        }
    }
}
