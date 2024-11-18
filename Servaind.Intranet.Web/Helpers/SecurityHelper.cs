using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proser.Common;
using Servaind.Intranet.Core;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Web.Helpers
{
    public enum SeccionPagina
    {
        Sistemas,
        Deposito_Instrumentos,
        General_Repositorio,
        Sgi_Multisitio,
        Administracion,
        Administracion_Vehiculos,
        Control_Acceso,
        Oportunidad_Mejora
    }

    public static class SecurityHelper
    {
        public const string LOGIN_URL = "/Usuario/Login";
        public const string ACCESS_DENIED_URL = "/Error/AccessDenied";

        // Properties.
        public static int CurrentPersonaId => CookieHelper.GetCurrentAccount().UserId;

        public static Persona CurrentPersona
        {
            get
            {
                int currentId = CookieHelper.GetCurrentAccount().UserId;
                if (currentId == Constants.InvalidInt) return Persona.Invalida();

                return Persona.Read(currentId);
            }
        }


        public static string BaseUrl()
        {
            HttpRequest request = HttpContext.Current.Request;

            return string.Format("{0}://{1}/", request.Url.Scheme, request.Url.Authority); ;
        }

        public static void Login(string username, string password, bool remember)
        {
            Persona p = UsuarioHelper.Login(username, password);
            Login(p, remember);
        }

        public static void Login(int id)
        {
            Persona p = Persona.Read(id);
            Login(p, true);
        }

        private static void Login(Persona p, bool remember)
        {
            if(p == null || p.IsInvalida) throw new InvalidOperationException();

            CookieHelper.UpdateUsuario(p.Id);
        }

        public static void Logout()
        {
            CookieHelper.Delete();
        }

        public static bool TieneAcceso(SeccionPagina s)
        {
            List<PermisosPersona> permisosPersona = CurrentPersona.Permisos;
          
            bool tieneAcceso = false;

            switch (s)
            {
                case SeccionPagina.Control_Acceso:      
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.ControlAcceso);

                    break;
                case SeccionPagina.Sistemas:
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.Administrador);
                    
                    break;
                case SeccionPagina.Deposito_Instrumentos:
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.InstrumentoSeguimiento);
                    
                    break;
                case SeccionPagina.General_Repositorio:
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.RDAVer) || permisosPersona.Contains(PermisosPersona.RDACrear);
                    
                    break;
                case SeccionPagina.Administracion:
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.VehicAdmin) || permisosPersona.Contains(PermisosPersona.VehicVer);
                    
                    break;
                case SeccionPagina.Administracion_Vehiculos:
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.VehicAdmin) || permisosPersona.Contains(PermisosPersona.VehicVer);
                    
                    break;
                case SeccionPagina.Sgi_Multisitio:

                    break;
                case SeccionPagina.Oportunidad_Mejora:
                    tieneAcceso = permisosPersona.Contains(PermisosPersona.CAL_Responsable);

                    break;
            }

            return permisosPersona.Contains(PermisosPersona.Administrador) || tieneAcceso;
        }
    }
}