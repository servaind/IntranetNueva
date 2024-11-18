using System;
using System.Collections.Generic;
using System.Data;
using Proser.Common;
using Proser.Common.Data;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    public class PersonaSummary
    {
        // Propiedades.
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Responsable { get; set; }
        public bool EnPc { get; set; }
        public bool Activo { get; set; }
    }

    public class Persona : IComparable<Persona>, IEquatable<Persona>
    {
        // Constantes.
        private const string DEFAULT_NOMBRE = "No encontrado";
        private const string DEFAULT_EMAIL = "NoEncontrado";
        private const string DEFAULT_USUARIO = "NoEncontrado";
        private const int MAX_REGS_PAGINA = 10;
        public const int ID_INVALIDO = 1;

        // Variables.
        private Persona responsable;
        private List<PermisosPersona> permisos;

        // Propiedades.
        public string Nombre { get; private set; }
        public string NombreDominio
        {
            get
            {
                return String.Format("{0}\\{1}", UsuarioHelper.DOMINIO, Usuario);
            }
        }
        public string Email { get; private set; }
        public string Usuario { get; private set; }
        public int Id { get; private set; }
        public int ResponsableId { get; private set; }
        public bool EnPanelControl { get; private set; }
        public bool Activo { get; private set; }
        public Persona Responsable
        {
            get
            {
                return responsable ?? (responsable = Read(ResponsableId));
            }
        }
        public string Cuil { get; private set; }
        public DateTime HoraEntrada { get; private set; }
        public DateTime HoraSalida { get; private set; }
        public string Legajo { get; private set; }
        public int BaseId { get; private set; }
        public List<PermisosPersona> Permisos
        {
            get { return permisos ?? (permisos = PermisoPersona.ListPersona(Id)); }
        }
        public bool IsInvalida { get { return Id == Constants.InvalidInt; } }


        internal Persona(int id, string nombre, string email, string usuario, int responsableId, bool enPanelControl,
            bool activo, string legajo, string cuil, DateTime horaEntrada, DateTime horaSalida, int baseId)
        {
            Id = id;
            Nombre = nombre;
            Email = email;
            Usuario = usuario;
            ResponsableId = responsableId;
            EnPanelControl = enPanelControl;
            Activo = activo;
            Legajo = legajo;
            Cuil = cuil;
            HoraEntrada = horaEntrada;
            HoraSalida = horaSalida;
            BaseId = baseId;
        }

        public int CompareTo(Persona p)
        {
            return Nombre.CompareTo(p.Nombre);
        }

        public bool Equals(Persona other)
        {
            return Id == other.Id;
        }

        public override string ToString()
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Nombre.ToLower());
        }

        public bool TieneAcceso(PermisosPersona permiso)
        {
            return PermisoPersona.TieneAcceso(this, permiso);
        }

        public static Persona Invalida()
        {
            return new Persona(Constants.InvalidInt, DEFAULT_NOMBRE, DEFAULT_EMAIL, DEFAULT_USUARIO,
                Constants.InvalidInt, false, false, String.Empty, String.Empty, DateTime.Now, DateTime.Now,
                Constants.InvalidInt);
        }

        internal static Persona Read(IDataReader dr)
        {
            Persona result;
            DateTime now = DateTime.Now;

            try
            {
                int hEntrada = Convert.ToInt32(dr["HorarioEntrada"]);
                int hSalida = Convert.ToInt32(dr["HorarioSalida"]);

                DateTime horaEntrada = new DateTime(now.Year, now.Month, now.Day, hEntrada/100,
                    hEntrada - (hEntrada/100)*100, 0);
                DateTime horaSalida = new DateTime(now.Year, now.Month, now.Day, hSalida / 100,
                    hSalida - (hSalida / 100) * 100, 0);

                result = new Persona(Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString(), dr["Email"].ToString(),
                    dr["Usuario"].ToString(), Convert.ToInt32(dr["ResponsableId"]),
                    Convert.ToBoolean(dr["EnPanelControl"]),
                    Convert.ToBoolean(dr["Activo"]), dr["Legajo"].ToString(), dr["CUIL"].ToString(),
                    horaEntrada, horaSalida, Convert.ToInt32(dr["BaseId"]));
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private static Persona Read(DataRow dr)
        {
            Persona result;
            DateTime now = DateTime.Now;

            try
            {
                int hEntrada = Convert.ToInt32(dr["HorarioEntrada"]);
                int hSalida = Convert.ToInt32(dr["HorarioSalida"]);

                DateTime horaEntrada = new DateTime(now.Year, now.Month, now.Day, hEntrada / 100,
                    hEntrada - (hEntrada / 100) * 100, 0);
                DateTime horaSalida = new DateTime(now.Year, now.Month, now.Day, hSalida / 100,
                    hSalida - (hSalida / 100) * 100, 0);

                result = new Persona(Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString(), dr["Email"].ToString(),
                    dr["Usuario"].ToString(), Convert.ToInt32(dr["ResponsableId"]),
                    Convert.ToBoolean(dr["EnPanelControl"]),
                    Convert.ToBoolean(dr["Activo"]), dr["Legajo"].ToString(), dr["CUIL"].ToString(),
                    horaEntrada, horaSalida, Convert.ToInt32(dr["BaseId"]));
            }
            catch
            {
                result = null;
            }

            return result;
        }

        internal static string PersonalToString(List<Persona> lstPersonal, char separador)
        {
            string s = "";

            foreach (Persona p in lstPersonal)
            {
                s += p.Nombre + separador;
            }
            s = s.TrimEnd(separador);

            return s;
        }

        public static List<Persona> ListActivas()
        {
            return List(true);
        }

        public static List<Persona> ListResponsablesNNC()
        {
            List<Persona> result = new List<Persona>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.*, ISNULL(b.BaseId, @ValorInvalido) AS BaseId FROM Personas p ";
                cmd.CommandText += "LEFT JOIN BasesPersonas b ON b.PersonaId = p.Id ";
                cmd.CommandText += "LEFT JOIN PersonasPermisos pp ON p.Id = pp.PersonaId ";
                cmd.CommandText += "WHERE p.Activo = 1 AND pp.PermisoId = @PermisoId ";
                cmd.CommandText += "ORDER BY p.Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@PermisoId", (int) PermisosPersona.NNCResponsable));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constants.InvalidInt));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Persona persona = Read(dr);
                    if (persona != null)
                    {
                        result.Add(persona);
                    }
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

        public static List<Persona> List(bool soloActivas = false)
        {
            return List(null, soloActivas);
        }

        public static List<Persona> List(int[] idsPersonal, bool soloActivas = false)
        {
            List<Persona> result = new List<Persona>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.*, ISNULL(b.BaseId, @ValorInvalido) AS BaseId FROM Personas p ";
                cmd.CommandText += "LEFT JOIN BasesPersonas b ON b.PersonaId = p.Id ";
                cmd.CommandText += "WHERE 1=1 ";
                cmd.CommandText += (soloActivas ? "AND p.Activo = @Activo " : "");
                if (idsPersonal != null)
                {
                    cmd.CommandText += "AND p.Id IN (";
                    foreach(int id in idsPersonal)
                    {
                        cmd.CommandText += id + ",";
                    }
                    cmd.CommandText = cmd.CommandText.TrimEnd(',');

                    cmd.CommandText += ") ";
                }
                cmd.CommandText += "ORDER BY p.Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constants.InvalidInt));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Persona persona = Read(dr);
                    if (persona != null)
                    {
                        result.Add(persona);
                    }
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

        public static bool ExistePersona(int id)
        {
            IDbConnection conn = null;
            IDbCommand cmd;
            bool resultado;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Count(Id) FROM Personas WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));

                resultado = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                resultado = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return resultado;
        }

        public static bool ExistePersona(string usuario)
        {
            IDbConnection conn = null;
            IDbCommand cmd;
            bool resultado;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Count(Id) FROM Personas WHERE Usuario = @Usuario";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));

                resultado = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                resultado = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return resultado;
        }

        public static Persona Read(int id)
        {
            IDbConnection conn = null;
            Persona personal;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);

                personal = Read(id, conn);
            }
            catch (Exception ex)
            {
                personal = Invalida();
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return personal;
        }

        internal static Persona Read(int id, IDbConnection conn)
        {
            IDataReader dr = null;
            Persona personal;

            try
            {
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.*, ISNULL(b.BaseId, @ValorInvalido) AS BaseId FROM Personas p ";
                cmd.CommandText += "LEFT JOIN BasesPersonas b ON b.PersonaId = p.Id ";
                cmd.CommandText += "WHERE p.Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constants.InvalidInt));
                dr = cmd.ExecuteReader();

                if (!dr.Read()) throw new ElementoInexistenteException();

                personal = Read(dr);

                dr.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
            }

            return personal;
        }

        public static Persona Read(string usuario)
        {
            IDbConnection conn = null;
            IDbCommand cmd;
            IDataReader dr;
            Persona personal;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.*, ISNULL(b.BaseId, @ValorInvalido) AS BaseId FROM Personas p ";
                cmd.CommandText += "LEFT JOIN BasesPersonas b ON b.PersonaId = p.Id ";
                cmd.CommandText += "WHERE p.Usuario = @Usuario";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constants.InvalidInt));
                dr = cmd.ExecuteReader();

                if (!dr.Read())
                {
                    //La persona no existe.
                    throw new ElementoInexistenteException();
                }

                personal = Read(dr);
            
                dr.Close();
            }
            catch
            {
                personal = Invalida();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return personal;
        }

        public static string ReadNombre(int id)
        {
            IDbConnection conn = null;
            IDbCommand cmd;
            string result = "";

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Nombre FROM Personas WHERE Id = @idPersonal";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", id));

                result = cmd.ExecuteScalar().ToString();
            }
            catch
            {
                result = DEFAULT_NOMBRE;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return result;
        }

        public static string ReadEmail(int id)
        {
            IDbConnection conn = null;
            IDbCommand cmd;
            string result = "";

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Email FROM Personas WHERE Id = @idPersonal";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", id));

                result = cmd.ExecuteScalar().ToString();
            }
            catch
            {
                result = DEFAULT_EMAIL;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return result;
        }

        private static void Validar(string nombre, string email, string usuario, int responsableId, bool enPanelControl,
            bool activo, string legajo, string cuil, string horaEntrada, string horaSalida, int baseId)
        {
            if (String.IsNullOrWhiteSpace(nombre)) throw new Exception("El Nombre no puede estar vacio.");
            if (String.IsNullOrWhiteSpace(email)) throw new Exception("El Email no puede estar vacio.");
            if (String.IsNullOrWhiteSpace(usuario)) throw new Exception("El Usuario no puede estar vacio.");
            if (responsableId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un responsable.");
            if (!TryParseHora(horaEntrada)) throw new Exception("La hora de entrada no es valida. El formato debe ser HH:mm.");
            if (!TryParseHora(horaSalida)) throw new Exception("La hora de salida no es valida. El formato debe ser HH:mm.");
        }

        public static int Create(string nombre, string email, string usuario, int responsableId, bool enPanelControl,
            bool activo, string legajo, string cuil, string horaEntrada, string horaSalida, int baseId)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;
            int result;

            Validar(nombre, email, usuario, responsableId, enPanelControl, activo, legajo, cuil, horaEntrada, horaSalida,
                baseId);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "INSERT INTO Personas (Nombre, Email, Usuario, ResponsableId, EnPanelControl, ";
                cmd.CommandText += "Activo, Cuil, HorarioEntrada, HorarioSalida, Legajo) VALUES (@Nombre, @Email, @Usuario, ";
                cmd.CommandText += "@ResponsableId, @EnPanelControl, @Activo, @Cuil, @HorarioEntrada, @HorarioSalida, @Legajo); ";
                cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM Personas;";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Email", email));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EnPanelControl", enPanelControl));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", activo));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Cuil", cuil));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioEntrada", ParseHora(horaEntrada)));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioSalida", ParseHora(horaSalida)));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Legajo", legajo));
                result = Convert.ToInt32(cmd.ExecuteScalar());

                // Lo asigno a la base.
                Base.AddPersona(baseId, result, trans);

                trans.Commit();
            }
            catch
            {
                result = Constants.InvalidInt;
                if(trans != null) trans.Rollback();
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return result;
        }

        public static void Update(int id, string nombre, string email, string usuario, int responsableId,
            bool enPanelControl, bool activo, string legajo, string cuil, string horaEntrada, string horaSalida, int baseId)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;
            IDbCommand cmd;

            Validar(nombre, email, usuario, responsableId, enPanelControl, activo, legajo, cuil, horaEntrada, horaSalida,
                baseId);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);
                cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "UPDATE Personas SET Nombre = @Nombre, Email = @email, ";
                cmd.CommandText += "Usuario = @Usuario, ResponsableId = @ResponsableId, EnPanelControl = @EnPanelControl, ";
                cmd.CommandText += "Activo = @Activo, HorarioEntrada = @HorarioEntrada, HorarioSalida = @HorarioSalida, ";
                cmd.CommandText += "Legajo = @Legajo, Cuil = @Cuil ";
                cmd.CommandText += "WHERE Id = @idPersonal";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Email", email));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EnPanelControl", enPanelControl));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", activo));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioEntrada", ParseHora(horaEntrada)));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioSalida", ParseHora(horaSalida)));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Legajo", legajo));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Cuil", cuil));
                cmd.ExecuteNonQuery();

                // Lo asigno a la base.
                Base.DeletePersona(id, trans);
                Base.AddPersona(baseId, id, trans);

                trans.Commit();
            }
            catch
            {
                if(trans != null) trans.Rollback();
                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
       
        internal static Dictionary<int, string> ListFromResponsable(int id, bool enPanelControl, bool soloActivas = true)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            IDbConnection conn = null;
            IDataReader dr = null;
            
            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);

                Persona usr = Read(id, conn);

                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Id, UPPER(Nombre) AS Nombre FROM Personas WHERE ";
                cmd.CommandText += "EnPanelControl = @EnPanelControl AND (1=1 ";
                if (usr != null && !usr.TieneAcceso(PermisosPersona.RolDireccion)
                    && !usr.TieneAcceso(PermisosPersona.PCVerGeneral))
                {
                    cmd.CommandText += "AND Id = @ResponsableId OR ResponsableId = @ResponsableId ";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", id));
                }
                // Elio Zapata tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
                if (id == 125)
                {
                    cmd.CommandText += "OR Id IN (21, 97, 108, 113, 126, 127)";
                }
                // José Garcés tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
                if (id == 188)
                {
                    cmd.CommandText += "OR Id IN (29, 195, 196)";
                }
                // Paulo Velardes tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
                if (id == 56)
                {
                    cmd.CommandText += "OR Id IN (56,37,318,132,108,78,126,113,127,97,315,125,31,205,166,304,47,186,278,29)";
                }
                // Mariano Arévalo tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
                if (id == 276)
                {
                    cmd.CommandText += "OR Id IN (161,153,232,156,157,183)";
                }
                cmd.CommandText += ")";
                if (soloActivas)
                {
                    cmd.CommandText += " AND Activo = @Activo ";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", soloActivas));
                }
                cmd.CommandText += " ORDER BY Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EnPanelControl", enPanelControl));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString());
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
                if (conn != null) { conn.Close(); }
            }
        
            List<int> customOrder = new List<int>();
            // Paulo Velardes no quiere el personal ordenado.
            if (id == 56)
            {
                customOrder = new List<int>
                {
                    56,
                    37,
                    318,
                    132,
                    108,
                    78,
                    126,
                    113,
                    127,
                    97,
                    315,
                    125,
                    31,
                    205,
                    166,
                    304,
                    47,
                    186,
                    278,
                    29
                };
            }
            if (customOrder.Count > 0)
            {
                Dictionary<int, string> newOrder = new Dictionary<int, string>();
                customOrder.ForEach(p =>
                {
                    if (result.ContainsKey(p)) newOrder.Add(p, result[p]);
                });
                result = newOrder;
            }

            return result;
        }

        internal static List<Persona> ListInfObra(int id, string tabla)
        {
            List<Persona> result = new List<Persona>();
            IDbConnection conn = null;
            IDbCommand cmd;
            IDataReader dr;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.*, ISNULL(b.BaseId, @ValorInvalido) AS BaseId FROM Personas p ";
                cmd.CommandText += "INNER JOIN " + tabla + " t ON p.Id = t.PersonaId ";
                cmd.CommandText += "LEFT JOIN BasesPersonas b ON b.PersonaId = p.Id ";
                cmd.CommandText += "WHERE p.Activo = @Activo AND idObraHistorico = @idObraHistorico ORDER BY p.Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idObraHistorico", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constants.InvalidInt));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Persona persona = Read(dr);
                    if (persona != null)
                    {
                        result.Add(persona);
                    }
                }

                dr.Close();
            }
            catch
            {
                result.Clear();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return result;
        }

        public static string NormalizarNombre(string n)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(n.ToLower());
        }

        private static int ParseHora(string h)
        {
            return Convert.ToInt32(h.Replace(":", ""));
        }

        private static bool TryParseHora(string h)
        {
            try
            {
                ParseHora(h);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #region Pager

        private static PersonaSummary ReadSummary(DataRow dr)
        {
            return new PersonaSummary
            {
                Id = Convert.ToInt32(dr["Id"]),
                Nombre = NormalizarNombre(dr["Nombre"].ToString()),
                Usuario = dr["Usuario"].ToString(),
                Responsable = NormalizarNombre(dr["Responsable"].ToString()),
                EnPc = Convert.ToBoolean(dr["EnPanelControl"]),
                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }

        internal static List<PersonaSummary> List(int pagina, List<Filtro> filtros)
        {
            List<PersonaSummary> result = DataAccess.GetDataList(BDConexiones.Intranet, pagina, filtros, MAX_REGS_PAGINA,
                ListQuery, ReadSummary);

            return result;
        }

        internal static int ListPaginas(List<Filtro> filtros)
        {
            return DataAccess.GetCantidadPaginasData(filtros, MAX_REGS_PAGINA, ListQuery);
        }

        private static string ListQuery(List<Filtro> filtros, bool cantidad)
        {
            string filtroJoin = "";
            string filtroWhere = "";
            string consulta;

            foreach (Filtro filtro in filtros)
            {
                switch (filtro.Tipo)
                {
                    case (int)FiltrosPersona.Id:
                        filtroWhere += "AND p.Id = " + filtro.Valor + " ";
                        break;
                    case (int)FiltrosPersona.Nombre:
                        filtroWhere += "AND p.Nombre LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case (int)FiltrosPersona.Usuario:
                        filtroWhere += "AND p.Usuario LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case (int)FiltrosPersona.Responsable:
                        filtroWhere += "AND p.ResponsableId = " + filtro.Valor + " ";
                        break;
                    case (int)FiltrosPersona.EnPanelControl:
                        filtroWhere += "AND p.EnPanelControl = " + filtro.Valor + " ";
                        break;
                    case (int)FiltrosPersona.Estado:
                        filtroWhere += "AND p.Activo = " + filtro.Valor + " ";
                        break;
                    default:
                        filtroWhere += "";
                        break;
                }
            }
            if (filtroWhere.Length > 0)
            {
                filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
            }

            if (cantidad)
            {
                consulta = "SELECT COUNT(p.Id) as TotalRegistros";
            }
            else
            {
                consulta = "SELECT p.Id, p.Nombre, p.Usuario, p.EnPanelControl, p.Activo, r.Nombre AS Responsable";
                filtroJoin += "LEFT JOIN Personas r ON r.Id = p.ResponsableId ";
            }

            if (filtroWhere.Length > 0)
            {
                filtroWhere = "WHERE " + filtroWhere;
            }
            consulta += " FROM Personas p " + filtroJoin + " " + filtroWhere;

            if (!cantidad)
            {
                consulta += " ORDER BY p.Nombre";
            }

            return consulta;
        }

        public static Pager<PersonaSummary> Pager(int pagina, List<Filtro> filtros)
        {
            Pager<PersonaSummary> result = new Pager<PersonaSummary>();

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
    }
}