using Microsoft.ProgramSynthesis.Utils.JetBrains.Annotations;
using OmniGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    public class HierarchyBuilderProgressReport : IProgressReport
    {
        public long CountGroup { get; set; } 
        public long CountProcessedGroup { get; set; }
        public long CountDocument { get; set; }
        public long CountProcessedDocument { get; set; }
        public long CountField { get; set; }

        public string[] GetTextReport()
        {
            return new string[]
            {
                $"Builded {CountProcessedGroup}/{CountGroup} groups",
                $"Builded {CountProcessedDocument}/{CountDocument} documents",
                $"Generated {CountField} fields",
            };
        }
    }
}
