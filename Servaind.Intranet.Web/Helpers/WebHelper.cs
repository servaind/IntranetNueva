using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using Proser.Common.Utils;

namespace Servaind.Intranet.Web.Helpers
{
    public class CookieConfig
    {
        // Propiedades.
        public bool EnableDomain { get; set; } = false;
        public string Nombre { get; set; }
        public string Dominio { get; set; }
    }

    public class CookieInfoBase
    {

    }

    public static class WebHelper
    {
        // Constantes.
        private const string COOKIE_VALUE = "VALUE";

        // Propiedades.
        private static CookieConfig cookieConfig;


        public static string GetBaseUrl()
        {
            return $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority‌​)}{HttpContext.Current.Request.ApplicationPath​.TrimEnd('/')}/";
        }

        public static void Init(CookieConfig cookieCfg)
        {
            cookieConfig = cookieCfg;
        }

        public static void SaveCookie(CookieInfoBase c)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieConfig.Nombre);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieConfig.Nombre) { Expires = DateTime.Now.AddDays(30) };
                if (cookieConfig.EnableDomain) cookie.Domain = cookieConfig.Dominio;
            }
            var ci = c.ToXmlDocument();
            cookie.Value = Security.Encrypt(ci.OuterXml);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void DeleteCookie()
        {
            var cookie = new HttpCookie(cookieConfig.Nombre) { Expires = DateTime.Now.AddDays(-1) };
            if (cookieConfig.EnableDomain) cookie.Domain = cookieConfig.Dominio;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static CookieInfoBase ReadCookie(Type cookieType, string nombre)
        {
            CookieInfoBase result = null;

            var cookie = HttpContext.Current.Request.Cookies.Get(nombre);
            if (cookie != null)
            {
                try
                {
                    var xml = Security.Decrypt(cookie.Value);
                    var _serializer = new XmlSerializer(typeof(CookieInfoBase), new[] { cookieType });
                    using (var _reader = new StringReader(xml))
                    {
                        result = (CookieInfoBase)_serializer.Deserialize(_reader);
                    }
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }
    }
}