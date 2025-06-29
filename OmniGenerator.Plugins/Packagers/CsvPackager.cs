using CsvHelper;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Tools;
using OmniGenerator.Plugins.Packagers.Tools;
using System.Drawing.Imaging;
using System.Globalization;

namespace OmniGenerator.Plugins.Packagers
{
    /// <summary>
    /// A packager that exports images along with CSV files containing each document's fields.
    /// </summary>
    [OmniGeneratorPluginMetadata("packager.omni.csv", "A packager that exports images along with csv files containing each document fields")]
    public class CsvPackager : OmniGeneratorPluginBase, IPackager
    {
        /// <summary>
        /// Processes the specified <see cref="Root"/> object and exports its documents and groups as CSV files,
        /// and document images as JPEG and TIFF files, into a new package directory under the given base path.
        /// </summary>
        /// <param name="root">The root object containing documents and groups to export.</param>
        /// <param name="basepath">The base directory path where the package will be created.</param>
        /// <param name="imageRenderingResolution">The resolution (in DPI) to use when rendering images.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
        {
            var packagename = $"CsvPackage_{DateTime.Now:yyyyMMddHHmmss}";
            var packagepath = Path.Combine(basepath, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            // Documents CSV generation
            foreach (var docsByType in root.GetDocuments().GroupBy(x => x.Name))
            {
                var filefullpath = Path.Combine(packagepath, $"{docsByType.Key}.csv");
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var data = docsByType
                        .Select(x => x.Fields.ToExpando());

                    if (data is not null && data.Any())
                    {
                        csv.WriteDynamicHeader(data.First());
                        csv.NextRecord();
                        csv.WriteRecords(data);

                        await File.WriteAllTextAsync(filefullpath, writer.ToString());
                    }
                    await File.WriteAllTextAsync(filefullpath, writer.ToString());
                }
            }

            // Groups CSV generation
            foreach (var grpByType in root.GetGroups().GroupBy(x => x.Name))
            {
                var filefullpath = Path.Combine(packagepath, $"{grpByType.Key}.csv");
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var data = grpByType
                        .Select(x => x.Fields.ToExpando());

                    if (data is not null && data.Any())
                    {
                        csv.WriteDynamicHeader(data.First());
                        csv.NextRecord();
                        csv.WriteRecords(data);

                        await File.WriteAllTextAsync(filefullpath, writer.ToString());
                    }
                }
            }

            // Document images generation
            var documents = root
                .GetDocuments()
                .ToArray();

            for (var i = 0; i < documents.Count(); i++)
            {
                await WriteDocumentImagesAsync(i+1, documents[i], packagepath, imageRenderingResolution);
            }
        }

        /// <summary>
        /// Writes the recto and verso images of a document to disk as JPEG and TIFF (Group 4) files, if present.
        /// </summary>
        /// <param name="i">The index of the document, used for file naming.</param>
        /// <param name="document">The document whose images are to be written.</param>
        /// <param name="path">The directory path where images will be saved.</param>
        /// <param name="imageRenderingResolution">The resolution (in DPI) to use when rendering images.</param>
        /// <returns>A task representing the asynchronous file writing operation.</returns>
        private async Task WriteDocumentImagesAsync(int i, Document document, string path, int imageRenderingResolution)
        {
            if (document.RectoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.RectoVectorImage, imageRenderingResolution);
                await File.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}R.jpg"), renderer.ToJpeg());
                await File.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}R.tiff"), renderer.ToTiffGroup4());
            }

            if (document.VersoVectorImage is not null)
            {
                var renderer = new SvgRenderer(document.VersoVectorImage, imageRenderingResolution);
                await File.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}V.jpg"), renderer.ToJpeg());
                await File.WriteAllBytesAsync(Path.Combine(path, $"{i:000000}V.tiff"), renderer.ToTiffGroup4());
            }
        }
    }
}

