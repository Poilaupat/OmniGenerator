using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Configuration.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Configuration
{
    /// <summary>
    /// Provides functionality for reading and validating OmniGenerator configuration files.
    /// Supports loading hierarchical configurations with external field definition files.
    /// </summary>
    public static class ConfigurationReader
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            TypeInfoResolver = new PolymorphicTypeResolver(),
            Converters = { new JsonStringEnumConverter() },
        };

        /// <summary>
        /// Reads and parses an OmniGenerator configuration file asynchronously.
        /// Automatically loads any external field configuration files referenced in the main configuration.
        /// </summary>
        /// <param name="filePath">The absolute or relative path to the main configuration file.</param>
        /// <returns>A fully populated <see cref="OmniGeneratorConfiguration"/> object with all field configurations merged.</returns>
        /// <exception cref="ConfigurationException">
        /// Thrown when the configuration file cannot be read, parsed, or contains invalid data.
        /// </exception>
        /// <remarks>
        /// This method performs the following operations:
        /// <list type="number">
        /// <item>Deserializes the main configuration file.</item>
        /// <item>Loads root-level field configurations if specified.</item>
        /// <item>Recursively loads field configurations for all hierarchy elements (groups and documents).</item>
        /// </list>
        /// </remarks>
        public static async Task<OmniGeneratorConfiguration> ReadConfigurationAsync(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            var config = await ConfigurationReader.DeserializeAsync<OmniGeneratorConfiguration>(filePath);

            if (!string.IsNullOrWhiteSpace(config.Hierarchy.FieldConfigurationFile))
            {
                var root = await ConfigurationReader.DeserializeAsync<OmniGeneratorFieldConfiguration>(directory ?? string.Empty, config.Hierarchy.FieldConfigurationFile);
                config.Hierarchy.Fields.Merge(root.Fields);
            }

            foreach (var configElement in config.Hierarchy.Root.GetElementsConfiguration(true))
            {
                if (!string.IsNullOrWhiteSpace(configElement.FieldConfigurationFile))
                {
                    var file = await ConfigurationReader.DeserializeAsync<OmniGeneratorFieldConfiguration>(directory ?? string.Empty, configElement.FieldConfigurationFile);
                    configElement.Fields.Merge(file.Fields);
                }
            }

            return config;
        }

        /// <summary>
        /// Deserializes a JSON file into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="filepath">The absolute path to the JSON file.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ConfigurationException">
        /// Thrown when the file cannot be found, read, or the JSON deserialization fails.
        /// </exception>
        private static async Task<T> DeserializeAsync<T>(string filepath)
        {
            try
            {
                string json = await File.ReadAllTextAsync(filepath);
                var config = JsonSerializer.Deserialize<T>(json, _options);

                if (config is not null)
                {
                    return (T)config;
                }

                throw new ConfigurationException($"The reading of the file '{filepath}' returned a null param object");
            }
            catch (ConfigurationException)
            {
                throw;
            }
            catch (FileNotFoundException)
            {
                throw new ConfigurationException($"Param file {filepath} was not found");
            }
            catch (JsonException ex)
            {
                throw new ConfigurationException($"Json deserialization failed for file {filepath}", ex);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException($"An error occured while reading param file {filepath}", ex);
            }
        }

        /// <summary>
        /// Deserializes a JSON file into an object of type <typeparamref name="T"/>.
        /// Supports both absolute and relative file paths.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="directory">The base directory to resolve relative paths. Used when <paramref name="filename"/> is not an absolute path.</param>
        /// <param name="filename">The filename or absolute path to the JSON file.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ConfigurationException">
        /// Thrown when the file cannot be found in either the specified absolute path or relative to the directory.
        /// </exception>
        /// <remarks>
        /// The method first checks if <paramref name="filename"/> is an absolute path and the file exists.
        /// If not, it attempts to locate the file relative to the specified <paramref name="directory"/>.
        /// </remarks>
        private static async Task<T> DeserializeAsync<T>(string directory, string filename)
        {
            if (Path.IsPathRooted(filename) && File.Exists(filename))
            {
                return await DeserializeAsync<T>(filename);
            }

            string filepath = Path.Combine(directory, filename);
            if (File.Exists(filepath))
            {
                return await DeserializeAsync<T>(filepath);
            }

            throw new ConfigurationException($"The file {filename} was not found.");
        }

        /// <summary>
        /// Validates an OmniGenerator configuration against business rules and constraints.
        /// </summary>
        /// <param name="config">The configuration to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="config"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ConfigurationException">
        /// Thrown when the configuration violates one or more validation rules.
        /// The exception contains a collection of all validation errors found.
        /// </exception>
        /// <remarks>
        /// The following validation rules are enforced:
        /// <list type="bullet">
        /// <item>Documents cannot contain aggregate fields.</item>
        /// <item>The name "omni.generator.hierarchy" is reserved and cannot be used for hierarchy elements.</item>
        /// </list>
        /// All validation errors are collected before throwing the exception, allowing the caller
        /// to view all issues at once rather than one at a time.
        /// </remarks>
        public static void CheckConfiguration(OmniGeneratorConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config, nameof(config));

            ConfigurationException exception = new("The provided parameter file in not valid");

            // Check : Document should not have aggregate fields
            var documents = config
                .Hierarchy
                .Root
                .GetDocumentsConfiguration(true);

            foreach (var document in documents)
            {
                foreach (var field in document
                    .Fields
                    .Where(x => x.GetType() == typeof(FieldConfigurationAggregate) || x.GetType().IsSubclassOf(typeof(FieldConfigurationAggregate))))
                {
                    exception.Errors.Add($"Document : {document.Name} / Field : {field.Name} / Error : Documents cannot have aggreate fields");
                }
            }

            //Check : omni.generator.hierarchy is a reserved name
            if (config.Hierarchy.Root.GetElementsConfiguration(true).Any(e => e.Name.Equals("omni.generator.hierarchy")))
                exception.Errors.Add("omni.generator.hierarchy is a reserved name");

            // Check : error simulation rules are valid
            CheckErrorSimulations(config, documents, exception);

            if (exception.Errors.Count > 0)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Validates the centralized error simulation configuration, accumulating any error into <paramref name="exception"/>.
        /// </summary>
        /// <param name="config">The configuration being validated.</param>
        /// <param name="documents">The documents declared in the hierarchy.</param>
        /// <param name="exception">The exception accumulating validation errors.</param>
        private static void CheckErrorSimulations(
            OmniGeneratorConfiguration config,
            IEnumerable<DocumentConfiguration> documents,
            ConfigurationException exception)
        {
            var errorSimulations = config.Hierarchy.ErrorSimulations;
            if (errorSimulations is null)
                return;

            var documentsByName = documents
                .GroupBy(d => d.Name)
                .ToDictionary(g => g.Key, g => g.First());

            for (int i = 0; i < errorSimulations.Rules.Count; i++)
            {
                var rule = errorSimulations.Rules[i];
                var prefix = $"Error simulation rule #{i + 1}";

                if (string.IsNullOrWhiteSpace(rule.TargetDocument))
                    exception.Errors.Add($"{prefix} : target-document is required");

                if (string.IsNullOrWhiteSpace(rule.TargetField))
                    exception.Errors.Add($"{prefix} : target-field is required");

                if (rule.Probability < 0 || rule.Probability > 1)
                    exception.Errors.Add($"{prefix} : probability {rule.Probability} must be within [0, 1]");

                if (string.IsNullOrWhiteSpace(rule.TargetDocument))
                    continue;

                if (!documentsByName.TryGetValue(rule.TargetDocument, out var document))
                {
                    exception.Errors.Add($"{prefix} : target-document '{rule.TargetDocument}' does not exist");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(rule.TargetField)
                    && !document.Fields.Any(f => f.Name.Equals(rule.TargetField)))
                {
                    exception.Errors.Add($"{prefix} : target-field '{rule.TargetField}' does not exist on document '{rule.TargetDocument}'");
                }
            }
        }
    }
}
