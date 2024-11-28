using Servaind.Intranet.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Servaind.Intranet.Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //DateTime lastCheck = DateTime.Now.AddDays(-1);

            //if ((DateTime.Now - lastCheck).TotalDays >= 1)
            //{
            //    //Instrumento.CheckProxVencer();
            //    var DEFAULT_SENDER = ConfigurationManager.AppSettings["DEFAULT_SENDER"];
            //    EmailHelper.Send(DEFAULT_SENDER, "mogel10@gmail.com", "mogel10@gmail.com", "Asunto", "Contenido");

            //    lastCheck = DateTime.Now;
            //}


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new svcMain()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
