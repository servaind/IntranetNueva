using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Servaind.Intranet.Core;

namespace Servaind.Intranet.Services
{
    public partial class svcMain : ServiceBase
    {
        // Variables.
        private CancellationTokenSource token;


        public svcMain()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            // Connection Strings.
            DataAccess.ConnectionStringIntranet = ConfigurationManager.ConnectionStrings["Servaind.Intranet"].ConnectionString;
            DataAccess.ConnectionStringTango = ConfigurationManager.ConnectionStrings["Tango"].ConnectionString;
            DataAccess.ConnectionStringProser = ConfigurationManager.ConnectionStrings["Proser"].ConnectionString;

            // Instrumentos.
            Instrumento.PATH_IMAGENES = ConfigurationManager.AppSettings["PathInstrumentosImg"];
            Instrumento.PATH_CERTIFICADOS = ConfigurationManager.AppSettings["PathInstrumentosCertificados"];
            Instrumento.PATH_MANUALES = ConfigurationManager.AppSettings["PathInstrumentosManuales"];
            Instrumento.PATH_EAC = ConfigurationManager.AppSettings["PathInstrumentosEAC"];
            Instrumento.PATH_COMPROB_MANT = ConfigurationManager.AppSettings["PathInstrumentosComprobMant"];


            token = new CancellationTokenSource();
            Task.Factory.StartNew(DoStuff, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(CheckInstrumentosProxVencer, TaskCreationOptions.LongRunning);
            //Task.Factory.StartNew(CheckVehiculosVencimientos, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(CheckFormsFg005Pendientes, TaskCreationOptions.LongRunning);
        }

        protected override void OnStop()
        {
            token.Cancel();
        }

        private void DoStuff()
        {
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(100);
            }
        }

        private void CheckInstrumentosProxVencer()
        {
            DateTime lastCheck = DateTime.Now.AddDays(-1);

            while (!token.IsCancellationRequested)
            {
                if ((DateTime.Now - lastCheck).TotalDays >= 1)
                {
                    Instrumento.CheckProxVencer();

                    lastCheck = DateTime.Now;
                }

                Thread.Sleep(100);
            }
        }

        private void CheckVehiculosVencimientos()
        {
            while (!token.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;

                if (now.Day == 25 && now.Hour == 0 && now.Minute == 0)
                {
                    Vehiculo.SendVencimientos();
                    Thread.Sleep(2 * 60 * 1000);
                }

                Thread.Sleep(100);
            }
        }

        private void CheckFormsFg005Pendientes()
        {
            var lastCheck = DateTime.Now.AddDays(-1);

            while (!token.IsCancellationRequested)
            {
                if ((DateTime.Now - lastCheck).TotalDays >= 1)
                {
                    FormFg005.SendPendientes();

                    lastCheck = DateTime.Now;
                }

                Thread.Sleep(100);
            }
        }
    }
}
