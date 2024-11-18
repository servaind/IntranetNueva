using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Proser.Common.Data;
using Proser.Common.Utils;
using Proser.Products.Monitoring;
using ExcelApp = Microsoft.Office.Interop.Excel.Application;
using System.Reflection;
using Constants = Proser.Common.Constants;

namespace Servaind.Intranet.Core
{
    public class VehiculoSummary : ICsvHeader, ICsvColumns, ICsvContent
    {
        // Propiedades.
        public int Id { get; set; }
        public string Patente { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }
        public int Anio { get; set; }
        public string Ubicacion { get; set; }
        public string Responsable { get; set; }
        public bool AfectadoGasmed { get; set; }
        public string VtoCedulaVerde { get; set; }
        public string NroRuta { get; set; }
        public string VtoRuta { get; set; }
        public string VtoVtv { get; set; }
        public string VtoStaCruz { get; set; }
        public string VtoPatente { get; set; }
        public string CiaSeguro { get; set; }
        public int PolizaSeguro { get; set; }
        public string VtoSeguro { get; set; }
        public string NroChasis { get; set; }
        public string NroMotor { get; set; }
        public string VtoCertIzaje { get; set; }

        public List<string> GetCsvHeader()
        {
            return new List<string>
            {
                "Listado de vehículos"
            };
        }

        public List<string> GetCsvColumns()
        {
            return new List<string>
            {
                "Dominio",
                "Modelo",
                "Tipo de Vehículo",
                "Año",
                "Ubicación",
                "Responsable",
                "Vto Cert. Izaje",
                "Vto. Cédula Verde",
                "Vto. R.U.T.A.",
                "Nº R.U.T.A.",
                "Vto. V.T.V.",
                "Hab Prov Santa Cruz",
                "Vto. Patente",
                "Vto. Seguro",
                "Compañía Seguro",
                "Póliza Seguro",
                "Nº de Chasis",
                "Nº de Motor"
            };
        }

        public List<object> GetCsvRows()
        {
            return new List<object>
            {
                Patente,
                Modelo,
                Tipo,
                Anio,
                Ubicacion,
                Responsable,
                Vehiculo.EsFechaNoAplica(VtoCertIzaje) ? "-" : VtoCertIzaje,
                Vehiculo.EsFechaNoAplica(VtoCedulaVerde) ? "-" : VtoCedulaVerde,
                Vehiculo.EsFechaNoAplica(VtoRuta) ? "-" : VtoRuta,
                NroRuta,
                Vehiculo.EsFechaNoAplica(VtoVtv) ? "-" : VtoVtv,
                Vehiculo.EsFechaNoAplica(VtoStaCruz) ? "-" : VtoStaCruz,
                Vehiculo.EsFechaNoAplica(VtoPatente) ? "-" : VtoPatente,
                Vehiculo.EsFechaNoAplica(VtoSeguro) ? "-" : VtoSeguro,
                CiaSeguro,
                PolizaSeguro,
                NroChasis,
                NroMotor
            };
        }
    }

    public class VencimientoVehiculo
    {
        // Propiedades.
        public string Patente { get; set; }
        public string Fecha { get; set; }
    }

    public class Vehiculo
    {
        class VencimientoEmail : EmailTemplate
        {
            public VencimientoEmail(DateTime mes, Dictionary<string, List<VencimientoVehiculo>> vencimientos)
            {
                SetTipo(EmailTemplateTipo.Info);

                SetEncabezado(String.Format("A continuaci&oacute;n se detallan los vencimientos del m&eacute;s {0}:", 
                    mes.ToString("MM/yyyy")));

                // Items.
                foreach (var t in vencimientos.Keys)
                {
                    var lst = vencimientos[t];
                    if (lst.Count == 0) continue;

                    AddItem("*" + t, "");
                    lst.ForEach(v => AddItem(v.Patente, v.Fecha));
                    AddItem("", "");
                }
            }
        }

        // Constantes.
        public const int MIN_DIAS_CEDULA = 20;
        public const int MIN_DIAS_AVISO = 15;
        public const string NO_APLICA = "N/A";


        public static List<VehiculoSummary> List(bool soloActivos = true, int ubicacionId = Constants.InvalidInt)
        {
            List<VehiculoSummary> result = new List<VehiculoSummary>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT v.*, t.Descripcion AS Tipo, p.Nombre AS Responsable, u.Nombre AS Ubicacion ";
                cmd.CommandText += "FROM Vehiculos v ";
                cmd.CommandText += "INNER JOIN VehiculosTipos t ON v.TipoId = t.Id ";
                cmd.CommandText += "INNER JOIN Personas p ON v.ResponsableId = p.Id ";
                cmd.CommandText += "INNER JOIN VehiculosUbicaciones u ON v.UbicacionId = u.Id ";
                cmd.CommandText += "WHERE 1=1 ";
                if (soloActivos)
                {
                    cmd.CommandText += "AND v.Activo = @Activo ";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
                }
                if (ubicacionId != Constants.InvalidInt)
                {
                    cmd.CommandText += "AND v.UbicacionId = @UbicacionId ";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@UbicacionId", ubicacionId));
                }
                cmd.CommandText += "ORDER BY v.Patente";
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var vehiculo = Read(dr);
                    if (vehiculo != null) result.Add(vehiculo);
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

        private static VehiculoSummary Read(IDataReader dr)
        {
            VehiculoSummary result;

            try
            {
                result = new VehiculoSummary
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Patente = dr["Patente"].ToString(),
                    Modelo = dr["Modelo"].ToString(),
                    Tipo = dr["Tipo"].ToString(),
                    Anio = Convert.ToInt32(dr["Anio"]),
                    Ubicacion = dr["Ubicacion"].ToString(),
                    Responsable = Persona.NormalizarNombre(dr["Responsable"].ToString()),
                    AfectadoGasmed = Convert.ToBoolean(dr["AfectadoGasmed"]),
                    VtoCedulaVerde = Convert.ToDateTime(dr["VtoCedulaVerde"]).ToString("dd/MM/yyyy"),
                    NroRuta = dr["NroRUTA"].ToString(),
                    VtoRuta = Convert.ToDateTime(dr["VtoRUTA"]).ToString("dd/MM/yyyy"),
                    VtoVtv = Convert.ToDateTime(dr["VtoVTV"]).ToString("dd/MM/yyyy"),
                    VtoStaCruz = Convert.ToDateTime(dr["VtoStaCruz"]).ToString("dd/MM/yyyy"),
                    VtoPatente = Convert.ToDateTime(dr["VtoPatente"]).ToString("dd/MM/yyyy"),
                    CiaSeguro = dr["CiaSeguro"].ToString(),
                    PolizaSeguro = Convert.ToInt32(dr["CiaSeguroPoliza"]),
                    VtoSeguro = Convert.ToDateTime(dr["VtoSeguro"]).ToString("dd/MM/yyyy"),
                    NroChasis = dr["NroChasis"].ToString(),
                    NroMotor = dr["NroMotor"].ToString(),
                    VtoCertIzaje = Convert.ToDateTime(dr["VtoCertIzaje"]).ToString("dd/MM/yyyy")
                };
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public static void Update(List<VehiculoSummary> vehiculos)
        {
            IDbConnection conn = null;
            IDbTransaction trans = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                vehiculos.ForEach(v => Update(v, trans));

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        private static void Update(VehiculoSummary v, IDbTransaction trans)
        {
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE Vehiculos SET ";
            cmd.CommandText += "UbicacionId = @UbicacionId, ResponsableId = @ResponsableId, AfectadoGasmed = @AfectadoGasmed, ";
            cmd.CommandText += "VtoCedulaVerde = @VtoCedulaVerde, NroRUTA = @NroRUTA, VtoRUTA = @VtoRUTA, VtoVTV = @VtoVTV, ";
            cmd.CommandText += "VtoPatente = @VtoPatente, CiaSeguro = @CiaSeguro, CiaSeguroPoliza = @CiaSeguroPoliza, ";
            cmd.CommandText += "VtoSeguro = @VtoSeguro, VtoStaCruz = @VtoStaCruz, ";
            cmd.CommandText += "VtoCertIzaje = @VtoCertIzaje ";
            cmd.CommandText += "WHERE Id = @Id; ";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Id", v.Id));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@UbicacionId", Convert.ToInt32(v.Ubicacion)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", Convert.ToInt32(v.Responsable)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AfectadoGasmed", v.AfectadoGasmed));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoCedulaVerde", !EsFechaNoAplica(v.VtoCedulaVerde) ? Funciones.ParseFecha(v.VtoCedulaVerde) : Constants.InvalidDateTime));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NroRUTA", v.NroRuta));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoRUTA", !EsFechaNoAplica(v.VtoRuta) ? Funciones.ParseFecha(v.VtoRuta) : Constants.InvalidDateTime));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoVTV", !EsFechaNoAplica(v.VtoVtv) ? Funciones.ParseFecha(v.VtoVtv) : Constants.InvalidDateTime));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoStaCruz", !EsFechaNoAplica(v.VtoStaCruz) ? Funciones.ParseFecha(v.VtoStaCruz) : Constants.InvalidDateTime));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoPatente", !EsFechaNoAplica(v.VtoPatente) ? Funciones.ParseFecha(v.VtoPatente) : Constants.InvalidDateTime));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CiaSeguro", v.CiaSeguro));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CiaSeguroPoliza", v.PolizaSeguro));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoSeguro", !EsFechaNoAplica(v.VtoSeguro) ? Funciones.ParseFecha(v.VtoSeguro) : Constants.InvalidDateTime));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoCertIzaje", !EsFechaNoAplica(v.VtoCertIzaje) ? Funciones.ParseFecha(v.VtoCertIzaje) : Constants.InvalidDateTime));

            cmd.ExecuteNonQuery();
        }

        internal static bool EsFechaNoAplica(string f)
        {
            return f.Equals(Constants.InvalidDateTime.ToString("dd/MM/yyyy"));
        }

        public static Dictionary<string, List<VencimientoVehiculo>> ListVencimientos(string mes)
        {
            var inicio = Funciones.DateFromMonth(mes);
            var fin = Utilities.NormalizeDateEnd(inicio).AddMonths(1).AddDays(-1);

            return ListVencimientos(inicio, fin);
        }

        internal static Dictionary<string, List<VencimientoVehiculo>> ListVencimientos(DateTime inicio, DateTime fin)
        {
            Dictionary<string, List<VencimientoVehiculo>> result = new Dictionary<string, List<VencimientoVehiculo>>();
            var vehiculos = List();

            // Vto Cert. Izaje.
            var vtoCertIzaje = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vence(inicio, fin, v.VtoCertIzaje)) vtoCertIzaje.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoCertIzaje
                });
            });
            result.Add("Vto Cert. Izaje", vtoCertIzaje);

            // Vto Cedula Verde.
            var vtoCedulaVerde = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vence(inicio, fin, v.VtoCedulaVerde)) vtoCedulaVerde.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoCedulaVerde
                });
            });
            result.Add("Vto Cedula Verde", vtoCedulaVerde);

            // Vto R.U.T.A..
            var vtoRuta = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vence(inicio, fin, v.VtoRuta)) vtoRuta.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoRuta
                });
            });
            result.Add("Vto R.U.T.A.", vtoRuta);

            // Vto V.T.V..
            var vtoVtv = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vence(inicio, fin, v.VtoVtv)) vtoVtv.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoVtv
                });
            });
            result.Add("Vto V.T.V.", vtoVtv);

            // Vto Patente.
            var vtoPatente = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoPatente)) vtoPatente.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoPatente
                });
            });
            result.Add("Vto Patente", vtoPatente);

            // Hab Prov Santa Cruz.
            var vtoStaCruz = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vence(inicio, fin, v.VtoStaCruz)) vtoStaCruz.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoStaCruz
                });
            });
            result.Add("Hab Prov Santa Cruz", vtoStaCruz);

            // Vto Seguro.
            var vtoSeguro = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vence(inicio, fin, v.VtoSeguro)) vtoSeguro.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoSeguro
                });
            });
            result.Add("Vto Seguro", vtoSeguro);

            return result;
        }
        
        private static bool Vence(DateTime inicio, DateTime fin, string fecha)
        {
            DateTime f = Funciones.ParseFecha(fecha);

            return f != Constants.InvalidDateTime && (f >= inicio && f <= fin);
        }

        private static bool Vencido(string fecha)
        {
            DateTime f = Funciones.ParseFecha(fecha);

            return f != Constants.InvalidDateTime && DateTime.Now > f;
        }

        public static void SendVencimientos()
        {
            var hoy = DateTime.Now;
            var fecha = new DateTime(hoy.Year, hoy.Month, 1, 0, 0, 0).AddMonths(2);
            var hasta = Utilities.NormalizeDateEnd(fecha).AddMonths(1).AddDays(-1);
            var vencimientos = ListVencimientos(fecha, hasta);

            VencimientoEmail email = new VencimientoEmail(fecha, vencimientos);
            try
            {
                email.SendFromIntranet(PermisoPersona.ListEmails(PermisosPersona.VehicAlerta), "", "Vencimientos " + 
                    fecha.ToString("MM/yyyy"));
            }
            catch
            {
                
            }
        }

        public static Dictionary<string, List<VencimientoVehiculo>> ListVencidos()
        {
            Dictionary<string, List<VencimientoVehiculo>> result = new Dictionary<string, List<VencimientoVehiculo>>();
            var vehiculos = List();

            DateTime hoy = DateTime.Now;

            // Vto Cert. Izaje.
            var vtoCertIzaje = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoCertIzaje)) vtoCertIzaje.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoCertIzaje
                });
            });
            result.Add("Vto Cert. Izaje", vtoCertIzaje);

            // Vto Cedula Verde.
            var vtoCedulaVerde = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoCedulaVerde)) vtoCedulaVerde.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoCedulaVerde
                });
            });
            result.Add("Vto Cedula Verde", vtoCedulaVerde);

            // Vto R.U.T.A..
            var vtoRuta = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoRuta)) vtoRuta.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoRuta
                });
            });
            result.Add("Vto R.U.T.A.", vtoRuta);

            // Vto V.T.V..
            var vtoVtv = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoVtv)) vtoVtv.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoVtv
                });
            });
            result.Add("Vto V.T.V.", vtoVtv);

            // Vto Patente.
            var vtoPatente = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoPatente)) vtoPatente.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoPatente
                });
            });
            result.Add("Vto Patente", vtoPatente);

            // Hab Prov Santa Cruz.
            var vtoStaCruz = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoStaCruz)) vtoStaCruz.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoStaCruz
                });
            });
            result.Add("Hab Prov Santa Cruz", vtoStaCruz);

            // Vto Seguro.
            var vtoSeguro = new List<VencimientoVehiculo>();
            vehiculos.ForEach(v =>
            {
                if (Vencido(v.VtoSeguro)) vtoSeguro.Add(new VencimientoVehiculo
                {
                    Patente = v.Patente,
                    Fecha = v.VtoSeguro
                });
            });
            result.Add("Vto Seguro", vtoSeguro);

            return result;
        }

        public static List<DataSourceItem> ListUbicaciones()
        {
            List<DataSourceItem> result = new List<DataSourceItem>();
            IDbConnection conn = null;
            IDataReader dr = null;

            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                IDbCommand cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT * FROM VehiculosUbicaciones ";
                cmd.CommandText += "ORDER BY Nombre";
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
                if (conn != null) conn.Close();
            }

            return result;
        }

        public static string Exportar()
        {
            const int FilaInicio = 5;

            var vehiculos = List();

            Random r = new Random(DateTime.Now.Millisecond);
            string path = Funciones.GetTempPath() + "archivo" + r.Next() + ".xls";
            bool celda_con_color = false;

            // Abro el Excel.
            ExcelApp excel = new ExcelApp
            {
                Visible = false, 
                DisplayAlerts = false
            };
            excel.ErrorCheckingOptions.NumberAsText = false;

            // Abro el libro.
            Workbook libro = excel.Workbooks.Add();

            // Abro la hoja.
            Worksheet hoja1 = (Worksheet)libro.Worksheets[1];

            // Título.
            int fila = 1;
            hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 18]).Merge(true);
            ((Range)hoja1.Cells[fila, 1]).FormulaR1C1 = "Vehículos";
            ((Range)hoja1.Cells[fila, 1]).Font.Bold = true;
            ((Range)hoja1.Cells[fila, 1]).Font.Size = 12;
            ((Range)hoja1.Cells[fila, 1]).HorizontalAlignment = 3;

            // Columnas.
            fila = 3;
            hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 17]).Interior.Color = Color.Gray.ToArgb();
            hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 17]).Font.Color = Color.White.ToArgb();
            hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 17]).Font.Bold = true;
            hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 17]).Font.Size = 10;
            hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 17]).HorizontalAlignment = 3;
            ((Range)hoja1.Cells[fila, 1]).FormulaR1C1 = "Dominio";
            ((Range)hoja1.Cells[fila, 2]).FormulaR1C1 = "Modelo";
            ((Range)hoja1.Cells[fila, 3]).FormulaR1C1 = "Tipo de Vehículo";
            ((Range)hoja1.Cells[fila, 4]).FormulaR1C1 = "Año";
            ((Range)hoja1.Cells[fila, 5]).FormulaR1C1 = "Ubicación";
            ((Range)hoja1.Cells[fila, 6]).FormulaR1C1 = "Responsable";
            ((Range)hoja1.Cells[fila, 7]).FormulaR1C1 = "Vto Cert. Izaje";
            ((Range)hoja1.Cells[fila, 8]).FormulaR1C1 = "Vto. Cédula Verde";
            ((Range)hoja1.Cells[fila, 9]).FormulaR1C1 = "Nº R.U.T.A.";
            ((Range)hoja1.Cells[fila, 10]).FormulaR1C1 = "Vto. R.U.T.A.";
            ((Range)hoja1.Cells[fila, 11]).FormulaR1C1 = "Vto. V.T.V.";
            ((Range)hoja1.Cells[fila, 12]).FormulaR1C1 = "Hab Prov Santa Cruz";
            ((Range)hoja1.Cells[fila, 13]).FormulaR1C1 = "Vto. Patente";
            ((Range)hoja1.Cells[fila, 14]).FormulaR1C1 = "Compañía Seguro";
            ((Range)hoja1.Cells[fila, 15]).FormulaR1C1 = "Póliza Seguro";
            ((Range)hoja1.Cells[fila, 16]).FormulaR1C1 = "Vto. Seguro";
            ((Range)hoja1.Cells[fila, 17]).FormulaR1C1 = "Nº de Chasis";
            ((Range)hoja1.Cells[fila, 18]).FormulaR1C1 = "Nº de Motor";

            // Anchos.
            ((Range)hoja1.Cells[fila, 1]).ColumnWidth = 9.1;
            ((Range)hoja1.Cells[fila, 2]).ColumnWidth = 33.8;
            ((Range)hoja1.Cells[fila, 3]).ColumnWidth = 17.29;
            ((Range)hoja1.Cells[fila, 4]).ColumnWidth = 5.2;
            ((Range)hoja1.Cells[fila, 5]).ColumnWidth = 16.12;
            ((Range)hoja1.Cells[fila, 6]).ColumnWidth = 14.95;
            ((Range)hoja1.Cells[fila, 7]).ColumnWidth = 17.03;
            ((Range)hoja1.Cells[fila, 8]).ColumnWidth = 16.12;
            ((Range)hoja1.Cells[fila, 9]).ColumnWidth = 9.88;
            ((Range)hoja1.Cells[fila, 10]).ColumnWidth = 11.31;
            ((Range)hoja1.Cells[fila, 11]).ColumnWidth = 9.62;
            ((Range)hoja1.Cells[fila, 12]).ColumnWidth = 11.05;
            ((Range)hoja1.Cells[fila, 13]).ColumnWidth = 11.05;
            ((Range)hoja1.Cells[fila, 14]).ColumnWidth = 15.99;
            ((Range)hoja1.Cells[fila, 15]).ColumnWidth = 12.61;
            ((Range)hoja1.Cells[fila, 16]).ColumnWidth = 10.66;
            ((Range)hoja1.Cells[fila, 17]).ColumnWidth = 18.85;
            ((Range)hoja1.Cells[fila, 18]).ColumnWidth = 18.85;

            // Completo los datos.
            fila = FilaInicio;
            int columna;
            foreach (var v in vehiculos)
            {
                columna = 1;

                // Dominio.
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.Patente;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Modelo.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.Modelo;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Tipo.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.Tipo;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Año.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.Anio;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Ubicación.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.Ubicacion;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Responsable.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.Responsable;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Afectado a gasmed.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + (EsFechaNoAplica(v.VtoCertIzaje) ? "-" : v.VtoCertIzaje);
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Vencimiento Cédula Verde.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + (EsFechaNoAplica(v.VtoCedulaVerde) ? "-" : v.VtoCedulaVerde);
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Nº RUTA.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.NroRuta;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Vto RUTA.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + (EsFechaNoAplica(v.VtoRuta) ? "-" : v.VtoRuta); ;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Vencimiento VTV.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + (EsFechaNoAplica(v.VtoVtv) ? "-" : v.VtoVtv);
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Vencimiento Patente.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + (EsFechaNoAplica(v.VtoPatente) ? "-" : v.VtoPatente);
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Cia Seguro.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.CiaSeguro;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Póliza seguro.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.PolizaSeguro;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Vto Seguro.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + (EsFechaNoAplica(v.VtoSeguro) ? "-" : v.VtoSeguro);
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Nº chasis.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.NroChasis;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                // Nº Motor.
                columna++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = v.NroMotor;
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

                fila++;
            }

            // Guardo el libro.
            libro.SaveAs(path, XlFileFormat.xlExcel9795, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);

            // Cierro el Excel.
            libro.Close(false, Missing.Value, Missing.Value);
            excel.Quit();

            // Libero los recursos.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(hoja1);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(libro);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

            return path;
        }

        public static string ExportarCsv()
        {
            var vehiculos = List();

            CsvHelper csv = new CsvHelper(vehiculos[0]);
            csv.SetColumns(vehiculos[0]);
            vehiculos.ForEach(csv.AppendRow);

            Random r = new Random(DateTime.Now.Millisecond);
            string path = Funciones.GetTempPath() + "archivo" + r.Next() + ".csv";

            try
            {
                File.WriteAllText(path, csv.MakeCsv(), Encoding.UTF8);
            }
            catch
            {
                
            }

            return path;
        }
    }
}
