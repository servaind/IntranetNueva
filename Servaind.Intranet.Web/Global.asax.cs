using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Proser.Common;
using Servaind.Intranet.Core;
using Servaind.Intranet.Core.Helpers;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Connection Strings.
            DataAccess.ConnectionStringIntranet = ConfigurationManager.ConnectionStrings["Servaind.Intranet"].ConnectionString;
            DataAccess.ConnectionStringTango = ConfigurationManager.ConnectionStrings["Tango"].ConnectionString;
            DataAccess.ConnectionStringProser = ConfigurationManager.ConnectionStrings["Proser"].ConnectionString;

            // Instrumentos.
            Instrumento.PATH_IMAGENES = ConfigurationManager.AppSettings["PathInstrumentosImg"];
            Instrumento.PATH_CERTIFICADOS = ConfigurationManager.AppSettings["PathInstrumentosCertificados"];
            Instrumento.PATH_MANUALES = ConfigurationManager.AppSettings["PathInstrumentosManuales"];
            Instrumento.PATH_EAC = ConfigurationManager.AppSettings["PathInstrumentosEAC"];
            Instrumento.PATH_COMPROB_MANT = ConfigurationManager.AppSettings["PathInstrumentosComprobMant"];

            VariablesHelper.RootPath = Server.MapPath("/");

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            var context = app.Context;

            // Cookies.
            CookieHelper.COOKIE_DOMAIN = Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
            CookieHelper.Init(ConfigurationManager.AppSettings["CookieName"]);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            return;

            Exception exception = Server.GetLastError();

            Response.Clear();

            HttpException httpException = exception as HttpException;

            int code;
            string message;
            if (httpException == null)
            {
                code = Constants.InvalidInt;
                message = "Unknown";
            }
            else
            {
                code = httpException.GetHttpCode();
                message = httpException.Message;
            }

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            // Handle the error.
            ErrorHelper.HandleError(code, message);
        }
    }
}