
namespace OnlineMahalla.Common.Utility
{
    public static class DateTimeUtility
    {
        public static DateTime? ToNullable(object input)
        {
            if (input == null)
                return (DateTime?)null;
            if (input.ToString() == "")
                return (DateTime?)null;
            return Convert.ToDateTime(input);
        }
        public static DateTime StartOfDay(this DateTime DateIn)
        {
            return new DateTime(DateIn.Year, DateIn.Month, DateIn.Day, 0, 0, 0);
        }
        public static DateTime EndOfDay(this DateTime DateIn)
        {
            return new DateTime(DateIn.Year, DateIn.Month, DateIn.Day, 23, 59, 59);
        }
        public static DateTime FirstDayOfNextMonth(this DateTime DateIn)
        {
            if (DateIn.Month == 12)
                return new DateTime(DateIn.Year + 1, 1, 1);
            else
                return new DateTime(DateIn.Year, DateIn.Month + 1, 1);
        }
        public static DateTime FirstDayOfNextMonth(this DateTime DateIn, bool IsStartDay)
        {
            if (DateIn.Month == 12)
            {
                if (IsStartDay)
                    return new DateTime(DateIn.Year + 1, 1, 1, 0, 0, 0);
                else
                    return new DateTime(DateIn.Year + 1, 1, 1, DateIn.Hour, DateIn.Minute, DateIn.Second);
            }
            else
            {
                if (IsStartDay)
                    return new DateTime(DateIn.Year, DateIn.Month + 1, 1, 0, 0, 0);
                else
                    return new DateTime(DateIn.Year, DateIn.Month + 1, 1, DateIn.Hour, DateIn.Minute, DateIn.Second);
            }
        }

        public static DateTime FirstDayOfMonth(this DateTime DateIn)
        {
            return FirstDayOfMonth(DateIn, true);
        }
        public static DateTime FirstDayOfMonth(this DateTime DateIn, bool IsStartDay)
        {
            if (IsStartDay)
                return new DateTime(DateIn.Year, DateIn.Month, 1, 0, 0, 0);
            else
                return new DateTime(DateIn.Year, DateIn.Month, 1, DateIn.Hour, DateIn.Minute, DateIn.Second);
        }
        public static System.DateTime LastDayOfMonth(this System.DateTime DateIn)
        {
            return LastDayOfMonth(DateIn, true);
        }
        public static System.DateTime LastDayOfMonth(this System.DateTime DateIn, bool IsEndDay)
        {
            if (IsEndDay)
                return new DateTime(DateIn.Year, DateIn.Month, DateTime.DaysInMonth(DateIn.Year, DateIn.Month), 23, 59, 59);
            else
                return new DateTime(DateIn.Year, DateIn.Month, DateTime.DaysInMonth(DateIn.Year, DateIn.Month), DateIn.Hour, DateIn.Minute, DateIn.Second);
        }
        public static int QuarterOfDay(this DateTime DateIn)
        {
            return (int)((DateIn.Month - 1) / 3) + 1;
        }
        public static DateTime FirstDayOfQuarter(this DateTime DateIn)
        {
            int intQuarterNum = (DateIn.Month - 1) / 3 + 1;
            return new DateTime(DateIn.Year, 3 * intQuarterNum - 2, 1);
        }
        public static System.DateTime LastDayOfQuarter(this System.DateTime DateIn)
        {
            return LastDayOfQuarter(DateIn, true);
        }
        public static System.DateTime LastDayOfQuarter(this System.DateTime DateIn, bool IsEndOfDay)
        {
            int intQuarterNum = (DateIn.Month - 1) / 3 + 1;
            if (IsEndOfDay)
                return new DateTime(DateIn.Year, 3 * intQuarterNum + 1, DateTime.DaysInMonth(DateIn.Year, DateIn.Month), 23, 59, 59);
            else
                return new DateTime(DateIn.Year, 3 * intQuarterNum + 1, DateTime.DaysInMonth(DateIn.Year, DateIn.Month), DateIn.Hour, DateIn.Minute, DateIn.Second);
        }
        public static DateTime FirstDayOfQuarter(int intQuarterNum, int yearindex)
        {
            return new DateTime(yearindex, 3 * intQuarterNum - 2, 1);
        }
        public static System.DateTime LastDayOfQuarter(int intQuarterNum, int yearindex)
        {
            return LastDayOfQuarter(intQuarterNum, yearindex, true);
        }
        public static System.DateTime LastDayOfQuarter(int intQuarterNum, int yearindex, bool IsEndOfDay)
        {
            int monthindex = intQuarterNum * 3;
            if (IsEndOfDay)
                return new DateTime(yearindex, 3 * intQuarterNum, DateTime.DaysInMonth(yearindex, monthindex), 23, 59, 59);
            else
                return new DateTime(yearindex, 3 * intQuarterNum, DateTime.DaysInMonth(yearindex, monthindex), 0, 0, 0);
        }
        public static System.DateTime FirstDayOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1, 0, 0, 0);
        }
        public static string GetMonthName(int monthNum, bool abbreviate)
        {
            if (monthNum < 1 || monthNum > 12)
                return "";
            DateTime date = new DateTime(1, monthNum, 1);
            if (abbreviate)
                return date.ToString("MMM");
            else
                return date.ToString("MMMM");
        }
        public static string GetMonthName(this DateTime dtSelDate, bool abbreviate)
        {
            string[] Months = new string[]{"Январь",
            "Февраль", "Март", "Апрель","Май","Июнь","Июль","Август","Сентябрь","Октябрь","Ноябрь","Декабрь"};

            if (abbreviate)
                return dtSelDate.ToString("MMM");
            else
                return Months[dtSelDate.Month - 1];
        }
        public static string GetMonthYearName(this DateTime dtSelDate, bool abbreviate)
        {
            if (abbreviate)
                return dtSelDate.ToString("MMM.yyyy г.");
            else
                return GetMonthName(dtSelDate, false) + dtSelDate.ToString(".yyyy г.");
        }

        public static List<DateTime> GetDayListFromPeriod(DateTime StartDate, DateTime EndDate)
        {
            if (EndDate < StartDate)
                return new List<DateTime>();

            List<DateTime> res = new List<DateTime>();
            while (true)
            {
                res.Add(StartDate);
                if (StartDate >= EndDate)
                    break;
                StartDate = StartDate.AddDays(1);
            }
            return res;
        }
        public static System.DateTime LastDayOfYear(this DateTime DateIn)
        {
            return new DateTime(DateIn.Year, 12, 31, 23, 59, 59);
        }
    }

    public static class BoolUtility
    {
        public static bool? ToNullable(object input)
        {
            if (input == null)
                return (bool?)null;
            if (input.ToString() == "")
                return (bool?)null;
            return Convert.ToBoolean(input);
        }
    }
}
