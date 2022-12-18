using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Model
{
    public class DbConfiguration
    {
        public string ConnectionString { get; set; }
        public string SecondaryConnectionString { get; set; }
    }
    public static class CheckModelUtil
    {
        public static bool CheckINNAlgorithm(string INN)
        {
            if (INN.Length != 9)
                return false;
            if (!IsNumber(INN))
                return false;

            char[] inndigits = INN.ToArray();
            decimal checksum = (decimal)(Convert.ToInt32(inndigits[0].ToString()) * 37 +
                Convert.ToInt32(inndigits[1].ToString()) * 29 +
                Convert.ToInt32(inndigits[2].ToString()) * 23 +
                Convert.ToInt32(inndigits[3].ToString()) * 19 +
                Convert.ToInt32(inndigits[4].ToString()) * 17 +
                Convert.ToInt32(inndigits[5].ToString()) * 13 +
                Convert.ToInt32(inndigits[6].ToString()) * 7 +
                Convert.ToInt32(inndigits[7].ToString()) * 3) / ((decimal)11);
            checksum = RoundDown((9 - (checksum - RoundDown(checksum, 0)) * 9), 0);

            if (Convert.ToInt32(checksum) != Convert.ToInt32(inndigits[8].ToString()))
                return false;
            else
                return true;
        }
        public static bool CheckSettlementCode(string SettlementCode, string BankCode)
        {
            if (SettlementCode.Length != 20)
                return false;
            if (!IsNumber(SettlementCode))
                return false;

            string code = BankCode + SettlementCode.Substring(0, 8) + SettlementCode.Substring(9) + "9";
            int sum = 0;
            char[] accdigits = code.ToArray();
            for (int i = 0; i < 24; i++)
            {
                sum += Convert.ToInt32(accdigits[i].ToString()) * Convert.ToInt32(accdigits[i + 1].ToString());
            }

            sum = sum % 11;
            string key = "";
            if (sum == 0)
                key = "9";
            else if (sum == 1)
                key = "0";
            else
                key = (11 - sum).ToString();

            if (key != SettlementCode.Substring(8, 1))
                return false;
            else
                return true;
        }

        public static bool IsNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }
        public static decimal Round(decimal d, int decimals)
        {
            decimal degree = (decimal)Math.Pow(10, decimals);
            decimal temp = RoundDown(d, decimals);
            if ((d - temp) * degree >= 0.5m)
                return temp + 1 / degree;
            else
                return temp;
        }

        public static double RoundUp(double d, int decimals)
        {
            double degree = Math.Pow(10, decimals);
            return Math.Ceiling(d * degree) / degree;
        }
        public static double RoundDown(double d, int decimals)
        {
            double degree = Math.Pow(10, decimals);
            return Math.Floor(d * degree) / degree;
        }
        public static decimal RoundDown(decimal d, int decimals)
        {
            decimal degree = (decimal)Math.Pow(10, decimals);
            return (decimal)(Math.Floor((double)(d * degree)) / (double)degree);
        }

        public static double RoundEqual(double d, int decimals)
        {
            double d1 = Math.Pow(10, decimals) * d;
            if ((double)((int)d1) != Convert.ToDouble(d1.ToString("0.000000")))
                d1 = (int)d1 + 1;
            else
                d1 = (int)d1;
            return d1 / Math.Pow(10, decimals);
            //double d1=Convert.ToDouble()
        }
        public static decimal RoundEqual(decimal d, int decimals)
        {
            double d1 = Math.Pow(10, decimals) * (double)d;
            if ((double)((int)d1) != Convert.ToDouble(d1.ToString("0.000000")))
                d1 = (int)d1 + 1;
            else
                d1 = (int)d1;
            return (decimal)(d1 / Math.Pow(10, decimals));
            //double d1=Convert.ToDouble()
        }

    }

    public class MathParser
    {
        public enum FormulaParameters
        {
            InSum
        }

        private Dictionary<string, decimal> _Parameters = new Dictionary<string, decimal>();
        private Dictionary<string, string> _ParameterRussian = new Dictionary<string, string>();

        private List<String> OperationOrder = new List<string>();

        public Dictionary<string, decimal> Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        public MathParser()
        {
            OperationOrder.Add("/");
            OperationOrder.Add("*");
            OperationOrder.Add("-");
            OperationOrder.Add("+");

            _ParameterRussian.Add(FormulaParameters.InSum.ToString(), "СуммаИсходная");
        }
        public decimal Calculate(string Formula)
        {
            try
            {
                string[] arr = Formula.Split("/+-*()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (KeyValuePair<string, decimal> de in _Parameters)
                {
                    string paramKey = de.Key.ToString();
                    //paramKey = _ParameterRussian.First(x => x.Key == paramKey).Value;
                    foreach (string s in arr)
                    {
                        if (s != paramKey && s.EndsWith(paramKey))
                        {
                            Formula = Formula.Replace(s, (OnlineMahalla.Common.Utility.NumberUtility.ObjecttoDecimal(s.Replace(paramKey, "")) * de.Value).ToString());
                        }
                    }
                    Formula = Formula.Replace(paramKey, (de.Value >= 0 ? de.Value.ToString() : "0" + de.Value.ToString()));
                }
                while (Formula.LastIndexOf("(") > -1)
                {
                    int lastOpenPhrantesisIndex = Formula.LastIndexOf("(");
                    int firstClosePhrantesisIndexAfterLastOpened = Formula.IndexOf(")", lastOpenPhrantesisIndex);
                    decimal result = ProcessOperation(Formula.Substring(lastOpenPhrantesisIndex + 1, firstClosePhrantesisIndexAfterLastOpened - lastOpenPhrantesisIndex - 1));
                    bool AppendAsterix = false;
                    if (lastOpenPhrantesisIndex > 0)
                    {
                        if (Formula.Substring(lastOpenPhrantesisIndex - 1, 1) != "(" && !OperationOrder.Contains(Formula.Substring(lastOpenPhrantesisIndex - 1, 1)))
                        {
                            AppendAsterix = true;
                        }
                    }

                    Formula = Formula.Substring(0, lastOpenPhrantesisIndex) + (AppendAsterix ? "*" : "") + result.ToString() + Formula.Substring(firstClosePhrantesisIndexAfterLastOpened + 1);

                }
                return ProcessOperation(Formula);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured While Calculating. Check Syntax" + Formula, ex);
            }
        }

        private decimal ProcessOperation(string operation)
        {
            List<object> arr = new List<object>();
            string s = "";
            for (int i = 0; i < operation.Length; i++)
            {
                string currentCharacter = operation.Substring(i, 1);
                if (OperationOrder.IndexOf(currentCharacter) > -1)
                {
                    if (s != "")
                    {
                        arr.Add(s);
                    }
                    arr.Add(currentCharacter);
                    s = "";
                }
                else
                {
                    s += currentCharacter;
                }
            }
            arr.Add(s);
            s = "";
            foreach (string op in OperationOrder)
            {
                while (arr.IndexOf(op) > -1)
                {
                    int operatorIndex = arr.IndexOf(op);
                    decimal digitBeforeOperator = OnlineMahalla.Common.Utility.NumberUtility.ObjecttoDecimal(arr[operatorIndex - 1]);
                    decimal digitAfterOperator = 0;
                    if (arr[operatorIndex + 1].ToString() == "-")
                    {
                        arr.RemoveAt(operatorIndex + 1);
                        digitAfterOperator = OnlineMahalla.Common.Utility.NumberUtility.ObjecttoDecimal(arr[operatorIndex + 1]) * -1;
                    }
                    else
                    {
                        digitAfterOperator = OnlineMahalla.Common.Utility.NumberUtility.ObjecttoDecimal(arr[operatorIndex + 1]);
                    }
                    arr[operatorIndex] = CalculateByOperator(digitBeforeOperator, digitAfterOperator, op);
                    arr.RemoveAt(operatorIndex - 1);
                    arr.RemoveAt(operatorIndex);
                }
            }
            return OnlineMahalla.Common.Utility.NumberUtility.ObjecttoDecimal(arr[0]);
        }
        private decimal CalculateByOperator(decimal number1, decimal number2, string op)
        {
            if (op == "/")
            {
                return number1 / number2;
            }
            else if (op == "*")
            {
                return number1 * number2;
            }
            else if (op == "-")
            {
                return number1 - number2;
            }
            else if (op == "+")
            {
                return number1 + number2;
            }
            else
            {
                return 0;
            }
        }
    }
}
