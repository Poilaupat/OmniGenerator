using Microsoft.ProgramSynthesis.Detection.Encoding;
using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Generators;
using OmniGenerator.Lib.Mapping.Interfaces;
using System.Text;

namespace OmniGenerator.Lib.Mapping
{
    /// <summary>
    /// Loads WeightedValue items from a file
    /// </summary>
    internal class WeightedListFileLoader : IListFileLoader<WeightedValue>
    {
        internal static readonly char[] Separators = [',', ';', '|'];

        /// <summary>
        /// Loads a collection of WeightedValue from a file by parsing each line
        /// </summary>
        /// <param name="filepath">The path to the file to load</param>
        /// <returns>A collection of parsed WeightedValue items</returns>
        public IEnumerable<WeightedValue> LoadFromFile(string filepath)
        {
            if (!File.Exists(filepath))
                yield break;

            using var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var encoding = DetectEncoding(stream);
            using var sr = new StreamReader(stream, encoding ?? Encoding.UTF8);

            string? line;
            while ((line = sr.ReadLine()) is not null)
            {
                WeightedValue item;
                try
                {
                    item = ParseLine(line);
                }
                catch (FormatException e)
                {
                    throw new ConfigurationException($"The file {filepath} cannot be parsed", e);
                }
                yield return item;
            }
        }

        /// <summary>
        /// Parses a line from the list file into a value-weight pair
        /// </summary>
        /// <param name="line">The line to parse, expected in the format "value" or "value,weight"</param>
        /// <returns>The parsed value-weight pair</returns>
        private static WeightedValue ParseLine(string line)
        {
            var parts = line.Split(Separators);

            // Case 1: Equiprobable format (single value)
            if (parts.Length == 1)
            {
                return new WeightedValue(parts[0].Trim(), 1.0);
            }

            // Case 2: Weighted format (value,weight)
            if (parts.Length == 2 && double.TryParse(parts[1].Trim(), out double weight))
            {
                return new WeightedValue(parts[0].Trim(), weight);
            }

            // Error case
            throw new FormatException($"Invalid format: '{line}'. Expected 'value' or 'value,weight'.");
        }

        /// <summary>
        /// Tries to detect file encoding by inspecting stream content
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>The encoding or null if identification has failed.</returns>
        private static Encoding? DetectEncoding(Stream stream)
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
