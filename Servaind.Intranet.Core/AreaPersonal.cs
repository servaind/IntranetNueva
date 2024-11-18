using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Servaind.Intranet.Core
{
    public class AreaPersonal
    {
        // Propiedades.
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public List<Persona> Responsables { get; private set; }


        private AreaPersonal(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
            Responsables = new List<Persona>();
        }

        private static AreaPersonal Read(IDataReader dr)
        {
            return new AreaPersonal(Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString());
        }

        public static List<AreaPersonal> List(bool soloActivas = true)
        {
            List<AreaPersonal> result = new List<AreaPersonal>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT * FROM Areas ";
                if (soloActivas)
                {
                    cmd.CommandText += "WHERE Activa = @Activa ";
                }
                cmd.CommandText += "ORDER BY Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", true));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var area = Read(dr);
                    if (area != null) result.Add(area);
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

        public static AreaPersonal Read(int id)
        {
            AreaPersonal result = null;
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT * FROM Areas WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                dr = cmd.ExecuteReader();

                if (dr.Read()) result = Read(dr);

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

        public void LoadResponsables()
        {
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT ResponsableId FROM AreasResponsables WHERE AreaId = @AreaId";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AreaId", Id));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var p = Persona.Read(Convert.ToInt32(dr["ResponsableId"]));
                    if (p != null && p.Activo) Responsables.Add(p);
                }

                dr.Close();
            }
            catch
            {
                Responsables.Clear();
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }
        }

        public string ListEmailsResponsables()
        {
            string result = "";

            Responsables.ForEach(p => result += p.Email + ",");
            result = result.TrimEnd(',');
            
            return result;
        }
    }
}
