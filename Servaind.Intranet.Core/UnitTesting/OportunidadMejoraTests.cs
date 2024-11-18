using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class OportunidadMejoraTests
    {
        [TestCase]
        public void EnviarTest()
        {
            Assert.Throws<Exception>(() => OportunidadMejora.Enviar(99, 85, 79, "Prueba", OportMejoraUrgencia.Alta));
            Assert.Throws<Exception>(() => OportunidadMejora.Enviar(1, -1, 79, "Prueba", OportMejoraUrgencia.Alta));
            Assert.Throws<Exception>(() => OportunidadMejora.Enviar(1, 85, -1, "Prueba", OportMejoraUrgencia.Alta));
            Assert.Throws<Exception>(() => OportunidadMejora.Enviar(1, 85, 79, "", OportMejoraUrgencia.Alta));

            Assert.DoesNotThrow(() => OportunidadMejora.Enviar(1, 85, 79, "Prueba", OportMejoraUrgencia.Alta));
            Assert.DoesNotThrow(() => OportunidadMejora.Enviar(1, 85, 79, "Prueba", OportMejoraUrgencia.Alta,
                new FileAttachment("Prueba.txt", new byte[] {0x65})));
        }
    }
}
