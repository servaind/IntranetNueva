using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using Proser.Common;
using Proser.Common.Data;
using Proser.Common.Extensions;
using Proser.Products.Monitoring;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core
{
    public enum FormFg005Categoria
    {
        [Description("No Conformidad")]
        NoConformidad = 0,
        [Description("Observación")]
        Observacion,
        [Description("Oportunidad de Mejora")]
        OportunidadMejora,
        [Description("No Corresponde")]
        NoCorresponde
    }

    public enum FormFg005Origen
    {
        [Description("Reclamo / Queja de Cliente Externo")]
        ReclamoExterno = 0,
        [Description("Reclamo / Queja de Cliente Interno")]
        ReclamoInterno,
        [Description("Auditoría Externa")]
        AuditoriaExterna,
        [Description("Auditoría Interna")]
        AuditoriaInterna,
        [Description("Plan de Gestión y Planes Derivados")]
        PlanGestionPlanes,
        [Description("Reporte de Accidentes / Incidentes")]
        ReporteAccidentes,
        [Description("Reportes Ambientales")]
        ReportesAmbientales,
        [Description("Requisitos Legales")]
        RequisitosLegales,
        [Description("Monitoreo de Procesos")]
        MonitoreoProcesos,
        [Description("Producto No Conforme")]
        ProductoNoConforme,
        [Description("Oportunidad de Mejora")]
        OportunidadMejora,
        [Description("Rechazo a Proveedores")]
        RechazoProveedores
    }

    public enum FormFg005Cierre
    {
        [Description("Cierre eficaz")]
        Eficaz = 0,
        [Description("Cierre no eficaz")]
        NoEficaz
    }

    public enum FormFg005Estado
    {
        [Description("Solicitud")]
        Solicitud = 1,
        [Description("Procesando SGI")]
        ProcesandoSgi,
        [Description("Procesando Responsable")]
        ProcesandoResponsable,
        [Description("Validando SGI")]
        ValidandoSgi,
        [Description("Esperando Cierre")]
        EvaluacionAcciones,
        [Description("Cerrada")]
        Cerrada
    }

    public enum FormFg005Filtro
    {
        Numero = 0,
        Categoria,
        Asunto,
        Estado,
        FechaAltaDesde,
        FechaAltaHasta,
        FechaCierreDesde,
        FechaCierreHasta,
        AreaResponsabilidad,
        DiasTratamiento,
        Origen
    }

    public class FormFg005Historico
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string CausasRaises { get; set; }
        public string AccCorr { get; set; }
        public int AccCorrRespId { get; set; }
        public DateTime AccCorrFin { get; set; }
        public string AccPrev { get; set; }
        public int AccPrevRespId { get; set; }
        public DateTime AccPrevFin { get; set; }
    }

    public class FormFg005Summary : ICsvHeader, ICsvColumns, ICsvContent
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string NumeroStr { get; set; }
        public string Categoria { get; set; }
        public string Asunto { get; set; }
        public string AreaResponsabilidad { get; set; }
        public string Fecha { get; set; }
        public string FechaCierre { get; set; }
        public int Estado { get; set; }
        public string EstadoStr { get; set; }
        public bool Cerrada { get; set; }



        public List<string> GetCsvHeader()
        {
            return new List<string>
            {
                "Sistema de NC"
            };
        }

        public List<string> GetCsvColumns()
        {
            return new List<string>
            {
                "Número",
                "Categoría",
                "Asunto",
                "Área de Responsabilidad",
                "Generada",
                "Cierre",
                "Estado"
            };
        }

        public List<object> GetCsvRows()
        {
            return new List<object>
            {
                NumeroStr,
                Categoria,
                Asunto,
                AreaResponsabilidad,
                Fecha,
                FechaCierre,
                EstadoStr
            };
        }
    }

    public class FormFg005
    {
        class FormEmail : EmailTemplate
        {
            public FormEmail(FormFg005 f)
            {
                switch (f.Estado)
                {
                    case FormFg005Estado.ProcesandoResponsable:
                        SetTipo(EmailTemplateTipo.Error);
                        break;
                    case FormFg005Estado.Cerrada:
                        SetTipo(EmailTemplateTipo.Ok);
                        break;
                }

                SetLinkAcceso(String.Format("{0}/Sgi/FormFg005/{1}", Constantes.UrlIntranet, f.Numero), "visualizar el formulario completo.");

                AddItem("Número", NumeroToString(f.Numero));
                AddItem("Categoría", f.Categoria.GetDescription());
                AddItem("Asunto", f.Asunto);
                AddItem("Emitida por", f.EmitidaPor);
                AddItem("Generada", f.Fecha.ToString("dd/MM/yyyy"));
                AddItem("Estado", f.Estado.GetDescription());
            }
        }

        class FormRecordatorioEmail : EmailTemplate
        {
            public FormRecordatorioEmail(FormFg005 f)
            {
                SetTipo(EmailTemplateTipo.Info);

                SetEncabezado("El siguiente formulario se encuentra pendiente de tratamiento");

                SetLinkAcceso(String.Format("{0}/Sgi/FormFg005/{1}", Constantes.UrlIntranet, f.Numero), "visualizar el formulario completo.");

                AddItem("Número", NumeroToString(f.Numero));
                AddItem("Categoría", f.Categoria.GetDescription());
                AddItem("Asunto", f.Asunto);
                AddItem("Emitida por", f.EmitidaPor);
                AddItem("Generada", f.Fecha.ToString("dd/MM/yyyy"));
                AddItem("Estado", f.Estado.GetDescription());
            }
        }


        class FormCargaNCEmail : EmailTemplate
        {
            public FormCargaNCEmail(FormFg005 f)
            {
                SetTipo(EmailTemplateTipo.Info);

                SetEncabezado("Se cargó el siguiente formulario");

                SetLinkAcceso(String.Format("{0}/Sgi/FormFg005/{1}", Constantes.UrlIntranet, f.Numero), "visualizar el formulario completo.");

                AddItem("Número", NumeroToString(f.Numero));
                AddItem("Categoría", f.Categoria.GetDescription());
                AddItem("Asunto", f.Asunto);
                AddItem("Emitida por", f.EmitidaPor);
                AddItem("Generada", f.Fecha.ToString("dd/MM/yyyy"));
                AddItem("Estado", f.Estado.GetDescription());
            }
        }

        // Constantes.
        private const string PATH_FORMS = @"\\" + Constantes.SERVER_STORAGE + "\\Intranet\\Calidad\\FormFg005\\";
        private const string PATH_FORM = PATH_FORMS + "{0}\\";
        private const string PATH_EV_RESULTADOS = PATH_FORM + "Evidencia de resultados\\";
        private const string PATH_EV_REVISION = PATH_FORM + "Evidencia de revision\\";
        private const string PATH_EV = PATH_FORM + "Evidencia\\";
        private const int MAX_REGS_PAGINA = 10;
        private const int MAX_DIAS_TRATAMIENTO_1 = 30;
        private const int MAX_DIAS_TRATAMIENTO_2 = 60;

        // Propiedades.
        public int Id { get; set; }
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public bool NorIso9001 { get; set; }
        public bool NorIso14001 { get; set; }
        public bool NorOhsas18001 { get; set; }
        public bool NorIram301 { get; set; }
        public string ApaIso9001 { get; set; }
        public string ApaIso14001 { get; set; }
        public string ApaOhsas18001 { get; set; }
        public string ApaIram301 { get; set; }
        public int AreaResponsabilidadId { get; set; }
        public string AreaResponsabilidad { get; set; }
        public FormFg005Categoria Categoria { get; set; }
        public int EmitidaPorId { get; set; }
        public string EmitidaPor { get; set; }
        public FormFg005Origen Origen { get; set; }
        public string Asunto { get; set; }
        public string Hallazgo { get; set; }
        public string AccInmediata { get; set; }
        public FormFg005Historico DatosResponsable { get; set; }
        public string EvResultados { get; set; }
        public string EvRevision { get; set; }
        public FormFg005Cierre Cierre { get; set; }
        public string Comentarios { get; set; }
        public int CierrePersonaId { get; set; }
        public string CierrePersona { get; set; }
        public DateTime CierreFecha { get; set; }
        public FormFg005Estado Estado { get; set; }
        public List<FileSummary> ArchivosEvidencia { get; set; }


        public static FormFg005 Read(int numero)
        {
            FormFg005 result;

            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT f.*, fh.CausasRaices, fh.AccCorr, fh.AccCorrRespId, fh.AccCorrFechaFin, ";
                cmd.CommandText += "fh.AccPrev, fh.AccPrevRespId, fh.AccPrevFechaFin, fh.Fecha AS FechaHistorial, ";
                cmd.CommandText += "p.Nombre AS EmitidaPor, a.Nombre AS AreaResponsabilidad, pc.Nombre AS CierrePersona ";
                cmd.CommandText += "FROM FormsFg005 f ";
                cmd.CommandText += "LEFT JOIN FormsFg005Historial fh ON f.HistorialId = fh.Id ";
                cmd.CommandText += "INNER JOIN Areas a ON f.AreaResponsabilidadId = a.Id ";
                cmd.CommandText += "INNER JOIN Personas p ON f.EmitidaPorId = p.Id ";
                cmd.CommandText += "INNER JOIN Personas pc ON f.CierrePersonaId = pc.Id ";
                cmd.CommandText += "WHERE f.Numero = @Numero";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                dr = cmd.ExecuteReader();

                dr.Read();

                result = new FormFg005
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Numero = numero,
                    Fecha = Convert.ToDateTime(dr["Fecha"]),
                    NorIso9001 = Convert.ToBoolean(dr["NorIso9001"]),
                    NorIso14001 = Convert.ToBoolean(dr["NorIso14001"]),
                    NorOhsas18001 = Convert.ToBoolean(dr["NorOhsas18001"]),
                    NorIram301 = Convert.ToBoolean(dr["NorIram301"]),
                    ApaIso9001 = dr["ApaIso9001"].ToString(),
                    ApaIso14001 = dr["ApaIso14001"].ToString(),
                    ApaOhsas18001 = dr["ApaOhsas18001"].ToString(),
                    ApaIram301 = dr["ApaIram301"].ToString(),
                    AreaResponsabilidadId = Convert.ToInt32(dr["AreaResponsabilidadId"]),
                    AreaResponsabilidad = dr["AreaResponsabilidad"].ToString(),
                    Categoria = (FormFg005Categoria)Convert.ToInt32(dr["CategoriaId"]),
                    EmitidaPorId = Convert.ToInt32(dr["EmitidaPorId"]),
                    EmitidaPor = dr["EmitidaPor"].ToString().ToTitleCase(),
                    Origen = (FormFg005Origen)Convert.ToInt32(dr["OrigenId"]),
                    Hallazgo = dr["Hallazgo"].ToString(),
                    AccInmediata = dr["AccionInmediata"].ToString(),
                    EvResultados = dr["EvidenciaResultados"].ToString(),
                    EvRevision = dr["EvidenciaRevision"].ToString(),
                    Cierre = (FormFg005Cierre)Convert.ToInt32(dr["CierreId"]),
                    Comentarios = dr["Comentarios"].ToString(),
                    CierrePersonaId = Convert.ToInt32(dr["CierrePersonaId"]),
                    CierrePersona = dr["CierrePersona"].ToString().ToTitleCase(),
                    CierreFecha = Convert.ToDateTime(dr["CierreFecha"]),
                    Estado = (FormFg005Estado)Convert.ToInt32(dr["EstadoId"]),
                    Asunto = dr["Asunto"].ToString()
                };

                if (dr["HistorialId"] != DBNull.Value)
                {
                    result.DatosResponsable = new FormFg005Historico
                    {
                        Fecha = Convert.ToDateTime(dr["FechaHistorial"]),
                        CausasRaises = dr["CausasRaices"].ToString(),
                        AccCorr = dr["AccCorr"].ToString(),
                        AccCorrRespId = Convert.ToInt32(dr["AccCorrRespId"]),
                        AccCorrFin = Convert.ToDateTime(dr["AccCorrFechaFin"]),
                        AccPrev = dr["AccPrev"].ToString(),
                        AccPrevRespId = Convert.ToInt32(dr["AccPrevRespId"]),
                        AccPrevFin = Convert.ToDateTime(dr["AccPrevFechaFin"])
                    };
                }
                
                dr.Close();

                if (result.Estado == FormFg005Estado.Cerrada) result.LoadFiles();
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

        // Paso 1.
        public static int Create(int personaId, FormFg005Origen origen, string asunto, string hallazgo, string accInmediata,
            string comentarios)
        {
            if (String.IsNullOrWhiteSpace(asunto)) throw new Exception("No se ha ingresado el Asunto.");
            if (String.IsNullOrWhiteSpace(hallazgo)) throw new Exception("No se ha ingresado el Hallazgo.");
            if (String.IsNullOrWhiteSpace(accInmediata)) throw new Exception("No se ha ingresado la Accion Inmediata.");

            int result;
            IDbConnection conn = null;
            IDbTransaction trans = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                // Obtengo un número para el formulario.
                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "SELECT TOP 1 Numero FROM FormsFg005 ORDER BY Numero DESC";
                result = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

                // Inserto el formulario.
                cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "INSERT INTO FormsFg005 (Numero, EmitidaPorId, OrigenId, Asunto, Hallazgo, ";
                cmd.CommandText += "AccionInmediata, Comentarios, EstadoId) VALUES (@Numero, @EmitidaPorId, ";
                cmd.CommandText += "@OrigenId, @Asunto, @Hallazgo, @AccionInmediata, @Comentarios, @EstadoId); ";
                cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM FormsFg005";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", result));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EmitidaPorId", personaId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@OrigenId", (int)origen));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Asunto", asunto));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Hallazgo", hallazgo.ReplaceEnters()));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccionInmediata", accInmediata.ReplaceEnters()));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Comentarios", comentarios));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoId", (int)FormFg005Estado.ProcesandoSgi));
                int formId = Convert.ToInt32(cmd.ExecuteScalar());

                // Creo los directorios para el formulario.
                /*Directory.CreateDirectory(String.Format(PATH_EV_REVISION, result));
                Directory.CreateDirectory(String.Format(PATH_EV_RESULTADOS, result));*/
                Directory.CreateDirectory(String.Format(PATH_EV, result));

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            // Enviar el formulario a SGI.
            Send(result);

            return result;
        }

        // Paso 2.
        public static void UpdateSgi(int numero, bool norIso9001, bool norIso14001, bool norOhsas18001, bool norIram301, 
            string apaIso9001, string apaIso14001, string apaOhsas18001, string apaIram301, int areaResponsabilidadId,
            FormFg005Categoria categoria)
        {
            IDbConnection conn = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                
                // Actualizo el formulario.
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "UPDATE FormsFg005 SET NorIso9001 = @NorIso9001, NorIso14001 = @NorIso14001, ";
                cmd.CommandText += "NorOhsas18001 = @NorOhsas18001, NorIram301 = @NorIram301, ApaIso9001 = @ApaIso9001, ";
                cmd.CommandText += "ApaIso14001 = @ApaIso14001, ApaOhsas18001 = @ApaOhsas18001, ApaIram301 = @ApaIram301, ";
                cmd.CommandText += "AreaResponsabilidadId = @AreaResponsabilidadId, CategoriaId = @CategoriaId, ";
                cmd.CommandText += "EstadoId = @EstadoId WHERE Numero = @Numero";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@NorIso9001", norIso9001));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@NorIso14001", norIso14001));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@NorOhsas18001", norOhsas18001));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@NorIram301", norIram301));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ApaIso9001", apaIso9001));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ApaIso14001", apaIso14001));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ApaOhsas18001", apaOhsas18001));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@ApaIram301", apaIram301));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AreaResponsabilidadId", areaResponsabilidadId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@CategoriaId", (int)categoria));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoId", (int)FormFg005Estado.ProcesandoResponsable));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            // Enviar el formulario a los responsables de área.
            Send(numero);
        }

        // Paso 3.
        public static void UpdateResponsable(int numero, string causasRaices, string accCorr, int accCorrRespId, 
            DateTime accCorrFechaFin, string accPrev, int accPrevRespId, DateTime accPrevFechaFin)
        {
            var form = Read(numero);

            if (String.IsNullOrWhiteSpace(causasRaices)) throw new Exception("No se ha ingresado las Causas Raices.");
            if (!String.IsNullOrWhiteSpace(accCorr))
            {
                if (accCorrRespId == Persona.ID_INVALIDO) throw new Exception("No se ha seleccionado un Responsable para las Acciones Correctivas.");
                if (accCorrFechaFin == Constants.InvalidDateTime) throw new Exception("La Fecha de finalizacion de las Acciones Correctivas no es valida.");
            }
            if (form.Categoria == FormFg005Categoria.OportunidadMejora)
            {
                if (String.IsNullOrWhiteSpace(accPrev)) throw new Exception("No se ha ingresado las Acciones Preventivas.");
                if (accPrevRespId == Persona.ID_INVALIDO) throw new Exception("No se ha seleccionado un Responsable para las Acciones Preventivas.");
                if (accPrevFechaFin == Constants.InvalidDateTime) throw new Exception("La Fecha de finalizacion de las Acciones Preventivas no es valida.");
            }

            IDbConnection conn = null;
            IDbTransaction trans = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                // Inserto el Registro.
                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "INSERT INTO FormsFg005Historial (FormId, CausasRaices, ";
                cmd.CommandText += "AccCorr, AccCorrRespId, AccCorrFechaFin, AccPrev, AccPrevRespId, AccPrevFechaFin) VALUES ";
                cmd.CommandText += "(@FormId, @CausasRaices, @AccCorr, @AccCorrRespId, @AccCorrFechaFin, @AccPrev, ";
                cmd.CommandText += "@AccPrevRespId, @AccPrevFechaFin); ";
                cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM FormsFg005Historial";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@FormId", form.Id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@CausasRaices", causasRaices.ReplaceEnters()));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccCorr", accCorr.ReplaceEnters()));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccCorrRespId", accCorrRespId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccCorrFechaFin", accCorrFechaFin));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccPrev", accPrev.ReplaceEnters()));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccPrevRespId", accPrevRespId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AccPrevFechaFin", accPrevFechaFin));
                int historialId = Convert.ToInt32(cmd.ExecuteScalar());

                // Actualizo el formulario.
                cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "UPDATE FormsFg005 SET HistorialId = @HistorialId, EstadoId = @EstadoId ";
                cmd.CommandText += "WHERE Id = @Id";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", form.Id));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HistorialId", historialId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoId", (int)FormFg005Estado.ValidandoSgi));
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            // Enviar el formulario a SGI.
            Send(numero);
        }

        // Paso 4.
        public static void UpdateValidandoSgi(int numero, bool aceptar)
        {
            IDbConnection conn = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);

                // Actualizo el formulario.
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "UPDATE FormsFg005 SET EstadoId = @EstadoId WHERE Numero = @Numero";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoId",
                    (int)(aceptar ? FormFg005Estado.EvaluacionAcciones : FormFg005Estado.ProcesandoResponsable)));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            // Enviar el formulario a según corresponda.
            if (!aceptar) Send(numero);
        }

        // Paso 5.
        public static void Close(int numero, string evResultados, string evRevision, FormFg005Cierre cierre, int personaId)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                // Actualizo el formulario.
                IDbCommand cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "UPDATE FormsFg005 SET CierreId = @CierreId, CierrePersonaId = @CierrePersonaId, ";
                cmd.CommandText += "CierreFecha = @CierreFecha, EstadoId = @EstadoId, ";
                cmd.CommandText += "EvidenciaResultados =  @EvidenciaResultados, EvidenciaRevision =  @EvidenciaRevision ";
                cmd.CommandText += "WHERE Numero = @Numero";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@CierrePersonaId", personaId));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@CierreFecha", DateTime.Now));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@CierreId", (int)cierre));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EvidenciaResultados", evResultados));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EvidenciaRevision", evRevision));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoId", (int)(cierre == FormFg005Cierre.Eficaz ? FormFg005Estado.Cerrada : FormFg005Estado.ProcesandoResponsable)));
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            // Enviar el formulario al solicitante y a SGI.
            Send(numero);
        }

        public static void CloseUploadFiles(int numero, List<FileAttachment> files)
        {
            try
            {
                // Copio los archivos.
                files.ForEach(f => CloseUploadFile(numero, f));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        public static void CloseUploadFile(int numero, FileAttachment file)
        {
            try
            {
                // Copio los archivos.
                FileHelper.Save(file, String.Format(PATH_EV, numero));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public static string NumeroToString(int numero)
        {
            return string.Format("0001-{0:000000}", numero);
        }

        private static FormFg005Summary ToFormFg005Summary(DataRow dr)
        {
            FormFg005Estado estado = (FormFg005Estado)Convert.ToInt32(dr["EstadoId"]);
            string fechaCierre = estado == FormFg005Estado.Cerrada
                ? Convert.ToDateTime(dr["CierreFecha"]).ToString("dd/MM/yyyy")
                : "-";

            return new FormFg005Summary
            {
                Id = Convert.ToInt32(dr["Id"]),
                Numero = Convert.ToInt32(dr["Numero"]),
                NumeroStr = NumeroToString(Convert.ToInt32(dr["Numero"])),
                Categoria = ((FormFg005Categoria)Convert.ToInt32(dr["CategoriaId"])).GetDescription(),
                Asunto = dr["Asunto"].ToString(),
                AreaResponsabilidad = dr["AreaResponsabilidad"].ToString(),
                Fecha = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy"),
                Estado = (int)estado,
                EstadoStr = estado.GetDescription(),
                FechaCierre = fechaCierre,
                Cerrada = estado == FormFg005Estado.Cerrada
            };
        }

        private static FormFg005Summary ToFormFg005Summary(IDataReader dr)
        {
            FormFg005Estado estado = (FormFg005Estado)Convert.ToInt32(dr["EstadoId"]);
            string fechaCierre = estado == FormFg005Estado.Cerrada
                ? Convert.ToDateTime(dr["CierreFecha"]).ToString("dd/MM/yyyy")
                : "-";

            return new FormFg005Summary
            {
                Id = Convert.ToInt32(dr["Id"]),
                Numero = Convert.ToInt32(dr["Numero"]),
                NumeroStr = NumeroToString(Convert.ToInt32(dr["Numero"])),
                Categoria = ((FormFg005Categoria)Convert.ToInt32(dr["CategoriaId"])).GetDescription(),
                Asunto = dr["Asunto"].ToString(),
                AreaResponsabilidad = dr["AreaResponsabilidad"].ToString(),
                Fecha = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy"),
                Estado = (int)estado,
                EstadoStr = estado.GetDescription(),
                FechaCierre = fechaCierre,
                Cerrada = estado == FormFg005Estado.Cerrada
            };
        }

        private void LoadFiles()
        {
            string path = String.Format(PATH_EV, Numero);

            ArchivosEvidencia = FileHelper.ReadPath(path);
        }

        public static string ReadFile(int numero, string nombre)
        {
            return String.Format(PATH_EV, numero) + nombre;
        }

        public static string ExportToCsv(List<Filtro> filtros)
        {
            var filas = List(filtros);
            var c = filas.Count;

            var csv = c > 0 ? new CsvHelper(filas[0]) : new CsvHelper(new FormFg005Summary());
            csv.SetColumns(c > 0 ? filas[0] : new FormFg005Summary());
            filas.ForEach(csv.AppendRow);

            Random r = new Random(DateTime.Now.Millisecond);
            var path = Funciones.GetTempPath() + "archivo" + r.Next() + ".csv";

            try
            {
                File.WriteAllText(path, csv.MakeCsv(), Encoding.UTF8);
            }
            catch
            {

            }

            return path;
        }

        private static List<FormFg005Summary> List(List<Filtro> filtros)
        {
            var result = new List<FormFg005Summary>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = ListQuery(filtros, false);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    try
                    {
                        var i = ToFormFg005Summary(dr);
                        result.Add(i);
                    }
                    catch
                    {
                        
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

        private static void Send(int numero)
        {
            var f = Read(numero);
            if (f == null) return;

            var solicito = Persona.Read(f.EmitidaPorId);

            var asunto = String.Format("Sistema NC - Nº{0} - {1}", NumeroToString(f.Numero), f.Estado.GetDescription());
            var destinatarios = "";
            var cc = "";

            switch (f.Estado)
            {
                case FormFg005Estado.ProcesandoSgi:
                    destinatarios = PermisoPersona.ListEmails(PermisosPersona.SGI_Responsable);
                    cc = solicito.Email;
                    break;
                case FormFg005Estado.ProcesandoResponsable:
                    var area = AreaPersonal.Read(f.AreaResponsabilidadId);
                    if (area == null) return;
                    area.LoadResponsables();

                    asunto += " - " + (f.DatosResponsable != null ? "[Rechazada]" : "");
                    destinatarios = area.ListEmailsResponsables();
                    break;
                case FormFg005Estado.ValidandoSgi:
                    destinatarios = PermisoPersona.ListEmails(PermisosPersona.SGI_Responsable);
                    break;
                case FormFg005Estado.EvaluacionAcciones:
                    return;
                    break;
                case FormFg005Estado.Cerrada:
                    destinatarios = PermisoPersona.ListEmails(PermisosPersona.SGI_Responsable);
                    cc = solicito.Email;
                    break;
            }

            try
            {
                var email = new FormEmail(f);
                email.SendFromIntranet(destinatarios, cc, asunto);
            }
            catch
            {
                
            }
        }

        public static void SendPendientes()
        {
            var filtros = new List<Filtro>
            {
                new Filtro((int)FormFg005Filtro.Estado, (int)FormFg005Estado.ProcesandoResponsable),
                new Filtro((int)FormFg005Filtro.DiasTratamiento, MAX_DIAS_TRATAMIENTO_1),
                new Filtro((int)FormFg005Filtro.DiasTratamiento, MAX_DIAS_TRATAMIENTO_2)
            };

            var forms = List(filtros);
            forms.ForEach(f => SendPendiente(f.Numero));
        }

        private static void SendPendiente(int numero)
        {
            var f = Read(numero);
            if (f == null) return;

            var asunto = String.Format("Sistema NC - Nº{0} - {1} - [Pendiente]", NumeroToString(f.Numero), f.Estado.GetDescription());
            
            var area = AreaPersonal.Read(f.AreaResponsabilidadId);
            if (area == null) return;
            area.LoadResponsables();

            var destinatarios = area.ListEmailsResponsables();
            var cc = PermisoPersona.ListEmails(PermisosPersona.SGI_Responsable);

            try
            {
                var email = new FormRecordatorioEmail(f);
                email.SendFromIntranet(destinatarios, cc, asunto);
            }
            catch
            {

            }
        }

        private static void SendCargaNuevoNC(int numero)
        {
            var f = Read(numero);
            if (f == null) return;

            var asunto = String.Format("Sistema NC - Nº{0} - {1} - [Nuevo]", NumeroToString(f.Numero), f.Estado.GetDescription());

            var area = AreaPersonal.Read(f.AreaResponsabilidadId);
            if (area == null) return;
            area.LoadResponsables();

            var destinatarios = area.ListEmailsResponsables();
            var cc = "";// PermisoPersona.ListEmails(PermisosPersona.SGI_Responsable);

            try
            {
                var email = new FormRecordatorioEmail(f);
                email.SendFromIntranet(destinatarios, cc, asunto);
            }
            catch
            {

            }
        }


        #region Pager

        internal static List<FormFg005Summary> List(int pagina, List<Filtro> filtros)
        {
            var result = DataAccess.GetDataList(BDConexiones.Intranet, pagina, filtros, MAX_REGS_PAGINA,
                ListQuery, ToFormFg005Summary);

            return result;
        }

        internal static int ListPaginas(List<Filtro> filtros)
        {
            return DataAccess.GetCantidadPaginasData(filtros, MAX_REGS_PAGINA, ListQuery);
        }

        private static string ListQuery(List<Filtro> filtros, bool cantidad)
        {
            var filtroWhere = "";
            var filtroJoin = "";
            var consulta = "";

            foreach (var filtro in filtros)
            {
                switch ((FormFg005Filtro)filtro.Tipo)
                {
                    case FormFg005Filtro.Numero:
                        filtroWhere += "AND f.Numero = " + filtro.Valor + " ";
                        break;
                    case FormFg005Filtro.Origen:
                        filtroWhere += "AND f.OrigenId = " + filtro.Valor + " ";
                        break;
                    case FormFg005Filtro.Categoria:
                        filtroWhere += "AND f.CategoriaId = " + filtro.Valor + " AND f.EstadoId > " + (int)FormFg005Estado.ProcesandoSgi + " ";
                        break;
                    case FormFg005Filtro.AreaResponsabilidad:
                        filtroWhere += "AND f.AreaResponsabilidadId = " + filtro.Valor + " AND f.EstadoId > " + (int)FormFg005Estado.ProcesandoSgi + " ";
                        break;
                    case FormFg005Filtro.Asunto:
                        filtroWhere += "AND f.Asunto LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FormFg005Filtro.Estado:
                        filtroWhere += "AND f.EstadoId = " + filtro.Valor + " ";
                        break;
                    case FormFg005Filtro.FechaAltaDesde:
                        filtroWhere += "AND f.Fecha >= \'" + filtro.Valor + "\' ";
                        break;
                    case FormFg005Filtro.FechaAltaHasta:
                        filtroWhere += "AND f.Fecha <= \'" + filtro.Valor + "\' ";
                        break;
                    case FormFg005Filtro.FechaCierreDesde:
                        filtroWhere += "AND f.CierreFecha >= \'" + filtro.Valor + "\' AND f.EstadoId = " + (int)FormFg005Estado.Cerrada + " ";
                        break;
                    case FormFg005Filtro.FechaCierreHasta:
                        filtroWhere += "AND f.CierreFecha <= \'" + filtro.Valor + "\' ";
                        break;
                    case FormFg005Filtro.DiasTratamiento:
                        filtroWhere += "AND DATEDIFF(day, f.Fecha, GetDate()) = " + filtro.Valor + " ";
                        break;
                    default:
                        filtroWhere += "";
                        break;
                }
            }

            filtroJoin = "INNER JOIN Areas a ON f.AreaResponsabilidadId = a.Id ";

            consulta = cantidad ? "SELECT COUNT(f.Id) as TotalRegistros" : "SELECT f.*, a.Nombre AS AreaResponsabilidad ";
            consulta += " FROM FormsFg005 f " + filtroJoin + " WHERE 1=1 " + filtroWhere;

            if (!cantidad)
            {
                consulta += " ORDER BY f.Numero DESC";
            }

            return consulta;
        }

        public static Pager<FormFg005Summary> Pager(int pagina, List<Filtro> filtros)
        {
            Pager<FormFg005Summary> result = new Pager<FormFg005Summary>();

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
