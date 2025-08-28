using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Proser.Common;
using Proser.Common.Data;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    public class InstrumentoRegistro
    {
        // Propiedades.
        public string Fecha { get; private set; }
        public string Descripcion { get; private set; }
        public int PersonaId { get; private set; }
        public string Persona { get; private set; }


        internal InstrumentoRegistro(DateTime fecha, string descripcion, int personaId, string persona)
        {
            Fecha = fecha.ToString("dd/MM/yyyy");
            Descripcion = descripcion;
            PersonaId = personaId;
            Persona = persona;
        }
    }

    public class InstrumentoComprobacion : InstrumentoRegistro
    {
        // Propiedades.
        public string Grupo { get; private set; }
        public string Ubicacion { get; private set; }


        internal InstrumentoComprobacion(DateTime fecha, string descripcion, int personaId, string persona,
            string grupo, string ubicacion)
            : base(fecha, descripcion, personaId, persona)
        {
            Grupo = grupo;
            Ubicacion = ubicacion;
        }
    }

    public class InstrumentoSummary
    {
        // Propiedades.
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string Resolucion { get; set; }
        public string Rango { get; set; }
        public string Clase { get; set; }
        public string Incertidumbre { get; set; }

        public bool Calib { get; set; }
        public int CalibFrec { get; set; }
        public string CalibFecha { get; set; }
        public bool CalibVencida { get; set; }
        public bool CalibProxVencer { get; set; }

        public int MantFrec { get; set; }
        public string MantFecha { get; set; }
        public bool MantVencido { get; set; }
        public bool MantProxVencer { get; set; }

        public string ComprobFecha { get; set; }
        public string ComprobGrupo { get; set; }
        public string ComprobUbicacion { get; set; }
        public string ComprobPersona { get; set; }
        public string ComprobPersonaEmail { get; set; }

        public bool HasCertCalib { get; set; }
        public bool HasManuales { get; set; }
        public bool HasEac { get; set; }
        public bool HasComprobMant { get; set; }

        public string ImagePathHash { get; set; }

        public bool Activo { get; set; }
    }

    public class Instrumento
    {
        class CambioEstadoEmail : EmailTemplate
        {
            public CambioEstadoEmail(InstrumentoSummary instrumento, Persona persona, string descripcion)
            {
                SetTipo(EmailTemplateTipo.Info);

                SetEncabezado(String.Format("{0} ha cambiado el estado del instrumento a {1}:", persona.Nombre, instrumento.Activo ? "Activo" : "Inactivo"));

                // Items.
                AddItem("Tipo", instrumento.Tipo);
                AddItem("Marca", instrumento.Marca);
                AddItem("Modelo", instrumento.Modelo);
                AddItem("Nº de serie", instrumento.NumeroSerie);
                AddItem("Número", instrumento.Numero.ToString());
                AddItem("Motivo", descripcion);
            }
        }

        class ProxVencerEmail : EmailTemplate
        {
            public ProxVencerEmail(InstrumentoSummary instrumento)
            {
                SetTipo(EmailTemplateTipo.Info);

                SetEncabezado(String.Format("El instrumento Nº{0} se encuentra próximo a vencer.", instrumento.Numero));

                // Items.
                AddItem("Tipo", instrumento.Tipo);
                AddItem("Marca", instrumento.Marca);
                AddItem("Modelo", instrumento.Modelo);
                AddItem("Nº de serie", instrumento.NumeroSerie);
                AddItem("Número", instrumento.Numero.ToString());
                AddItem("Última calib.", instrumento.CalibFecha);
            }
        }

        // Constantes.
        private const int MAX_REGS_PAGINA = 10;
        public static string PATH_IMAGENES { get; set; }
        public static string PATH_CERTIFICADOS { get; set; }
        public static string PATH_MANUALES { get; set; }
        public static string PATH_EAC { get; set; }
        public static string PATH_COMPROB_MANT { get; set; }
        private const int DIAS_AVISO_VTO = 60;


        private static void Validar(int numero, int tipoId, string descripcion, int grupoId, int marcaId,
            string modelo, string numSerie, string rango, string resolucion, string clase, string incertidumbre,
            int frecCalib, DateTime ultCalib, int ultCalibPersonaId, 
            int frecMant, DateTime ultMant, int ultMantPersonaId,
            string ubicacion, DateTime ultComprob, int ultComprobPersonaId)
        {
            if (numero <= 0) throw new Exception("El Numero debe ser mayor a 0.");
            if (tipoId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Tipo.");
            if (String.IsNullOrWhiteSpace(descripcion)) throw new Exception("No se ha ingresado una Descripcion.");
            if (grupoId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Grupo.");
            if (marcaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado una Marca.");
            if (String.IsNullOrWhiteSpace(modelo)) throw new Exception("No se ha ingresado un Modelo.");
            if (String.IsNullOrWhiteSpace(numSerie)) throw new Exception("No se ha ingresado un Nº de serie.");
            if (String.IsNullOrWhiteSpace(rango)) throw new Exception("No se ha ingresado un Rango.");
            if (String.IsNullOrWhiteSpace(clase)) throw new Exception("No se ha ingresado una Clase.");
            if (String.IsNullOrWhiteSpace(incertidumbre)) throw new Exception("No se ha ingresado una Incertidumbre.");
            
            if (frecCalib < 0) throw new Exception("La Frecuencia de calibracion debe ser mayor a 0.");
            if (frecCalib > 0)
            {
                if (ultCalib == Constants.InvalidDateTime) throw new Exception("La Fecha de ultima calibracion no es valida.");
                if (ultCalibPersonaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Responsable para la ultima calibracion.");
            }

            if (frecMant <= 0) throw new Exception("La Frecuencia de mantenimiento debe ser mayor a 0.");
            if (ultMant == Constants.InvalidDateTime) throw new Exception("La Fecha de ultima mantenimiento no es valida.");
            if (ultMantPersonaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Responsable para el ultimo mantenimiento.");

            if (String.IsNullOrWhiteSpace(ubicacion)) throw new Exception("No se ha ingresado una Ubicacion.");
            if (ultComprob == Constants.InvalidDateTime) throw new Exception("La Fecha de ultima comprobacion no es valida.");
            if (ultComprobPersonaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Responsable para la ultima comprobacion.");
        }

        public static int Create(int numero, int tipoId, string descripcion, int grupoId, int marcaId,
            string modelo, string numSerie, string rango, string resolucion, string clase, string incertidumbre,
            int frecCalib, DateTime ultCalib, int ultCalibPersonaId,
            int frecMant, DateTime ultMant, int ultMantPersonaId,
            string ubicacion, DateTime ultComprob, int ultComprobPersonaId)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;
            int result;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                // Inserto el instrumento.
                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "INSERT INTO Instrumentos (Numero, TipoId, Descripcion, MarcaId, Modelo,  ";
                cmd.CommandText += "NumSerie, Rango, Resolucion, Clase, Incertidumbre, FrecCalib, FrecMant) VALUES ";
                cmd.CommandText += "(@Numero, @TipoId, @Descripcion, @MarcaId, @Modelo, @NumSerie, @Rango, ";
                cmd.CommandText += "@Resolucion, @Clase, @Incertidumbre, @FrecCalib, @FrecMant); ";
                cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM Instrumentos;";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoId", tipoId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@MarcaId", marcaId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Modelo", modelo));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@NumSerie", numSerie));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Rango", rango));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Resolucion", resolucion));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Clase", clase));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Incertidumbre", incertidumbre));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@FrecCalib", frecCalib));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@FrecMant", frecMant));
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                // Agrego el registro de la última calibración.
                if(frecCalib > 0) CreateCalibracion(id, ultCalib, "Alta de instrumento", ultCalibPersonaId, trans);

                // Agrego el registro de la última comprobación.
                CreateComprobacion(id, ultComprob, "Alta de instrumento", ultComprobPersonaId, grupoId, ubicacion, trans);

                // Agrego el registro del último mantenimiento.
                CreateMantenimiento(id, ultMant, "Alta de instrumento", ultMantPersonaId, trans);

                trans.Commit();

                result = id;
            }
            catch
            {
                if (trans != null) trans.Rollback();
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return result;
        }

        private static void UpdateStatus(int id, bool activo, int personaId, string description)
        {
          
            IDbConnection conn = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "UPDATE Instrumentos SET Activo = @Activo WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", activo));
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null) conn.Close();
            }

      
            try
            {
                // Send the notification email.
                InstrumentoSummary instrumento = Read(id);
                Persona persona = Persona.Read(personaId);
                CambioEstadoEmail email = new CambioEstadoEmail(instrumento, persona, description);

                email.SendFromIntranet(PermisoPersona.ListEmails(PermisosPersona.InstrumentoNotifCambio), "",
                    string.Format("Instrumento Nº{0} - Cambio de estado", instrumento.Numero));
            }
            catch
            {
                throw new EmailException();
            }
        }

        public static void Delete(int id, int personaId, string description)
        {
            UpdateStatus(id, false, personaId, description);
        }

        public static void Activate(int id, int personaId, string description)
        {
            UpdateStatus(id, true, personaId, description);
        }

        public static List<DataSourceItem> ListTipos()
        {
            List<DataSourceItem> result = new List<DataSourceItem>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT * FROM InstrumentosTipos ORDER BY Descripcion";
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new DataSourceItem(Convert.ToInt32(dr["Id"]), dr["Descripcion"].ToString()));
                }

                dr.Close();
            }
            catch
            {
                
                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static List<DataSourceItem> ListMarcas()
        {
            List<DataSourceItem> result = new List<DataSourceItem>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT * FROM InstrumentosMarcas ORDER BY Descripcion";
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new DataSourceItem(Convert.ToInt32(dr["Id"]), dr["Descripcion"].ToString()));
                }

                dr.Close();
            }
            catch
            {

                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static List<DataSourceItem> ListGrupos()
        {
            List<DataSourceItem> result = new List<DataSourceItem>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT * FROM InstrumentosGrupos ORDER BY Nombre";
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new DataSourceItem(Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString()));
                }

                dr.Close();
            }
            catch
            {

                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        internal static bool IsVencido(int frec, DateTime ultimo)
        {
            DateTime prox = ultimo.AddMonths(frec);

            return (DateTime.Now - prox).TotalDays >= 0;
        }

        internal static bool IsProxVencer(int frec, DateTime ultimo)
        {
            DateTime prox = ultimo.AddMonths(frec);
            int dias = (int)(DateTime.Now - prox).TotalDays;

            return dias < 0 && dias > -DIAS_AVISO_VTO;
        }

        private static InstrumentoSummary ReadSummary(IDataReader dr)
        {
            var result = new InstrumentoSummary
            {
                Id = Convert.ToInt32(dr["Id"]),
                Numero = Convert.ToInt32(dr["Numero"]),
                Tipo = dr["Tipo"] as string,
                Descripcion = dr["Descripcion"] as string,
                Marca = dr["Marca"] as string,
                Modelo = dr["Modelo"] as string,
                NumeroSerie = dr["NumSerie"] as string,
                Resolucion = dr["Resolucion"] as string,
                Rango = dr["Rango"] as string,
                Clase = dr["Clase"] as string,
                Incertidumbre = dr["Incertidumbre"] as string,

                CalibFrec = Convert.ToInt32(dr["FrecCalib"]),
                MantFrec = Convert.ToInt32(dr["FrecMant"]),

                CalibFecha = dr["CalibFecha"] == DBNull.Value ? Constants.InvalidDate : Convert.ToDateTime(dr["CalibFecha"]).ToString("dd/MM/yyyy"),
                MantFecha = dr["MantFecha"] == DBNull.Value ? Constants.InvalidDate : Convert.ToDateTime(dr["MantFecha"]).ToString("dd/MM/yyyy"),
                ComprobFecha = dr["ComprobFecha"] == DBNull.Value ? Constants.InvalidDate : Convert.ToDateTime(dr["ComprobFecha"]).ToString("dd/MM/yyyy"),

                ComprobGrupo = dr["ComprobGrupo"] == DBNull.Value ? "N/D" : dr["ComprobGrupo"].ToString(),
                ComprobUbicacion = dr["ComprobUbicacion"] == DBNull.Value ? "N/D" : dr["ComprobUbicacion"].ToString(),
                ComprobPersona = dr["ComprobPersona"] == DBNull.Value ? "N/D" : dr["ComprobPersona"].ToString(),
                ComprobPersonaEmail = dr["ComprobPersonaEmail"] == DBNull.Value ? "" : dr["ComprobPersonaEmail"].ToString(),

                Activo = Convert.ToBoolean(dr["Activo"])
            };

            result.Calib = result.CalibFrec > 0;
            result.CalibVencida = IsVencido(result.CalibFrec, Convert.ToDateTime(result.CalibFecha));
            result.CalibProxVencer = IsProxVencer(result.CalibFrec, Convert.ToDateTime(result.CalibFecha));

            result.MantVencido = IsVencido(result.MantFrec, Convert.ToDateTime(result.MantFecha));
            result.MantProxVencer = IsProxVencer(result.MantFrec, Convert.ToDateTime(result.MantFecha));

            result.HasCertCalib = HasCertif(result.Numero);
            result.HasManuales = HasManuales(result.Numero);
            result.HasEac = HasEac(result.Numero);
            result.HasComprobMant = HasComprobMantFiles(result.Numero);

            result.ImagePathHash = FileHelper.EncryptPath(PathImagen(result.Numero) + "0.jpg", "inst.jpg");

            return result;
        }

        public static InstrumentoSummary Read(int id)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            InstrumentoSummary result;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT i.Id, i.Numero, i.Descripcion, i.NumSerie, m.Descripcion AS Marca, ";
                cmd.CommandText += "t.Descripcion AS Tipo, i.Modelo, i.Rango, i.Resolucion, i.Clase, i.Incertidumbre, ";
                cmd.CommandText += "i.Activo, i.FrecCalib, c.Fecha AS CalibFecha, i.FrecMant, ma.Fecha AS MantFecha, ";
                cmd.CommandText +=  "co.Fecha AS ComprobFecha, co.Ubicacion AS ComprobUbicacion, g.Nombre AS ComprobGrupo, ";
                cmd.CommandText += "cop.Nombre AS ComprobPersona, cop.Email AS ComprobPersonaEmail ";
                cmd.CommandText += "FROM Instrumentos i ";
                cmd.CommandText += "LEFT JOIN InstrumentosTipos t ON i.TipoId = t.Id ";
                cmd.CommandText += "LEFT JOIN InstrumentosMarcas m ON i.MarcaId = m.Id ";
                cmd.CommandText += "LEFT JOIN InstrumentosCalibraciones c ON i.UltCalibId = c.Id ";
                cmd.CommandText += "LEFT JOIN InstrumentosMantenimientos ma ON i.UltMantId = ma.Id ";
                cmd.CommandText += "LEFT JOIN InstrumentosComprobaciones co ON i.UltComprobId = co.Id ";
                cmd.CommandText += "LEFT JOIN Personas cop ON cop.Id = co.PersonaId ";
                cmd.CommandText += "LEFT JOIN InstrumentosGrupos g ON co.GrupoId = g.Id ";
                cmd.CommandText += "WHERE i.Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                dr = cmd.ExecuteReader();

                if (!dr.Read()) throw new ElementoInexistenteException();

                result = ReadSummary(dr);

                dr.Close();
            }
            catch
            {
                result = null;
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static int ReadProxNumero()
        {
            IDbConnection conn = null;
            int result;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT TOP 1 Numero FROM Instrumentos ORDER BY Numero DESC";
                object aux = cmd.ExecuteScalar();

                result = aux == null ? 1 : (Convert.ToInt32(aux) + 1);
            }
            catch
            {
                result = 0;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static void CheckProxVencer()
        {
            List<Filtro> filtros = new List<Filtro>();

            filtros.Add(new Filtro((int)FiltroInstrumento.CalibProx, null));
            filtros.Add(new Filtro((int)FiltroInstrumento.BusquedaExtendida, null));

            string query = ListQuery(filtros, false);

            IDbConnection conn = null;
            IDataReader dr = null;
            List<InstrumentoSummary> result = new List<InstrumentoSummary>();

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = query;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var i = ReadSummary(dr);
                    var fecha = DateTime.ParseExact(i.CalibFecha, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None);
                    var dias = 30 * i.CalibFrec - (int)(DateTime.Now - fecha).TotalDays;

                    if (dias == 60 || dias == 30 || dias == 15) result.Add(i);
                }

                dr.Close();
            }
            catch(Exception e)
            {
                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            // Envío las alertas.
            foreach (InstrumentoSummary item in result)
            {
                Thread th = new Thread(new ParameterizedThreadStart(SendEmailProxVencerTH));
                th.Start(item);
                th.Join();

                //Task.Factory.StartNew(() => SendEmailProxVencer(item));
            }
        }

        private static void SendEmailProxVencerTH(object obj)
        {
            InstrumentoSummary i = (InstrumentoSummary)obj;

            SendEmailProxVencer(i);
            
        }

        private static void SendEmailProxVencer(InstrumentoSummary i)
        {
            var email = new ProxVencerEmail(i);

            string para = PermisoPersona.ListEmails(PermisosPersona.InstrumentoSeguimiento);

            email.SendFromIntranet(para,
                                  string.IsNullOrEmpty(i.ComprobPersonaEmail) ? "" : i.ComprobPersonaEmail,
                                  $"Instrumento Nº{i.Numero} - Próximo a vencer");
        }

        public static List<InstrumentoSummary> List(List<Filtro> filtros)
        {
            var result = new List<InstrumentoSummary>();

            try
            {
                using (var conn = DataAccess.GetConnection(BDConexiones.Intranet))
                {
                    var cmd = DataAccess.GetCommand(conn);
                    cmd.CommandText = ListQuery(filtros, false);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            try
                            {
                                var item = ReadSummary(dr);
                                result.Add(item);
                            }
                            catch (Exception e)
                            {
                                
                            }
                           
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }

            return result;
        }

        public static string ExportToCsv(List<Filtro> filtros)
        {
            const string SEP = ";";
            var result = new StringBuilder();

            // Encabezado.
            result.Append($"Numero{SEP}Tipo{SEP}Marca{SEP}Modelo{SEP}Num Serie{SEP}");
            result.Append($"Ubicacion{SEP}Grupo{SEP}Responsable{SEP}");
            //result.AppendLine($"Comprobacion{SEP}Mantenimiento{SEP}Calibracion{SEP}");			ANTES de 28/08/25, cambio para incluir Frecuencias de calibracion y mantenimiento en exportacion CSV
            result.AppendLine($"Comprobacion{SEP}Mantenimiento{SEP}FrecMant{SEP}Calibracion{SEP}FrecCalib{SEP}");

            // Resultados.
            var items = List(filtros);
            items.ForEach(i =>
            {
                result.Append($"{i.Numero}{SEP}{i.Tipo}{SEP}{i.Marca}{SEP}{i.Modelo}{SEP}{i.NumeroSerie}{SEP}");
                result.Append($"{i.ComprobUbicacion}{SEP}{i.ComprobGrupo}{SEP}{i.ComprobPersona}{SEP}");
                //result.AppendLine($"{i.ComprobFecha}{SEP}{i.MantFecha}{SEP}{i.CalibFecha}{SEP}");		ANTES de 28/08/25, cambio para incluir Frecuencias de calibracion y mantenimiento en exportacion CSV
                result.AppendLine($"{i.ComprobFecha}{SEP}{i.MantFecha}{SEP}{i.MantFrec}{SEP}{i.CalibFecha}{SEP}{i.CalibFrec}{SEP}");

            });

            return result.ToString();
        }

        #region Calibraciones

        private static void ValidarCalibracion(DateTime fecha, string descripcion, int personaId)
        {
            if (fecha == Constants.InvalidDateTime) throw new Exception("La Fecha no es valida.");
            if (String.IsNullOrWhiteSpace(descripcion)) throw new Exception("No se ha ingresado una Descripcion.");
            if (personaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado una Persona.");
        }

        public static void CreateCalibracion(int instrumentoId, DateTime fecha, string descripcion, int personaId)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            ValidarCalibracion(fecha, descripcion, personaId);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                CreateCalibracion(instrumentoId, fecha, descripcion, personaId, trans);

                trans.Commit();
            }
            catch
            {
                if (trans != null) trans.Rollback();
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private static void CreateCalibracion(int instrumentoId, DateTime fecha, string descripcion, int personaId, 
            IDbTransaction trans)
        {
            // Inserto la calibración.
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO InstrumentosCalibraciones (InstrumentoId, Fecha, Descripcion, PersonaId) ";
            cmd.CommandText += "VALUES (@InstrumentoId, @Fecha, @Descripcion, @PersonaId); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM InstrumentosCalibraciones;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@InstrumentoId", instrumentoId));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", personaId));
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            // Actualizo el instrumento.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE Instrumentos SET UltCalibId = @UltCalibId WHERE Id = @Id";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@UltCalibId", id));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", instrumentoId));
            cmd.ExecuteNonQuery();
        }
        
        public static List<InstrumentoRegistro> ListCalibraciones(int id)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            List<InstrumentoRegistro> result = new List<InstrumentoRegistro>();

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT c.Fecha, c.Descripcion, c.PersonaId, p.Nombre ";
                cmd.CommandText += "FROM InstrumentosCalibraciones c ";
                cmd.CommandText += "INNER JOIN Personas p ON c.PersonaId = p.Id ";
                cmd.CommandText += "WHERE c.InstrumentoId = @InstrumentoId ORDER BY c.Fecha DESC";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@InstrumentoId", id));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new InstrumentoRegistro(Convert.ToDateTime(dr["Fecha"]), dr["Descripcion"].ToString(),
                        Convert.ToInt32(dr["PersonaId"]), Persona.NormalizarNombre(dr["Nombre"].ToString())));
                }

                dr.Close();
            }
            catch
            {
                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }
        
        #endregion

        #region Mantenimientos

        private static void ValidarMantenimiento(DateTime fecha, string descripcion, int personaId)
        {
            if (fecha == Constants.InvalidDateTime) throw new Exception("La Fecha no es valida.");
            if (String.IsNullOrWhiteSpace(descripcion)) throw new Exception("No se ha ingresado una Descripcion.");
            if (personaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado una Persona.");
        }

        public static void CreateMantenimiento(int instrumentoId, DateTime fecha, string descripcion, int personaId)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            ValidarMantenimiento(fecha, descripcion, personaId);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                CreateMantenimiento(instrumentoId, fecha, descripcion, personaId, trans);

                trans.Commit();
            }
            catch
            {
                if (trans != null) trans.Rollback();
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private static void CreateMantenimiento(int instrumentoId, DateTime fecha, string descripcion, int personaId,
            IDbTransaction trans)
        {
            // Inserto el mantenimiento.
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO InstrumentosMantenimientos (InstrumentoId, Fecha, Descripcion, PersonaId) ";
            cmd.CommandText += "VALUES (@InstrumentoId, @Fecha, @Descripcion, @PersonaId); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM InstrumentosMantenimientos;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@InstrumentoId", instrumentoId));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", personaId));
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            // Actualizo el instrumento.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE Instrumentos SET UltMantId = @UltMantId WHERE Id = @Id";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@UltMantId", id));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", instrumentoId));
            cmd.ExecuteNonQuery();
        }

        public static List<InstrumentoRegistro> ListMantenimientos(int id)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            List<InstrumentoRegistro> result = new List<InstrumentoRegistro>();

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT c.Fecha, c.Descripcion, c.PersonaId, p.Nombre ";
                cmd.CommandText += "FROM InstrumentosMantenimientos c ";
                cmd.CommandText += "INNER JOIN Personas p ON c.PersonaId = p.Id ";
                cmd.CommandText += "WHERE c.InstrumentoId = @InstrumentoId ORDER BY c.Fecha DESC";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@InstrumentoId", id));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new InstrumentoRegistro(Convert.ToDateTime(dr["Fecha"]), dr["Descripcion"].ToString(),
                        Convert.ToInt32(dr["PersonaId"]), Persona.NormalizarNombre(dr["Nombre"].ToString())));
                }

                dr.Close();
            }
            catch
            {
                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        #endregion

        #region Comprobaciones

        private static void ValidarComprobacion(DateTime fecha, string descripcion, int personaId, int grupoId, 
            string ubicacion)
        {
            if (fecha == Constants.InvalidDateTime) throw new Exception("La Fecha no es valida.");
            if (String.IsNullOrWhiteSpace(descripcion)) throw new Exception("No se ha ingresado una Descripcion.");
            if (personaId == Constants.InvalidInt) throw new Exception("No se ha seleccionado una Persona.");
            if (grupoId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Grupo.");
            if (String.IsNullOrWhiteSpace(ubicacion)) throw new Exception("No se ha ingresado una Ubicacion.");
        }

        public static void CreateComprobacion(int instrumentoId, DateTime fecha, string descripcion, int personaId, 
            int grupoId, string ubicacion)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            ValidarComprobacion(fecha, descripcion, personaId, grupoId, ubicacion);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                CreateComprobacion(instrumentoId, fecha, descripcion, personaId, grupoId, ubicacion, trans);

                trans.Commit();
            }
            catch
            {
                if (trans != null) trans.Rollback();
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private static void CreateComprobacion(int instrumentoId, DateTime fecha, string descripcion, int personaId,
            int grupoId, string ubicacion, IDbTransaction trans)
        {
            // Inserto el Comprobacion.
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO InstrumentosComprobaciones (InstrumentoId, Fecha, Descripcion, PersonaId, ";
            cmd.CommandText += "GrupoId, Ubicacion) VALUES (@InstrumentoId, @Fecha, @Descripcion, @PersonaId, @GrupoId, ";
            cmd.CommandText += "@Ubicacion); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM InstrumentosComprobaciones;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@InstrumentoId", instrumentoId));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", personaId));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@GrupoId", grupoId));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Ubicacion", ubicacion));
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            // Actualizo el instrumento.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE Instrumentos SET UltComprobId = @UltComprobId WHERE Id = @Id";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@UltComprobId", id));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", instrumentoId));
            cmd.ExecuteNonQuery();
        }

        public static List<InstrumentoComprobacion> ListComprobaciones(int id)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            List<InstrumentoComprobacion> result = new List<InstrumentoComprobacion>();

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT c.Fecha, c.Descripcion, c.PersonaId, p.Nombre, g.Nombre AS Grupo, c.Ubicacion ";
                cmd.CommandText += "FROM InstrumentosComprobaciones c ";
                cmd.CommandText += "INNER JOIN Personas p ON c.PersonaId = p.Id ";
                cmd.CommandText += "INNER JOIN InstrumentosGrupos g ON c.GrupoId = g.Id ";
                cmd.CommandText += "WHERE c.InstrumentoId = @InstrumentoId ORDER BY c.Fecha DESC";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@InstrumentoId", id));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new InstrumentoComprobacion(Convert.ToDateTime(dr["Fecha"]), dr["Descripcion"].ToString(),
                        Convert.ToInt32(dr["PersonaId"]), Persona.NormalizarNombre(dr["Nombre"].ToString()), dr["Grupo"].ToString(),
                        dr["Ubicacion"].ToString()));
                }

                dr.Close();
            }
            catch
            {
                result.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        #endregion

        #region Pager

        private static InstrumentoSummary ReadSummary(DataRow dr)
        {
            var result = new InstrumentoSummary
            {
                Id = Convert.ToInt32(dr["Id"]),
                Numero = Convert.ToInt32(dr["Numero"]),
                Tipo = dr["Tipo"] as string,
                Descripcion = dr["Descripcion"] as string,
                Marca = dr["Marca"] as string,
                Modelo = dr["Modelo"] as string,
                NumeroSerie = dr["NumSerie"] as string,
                Resolucion = dr["Resolucion"] as string,
                Rango = dr["Rango"] as string,
                Clase = dr["Clase"] as string,
                Incertidumbre = dr["Incertidumbre"] as string,
                
                CalibFrec = Convert.ToInt32(dr["FrecCalib"]),
                CalibFecha = dr["CalibFecha"] == DBNull.Value ? Constants.InvalidDate : Convert.ToDateTime(dr["CalibFecha"]).ToString("dd/MM/yyyy"),

                MantFrec = Convert.ToInt32(dr["FrecMant"]),
                MantFecha = dr["MantFecha"] == DBNull.Value ? Constants.InvalidDate : Convert.ToDateTime(dr["MantFecha"]).ToString("dd/MM/yyyy"),

                ComprobFecha = dr["ComprobFecha"] == DBNull.Value ? Constants.InvalidDate : Convert.ToDateTime(dr["ComprobFecha"]).ToString("dd/MM/yyyy"),
                ComprobGrupo = dr["ComprobGrupo"] == DBNull.Value ? "N/D" : dr["ComprobGrupo"].ToString(),
                ComprobUbicacion = dr["ComprobUbicacion"] == DBNull.Value ? "N/D" : dr["ComprobUbicacion"].ToString(),
                ComprobPersona = dr["ComprobPersona"] == DBNull.Value ? "N/D" : dr["ComprobPersona"].ToString(),
                ComprobPersonaEmail = dr["ComprobPersonaEmail"] == DBNull.Value ? "" : dr["ComprobPersonaEmail"].ToString(),

                Activo = Convert.ToBoolean(dr["Activo"])
            };

            result.Calib = result.CalibFrec > 0;
            result.CalibVencida = IsVencido(result.CalibFrec, Convert.ToDateTime(result.CalibFecha));
            result.CalibProxVencer = IsProxVencer(result.CalibFrec, Convert.ToDateTime(result.CalibFecha));

            result.MantVencido = IsVencido(result.MantFrec, Convert.ToDateTime(result.MantFecha));
            result.MantProxVencer = IsProxVencer(result.MantFrec, Convert.ToDateTime(result.MantFecha));

            result.HasCertCalib = HasCertif(result.Numero);
            result.HasManuales = HasManuales(result.Numero);
            result.HasEac = HasEac(result.Numero);
            result.HasComprobMant = HasComprobMantFiles(result.Numero);

            result.ImagePathHash = FileHelper.EncryptPath(PathImagen(result.Numero) + "0.jpg", "inst.jpg");

            return result;
        }

        internal static List<InstrumentoSummary> List(int pagina, List<Filtro> filtros)
        {
            List<InstrumentoSummary> result = DataAccess.GetDataList(BDConexiones.Intranet, pagina, filtros, 
                MAX_REGS_PAGINA, ListQuery, ReadSummary);

            return result;
        }

        internal static int ListPaginas(List<Filtro> filtros)
        {
            return DataAccess.GetCantidadPaginasData(filtros, MAX_REGS_PAGINA, ListQuery);
        }

        private static string ListQuery(List<Filtro> filtros, bool cantidad)
        {
            return filtros.Any(f => f.Tipo == (int)FiltroInstrumento.BusquedaExtendida)
                ? ListQueryExt(filtros, cantidad)
                : ListQueryNormal(filtros, cantidad);
        }

        private static string ListQueryExt(List<Filtro> filtros, bool cantidad)
        {
            string filtroWhere = "1=1 ";
            string filtroJoin = "";
            string consulta;

            foreach (Filtro filtro in filtros)
            {
                switch (filtro.Tipo)
                {
                    case (int)FiltroInstrumento.Activo:
                        filtroWhere += "AND i.Activo = 1 ";
                        break;
                    case (int)FiltroInstrumento.Calib:
                        filtroWhere += "AND i.FrecCalib > 0 AND -DATEDIFF(DAY, GETDATE(), c.Fecha) <= (i.FrecCalib*30) ";
                        break;
                    case (int)FiltroInstrumento.CalibProx:
                        filtroWhere += "AND i.FrecCalib > 0 AND ((-DATEDIFF(MONTH, GETDATE(), c.Fecha))*30) >= "; 
                        filtroWhere += "(i.FrecCalib*30 - " + DIAS_AVISO_VTO + ") AND ((-DATEDIFF(MONTH, GETDATE(), c.Fecha))*30) < ";
                        filtroWhere += "(i.FrecCalib*30) ";
                        break;
                    case (int)FiltroInstrumento.CalibVencido:
                        filtroWhere += "AND i.FrecCalib > 0 AND (c.Fecha IS NULL OR -DATEDIFF(DAY, GETDATE(), c.Fecha) > (i.FrecCalib*30)) ";
                        break;
                    case (int)FiltroInstrumento.Mant:
                        filtroWhere += "AND i.FrecMant > 0 AND -DATEDIFF(DAY, GETDATE(), ma.Fecha) <= (i.FrecMant*30) ";
                        break;
                    case (int)FiltroInstrumento.MantProx:
                        filtroWhere += "AND i.FrecMant > 0 AND  ((-DATEDIFF(MONTH, GETDATE(), ma.Fecha))*30) >= ";
                        filtroWhere += "(i.FrecMant*30 - " + DIAS_AVISO_VTO + ") AND ((-DATEDIFF(MONTH, GETDATE(), ma.Fecha))*30) < ";
                        filtroWhere += "(i.FrecMant*30) ";
                        break;
                    case (int)FiltroInstrumento.MantVencido:
                        filtroWhere += "AND i.FrecMant > 0 AND (ma.Fecha IS NULL OR -DATEDIFF(DAY, GETDATE(), ma.Fecha) > (i.FrecMant*30)) ";
                        break;
                    case (int)FiltroInstrumento.BusquedaExtendida:
                        break;
                    default:
                        filtroWhere += "AND (";
                        filtroWhere += "i.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR m.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR i.Modelo LIKE '%" + filtro.Valor + "%' ";
                        int i;
                        if (Int32.TryParse(filtro.Valor.ToString(), out i))
                        {
                            filtroWhere += "OR i.Numero = " + filtro.Valor + " ";
                        }
                        filtroWhere += "OR i.NumSerie LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR t.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += ")";
                        break;
                }
            }

            filtroJoin = "LEFT JOIN InstrumentosTipos t ON i.TipoId = t.Id ";
            filtroJoin += "LEFT JOIN InstrumentosMarcas m ON i.MarcaId = m.Id ";
            filtroJoin += "LEFT JOIN InstrumentosCalibraciones c ON i.UltCalibId = c.Id ";
            filtroJoin += "LEFT JOIN InstrumentosMantenimientos ma ON i.UltMantId = ma.Id ";
            filtroJoin += "LEFT JOIN InstrumentosComprobaciones co ON i.UltComprobId = co.Id ";
            filtroJoin += "LEFT JOIN Personas cop ON cop.Id = co.PersonaId ";
            filtroJoin += "LEFT JOIN InstrumentosGrupos g ON co.GrupoId = g.Id ";

            consulta = cantidad ? 
                "SELECT COUNT(i.Id) as TotalRegistros" : 
                ("SELECT i.Id, i.Numero, i.Descripcion, i.NumSerie, m.Descripcion AS Marca, "
                + "t.Descripcion AS Tipo, i.Modelo, i.Rango, i.Resolucion, i.Clase, i.Incertidumbre, "
                + "i.Activo, i.FrecCalib, c.Fecha AS CalibFecha, i.FrecMant, ma.Fecha AS MantFecha, "
                + "co.Fecha AS ComprobFecha, co.Ubicacion AS ComprobUbicacion, cop.Nombre AS ComprobPersona, "
                + "g.Nombre AS ComprobGrupo, cop.Email AS ComprobPersonaEmail ");
            
            consulta += " FROM Instrumentos i " + filtroJoin + " WHERE " + filtroWhere;

            if (!cantidad)
            {
                consulta += " ORDER BY i.Numero";
            }

            return consulta;
        }

        private static string ListQueryNormal(List<Filtro> filtros, bool cantidad)
        {
            // Modelo, Marca, Nro.serie, Número.
            // Sólo para comprobación, buscar por Ubicación, Grupo y Responsable (cuando es el último registro del historial)

            var filtroWhere = "";
            var filtroJoin = "";

            foreach (var filtro in filtros)
            {
                switch (filtro.Tipo)
                {
                    default:
                        if (string.IsNullOrEmpty(filtro.Valor.ToString())) break;

                        filtroWhere += "AND (";

                        // Marca.
                        filtroWhere += "m.Descripcion LIKE '%" + filtro.Valor + "%' ";

                        // Modelo.
                        filtroWhere += "OR i.Modelo LIKE '%" + filtro.Valor + "%' ";

                        // Número.
                        int i;
                        if (int.TryParse(filtro.Valor.ToString(), out i))
                        {
                            filtroWhere += "OR i.Numero = " + filtro.Valor + " ";
                        }

                        // Número de serie.
                        filtroWhere += "OR i.NumSerie LIKE '%" + filtro.Valor + "%' ";

                        // Comprobación.
                        filtroWhere += "OR co.Ubicacion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR g.Nombre LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR cop.Nombre LIKE '%" + filtro.Valor + "%' ";

                        filtroWhere += ")";
                        break;
                }
            }

            filtroJoin = "LEFT JOIN InstrumentosTipos t ON i.TipoId = t.Id ";
            filtroJoin += "LEFT JOIN InstrumentosMarcas m ON i.MarcaId = m.Id ";
            filtroJoin += "LEFT JOIN InstrumentosCalibraciones c ON i.UltCalibId = c.Id ";
            filtroJoin += "LEFT JOIN InstrumentosMantenimientos ma ON i.UltMantId = ma.Id ";
            filtroJoin += "LEFT JOIN InstrumentosComprobaciones co ON i.UltComprobId = co.Id ";
            filtroJoin += "LEFT JOIN Personas cop ON cop.Id = co.PersonaId ";
            filtroJoin += "LEFT JOIN InstrumentosGrupos g ON co.GrupoId = g.Id ";

            var consulta = cantidad ?
                "SELECT COUNT(i.Id) as TotalRegistros" :
                ("SELECT i.Id, i.Numero, i.Descripcion, i.NumSerie, m.Descripcion AS Marca, "
                 + "t.Descripcion AS Tipo, i.Modelo, i.Rango, i.Resolucion, i.Clase, i.Incertidumbre, "
                 + "i.Activo, i.FrecCalib, c.Fecha AS CalibFecha, i.FrecMant, ma.Fecha AS MantFecha, "
                 + "co.Fecha AS ComprobFecha, co.Ubicacion AS ComprobUbicacion, cop.Nombre AS ComprobPersona, "
                 + "g.Nombre AS ComprobGrupo, cop.Email AS ComprobPersonaEmail ");

            consulta += " FROM Instrumentos i " + filtroJoin + " WHERE 1=1 " + filtroWhere;

            if (!cantidad)
            {
                consulta += " ORDER BY i.Numero";
            }

            return consulta;
        }

        public static Pager<InstrumentoSummary> Pager(int pagina, List<Filtro> filtros)
        {
            Pager<InstrumentoSummary> result = new Pager<InstrumentoSummary>();

            try
            {
                result.Items = List(pagina, filtros);
                result.TotalPages = ListPaginas(filtros);
            }
            catch
            {
                result.Items = null;
                result.TotalPages = Constants.InvalidInt;
            }

            return result;
        }

        #endregion

        #region Paths

        private static string PathBase(string path, int numero)
        {
            string result;

            result = path.Replace("@NUMERO", numero.ToString());

            return result;
        }

        public static string PathImagen(int numero)
        {
            return PathBase(PATH_IMAGENES, numero);
        }

        public static string PathCertif(int numero)
        {
            return PathBase(PATH_CERTIFICADOS, numero);
        }

        public static bool HasCertif(int numero)
        {
            return File.Exists(PathCertif(numero));
        }

        public static string PathEac(int numero)
        {
            return PathBase(PATH_EAC, numero);
        }

        public static bool HasEac(int numero)
        {
            return File.Exists(PathEac(numero));
        }

        public static bool HasManuales(int numero)
        {
            string path = PathBase(PATH_MANUALES, numero);
            bool result = Directory.Exists(path) && Directory.GetFiles(path).Length > 0;

            return result;
        }

        public static List<FileSummary> ListImagenes(int numero)
        {
            List<FileInfo> result = new List<FileInfo>();

            try
            {
                string path = PathImagen(numero);
                List<string> archivos = new List<string>(Directory.GetFiles(path, "*.jpg"));
                archivos.ForEach(m => result.Add(new FileInfo(m)));
            }
            catch
            {
                result.Clear();
            }

            return FileHelper.FileInfoToFileSummary(result);
        }

        public static List<FileSummary> ListManuales(int numero)
        {
            List<FileInfo> result = new List<FileInfo>();

            try
            {
                string path = PathBase(PATH_MANUALES, numero);
                List<string> manuales = new List<string>(Directory.GetFiles(path, "*.pdf"));
                manuales.ForEach(m => result.Add(new FileInfo(m)));
            }
            catch
            {
                result.Clear();
            }

            return FileHelper.FileInfoToFileSummary(result);
        }

        public static bool HasComprobMantFiles(int numero)
        {
            return GetComprobMantFiles(numero).Count > 0;
        }

        public static List<FileSummary> GetComprobMantFiles(int numero)
        {
            var result = new List<FileSummary>();

            try
            {
                var path = PathBase(PATH_COMPROB_MANT, numero);
                result = FileHelper.GetFilesFromPath(path, f => true);
            }
            catch
            {
                result.Clear();
            }

            return result;
        }

        #endregion
    }
}