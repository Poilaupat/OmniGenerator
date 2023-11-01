using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mustache;

namespace SeedGenerator.Lib.Data.Fields.Generators
{
    internal class FieldGeneratorComposite : FieldGeneratorDependantBase
    {
        public string Format { get; set; }

        public FieldGeneratorComposite(string name, string dependantUpon, string format) 
            : base(name, dependantUpon)
        {
            Format = format;
        }

        public override string NextValue()
        {
            FormatCompiler compiler = new FormatCompiler();
            Generator generator = compiler.Compile(Format);
            string result = generator.Render(Dependances);

            return result;
        }
    }
}
