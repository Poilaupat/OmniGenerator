using SeedGenerator.Lib.Data;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IPackager
    {
        Task ProcessAsync(Root root, string path);
    }
}
