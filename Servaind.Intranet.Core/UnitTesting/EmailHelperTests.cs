using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Proser.Communications.Network.Mailing;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class EmailHelperTests
    {
        [TestCase]
        public void SendTest()
        {
            string from;
            string to;
            string cc;
            string subject;
            string body;
            List<Attachment> attachments;

            // Send default.
            to = "martin.duran@servaind.com";
            cc = "";
            subject = "Test default";
            body = "Test message";
            attachments = null;
            Assert.DoesNotThrow(() => EmailHelper.SendFromIntranet(to, cc, subject, body, attachments));

            // Send from.
            from = "test@servaind.com";
            to = "martin.duran@servaind.com";
            cc = "";
            subject = "Test from";
            body = "Test message";
            attachments = null;
            Assert.DoesNotThrow(() => EmailHelper.Send(from, to, cc, subject, body, attachments));

            // Send with attachments.
            to = "martin.duran@servaind.com";
            cc = "";
            subject = "Test with attachment";
            body = "Test message";
            attachments = new List<Attachment>
            {
                Attachment.GetAttachment(@"C:\config.psr", "adjunto1.jpg")
            };
            Assert.DoesNotThrow(() => EmailHelper.SendFromIntranet(to, cc, subject, body, attachments));
        }
    }
}
