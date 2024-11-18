using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Proser.Common;
using Proser.Common.Data;
using Servaind.Intranet.Core;
using Servaind.Intranet.Core.Helpers;
using Servaind.Intranet.Web.Helpers;
using FileHelper = Servaind.Intranet.Core.Helpers.FileHelper;

namespace Servaind.Intranet.Web.Controllers
{
    public class InstrumentosController : BaseController
    {
        protected override MenuItem GetSelectedMenu()
        {
            return MenuItem.Deposito;
        }

        [Logged]
        [Restricted(SeccionPagina.Deposito_Instrumentos)]
        public ActionResult Index()
        {
            return View();
        }

        [Logged]
        [HttpPost]
        public ActionResult List(int pagina, string descripcion, int mantStatus, int calibStatus, bool busquedaExtendida)
        {
            var filtros = new List<Filtro>();

            if (busquedaExtendida)
            {
                var filtroActivos = true;

                filtros.Add(new Filtro((int)FiltroInstrumento.BusquedaExtendida, null));

                if (!string.IsNullOrWhiteSpace(descripcion))
                {
                    filtroActivos = false;
                    filtros.Add(new Filtro((int)FiltroInstrumento.Todo, descripcion));
                }

                if (Enum.IsDefined(typeof(EstadoRegInstrumento), mantStatus))
                {
                    var e = (EstadoRegInstrumento)mantStatus;
                    FiltroInstrumento f;
                    if (e == EstadoRegInstrumento.Activo) f = FiltroInstrumento.Mant;
                    else if (e == EstadoRegInstrumento.ProxVencer) f = FiltroInstrumento.MantProx;
                    else f = FiltroInstrumento.MantVencido;

                    filtros.Add(new Filtro((int)f, null));
                }

                if (Enum.IsDefined(typeof(EstadoRegInstrumento), calibStatus))
                {
                    var e = (EstadoRegInstrumento)calibStatus;
                    FiltroInstrumento f;
                    if (e == EstadoRegInstrumento.Activo) f = FiltroInstrumento.Calib;
                    else if (e == EstadoRegInstrumento.ProxVencer) f = FiltroInstrumento.CalibProx;
                    else f = FiltroInstrumento.CalibVencido;

                    filtros.Add(new Filtro((int)f, null));
                }

                if (filtroActivos) filtros.Add(new Filtro((int)FiltroInstrumento.Activo, null));
            }
            else
            {
                filtros.Add(new Filtro((int)FiltroInstrumento.Todo, descripcion));
            }
            
            var result = Instrumento.Pager(pagina, filtros);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult ListComprob(int id)
        {
            List<InstrumentoComprobacion> result = Instrumento.ListComprobaciones(id);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult CreateComprob(int id, string fecha, int personaId, int grupoId, string ubicacion, 
            string descripcion)
        {
            bool result = true;
            string message = String.Empty;
            InstrumentoSummary item = null;

            try
            {
                DateTime f;
                if (!DateTime.TryParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out f))
                {
                    throw new Exception("La fecha no es valida.");
                }

                Instrumento.CreateComprobacion(id, f, descripcion, personaId, grupoId, ubicacion);

                item = Instrumento.Read(id);
            }
            catch(Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return OpResultWithItems(result, message, item);
        }

        [Logged]
        [HttpPost]
        public ActionResult ListMant(int id)
        {
            List<InstrumentoRegistro> result = Instrumento.ListMantenimientos(id);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult CreateMant(int id, string fecha, int personaId, string descripcion)
        {
            bool result = true;
            string message = String.Empty;
            InstrumentoSummary item = null;

            try
            {
                DateTime f;
                if (!DateTime.TryParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out f))
                {
                    throw new Exception("La fecha no es valida.");
                }

                Instrumento.CreateMantenimiento(id, f, descripcion, personaId);

                item = Instrumento.Read(id);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return OpResultWithItems(result, message, item);
        }

        [Logged]
        [HttpPost]
        public ActionResult ListCalib(int id)
        {
            List<InstrumentoRegistro> result = Instrumento.ListCalibraciones(id);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult CreateCalib(int id, string fecha, int personaId, string descripcion)
        {
            bool result = true;
            string message = String.Empty;
            InstrumentoSummary item = null;

            try
            {
                DateTime f;
                if (!DateTime.TryParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out f))
                {
                    throw new Exception("La fecha no es valida.");
                }

                Instrumento.CreateCalibracion(id, f, descripcion, personaId);

                item = Instrumento.Read(id);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return OpResultWithItems(result, message, item);
        }

        [Logged]
        [Restricted(SeccionPagina.Deposito_Instrumentos)]
        public ActionResult Create()
        {
            return View();
        }

        [Logged]
        [HttpPost]
        public ActionResult Create(int numero, int tipoId, string descripcion, int grupoId, int marcaId,
            string modelo, string numSerie, string rango, string resolucion, string clase, string incertidumbre,
            int frecCalib, string ultCalib, int ultCalibPersonaId,
            int frecMant, string ultMant, int ultMantPersonaId,
            string ubicacion, string ultComprob, int ultComprobPersonaId)
        {
            bool result = true;
            string message = String.Empty;

            try
            {
                DateTime fUltCalib;
                if (frecCalib > 0)
                {
                    if (
                        !DateTime.TryParseExact(ultCalib, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out fUltCalib))
                    {
                        throw new Exception("La fecha de Calibracion no es valida.");
                    }
                }
                else fUltCalib = Constants.InvalidDateTime;

                DateTime fUltMant;
                if (!DateTime.TryParseExact(ultMant, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out fUltMant))
                {
                    throw new Exception("La fecha de Mantenimiento no es valida.");
                }

                DateTime fUltComprob;
                if (!DateTime.TryParseExact(ultComprob, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out fUltComprob))
                {
                    throw new Exception("La fecha de Comprobacion no es valida.");
                }

                Instrumento.Create(numero, tipoId, descripcion, grupoId, marcaId, modelo, numSerie, rango,
                    resolucion, clase, incertidumbre, frecCalib, fUltCalib, ultCalibPersonaId, frecMant, fUltMant,
                    ultMantPersonaId, ubicacion, fUltComprob, ultComprobPersonaId);
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
        public ActionResult Delete(int id, string descripcion)
        {
            bool result = true;
            string message = String.Empty;
            InstrumentoSummary item = null;

            try
            {
                Instrumento.Delete(id, Usuario.Id, descripcion);

                item = Instrumento.Read(id);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return OpResultWithItems(result, message, item);
        }

        [Logged]
        [HttpPost]
        public ActionResult Activate(int id, string descripcion)
        {
            bool result = true;
            string message = String.Empty;
            InstrumentoSummary item = null;

            try
            {
                Instrumento.Activate(id, Usuario.Id, descripcion);

                item = Instrumento.Read(id);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return OpResultWithItems(result, message, item);
        }

        [Logged]
        public ActionResult CertCalib(int id)
        {
            string path = Instrumento.PathCertif(id);

            return RedirectToAction("File", "Common", new { path, name = String.Format("{0} - Certificado de calibracion.pdf", id) });
        }

        [Logged]
        public ActionResult Eac(int id)
        {
            string path = Instrumento.PathEac(id);

            return RedirectToAction("File", "Common", new { path, name = String.Format("{0} - EAC.pdf", id) });
        }

        [Logged]
        [HttpPost]
        public ActionResult Manuales(int id)
        {
            List<FileSummary> result = Instrumento.ListManuales(id);

            return Json(result);
        }

        [Logged]
        public ActionResult Manual(string hash)
        {
            var file = FileHelper.DecryptPath(hash);

            return RedirectToAction("File", "Common", new { path = file.Item1, name = file.Item2 });
        }

        [Logged]
        [HttpPost]
        public ActionResult Imagenes(int id)
        {
            List<FileSummary> result = Instrumento.ListImagenes(id);

            return Json(result);
        }

        [Logged]
        public ActionResult ComprobMantFiles(int id)
        {
            var result = Instrumento.GetComprobMantFiles(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Logged]
        public ActionResult Csv(string descripcion, int mantStatus, int calibStatus, bool busquedaExtendida)
        {
            var filtros = new List<Filtro>();

            if (busquedaExtendida)
            {
                var filtroActivos = true;

                filtros.Add(new Filtro((int)FiltroInstrumento.BusquedaExtendida, null));

                if (!string.IsNullOrWhiteSpace(descripcion))
                {
                    filtroActivos = false;
                    filtros.Add(new Filtro((int)FiltroInstrumento.Todo, descripcion));
                }

                if (Enum.IsDefined(typeof(EstadoRegInstrumento), mantStatus))
                {
                    var e = (EstadoRegInstrumento)mantStatus;
                    FiltroInstrumento f;
                    if (e == EstadoRegInstrumento.Activo) f = FiltroInstrumento.Mant;
                    else if (e == EstadoRegInstrumento.ProxVencer) f = FiltroInstrumento.MantProx;
                    else f = FiltroInstrumento.MantVencido;

                    filtros.Add(new Filtro((int)f, null));
                }

                if (Enum.IsDefined(typeof(EstadoRegInstrumento), calibStatus))
                {
                    var e = (EstadoRegInstrumento)calibStatus;
                    FiltroInstrumento f;
                    if (e == EstadoRegInstrumento.Activo) f = FiltroInstrumento.Calib;
                    else if (e == EstadoRegInstrumento.ProxVencer) f = FiltroInstrumento.CalibProx;
                    else f = FiltroInstrumento.CalibVencido;

                    filtros.Add(new Filtro((int)f, null));
                }

                if (filtroActivos) filtros.Add(new Filtro((int)FiltroInstrumento.Activo, null));
            }
            else
            {
                filtros.Add(new Filtro((int)FiltroInstrumento.Todo, descripcion));
            }

            var result = Instrumento.ExportToCsv(filtros);

            return DownloadBase(Encoding.Unicode.GetBytes(result), "resultado.csv");
        }
    }
}
