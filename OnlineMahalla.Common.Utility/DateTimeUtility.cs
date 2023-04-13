
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
    }
}
