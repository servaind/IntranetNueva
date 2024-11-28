using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Proser.Communications.Network.Mailing;

namespace Servaind.Intranet.Core.Helpers
{
    public static class EmailHelper
    {
        //18/11/2024-AGM: Tomo los valores de configuracion para envio de mail del App.config
        private static string DEFAULT_SERVER = ConfigurationManager.AppSettings["DEFAULT_SERVER"];
        private static int DEFAULT_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["DEFAULT_PORT"].ToString());
        private static string DEFAULT_USER = ConfigurationManager.AppSettings["DEFAULT_USER"];
        private static string DEFAULT_PWD = ConfigurationManager.AppSettings["DEFAULT_PWD"];
        private static string DEFAULT_SENDER = ConfigurationManager.AppSettings["DEFAULT_SENDER"];

        public static void SendFromIntranet(string to, string cc, string subject, string body,
            List<Attachment> attachments = null)
        {
            Send(DEFAULT_SENDER, to, cc, subject, body, attachments);
        }

        // 18/11/2024-AGM: Reemplazo de uso proser.Communications por librería mailkit para envío de mails
        public static void Send(string from, string to, string cc, string subject, string contenido, 
            List<Attachment> attachments = null)
        {
            try
            {
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(from, from));

                var destinatarios = to.Split(',');
                foreach (var destinatario in destinatarios)
                {
                    mensaje.To.Add(new MailboxAddress(destinatario, destinatario));
                }

                var copias = to.Split(',');
                foreach (var copia in copias)
                {
                    mensaje.Cc.Add(new MailboxAddress(copia, copia));
                }

                mensaje.Subject = subject;
                var body = new TextPart(TextFormat.Html)
                {
                    Text = contenido
                };

                var multipart = new Multipart("mixed");
                multipart.Add(body);

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        var mimePart = new MimePart("application", "octet-stream")
                        {
                            Content = new MimeContent(new MemoryStream(attachment.File), ContentEncoding.Default),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = attachment.Name
                        };
                        multipart.Add(mimePart);
                    }
                }

                mensaje.Body = multipart;

                var client = new SmtpClient();
                client.Connect(DEFAULT_SERVER, DEFAULT_PORT, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(DEFAULT_USER, DEFAULT_PWD);
                client.Send(mensaje);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo enviar el email. " + ex.Message);
            }
        }
    }
}
