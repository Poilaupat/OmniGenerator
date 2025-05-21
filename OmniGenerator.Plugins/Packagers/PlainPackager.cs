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
    /// A <see cref="IPackager"/> that writes data and image file in a directory
    /// The directory name is the concatenation of the current date+time with the root numlot
    /// </summary>
    [OmniGeneratorPluginMetadata("packager.omni.plain", "A packager that exports images along with csv files containing each document fields")]
    public class PlainPackager : OmniGeneratorPluginBase, IPackager
    {
        public async Task ProcessAsync(Root root, string basepath)
        {
            var packagename = $"PlainPackage_{DateTime.Now:yyyyMMddHHmmss}";
            var packagepath = Path.Combine(basepath, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            //Documents CSV generation
            foreach (var docsByType in root.GetDocuments().GroupBy(x => x.Name))
            {
                var filefullpath = Path.Combine(packagepath, $"{docsByType.Key}.csv");
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    foreach (var document in docsByType)
                    {
                        csv.WriteRecord(document.Fields.ToExpando());
                        csv.NextRecord();
                    }
                    await File.WriteAllTextAsync(filefullpath, writer.ToString());
                }
            }

            //Groups CSV generation
            foreach (var grpByType in root.GetGroups().GroupBy(x => x.Name))
            {
                var filefullpath = Path.Combine(packagepath, $"{grpByType.Key}.csv");
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var data = grpByType
                        //.SelectMany(x => x.GetGroups(grpByType.Key))
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

            //Document images generation
            var documents = root
                .GetDocuments()
                .ToArray();

            for (var i = 0; i < documents.Count(); i++)
            {
                WriteDocumentImages(i, documents[i], packagepath);
            }
        }

        private void WriteDocumentImages(int i, Document document, string path)
        {
            if (document.RectoImage is not null)
                PackagerTools.WriteImage(
                    document.RectoImage,
                    Path.Combine(path, $"{i:000000}R.jpg"),
                    200,
                    ImageFormat.Jpeg);

            if (document.VersoImage is not null)
                PackagerTools.WriteImage(
                    document.VersoImage,
                    Path.Combine(path, $"{i:000000}V.jpg"),
                    200,
                    ImageFormat.Jpeg);
        }
    }
}

