using System;
using System.Web.Mvc;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class BaseController : Controller
    {
        // Propiedades.
        public Persona Usuario
        {
            get { return ViewBag.Usuario; }
            set { ViewBag.Usuario = value; }
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Usuario = SecurityHelper.CurrentPersona;
            ViewBag.SelectedMenu = GetSelectedMenu();

            ViewBag.FileVersion = $"?v={DateTime.Now.Ticks}";
        }

        protected virtual MenuItem GetSelectedMenu()
        {
            return MenuItem.None;
        }

        protected ActionResult BasicOpResult(bool result, string msg)
        {
            return Json(new { Success = result, Message = msg });
        }

        protected ActionResult OpResultWithId(bool result, string msg, object id)
        {
            return Json(new { Success = result, Message = msg, Id = id });
        }

        protected ActionResult OpResultWithItems(bool result, string msg, object items)
        {
            return Json(new { Success = result, Message = msg, Items = items });
        }

        [Logged]
        public ActionResult DownloadFile(string hash)
        {
            var file = Core.Helpers.FileHelper.DecryptPath(hash);

            return RedirectToAction("File", "Common", new { path = file.Item1, name = file.Item2 });
        }

        public ActionResult DownloadBase(byte[] content, string name)
        {
            return File(content, "application/octet-stream", name);
        }
    }
}