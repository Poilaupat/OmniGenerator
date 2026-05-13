using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Reporting
{
    public sealed class JobExecutionStats
    {
        public DateTime StartTime { get; set; }
        public int BatchCount { get; set; }
        public int DocumentCount { get; set; }
        public int GroupCount { get; set; }
        public int FieldCount { get; set; }
    }
}
