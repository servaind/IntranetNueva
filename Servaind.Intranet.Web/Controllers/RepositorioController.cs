using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class RepositorioController : BaseController
    {
        protected override MenuItem GetSelectedMenu()
        {
            return MenuItem.General;
        }

        [Logged]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult Index()
        {
            return RedirectToAction("View", new { id = (int)RepositoriosArchivos.Comun });
        }

        [Logged]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult View(int? id)
        {
            RepositoriosArchivos r = RepositoriosArchivos.Comun;
            if (id != null && Enum.IsDefined(typeof(RepositoriosArchivos), (int)id))
            {
                r = (RepositoriosArchivos)id;
            }

            RepositorioArchivos rep = new RepositorioArchivos(SecurityHelper.CurrentPersona, r);

            return View("Index", rep);
        }

        [Logged]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult List(int id, string path)
        {
            if (!Enum.IsDefined(typeof (RepositoriosArchivos), id)) return BasicOpResult(false, "Repositorio invalido.");

            RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
            repo.Navegar(path);

            object result = new
            {
                repo.Carpetas,
                repo.Archivos,
                CanEditar = repo.TienePermiso(PermisoRepositorio.Escritura)
            };

            return OpResultWithItems(true, String.Empty, result);
        }


        public ActionResult Download(int id, string path, string nombre)
        {
            if (!Enum.IsDefined(typeof(RepositoriosArchivos), id)) throw new Exception("Repositorio invalido.");

            RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
            repo.Navegar(path);

            string filePath = repo.ReadArchivo(nombre);

            return RedirectToAction("File", "Common", new { path = filePath, name = nombre });
        }


        [Logged]
        [HttpPost]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult CreateCarpeta(int id, string path, string nombre)
        {
            if (!Enum.IsDefined(typeof(RepositoriosArchivos), id)) return BasicOpResult(false, "Repositorio invalido.");

            RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
            repo.Navegar(path);

            bool result = false;
            string message = String.Empty;
            try
            {
                repo.CreateCarpeta(nombre);
                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult DeleteCarpeta(int id, string path, string nombre)
        {
            if (!Enum.IsDefined(typeof(RepositoriosArchivos), id)) return BasicOpResult(false, "Repositorio invalido.");

            RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
            repo.Navegar(path);

            bool result = false;
            string message = String.Empty;
            try
            {
                repo.RemoveCarpeta(nombre);
                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult DeleteArchivo(int id, string path, string nombre)
        {
            if (!Enum.IsDefined(typeof(RepositoriosArchivos), id)) return BasicOpResult(false, "Repositorio invalido.");

            RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
            repo.Navegar(path);

            bool result = false;
            string message = String.Empty;
            try
            {
                repo.RemoveArchivo(nombre);
                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult UpdateCarpetaNombre(int id, string path, string viejo, string nuevo)
        {
            if (!Enum.IsDefined(typeof(RepositoriosArchivos), id)) return BasicOpResult(false, "Repositorio invalido.");

            RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
            repo.Navegar(path);

            bool result = false;
            string message = String.Empty;
            try
            {
                repo.UpdateCarpetaNombre(viejo, nuevo);
                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }

        [Logged]
        [HttpPost]
        [Restricted(SeccionPagina.General_Repositorio)]
        public ActionResult UploadFiles(int id, string path, IEnumerable<HttpPostedFileBase> files)
        {
            List<object> result = new List<object>();

            if (files != null)
            {
                if (!Enum.IsDefined(typeof(RepositoriosArchivos), id)) return BasicOpResult(false, "Repositorio invalido.");

                RepositorioArchivos repo = new RepositorioArchivos(SecurityHelper.CurrentPersona, (RepositoriosArchivos)id);
                repo.Navegar(path);

                HttpPostedFileBase file = files.ElementAt(0);

                try
                {
                    byte[] data;
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    repo.CreateArchivo(file.FileName, data);

                    result.Add(new
                    {
                        name = file.FileName,
                        size = file.ContentLength
                    });
                }
                catch (Exception ex)
                {
                    result.Add(new
                    {
                        name = file.FileName,
                        size = file.ContentLength,
                        error = ex.Message
                    });                    
                }
            }

            return Json(new 
            {
                files = result
            });
        }
    }
}
