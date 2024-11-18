using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class UsuarioHelperTests
    {
        [TestCase]
        public void LoginTest()
        {
            try
            {
                UsuarioHelper.Login(0);
                Assert.Fail();
            }
            catch
            {
                
            }

            try
            {
                UsuarioHelper.Login(89);
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                UsuarioHelper.Login("spam", "gador.1");
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                UsuarioHelper.Login("sarasa", "123");
                Assert.Fail();
            }
            catch
            {
                
            }
        }
    }
}
