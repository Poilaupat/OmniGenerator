using Microsoft.ProgramSynthesis.Detection.Encoding;
using System.Text;

namespace SeedGenerator.Lib.Tools
{
    /// <summary>
    /// Tools to use with files read as streams
    /// </summary>
    public static class StreamTools
    {
        /// <summary>
        /// Tries to detect file encoding by inspecting stream content
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>The encoding or null if identification has failed.</returns>
        public static Encoding? DetectEncoding(Stream stream)
        {
            try
            {
                if (stream.CanSeek)
                {
                    // Read from the beginning if possible
                    stream.Seek(0, SeekOrigin.Begin);
                }

                // Detect encoding type (enum)
                var encodingType = EncodingIdentifier.IdentifyEncoding(stream);

                // Get the corresponding encoding name to be passed to System.Text.Encoding.GetEncoding
                var encodingDotNetName = EncodingTypeUtils.GetDotNetName(encodingType);

                if (!string.IsNullOrEmpty(encodingDotNetName))
                {
                    return Encoding.GetEncoding(encodingDotNetName);
                }
            }
            finally
            {
                if (stream.CanSeek)
                {
                    // Reinit stream to the beginning after detection if possible
                    stream.Seek(0, SeekOrigin.Begin);
                }
            }

            // In case of error return null or a default value
            return null;
        }
    }
}
