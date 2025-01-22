using CommandLine;
using CommandLine.Text;
using OmniGenerator.Lib.Exceptions;

namespace OmniGenerator.Cli.Options
{
    public sealed class DefaultOptions : ICommandLineOptions
    {
        [Option('p', "param", Required = true, HelpText = "The path of the json param file describing what should be generated")]
        public required string ParamFilePath { get; set; }

        [Option('o', "output", Required = true, HelpText = "The path of the folder where files will be generated")]
        public required string OutputFolderPath { get; set; }
    }
}
