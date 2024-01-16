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
        public static async Task<RootParam> GetParamFromFileAsync(string filepath)
        {
            var options = new JsonSerializerOptions
            {
                //WriteIndented = true,
                //Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            try
            {
                string json = await File.ReadAllTextAsync(filepath);
                var param = JsonSerializer.Deserialize<RootParam>(json, options);

                if (param is null)
                {
                    throw new ParamException($"The reading of the file '{filepath}' returned a null param object");
                }

                CheckParam(param);

                return (RootParam)param;
            }
            catch (ParamException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ParamException($"An error occured while reading param file {filepath}", ex);
            }
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
                    .FieldParams
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
