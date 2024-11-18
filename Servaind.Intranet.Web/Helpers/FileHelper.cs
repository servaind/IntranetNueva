using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Proser.Common.Utils;
using Servaind.Intranet.Core;

namespace Servaind.Intranet.Web.Helpers
{
    public class FileResult
    {
        // Propiedades.
        public string name { get; set; }
        public int size { get; set; }
        public string error { get; set; }
    }

    public static class FileHelper
    {
        public static string GetFilePathHash(ElementType type, int id)
        {
            return Security.EncryptParameter(String.Format("Type={0}&Id={1}", (int)type, id));
        }

        public static byte[] GetFile(string hash)
        {
            Dictionary<string, string> parameters = Security.GetURLParameters(hash);
            if (parameters == null || !parameters.ContainsKey("Type") || !parameters.ContainsKey("Id"))
                return null;

            byte[] result = null;

            switch ((ElementType)Convert.ToInt32(parameters["Type"]))
            {
                case ElementType.ImagenInstrumento:
                    string path = parameters["Id"];
                    Image img = Image.FromFile(path);
                    result = ImageToByte(img);
                    break;
            }

            return result;
        }

        private static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static string GetFileIcon(string e)
        {
            if (e == "pdf") return "file-pdf-o";
            if (e == "txt") return "file-text-o";
            if (e == "exe") return "file-desktop";
            if (e == "iso") return "file-hdd-o";
            if (e == "xls" || e == "xlsx") return "file-excel-o";
            if (e == "doc" || e == "docx") return "file-word-o";
            if (e == "ppt" || e == "pptx" || e == "pps" || e == "ppsx") return "file-powerpoint-o";
            if (e == "zip" || e == "rar") return "file-zip-o";
            if (e == "jpg" || e == "png" || e == "gif") return "file-image-o";

            return "file";
        }
    }
}