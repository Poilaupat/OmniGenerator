using Autofac.Features.Metadata;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;

namespace SeedGenerator.Lib.Image
{
    public class ImageComposerProcessor : IImageComposerProcessor
    {
        IEnumerable<Meta<IImageComposer>> _composers;

        public ImageComposerProcessor(IEnumerable<Meta<IImageComposer>> composers)
        {
            _composers = composers;
        }

        public async Task ProcessAsync(Root root)
        {
            var docByNamesGrp = root.GetDocuments(true)
                .GroupBy(x => x.Name);

            foreach (var docByName in docByNamesGrp)
            {
                //Getting the appropriate IImageComposer implementation from DI container for the current document type
                var composer = _composers.SingleOrDefault(x => (x.Metadata["DocumentName"] ?? string.Empty).Equals(docByName.Key));

                if (composer is not null)
                {
                    //Compositing image(s)
                    foreach (var doc in docByName)
                    {
                        var recto = composer.Value.ComposeImageRecto(doc);
                        var verso = composer.Value.ComposeImageVerso(doc);
                        doc.RectoImage = recto;
                        doc.VersoImage = verso;
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
