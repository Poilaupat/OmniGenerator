using Microsoft.ProgramSynthesis.Utils.JetBrains.Annotations;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommandSettings(string settingsFilePath, string outputFolderPath, string cronSchedule) : GenerateCommandSettingsBase
    {
        /// <summary>
        /// Gets or sets the path to the generation settings file.
        /// </summary>
        [Description("The path of the generation setting file")]
        [CommandArgument(0, "<SettingsFilePath>")]
        public string SettingsFilePath { get; set; } = settingsFilePath;

        /// <summary>
        /// Gets or sets the path to the output folder where generated content will be saved.
        /// </summary>
        [Description("The path of the output folder")]
        [CommandArgument(1, "<OutputFolder>")]
        public string OutputFolderPath { get; set; } = outputFolderPath;

        /// <summary>
        /// Gets or sets the cron schedule for the generation task, which determines when the task will be executed.
        /// </summary>
        [Description("The cron schedule for the generation task")]
        [CommandArgument(2, "<CronSchedule>")]
        public string CronSchedule { get; set; } = cronSchedule;
    }
}
