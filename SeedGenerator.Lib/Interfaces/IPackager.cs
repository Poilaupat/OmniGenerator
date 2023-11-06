using SeedGenerator.Lib.Data;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IPackager
    {
        Task GenerateFilesAsync(PacketData packet, string path);
    }
}
