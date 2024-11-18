using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NUnit.Framework;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class TestBase
    {
        protected TransactionScope transScope;

        [TestFixtureSetUp]
        public void Setup()
        {
            transScope = new TransactionScope();
        }

        [TestFixtureTearDown]
        public void End()
        {
            transScope.Dispose();
        }
    }
}
