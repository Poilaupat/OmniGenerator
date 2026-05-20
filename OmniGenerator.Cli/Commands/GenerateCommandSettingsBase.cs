using Spectre.Console;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    internal class GenerateCommandSettingsBase : CommandSettings
    {
        /// <summary>
        /// Validates that the settings file exists and that the output path is a valid (or creatable) directory.
        /// Creates the output directory if it does not yet exist.
        /// </summary>
        protected static ValidationResult ValidateSettingsAndOutputFolder(string settingsFilePath, string outputFolderPath, string context = "")
        {

            if (!File.Exists(settingsFilePath))
            {
                return ValidationResult.Error($"The file '{settingsFilePath}' was not found ({context})");
            }

            if (File.Exists(outputFolderPath))
            {
                return ValidationResult.Error($"The output folder '{outputFolderPath}' must be a directory, not a file ({context})");
            }

            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            return ValidationResult.Success();
        }
    }
}
