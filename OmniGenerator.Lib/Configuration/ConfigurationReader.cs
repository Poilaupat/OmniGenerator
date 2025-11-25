using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Configuration.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using OmniGenerator.Lib.Tools;

namespace OmniGenerator.Lib.Configuration
{
    public static class ConfigurationReader
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            TypeInfoResolver = new PolymorphicTypeResolver(),
        };

        public static async Task<OmniGeneratorConfiguration> ReadConfigurationAsync(string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                var config = await ConfigurationReader.DeserializeAsync<OmniGeneratorConfiguration>(filePath);

                if (!string.IsNullOrWhiteSpace(config.Hierarchy.FieldConfigurationFile))
                {
                    var rootfields = await ConfigurationReader.DeserializeAsync<List<AbstractFieldConfigurationBase>>(directory, config.Hierarchy.FieldConfigurationFile);
                    config.Hierarchy.Fields.Merge(rootfields);
                }

                foreach (var configElement in config.Hierarchy.Root.GetElementsConfiguration(true))
                {
                    if (!string.IsNullOrWhiteSpace(configElement.FieldConfigurationFile))
                    {
                        var filefields = await ConfigurationReader.DeserializeAsync<List<AbstractFieldConfigurationBase>>(directory, configElement.FieldConfigurationFile);
                        configElement.Fields.Merge(filefields);
                    }
                }

                return config;
            }
            catch (ConfigurationException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

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

        private static async Task<T> DeserializeAsync<T>(string? directory, string filename)
        {
            if (Directory.Exists(directory))
            {
                string filepath = Path.Combine(directory, filename);
                return await DeserializeAsync<T>(filepath);
            }

            throw new ConfigurationException($"The directory {directory} was not found.");
        }

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

            if (exception.Errors.Count > 0)
            {
                throw exception;
            }
        }
    }
}
