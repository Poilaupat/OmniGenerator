using SeedGenerator.Lib.Exceptions;
using SeedGenerator.Lib.Param.FieldParams;
using SeedGenerator.Lib.Param.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Param
{
    public class ParamTools
    {
        public static async Task<T> ReadParamFromFileAsync<T>(string filepath)
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
            catch(ParamException)
            {
                throw;
            }
            catch(FileNotFoundException)
            {
                throw new ParamException($"Param file {filepath} was not found");
            }
            catch(JsonException ex)
            {
                throw new ParamException($"Json deserialization failed for file {filepath}", ex);
            }
            catch (Exception ex)
            {
                throw new ParamException($"An error occured while reading param file {filepath}", ex);
            }
        }

        public static async Task<T> ReadParamFromFileAsync<T>(string? directory, string filename)
        {
            if(Directory.Exists(directory))
            {
                string filepath = Path.Combine(directory, filename);
                return await ReadParamFromFileAsync<T>(filepath);
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

                if (exception.Errors.Count > 0)
                {
                    throw exception;
                }
            }
        }
    }
}
