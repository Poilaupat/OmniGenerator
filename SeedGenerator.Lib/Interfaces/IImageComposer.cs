using SeedGenerator.Lib.Data;
using Svg;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IImageComposer
    {
        void ComposeDocumentImagesAsync(PacketData packet);
    }
}