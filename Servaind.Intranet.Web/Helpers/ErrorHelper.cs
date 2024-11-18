using System;
using System.Web;

namespace Servaind.Intranet.Web.Helpers
{
    public static class ErrorHelper
    {
        public static void HandleError(int code, string message)
        {
            //LogHelper.LogError(String.Format("[{0}] - {1}", code, message));

            string url = "/Error/";
            switch (code)
            {
                case 404:
                    url += "Error404";
                    break;
                default:
                    url += "Unknown";
                    break;
            }

            //url += "?message=" + message;

            HttpContext.Current.Response.Redirect(url, true);
        }
    }
}