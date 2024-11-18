using System.Web.Mvc;

namespace Servaind.Intranet.Web.Controllers
{
    public class ErrorController : BaseController
    {
        //
        // GET: /Error/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        //
        // GET: /Error/Error404
        public ActionResult Error404()
        {
            return View();
        }

        //
        // GET: /Error/Unknown
        public ActionResult Unknown()
        {
            return View();
        }
    }
}
