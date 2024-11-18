using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Servaind.Intranet.Core
{
    public static class VersionControl
    {
        public static string GetVersion()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;

            return String.Format("{0}.{1}", v.Major, v.Minor);
        }
    }
}
