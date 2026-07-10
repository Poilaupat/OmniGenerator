namespace OmniGenerator.Lib.Configuration.ErrorSimulation
{
    /// <summary>
    /// Enumerates the supported scanner and human error simulation types.
    /// </summary>
    public enum EErrorSimulationType
    {
        /// <summary>
        /// Simulates an OCR misread: a block of consecutive characters is replaced by '?'.
        /// Affects the metadata (data channel) only; the image is unchanged.
        /// </summary>
        Misread,

        /// <summary>
        /// Simulates a scanner confusion: one or more characters are substituted using a confusion table.
        /// Affects the metadata (data channel) only; the image is unchanged.
        /// </summary>
        Substitution,

        /// <summary>
        /// Simulates a human inconsistency: the value visible on the image diverges from the correct metadata.
        /// Affects the image channel only; the metadata is unchanged.
        /// </summary>
        Inconsistency,
    }
}
