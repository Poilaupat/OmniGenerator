using System.Text.RegularExpressions;

namespace OmniGenerator.Lib.Tools
{
    /// <summary>
    /// Some tools to compute different types of checksum
    /// </summary>
    public static class KeyTools
    {
        /// <summary>
        /// Computes a RIB checksum
        /// </summary>
        /// <param name="rib">The RIB. Can contain letters.</param>
        /// <returns>The checksum</returns>
        public static string ComputeRibKey(string rib)
        {
            string numericstring = string.Concat(rib, "00");
            var key = 97 - ComputeModulo(numericstring, 97, true);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes a RLMC checksum
        /// </summary>
        /// <param name="z4">The Z4 of the CMC7</param>
        /// <param name="z3">The Z3 of the CMC7</param>
        /// <param name="z2">The Z2 of the CMC7</param>
        /// <returns>The RLMC key</returns>
        public static string ComputeRlmcKey(string z4, string z3, string z2)
        {
            string numericstring = string.Concat(z4, z3, z2);
            return ComputeRlmcKey(numericstring);
        }

        /// <summary>
        /// Computes a RLMC checksum
        /// </summary>
        /// <param name="cmc7">The CMC7 of the check. It can be a space separated strings or in one go.</param>
        /// <returns>The RLMC key</returns>
        public static string ComputeRlmcKey(string cmc7)
        {
            string numericstring = string.Concat(string.Join("", cmc7.Split(' ', StringSplitOptions.RemoveEmptyEntries)), "00");
            var key = 97 - ComputeModulo(numericstring, 97, false);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes a TIP checksum
        /// </summary>
        /// <param name="numericstring">A numeric string. Spaces are allowed.</param>
        /// <returns>The checksum</returns>
        /// <exception cref="ArgumentException">Thrown if the string contains non-numeric chars</exception>
        public static string ComputeTipKey(string numericstring)
        {
            numericstring = Regex.Replace(numericstring, @"\s", "");
            
            if (!Regex.IsMatch(numericstring, @"\d+"))
            {
                throw new ArgumentException("Parameter numericstring must contain only digits");
            }

            int key = 0;

            for (int i = 0; i < numericstring.Length; i++)
		    {
                key = (key + int.Parse(numericstring.Substring(numericstring.Length - i - 1, 1)) * (i + 1)) % 100;
            }

            return key.ToString("00");
        }

        /// <summary>
        /// Compute the specific checksum used for the 6th group in a SEPA slip
        /// </summary>
        /// <param name="numericstring">A numeric string</param>
        /// <returns>The checksum</returns>
        public static string ComputeTipGroup6Key(string numericstring)
        {
            int twoDigitsKey = 11 - ComputeModulo(numericstring, 11, false);
            int oneDigitKey = ComputeModulo(twoDigitsKey.ToString(), 10, false);
            return oneDigitKey.ToString();
        }

        /// <summary>
        /// Computes a modulo on a numeric string
        /// Optionnaly can replace letters by digits according to the RIB specification
        /// </summary>
        /// <param name="numericstring">The string</param>
        /// <param name="modulo">The value of the modulo</param>
        /// <param name="replaceLetters">If set to true, replaces alpha chars by digits</param>
        /// <returns>The modulo</returns>
        /// <exception cref="ArgumentException">Thrown if replace letters is false and numericstring contains letters</exception>
        private static int ComputeModulo(string numericstring, int modulo, bool replaceLetters)
        {
            numericstring = Regex.Replace(numericstring, @"\s", "");

            if (replaceLetters)
            {
                numericstring = ReplaceLetters(numericstring);
            }

            if (!Regex.IsMatch(numericstring, @"^\d+$"))
            {
                throw new ArgumentException("Input string contains non supported caracters");
            }

            int result = 0;

            for (int i = 0; i < numericstring.Length; i++)
            {
                result = (result * 10 + int.Parse(numericstring.Substring(i, 1))) % modulo;
            }

            return result;
        }

        /// <summary>
        /// Replaces alpha chars by digit according to RIB specifications
        /// </summary>
        /// <param name="str">The input string</param>
        /// <returns>A numeric string</returns>
        private static string ReplaceLetters(string str)
        {
            return str
                .ToUpper()
                .Replace('A', '1')
                .Replace('B', '2')
                .Replace('C', '3')
                .Replace('D', '4')
                .Replace('E', '5')
                .Replace('F', '6')
                .Replace('G', '7')
                .Replace('H', '8')
                .Replace('I', '9')
                .Replace('J', '1')
                .Replace('K', '2')
                .Replace('L', '3')
                .Replace('M', '4')
                .Replace('N', '5')
                .Replace('O', '6')
                .Replace('P', '7')
                .Replace('Q', '8')
                .Replace('R', '9')
                .Replace('S', '2')
                .Replace('T', '3')
                .Replace('U', '4')
                .Replace('V', '5')
                .Replace('W', '6')
                .Replace('X', '7')
                .Replace('Y', '8')
                .Replace('Z', '9')
                ;
        }      
    }
}

