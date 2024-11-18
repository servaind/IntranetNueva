using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Proser.Common;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class ImputacionTests : TestBase
    {
        [TestCase]
        public void ReadTest()
        {
            Assert.IsNull(Imputacion.Read(0));

            Imputacion o = Imputacion.Read(1);
            Assert.IsNotNull(o);

            Assert.AreEqual(1, o.Id);
            Assert.AreEqual(1, o.Numero);
            Assert.AreEqual("Administracion", o.Descripcion);
            Assert.AreEqual("1 - Administracion", o.DescripcionFull);
            Assert.AreEqual(true, o.Activa);

            o = Imputacion.ReadFromNumero(1);
            Assert.IsNotNull(o);

            Assert.AreEqual(1, o.Id);
            Assert.AreEqual(1, o.Numero);
            Assert.AreEqual("Administracion", o.Descripcion);
            Assert.AreEqual("1 - Administracion", o.DescripcionFull);
            Assert.AreEqual(true, o.Activa);
        }

        [TestCase]
        public void ListTest()
        {
            var lst1 = Imputacion.ListActivas();
            Assert.AreEqual(651, lst1.Count);

            var lst2 = Imputacion.List();
            Assert.AreEqual(896, lst2.Count);
        }

        [TestCase]
        public void CreateTest()
        {
            try
            {
                Imputacion.Create(Constants.InvalidInt, String.Empty, true);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Imputacion.Create(1, String.Empty, true);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            int numero = 10000;
            string descripcion = "Testing";
            bool activa = false;

            try
            {
                Imputacion.Create(numero, descripcion, activa);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Imputacion o = Imputacion.ReadFromNumero(numero);
            Assert.IsNotNull(o);
            Assert.AreEqual(numero, o.Numero);
            Assert.AreEqual(descripcion, o.Descripcion);
            Assert.AreEqual(activa, o.Activa);
        }

        [TestCase]
        public void UpdateTest()
        {
            int id = 1;
            int numero = 10000;
            string descripcion = "Testing";
            bool activa = false;

            try
            {
                Imputacion.Update(id, numero, descripcion, activa);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Imputacion o = Imputacion.Read(id);
            Assert.IsNotNull(o);

            Assert.AreEqual(numero, o.Numero);
            Assert.AreEqual(descripcion, o.Descripcion);
            Assert.AreEqual(activa, o.Activa);
        }
    }
}
