using SeedGenerator.Lib.Data;
using Svg;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IImageComposer
    {
        Task ComposeDocumentImagesAsync(PacketData packet);
    }
}