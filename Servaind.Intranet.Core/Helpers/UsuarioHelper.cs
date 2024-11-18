using System;
using System.DirectoryServices;
using Proser.Common;

namespace Servaind.Intranet.Core.Helpers
{
    public static class UsuarioHelper
    {
        // Constantes.
        internal const string DOMINIO = "DOMINIO";
        private const string DOMINIO_SRV = "10.0.0.2";


        public static Persona Login(int id)
        {
            Persona result = Persona.Read(id);
            if (result == null) throw new Exception("El usuario no se encuentra registrado en el sistema.");

            return result;
        }

        public static Persona Login(string usuario, string pwd)
        {

#if !DEBUG

            string dominioUsuario = String.Format("{0}\\{1}", DOMINIO, usuario);
            DirectoryEntry entrada = new DirectoryEntry(
                "LDAP://" + DOMINIO_SRV, dominioUsuario, pwd);

            try
            {
                // Fuerzo la autentificación.
                object obj = entrada.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entrada)
                {
                    Filter = "(SAMAccountName=" + usuario + ")"
                };

                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (result == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception("El usuario o contraseña no son validos.");
            }
#endif

            var usr = Persona.Read(usuario);
            if (usr == null) throw new Exception("El usuario no se encuentra registrado en el sistema.");

            return usr;
        }
    }
}
