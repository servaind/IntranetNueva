using Proser.Common;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Models;

namespace Servaind.Intranet.Web.Helpers
{
    public class CookieInfo : CookieInfoBase
    {
        // Propiedades.
        public int UsuarioId { get; set; } = Constants.InvalidInt;


        public CookieInfo()
        {

        }
    }

    public static class CookieHelper
    {
        // Variables.
        public static string COOKIE_NAME { get; set; }
        public static string COOKIE_DOMAIN;


        public static Account GetCurrentAccount()
        {
            var result = new Account();

            var cookie = Read();

            var user = Persona.Read(cookie.UsuarioId);
            if (user != null && user.Activo)
            {
                result = new Account
                {
                    UserId = user.Id
                };
            }

            return result;
        }

        public static CookieInfo Read()
        {
            return (CookieInfo)(WebHelper.ReadCookie(typeof(CookieInfo), COOKIE_NAME) ?? new CookieInfo());
        }

        public static void Init(string cookieName)
        {
            COOKIE_NAME = cookieName;

            var cfg = new CookieConfig
            {
                Nombre = COOKIE_NAME,
                Dominio = COOKIE_DOMAIN
            };

            WebHelper.Init(cfg);
        }

        public static void Save(CookieInfo ci)
        {
            WebHelper.SaveCookie(ci);
        }

        public static void Delete()
        {
            WebHelper.DeleteCookie();
        }

        public static void UpdateUsuario(int id)
        {
            var cookie = Read();
            cookie.UsuarioId = id;
            Save(cookie);
        }
    }
}