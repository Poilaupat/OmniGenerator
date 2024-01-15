using SeedGenerator.Lib.Data;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IImageComposerProcessor
    {
        Task ProcessAsync(Root root);
    }
}
