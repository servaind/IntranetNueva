using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Proser.Communications.Network.Mailing;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class EmailTemplateTests
    {
        class TestTemplate : EmailTemplate
        {
            public TestTemplate(EmailTemplateTipo type)
            {
                SetEncabezado("Este es el encabezado");
                SetTipo(type);

                for(int i = 1; i <= 5; i++) AddItem("Item " + i, "Valor " + i);
                AddItem("Item adasdas", "Valor dasdasdasdasdasd");

                SetLinkAcceso("http://www.google.com.ar", "acceder a Google");
            }
        }

        [TestCase]
        public void SendTest()
        {
            string from = "test@servaind.com";
            string to = "martin.duran@servaind.com";
            string cc = "";
            TestTemplate template;

            // Ok.
            template = new TestTemplate(EmailTemplateTipo.Ok);
            template.Send(from, to, cc, "Test OK");

            // Error.
            //template = new TestTemplate(EmailTemplateTipo.Error);
            //template.Send(from, to, cc, "Test Error");

            // Info.
            //template = new TestTemplate(EmailTemplateTipo.Info);
            //template.Send(from, to, cc, "Test Info");
        }
    }
}
