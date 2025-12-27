namespace OmniGenerator.Lib.Drawers
{
    /// <summary>
    /// Represents the progress of the document drawer manager.
    /// </summary>
    public sealed class DocumentDrawerManagerProgress
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
        public int Percentage => TotalDocuments > 0
            ? (int)((ProcessedDocuments * 100) / TotalDocuments)
            : 0;
    }
}
