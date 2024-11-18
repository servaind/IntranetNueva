using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Servaind.Intranet.Services
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        //18/11/2024-AGM: Agregado para poder cambiar nombre default del servicio al instalarlo
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            // Leer el nombre del servicio desde los parámetros de instalación
            if (Context.Parameters.ContainsKey("ServiceName"))
            {
                var serviceInstaller = (ServiceInstaller)Installers[1];
                serviceInstaller.ServiceName = Context.Parameters["ServiceName"];
                serviceInstaller.DisplayName = Context.Parameters["ServiceName"];
            }
            base.Install(stateSaver);
        }
    }
}
