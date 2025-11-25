using CsvHelper;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Plugins.Packagers.Tools;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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

            await using var fs = new FileStream($"{Path.Combine(basepath, string.Concat(packagename, ".zip"))}", FileMode.CreateNew);
            using var archive = new ZipArchive(fs, ZipArchiveMode.Create);

            // Documents CSV generation
            foreach (var docsByType in root.GetDocuments().GroupBy(x => x.Name))
            {
                var entry = archive.CreateEntry($"{docsByType.Key}.csv");

                await using var stream = entry.Open();
                await using var writer = new StreamWriter(stream);
                await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                var data = docsByType
                    .Select(x => x.Fields.ToDynamic());

                if (data?.Any() == true)
                {
                    csv.WriteDynamicHeader(data.First());
                    csv.NextRecord();
                    csv.WriteRecords(data);
                }
            }

            // Groups CSV generation
            foreach (var grpByType in root.GetGroups().GroupBy(x => x.Name))
            {
                var entry = archive.CreateEntry($"{grpByType.Key}.csv");

                await using var stream = entry.Open();
                await using var writer = new StreamWriter(stream);
                await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                var data = grpByType
                    .Select(x => x.Fields.ToDynamic());

                if (data?.Any() == true)
                {
                    csv.WriteDynamicHeader(data.First());
                    csv.NextRecord();
                    csv.WriteRecords(data);
                }
            }

            var documents = root
            .GetDocuments()
            .ToArray();

            for (var i = 0; i < documents.Length; i++)
            {
                await WriteDocumentImagesAsync(i + 1, documents[i], archive, imageRenderingResolution);
            }
        }
        private static async Task WriteDocumentImagesAsync(int i, Document document, ZipArchive archive, int imageRenderingResolution)
        {
            if (document.RectoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.RectoVectorImage, imageRenderingResolution);

                var jpgEntry = archive.CreateEntry($"{i:000000}R.jpg");
                await using (var ms = new MemoryStream(renderer.ToJpeg()))
                await using (var es = jpgEntry.Open())
                {
                    await ms.CopyToAsync(es);
                }

                var tiffEntry = archive.CreateEntry($"{i:000000}R.tiff");
                await using (var ms = new MemoryStream(renderer.ToTiffGroup4()))
                await using (var es = tiffEntry.Open())
                {
                    await ms.CopyToAsync(es);
                }
            }

            if (document.VersoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.VersoVectorImage, imageRenderingResolution);

                var jpgEntry = archive.CreateEntry($"{i:000000}V.jpg");
                await using (var ms = new MemoryStream(renderer.ToJpeg()))
                await using (var es = jpgEntry.Open())
                {
                    await ms.CopyToAsync(es);
                }

                var tiffEntry = archive.CreateEntry($"{i:000000}V.tiff");
                await using (var ms = new MemoryStream(renderer.ToTiffGroup4()))
                await using (var es = tiffEntry.Open())
                {
                    await ms.CopyToAsync(es);
                }
            }
        }
    }
}
