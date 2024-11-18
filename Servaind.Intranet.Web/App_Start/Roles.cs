using System;
using System.Web;
using System.Web.Mvc;
using Proser.Common;
using Servaind.Intranet.Core.Helpers;
using Servaind.Intranet.Web.Helpers;

namespace Servaind.Intranet.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LoggedAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult(SecurityHelper.LOGIN_URL + "?url=" + filterContext.HttpContext.Request.Url);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result;

            result = SecurityHelper.CurrentPersonaId != Constants.InvalidInt;

            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NotLoggedAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("/");
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result;

            result = SecurityHelper.CurrentPersonaId == Constants.InvalidInt;

            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RestrictedAttribute : AuthorizeAttribute
    {
        // Properties.
        public SeccionPagina Seccion { get; private set; }


        public RestrictedAttribute(SeccionPagina s)
        {
            Seccion = s;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("/Error/AccessDenied");
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = SecurityHelper.TieneAcceso(Seccion);

            return result;
        }
    }
}