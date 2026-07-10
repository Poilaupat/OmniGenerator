namespace OmniGenerator.Lib.Hierarchy
{
    /// <summary>
    /// Identifies which output channel of a <see cref="Field"/> is being read.
    /// Error simulation can make these channels diverge (e.g. the image shows a
    /// different value than the metadata), so consumers must read the channel that
    /// matches the artifact they produce.
    /// </summary>
    public enum FieldChannel
    {
        /// <summary>
        /// The value that ends up in the data package output. Read by packagers.
        /// This is the default channel.
        /// </summary>
        Data = 0,

        /// <summary>
        /// The value that ends up in the rendered image. Read by renderers.
        /// </summary>
        Image = 1,
    }
}
