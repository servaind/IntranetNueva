using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proser.Communications.Network.Mailing;

namespace Servaind.Intranet.Core.Helpers
{
    public static class EmailHelper
    {
        // Constants.
        private const string DEFAULT_SERVER = "mailsrv1";
        private const string DEFAULT_USER = "dominio\\intranet";
        private const string DEFAULT_PWD = "gador.1";
        private const string DEFAULT_SENDER = "intranet@servaind.com";


        public static void SendFromIntranet(string to, string cc, string subject, string body,
            List<Attachment> attachments = null)
        {
            Send(DEFAULT_SENDER, to, cc, subject, body, attachments);
        }

        public static void Send(string from, string to, string cc, string subject, string body, 
            List<Attachment> attachments = null)
        {
            EmailCredential credentials = new EmailCredential(DEFAULT_SERVER, DEFAULT_USER, DEFAULT_PWD);

            try
            {
                Email.Send(DEFAULT_SENDER, to, cc, subject, body, attachments, credentials, true);
            }
            catch(Exception ex)
            {
                throw new Exception("No se pudo enviar el email. " + ex.Message);
            }
        }
    }
}
