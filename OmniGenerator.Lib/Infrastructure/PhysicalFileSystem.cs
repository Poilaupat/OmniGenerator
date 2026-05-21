namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Production implementation of <see cref="IFileSystem"/> that delegates to <see cref="System.IO"/>.
    /// </summary>
    public sealed class PhysicalFileSystem : IFileSystem
    {
        public bool DirectoryExists(string path) => Directory.Exists(path);

        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public Task WriteAllTextAsync(string path, string contents) =>
            File.WriteAllTextAsync(path, contents);

        public Task WriteAllBytesAsync(string path, byte[] bytes) =>
            File.WriteAllBytesAsync(path, bytes);
    }
}
