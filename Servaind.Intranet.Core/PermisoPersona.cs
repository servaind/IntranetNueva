using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Proser.Common.Data;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    public class GrupoPermisos : IComparable<GrupoPermisos>
    {
        // Propiedades.
        public string Nombre { get; private set; }
        public SortedList<string, PermisosPersona> Permisos { get; private set; }


        internal GrupoPermisos(string nombre, SortedList<string, PermisosPersona> permisos)
        {
            Nombre = nombre;
            Permisos = permisos;
        }

        public int CompareTo(GrupoPermisos other)
        {
            return Nombre.CompareTo(other.Nombre);
        }
    }

    public class PermisoPersona
    {
        public static List<PermisosPersona> ListPersona(int id)
        {
            List<PermisosPersona> result = new List<PermisosPersona>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT PermisoId FROM PersonasPermisos WHERE PersonaId = @PersonaId";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", id));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    try
                    {
                        PermisosPersona permiso = (PermisosPersona)Convert.ToInt32(dr["PermisoId"]);
                        result.Add(permiso);
                    }
                    catch
                    {

                    }
                }

                dr.Close();
            }
            catch
            {

            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static bool TieneAcceso(Persona persona, PermisosPersona permiso)
        {
            bool result = persona.Permisos.Any(p => p == permiso);

            // Reviso si es usuario público.
            if (!result) result = persona.Permisos.Contains(PermisosPersona.Publico);

            return result;
        }

        public static List<GrupoPermisos> List()
        {
            List<GrupoPermisos> result = new List<GrupoPermisos>();
            
            // Administración del sistema.
            SortedList<string, PermisosPersona> lstAdminSistema = new SortedList<string, PermisosPersona>();
            lstAdminSistema.Add("Administrador", PermisosPersona.Administrador);
            lstAdminSistema.Add("Administración de imputaciones", PermisosPersona.AdminImputaciones);
            lstAdminSistema.Add("Administración de paneles de control de partes diarios", PermisosPersona.AdminPanelesControlPD);
            lstAdminSistema.Add("Administración de personal", PermisosPersona.AdminPersonal);
            lstAdminSistema.Add("Administración de partes diarios", PermisosPersona.AdminPartesDiarios);
            GrupoPermisos grpAdminSistema = new GrupoPermisos("Administración del sistema", lstAdminSistema);

            // Vale de Materiales.
            SortedList<string, PermisosPersona> lstValeMateriales = new SortedList<string, PermisosPersona>();
            lstValeMateriales.Add("Recibir responsable", PermisosPersona.ValeMaterialesRecibeResp);
            lstValeMateriales.Add("Aprobar responsable", PermisosPersona.ValeMaterialesApruebaResp);
            lstValeMateriales.Add("Recibir depósito", PermisosPersona.ValeMaterialesRecibeDep);
            lstValeMateriales.Add("Entregar", PermisosPersona.ValeMaterialesEntrega);
            lstValeMateriales.Add("Ver", PermisosPersona.ValeMaterialesVer);
            lstValeMateriales.Add("Informe", PermisosPersona.ValeMaterialesInforme);
            GrupoPermisos grpValeMateriales = new GrupoPermisos("Vale de materiales", lstValeMateriales);

            // Solicitud de Envío de Materiales.
            SortedList<string, PermisosPersona> lstSolEnvMat = new SortedList<string, PermisosPersona>();
            lstSolEnvMat.Add("Ver", PermisosPersona.SolEnvMatVer);
            lstSolEnvMat.Add("Guardar", PermisosPersona.SolEnvMatGuardar);
            lstSolEnvMat.Add("Confirmar", PermisosPersona.SolEnvMatConfirmar);
            lstSolEnvMat.Add("Cerrar", PermisosPersona.SolEnvMatCerrar);
            GrupoPermisos grpSolEnvMat = new GrupoPermisos("Solicitud de envío de materiales", lstSolEnvMat);

            // Nota de No Conformidad.
            SortedList<string, PermisosPersona> lstSgi = new SortedList<string, PermisosPersona>();
            lstSgi.Add("Responsable", PermisosPersona.SGI_Responsable);
            GrupoPermisos grpSgi = new GrupoPermisos("SGI", lstSgi);

            // Nota de No Conformidad.
            SortedList<string, PermisosPersona> lstNNC = new SortedList<string, PermisosPersona>();
            lstNNC.Add("Administrador", PermisosPersona.NNCAdministrador);
            lstNNC.Add("Ver", PermisosPersona.NNCVer);
            lstNNC.Add("Editar", PermisosPersona.NNCEditar);
            lstNNC.Add("Responsable", PermisosPersona.NNCResponsable);
            GrupoPermisos grpNNC = new GrupoPermisos("Nota de no conformidad", lstNNC);

            // Solicitud de Viajes.
            SortedList<string, PermisosPersona> lstSolViajes = new SortedList<string, PermisosPersona>();
            lstSolViajes.Add("Ver", PermisosPersona.SolViajeVer);
            lstSolViajes.Add("Editar", PermisosPersona.SolViajeEditar);
            GrupoPermisos grpSolViajes = new GrupoPermisos("Solicitud de viajes", lstSolViajes);

            // Repositorio de archivos.
            SortedList<string, PermisosPersona> lstRDA = new SortedList<string, PermisosPersona>();
            lstRDA.Add("Ver", PermisosPersona.RDAVer);
            lstRDA.Add("Crear", PermisosPersona.RDACrear);
            GrupoPermisos grpRDA = new GrupoPermisos("Repositorio de archivos", lstRDA);

            // Stock.
            SortedList<string, PermisosPersona> lstStock = new SortedList<string, PermisosPersona>();
            lstStock.Add("Cotizador on-line", PermisosPersona.CotizadorOnLine);
            lstStock.Add("Cotizador on-line de equipos", PermisosPersona.CotizadorOnLineEquipos);
            lstStock.Add("Control de herramientas", PermisosPersona.ControlHerramientas);
            lstStock.Add("Responsable de herramientas", PermisosPersona.HerramientaAdministrador);
            lstStock.Add("Instrumentos: responsable", PermisosPersona.InstrumentoSeguimiento);
            lstStock.Add("Instrumentos: recibe notificación de cambios", PermisosPersona.InstrumentoNotifCambio);
            lstStock.Add("Visualización de stock", PermisosPersona.StockVer);
            lstStock.Add("Ingreso de stock", PermisosPersona.StockIngreso);
            lstStock.Add("Egreso de stock", PermisosPersona.StockEgreso);
            lstStock.Add("Responsable de alta de artículos", PermisosPersona.SAAResponsable);
            GrupoPermisos grpStock = new GrupoPermisos("Stock", lstStock);

            // Ausencias - Licencias.
            SortedList<string, PermisosPersona> lstAL = new SortedList<string, PermisosPersona>();
            lstAL.Add("Recursos Humanos", PermisosPersona.LicRRHH);
            lstAL.Add("Carga de asistencia [entrada]", PermisosPersona.ADP_CargaEntrada);
            lstAL.Add("Carga de asistencia [salida]", PermisosPersona.ADP_CargaSalida);
            lstAL.Add("Panel de control de asistencia", PermisosPersona.ADP_PanelControlAsistencia);
            lstAL.Add("Panel de control de asistencia (Responsable)", PermisosPersona.ADP_PcResponsable);
            GrupoPermisos grpAL = new GrupoPermisos("Administración de personal", lstAL);

            // Paneles de control.
            SortedList<string, PermisosPersona> lstPC = new SortedList<string, PermisosPersona>();
            lstPC.Add("Ver panel de control general", PermisosPersona.PCVerGeneral);
            GrupoPermisos grpPC = new GrupoPermisos("Paneles de control", lstPC);

            // Información interna de obra.
            SortedList<string, PermisosPersona> lstIIO = new SortedList<string, PermisosPersona>();
            lstIIO.Add("Generar", PermisosPersona.IIOGenerar);
            lstIIO.Add("Ver", PermisosPersona.IIOVer);
            GrupoPermisos grpIIO = new GrupoPermisos("Información interna de obra", lstIIO);

            // Roles.
            SortedList<string, PermisosPersona> lstRoles = new SortedList<string, PermisosPersona>();
            lstRoles.Add("Dirección", PermisosPersona.RolDireccion);
            lstRoles.Add("Gerencia", PermisosPersona.RolGerencia);
            GrupoPermisos grpRoles = new GrupoPermisos("Roles", lstRoles);

            // SSM.
            SortedList<string, PermisosPersona> lstSSM = new SortedList<string, PermisosPersona>();
            lstSSM.Add("Administrador", PermisosPersona.SSM_Admin);
            GrupoPermisos grpSSM = new GrupoPermisos("Sistema de Seguimiento Multisitio", lstSSM);

            // Vehículos.
            SortedList<string, PermisosPersona> lstVehiculos = new SortedList<string, PermisosPersona>();
            lstVehiculos.Add("Administrador", PermisosPersona.VehicAdmin);
            lstVehiculos.Add("Ver", PermisosPersona.VehicVer);
            lstVehiculos.Add("Recibir alertas", PermisosPersona.VehicAlerta);
            GrupoPermisos grpVehiculos = new GrupoPermisos("Administración de vehículos", lstVehiculos);

            // Autorizaciones.
            SortedList<string, PermisosPersona> lstAutorizaciones = new SortedList<string, PermisosPersona>();
            lstAutorizaciones.Add("Administrar", PermisosPersona.AutorizAdministrar);
            GrupoPermisos grpAutorizaciones = new GrupoPermisos("Autorizaciones", lstAutorizaciones);

            // Sistema de Notificación de Ventas.
            SortedList<string, PermisosPersona> lstSNV = new SortedList<string, PermisosPersona>();
            lstSNV.Add("Visualización", PermisosPersona.SNV_Visualizacion);
            lstSNV.Add("Vendedor", PermisosPersona.SNV_Vendedor);
            lstSNV.Add("Alta de factura / remito", PermisosPersona.SNV_AltaFacRem);
            lstSNV.Add("Alta de clientes", PermisosPersona.SNV_AltaCliente);
            lstSNV.Add("Alta de imputaciones", PermisosPersona.SNV_AltaImputacion);
            lstSNV.Add("Alta de transporte", PermisosPersona.SNV_AltaTransporte);
            lstSNV.Add("Notificación de alta de OC", PermisosPersona.SNV_NotifOC);
            lstSNV.Add("Notificación de cierre", PermisosPersona.SNV_NotifCierre);
            lstSNV.Add("Notificación de recordatorio", PermisosPersona.SNV_NotifRecordatorio);
            lstSNV.Add("Notificación de RMA", PermisosPersona.SNV_NotifRMA);
            lstSNV.Add("Notificación de Producto", PermisosPersona.SNV_NotifProducto);
            GrupoPermisos grpSNV = new GrupoPermisos("Sistema de notificación de ventas", lstSNV);

            // General.
            SortedList<string, PermisosPersona> lstGeneral = new SortedList<string, PermisosPersona>();
            lstGeneral.Add("Carga parte diario", PermisosPersona.GEN_CargaParteDiario);
            GrupoPermisos grpGeneral = new GrupoPermisos("General", lstGeneral);

            result.Add(grpAdminSistema);
            result.Add(grpNNC);
            result.Add(grpSolEnvMat);        
            result.Add(grpSolViajes);
            result.Add(grpRDA);
            result.Add(grpStock);
            result.Add(grpAL);
            result.Add(grpPC);
            result.Add(grpIIO);
            result.Add(grpValeMateriales);
            result.Add(grpRoles);
            result.Add(grpSSM);
            result.Add(grpVehiculos);
            result.Add(grpAutorizaciones);
            result.Add(grpSNV);
            result.Add(grpGeneral);
            result.Add(grpSgi);

            result.Sort();

            return result;
        }

        public static void Update(int id, List<PermisosPersona> permisos)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = conn.BeginTransaction();

                // Borro los permisos existentes.
                IDbCommand cmd = DataAccess.GetCommand(conn, trans);
                cmd.CommandText = "DELETE FROM PersonasPermisos WHERE PersonaId = @PersonaId";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", id));
                cmd.ExecuteNonQuery();

                // Inserto los permisos.
                foreach (PermisosPersona permiso in permisos)
                {
                    cmd = DataAccess.GetCommand(conn, trans);
                    cmd.CommandText = "INSERT INTO PersonasPermisos (PersonaId, PermisoId) VALUES ";
                    cmd.CommandText += "(@PersonaId, @PermisoId)";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", id));
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@PermisoId", (int)permiso));
                    cmd.ExecuteNonQuery();
                }

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

        public static string ListEmails(PermisosPersona p)
        {
            string result = String.Empty;
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT distinct p.Email FROM Personas p ";
                cmd.CommandText += "INNER JOIN PersonasPermisos pe ON pe.PersonaId = p.Id ";
                cmd.CommandText += "WHERE pe.PermisoId = @PermisoId AND p.Activo = @Activo;";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@PermisoId", (int)p));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result += dr["Email"] + ",";
                }
                result = result.TrimEnd(',');

                dr.Close();
            }
            catch(Exception e)
            {
                result = String.Empty;
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static List<DataSourceItem> ListPersonas(PermisosPersona p)
        {
            List<DataSourceItem> result = new List<DataSourceItem>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT p.Id, p.Nombre FROM Personas p ";
                cmd.CommandText += "INNER JOIN PersonasPermisos pe ON pe.PersonaId = p.Id ";
                cmd.CommandText += "WHERE pe.PermisoId = @PermisoId AND p.Activo = @Activo ";
                cmd.CommandText += "ORDER BY p.Nombre";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@PermisoId", (int)p));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new DataSourceItem(dr["Id"], Funciones.ToTitleCase(dr["Nombre"].ToString())));
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

        
    }
}