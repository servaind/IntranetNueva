using System;
using System.Collections.Generic;
using System.Data;
using Proser.Common;
using Proser.Common.Data;

namespace Servaind.Intranet.Core
{
    public class Imputacion : IComparable<Imputacion>
    {
        // Constantes.
        private const int MAX_REGS_PAGINA = 10;

        // Propiedades.
        public int Id { get; private set; }
        public int Numero { get; private set; }
        public string Descripcion { get; private set; }
        public string DescripcionFull { get; private set; }
        public bool Activa { get; private set; }


        internal Imputacion(int id, int numero, string descripcion, bool activa)
        {
            Id = id;
            Numero = numero;
            Descripcion = descripcion;
            DescripcionFull = String.Format("{0} - {1}", numero, descripcion);
            Activa = activa;
        }

        public int CompareTo(Imputacion i)
        {
            return Numero.CompareTo(i.Numero);
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Numero, Descripcion);
        }

        private static Imputacion Read(DataRow dr)
        {
            Imputacion result;

            try
            {
                result = new Imputacion(Convert.ToInt32(dr["Id"]), Convert.ToInt32(dr["Numero"]), 
                    dr["Descripcion"].ToString(), Convert.ToBoolean(dr["Activa"]));
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private static Imputacion Read(IDataReader dr)
        {
            Imputacion result;

            try
            {
                result = new Imputacion(Convert.ToInt32(dr["Id"]), Convert.ToInt32(dr["Numero"]),
                    dr["Descripcion"].ToString(), Convert.ToBoolean(dr["Activa"]));
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public static Imputacion Read(int id)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            Imputacion result;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Id, Numero, RTRIM(Descripcion) AS Descripcion, Activa FROM ";
                cmd.CommandText += "Imputaciones WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                dr = cmd.ExecuteReader();

                if (!dr.Read()) throw new ElementoInexistenteException();

                result = Read(dr);

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

        public static Imputacion ReadFromNumero(int numero)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            Imputacion result;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Id, Numero, Descripcion, Activa FROM ";
                cmd.CommandText += "Imputaciones WHERE Numero = @Numero";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                dr = cmd.ExecuteReader();

                if (!dr.Read()) throw new ElementoInexistenteException();

                result = Read(dr);

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

        public static List<Imputacion> ListActivas()
        {
            return List(false);
        }

        public static List<Imputacion> List(bool cargarTodas = true)
        {
            IDbConnection conn = null;
            IDataReader dr = null;
            List<Imputacion> result = new List<Imputacion>();

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Id, Numero, Descripcion, Activa ";
                cmd.CommandText += "FROM Imputaciones ";
                if (!cargarTodas)
                {
                    cmd.CommandText += "WHERE Activa = 1 ";
                }
                cmd.CommandText += "ORDER BY Numero";
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Imputacion imputacion = Read(dr);
                    if (imputacion != null) result.Add(imputacion);
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

            result.Sort();

            return result;
        }

        private static void Validar(int numero, string descripcion)
        {
            if (numero <= 0) throw new Exception("El Numero debe ser mayor a 0.");
            if (String.IsNullOrWhiteSpace(descripcion)) throw new Exception("La Descripcion no puede estar vacia.");
        }

        public static void Create(int numero, string descripcion, bool activa = true)
        {
            Validar(numero, descripcion);

            if (Existe(numero)) throw new Exception("La Imputacion ya existe.");

            IDbConnection conn = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "INSERT INTO Imputaciones (Numero,Descripcion,Activa) VALUES ";
                cmd.CommandText += "(@Numero,@Descripcion,@Activa)";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));

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
        }

        public static bool Existe(int numero)
        {
            IDbConnection conn = null;
            bool result;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Count(Id) FROM Imputaciones WHERE Numero = @Numero";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));

                result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static void Update(int id, int numero, string descripcion, bool activa)
        {
            IDbConnection conn = null;

            Validar(numero, descripcion);

            Imputacion imp = ReadFromNumero(numero);
            if (imp != null && imp.Id != id) throw new Exception("La Imputacion ya existe.");

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "UPDATE Imputaciones SET Numero = @Numero, Descripcion = @Descripcion, Activa = @Activa WHERE ";
                cmd.CommandText += "Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));
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
        }

        #region Pager

        internal static List<Imputacion> List(int pagina, List<Filtro> filtros)
        {
            List<Imputacion> result = DataAccess.GetDataList(BDConexiones.Intranet, pagina, filtros, MAX_REGS_PAGINA,
                ListQuery, Read);

            return result;
        }

        internal static int ListPaginas(List<Filtro> filtros)
        {
            return DataAccess.GetCantidadPaginasData(filtros, MAX_REGS_PAGINA, ListQuery);
        }

        private static string ListQuery(List<Filtro> filtros, bool cantidad)
        {
            string filtroWhere = "";
            string filtroJoin = "";
            string consulta;

            foreach (Filtro filtro in filtros)
            {
                switch (filtro.Tipo)
                {
                    case (int)FiltrosImputacion.Numero:
                        filtroWhere += "AND Numero = " + filtro.Valor + " ";
                        break;
                    case (int)FiltrosImputacion.Descripcion:
                        filtroWhere += "AND Descripcion LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case (int)FiltrosImputacion.Estado:
                        filtroWhere += "AND Activa = " + filtro.Valor + " ";
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

            consulta = cantidad ? "SELECT COUNT(Id) as TotalRegistros" : "SELECT *";

            if (filtroWhere.Length > 0)
            {
                filtroWhere = "WHERE " + filtroWhere;
            }
            consulta += " FROM Imputaciones " + filtroJoin + " " + filtroWhere;

            if (!cantidad)
            {
                consulta += " ORDER BY Numero";
            }

            return consulta;
        }

        public static Pager<Imputacion> Pager(int pagina, List<Filtro> filtros)
        {
            Pager<Imputacion> result = new Pager<Imputacion>();

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
