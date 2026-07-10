using OmniGenerator.Lib.Configuration.Fields;
using OmniGenerator.Lib.Hierarchy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Merges collection2 into collection1
        /// If collection2 contains objects already present in collection1, collection1 ones are preseved.
        /// </summary>
        /// <typeparam name="T">The type of objects of the collection</typeparam>
        public static void Merge<T>(this IList<T> collection1, IEnumerable<T> collection2)
            where T : notnull
        {
            foreach (var item in collection2)
            {
                if (!collection1.Any(x => x.Equals(item)))
                {
                    collection1.Add(item);
                }
            }
        }

        /// <summary>
        /// Merges collection2 into collection1
        /// If collection2 contains objects already present in collection1, collection1 ones are replaced.
        /// </summary>
        /// <typeparam name="T">The type of objects of the collection</typeparam>
        public static void MergeWithReplace<T>(this IList<T> collection1, IEnumerable<T> collection2)
            where T : notnull
        {
            foreach (var item in collection2)
            {
                var item2remove = collection1.SingleOrDefault(x => x.Equals(item));
                if (item2remove is not null)
                    collection1.Remove(item2remove);
                collection1.Add(item);
            }
        }

        /// <summary>
        /// Merges collection2 into collection1
        /// If collection2 contains objects already present in collection1, collection1 ones are preseved.
        /// </summary>
        /// <typeparam name="T">The type of objects of the collection</typeparam>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> collection1, IEnumerable<KeyValuePair<TKey, TValue>> collection2)
            where TKey : notnull
        {
            foreach (var item in collection2)
            {
                if (!collection1.ContainsKey(item.Key))
                {
                    collection1.Add(item);
                }
            }
        }

        /// <summary>
        /// Merges collection2 into collection1
        /// If collection2 contains objects already present in collection1, collection1 ones are replaced.
        /// </summary>
        /// <typeparam name="T">The type of objects of the collection</typeparam>
        public static void MergeWithReplace<TKey, TValue>(this IDictionary<TKey, TValue> collection1, IEnumerable<KeyValuePair<TKey, TValue>> collection2)
            where TKey : notnull
        {
            foreach (var item in collection2)
            {
                if (!collection1.ContainsKey(item.Key))
                {
                    collection1.Add(item);
                }
                else
                {
                    collection1[item.Key] = item.Value;
                }
            }
        }

        /// <summary>
        /// Converts a dictionary of string keys and object values into an <see cref="ExpandoObject"/>.
        /// This method recursively converts nested dictionaries and collections into dynamic objects.
        /// </summary>
        /// <param name="fields">The dictionary to convert into an <see cref="ExpandoObject"/>.</param>
        /// <returns>An <see cref="ExpandoObject"/> representation of the dictionary.</returns>
        public static ExpandoObject ToExpando(this IDictionary<string, Field> fields)
        {
            var expando = new ExpandoObject();
            var expandoAsDic = (IDictionary<string, object?>)expando;

            foreach (var kvp in fields)
            {
                expandoAsDic.Add(kvp.Key, kvp.Value.DataStringValue);
            }

            return expando;
        }

    }
}
