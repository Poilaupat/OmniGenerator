using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Renderers;
using System.Drawing;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OmniGenerator.Plugins.Tessi.Packagers.IoT.Postman
{
    [OmniGeneratorPluginMetadata("packager.tessi.iot.postman", "A packager that produces data and image payload for IOT's test postman collection")]
    internal class Postman : OmniGeneratorPluginBase, IPackager
    {
        private readonly IFileSystem _fileSystem;

        public Postman() : this(new PhysicalFileSystem())
        { }

        public Postman(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
        {
            var packagepath = Path.Combine(basepath, $"IOT_Postman_{DateTime.Now:yyyyMMddHHmmss}");

            var batchFields = new BatchFields(root.Fields);
            var filenamebase = $"{batchFields.CodeOrganisation}{batchFields.CodeOrganizationUnit}{batchFields.CodeScanner}{DateTime.Now:yyyyMMddHHmmssfff}";

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            var documents = root
                .GetAllDocuments()
                .ToArray();

            JsonData data = new JsonData();

            string? batchCode = null, remittanceCode = null;
            
            for (var i = 0; i < documents.Count(); i++)
            {
                var filename = $"{filenamebase}{i + 1:000000000}";
                await WriteDocumentImagesAsync(documents[i], packagepath, filename, imageRenderingResolution);


                if (documents[i].Name == "batch-ticket")
                {
                    var fields = new DocumentFields(documents[i].Fields);

                    data.Documents.Add(new JsonDocument
                    {
                        SequenceNumber = i + 1,
                        CaptureDate = "{{batchCaptureDate}}",
                        DocumentType = "BatchBegin",
                        BlackWhiteFront = $"{filename}_bwf.tiff",
                        BlackWhiteRear = $"{filename}_bwb.tiff",
                        GrayScaleFront = $"{filename}_gsf.jpg",
                        Z1 = string.Empty,
                        Z2 = string.Empty.PadRight(32, ' '),
                        Z3 = fields.Z3,
                        Z4 = $"{fields.Z4} ",
                        Dataread = $"<{fields.Z4} <{fields.Z3}>".PadRight(55, ' '),
                    });

                    batchCode = $"{{{{ORG_CODE}}}}{{{{ORG_UNIT_CODE}}}}{{{{CS_CODE}}}}{{{{batchCaptureDate}}}}{i + 1:000000000}";
                }

                if (documents[i].Name == "deposit-slip")
                {
                    var fields = new DocumentFields(documents[i].Fields);

                    data.Documents.Add(new JsonDocument
                    {
                        CaptureDate = "{{remittanceCaptureDate}}",
                        BatchCode = batchCode,
                        DocumentType = "Slip",
                        SequenceNumber = i + 1,
                        BlackWhiteFront = $"{filename}_bwf.tiff",
                        BlackWhiteRear = $"{filename}_bwb.tiff",
                        GrayScaleFront = $"{filename}_gsf.jpg",
                        Z1 = string.Empty.PadRight(18, ' '),
                        Z2 = fields.Z2,
                        Z3 = fields.Z3,
                        Z4 = $"{fields.Z4} ",
                        Dataread = $"<{fields.Z4} <{fields.Z3}> {fields.Z2}:".PadRight(55, ' '),
                    });

                    remittanceCode = $"{{{{ORG_CODE}}}}{{{{ORG_UNIT_CODE}}}}{{{{CS_CODE}}}}{{{{remittanceCaptureDate}}}}{i + 1:000000000}";
                }

                if (documents[i].Name == "cheque")
                {
                    var chequeFields = new DocumentFields(documents[i].Fields);
                    var cmc7 = chequeFields.Dataread.Split(' ');
                    data.Documents.Add(new JsonDocument
                    {
                        CaptureDate = "{{remittanceCaptureDate}}",
                        BatchCode = batchCode,
                        DocumentType = "Check",
                        RemittanceCode = remittanceCode,
                        SequenceNumber = i + 1,
                        BlackWhiteFront = $"{filename}_bwf.tiff",
                        BlackWhiteRear = $"{filename}_bwb.tiff",
                        GrayScaleFront = $"{filename}_gsf.jpg",
                        Z1 = string.Empty.PadRight(18, ' '),
                        Z2 = $" {cmc7[2]}",
                        Z3 = cmc7[1],
                        Z4 = $"{cmc7[0]} ",
                        Dataread = $"<{cmc7[0]} <{cmc7[1]}> {cmc7[2]}>".PadRight(55, ' '),
                    });
                }
            }

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // There is < and > in the dataread field, so we need to use UnsafeRelaxedJsonEscaping to avoid escaping them
            });
            await _fileSystem.WriteAllTextAsync(Path.Combine(packagepath, $"{filenamebase}.json"), json);

            return;
        }

        /// <summary>
        /// Writes the recto and verso images of a document as JPEG and TIFF (Group 4) files to the specified path.
        /// </summary>
        /// <param name="i">The index of the document (used for file naming).</param>
        /// <param name="document">The <see cref="Document"/> whose images are to be written.</param>
        /// <param name="path">The directory path where the images will be saved.</param>
        /// <param name="imageRenderingResolution">The resolution (in DPI) to use when rendering images.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous file writing operation.</returns>
        private async Task WriteDocumentImagesAsync(Document document, string path, string filenamebase, int imageRenderingResolution)
        {
            if (document.RectoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.RectoVectorImage, imageRenderingResolution);
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{filenamebase}_gsf.jpg"), renderer.ToJpeg());
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{filenamebase}_bwf.tiff"), renderer.ToTiffGroup4());
            }

            if (document.VersoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.VersoVectorImage, imageRenderingResolution);
                await _fileSystem.WriteAllBytesAsync(Path.Combine(path, $"{filenamebase}_bwb.tiff"), renderer.ToTiffGroup4());
            }
        }
    }


    internal class JsonData
    {
        [JsonPropertyName("creationDate")]
        public string CreationDate => "{{batchCaptureDate}}";

        [JsonPropertyName("documents")]
        public List<JsonDocument> Documents { get; set; } = new List<JsonDocument>();

        public int SequenceNumber => 1;

        [JsonPropertyName("statistics")]
        public Statistics Statistics { get; set; } = new Statistics();

    }

    internal class JsonDocument
    {
        [JsonPropertyName("backing")]
        public string Backing => string.Empty;
        [JsonPropertyName("batchCode")]
        public string? BatchCode { get; set; } = null;
        [JsonPropertyName("blackWhiteFront")]
        public string BlackWhiteFront { get; set; } = string.Empty;
        [JsonPropertyName("blackWhiteRear")]
        public string BlackWhiteRear { get; set; } = string.Empty;
        [JsonPropertyName("captureDate")]
        public string CaptureDate { get; set; } = string.Empty;
        [JsonPropertyName("dataread")]
        public string Dataread { get; set; } = string.Empty;
        [JsonPropertyName("documentType")]
        public string DocumentType { get; set; } = string.Empty;
        [JsonPropertyName("grayScaleFront")]
        public string GrayScaleFront { get; set; } = string.Empty;
        [JsonPropertyName("remittanceCode")]
        public string? RemittanceCode { get; set; } = null;
        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }
        [JsonPropertyName("z1")]
        public string Z1 { get; set; } = string.Empty;
        [JsonPropertyName("z2")]
        public string Z2 { get; set; } = string.Empty;
        [JsonPropertyName("z3")]
        public string Z3 { get; set; } = string.Empty;
        [JsonPropertyName("z4")]
        public string Z4 { get; set; } = string.Empty;
    }

    internal class Statistics
    {
        [JsonPropertyName("numberOfBlank")]
        public int NumberOfBlanks { get; set; } = 0;
        [JsonPropertyName("numberOfJam")]
        public int NumberOfJams { get; set; } = 0;
        [JsonPropertyName("numberOfThick")]
        public int NumberOfThicks { get; set; } = 0;
    }
}
