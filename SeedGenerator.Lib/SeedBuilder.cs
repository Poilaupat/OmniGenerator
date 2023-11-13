using AutoMapper;
using Microsoft.Extensions.Configuration;
using SeedGenerator.Lib.DataGenerators;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Param;
using SeedGenerator.Lib.Param.Serialization;
using SeedGenerator.Lib.Tools;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SeedGenerator.Lib
{
    public class SeedBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPackager _packager;
        private readonly IImageComposer _imageComposer;

        private PacketParam? _param;

        public SeedBuilder(IConfiguration configuration, IMapper mapper, IPackager packager, IImageComposer imageComposer)
        {
            _configuration = configuration;
            _mapper = mapper;
            _packager = packager;
            _imageComposer = imageComposer;
        }

        public async Task LoadParam(string paramFilePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new PolymorphicTypeResolver(),
            };

            string jsonparam = await File.ReadAllTextAsync(paramFilePath);
            _param = JsonSerializer.Deserialize<PacketParam>(jsonparam, options);
        }

        public async Task BuildSeed(string outputPath)
        {
            try
            {
                var packetData = GenerateSeedData();
                _imageComposer.ComposeDocumentImagesAsync(packetData);

                foreach (var doc in packetData.Documents)
                {
                    if (doc.Image is not null && doc.Fields is not null)
                    {
                        using (var bitmap = ImageTools.RenderSvg(doc.Image, 200))
                        {
                            bitmap.Save($@"C:\Users\Ruben\source\repos\SeedGenerator\Output\svg2.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }

                await _packager.GenerateFilesAsync(packetData, outputPath);
            }
            catch
            {

            }
        }

        private PacketData GenerateSeedData()
        {
            if (_param is null)
                throw new NullReferenceException($"No param was provided");


            var packetDataBuilder = new PacketDataGenerator(_param, _mapper);
            return packetDataBuilder.GeneratePacketData();
        }
    }
}
