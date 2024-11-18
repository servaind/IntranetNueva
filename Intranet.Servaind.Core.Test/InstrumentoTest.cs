using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Servaind.Intranet.Core;

namespace Intranet.Servaind.Core.Test
{
    [TestClass]
    public class InstrumentoTest
    {
        [TestMethod]
        public void CheckProxVencerTest()
        {
            Instrumento.PATH_CERTIFICADOS = "";
            Instrumento.PATH_MANUALES = "";
            Instrumento.PATH_EAC = "";
            Instrumento.PATH_IMAGENES = "";
            Instrumento.PATH_COMPROB_MANT = "";

            Instrumento.CheckProxVencer();
        }
    }
}
