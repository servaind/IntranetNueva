using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
                //WriteToEventLog("to: " + to + " - " + "cc: " + cc, EventLogEntryType.Information);

                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(from, from));

                var destinatarios = to.Split(',');
                foreach (var destinatario in destinatarios)
                {
                    if (destinatario.Trim() != string.Empty)
                    {
                        mensaje.To.Add(new MailboxAddress(destinatario, destinatario));
                    }
                }

                var copias = cc.Split(',');
                foreach (var copia in copias)
                {
                    if (copia.Trim() != string.Empty)
                    {
                        mensaje.Cc.Add(new MailboxAddress(copia, copia));
                    }
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
                LogError(ex);
                throw new Exception(string.Format("No se pudo enviar el email. To: {0} CC: {1} ", to, cc) + " - " + ex.Message);
            }
        }

        private static void LogError(Exception ex)
        {
            string logFilePath = @"C:\Logs\Servaind.Intranet.Service.log"; // Ruta donde se almacenarán los logs
            string errorMessage = $"[{DateTime.Now}] ERROR: {ex.Message}\n{ex.StackTrace}\n";

            // 1. Escribir en un archivo de texto
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)); // Crear carpeta si no existe
                File.AppendAllText(logFilePath, errorMessage);
            }
            catch (Exception fileEx)
            {
                // Si no se puede escribir en el archivo, registrar en el visor de eventos
                WriteToEventLog($"Error al escribir en el archivo de log: {fileEx.Message}", EventLogEntryType.Error);
            }

            // 2. Escribir en el Visor de Eventos
            WriteToEventLog(errorMessage, EventLogEntryType.Error);
        }

        private static void WriteToEventLog(string message, EventLogEntryType type)
        {
            string source = "Servaind.Intranet.2024"; // Cambia esto por el nombre de tu servicio
            string logName = "Application";

            try
            {
                // Crear la fuente en el Visor de Eventos si no existe
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }

                // Escribir el mensaje en el Visor de Eventos
                EventLog.WriteEntry(source, message, type);
            }
            catch
            {
                // Si falla el registro en el Visor de Eventos, no queremos que afecte la ejecución
            }
        }

    }
}
