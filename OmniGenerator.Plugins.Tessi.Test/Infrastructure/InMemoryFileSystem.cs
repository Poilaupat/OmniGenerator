using OmniGenerator.Lib.Infrastructure;

namespace OmniGenerator.Plugins.Tessi.Test.Infrastructure
{
    /// <summary>
    /// In-memory implementation of <see cref="IFileSystem"/> for use in unit tests.
    /// No disk access is performed.
    /// </summary>
    public sealed class InMemoryFileSystem : IFileSystem
    {
        public Dictionary<string, string> TextFiles { get; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, byte[]> BinaryFiles { get; } = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _directories = new(StringComparer.OrdinalIgnoreCase);

        public bool DirectoryExists(string path) => _directories.Contains(path);

        public void CreateDirectory(string path) => _directories.Add(path);

        public Task WriteAllTextAsync(string path, string contents)
        {
            TextFiles[path] = contents;
            return Task.CompletedTask;
        }

        public Task WriteAllBytesAsync(string path, byte[] bytes)
        {
            BinaryFiles[path] = bytes;
            return Task.CompletedTask;
        }
    }
}
