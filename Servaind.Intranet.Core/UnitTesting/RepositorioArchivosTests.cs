using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class RepositorioArchivosTests
    {
        [TestCase]
        public void ConstructorTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = null;
            Assert.DoesNotThrow(() => rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun));

            Assert.AreEqual("Repositorio común", rep1.Nombre);
            Assert.AreNotEqual(0, rep1.Permisos.Count);
            Assert.AreNotEqual(0, rep1.Archivos.Count);
            Assert.AreNotEqual(0, rep1.Carpetas.Count);
        }

        [TestCase]
        public void NavegarTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);

            Assert.AreEqual("Repositorio común", rep1.Nombre);
            Assert.AreNotEqual(0, rep1.Permisos.Count);
            Assert.AreNotEqual(0, rep1.Archivos.Count);
            Assert.AreNotEqual(0, rep1.Carpetas.Count);

            rep1.Navegar("Bolivia");
            Assert.AreEqual("Repositorio común", rep1.Nombre);
            Assert.AreNotEqual(0, rep1.Permisos.Count);
            Assert.AreNotEqual(0, rep1.Archivos.Count);
            Assert.AreNotEqual(0, rep1.Carpetas.Count);

            rep1.Navegar("Sarasa");
            Assert.AreEqual("Repositorio común", rep1.Nombre);
            Assert.AreEqual(0, rep1.Permisos.Count);
            Assert.AreEqual(0, rep1.Archivos.Count);
            Assert.AreEqual(0, rep1.Carpetas.Count);

            rep1.Navegar(@"Bolivia\Claudio Castro");
            Assert.AreEqual("Repositorio común", rep1.Nombre);
            Assert.AreNotEqual(0, rep1.Permisos.Count);
            Assert.AreNotEqual(0, rep1.Archivos.Count);
            Assert.AreNotEqual(0, rep1.Carpetas.Count);
        }

        [TestCase]
        public void CreateCarpetaTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);
            Assert.DoesNotThrow(() => rep1.CreateCarpeta("Prueba"));

            rep1.Navegar("Prueba");
            Assert.AreEqual("Repositorio común", rep1.Nombre);
            Assert.AreNotEqual(0, rep1.Permisos.Count);
            Assert.AreEqual(0, rep1.Archivos.Count);
            Assert.AreEqual(0, rep1.Carpetas.Count);
        }

        [TestCase]
        public void RemoveCarpetaTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);
            Assert.DoesNotThrow(() => rep1.CreateCarpeta("Prueba"));

            Thread.Sleep(500);

            Assert.DoesNotThrow(() => rep1.RemoveCarpeta("Prueba"));
        }

        [TestCase]
        public void CreateArchivoTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);
            Assert.DoesNotThrow(() => rep1.CreateCarpeta("Prueba"));

            rep1.Navegar("Prueba");

            Assert.DoesNotThrow(() => rep1.CreateArchivo("Prueba.txt", new byte[] {65}));
        }

        [TestCase]
        public void RemoveArchivoTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);
            Assert.DoesNotThrow(() => rep1.CreateCarpeta("Prueba"));

            rep1.Navegar("Prueba");

            Assert.DoesNotThrow(() => rep1.CreateArchivo("Prueba.txt", new byte[] { 65 }));

            Thread.Sleep(500);

            Assert.DoesNotThrow(() => rep1.RemoveArchivo("Prueba.txt"));
        }

        [TestCase]
        public void UpdateCarpetaNombreTest()
        {
            Persona usuario = Persona.Read(89);

            RepositorioArchivos rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);
            Assert.DoesNotThrow(() => rep1.CreateCarpeta("Prueba"));

            Thread.Sleep(500);

            rep1 = new RepositorioArchivos(usuario, RepositoriosArchivos.Comun);
            Assert.DoesNotThrow(() => rep1.UpdateCarpetaNombre("Prueba", "Prueba 2"));
        }
    }
}
