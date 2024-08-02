using OmniGenerator.Lib.Configuration.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Configuration
{
    public static class Extensions
    {
        /// <summary>
        /// Merges a collection of <see cref="FieldConfigurationBase"/> into another collection. If the field already exists it is ignored.
        /// </summary>
        /// <param name="collection1">The base list of fields</param>
        /// <param name="collection2">The collection of fields to merge</param>
        public static void Merge(this IList<FieldConfigurationBase> collection1, IEnumerable<FieldConfigurationBase> collection2)
        {
            foreach (var field in collection2)
            {
                if (!collection1.Any(x => x.Name == field.Name))
                {
                    collection1.Add(field);
                }
            }
        }
    }
}
