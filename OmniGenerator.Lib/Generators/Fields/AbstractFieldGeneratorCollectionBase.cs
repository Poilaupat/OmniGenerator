using OmniGenerator.Lib.Exceptions;
using OmniGenerator.Lib.Interfaces.FieldGenerators;
using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators.Fields
{
    /// <summary>
    /// The base class fot all generators that picks up random values in a list
    /// Items can be loaded from a configured list or from a file
    /// If the two are specified, only the items from the file are loaded
    /// </summary>
    /// <typeparam name="TGenerator">The generator type</typeparam>
    /// <typeparam name="TCollection">The type of collection</typeparam>
    internal abstract class AbstractFieldGeneratorCollectionBase<TGenerator, TCollection>
        : AbstractFieldGenerator<TGenerator>, IFieldGeneratorCollection<TCollection>
        where TGenerator : notnull
        where TCollection : notnull
    {
        public virtual IEnumerable<TCollection> List { get; }

        /// <summary>
        /// Creates a new <see cref="AbstractFieldGeneratorListBase"/>
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="list">The configured list</param>
        /// <param name="listFilePath">The path of the configured file</param>
        public AbstractFieldGeneratorCollectionBase(string name, IEnumerable<TCollection>? list, string? listFilePath)
            : base(name)
        {
            //Priority to the items from the file if specified
            if (!string.IsNullOrEmpty(listFilePath))
            {
                var fileItems = LoadFile(listFilePath);
                if (fileItems.Any())
                    List = fileItems;
            }

            //Then, if nothing has been loaded from the file, try to load the list from the configuration file, empty list otherwise
            List ??= list ?? new List<TCollection>();
        }

        protected override abstract TGenerator GenerateValue();

        /// <summary>
        /// Parses a line of the provided file to a suitable <see cref="TCollection"/> item
        /// </summary>
        /// <param name="line">One line of the text file</param>
        /// <returns>A <see cref="TCollection"/> item</returns>
        protected abstract TCollection ParseLine(string line);

        /// <summary>
        /// Reads the configured file and parse each line
        /// </summary>
        /// <param name="filepath">The configured file path</param>
        /// <returns>A collection of <see cref="TCollection"/></returns>
        protected virtual IEnumerable<TCollection> LoadFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var encoding = StreamTools.DetectEncoding(stream);
                    using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8))
                    {
                        string? line;
                        while ((line = sr.ReadLine()) is not null)
                        {
                            TCollection item;
                            try
                            {
                                item = ParseLine(line);
                            }
                            catch(FormatException e)
                            {
                                throw new ConfigurationException($"The file {filepath} cannot be parsed", e);
                            }
                            yield return item;
                        }
                    }
                }
            }
        }
    }
}
