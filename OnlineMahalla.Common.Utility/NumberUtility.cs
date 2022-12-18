
using System.Text.RegularExpressions;


namespace OnlineMahalla.Common.Utility
{
    public class NumberToWord
    {
        static public string TriadInWords(string Triad, short Gender, string Unit1, string Unit2, string Unit5, bool IsUZB)
        {
            string returnValue;
            string Result;
            if (Triad == "000")
            {
                returnValue = "";
            }
            else
            {
                Result = new string[] { "", " сто", " двести", " триста", " четыреста", " пятьсот", " шестьсот", " семьсот", " восемьсот", " девятьсот" }[System.Convert.ToInt32(Triad.Substring(0, 1)) + 1 - 1];
                if (IsUZB)
                    Result = new string[] { "", " бир юз", " икки юз", " уч юз", " тўрт юз", " беш юз", " олти юз", " етти юз", " саккиз юз", " тўққиз юз" }[System.Convert.ToInt32(Triad.Substring(0, 1)) + 1 - 1];

                if (System.Convert.ToDouble(Triad.Substring(1, 1)) == 1)
                {
                    if (IsUZB)
                        Result = Result + " " + new string[] { "ўн", "ўн бир", "ўн икки", "ўн уч", "ўн тўрт", "ўн беш", "ўн олти", "ўн етти", "ўн саккиз", "ўн тўккиз" }[System.Convert.ToInt32(Triad.Substring(2, 1)) + 1 - 1] + " " + Unit5;
                    else
                        Result = Result + " " + new string[] { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" }[System.Convert.ToInt32(Triad.Substring(2, 1)) + 1 - 1] + " " + Unit5;
                }
                else
                {
                    if (IsUZB)
                        Result = Result + new string[] { "", "", " йигирма", " ўттиз", " қирқ", " эллик", " олтмиш", " етмиш", " саксон", " тўқсон" }[System.Convert.ToInt32(Triad.Substring(1, 1)) + 1 - 1];
                    else
                        Result = Result + new string[] { "", "", " двадцать", " тридцать", " сорок", " пятьдесят", " шестьдесят", " семьдесят", " восемьдесят", " девяносто" }[System.Convert.ToInt32(Triad.Substring(1, 1)) + 1 - 1];
                    switch (Triad.Substring(2, 1))
                    {
                        default:
                            if (Triad.Substring(2, 1) == 4.ToString())
                            {

                                if (IsUZB)
                                    Result = Result + " тўрт " + Unit2;
                                else
                                    Result = Result + " четыре " + Unit2;
                                break;
                            }
                            if (Triad.Substring(2, 1) == 3.ToString())
                            {

                                if (IsUZB)
                                    Result = Result + " уч " + Unit2;
                                else
                                    Result = Result + " три " + Unit2;
                                break;
                            }
                            if (Triad.Substring(2, 1) == 2.ToString())
                            {

                                if (IsUZB)
                                    Result = Result + " икки " + Unit2;
                                else
                                    Result = Result + new string[] { " два ", " две ", " два " }[Gender - 1] + Unit2;
                                break;
                            }
                            if (Triad.Substring(2, 1) == 1.ToString())
                            {

                                if (IsUZB)
                                    Result = Result + " бир " + Unit1;
                                else
                                    Result = Result + new string[] { " один ", " одна ", " одно " }[Gender - 1] + Unit1;
                                break;
                            }

                            if (IsUZB)
                                Result = Result + new string[] { "", "", "", "", "", " беш", " олти", " етти", " саккиз", " тўққиз" }[System.Convert.ToInt32(Triad.Substring(2, 1)) + 1 - 1] + " " + Unit5;
                            else
                                Result = Result + new string[] { "", "", "", "", "", " пять", " шесть", " семь", " восемь", " девять" }[System.Convert.ToInt32(Triad.Substring(2, 1)) + 1 - 1] + " " + Unit5;
                            break;
                    }
                }
                returnValue = Result;
            }
            return returnValue;
        }

        static public object NumberInWords(object Number, ref short Gender, ref string Unit1, ref string Unit2, ref string Unit5, bool IsUZB)
        {
            object returnValue;
            string Image;
            string Modulus;
            if (Number == DBNull.Value)
            {
                returnValue = System.DBNull.Value;
            }
            else
            {
                double tempnumber = 0;
                double.TryParse(Number.ToString(), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out tempnumber);
                Image = System.Math.Abs(tempnumber).ToString(new string('0', 15));
                if (System.Convert.ToDouble(Image) == 0)
                {
                    returnValue = (IsUZB ? "нол " : "ноль ") + Unit5.Trim();
                }
                else
                {
                    if (Image.Length > 15)
                    {
                        Modulus = " <много> " + Unit5;
                    }
                    else
                    {
                        Modulus = TriadInWords(Image.Substring(0, 3), 1, (IsUZB ? "триллион" : "триллион"), (IsUZB ? "триллион" : "триллионa"), (IsUZB ? "триллион" : "триллионов"), IsUZB) + TriadInWords(Image.Substring(3, 3), 1, (IsUZB ? "миллиард" : "миллиард"), (IsUZB ? "миллиард" : "миллиарда"), (IsUZB ? "миллиард" : "миллиардов"), IsUZB) + TriadInWords(Image.Substring(6, 3), 1, (IsUZB ? "миллион" : "миллион"), (IsUZB ? "миллион" : "миллиона"), (IsUZB ? "миллион" : "миллионов"), IsUZB) + TriadInWords(Image.Substring(9, 3), 2, (IsUZB ? "минг" : "тысяча"), (IsUZB ? "минг" : "тысячи"), (IsUZB ? "минг" : "тысяч"), IsUZB) + ((Image.Substring(12, 3) == "000") ? " " + Unit5 : (TriadInWords(Image.Substring(12, 3), Gender, Unit1, Unit2, Unit5, IsUZB)));
                    }
                    returnValue = ((System.Convert.ToDouble(Number) < 0 ? "минус" : "") + Modulus).Trim();
                }
            }
            return returnValue;
        }

        static public object AmountInWords(object Amount, ref short Gender1, ref string Dollar1, ref string Dollar2, ref string Dollar5, short Gender2, string Cent1, string Cent2, string Cent5, bool IsUZB)
        {
            object returnValue;
            string Image;
            if (Amount == null)
            {
                returnValue = System.DBNull.Value;
            }
            else
            {
                Image = ((decimal)Amount).ToString("0.00");
                object temp_object = Gender1;
                short temp_short = (short)temp_object;
                object temp_object2 = Dollar1;
                string temp_string = (string)temp_object2;
                object temp_object3 = Dollar2;
                string temp_string2 = (string)temp_object3;
                object temp_object4 = Dollar5;
                string temp_string3 = (string)temp_object4;
                returnValue = ((Image.Substring(0, 1) == "-") ? "минус " : "") + NumberInWords(System.Math.Abs(System.Convert.ToDouble(Image.Substring(0, Image.Length - 3))), ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, IsUZB) + "  " + Image.Substring(Image.Length - 2, 2) + " " + Cent5;
            }
            return returnValue;
        }

        static public object IntegerInWords(ref object Number)
        {
            object returnValue;
            short temp_short = 1;
            string temp_string = "";
            string temp_string2 = "";
            string temp_string3 = "";
            returnValue = NumberInWords(Number, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, false);
            return returnValue;
        }

        static public object RoublesInWords(ref object Amount)
        {
            object returnValue;
            short temp_short = 1;
            string temp_string = "рубль";
            string temp_string2 = "рубля";
            string temp_string3 = "рублей";
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 2, "копейка", "копейки", "копеек", false);
            return returnValue;
        }

        static public string DollarsInWords(ref object Amount)
        {
            string returnValue;
            short temp_short = 1;
            string temp_string = "доллар";
            string temp_string2 = "доллара";
            string temp_string3 = "долларов";
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 1, "цент", "цента", "центов", false).ToString();
            return returnValue;
        }

        static public object MarksInWords(ref object Amount)
        {
            object returnValue;
            short temp_short = 2;
            string temp_string = "марка";
            string temp_string2 = "марки";
            string temp_string3 = "марок";
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 1, "пфеннинг", "пфеннинга", "пфеннингов", false);
            return returnValue;
        }
        static public object SumInWords(ref object Amount, bool IsUZB)
        {
            object returnValue;
            short temp_short = 1;
            string temp_string = "сум";
            string temp_string2 = "сум";
            string temp_string3 = "сум";
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 2, "тийин", "тийин", "тийин", IsUZB);
            return returnValue;
        }
        static public object DoubleInWords(ref object Amount, bool IsUZB)
        {
            object returnValue;
            short temp_short = 1;
            string temp_string = "";
            string temp_string2 = "";
            string temp_string3 = "";
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 2, "", "", "", IsUZB);
            return returnValue;
        }
        //static public string SumInWordsCapital(decimal Amount)
        //{
        //    return SumInWordsCapital(Amount, false);

        //}
        static public string SumInWordsCapital(decimal Amount, bool IsUZB = false)
        {
            object returnValue;
            short temp_short = 1;
            string temp_string = (IsUZB ? "сўм" : "сум");
            string temp_string2 = (IsUZB ? "сўм" : "сум");
            string temp_string3 = (IsUZB ? "сўм" : "сум");
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 2, "тийин", "тийин", "тийин", IsUZB);
            return returnValue.ToString().Substring(0, 1).ToUpper() + returnValue.ToString().Substring(1);

        }

        static public string QuantityInWordsCapital(decimal Amount, bool IsUZB)
        {
            object returnValue;
            short temp_short = 1;
            string temp_string = (IsUZB ? "сўм" : "сум");
            string temp_string2 = (IsUZB ? "сўм" : "сум");
            string temp_string3 = (IsUZB ? "сўм" : "сум");
            returnValue = AmountInWords(Amount, ref temp_short, ref temp_string, ref temp_string2, ref temp_string3, 2, "тийин", "тийин", "тийин", IsUZB);
            return (returnValue.ToString().Substring(0, 1).ToUpper() + returnValue.ToString().Substring(1)).Replace(" сум  00 тийин", "");

        }
        static public string RemoveDecimalFloats(decimal number)
        {
            return number.ToString().Substring(0, number.ToString().Length - 3);
        }

        public static bool IsNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }
    }

    public static class NumberUtility
    {
        public static decimal Round(decimal d, int decimals)
        {
            decimal degree = (decimal)Math.Pow(10, decimals);
            decimal temp = RoundDown(d, decimals);
            if ((d - temp) * degree >= 0.5m)
                return temp + 1 / degree;
            else
                return temp;
        }

        public static decimal RoundDown(decimal d, int decimals)
        {
            decimal degree = (decimal)Math.Pow(10, decimals);
            return (decimal)(Math.Floor((double)(d * degree)) / (double)degree);
        }

        public static decimal RoundDown(decimal d, decimal degree)
        {
            return (decimal)(Math.Floor((double)(d * degree)) / (double)degree);
        }
        public static decimal ObjecttoDecimal(object val)
        {
            decimal d = 0.0m;
            if (val == null || val == DBNull.Value)
                d = 0.0m;
            else
            {
                val = val.ToString().Replace("/", ",").Replace(@"\", ",");
                decimal.TryParse(val.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign, System.Globalization.CultureInfo.CurrentCulture, out d);
            }
            return d;

        }
        public static decimal ToDecimal(this object val)
        {
            return ObjecttoDecimal(val);

        }
    }

}