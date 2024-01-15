using Autofac.Features.Metadata;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;

namespace SeedGenerator.Lib.Images
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
                //Getting a suitable IImageComposer implementation from DI container for the current document type
                var composer = _composers.SingleOrDefault(x => (x.Metadata["DocumentName"] ?? string.Empty).Equals(docByName.Key));

                if (composer is not null)
                {
                    //Compositing image(s)
                    foreach (var doc in docByName)
                    {
                        composer.Value.ComposeDocumentImages((Document)doc);
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
