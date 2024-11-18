using System;

namespace Servaind.Intranet.Core
{
    public class IntervaloAsignadoException : Exception
    {
        public IntervaloAsignadoException()
            : base("El intervalo ingresado no es válido.")
        {

        }
    }
    public class ElementoExistenteException : Exception
    {
        public ElementoExistenteException()
            : base("El elemento ya se encuentra presente.")
        {

        }
    }
    public class NoHayItemsException : Exception
    {
        public NoHayItemsException()
            : base("No se encontró ningún elemento.")
        {

        }
    }
    public class EmailException : Exception
    {
        public EmailException()
            : base("No se pudo enviar el e-mail.")
        {

        }
        public EmailException(string msg)
           : base("No se pudo enviar el e-mail. Datalle: " + msg)
        {

        }
    }
    public class ErrorOperacionException : Exception
    {
        public ErrorOperacionException()
            : base("No se pudo completar la operación.")
        {

        }
    }
    public class PlantillaInexistenteException : Exception
    {
        public PlantillaInexistenteException()
            : base("No se encontró la plantilla necesaria.")
        {

        }
    }
    public class ElementoInexistenteException : Exception
    {
        public ElementoInexistenteException()
            : base("No se encontró el elemento.")
        {

        }
    }
    public class DatosInvalidosException : Exception
    {
        public DatosInvalidosException()
            : base("Los datos ingresados no son válidos.")
        {

        }
    }
    public class PrivilegiosException : Exception
    {
        public PrivilegiosException()
            : base("No posee los privilegios necesarios para realizar la operación.")
        {

        }
    }
    public class LoginException : Exception
    {
        public LoginException()
            : base("El nombre de usuario o contraseña no es válido.")
        {

        }
    }
}