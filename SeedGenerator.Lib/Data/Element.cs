using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Data
{
    public class Element
    {
        public string Type { get; }

        public long Id { get; set; }

        public string Name { get; set; }

        public FieldCollection Fields { get; set; } = new FieldCollection();

        public Element(string type, string name, long id) 
        {
            Type = type;
            Name = name;
            Id = id;
        }
    }
}