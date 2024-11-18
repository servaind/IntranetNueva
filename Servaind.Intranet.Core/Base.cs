using System;
using System.Collections.Generic;
using System.Data;
using Proser.Common;
using Proser.Common.Data;

namespace Servaind.Intranet.Core
{
    public class Base
    {
        // Properties.
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public int ResponsableId { get; private set; }
        public string Responsable { get; private set; }
        public int AlternateId { get; private set; }
        public string Alternate { get; private set; }
        public List<Persona> Integrantes { get; private set; }
        public bool Activa { get; private set; }


        internal Base(int id, string nombre, int responsableId, string responsable, int alternateId, string alternate, 
            bool activa, List<Persona> integrantes)
        {
            Id = id;
            Nombre = nombre;
            ResponsableId = responsableId;
            Responsable = responsable;
            AlternateId = alternateId;
            Alternate = alternate;
            Activa = activa;
            Integrantes = integrantes;
        }

        internal void SetIntegrantes(List<Persona> integrantes)
        {
            Integrantes = integrantes;
        }

        private static void Validar(string nombre, int responsableId, int alternateId)
        {
            if (String.IsNullOrWhiteSpace(nombre)) throw new Exception("El Nombre no puede estar vacio.");
            if (responsableId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Responsable.");
            if (alternateId == Constants.InvalidInt) throw new Exception("No se ha seleccionado un Alternate.");
        }

        public static void Create(string nombre, int responsableId, int alternateId, bool activa = true)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            Validar(nombre, responsableId, alternateId);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "INSERT INTO Bases(Nombre, ResponsableId, AlternateId, Activa) VALUES (@Nombre, @ResponsableId, ";
                cmd.CommandText += "@AlternateId, @Activa); ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AlternateId", alternateId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));
                cmd.ExecuteNonQuery();

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

        public static void Delete(int id)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;
            IDbCommand cmd;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "DELETE FROM Bases WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                cmd.ExecuteNonQuery();

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

        internal static void AddPersona(int id, int personaId, IDbTransaction trans)
        {
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO BasesPersonas(BaseId, PersonaId) VALUES (@BaseId, @PersonaId)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseId", id));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", personaId));
            cmd.ExecuteNonQuery();
        }

        internal static void DeletePersona(int id, IDbTransaction trans)
        {
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "DELETE FROM BasesPersonas WHERE PersonaId = @PersonaId";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", id));
            cmd.ExecuteNonQuery();
        }

        public static void Update(int id, string nombre, int responsableId, int alternateId, bool activa)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            Validar(nombre, responsableId, alternateId);

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "UPDATE Bases SET Nombre = @Nombre, ResponsableId = @ResponsableId, Activa = @Activa, ";
                cmd.CommandText += "AlternateId = @AlternateId WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AlternateId", alternateId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));
                cmd.ExecuteNonQuery();

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

        public static List<DataSourceItem> List()
        {
            List<DataSourceItem> result = new List<DataSourceItem>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT Id, Nombre FROM Bases ORDER BY Nombre";
                dr = cmd.ExecuteReader();

                while (dr.Read()) result.Add(new DataSourceItem(Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString()));

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

            return result;
        }

        public static List<Base> ListAll(bool loadIntegrantes = true)
        {
            return List(false, loadIntegrantes);
        }

        public static List<Base> ListActivas(bool loadIntegrantes = true)
        {
            return List(true, loadIntegrantes);
        }

        private static List<Base> List(bool soloActivas, bool loadIntegrantes = true)
        {
            List<Base> result = new List<Base>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT b.Id, b.Nombre AS Base, b.Activa, p.Id AS ResponsableId, ";
                cmd.CommandText += "p.Nombre AS Responsable, p1.Id AS AlternateId, p1.Nombre AS Alternate FROM Bases b ";
                cmd.CommandText += "INNER JOIN Personas p ON p.Id = b.ResponsableId ";
                cmd.CommandText += "INNER JOIN Personas p1 ON p1.Id = b.AlternateId ";
                if (soloActivas)
                {
                    cmd.CommandText += "WHERE b.Activa = @Activa ";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", soloActivas));
                }
                cmd.CommandText += "ORDER BY b.Nombre";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(new Base(Convert.ToInt32(dr["Id"]), dr["Base"].ToString(),
                        Convert.ToInt32(dr["ResponsableId"]), dr["Responsable"].ToString(),
                        Convert.ToInt32(dr["AlternateId"]),
                        dr["Alternate"].ToString(), Convert.ToBoolean(dr["Activa"]), null));
                }
                dr.Close();

                if (loadIntegrantes) result.ForEach(b => b.SetIntegrantes(ListPersonas(b.Id, conn)));
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

            return result;
        }

        private static List<Persona> ListPersonas(int id, IDbConnection conn)
        {
            List<Persona> result = new List<Persona>();
            IDataReader dr = null;

            try
            {
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.*, b.BaseId FROM Personas p ";
                cmd.CommandText += "INNER JOIN BasesPersonas b ON b.PersonaId = p.Id ";
                cmd.CommandText += "WHERE b.BaseId = @BaseID AND p.Activo = @Activo ";
                cmd.CommandText += "ORDER BY p.Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseId", id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Persona persona = Persona.Read(dr);
                    if (persona != null) result.Add(persona);
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
            }

            return result;
        }

        public static Base Read(int id, bool loadIntegrantes = true)
        {
            Base result;
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT b.Nombre AS Base, b.Activa, p.Id AS ResponsableId, ";
                cmd.CommandText += "p.Nombre AS Responsable, p1.Id AS AlternateId, p1.Nombre AS Alternate FROM Bases b ";
                cmd.CommandText += "INNER JOIN Personas p ON p.Id = b.ResponsableId ";
                cmd.CommandText += "INNER JOIN Personas p1 ON p1.Id = b.AlternateId ";
                cmd.CommandText += "WHERE b.Id = @Id ORDER BY b.Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", id));
                dr = cmd.ExecuteReader();

                dr.Read();
                result = new Base(id, dr["Base"].ToString(),
                    Convert.ToInt32(dr["ResponsableId"]), dr["Responsable"].ToString(),
                    Convert.ToInt32(dr["AlternateId"]),
                    dr["Alternate"].ToString(), Convert.ToBoolean(dr["Activa"]), null);
                dr.Close();

                if (loadIntegrantes) result.SetIntegrantes(ListPersonas(id, conn));
            }
            catch
            {
                result = null;
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) { conn.Close(); }
            }

            return result;
        }
    }
}