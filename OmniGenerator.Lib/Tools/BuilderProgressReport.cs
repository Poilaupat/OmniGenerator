using Microsoft.ProgramSynthesis.Utils.JetBrains.Annotations;
using OmniGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    public class BuilderProgressReport : IProgressReport
    {
        public long CountGroup { get; set; } 
        public long CountProcessedGroup { get; set; }
        public long CountDocument { get; set; }
        public long CountProcessedDocument { get; set; }
        public long CountField { get; set; }

        public DateTime? StartHierarchyBuild { get; set; }
        public DateTime? EndHierarchyBuild { get; set; }
        public DateTime? StartFieldGeneration { get; set; }
        public DateTime? EndFieldGeneration { get; set; }
        public DateTime? StartAggregateFieldGeneration { get; set; }
        public DateTime? EndAggregateFieldGeneration { get; set; }

        public TimeSpan ElapsedHierarchyBuild => StartHierarchyBuild is null ? default : (EndHierarchyBuild ?? DateTime.Now) - (DateTime)StartHierarchyBuild;
        public TimeSpan ElapsedFieldGeneration => StartFieldGeneration is null ? default : (EndFieldGeneration ?? DateTime.Now) - (DateTime)StartFieldGeneration;
        public TimeSpan ElapsedAggregateFieldGeneration => StartAggregateFieldGeneration is null ? default : (EndAggregateFieldGeneration ?? DateTime.Now) - (DateTime)StartAggregateFieldGeneration;

        public string[] GetTextReport()
        {
            return new string[]
            {
                $"Builded {CountProcessedGroup}/{CountGroup} groups ({(double)CountProcessedGroup/(double)(ElapsedHierarchyBuild.TotalSeconds)} g/s)",
                $"Builded {CountProcessedDocument}/{CountDocument} documents ({(double)CountProcessedDocument/(double)(ElapsedHierarchyBuild.TotalSeconds)} d/s)",
                $"Generated {CountField} fields",
                $"Elapsed time : {ElapsedHierarchyBuild.ToFluidUnitString()}",
            };
        }
    }
}
