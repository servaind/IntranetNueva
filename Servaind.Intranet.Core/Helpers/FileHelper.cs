using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Proser.Common.Utils;

namespace Servaind.Intranet.Core.Helpers
{
    public class FileAttachment
    {
        // Propiedades.
        public string Nombre { get; private set; }
        public byte[] Contenido { get; private set; }


        public FileAttachment(string nombre, byte[] contenido)
        {
            if (String.IsNullOrWhiteSpace(nombre)) throw new Exception("El nombre no puede estar vacio.");
            if (contenido == null || contenido.Length == 0) throw new Exception("El archivo no puede estar vacio.");

            Nombre = nombre;
            Contenido = contenido;
        }
    }

    public class FileSummary
    {
        // Propiedades.
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Tipo { get; set; }
        public string Tamano { get; set; }
        public string PathHash { get; set; }
    }

    public static class FileHelper
    {
        // Constantes.
        private const string HASH_PATH = "Path";
        private const string HASH_NAME = "Name";
        private const string NO_AVAILABLE = "<no disponible>";


        public static List<FileSummary> FileInfoToFileSummary(List<FileInfo> files)
        {
            List<FileSummary> result = new List<FileSummary>();

            if (files != null) files.ForEach(f => result.Add(FileInfoToFileSummary(f)));

            return result;
        }

        public static FileSummary FileInfoToFileSummary(FileInfo file)
        {
            return new FileSummary
            {
                Nombre = file.Name,
                Extension = file.Extension.TrimStart('.'),
                Tipo = FileTypeDescription(file.Extension),
                Tamano = FileSizeToString(file.Length),
                PathHash = EncryptPath(file.FullName, file.Name)
            };
        }        

        public static string FileTypeDescription(string extension)
        {
            extension = extension.ToLower().TrimStart('.');

            if (extension.Equals("txt")) return "Archivo de texto";
            if (extension.Equals("jpg")) return "Archivo de imagen";
            if (extension.Equals("png")) return "Archivo de imagen";
            if (extension.Equals("gif")) return "Archivo de imagen";
            if (extension.Equals("zip")) return "Archivo comprimido";
            if (extension.Equals("rar")) return "Archivo comprimido";
            if (extension.Equals("iso")) return "Imagen de CD / DVD";
            if (extension.Equals("xls")) return "Documento de Microsoft Excel";
            if (extension.Equals("xlsx")) return "Documento de Microsoft Excel";
            if (extension.Equals("doc")) return "Documento de Microsoft Word";
            if (extension.Equals("docx")) return "Documento de Microsoft Word";
            if (extension.Equals("pdf")) return "Documento PDF";
            if (extension.Equals("exe")) return "Aplicacion";

            return "Desconocido";
        }

        public static string FileSizeToString(long bytes)
        {
            return bytes.ToFileSize();
        }

        public static string EncryptPath(string path, string name)
        {
            return Security.EncryptParameter(String.Format("{0}={1}&{2}={3}", HASH_PATH, path, HASH_NAME, name));
        }

        public static Tuple<string, string> DecryptPath(string hash)
        {
            var aux = Security.GetURLParameters(hash);

            return new Tuple<string, string>(aux[HASH_PATH], aux[HASH_NAME]);
        }

        public static string ReadPathName(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                return dir.Name;
            }
            catch
            {
                return NO_AVAILABLE;
            }
        }

        public static void Save(FileAttachment file, string path)
        {
            if (file == null) return;

            File.WriteAllBytes(path + file.Nombre, file.Contenido);
        }

        public static List<FileSummary> ReadPath(string path)
        {
            List<FileSummary> result = new List<FileSummary>();

            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                dir.EnumerateFiles().ToList().ForEach(f => result.Add(FileInfoToFileSummary(f)));
            }
            catch
            {
                result.Clear();
            }

            return result;
        }

        public static List<FileSummary> GetFilesFromPath(string path, Func<FileSummary, bool> filter)
        {
            var result = new List<FileSummary>();

            try
            {
                var dir = new DirectoryInfo(path);
                dir.EnumerateFiles().ToList().ForEach(f =>
                {
                    var file = FileInfoToFileSummary(f);
                    if (filter(file)) result.Add(file);
                });
            }
            catch
            {
                result.Clear();
            }

            return result;
        }

        public static byte[] Read(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch
            {
                return null;
            }
        }
    }
}
