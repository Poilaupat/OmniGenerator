using Spectre.Console.Cli;
using System.ComponentModel;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommandSettings : GenerateCommandSettingsBase
    {
        /// <summary>
        /// Gets or sets the path to the schedule plan JSON file.
        /// The file must contain a <see cref="SchedulePlan"/> object with one or more <see cref="ScheduledJobEntry"/> items.
        /// </summary>
        [Description("The path to the schedule plan JSON file")]
        [CommandArgument(0, "<SchedulePlanFile>")]
        public string SchedulePlanFilePath { get; set; } = string.Empty;
    }
}
