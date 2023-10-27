using SeedGenerator.Lib.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib
{
    public class Packet
    {
        public FieldCollection Fields { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();
    }
}
