using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Proser.Common.Extensions;
using Proser.Communications.Network.Mailing;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    internal enum EmailTemplateTipo
    {
        [Description("success")]
        Ok,
        [Description("error")]
        Error,
        [Description("info")]
        Info
    }

    internal abstract class EmailTemplate
    {
        // Variables.
        private EmailTemplateTipo tipo;
        private string encabezado;
        private List<Tuple<string, string>> items;
        private string linkAcceso;
        private string linkAccesoDesc;


        protected EmailTemplate()
        {
            tipo = EmailTemplateTipo.Info;
            encabezado = String.Empty;
            items = new List<Tuple<string, string>>();
            linkAcceso = String.Empty;
            linkAccesoDesc = String.Empty;
        }

        protected void SetTipo(EmailTemplateTipo tipo)
        {
            this.tipo = tipo;
        }

        protected void SetEncabezado(string encabezado)
        {
            this.encabezado = encabezado;
        }

        protected void SetLinkAcceso(string linkAcceso, string descripcionPara)
        {
            this.linkAcceso = linkAcceso;
            linkAccesoDesc = descripcionPara;
        }

        protected void AddItem(string name, string value)
        {
            items.Add(new Tuple<string, string>(name, value));
        }

        private string Build()
        {
            string template = Properties.Resources.TemplateBase;
            string contenido = BuildContenido();

            template = template.Replace("@CONTENIDO", contenido);

            return template;
        }

        private string BuildContenido()
        {
            StringBuilder result = new StringBuilder();

            // Encabezado.
            if (!String.IsNullOrWhiteSpace(encabezado))
            {
                result.AppendLine(String.Format("<p>{0}</p>", encabezado));
            }
            
            // Items.
            result.AppendLine(String.Format("<div class=\"{0}\">", tipo.GetDescription()));
                result.AppendLine("<div class=\"header\">");
                    items.ForEach(item =>
                    {
                        if (String.IsNullOrEmpty(item.Item1))
                        {
                            result.AppendLine("<br/>");
                        }
                        else
                        {
                            result.AppendLine(String.Format(
                                    "&nbsp;<span class=\"item\">{0}{1} </span><span class=\"content\">{2}</span><br />",
                                    item.Item1, String.IsNullOrWhiteSpace(item.Item2) ? "" : ":", item.Item2));
                        }
                    });
                 result.AppendLine("</div>");
            result.AppendLine("</div>");

            // Link de acceso.
            if (!String.IsNullOrWhiteSpace(linkAcceso))
            {
                result.AppendLine(String.Format("<p>Haga click <a href=\"{0}\">aqui</a> para {1}.</p>", linkAcceso, linkAccesoDesc));
            }

            return result.ToString();
        }

        public void SendFromIntranet(string para, string cc, string asunto, List<Attachment> adjuntos = null)
        {
            string body = Build();

            EmailHelper.SendFromIntranet(para, cc, asunto, body, adjuntos);
        }

        public void Send(string de, string para, string cc, string asunto, List<Attachment> adjuntos = null)
        {
            string body = Build();

            EmailHelper.Send(de, para, cc, asunto, body, adjuntos);
        }
    }
}
