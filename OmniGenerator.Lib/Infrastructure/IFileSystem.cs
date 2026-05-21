namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Abstraction over file system I/O operations, enabling testability without disk access.
    /// </summary>
    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        Task WriteAllTextAsync(string path, string contents);
        Task WriteAllBytesAsync(string path, byte[] bytes);
    }
}
