using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents the command-line settings for the "generate" command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GenerateCommandSettings"/> class.
    /// </remarks>
    /// <param name="settingsFilePath">The path to the settings file used for generation.</param>
    /// <param name="outputFolderPath">The path to the output directory.</param>
    internal class GenerateCommandSettings(string settingsFilePath, string outputFolderPath) : CommandSettings
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
        /// Validates the provided command-line arguments.
        /// Ensures that the settings file exists and that the output path is a directory.
        /// </summary>
        /// <returns>A <see cref="ValidationResult"/> indicating success or a validation error.</returns>
        public override ValidationResult Validate()
        {
            if (!File.Exists(SettingsFilePath))
            {
                return ValidationResult.Error($"The file {SettingsFilePath} was not found");
            }

            if (File.Exists(OutputFolderPath))
            {
                return ValidationResult.Error("The output-folder must be a directory");
            }

            if (!Directory.Exists(OutputFolderPath))
            {
                Directory.CreateDirectory(OutputFolderPath);
            }

            return ValidationResult.Success();
        }
    }
}
