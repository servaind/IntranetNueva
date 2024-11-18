using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Proser.Common;
using Proser.Common.Data;
using Servaind.Intranet.Core;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web.Controllers
{
    public class PersonasController : BaseController
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
        [Restricted(SeccionPagina.Sistemas)]
        public ActionResult Permisos()
        {
            return View();
        }

        [Logged]
        [Restricted(SeccionPagina.Control_Acceso)]
        public ActionResult Asistencias()
        {
            return Redirect("http://intra.servaind.com:8085/ac/");
        }

        [Logged]
        [HttpPost]
        public ActionResult List(int pagina, int id, string nombre, string usuario, int responsableId)
        {
            List<Filtro> filtros = new List<Filtro>();
            if (id != Constants.InvalidInt) filtros.Add(new Filtro((int) FiltrosPersona.Id, id));
            if (!String.IsNullOrWhiteSpace(nombre)) filtros.Add(new Filtro((int)FiltrosPersona.Nombre, nombre));
            if (!String.IsNullOrWhiteSpace(usuario)) filtros.Add(new Filtro((int)FiltrosPersona.Usuario, usuario));
            if (responsableId != Constants.InvalidInt) filtros.Add(new Filtro((int)FiltrosPersona.Responsable, responsableId));

            Pager<PersonaSummary> result = Persona.Pager(pagina, filtros);

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult Detalle(int id)
        {
            Persona p = Persona.Read(id);

            return Json(p != null
                ? new
                {
                    Nombre = p.ToString(),
                    p.Usuario,
                    p.Email,
                    p.Legajo,
                    p.Cuil,
                    p.ResponsableId,
                    p.BaseId,
                    HoraEntrada = p.HoraEntrada.ToString("HH:mm"),
                    HoraSalida = p.HoraSalida.ToString("HH:mm"),
                    p.EnPanelControl,
                    p.Activo
                }
                : null);
        }

        [Logged]
        [HttpPost]
        public ActionResult Save(int id, string nombre, string usuario, string email, string legajo, string cuil, 
            int responsableId, int baseId, string horaEntrada, string horaSalida, bool enPc, bool activo)
        {
            bool result = true;
            string message = String.Empty;
            object info = null;

            try
            {
                if (id == Constants.InvalidInt)
                {
                    id = Persona.Create(nombre, email, usuario, responsableId, enPc, activo, legajo, cuil, horaEntrada,
                        horaSalida, baseId);
                }
                else Persona.Update(id, nombre, email, usuario, responsableId, enPc, activo, legajo, cuil, horaEntrada,
                        horaSalida, baseId);
            }
            catch(Exception ex)
            {
                result = false;
                message = ex.Message;
                info = null;
            }

            if (result)
            {
                info = Persona.NormalizarNombre(nombre);
            }

            return OpResultWithItems(result, message, info);
        }

        [Logged]
        [HttpPost]
        public ActionResult Permisos(int id)
        {
            List<int> result = new List<int>();

            List<PermisosPersona> permisos = PermisoPersona.ListPersona(id);
            permisos.ForEach(p => result.Add((int)p));

            return Json(result);
        }

        [Logged]
        [HttpPost]
        public ActionResult SavePermisos(int id, int[] permisos)
        {
            bool result = true;
            string message = String.Empty;

            try
            {
                List<PermisosPersona> lst = new List<PermisosPersona>();
                if (permisos != null)
                {
                    permisos.ToList().ForEach(p =>
                    {
                        if (Enum.IsDefined(typeof (PermisosPersona), p)) lst.Add((PermisosPersona) p);
                    });
                }

                PermisoPersona.Update(id, lst);
            }
            catch(Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return BasicOpResult(result, message);
        }
    }
}
