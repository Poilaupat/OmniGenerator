using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Reporting
{
    /// <summary>
    /// Represents the progress of a packaging operation.
    /// </summary>
    public sealed class PackagingProgress
    {
        /// <summary>
        /// Gets or sets the total number of bytes written so far.
        /// </summary>
        public long BytesWritten { get; set; }

        /// <summary>
        /// Gets or sets the total number of files written so far.
        /// </summary>
        public long FilesWritten { get; set; }

        /// <summary>
        /// Gets or sets the name of the file currently being written, or null if no file is being written.
        /// </summary>
        public string? CurrentFileName { get; set; }

        /// <summary>
        /// Gets the bytes written formatted as a human-readable string (e.g., "1.23 MB").
        /// </summary>
        [JsonIgnore]
        public string FormattedBytesWritten => FormatBytes(BytesWritten);

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
