using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Proser.Common;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class FormFg005Tests : TestBase
    {
        [TestCase]
        public void ReadTest()
        {
            int numero;
            var origen = FormFg005Origen.AuditoriaExterna;
            var asunto = "Asunto";
            var hallazgo = "Hallazgo";
            var accInmediata = "Accion Inmediata";
            var comentarios = "Comentarios";

            numero = FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo", "Accion Inmediata", "Comentarios");

            var form = FormFg005.Read(numero);
            Assert.IsNotNull(form);
            Assert.AreEqual(origen, form.Origen);
            Assert.AreEqual(asunto, form.Asunto);
            Assert.AreEqual(hallazgo, form.Hallazgo);
            Assert.AreEqual(accInmediata, form.AccInmediata);
            Assert.AreEqual(comentarios, form.Comentarios);
        }

        [TestCase]
        public void CreateTest()
        {
            int numero = 0;
            Assert.DoesNotThrow(() => numero = FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo",
                "Accion Inmediata", "Comentarios"));
            Assert.AreNotEqual(0, numero);

            var form = FormFg005.Read(numero);
            Assert.AreEqual(FormFg005Estado.ProcesandoSgi, form.Estado);
        }

        [TestCase]
        public void UpdateSgiTest()
        {
            bool norIso9001 = true;
            bool norIso14001 = false;
            bool norOhsas18001 = true;
            bool norIram301 = false;
            string apaIso9001 = "Apa ISO 9001";
            string apaIso14001 = "Vacío 1";
            string apaOhsas18001 = "Apa OHSAS 18001";
            string apaIram301 = "Vacío 2";
            int areaResponsabilidadId = 1;
            FormFg005Categoria categoria = FormFg005Categoria.Observacion;

            var numero = FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo", "Accion Inmediata", "Comentarios");

            Assert.DoesNotThrow(() => FormFg005.UpdateSgi(numero, norIso9001, norIso14001, norOhsas18001, norIram301,
                apaIso9001, apaIso14001, apaOhsas18001, apaIram301, areaResponsabilidadId, categoria));

            var form = FormFg005.Read(numero);

            Assert.AreEqual(norIso9001, form.NorIso9001);
            Assert.AreEqual(norIso14001, form.NorIso14001);
            Assert.AreEqual(norOhsas18001, form.NorOhsas18001);
            Assert.AreEqual(norIram301, form.NorIram301);
            Assert.AreEqual(apaIso9001, form.ApaIso9001);
            Assert.AreEqual(apaIso14001, form.ApaIso14001);
            Assert.AreEqual(apaOhsas18001, form.ApaOhsas18001);
            Assert.AreEqual(apaIram301, form.ApaIram301);
            Assert.AreEqual(areaResponsabilidadId, form.AreaResponsabilidadId);
            Assert.AreEqual(categoria, form.Categoria);
            Assert.AreEqual(FormFg005Estado.ProcesandoResponsable, form.Estado);
        }

        [TestCase]
        public void UpdateResponsableTest()
        {
            var causasRaices = "Causas Raices";
            string accCorr = "Accion Correctiva";
            int accCorrRespId = 89;
            DateTime accCorrFechaFin = new DateTime(2016, 02, 15);
            string accPrev = "Accion Preventiva";
            int accPrevRespId = 75;
            DateTime accPrevFechaFin = new DateTime(2016, 02, 18);

            var numero = FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo", "Accion Inmediata", "Comentarios");

            Assert.DoesNotThrow(() => FormFg005.UpdateResponsable(numero, causasRaices, accCorr, accCorrRespId, 
                accCorrFechaFin, accPrev, accPrevRespId, accPrevFechaFin));

            var form = FormFg005.Read(numero);

            Assert.IsNotNull(form.DatosResponsable);
            Assert.AreEqual(causasRaices, form.DatosResponsable.CausasRaises);
            Assert.AreEqual(accCorr, form.DatosResponsable.AccCorr);
            Assert.AreEqual(accCorrRespId, form.DatosResponsable.AccCorrRespId);
            Assert.AreEqual(accCorrFechaFin, form.DatosResponsable.AccCorrFin);
            Assert.AreEqual(accPrev, form.DatosResponsable.AccPrev);
            Assert.AreEqual(accPrevRespId, form.DatosResponsable.AccPrevRespId);
            Assert.AreEqual(accPrevFechaFin, form.DatosResponsable.AccPrevFin);
            Assert.AreEqual(FormFg005Estado.ValidandoSgi, form.Estado);
        }

        [TestCase]
        public void UpdateValidandoSgiTest()
        {
            var causasRaices = "Causas Raices";
            string accCorr = "Accion Correctiva";
            int accCorrRespId = 89;
            DateTime accCorrFechaFin = new DateTime(2016, 02, 15);
            string accPrev = "Accion Preventiva";
            int accPrevRespId = 75;
            DateTime accPrevFechaFin = new DateTime(2016, 02, 18);

            var numero = FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo", "Accion Inmediata", 
                "Comentarios");
            FormFg005.UpdateResponsable(numero, causasRaices, accCorr, accCorrRespId, accCorrFechaFin, accPrev, 
                accPrevRespId, accPrevFechaFin);

            bool aceptar = true;
            Assert.DoesNotThrow(() => FormFg005.UpdateValidandoSgi(numero, aceptar));
            var form = FormFg005.Read(numero);
            Assert.AreEqual(FormFg005Estado.EvaluacionAcciones, form.Estado);

            aceptar = false;
            Assert.DoesNotThrow(() => FormFg005.UpdateValidandoSgi(numero, aceptar));
            form = FormFg005.Read(numero);
            Assert.AreEqual(FormFg005Estado.ProcesandoResponsable, form.Estado);
        }

        [TestCase]
        public void CloseTest()
        {
            var causasRaices = "Causas Raices";
            string accCorr = "Accion Correctiva";
            int accCorrRespId = 89;
            DateTime accCorrFechaFin = new DateTime(2016, 02, 15);
            string accPrev = "Accion Preventiva";
            int accPrevRespId = 75;
            DateTime accPrevFechaFin = new DateTime(2016, 02, 18);

            // Paso 2.
            var numero = FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo", "Accion Inmediata",
                "Comentarios");

            // Paso 3.
            FormFg005.UpdateResponsable(numero, causasRaices, accCorr, accCorrRespId, accCorrFechaFin, accPrev,
                accPrevRespId, accPrevFechaFin);

            // Paso 4.
            FormFg005.UpdateValidandoSgi(numero, true);

            // Paso 5.
            var evResultados = "Resultados.";
            List<FileAttachment> evResultadosFiles = new List<FileAttachment>();
            var evRevision = "Revision.";
            FormFg005Cierre cierre = FormFg005Cierre.Eficaz;
            int personaId = 89;

            evResultadosFiles.Add(new FileAttachment("Prueba1.txt", new byte[] {0x00}));
            evResultadosFiles.Add(new FileAttachment("Prueba2.txt", new byte[] {0x00}));
            evResultadosFiles.Add(new FileAttachment("Prueba3.txt", new byte[] {0x00}));

            Assert.DoesNotThrow(() => FormFg005.CloseUploadFiles(numero, evResultadosFiles));
            Assert.DoesNotThrow(() => FormFg005.Close(numero, evResultados, evRevision, cierre, personaId));

            var form = FormFg005.Read(numero);
            Assert.AreEqual(FormFg005Estado.Cerrada, form.Estado);
            Assert.AreEqual(evResultados, form.EvResultados);
            Assert.AreEqual(evRevision, form.EvRevision);
            Assert.AreEqual(cierre, form.Cierre);
            Assert.AreEqual(personaId, form.CierrePersonaId);
            Assert.AreNotEqual(Constants.InvalidDateTime, form.CierreFecha);
        }

        [TestCase]
        public void PagerTest()
        {
            for (int i = 1; i < 5; i++)
            {
                FormFg005.Create(89, FormFg005Origen.AuditoriaExterna, "Asunto", "Hallazgo",
                    "Accion Inmediata", "Comentarios");
            }

            var lst = FormFg005.Pager(1, new List<Filtro>());
            Assert.AreNotEqual(0, lst.Items.Count);
        }
    }
}
