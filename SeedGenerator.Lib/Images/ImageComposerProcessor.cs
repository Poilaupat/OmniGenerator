using Autofac.Features.Metadata;
using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Images
{
    public class ImageComposerProcessor : IImageComposerProcessor
    {
        IEnumerable<Meta<IImageComposer>> _composers;

        public ImageComposerProcessor(IEnumerable<Meta<IImageComposer>> composers)
        {
            _composers = composers;
        }

        public async Task ProcessAsync(PacketData packetData)
        {
            foreach (var grp in packetData.Documents.GroupBy(x => x.Name))
            {
                //Getting a suitable IImageComposer implementation from DI container for the current document type
                var composer = _composers.SingleOrDefault(x => (x.Metadata["DocumentName"] ?? string.Empty).Equals(grp.Key));

                if (composer is not null)
                {
                    //Compositing image(s)
                    foreach (var doc in grp)
                    {
                        composer.Value.ComposeDocumentImages(doc);
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
