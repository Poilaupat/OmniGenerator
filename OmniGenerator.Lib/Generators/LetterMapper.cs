using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Generators
{
    public enum ELetterMap
    {
        Rib = 0,
        Iban = 1,
    }

    public static class LetterMapper
    {
        private static string[] _letters = "A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z".Split('|');
        private static string[] _ribmap = "1|2|3|4|5|6|7|8|9|1|2|3|4|5|6|7|8|9|2|3|4|5|6|7|8|9".Split('|');
        private static string[] _ibanmap = "10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35".Split('|');

        public static string Map(string input, ELetterMap maptype)
        {
            input = input.ToUpper();
            string[] map = GetMap(maptype);

            for (int i = 0; i < _letters.Count(); i++)
            {
                input = input.Replace(_letters[i], map[i]);
            }

            return input;
        }

        private static string[] GetMap(ELetterMap maptype)
        {
            switch (maptype)
            {
                case ELetterMap.Rib:
                    return _ribmap;

                case ELetterMap.Iban:
                    return _ibanmap;

                default:
                    throw new ArgumentException("Unexpected map");
            }
        }
    }
}
