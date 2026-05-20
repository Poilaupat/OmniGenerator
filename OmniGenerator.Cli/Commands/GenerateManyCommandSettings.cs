using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class GenerateManyCommandSettings : GenerateCommandSettingsBase
    {
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Gets or sets the path to the schedule plan JSON file.
        /// The file must contain a <see cref="SchedulePlan"/> object with one or more <see cref="ScheduledJobEntry"/> items.
        /// </summary>
        [Description("The path to the schedule plan JSON file")]
        [CommandArgument(0, "<SchedulePlanFile>")]
        public string SchedulePlanFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Validates the schedule plan file path and each job entry's settings file and output folder.
        /// </summary>
        public override ValidationResult Validate()
        {
            if (!File.Exists(SchedulePlanFilePath))
            {
                return ValidationResult.Error($"The schedule plan file '{SchedulePlanFilePath}' was not found");
            }

            SchedulePlan plan;
            try
            {
                var json = File.ReadAllText(SchedulePlanFilePath);
                plan = JsonSerializer.Deserialize<SchedulePlan>(json, _jsonOptions)
                    ?? throw new InvalidOperationException("Deserialization returned null.");
            }
            catch (Exception ex)
            {
                return ValidationResult.Error($"Failed to read schedule plan '{SchedulePlanFilePath}': {ex.Message}");
            }

            foreach (var entry in plan.Jobs)
            {
                var result = ValidateSettingsAndOutputFolder(entry.SettingsFilePath, entry.OutputFolderPath, entry.Name);
                if (!result.Successful)
                    return result;
            }

            return ValidationResult.Success();
        }
    }
}
