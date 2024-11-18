using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proser.Common;
using Proser.Common.Data;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class BasesController : BaseController
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
        public ActionResult List()
        {
            List<Base> result = Base.ListAll(false);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult Detalle(int id)
        {
            Base p = Base.Read(id, false);

            return Json(p);
        }

        [Logged]
        [HttpPost]
        public ActionResult Save(int id, string nombre, int responsableId, int alternateId, bool activa)
        {
            bool result = true;
            string message = String.Empty;

            try
            {
                if (id == Constants.InvalidInt) Base.Create(nombre, responsableId, alternateId, activa);
                else Base.Update(id, nombre, responsableId, alternateId, activa);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }
    }
}
