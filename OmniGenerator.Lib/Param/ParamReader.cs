using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Param.FieldParams;
using OmniGenerator.Lib.Param.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Param
{
    public class ParamReader
    {
        public static async Task<RootParam> ReadParamAsync(string paramFilePath)
        {
            try
            {
                var param = await ParamReader.DeserializeParamAsync<RootParam>(paramFilePath);

                foreach (var elementParam in param.RootGroupParam.GetElementParams(true))
                {
                    if (!string.IsNullOrWhiteSpace(elementParam.FieldConfigurationFile))
                    {
                        var directory = Path.GetDirectoryName(paramFilePath);
                        var filefields = await ParamReader.DeserializeParamFileAsync<List<FieldParamBase>>(directory, elementParam.FieldConfigurationFile);
                        elementParam.MergeFields(filefields);
                    }
                }

                return param;
            }
            catch (ParamException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static async Task<T> DeserializeParamAsync<T>(string filepath)
        {
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            try
            {
                string json = await File.ReadAllTextAsync(filepath);
                var param = JsonSerializer.Deserialize<T>(json, options);

                if (param is null)
                {
                    throw new ParamException($"The reading of the file '{filepath}' returned a null param object");
                }

                return (T)param;
            }
            catch (ParamException)
            {
                throw;
            }
            catch (FileNotFoundException)
            {
                throw new ParamException($"Param file {filepath} was not found");
            }
            catch (JsonException ex)
            {
                throw new ParamException($"Json deserialization failed for file {filepath}", ex);
            }
            catch (Exception ex)
            {
                throw new ParamException($"An error occured while reading param file {filepath}", ex);
            }
        }

        private static async Task<T> DeserializeParamFileAsync<T>(string? directory, string filename)
        {
            if (Directory.Exists(directory))
            {
                string filepath = Path.Combine(directory, filename);
                return await DeserializeParamAsync<T>(filepath);
            }

            throw new ParamException($"The directory {directory} was not found.");
        }

        public static void CheckParam(RootParam rootParam)
        {
            ParamException exception = new ParamException("The provided parameter file in not valid");

            var documents = rootParam
                .RootGroupParam
                .GetDocumentParams(true);

            foreach (var document in documents)
            {
                foreach (var field in document
                    .Fields
                    .Where(x => x.GetType() == typeof(FieldParamAggregate) || x.GetType().IsSubclassOf(typeof(FieldParamAggregate))))
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
