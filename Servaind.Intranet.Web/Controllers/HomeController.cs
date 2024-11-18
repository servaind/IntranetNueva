using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Servaind.Intranet.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Logged]
        public ActionResult Index()
        {
            return View();
        }
    }
}
