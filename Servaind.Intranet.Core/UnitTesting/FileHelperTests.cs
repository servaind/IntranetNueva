using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class FileHelperTests
    {
        [TestCase]
        public void FileInfoToFileSummaryTest()
        {
            string file = "C:\\test.pdf";
            FileInfo fileInfo = new FileInfo(file);

            var filesSummary = FileHelper.FileInfoToFileSummary(new List<FileInfo>
            {
                fileInfo
            });
            Assert.AreNotEqual(0, filesSummary.Count);

            FileSummary f = filesSummary[0];
            Assert.AreEqual("test.pdf", f.Nombre);
            Assert.AreEqual("pdf", f.Extension);
            Assert.AreEqual("Documento PDF", f.Tipo);
            Assert.AreEqual("4.65 MB", f.Tamano);
        }

        [TestCase]
        public void ReadPathTest()
        {
            var lst = FileHelper.ReadPath(@"C:\");
            Assert.AreNotEqual(0, lst.Count);

            lst = FileHelper.ReadPath(@"C:\sarasa");
            Assert.AreEqual(0, lst.Count);
        }
    }
}
