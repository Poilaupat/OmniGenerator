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
        public static string ComputeIcsKey(string ics)
        {
            var key = ComputeModulo(String.Concat(ics, "00"), 100, replaceLetters: true, reverseString: true, withRankMultiplier: true);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes the IBAN key for a given country code and BBAN.
        /// </summary>
        /// <param name="input">Input must be the BBAN (Basic Bank Account Number, RIB including key for french accounts) + The country code (FR for french IBAN)</param>
        /// <returns>The computed checksum key as a two digits string</returns>
        public static string ComputeIbanKey(string input)
        {
            int key = 98 - ComputeModulo(input + "00", 97, replaceLetters: true, letterMap: ELetterMap.Iban);
            return key.ToString("00");
        }

        /// <summary>
        /// Computes a modulo on a numeric string.
        /// Optionally replaces letters by digits according to the RIB specification.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="modulo">The modulo value.</param>
        /// <param name="replaceLetters">If set to true, replaces alpha characters by digits.</param>
        /// <returns>The computed modulo value.</returns>
        /// <exception cref="ArgumentException">Thrown if replaceLetters is false and numericstring contains letters.</exception>
        private static int ComputeModulo(string input,
            int modulo,
            bool replaceLetters = false,
            bool reverseString = false,
            bool withRankMultiplier = false,
            ELetterMap letterMap = ELetterMap.Rib)
        {
            input = Regex.Replace(input, @"\s", "");

            if (replaceLetters)
            {
                input = LetterMapper.Map(input, letterMap);
            }

            if (!Regex.IsMatch(input, @"^\d+$"))
            {
                throw new ArgumentException("Input string contains unsupported characters");
            }

            if (reverseString)
            {
                input = input.ReverseString();
            }

            int result = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (withRankMultiplier)
                    result = (result + int.Parse(input.Substring(i, 1)) * (i + 1)) % modulo;
                else
                    result = (result * 10 + int.Parse(input.Substring(i, 1))) % modulo;
            }

            return result;
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="str">The string to reverse</param>
        /// <returns>The reversed string</returns>
        private static string ReverseString(this string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}

