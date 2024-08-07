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
    public class ConfigurationReader
    {
        public static async Task<OmniGeneratorConfiguration> ReadConfigurationAsync(string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                var config = await ConfigurationReader.DeserializeAsync<OmniGeneratorConfiguration>(filePath);

                if(!string.IsNullOrWhiteSpace(config.Root.FieldConfigurationFile))
                {
                    var rootfields = await ConfigurationReader.DeserializeAsync<List<AbstractFieldConfigurationBase>>(directory, config.Root.FieldConfigurationFile);
                    config.Root.Fields.Merge(rootfields);
                }

                foreach (var configElement in config.Root.Group.GetElementsConfiguration(true))
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
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            try
            {
                string json = await File.ReadAllTextAsync(filepath);
                var config = JsonSerializer.Deserialize<T>(json, options);

                if (config is null)
                {
                    throw new ConfigurationException($"The reading of the file '{filepath}' returned a null param object");
                }

                return (T)config;
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
            ConfigurationException exception = new ConfigurationException("The provided parameter file in not valid");

            var documents = config
                .Root
                .Group
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

            if (exception.Errors.Count > 0)
            {
                throw exception;
            }
        }
    }
}
