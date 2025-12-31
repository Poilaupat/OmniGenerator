using System.Drawing;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace OmniGenerator.Lib.Tools
{
    /// <summary>
    /// Provides utility methods to compute various types of checksums and keys, such as RIB, RLMC, TIP, and SEPA group 6 keys.
    /// </summary>
    public static class KeyTools
    {
        /// <summary>
        /// Returns the input string as a dummy key.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The same input string.</returns>
        public static string ComputeDummyKey(string input)
        {
            return input;
        }

        /// <summary>
        /// Computes a RIB checksum key.
        /// </summary>
        /// <param name="rib">The RIB string, which can contain letters.</param>
        /// <returns>The computed RIB checksum as a two-digit string.</returns>
        public static string ComputeRibKey(string rib)
        {
            var key = 97 - ComputeModulo(string.Concat(rib, "00"), 97, true);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes a RLMC checksum key from Z4, Z3, and Z2 CMC7 components.
        /// </summary>
        /// <param name="z4">The Z4 component of the CMC7.</param>
        /// <param name="z3">The Z3 component of the CMC7.</param>
        /// <param name="z2">The Z2 component of the CMC7.</param>
        /// <returns>The computed RLMC key as a two-digit string.</returns>
        public static string ComputeRlmcKey(string z4, string z3, string z2)
        {
            string numericstring = string.Concat(z4, z3, z2);
            return ComputeRlmcKey(numericstring);
        }

        /// <summary>
        /// Computes a RLMC checksum key from a CMC7 string.
        /// </summary>
        /// <param name="cmc7">The CMC7 string, which can be space-separated or a single string.</param>
        /// <returns>The computed RLMC key as a two-digit string.</returns>
        public static string ComputeRlmcKey(string cmc7)
        {
            string numericstring = string.Concat(string.Join("", cmc7.Split(' ', StringSplitOptions.RemoveEmptyEntries)), "00");
            var key = 97 - ComputeModulo(numericstring, 97);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes a TIP checksum key.
        /// </summary>
        /// <param name="numericstring">A numeric string. Spaces are allowed.</param>
        /// <returns>The computed TIP checksum as a two-digit string.</returns>
        /// <exception cref="ArgumentException">Thrown if the string contains non-numeric characters.</exception>
        public static string ComputeTipKey(string numericstring)
        {
            var key = ComputeModulo(numericstring, 100, replaceLetters: false, reverseString: true, withRankMultiplier: true);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes the specific checksum used for the 6th group in a SEPA slip.
        /// </summary>
        /// <param name="numericstring">A numeric string.</param>
        /// <returns>The computed checksum as a single-digit string.</returns>
        public static string ComputeTipGroup6Key(string numericstring)
        {
            int twoDigitsKey = 11 - ComputeModulo(numericstring, 11);
            int oneDigitKey = ComputeModulo(twoDigitsKey.ToString(), 10);
            return oneDigitKey.ToString();
        }

        /// <summary>
        /// Computes the checksum key for an ICS (Identifiant Créancier SEPA).
        /// </summary>
        /// <param name="ics">The ICS value (3 letters followed by 6 digits).</param>
        /// <returns>The computed checksum key as a two digits string</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string ComputeIcsKey(string ics)
        {
            var key = ComputeModulo(String.Concat(ics, "00"), 100, replaceLetters: true, reverseString: true, withRankMultiplier: true);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes a modulo on a numeric string.
        /// Optionally replaces letters by digits according to the RIB specification.
        /// </summary>
        /// <param name="numericstring">The input string.</param>
        /// <param name="modulo">The modulo value.</param>
        /// <param name="replaceLetters">If set to true, replaces alpha characters by digits.</param>
        /// <returns>The computed modulo value.</returns>
        /// <exception cref="ArgumentException">Thrown if replaceLetters is false and numericstring contains letters.</exception>
        private static int ComputeModulo(string numericstring, int modulo, bool replaceLetters = false, bool reverseString = false, bool withRankMultiplier = false)
        {
            numericstring = Regex.Replace(numericstring, @"\s", "");

            if (replaceLetters)
            {
                numericstring = ReplaceLetters(numericstring);
            }

            if (!Regex.IsMatch(numericstring, @"^\d+$"))
            {
                throw new ArgumentException("Input string contains unsupported characters");
            }

            if (reverseString)
            {
                numericstring = numericstring.ReverseString();
            }

            int result = 0;

            for (int i = 0; i < numericstring.Length; i++)
            {
                if (withRankMultiplier)
                    result = (result + int.Parse(numericstring.Substring(i, 1)) * (i + 1)) % modulo;
                else
                    result = (result * 10 + int.Parse(numericstring.Substring(i, 1))) % modulo;
            }

            return result;
        }

        /// <summary>
        /// Replaces alpha characters by digits according to RIB specifications.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>A numeric string with letters replaced by digits.</returns>
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

        private static string ReverseString(this string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}

