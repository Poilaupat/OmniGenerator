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
        /// <param name="fields">The base list of fields</param>
        /// <param name="fieldsToMerge">The collection of fields to merge</param>
        public static void Merge(this IList<FieldConfigurationBase> fields, IEnumerable<FieldConfigurationBase> fieldsToMerge)
        {
            foreach (var field in fieldsToMerge)
            {
                if (!fields.Any(x => x.Name == field.Name))
                {
                    fields.Add(field);
                }
            }
        }
    }
}
