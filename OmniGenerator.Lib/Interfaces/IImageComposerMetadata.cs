namespace OmniGenerator.Lib.Interfaces
{
    /// <summary>
    /// The interface that enable the association of document types to a specific implementation of IImageComposer
    /// </summary>
    public interface IImageComposerMetadata
    {
        public string DocumentName { get; set; }
    }
}
