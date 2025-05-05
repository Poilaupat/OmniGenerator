using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    internal class GenerateCommandSettings : CommandSettings
    {
        public GenerateCommandSettings(string settingsFilePath, string outputFolderPath)
        {
            SettingsFilePath = settingsFilePath;
            OutputFolderPath = outputFolderPath;
        }

        [Description("The path of the generation setting file")]
        [CommandArgument(0, "<settings-file>")]
        public string SettingsFilePath { get; set; }

        [Description("The path of the output folder")]
        [CommandArgument(1, "<output-folder>")]
        public string OutputFolderPath { get; set; }

        public override ValidationResult Validate()
        {
            if(!File.Exists(SettingsFilePath))
            {
                return ValidationResult.Error($"The file {SettingsFilePath} was not found");
            }

            if(File.Exists(OutputFolderPath))
            {
                return ValidationResult.Error($"The output-folder must be a directory");
            }

            if(!Directory.Exists(OutputFolderPath))
            {
                Directory.CreateDirectory(OutputFolderPath);
            }

            return ValidationResult.Success();
        }
    }
}
