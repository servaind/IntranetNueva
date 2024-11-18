using Proser.Common.Extensions;

namespace Servaind.Intranet.Web.Helpers
{
    public static class NumberHelper
    {
        private static string DecimalFormat(int decimales)
        {
            string result = "0";

            if (decimales > 0)
            {
                result += ".";
                for (int i = 0; i < decimales; i++) result += "0";
            }

            return result;
        }

        public static string ToDecimal(decimal d, int decimales)
        {
            return d.ToString(DecimalFormat(decimales)).ToWebDecimal();
        }

        public static string ToDecimal(float d, int decimales)
        {
            return d.ToString(DecimalFormat(decimales)).ToWebDecimal();
        }

        public static string ToDecimal(int d, int decimales)
        {
            return d.ToString(DecimalFormat(decimales)).ToWebDecimal();
        }
    }
}