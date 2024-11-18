using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class VehiculosController : BaseController
    {
        protected override MenuItem GetSelectedMenu()
        {
            return MenuItem.Administracion;
        }

        [Logged]
        [Restricted(SeccionPagina.Administracion_Vehiculos)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Logged]
        [Restricted(SeccionPagina.Administracion_Vehiculos)]
        public ActionResult Info(int id)
        {
            var lst = Vehiculo.List();
            var result = lst.FirstOrDefault(v => v.Id == id);

            return result != null ? View(result) : null;
        }

        [HttpPost]
        [Logged]
        [Restricted(SeccionPagina.Administracion_Vehiculos)]
        public ActionResult Index(int id)
        {
            var lst = Vehiculo.List();
            var result = lst.FirstOrDefault(v => v.Id == id);

            return Json(result);
        }

        [HttpPost]
        [Logged]
        [Restricted(SeccionPagina.Administracion_Vehiculos)]
        public ActionResult Update(int id, string ubicacion, int responsableId, string vtoCertIzaje, string vtoCedulaVerde,
            string nroRuta, string vtoRuta, string vtoVtv, string vtoStaCruz, string ciaSeguro, int polizaSeguro,
            string vtoSeguro, string vtoPatente)
        {
            bool result;
            string message = String.Empty;

            var lst = Vehiculo.List();
            var vehiculo = lst.FirstOrDefault(v => v.Id == id);

            if (vehiculo == null)
            {
                result = false;
                message = "El vehiculo no existe.";
            }
            else
            {
                try
                {
                    vehiculo.Ubicacion = ubicacion;
                    vehiculo.Responsable = responsableId.ToString();
                    vehiculo.VtoCertIzaje = vtoCertIzaje;
                    vehiculo.VtoCedulaVerde = vtoCedulaVerde;
                    vehiculo.NroRuta = nroRuta;
                    vehiculo.VtoRuta = vtoRuta;
                    vehiculo.VtoVtv = vtoVtv;
                    vehiculo.VtoStaCruz = vtoStaCruz;
                    vehiculo.CiaSeguro = ciaSeguro;
                    vehiculo.PolizaSeguro = polizaSeguro;
                    vehiculo.VtoSeguro = vtoSeguro;
                    vehiculo.VtoPatente = vtoPatente;

                    Vehiculo.Update(new List<VehiculoSummary>
                    {
                        vehiculo
                    });

                    result = true;
                }
                catch(Exception ex)
                {
                    result = false;
                    message = ex.Message;
                }
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [Restricted(SeccionPagina.Administracion_Vehiculos)]        
        public ActionResult Vencimientos(string mes, string vencidos)
        {
            if (String.IsNullOrWhiteSpace(mes))
            {
                mes = DateTime.Now.ToString("MM/yyyy");
            }
            ViewBag.Mes = mes;

            Dictionary<string, List<VencimientoVehiculo>> result = !String.IsNullOrWhiteSpace(vencidos) ? Vehiculo.ListVencidos() : Vehiculo.ListVencimientos(mes);
            ViewBag.Vencidos = !String.IsNullOrWhiteSpace(vencidos);

            return View(result);
        }

        [Logged]
        public ActionResult Exportar()
        {
            string path = Vehiculo.ExportarCsv();

            return RedirectToAction("File", "Common", new { path, name = "Listado de vehiculos.csv" });
        }
    }
}
