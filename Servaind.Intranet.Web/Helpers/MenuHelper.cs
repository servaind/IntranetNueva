using System.Collections.Generic;
using System.Linq;
using Servaind.Intranet.Core;

namespace Servaind.Intranet.Web.Helpers
{
    public enum MenuItem
    {
        None,
        Sistemas,
        Deposito,
        General,
        Sgi,
        Administracion,
        ControlAcceso
    }

    public static class MenuHelper
    {
        public static string IsSelected(MenuItem item, object sel)
        {
            MenuItem selected = sel == null ? MenuItem.None : (MenuItem)sel;

            return item == selected ? " start active" : "";
        }
    }
}