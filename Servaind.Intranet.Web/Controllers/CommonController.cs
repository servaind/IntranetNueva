using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proser.Common.Utils;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult Image(string hash)
        {
            var fileArgs = FileHelper.DecryptPath(hash);

            byte[] file = System.IO.File.ReadAllBytes(fileArgs.Item1);

            return File(file, "image/png");
        }

        public ActionResult File(string path, string name)
        {
            byte[] file = System.IO.File.ReadAllBytes(path);

            return File(file, "application/octet-stream", name);
        }
    }
}
