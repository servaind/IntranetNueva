using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class VehiculoTests
    {
        [TestCase]
        public void ListTest()
        {
            var lst = Vehiculo.List();
            Assert.AreNotEqual(0, lst.Count);
        }

        [TestCase]
        public void ListVencimientosTest()
        {
            var vencimientos = Vehiculo.ListVencimientos(new DateTime(2017, 10, 01), new DateTime(2017, 10, 31));
            bool any = false;
            foreach (var t in vencimientos.Keys)
            {
                var lst = vencimientos[t];
                if (lst.Count > 0)
                {
                    any = true;
                    break;
                }
            }

            Assert.IsTrue(any);
        }

        [TestCase]
        public void SendVencimientosTest()
        {
            Assert.DoesNotThrow(Vehiculo.SendVencimientos);
        }

        [TestCase]
        public void ListVencidosTest()
        {
            var vencimientos = Vehiculo.ListVencidos();
            bool any = false;
            foreach (var t in vencimientos.Keys)
            {
                var lst = vencimientos[t];
                if (lst.Count > 0)
                {
                    any = true;
                    break;
                }
            }

            Assert.IsTrue(any);
        }
    }
}
