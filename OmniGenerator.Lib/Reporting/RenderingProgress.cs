using System.Text.Json.Serialization;

namespace OmniGenerator.Lib.Reporting
{
    /// <summary>
    /// Represents the progress of the document renderer manager.
    /// </summary>
    public sealed class RenderingProgress
    {
        /// <summary>
        /// Gets or sets the total number of documents to process.
        /// </summary>
        public long TotalDocuments { get; set; }

        /// <summary>
        /// Gets or sets the number of documents that have been processed.
        /// </summary>
        public long ProcessedDocuments { get; set; }

        /// <summary>
        /// Gets the percentage of completion (0-100).
        /// </summary>
        [JsonIgnore]
        public int Percentage => TotalDocuments > 0
            ? (int)(ProcessedDocuments * 100 / TotalDocuments)
            : 0;
    }
}
