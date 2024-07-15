namespace OmniGenerator.Lib.Tools
{
    /// <summary>
    /// Provides a string representation of numbers in french
    /// This code is based on Eric Moreau's algorithm
    /// https://www.emoreau.com/Entries/Articles/2017/02/Net-code-to-convert-numbers-to-words.aspx
    /// </summary>
    public class NumberToWords
    {

        public static string Convert(int value)
        {
            string strReturn;

            if (value < 0)
                throw new NotSupportedException("negative numbers not supported");
            else if (value == 0)
                strReturn = "zéro";
            else if (value < 10)
                strReturn = ConvertDigitToWords(value);
            else if (value < 20)
                strReturn = ConvertTeensToWords(value);
            else if (value < 100)
                strReturn = ConvertHighTensToWords(value);
            else if (value < 1000)
                strReturn = ConvertBigNumberToWords(value, 100, "cent");
            else if (value < 1000000)
                strReturn = ConvertBigNumberToWords(value, 1000, "mille");
            else if (value < 1000000000)
                strReturn = ConvertBigNumberToWords(value, 1000000, "million");
            else
                throw new NotSupportedException("Number is too large!!!");

            if (strReturn.EndsWith("quatre-vingt"))
            {
                strReturn += "s";
            }

            return strReturn;
        }

        // assumes a number from 0 to 9
        private static string ConvertDigitToWords(int value)
        {
            return value switch
            {
                0 => "",
                1 => "un",
                2 => "deux",
                3 => "trois",
                4 => "quatre",
                5 => "cinq",
                6 => "six",
                7 => "sept",
                8 => "huit",
                9 => "neuf",
                _ => throw new IndexOutOfRangeException($"{value} not a digit")
            };
        }

        //assumes a number between 10 & 19
        private static string ConvertTeensToWords(int value)
        {
            return value switch
            {
                10 => "dix",
                11 => "onze",
                12 => "douze",
                13 => "treize",
                14 => "quatorze",
                15 => "quinze",
                16 => "seize",
                17 => "dix-sept",
                18 => "dix-huit",
                19 => "dix-neuf",
                _ => throw new IndexOutOfRangeException($"{value} not a teen")
            };
        }

        //assumes a number between 20 and 99
        private static string ConvertHighTensToWords(int value)
        {
            int tensDigit = (int)(Math.Floor((double)value / 10.0));

            var tensStr = tensDigit switch
            {
                2 => "vingt",
                3 => "trente",
                4 => "quarante",
                5 => "cinquante",
                6 => "soixante",
                7 => "soixante-dix",
                8 => "quatre-vingt",
                9 => "quatre-vingt-dix",
                _ => throw new IndexOutOfRangeException($"{value} not in range 20-99")
            };

            if (value % 10 == 0) return tensStr;

            //French sometime has a prefix in front of 1
            string strPrefix = string.Empty;
            if ((tensDigit < 8) && (value - tensDigit * 10 == 1))
                strPrefix = "-et";

            string onesStr;
            if ((tensDigit == 7 || tensDigit == 9))
            {
                tensStr = ConvertHighTensToWords(10 * (tensDigit - 1));
                onesStr = ConvertTeensToWords(10 + value - tensDigit * 10);
            }
            else
                onesStr = ConvertDigitToWords(value - tensDigit * 10);

            return tensStr + strPrefix + "-" + onesStr;
        }

        // Use this to convert any integer bigger than 99
        private static string ConvertBigNumberToWords(int value, int baseNum, string baseNumStr)
        {
            string separator = " ";

            // Strategy: translate the first portion of the number, then recursively translate the remaining sections.

            // Step 1: strip off first portion, and convert it to string:
            int bigPart = (int)(Math.Floor((double)value / baseNum));
            string bigPartStr;

            if (bigPart == 1 && value < 1000000)
                bigPartStr = baseNumStr;
            else
                bigPartStr = Convert(bigPart) + " " + baseNumStr;


            // Step 2: check to see whether we're done:
            if (value % baseNum == 0)
            {
                if (bigPart > 1)
                {
                    //in French, a s is required to cent/mille/million/milliard if there is a value in front but nothing after
                    return bigPartStr + "s";
                }
                else
                    return bigPartStr;
            }

            // Step 3: concatenate 1st part of string with recursively generated remainder:
            int restOfNumber = value - bigPart * baseNum;
            return bigPartStr + separator + Convert(restOfNumber);
        }
    }
}
