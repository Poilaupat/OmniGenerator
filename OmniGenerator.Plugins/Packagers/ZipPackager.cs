using CsvHelper;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Plugins.Packagers.Tools;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO.Compression;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// A <see cref="IPackager"/> that writes data and image file in a zip file
    /// The zip name is the concatenation of the current date+time with the root numlot
    /// </summary>
    [OmniGeneratorPluginMetadata("packager.omni.zip", "Similar to CsvPackager but the output is zipped")]
    public class ZipPackager : OmniGeneratorPluginBase, IPackager
    {
        public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
        {
            var packagename = $"ZipPackage_{DateTime.Now:yyyyMMddHHmmss}";

            using (var fs = new FileStream($"{Path.Combine(basepath, string.Concat(packagename, ".zip"))}", FileMode.CreateNew))
            using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                // Documents CSV generation
                foreach (var docsByType in root.GetDocuments().GroupBy(x => x.Name))
                {
                    var entry = archive.CreateEntry($"{docsByType.Key}.csv");
                    using (var stream = entry.Open())
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        var data = docsByType
                            .Select(x => x.Fields.ToExpando());

                        if (data is not null && data.Any())
                        {
                            csv.WriteDynamicHeader(data.First());
                            csv.NextRecord();
                            csv.WriteRecords(data);
                        }
                    }
                }

                // Groups CSV generation
                foreach (var grpByType in root.GetGroups().GroupBy(x => x.Name))
                {
                    var entry = archive.CreateEntry($"{grpByType.Key}.csv");
                    using (var stream = entry.Open())
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        var data = grpByType
                            .Select(x => x.Fields.ToExpando());

                        if (data is not null && data.Any())
                        {
                            csv.WriteDynamicHeader(data.First());
                            csv.NextRecord();
                            csv.WriteRecords(data);
                        }
                    }
                }

                var documents = root
                .GetDocuments()
                .ToArray();

                for (var i = 0; i < documents.Count(); i++)
                {
                    WriteDocumentImages(i, documents[i], archive, imageRenderingResolution);
                }
            }
        }
        private void WriteDocumentImages(int i, Document document, ZipArchive archive, int imageRenderingResolution)
        {
            if (document.RectoImage is not null)
                PackagerTools.WriteImage(
                    document.RectoImage,
                    archive,
                    $"{i:000000}R.jpg",
                    imageRenderingResolution,
                    ImageFormat.Jpeg);

            if (document.VersoImage is not null)
                PackagerTools.WriteImage(
                    document.VersoImage,
                    archive,
                    $"{i:000000}V.jpg",
                    imageRenderingResolution,
                    ImageFormat.Jpeg);
        }
    }
}
