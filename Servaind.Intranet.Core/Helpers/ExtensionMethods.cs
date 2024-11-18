using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proser.Common;

namespace Servaind.Intranet.Core.Helpers
{
    public static class ExtensionMethods
    {
        public static string ToFileSize(this long l)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }

        public static string ReplaceEnters(this string cadena)
        {
            char[] c = cadena.ToCharArray();
            string texto = "";
            foreach (char t in c)
            {
                if (t == 13 || t == '\n')
                {
                    texto += "<br>";
                }
                else
                {
                    texto += t;
                }
            }

            return texto;
        }

        public static string ReverseEnters(this string cadena)
        {
            return cadena.Replace("<br>", "\n");
        }

        public static string ToTitleCase(this string s)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string ToShortString(this DateTime d)
        {
            return d == Constants.InvalidDateTime ? "-" : d.ToString("dd/MM/yyyy");
        }
    }
}
