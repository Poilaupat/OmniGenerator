using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Packagers.Tools
{
    /// <summary>
    /// Provides extension methods for collections and dictionaries, 
    /// including bulk addition and conversion to dynamic objects.
    /// </summary>
    internal static class FieldDictionaryExtensions
    {
        /// <summary>
        /// Adds a range of items to the target collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to which items will be added.</param>
        /// <param name="items">The items to add to the collection.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Converts a dictionary of string keys and object values to a dynamic object.
        /// Each key-value pair in the dictionary becomes a property on the dynamic object.
        /// </summary>
        /// <param name="source">The source dictionary to convert.</param>
        /// <returns>
        /// A dynamic object (ExpandoObject) with properties corresponding to the dictionary entries.
        /// </returns>
        public static dynamic ToDynamicObject(this IDictionary<string, object> source)
        {
            var someObject = new ExpandoObject() as IDictionary<string, object>;
            someObject.AddRange(source);
            return someObject;
        }
    }
}
