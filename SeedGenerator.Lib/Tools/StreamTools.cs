using Microsoft.ProgramSynthesis.Detection.Encoding;
using System.Text;

namespace SeedGenerator.Lib.Tools
{
    public static class StreamTools
    {
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
