using SeedGenerator.Lib.DataGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.DataGenerators
{
    internal class FieldGeneratorDate : FieldGeneratorBase
    {
        public int DayDiffMin { get; set; }
        public int DayDiffMax { get; set; }

        public FieldGeneratorDate(string name, int dayDiffMin, int dayDiffMax) : base(name)
        {
            DayDiffMin = dayDiffMin;
            DayDiffMax = dayDiffMax;
        }

        public override string NextValue()
        {
            int diff = new Random().Next(DayDiffMin, DayDiffMax);
            return DateTime.Today.AddDays(-diff).ToString("dd/MM/yyyy");
        }
    }
}
