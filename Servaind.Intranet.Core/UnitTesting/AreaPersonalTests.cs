using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class AreaPersonalTests
    {
        [TestCase]
        public void ListTest()
        {
            var lst = AreaPersonal.List();
            Assert.AreNotEqual(0, lst.Count);

            lst = AreaPersonal.List(false);
            Assert.AreNotEqual(0, lst.Count);
        }

        [TestCase]
        public void ReadTest()
        {
            var area = AreaPersonal.Read(1);
            Assert.IsNotNull(area);
            Assert.AreEqual(1, area.Id);
            Assert.AreEqual("Mantenimiento: Laboratorio", area.Nombre);

            area = AreaPersonal.Read(99);
            Assert.IsNull(area);
        }

        [TestCase]
        public void LoadResponsablesTest()
        {
            var area = AreaPersonal.Read(1);
            area.LoadResponsables();

            Assert.AreNotEqual(0, area.Responsables.Count);
        }

        [TestCase]
        public void ListEmailsResponsablesTest()
        {
            var area = AreaPersonal.Read(1);
            area.LoadResponsables();

            var emails = area.ListEmailsResponsables();

            Assert.IsNotNullOrEmpty(emails);
        }
    }
}
