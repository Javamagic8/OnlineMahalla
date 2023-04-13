
using System.Text.RegularExpressions;


namespace OnlineMahalla.Common.Utility
{
    public class NumberToWord
    {

        public static bool IsNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }
    }
}