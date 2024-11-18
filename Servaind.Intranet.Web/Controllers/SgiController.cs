using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Servaind.Intranet.Core;
using Servaind.Intranet.Core.Helpers;
using Servaind.Intranet.Web.Helpers;
using Servaind.Intranet.Web.Models;


namespace Servaind.Intranet.Web.Controllers
{
    public class SgiController : BaseController
    {
        protected override MenuItem GetSelectedMenu()
        {
            return MenuItem.Sgi;
        }

        [Logged]
        public ActionResult Index()
        {
            return View("Multisitio");
        }

        [Logged]
        public ActionResult OportMejora()
        {
            return View();
        }

        [Logged]
        [HttpPost]
        public ActionResult OportMejora(int areaId, int responsableId, string comentarios,
            int urgenciaId, IEnumerable<HttpPostedFileBase> files)
        {
            List<Helpers.FileResult> uploadedFiles = new List<Helpers.FileResult>();
            bool result = true;
            string message = String.Empty;

            FileAttachment adjunto = null;
            if (files != null)
            {
                HttpPostedFileBase file = files.ElementAt(0);

                try
                {
                    byte[] data;
                    using (var inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    uploadedFiles.Add(new Helpers.FileResult
                    {
                        name = file.FileName,
                        size = file.ContentLength
                    });

                    adjunto = new FileAttachment(file.FileName, data);
                }
                catch (Exception ex)
                {
                    uploadedFiles.Add(new Helpers.FileResult
                    {
                        name = file.FileName,
                        size = file.ContentLength,
                        error = ex.Message
                    });
                }
            }

            OportMejoraUrgencia urgencia = Enum.IsDefined(typeof (OportMejoraUrgencia), urgenciaId)
                ? (OportMejoraUrgencia) urgenciaId
                : OportMejoraUrgencia.Alta;

            try
            {
                OportunidadMejora.Enviar(areaId, responsableId, SecurityHelper.CurrentPersonaId, comentarios, urgencia, 
                    adjunto);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
                uploadedFiles.ForEach(f => f.error = ex.Message);
            }

            return adjunto == null ? BasicOpResult(result, message) : Json(new
            {
                files = uploadedFiles
            });
        }

        [Logged]
        public ActionResult FormFg005(int? id)
        {
            // id => numero.
            if (id != null)
            {
                var form = Core.FormFg005.Read((int)id);
                if (form != null) return View(form);
            }

            return View("FormFg005Create");
        }

        [Logged]
        public ActionResult FormFg005Print(int? id)
        {
            if (id != null)
            {
                var form = Core.FormFg005.Read((int)id);
                if (form != null) return View("FormFg005Print", form);

            }

            return View("FormFg005Create");
        }

        [Logged]
        [HttpPost]
        public ActionResult FormFg005Create(int origenId, string asunto, string hallazgo, string accInmediata, 
            string comentarios)
        {
            bool result;
            string message = String.Empty;

            try
            {
                int numero = Core.FormFg005.Create(SecurityHelper.CurrentPersonaId, (FormFg005Origen) origenId, asunto,
                    hallazgo, accInmediata, comentarios);
                message = Core.FormFg005.NumeroToString(numero);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        public ActionResult FormFg005ProcesandoSgi(int id, bool norIso9001, bool norIso14001, bool norOhsas18001, 
            bool norIram301, string apaIso9001, string apaIso14001, string apaOhsas18001, string apaIram301, 
            int areaResponsabilidadId, int categoriaId)
        {
            // id => numero.
            bool result;
            string message = String.Empty;

            try
            {
                Core.FormFg005.UpdateSgi(id, norIso9001, norIso14001, norOhsas18001, norIram301, apaIso9001,
                    apaIso14001, apaOhsas18001, apaIram301, areaResponsabilidadId, (FormFg005Categoria)categoriaId);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        public ActionResult FormFg005ProcesandoResponsable(int id, string causasRaices, string accCorr, int accCorrRespId,
            string accCorrFechaFin, string accPrev, int accPrevRespId, string accPrevFechaFin)
        {
            // id => numero.
            bool result;
            string message = String.Empty;

            try
            {
                Core.FormFg005.UpdateResponsable(id, causasRaices, accCorr, accCorrRespId, 
                    DateTime.ParseExact(accCorrFechaFin, "dd/MM/yyyy", CultureInfo.InstalledUICulture, DateTimeStyles.None),
                    accPrev, accPrevRespId,
                    DateTime.ParseExact(accPrevFechaFin, "dd/MM/yyyy", CultureInfo.InstalledUICulture, DateTimeStyles.None));

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        public ActionResult FormFg005ValidandoSgi(int id, bool aprobar)
        {
            // id => numero.
            bool result;
            string message = String.Empty;

            try
            {
                Core.FormFg005.UpdateValidandoSgi(id, aprobar);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        public ActionResult FormFg005CerrarFiles(int id)
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                byte[] data;
                using (Stream inputStream = hpf.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                FileAttachment f = new FileAttachment(hpf.FileName, data);
                Core.FormFg005.CloseUploadFile(id, f);

                r.Add(new ViewDataUploadFilesResult()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }

            return Json(new { files = r.ToArray() });
        }

        [Logged]
        [HttpPost]
        public ActionResult FormFg005Cerrar(int id, string evResultados, string evRevision, int cierreId)
        {
            // id => numero.
            bool result;
            string message = String.Empty;

            try
            {
                Core.FormFg005.Close(id, evResultados, evRevision, (FormFg005Cierre)cierreId, SecurityHelper.CurrentPersonaId);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        public ActionResult FormsFg005()
        {
            return View();
        }

        [Logged]
        [HttpPost]
        public ActionResult FormsFg005(int pagina, string numero, string categoria, string areaResponsabilidad,
            string asunto, string estado, string fechaDesde, string fechaHasta, string cierreDesde, string cierreHasta,
            string origen)
        {
            var filtros = new List<Filtro>();

            int aux;
            if (Int32.TryParse(numero, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Numero, aux));
            if (Int32.TryParse(categoria, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Categoria, aux));
            if (Int32.TryParse(origen, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Origen, aux));
            if (Int32.TryParse(areaResponsabilidad, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.AreaResponsabilidad, aux));
            if (!String.IsNullOrWhiteSpace(asunto)) filtros.Add(new Filtro((int)FormFg005Filtro.Asunto, asunto));
            if (Int32.TryParse(estado, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Estado, aux));
            DateTime aux2;
            if (DateTime.TryParseExact(fechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                filtros.Add(new Filtro((int)FormFg005Filtro.FechaAltaDesde, aux2.ToString("dd/MM/yyyy")));
            if (DateTime.TryParseExact(fechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                filtros.Add(new Filtro((int)FormFg005Filtro.FechaAltaHasta, aux2.ToString("dd/MM/yyyy")));
            if (DateTime.TryParseExact(cierreDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                filtros.Add(new Filtro((int)FormFg005Filtro.FechaCierreDesde, aux2.ToString("dd/MM/yyyy")));
            if (DateTime.TryParseExact(cierreHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                filtros.Add(new Filtro((int)FormFg005Filtro.FechaCierreHasta, aux2.ToString("dd/MM/yyyy")));

            var pager = Core.FormFg005.Pager(pagina, filtros);

            return Json(pager);
        }

        [Logged]
        public ActionResult FormFg005Download(int id, string nombre)
        {
            string filePath = Core.FormFg005.ReadFile(id, nombre);

            return RedirectToAction("File", "Common", new { path = filePath, name = nombre });
        }

        [Logged]
        public ActionResult FormsFg005ToCsv(string numero, string categoria, string areaResponsabilidad,
            string asunto, string estado, string fechaDesde, string fechaHasta, string cierreDesde, string cierreHasta,
            int todos, string origen)
        {
            var filtros = new List<Filtro>();

            if (todos == 0)
            {
                int aux;
                if (Int32.TryParse(numero, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Numero, aux));
                if (Int32.TryParse(categoria, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Categoria, aux));
                if (Int32.TryParse(origen, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Origen, aux));
                if (Int32.TryParse(areaResponsabilidad, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.AreaResponsabilidad, aux));
                if (!String.IsNullOrWhiteSpace(asunto)) filtros.Add(new Filtro((int)FormFg005Filtro.Asunto, asunto));
                if (Int32.TryParse(estado, out aux)) filtros.Add(new Filtro((int)FormFg005Filtro.Estado, aux));
                DateTime aux2;
                if (DateTime.TryParseExact(fechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                    filtros.Add(new Filtro((int)FormFg005Filtro.FechaAltaDesde, aux2.ToString("dd/MM/yyyy")));
                if (DateTime.TryParseExact(fechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                    filtros.Add(new Filtro((int)FormFg005Filtro.FechaAltaHasta, aux2.ToString("dd/MM/yyyy")));
                if (DateTime.TryParseExact(cierreDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                    filtros.Add(new Filtro((int)FormFg005Filtro.FechaCierreDesde, aux2.ToString("dd/MM/yyyy")));
                if (DateTime.TryParseExact(cierreHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux2))
                    filtros.Add(new Filtro((int)FormFg005Filtro.FechaCierreHasta, aux2.ToString("dd/MM/yyyy")));
            }

            var path = Core.FormFg005.ExportToCsv(filtros);

            return RedirectToAction("File", "Common", new { path, name = "Sistema de NC.csv" });
        }
    }
}
