using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Proser.Common;
using Proser.Common.Data;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class ImputacionesController : BaseController
    {
        protected override MenuItem GetSelectedMenu()
        {
            return MenuItem.Sistemas;
        }

        [Logged]
        [Restricted(SeccionPagina.Sistemas)]
        public ActionResult Index()
        {
            return View();
        }

        [Logged]
        [HttpPost]
        public ActionResult List(int pagina, int numero, string descripcion)
        {
            List<Filtro> filtros = new List<Filtro>();
            if (numero != Constants.InvalidInt) filtros.Add(new Filtro((int) FiltrosImputacion.Numero, numero));
            if (!String.IsNullOrWhiteSpace(descripcion)) filtros.Add(new Filtro((int)FiltrosImputacion.Descripcion, descripcion));

            Pager<Imputacion> result = Imputacion.Pager(pagina, filtros);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult Detalle(int id)
        {
            Imputacion o = Imputacion.Read(id);

            return Json(o);
        }

        [Logged]
        [HttpPost]
        public ActionResult Save(int id, int numero, string descripcion, bool activa)
        {
            bool result = true;
            string message = String.Empty;
            object info = null;

            try
            {
                if (id == Constants.InvalidInt) Imputacion.Create(numero, descripcion, activa);
                else Imputacion.Update(id, numero, descripcion, activa);
            }
            catch(Exception ex)
            {
                result = false;
                message = ex.Message;
                info = null;
            }

            return OpResultWithItems(result, message, info);
        }
    }
}
