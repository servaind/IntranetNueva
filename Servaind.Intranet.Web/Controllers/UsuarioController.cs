using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Servaind.Intranet.Core.Helpers;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class UsuarioController : BaseController
    {
        //
        // GET: /Usuario/
        [NotLogged]
        public ActionResult Login()
        {
            return View();
        }

        [NotLogged]
        [HttpPost]
        public ActionResult Login(string username, string password, bool remember)
        {
            bool result = false;
            string message = String.Empty;

            try
            {
                SecurityHelper.Login(username, password, remember);
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }
        
        public ActionResult Logout()
        {
            SecurityHelper.Logout();

            return Redirect("/");
        }

        public ActionResult LoginDirect(int id)
        {
            try
            {
                SecurityHelper.Login(id);
                return Redirect("/");
            }
            catch (Exception ex)
            {
                return Redirect("/Error/AccessDenied");
            }
        }
    }
}
