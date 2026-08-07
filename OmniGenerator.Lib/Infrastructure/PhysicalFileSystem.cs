using OmniGenerator.Lib.Reporting;

namespace OmniGenerator.Lib.Infrastructure
{
    /// <summary>
    /// Production implementation of <see cref="IFileSystem"/> that delegates to <see cref="System.IO"/>.
    /// Optionally reports packaging progress when a hub and job ID are provided.
    /// </summary>
    public sealed class PhysicalFileSystem : IFileSystem
    {
        private const int BufferSize = 81920; // 80 KB buffer
        private readonly IProgressHub<PackagingProgress>? _packagingHub;
        private string? _activeJobId;
        private long _totalBytesWritten;
        private long _filesWritten;

        /// <summary>
        /// Initializes a new instance of <see cref="PhysicalFileSystem"/>.
        /// </summary>
        /// <param name="packagingHub">Optional progress hub for reporting packaging operations.</param>
        public PhysicalFileSystem(IProgressHub<PackagingProgress>? packagingHub = null)
        {
            _packagingHub = packagingHub;
        }

        /// <summary>
        /// Starts tracking progress for a packaging job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        public void StartPackagingJob(string jobId)
        {
            _activeJobId = jobId;
            _totalBytesWritten = 0;
            _filesWritten = 0;
            ReportProgress(null);
        }

        /// <summary>
        /// Stops tracking progress for the current packaging job.
        /// </summary>
        public void EndPackagingJob()
        {
            ReportProgress(null);
            _activeJobId = null;
        }

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public async Task WriteAllTextAsync(string path, string contents)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(contents);
            await WriteAllBytesAsync(path, bytes);
        }

        public async Task WriteAllBytesAsync(string path, byte[] bytes)
        {
            var fileName = Path.GetFileName(path);

            // Report file start
            ReportProgress(fileName);

            // Write with progress reporting
            await using var source = new MemoryStream(bytes);
            await WriteFileWithProgressAsync(path, source, fileName);

            // Report file complete
            Interlocked.Increment(ref _filesWritten);
            ReportProgress(null);
        }

        private async Task WriteFileWithProgressAsync(string path, Stream source, string fileName)
        {
            byte[] buffer = new byte[BufferSize];
            long totalBytes = source.Length;
            long writtenBytes = 0;

            await using var destination = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                BufferSize,
                useAsync: true);

            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer)) > 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, bytesRead));

                writtenBytes += bytesRead;
                Interlocked.Add(ref _totalBytesWritten, bytesRead);

                // Report progress every 10% or every buffer for small files
                if (totalBytes < BufferSize || writtenBytes % (totalBytes / 10) < BufferSize)
                {
                    ReportProgress(fileName);
                }
            }
        }

        private void ReportProgress(string? currentFileName)
        {
            if (_packagingHub is null || string.IsNullOrEmpty(_activeJobId))
                return;

            var progress = new PackagingProgress
            {
                BytesWritten = Interlocked.Read(ref _totalBytesWritten),
                FilesWritten = Interlocked.Read(ref _filesWritten),
                CurrentFileName = currentFileName
            };

            _packagingHub.Report(_activeJobId, progress);
        }
    }
}
