using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proser.Common.Extensions;
using Proser.Communications.Network.Mailing;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    public class OportunidadMejora
    {
        class OportunidadMejoraEmail : EmailTemplate
        {
            public OportunidadMejoraEmail(OportunidadMejora p)
            {
                SetTipo(EmailTemplateTipo.Info);

                // Items.
                AddItem("Sector", p.Area.Nombre);
                AddItem("Responsable", p.Responsable.Nombre);
                AddItem("Solicitante", p.Solicitante.Nombre);
                AddItem("Comentarios", p.Comentarios);
                AddItem("Urgencia", p.Urgencia.GetDescription());
            }
        }

        // Constants.
        private const int MAX_FILE_SIZE = 8388608;

        // Properties.
        public AreaPersonal Area { get; private set; }
        public Persona Responsable { get; private set; }
        public Persona Solicitante { get; private set; }
        public string Comentarios { get; private set; }
        public OportMejoraUrgencia Urgencia { get; private set; }
        public FileAttachment ArchivoAdjunto { get; private set; }


        public static void Enviar(int areaId, int responsableId, int solicitanteId, string comentarios,
            OportMejoraUrgencia urgencia, FileAttachment adjunto = null)
        {
            var area = AreaPersonal.Read(areaId);
            if (area == null) throw new Exception("El Sector no existe.");

            var responsable = Persona.Read(responsableId);
            if (responsable == null || responsable.IsInvalida) throw new Exception("El Responsable no existe.");

            var solicitante = Persona.Read(solicitanteId);
            if (solicitante == null || solicitante.IsInvalida) throw new Exception("El Solicitante no existe.");

            if (String.IsNullOrWhiteSpace(comentarios)) throw new Exception("No se han ingresado comentarios.");

            if (adjunto != null && adjunto.Contenido.Length > MAX_FILE_SIZE) throw new Exception("El archivo es demasiado grande.");

            try
            {
                Enviar(new OportunidadMejora
                {
                    Area = area,
                    Responsable = responsable,
                    Solicitante = solicitante,
                    Comentarios = comentarios,
                    Urgencia = urgencia,
                    ArchivoAdjunto = adjunto
                });
            }
            catch(Exception ex)
            {
                throw new Exception("Se produjo un error al intentar enviar la Propuesta de Cambio. Detalles: " + ex.Message);
            }
        }

        private static void Enviar(OportunidadMejora o)
        {
            OportunidadMejoraEmail email = new OportunidadMejoraEmail(o);

            List<Attachment> adjuntos = new List<Attachment>();
            if (o.ArchivoAdjunto != null)
            {
                adjuntos.Add(new Attachment(o.ArchivoAdjunto.Contenido, o.ArchivoAdjunto.Nombre));
            }

            string to = PermisoPersona.ListEmails(PermisosPersona.SGI_Responsable);
            email.SendFromIntranet(to, String.Empty, String.Format("Oportunidad de Mejora [Urgencia: {0}]", 
                o.Urgencia.GetDescription()), adjuntos);
        }
    }
}
