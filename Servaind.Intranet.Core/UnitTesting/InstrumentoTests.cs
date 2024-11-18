using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Proser.Common;
using Proser.Common.Data;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class InstrumentoTests : TestBase
    {
        [TestCase]
        public void CreateTest()
        {
            try
            {
                Instrumento.Create(Constants.InvalidInt, Constants.InvalidInt, "", Constants.InvalidInt,
                    Constants.InvalidInt, "", "", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {
                
            }

            try
            {
                Instrumento.Create(1, Constants.InvalidInt, "", Constants.InvalidInt,
                    Constants.InvalidInt, "", "", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "", Constants.InvalidInt,
                    Constants.InvalidInt, "", "", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", Constants.InvalidInt,
                    Constants.InvalidInt, "", "", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    Constants.InvalidInt, "", "", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            } 
            
            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, Constants.InvalidDateTime,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    Constants.InvalidInt, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, Constants.InvalidInt, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, Constants.InvalidDateTime,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    Constants.InvalidInt, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "Ubicacion", Constants.InvalidDateTime, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "Ubicacion", DateTime.Now, Constants.InvalidInt);
                Assert.Fail();
            }
            catch
            {

            }

            try
            {
                int id = Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "Ubicacion", DateTime.Now, 89);

                Assert.AreNotEqual(Constants.InvalidInt, id);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestCase]
        public void CalibracionesTest()
        {
            int id = Constants.InvalidInt;

            try
            {
                id = Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "Ubicacion", DateTime.Now, 89);
            }
            catch
            {
                Assert.Fail();
            }

            var lst1 = Instrumento.ListCalibraciones(id);
            Assert.AreEqual(1, lst1.Count);
        }

        [TestCase]
        public void MantenimientosTest()
        {
            int id = Constants.InvalidInt;

            try
            {
                id = Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "Ubicacion", DateTime.Now, 89);
            }
            catch
            {
                Assert.Fail();
            }

            var lst1 = Instrumento.ListMantenimientos(id);
            Assert.AreEqual(1, lst1.Count);
        }

        [TestCase]
        public void ComprobacionesTest()
        {
            int id = Constants.InvalidInt;

            try
            {
                id = Instrumento.Create(1, 1, "Test", 1,
                    1, "Modelo", "NumSerie", "Rango", "Resolucion", "Clase", "Incertidumbre", 1, DateTime.Now,
                    89, 1, DateTime.Now,
                    89, "Ubicacion", DateTime.Now, 89);
            }
            catch
            {
                Assert.Fail();
            }

            var lst1 = Instrumento.ListComprobaciones(id);
            Assert.AreEqual(1, lst1.Count);
        }

        [TestCase]
        public void ListTiposTest()
        {
            var lst = Instrumento.ListTipos();
            Assert.AreEqual(122, lst.Count);
        }

        [TestCase]
        public void ListMarcasTest()
        {
            var lst = Instrumento.ListMarcas();
            Assert.AreEqual(123, lst.Count);
        }

        [TestCase]
        public void ListGruposTest()
        {
            var lst = Instrumento.ListGrupos();
            Assert.AreEqual(12, lst.Count);
        }

        [TestCase]
        public void PathTest()
        {
            int numero = 2;

            Assert.AreEqual(true, Instrumento.HasCertif(numero));
            Assert.AreEqual(true, Instrumento.HasEac(numero));

            var lst1 = Instrumento.ListImagenes(numero);
            Assert.AreNotEqual(0, lst1.Count);

            var lst2 = Instrumento.ListManuales(numero);
            Assert.AreEqual(0, lst2.Count);
        }

        [TestCase]
        public void VencimientoTest()
        {
            int frec;
            DateTime ultimo;
            bool vencido;
            bool proxVencer;

            frec = 12;
            ultimo = DateTime.Now.AddMonths(-11);
            vencido = Instrumento.IsVencido(frec, ultimo);
            proxVencer = Instrumento.IsProxVencer(frec, ultimo);
            Assert.AreEqual(false, vencido);
            Assert.AreEqual(true, proxVencer);

            frec = 12;
            ultimo = DateTime.Now.AddMonths(-12);
            vencido = Instrumento.IsVencido(frec, ultimo);
            proxVencer = Instrumento.IsProxVencer(frec, ultimo);
            Assert.AreEqual(true, vencido);
            Assert.AreEqual(false, proxVencer);

            frec = 12;
            ultimo = DateTime.Now.AddMonths(-8);
            vencido = Instrumento.IsVencido(frec, ultimo);
            proxVencer = Instrumento.IsProxVencer(frec, ultimo);
            Assert.AreEqual(false, vencido);
            Assert.AreEqual(false, proxVencer);
        }

        [TestCase]
        public void ListTest()
        {
            Pager<InstrumentoSummary> pager = Instrumento.Pager(1, new List<Filtro>());
            Assert.AreNotEqual(0, pager.TotalPages);

            pager.Items.ForEach(Assert.IsNotNull);
        }

        [TestCase]
        public void ReadTest()
        {
            InstrumentoSummary ins1 = Instrumento.Read(0);
            Assert.IsNotNull(ins1);

            InstrumentoSummary ins2 = Instrumento.Read(-1);
            Assert.IsNull(ins2);
        }

        [TestCase]
        public void CheckProxVencerTest()
        {
            Assert.DoesNotThrow(Instrumento.CheckProxVencer);
        }
    }
}
