namespace OmniGenerator.Lib.Reporting
{
    public sealed class HierarchyBuildingProgress
    {
        public long GroupCount { get; set; }
        public long ProcessedGroupCount { get; set; }
        public long DocumentCount { get; set; }
        public long ProcessedDocumentCount { get; set; }
        public long FieldCount { get; set; }
    }
}
