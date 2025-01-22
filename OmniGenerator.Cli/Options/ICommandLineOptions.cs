using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Options
{
    public interface ICommandLineOptions
    {
        public string ParamFilePath { get; set; }

        public string OutputFolderPath { get; set; }
    }
}
