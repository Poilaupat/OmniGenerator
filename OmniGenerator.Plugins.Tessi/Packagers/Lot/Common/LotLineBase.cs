using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Plugins.Tessi.Packagers.Lot.Common
{
    internal class LotLineBase
    {
        public virtual string ToFixedLengthString()
        {
            var lineAttr = GetType().GetCustomAttribute<FixedLengthLineAttribute>();
            if (lineAttr == null)
                throw new InvalidOperationException("Missing FixedLengthLine attribute on class.");

            var line = new StringBuilder(new string(' ', lineAttr.Length));
            
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<LotFieldAttribute>();
                if (attr == null) 
                    continue;

                var value = property.GetValue(this) as string ?? string.Empty;
                
                if (value.Length > attr.Length)
                    value = value.Substring(0, attr.Length);

                string padded = attr.PadDirection switch
                {
                    PadDirection.Left => value.PadLeft(attr.Length, attr.PaddingChar),
                    PadDirection.Right => value.PadRight(attr.Length, attr.PaddingChar),
                    _ => throw new InvalidOperationException("Unknown PadDirection")
                };

                line.Remove(attr.Offset, attr.Length);
                line.Insert(attr.Offset, padded);
            }

            return line.ToString();
        }

    }
}
