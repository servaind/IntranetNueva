using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using Proser.Common.Extensions;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    public class RepositorioArchivos
    {
        // Constantes.
        private const string TIPOS_ARCHIVOS = ".exe .txt .jpg .png .gif .zip .rar .iso .xls .doc .xlsx .docx .pdf .ppt .pptx .pps .ppsx";
        private const string ERR_PERMISOS = "No posee los permisos necesarios.";

        // Propiedades.
        private RepositoriosArchivos Repositorio { get; set; }
        public int Id { get { return (int)Repositorio; } }

        public string Inicio { get; set; }
        public string General { get; set; }

        private Persona Usuario { get; set; }
        public string Nombre { get; private set; }

        public List<string> Carpetas { get; private set; }
        public List<FileSummary> Archivos { get; private set; }
        internal List<PermisoRepositorio> Permisos { get; private set; }
        private string PathActual { get; set; }


        public RepositorioArchivos(Persona usuario, RepositoriosArchivos repositorio)
        {
            Repositorio = repositorio;

            var path = Repositorio.GetDescription();

            Usuario = usuario;

            Inicio = ReadInicio(repositorio);
            //General = ReadGeneral(repositorio);
            Nombre = ReadNombre(repositorio);

            Permisos = ListPermisosCarpeta(path, usuario);
            Carpetas = TienePermiso(PermisoRepositorio.Lectura) ? ListCarpetas(usuario, path) : new List<string>();
            Archivos = TienePermiso(PermisoRepositorio.Lectura) ? ListArchivos(path) : new List<FileSummary>();
        }


        /// <summary>
        /// Obtiene los repositorios de archivos disponibles.
        /// </summary>
        public static Dictionary<int, string> ListRepositorios(Persona usuario)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            if (usuario != null)
            {
                result.Add((int)RepositoriosArchivos.Comun, "Repositorio común");
                result.Add((int)RepositoriosArchivos.SGI_Multisitio, "Repositorio SGI: Multisitio");
                result.Add((int)RepositoriosArchivos.SGI_Gas, "Repositorio SGI: Gas");
                result.Add((int)RepositoriosArchivos.SGI_Liquidos, "Repositorio SGI: Líquidos");
                result.Add((int)RepositoriosArchivos.SGI_Valvulas, "Repositorio SGI: Válvulas");
                result.Add((int)RepositoriosArchivos.SGI_Bolivia, "Repositorio SGI: Bolivia");
                result.Add((int)RepositoriosArchivos.Manual_SGI, "Repositorio SGI: Manual de SGI");
                result.Add((int)RepositoriosArchivos.Politica_SGI, "Repositorio SGI: Política de SGI");
                result.Add((int)RepositoriosArchivos.Politica_Alcohol_Drogas,
                    "Repositorio SGI: Política, alcohol y drogas");
                result.Add((int)RepositoriosArchivos.Certificaciones, "Repositorio SGI: Certificaciones");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_AdminFinanz,
                    "Repositorio SGI: BsAs - Administración y finanzas");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Compras, "Repositorio SGI: BsAs - Compras");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Deposito, "Repositorio SGI: BsAs - Depósito");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Desarrollo, "Repositorio SGI: BsAs - Desarrollo");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Informatica, "Repositorio SGI: BsAs - Informática");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Ingenieria, "Repositorio SGI: BsAs - Ingeniería");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Mantenimiento, "Repositorio SGI: BsAs - Gas y Líquidos");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Obras, "Repositorio SGI: BsAs - Obras");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_RRHH, "Repositorio SGI: BsAs - Recursos Humanos");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Seguridad_Higiene,
                    "Repositorio SGI: BsAs - Seguridad e Higiene");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Ventas, "Repositorio SGI: BsAs - Ventas");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Metrologia, "Repositorio SGI: BsAs - Laboratorio");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Organigramas, "Repositorio SGI: BsAs - Organigramas");
                result.Add((int)RepositoriosArchivos.SGI_BsAs_Proyectos, "Repositorio SGI: BsAs - Proyectos");
                result.Add((int)RepositoriosArchivos.SGI_Politica_SGI, "SGI: Política SGI");
                result.Add((int)RepositoriosArchivos.DE_Lista_Doc_Ext, "Documentos externos: Lista");
                result.Add((int)RepositoriosArchivos.DE_Doc_Ext, "Documentos externos: Documentos");
                result.Add((int)RepositoriosArchivos.ITR, "Repositorio de ITR");
                result.Add((int)RepositoriosArchivos.SGI, "Repositorio de SGI");
                result.Add((int)RepositoriosArchivos.MedioAmbiente, "Repositorio de Medio Ambiente");
                result.Add((int)RepositoriosArchivos.Seguridad, "Repositorio de Seguridad e Higiene");
                result.Add((int)RepositoriosArchivos.RRHH, "Repositorio de Recursos Humanos");
                result.Add((int)RepositoriosArchivos.Mat_Leg_Int, "Matriz Legal Interna");
                result.Add((int)RepositoriosArchivos.DOC_SGI, "Documentación SGI");
                result.Add((int)RepositoriosArchivos.DOC_MedioAmbiente, "Documentación Medio Ambiente");
                result.Add((int)RepositoriosArchivos.DOC_RRHH, "Documentación Recursos Humanos");
            }
            else
            {
                /*if (Constantes.RepositoriosPublicos.ContainsKey(Constantes.Usuario.ID))
                {
                    RepositoriosArchivos[] repositorios = Constantes.RepositoriosPublicos[Constantes.Usuario.ID];
                    foreach (RepositoriosArchivos r in repositorios)
                    {
                        string nombre = "";
                        switch (r)
                        {
                            case RepositoriosArchivos.Petrobras:
                                nombre = "Repositorio Petrobras";
                                break;
                        }

                        result.Add((int) r, nombre);
                    }
                }*/
            }

            return result;
        }

        /// <summary>
        /// Obtiene los permisos de la carpeta para el usuario.
        /// </summary>
        private static List<PermisoRepositorio> ListPermisosCarpeta(string path, Persona usuario)
        {
            List<PermisoRepositorio> result = new List<PermisoRepositorio>();

            try
            {
                DirectorySecurity dirSecurity = Directory.GetAccessControl(path);
                AuthorizationRuleCollection reglas = dirSecurity.GetAccessRules(true, true,
                    typeof(System.Security.Principal.NTAccount));

                List<string> aux = new List<string>();

                foreach (FileSystemAccessRule regla in reglas)
                {
                    string identityReference = regla.IdentityReference.Value.ToLower();
                    aux.Add(identityReference);
                    if (identityReference.Equals(usuario.NombreDominio.ToLower()) || identityReference.Equals("todos"))
                    {
                        if ((regla.FileSystemRights & FileSystemRights.Read) != FileSystemRights.Read) continue;

                        if (regla.AccessControlType == AccessControlType.Allow)
                        {
                            result.Add(PermisoRepositorio.Lectura);

                            if ((regla.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write
                                && regla.AccessControlType == AccessControlType.Allow)
                            {
                                result.Add(PermisoRepositorio.Escritura);
                            }
                        }

                        break;
                    }
                }

                string x = "stop";
            }
            catch(Exception e)
            {
                result.Clear();
            }

            return result;
        }

        /// <summary>
        /// Obtiene los archivos de la carpeta.
        /// </summary>
        private static List<FileSummary> ListArchivos(string path)
        {
            List<FileInfo> result = new List<FileInfo>();
            string[] extensiones = TIPOS_ARCHIVOS.Split(' ');

            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                var busqueda = dir.GetFiles("*.*").Where(s => extensiones.Contains(s.Extension.ToLower()));

                List<FileInfo> archivos = new List<FileInfo>();
                foreach (FileInfo a in busqueda) result.Add(a);
            }
            catch
            {
                result.Clear();
            }

            return FileHelper.FileInfoToFileSummary(result);
        }

        /// <summary>
        /// Verifica si el usuario tiene permisos.
        /// </summary>
        public bool TienePermiso(PermisoRepositorio permiso)
        {
            return Permisos != null && Permisos.Contains(permiso);
        }

        /// <summary>
        /// Obtiene las subcarpetas de la carpeta.
        /// </summary>
        internal static List<string> ListCarpetas(Persona usuario, string path)
        {
            List<string> result = new List<string>();

            try
            {
                DirectoryInfo carpeta = new DirectoryInfo(path);
                foreach (DirectoryInfo subcarpeta in carpeta.GetDirectories())
                {
                    var permisos = ListPermisosCarpeta(subcarpeta.FullName, usuario);
                    if (permisos.Contains(PermisoRepositorio.Lectura)) result.Add(subcarpeta.Name);
                }
            }
            catch
            {
                result.Clear();
            }

            return result;
        }

        /// <summary>
        /// Navega dentro del repositorio de archivos.
        /// </summary>
        public void Navegar(string path)
        {
            PathActual = path;
            path = String.Format("{0}\\{1}", Repositorio.GetDescription(), path);

            Permisos = ListPermisosCarpeta(path, Usuario);
            Carpetas = TienePermiso(PermisoRepositorio.Lectura) ? ListCarpetas(Usuario, path) : new List<string>();
            Archivos = TienePermiso(PermisoRepositorio.Lectura) ? ListArchivos(path) : new List<FileSummary>();
        }

        /// <summary>
        /// Obtiene el path actual.
        /// </summary>
        private string GetPathActual()
        {
            string result = Repositorio.GetDescription();

            if (!String.IsNullOrWhiteSpace(PathActual)) result += "\\" + PathActual;

            return result;
        }


        private string ReadInicio(RepositoriosArchivos repositorio)
        { 

            string inicio = "#";

            switch (repositorio)
            {
                case RepositoriosArchivos.Comun:
                    inicio = "/Repositorio/View/0";
                    break;
                case RepositoriosArchivos.SGI_Multisitio:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BuenosAires:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Gas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Liquidos:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Valvulas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Bolivia:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.Petrobras:
            
                    break;
                case RepositoriosArchivos.Manual_SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.Politica_SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.Politica_Alcohol_Drogas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.Certificaciones:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Compras:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Desarrollo:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Informatica:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Ingenieria:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Mantenimiento:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Obras:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_RRHH:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Seguridad_Higiene:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Ventas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Metrologia:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Organigramas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Proyectos:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_AdminFinanz:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_BsAs_Deposito:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Politica_SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Manual_SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Politica_Alcohol_Drogas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Certificaciones:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Normas:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.SGI_Procedimientos_SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.MA_ControlResiduos:
                    break;
                case RepositoriosArchivos.MA_Emergencias_Ambientales:
                    break;
                case RepositoriosArchivos.MA_Actuacion_Derrames:
                    break;
                case RepositoriosArchivos.SEG_Matriz:
                    break;
                case RepositoriosArchivos.SEG_Investigacion_Incidentes:
                    break;
                case RepositoriosArchivos.SEG_EPP:
                    break;
                case RepositoriosArchivos.SEG_Seguridad_Salud_Operaciones:
                    break;
                case RepositoriosArchivos.SEG_Plan_Emergencias:
                    break;
                case RepositoriosArchivos.RRHH_Manual_Empleado:
                    break;
                case RepositoriosArchivos.RRHH_Organigrama:
                    break;
                case RepositoriosArchivos.RRHH_Registro_Capacitacion:
                    break;
                case RepositoriosArchivos.DE_Lista_Doc_Ext:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.DE_Doc_Ext:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.ITR:
                    break;
                case RepositoriosArchivos.SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.MedioAmbiente:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.Seguridad:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.RRHH:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.Mat_Leg_Int:
                    break;
                case RepositoriosArchivos.DOC_SGI:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.DOC_MedioAmbiente:
                    inicio = "/Sgi";
                    break;
                case RepositoriosArchivos.DOC_RRHH:
                    inicio = "/Sgi";
                    break;
                default:
                    break;
            }


            return inicio;
        }

       
        /// <summary>
        /// Obtiene el nombre para el Repositorio de archivos.
        /// </summary>
        public static string ReadNombre(RepositoriosArchivos repositorio)
        {
            if (repositorio == RepositoriosArchivos.Comun) return "Común";
            if (repositorio == RepositoriosArchivos.SGI_Multisitio) return "SGI: Multisitio";
            if (repositorio == RepositoriosArchivos.SGI_Gas) return "SGI: Gas";
            if (repositorio == RepositoriosArchivos.SGI_Liquidos) return "SGI: Líquidos";
            if (repositorio == RepositoriosArchivos.SGI_Valvulas) return "SGI: Válvulas";
            if (repositorio == RepositoriosArchivos.SGI_Bolivia) return "SGI: Bolivia";
            if (repositorio == RepositoriosArchivos.Manual_SGI) return "SGI: Manual de SGI";
            if (repositorio == RepositoriosArchivos.Politica_SGI) return "SGI: Política de SGI";
            if (repositorio == RepositoriosArchivos.Politica_Alcohol_Drogas) return "SGI: Política, alcohol y drogas";
            if (repositorio == RepositoriosArchivos.Certificaciones) return "SGI: Certificaciones";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_AdminFinanz) return "SGI: BsAs - Administración y finanzas";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Compras) return "SGI: BsAs - Compras";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Deposito) return "SGI: BsAs - Depósito";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Desarrollo) return "SGI: BsAs - Desarrollo";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Informatica) return "SGI: BsAs - Informática";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Ingenieria) return "SGI: BsAs - Ingeniería";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Mantenimiento) return "SGI: BsAs - Gas y Líquidos";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Obras) return "SGI: BsAs - Obras";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_RRHH) return "SGI: BsAs - Recursos Humanos";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Seguridad_Higiene) return "SGI: BsAs - Seguridad e Higiene";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Ventas) return "SGI: BsAs - Ventas";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Metrologia) return "SGI: BsAs - Laboratorio";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Organigramas) return "SGI: BsAs - Organigramas";
            if (repositorio == RepositoriosArchivos.SGI_BsAs_Proyectos) return "SGI: BsAs - Proyectos";
            if (repositorio == RepositoriosArchivos.SGI_Politica_SGI) return "SGI: Política SGI";
            if (repositorio == RepositoriosArchivos.DE_Lista_Doc_Ext) return "Documentos externos: Lista";
            if (repositorio == RepositoriosArchivos.DE_Doc_Ext) return "Documentos externos: Documentos";
            if (repositorio == RepositoriosArchivos.ITR) return "ITR";
            if (repositorio == RepositoriosArchivos.SGI) return "Documentación SGI";
            if (repositorio == RepositoriosArchivos.MedioAmbiente) return "Documentación Medio Ambiente";
            if (repositorio == RepositoriosArchivos.Seguridad) return "SGI: BsAs - Seguridad e Higiene";
            if (repositorio == RepositoriosArchivos.RRHH) return "Documentación Recursos Humanos";
            if (repositorio == RepositoriosArchivos.Mat_Leg_Int) return "Matriz Legal Interna";
            if (repositorio == RepositoriosArchivos.DOC_SGI) return "Documentación SGI";
            if (repositorio == RepositoriosArchivos.DOC_MedioAmbiente) return "Documentación Medio Ambiente";
            if (repositorio == RepositoriosArchivos.DOC_RRHH) return "Documentación Recursos Humanos";

            return "<no disponible>";
        }

        /// <summary>
        /// Crea una carpeta en el repositorio actual.
        /// </summary>
        public void CreateCarpeta(string nombre)
        {
            if (!TienePermiso(PermisoRepositorio.Escritura))
            {
                throw new Exception(ERR_PERMISOS);
            }

            var path = String.Format("{0}\\{1}", GetPathActual(), nombre);

            try
            {
                Directory.CreateDirectory(path);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Borra una carpeta en el repositorio actual.
        /// </summary>
        public void RemoveCarpeta(string nombre)
        {
            if (!TienePermiso(PermisoRepositorio.Escritura))
            {
                throw new Exception(ERR_PERMISOS);
            }

            var path = String.Format("{0}\\{1}", GetPathActual(), nombre);

            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Crea un archivo en el repositorio actual.
        /// </summary>
        public void CreateArchivo(string nombre, byte[] contenido)
        {
            if (!TienePermiso(PermisoRepositorio.Escritura))
            {
                throw new Exception(ERR_PERMISOS);
            }

            var path = String.Format("{0}\\{1}", GetPathActual(), nombre);

            try
            {
                File.WriteAllBytes(path, contenido);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Borra un archivo en el repositorio actual.
        /// </summary>
        public void RemoveArchivo(string nombre)
        {
            if (!TienePermiso(PermisoRepositorio.Escritura))
            {
                throw new Exception(ERR_PERMISOS);
            }

            var path = String.Format("{0}\\{1}", GetPathActual(), nombre);

            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza el nombre de una carpeta.
        /// </summary>
        public void UpdateCarpetaNombre(string vieja, string nuevo)
        {
            if (!TienePermiso(PermisoRepositorio.Escritura))
            {
                throw new Exception(ERR_PERMISOS);
            }

            if (!Carpetas.Contains(vieja)) throw new Exception("La carpeta no existe.");

            var pathViejo = String.Format("{0}\\{1}", GetPathActual(), vieja);
            var pathNuevo = String.Format("{0}\\{1}", GetPathActual(), nuevo);

            try
            {
                Directory.Move(pathViejo, pathNuevo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el contenido de un archivo en la ubicación actual.
        /// </summary>
        public string ReadArchivo(string nombre)
        {
            return String.Format("{0}\\{1}", GetPathActual(), nombre);
        }
    }
}