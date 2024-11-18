using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NUnit.Framework;
using Proser.Common;
using Proser.Common.Data;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class BaseTests : TestBase
    {
        [TestCase]
        public void CreateTest()
        {
            try
            {
                Base.Create(String.Empty, Constants.InvalidInt, Constants.InvalidInt);
                Assert.Fail();
            }
            catch(Exception ex)
            {
                
            }

            try
            {
                Base.Create("Sarasa", Constants.InvalidInt, Constants.InvalidInt);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Base.Create(String.Empty, 1, Constants.InvalidInt);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Base.Create("Test", 89, 89);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestCase]
        public void ReadTest()
        {
            Assert.IsNull(Base.Read(0));

            Base b = Base.Read(1);
            Assert.IsNotNull(b);

            Assert.AreEqual("Buenos Aires", b.Nombre);
            Assert.AreEqual(104, b.ResponsableId);
            Assert.AreEqual("TAMAYO, RAMON", b.Responsable);
            Assert.AreEqual(180, b.AlternateId);
            Assert.AreEqual("NIELSEN, ADRIAN", b.Alternate);
            Assert.IsTrue(b.Activa);
        }

        [TestCase]
        public void UpdateTest()
        {
            int id = 1;

            try
            {
                Base.Update(id, String.Empty, Constants.InvalidInt, Constants.InvalidInt, false);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Base.Update(id, "Sarasa", Constants.InvalidInt, Constants.InvalidInt, false);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Base.Update(id, String.Empty, 1, Constants.InvalidInt, false);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Base.Update(id, "Test", 89, 75, false);
                Base b = Base.Read(id);
                Assert.AreEqual("Test", b.Nombre);
                Assert.AreEqual(89, b.ResponsableId);
                Assert.AreEqual(75, b.AlternateId);
                Assert.AreEqual(false, b.Activa);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestCase]
        public void DeleteTest()
        {
            Base b = Base.Read(1);
            Assert.IsNotNull(b);

            Base.Delete(1);
            Assert.IsNull(Base.Read(1));
        }

        [TestCase]
        public void ListTest()
        {
            var bases1 = Base.List();
            Assert.AreEqual(11, bases1.Count);

            var bases2 = Base.ListAll();
            Assert.AreEqual(11, bases2.Count);

            var bases3 = Base.ListActivas();
            Assert.AreEqual(8, bases3.Count);
        }
    }
}
