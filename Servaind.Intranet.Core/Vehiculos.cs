using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Web;
using Microsoft.Office.Interop.Excel;
using ExcelApp = Microsoft.Office.Interop.Excel.Application;
using System.Reflection;

public class Vehiculo : IEquatable<Vehiculo>
{
    // Variables.
    private int idVehiculo;
    private string patente;
    private string modelo;
    private TipoVehiculo tipoVehiculo;
    private int anio;
    private string ubicacion;
    private int idResponsable;
    private bool afectadoGasmed;
    private DateTime vtoCedulaVerde;
    private string nroRUTA;
    private DateTime vtoRUTA;
    private DateTime vtoVTV;
    private DateTime vtoPatente;
    private string ciaSeguro;
    private int polizaSeguro;
    private DateTime vtoSeguro;
    private string nroChasis;
    private string nroMotor;

    // Properties.
    public int IdVehiculo
    {
        get { return this.idVehiculo; }
    }
    public string Patente
    {
        get { return this.patente; }
    }
    public string Modelo
    {
        get { return this.modelo; }
    }
    public TipoVehiculo TipoVehiculo
    {
        get { return this.tipoVehiculo; }
    }
    public int Anio
    {
        get { return this.anio; }
    }
    public string Ubicacion
    {
        get { return this.ubicacion; }
    }
    public int IdResponsable
    {
        get { return this.idResponsable; }
    }
    public bool AfectadoGasmed
    {
        get { return this.afectadoGasmed; }
    }
    public DateTime VtoCedulaVerde
    {
        get { return this.vtoCedulaVerde; }
    }
    public string NroRUTA
    {
        get { return this.nroRUTA; }
    }
    public DateTime VtoRUTA
    {
        get { return this.vtoRUTA; }
    }
    public DateTime VtoVTV
    {
        get { return this.vtoVTV; }
    }
    public DateTime VtoPatente
    {
        get { return this.vtoPatente; }
    }
    public string CiaSeguro 
    {
        get { return this.ciaSeguro; }
    }
    public int PolizaSeguro
    {
        get { return this.polizaSeguro; }
    }
    public DateTime VtoSeguro
    {
        get { return this.vtoSeguro; }
    }
    public string NroChasis
    {
        get { return this.nroChasis; }
    }
    public string NroMotor
    {
        get { return this.nroMotor; }
    }


    internal Vehiculo(int idVehiculo, string patente, string modelo, TipoVehiculo tipoVehiculo, int anio, string ubicacion,
        int idResponsable, bool afectadoGasmed, DateTime vtoCedulaVerde, string nroRUTA, DateTime vtoRUTA, DateTime vtoVTV,
        DateTime vtoPatente, string ciaSeguro, int polizaSeguro, DateTime vtoSeguro, string nroChasis, string nroMotor)
    {
        this.idVehiculo = idVehiculo;
        this.patente = patente;
        this.modelo = modelo;
        this.tipoVehiculo = tipoVehiculo;
        this.anio = anio;
        this.ubicacion = ubicacion;
        this.idResponsable = idResponsable;
        this.afectadoGasmed = afectadoGasmed;
        this.vtoCedulaVerde = vtoCedulaVerde;
        this.nroRUTA = nroRUTA;
        this.vtoRUTA = vtoRUTA;
        this.vtoVTV = vtoVTV;
        this.vtoPatente = vtoPatente;
        this.ciaSeguro = ciaSeguro;
        this.polizaSeguro = polizaSeguro;
        this.vtoSeguro = vtoSeguro;
        this.nroChasis = nroChasis;
        this.nroMotor = nroMotor;

    }

    public override string ToString()
    {
        return this.Patente;
    }

    public bool Equals(Vehiculo other)
    {
        return this.IdVehiculo == other.IdVehiculo;
    }
}

public class VencimientoVehiculo
{
    // Variables.
    private string patente;
    private int diasVtoCedulaVerde;
    private int diasVtoRUTA;
    private int diasVtoVTV;
    private int diasVtoPatente;
    private int diasVtoSeguro;
    private string emailResponsable;

    // Propiedades.
    public string Patente
    {
        get { return this.patente; }
    }
    public int DiasVtoCedulaVerde
    {
        get { return this.diasVtoCedulaVerde; }
    }
    public int DiasVtoRUTA
    {
        get { return this.diasVtoRUTA; }
    }
    public int DiasVtoVTV
    {
        get { return this.diasVtoVTV; }
    }
    public int DiasVtoPatente
    {
        get { return this.diasVtoPatente; }
    }
    public int DiasVtoSeguro
    {
        get { return this.diasVtoSeguro; }
    }
    public string EmailResponsable
    {
        get { return this.emailResponsable; }
    }


    internal VencimientoVehiculo(string patente, int diasVtoCedulaVerde, int diasVtoRUTA, int diasVtoVTV, int diasVtoPatente,
        int diasVtoSeguro, string emailResponsable)
    {
        this.patente = patente;
        this.diasVtoCedulaVerde = diasVtoCedulaVerde;
        this.diasVtoRUTA = diasVtoRUTA;
        this.diasVtoVTV = diasVtoVTV;
        this.diasVtoPatente = diasVtoPatente;
        this.diasVtoSeguro = diasVtoSeguro;
        this.emailResponsable = emailResponsable;
    }

    public override string ToString()
    {
        return this.Patente;
    }
}

public class AlertaVencimiento
{
    // Variables.
    private string patente;
    private string campo;
    private string descripcion;
    private TipoAlertaVencimiento alerta;
    private string emailResponsable;

    // Propiedades.
    public string Patente
    {
        get { return this.patente; }
    }
    public string Campo
    {
        get { return this.campo; }
    }
    public string Descripcion
    {
        get { return this.descripcion; }
    }
    public TipoAlertaVencimiento Alerta
    {
        get { return this.alerta; }
    }
    public string EmailResponsable
    {
        get { return this.emailResponsable; }
    }


    internal AlertaVencimiento(string patente, string campo, string descripcion, TipoAlertaVencimiento alerta, 
        string emailResponsable)
    {
        this.patente = patente;
        this.campo = campo;
        this.descripcion = descripcion;
        this.alerta = alerta;
        this.emailResponsable = emailResponsable;
    }
}

public class ItemVencimiento : IComparable
{
    // Variables.
    private string patente;
    private DateTime fecha;

    // Propiedades.
    public string Patente
    {
        get { return this.patente; }
    }
    public DateTime Fecha
    {
        get { return this.fecha; }
    }


    internal ItemVencimiento(string patente, DateTime fecha)
    {
        this.patente = patente;
        this.fecha = fecha;
    }

    public int CompareTo(object obj)
    {
        ItemVencimiento i = (ItemVencimiento)obj;

        return this.Fecha.CompareTo(i.Fecha);
    }
}

/// <summary>
/// Summary description for Vehiculos
/// </summary>
public static class Vehiculos
{
    // Constantes.
    public const int MinDiasCedula = 20;
    public const int MinDiasAviso = 15;
    public const string NoAplica = "N/A";


    /// <summary>
    /// Obtiene un vehículo.
    /// </summary>
    private static Vehiculo GetVehiculo(IDataReader dr)
    {
        Vehiculo result;

        try
        {
            result = new Vehiculo(Convert.ToInt32(dr["idVehiculo"]), dr["Patente"].ToString(),
                dr["Modelo"].ToString(), TiposVehiculo.GetTipoVehiculo(Convert.ToInt32(dr["idTipoVehiculo"])),
                Convert.ToInt32(dr["Anio"]), dr["Ubicacion"].ToString(), Convert.ToInt32(dr["idResponsable"]),
                Convert.ToBoolean(dr["AfectadoGasmed"]), Convert.ToDateTime(dr["VtoCedulaVerde"]), dr["NroRUTA"].ToString(),
                Convert.ToDateTime(dr["VtoRUTA"]), Convert.ToDateTime(dr["VtoVTV"]), Convert.ToDateTime(dr["VtoPatente"]),
                dr["CiaSeguro"].ToString(), Convert.ToInt32(dr["CiaSeguroPoliza"]), Convert.ToDateTime(dr["VtoSeguro"]), 
                dr["NroChasis"].ToString(), dr["NroMotor"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una alerta de vencimiento para un vehículo.
    /// </summary>
    private static VencimientoVehiculo GetAlertaVencimiento(IDataReader dr)
    {
        VencimientoVehiculo result;

        try
        {
            result = new VencimientoVehiculo(dr["Patente"].ToString(), Convert.ToInt32(dr["VtoCedulaVerde"]),
                Convert.ToInt32(dr["VtoRUTA"]), Convert.ToInt32(dr["VtoVTV"]), Convert.ToInt32(dr["VtoPatente"]),
                Convert.ToInt32(dr["VtoSeguro"]), dr["EmailResponsable"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un vehículo.
    /// </summary>
    public static Vehiculo GetVehiculo(int idVehiculo)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        Vehiculo result = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Vehiculos WHERE idVehiculo = @idVehiculo";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idVehiculo", idVehiculo));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetVehiculo(dr);
            }

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Actualiza los vehículos.
    /// </summary>
    public static void ActualizarVehiculos(object[][] datos)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            foreach (object[] dato in datos)
            {
                ActualizarVehiculo(dato, trans);
            }

            trans.Commit();
        }
        catch
        {
            if (trans != null)
            {
                trans.Rollback();
            }
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
    /// <summary>
    /// Actualiza un vehículo.
    /// </summary>
    private static void ActualizarVehiculo(object[] datos, IDbTransaction trans)
    {
        IDbCommand cmd = DataAccess.GetCommand(trans);

        cmd.CommandText = "UPDATE tbl_Vehiculos SET idTipoVehiculo = @idTipoVehiculo, Modelo = @Modelo, Anio = @Anio, ";
        cmd.CommandText += "Ubicacion = @Ubicacion, idResponsable = @idResponsable, AfectadoGasmed = @AfectadoGasmed, ";
        cmd.CommandText += "VtoCedulaVerde = @VtoCedulaVerde, NroRUTA = @NroRUTA, VtoRUTA = @VtoRUTA, VtoVTV = @VtoVTV, ";
        cmd.CommandText += "VtoPatente = @VtoPatente, CiaSeguro = @CiaSeguro, CiaSeguroPoliza = @CiaSeguroPoliza, ";
        cmd.CommandText += "VtoSeguro = @VtoSeguro, NroChasis = @NroChasis, NroMotor = @NroMotor ";
        cmd.CommandText += "WHERE idVehiculo = @idVehiculo; ";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idVehiculo", datos[0]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Modelo", datos[1]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idTipoVehiculo", datos[2]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Anio", datos[3]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Ubicacion", datos[4]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idResponsable", datos[5]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@AfectadoGasmed", datos[6]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoCedulaVerde", !datos[7].Equals(NoAplica) ? datos[7] : Constantes.FechaInvalida));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@NroRUTA", datos[8]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoRUTA", !datos[9].Equals(NoAplica) ? datos[9] : Constantes.FechaInvalida));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoVTV", !datos[10].Equals(NoAplica) ? datos[10] : Constantes.FechaInvalida));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoPatente", !datos[11].Equals(NoAplica) ? datos[11] : Constantes.FechaInvalida));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@CiaSeguro", datos[12]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@CiaSeguroPoliza", datos[13]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@VtoSeguro", !datos[14].Equals(NoAplica) ? datos[14] : Constantes.FechaInvalida));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@NroChasis", datos[15]));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@NroMotor", datos[16]));

        cmd.ExecuteNonQuery();
    }
    /// <summary>
    /// Obtiene una lista de vehículos.
    /// </summary>
    public static List<Vehiculo> GetVehiculos()
    {
        List<Vehiculo> result = new List<Vehiculo>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Vehiculos WHERE Activo = 1 ORDER BY Patente";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Vehiculo vehiculo = GetVehiculo(dr);
                if (vehiculo != null)
                {
                    result.Add(vehiculo);
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
    /// <summary>
    /// Obtiene una lista de vehículos.
    /// </summary>
    internal static List<Vehiculo> GetVehiculosInfObra(int idObraHistorico)
    {
        List<Vehiculo> result = new List<Vehiculo>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT v.* FROM tbl_Vehiculos v INNER JOIN tbl_InformacionObrasVehiculos t ON v.idVehiculo = ";
            cmd.CommandText += "t.idVehiculo WHERE idObraHistorico = @idObraHistorico ORDER BY v.Patente";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObraHistorico", idObraHistorico));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Vehiculo vehiculo = GetVehiculo(dr);
                if (vehiculo != null)
                {
                    result.Add(vehiculo);
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
    /// <summary>
    /// Procesa los vencimientos.
    /// </summary>
    public static void ProcesarVencimientos()
    {
        List<VencimientoVehiculo> vencimientos = GetAlertasVencimientos();
        List<AlertaVencimiento> alertas = new List<AlertaVencimiento>();

        foreach (VencimientoVehiculo vencimiento in vencimientos)
        {
            // Cédula verde.
            AlertaVencimiento cedulaVerde = GetAlertaVencimiento(vencimiento.Patente, "Cédula verde", vencimiento.DiasVtoCedulaVerde,
                MinDiasCedula, vencimiento.EmailResponsable);
            if (cedulaVerde != null)
            {
                alertas.Add(cedulaVerde);
            }

            // R.U.T.A.
            AlertaVencimiento ruta = GetAlertaVencimiento(vencimiento.Patente, "R.U.T.A.", vencimiento.DiasVtoRUTA, MinDiasAviso, 
                vencimiento.EmailResponsable);
            if (ruta != null)
            {
                alertas.Add(ruta);
            }

            // VTV.
            AlertaVencimiento vtv = GetAlertaVencimiento(vencimiento.Patente, "VTV", vencimiento.DiasVtoVTV, MinDiasAviso, 
                vencimiento.EmailResponsable);
            if (vtv != null)
            {
                alertas.Add(vtv);
            }

            // Patente.
            AlertaVencimiento patente = GetAlertaVencimiento(vencimiento.Patente, "Patente", vencimiento.DiasVtoPatente, MinDiasAviso, 
                vencimiento.EmailResponsable);
            if (patente != null)
            {
                alertas.Add(patente);
            }

            // Seguro.
            AlertaVencimiento seguro = GetAlertaVencimiento(vencimiento.Patente, "Seguro", vencimiento.DiasVtoSeguro, MinDiasAviso, 
                vencimiento.EmailResponsable);
            if (seguro != null)
            {
                alertas.Add(seguro);
            }
        }

        alertas.ForEach(a => EnviarAlertaVencimiento(a));
    }
    /// <summary>
    /// Envía una alerta de vencimiento.
    /// </summary>
    private static void EnviarAlertaVencimiento(AlertaVencimiento alerta)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_ALERTA_VENCIMIENTO);

        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        string asunto = "Alerta de vencimiento " + alerta.Patente + " [" + alerta.Campo + "] ";
        string tipoMensaje = "info";
        switch (alerta.Alerta)
        {
            case TipoAlertaVencimiento.Precaucion:
                asunto += "[PRÓXIMO A VENCIMIENTO]";
                tipoMensaje = "warning";
                break;
            case TipoAlertaVencimiento.Vencido:
                asunto += "[VENCIDO]";
                tipoMensaje = "error";
                break;
            case TipoAlertaVencimiento.Recordatorio:
                asunto += "[VENCIDO - Recordatorio]";
                tipoMensaje = "error";
                break;
            default:
                throw new EmailException();
        }

        strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", tipoMensaje);
        strPlantilla = strPlantilla.Replace("@PATENTE", alerta.Patente);
        strPlantilla = strPlantilla.Replace("@CAMPO", alerta.Campo);
        strPlantilla = strPlantilla.Replace("@DESCRIPCION", alerta.Descripcion);

        Email email = new Email(Constantes.EmailIntranet, Constantes.EmailResponsablesVehiculos, alerta.EmailResponsable, asunto, 
            strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene una alerta de vencimiento.
    /// </summary>
    private static AlertaVencimiento GetAlertaVencimiento(string patente, string campo, int diff, int max, string emailResponsable)
    {
        AlertaVencimiento result = null;

        if (diff > 0)
        {
            // Recordatorio.
            result = new AlertaVencimiento(patente, campo, "vencido hace " + (diff) + " día" + (diff == 1 ? "" : "s") + ".", 
                TipoAlertaVencimiento.Recordatorio, emailResponsable);
        }
        else if (diff == 0)
        {
            // Vencido.
            result = new AlertaVencimiento(patente, campo, "ha vencido.", TipoAlertaVencimiento.Vencido, emailResponsable);
        }
        else if((diff * -1) <= max)
        {
            diff = -diff;

            if (diff == 15 || diff == 10 || diff == 5)
            {
                // Precaución.
                result = new AlertaVencimiento(patente, campo, "queda" + (diff == 1 ? "" : "n") + " " + diff + " día" +
                    (diff == 1 ? "" : "s") + " para el vencimiento.", TipoAlertaVencimiento.Precaucion, emailResponsable);
            }
        }

        return result;
    }
    /// <summary>
    /// Obtiene los vencimientos en alerta.
    /// </summary>
    public static List<VencimientoVehiculo> GetAlertasVencimientos()
    {
        List<VencimientoVehiculo> result = new List<VencimientoVehiculo>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Patente, ";
            cmd.CommandText += "CASE VtoCedulaVerde WHEN @FechaInv THEN @Max ELSE DATEDIFF(dd, VtoCedulaVerde, GetDate()) END AS VtoCedulaVerde, ";
            cmd.CommandText += "CASE VtoRUTA WHEN @FechaInv THEN @Max ELSE DATEDIFF(dd, VtoRUTA, GetDate()) END AS VtoRUTA, ";
            cmd.CommandText += "CASE VtoVTV WHEN @FechaInv THEN @Max ELSE DATEDIFF(dd, VtoVTV, GetDate()) END AS VtoVTV, ";
            cmd.CommandText += "CASE VtoPatente WHEN @FechaInv THEN @Max ELSE DATEDIFF(dd, VtoPatente, GetDate()) END AS VtoPatente, ";
            cmd.CommandText += "CASE VtoSeguro WHEN @FechaInv THEN @Max ELSE DATEDIFF(dd, VtoSeguro, GetDate()) END AS VtoSeguro, ";
            cmd.CommandText += "p.Email AS EmailResponsable ";
            cmd.CommandText += "FROM tbl_Vehiculos v INNER JOIN tbl_Personal p ON p.idPersonal = v.idResponsable ";
            cmd.CommandText += "AND v.Activo = @Activo ORDER BY Patente";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaInv", Constantes.FechaInvalida));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Max", -1000));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                VencimientoVehiculo alerta = GetAlertaVencimiento(dr);
                if (alerta != null)
                {
                    result.Add(alerta);
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
    /// <summary>
    /// Obtiene los vencimientos para el mes.
    /// </summary>
    public static Dictionary<string, List<ItemVencimiento>> GetVencimientosMes(int mes, int anio)
    {
        Dictionary<string, List<ItemVencimiento>> result = new Dictionary<string, List<ItemVencimiento>>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        List<ItemVencimiento> vtosCedulaVerde = new List<ItemVencimiento>();
        List<ItemVencimiento> vtosRUTA = new List<ItemVencimiento>();
        List<ItemVencimiento> vtosVTV = new List<ItemVencimiento>();
        List<ItemVencimiento> vtosPatente = new List<ItemVencimiento>();
        List<ItemVencimiento> vtosSeguro = new List<ItemVencimiento>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Patente, VtoCedulaVerde, VtoRUTA, VtoVTV, VtoPatente, VtoSeguro ";
            cmd.CommandText += "FROM tbl_Vehiculos WHERE Activo = 1 AND ";
            cmd.CommandText += "((MONTH(VtoCedulaVerde) = @mes AND YEAR(VtoCedulaVerde) = @anio) OR ";
            cmd.CommandText += "(MONTH(VtoRUTA) = @mes AND YEAR(VtoRUTA) = @anio) OR ";
            cmd.CommandText += "(MONTH(VtoVTV) = @mes AND YEAR(VtoVTV) = @anio) OR ";
            cmd.CommandText += "(MONTH(VtoPatente) = @mes AND YEAR(VtoPatente) = @anio) OR ";
            cmd.CommandText += "(MONTH(VtoSeguro) = @mes AND YEAR(VtoSeguro) = @anio)";
            cmd.CommandText += ") ";
            cmd.CommandText += "ORDER BY Patente";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@mes", mes));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@anio", anio));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string patente = dr["Patente"].ToString();
                DateTime vtoCedulaVerde = Convert.ToDateTime(dr["VtoCedulaVerde"]);
                DateTime vtoRUTA = Convert.ToDateTime(dr["VtoRUTA"]);
                DateTime vtoVTV = Convert.ToDateTime(dr["VtoVTV"]);
                DateTime vtoPatente = Convert.ToDateTime(dr["VtoPatente"]);
                DateTime vtoSeguro = Convert.ToDateTime(dr["VtoSeguro"]);

                if (vtoCedulaVerde != Constantes.FechaInvalida && vtoCedulaVerde.Month == mes && vtoCedulaVerde.Year == anio)
                {
                    vtosCedulaVerde.Add(new ItemVencimiento(patente, vtoCedulaVerde));
                }
                if (vtoRUTA != Constantes.FechaInvalida && vtoRUTA.Month == mes && vtoRUTA.Year == anio)
                {
                    vtosRUTA.Add(new ItemVencimiento(patente, vtoRUTA));
                }
                if (vtoVTV != Constantes.FechaInvalida && vtoVTV.Month == mes && vtoVTV.Year == anio)
                {
                    vtosVTV.Add(new ItemVencimiento(patente, vtoVTV));
                }
                if (vtoPatente != Constantes.FechaInvalida && vtoPatente.Month == mes && vtoPatente.Year == anio)
                {
                    vtosPatente.Add(new ItemVencimiento(patente, vtoPatente));
                }
                if (vtoSeguro != Constantes.FechaInvalida && vtoSeguro.Month == mes && vtoSeguro.Year == anio)
                {
                    vtosSeguro.Add(new ItemVencimiento(patente, vtoSeguro));
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

        if (vtosCedulaVerde.Count > 0)
        {
            vtosCedulaVerde.Sort();
            result.Add("Cédula verde", vtosCedulaVerde);
        }
        if (vtosRUTA.Count > 0)
        {
            vtosRUTA.Sort();
            result.Add("R.U.T.A.", vtosRUTA);
        }
        if (vtosVTV.Count > 0)
        {
            vtosVTV.Sort();
            result.Add("V.T.V.", vtosVTV);
        }
        if (vtosPatente.Count > 0)
        {
            vtosPatente.Sort();
            result.Add("Patente", vtosPatente);
        }
        if (vtosSeguro.Count > 0)
        {
            vtosSeguro.Sort();
            result.Add("Seguro", vtosSeguro);
        }

        return result;
    }
    /// <summary>
    /// Exporta los vencimientos a excel.
    /// </summary>
    public static string ExportarVencimientos(int mes, int anio)
    {
        ExcelApp excel;
        const int FilaInicio = 5;

        Dictionary<string, List<ItemVencimiento>> vencimientos = GetVencimientosMes(mes, anio);
        int total = vencimientos.Keys.Count;
        if (total == 0)
        {
            throw new Exception("No se han detectado vencimientos para el mes seleccionado.");
        }

        Random r = new Random(DateTime.Now.Millisecond);
        string path = Constantes.PATH_TEMP + "archivo" + r.Next() + ".xls";
        bool celda_con_color = false;

        // Abro el Excel.
        excel = new ExcelApp();
        excel.Visible = false;
        excel.DisplayAlerts = false;
        excel.ErrorCheckingOptions.NumberAsText = false;

        // Abro el libro.
        Workbook libro = excel.Workbooks.Add();

        // Abro la hoja.
        Worksheet hoja1 = (Worksheet)libro.Worksheets[1];

        // Título.
        int fila = 1;
        hoja1.get_Range(hoja1.Cells[fila, 1], hoja1.Cells[fila, 4 * total]).Merge(true);
        ((Range)hoja1.Cells[fila, 1]).FormulaR1C1 = String.Format("Vencimientos {0:00}/{1:0000}", mes, anio);
        ((Range)hoja1.Cells[fila, 1]).Font.Bold = true;
        ((Range)hoja1.Cells[fila, 1]).Font.Size = 12;
        ((Range)hoja1.Cells[fila, 1]).HorizontalAlignment = 3;

        // Completo los datos.
        int x = 0;
        int columna = 1;
        foreach (string documento in vencimientos.Keys)
        {
            fila = FilaInicio;
            columna = 2 + x * 3;
            hoja1.get_Range(hoja1.Cells[fila, columna], hoja1.Cells[fila, columna + 1]).Merge(true);
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = documento;
            ((Range)hoja1.Cells[fila, columna]).Interior.Color = Color.Gray.ToArgb();
            ((Range)hoja1.Cells[fila, columna]).Font.Color = Color.White.ToArgb();
            ((Range)hoja1.Cells[fila, columna]).Font.Bold = true;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            fila++;

            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "Fecha";
            ((Range)hoja1.Cells[fila, columna]).Interior.Color = Color.Gray.ToArgb();
            ((Range)hoja1.Cells[fila, columna]).Font.Color = Color.White.ToArgb();
            ((Range)hoja1.Cells[fila, columna]).Font.Bold = true;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            ((Range)hoja1.Cells[fila, columna + 1]).FormulaR1C1 = "Dominio";
            ((Range)hoja1.Cells[fila, columna + 1]).Interior.Color = Color.Gray.ToArgb();
            ((Range)hoja1.Cells[fila, columna + 1]).Font.Color = Color.White.ToArgb();
            ((Range)hoja1.Cells[fila, columna + 1]).Font.Bold = true;
            ((Range)hoja1.Cells[fila, columna + 1]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna + 1]).HorizontalAlignment = 3;

            foreach (ItemVencimiento item in vencimientos[documento])
            {
                fila++;
                ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + item.Fecha.ToShortDateString();
                ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna + 1]).FormulaR1C1 = item.Patente;
                ((Range)hoja1.Cells[fila, columna + 1]).Font.Size = 10;
                ((Range)hoja1.Cells[fila, columna + 1]).HorizontalAlignment = 3;
            }

            x++;
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
    /// <summary>
    /// Exporta los vehículos a excel.
    /// </summary>
    public static string ExportarVehiculos()
    {
        ExcelApp excel;
        const int FilaInicio = 5;

        List<Vehiculo> vehiculos = GetVehiculos();
        int total = vehiculos.Count;
        if (total == 0)
        {
            throw new Exception("No hay vehículos disponibles.");
        }

        Random r = new Random(DateTime.Now.Millisecond);
        string path = Constantes.PATH_TEMP + "archivo" + r.Next() + ".xls";
        bool celda_con_color = false;

        // Abro el Excel.
        excel = new ExcelApp();
        excel.Visible = false;
        excel.DisplayAlerts = false;
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
        ((Range)hoja1.Cells[fila, 7]).FormulaR1C1 = "Afectado a GasMed";
        ((Range)hoja1.Cells[fila, 8]).FormulaR1C1 = "Vto. Cédula Verde";
        ((Range)hoja1.Cells[fila, 9]).FormulaR1C1 = "Nº R.U.T.A.";
        ((Range)hoja1.Cells[fila, 10]).FormulaR1C1 = "Vto. R.U.T.A.";
        ((Range)hoja1.Cells[fila, 11]).FormulaR1C1 = "Vto. V.T.V.";
        ((Range)hoja1.Cells[fila, 12]).FormulaR1C1 = "Vto. Patente";
        ((Range)hoja1.Cells[fila, 13]).FormulaR1C1 = "Compañía Seguro";
        ((Range)hoja1.Cells[fila, 14]).FormulaR1C1 = "Póliza Seguro";
        ((Range)hoja1.Cells[fila, 15]).FormulaR1C1 = "Vto. Seguro";
        ((Range)hoja1.Cells[fila, 16]).FormulaR1C1 = "Nº de Chasis";
        ((Range)hoja1.Cells[fila, 17]).FormulaR1C1 = "Nº de Motor";

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
        ((Range)hoja1.Cells[fila, 13]).ColumnWidth = 15.99;
        ((Range)hoja1.Cells[fila, 14]).ColumnWidth = 12.61;
        ((Range)hoja1.Cells[fila, 15]).ColumnWidth = 10.66;
        ((Range)hoja1.Cells[fila, 16]).ColumnWidth = 18.85;
        ((Range)hoja1.Cells[fila, 17]).ColumnWidth = 18.85;

        // Completo los datos.
        fila = FilaInicio;
        int columna;
        foreach (Vehiculo vehiculo in vehiculos)
        {
            columna = 1;

            // Dominio.
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.Patente;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Modelo.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.Modelo;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Tipo.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.TipoVehiculo.Descripcion;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Año.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.Anio;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Ubicación.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.Ubicacion;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Responsable.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = GPersonal.GetPersona(vehiculo.IdResponsable).Nombre;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Afectado a gasmed.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.AfectadoGasmed ? "Si" : "No";
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Vencimiento Cédula Verde.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + vehiculo.VtoCedulaVerde.ToShortDateString();
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Nº RUTA.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.NroRUTA;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Vto RUTA.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + vehiculo.VtoRUTA.ToShortDateString();
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Vencimiento VTV.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + vehiculo.VtoVTV.ToShortDateString();
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Vencimiento Patente.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + vehiculo.VtoPatente.ToShortDateString();
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Cia Seguro.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.CiaSeguro;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Póliza seguro.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.PolizaSeguro;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Vto Seguro.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = "'" + vehiculo.VtoSeguro.ToShortDateString();
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Nº chasis.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.NroChasis;
            ((Range)hoja1.Cells[fila, columna]).Font.Size = 10;
            ((Range)hoja1.Cells[fila, columna]).HorizontalAlignment = 3;

            // Nº Motor.
            columna++;
            ((Range)hoja1.Cells[fila, columna]).FormulaR1C1 = vehiculo.NroMotor;
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
    /// <summary>
    /// Informa a los responsables sobre los vencimientos del próximo més.
    /// </summary>
    public static void InformarVencimientosMes()
    {
        DateTime mes = DateTime.Now.AddMonths(1);

        if (mes.Day != 25)
        {
            return;
        }

        string doc = ExportarVencimientos(mes.Month, mes.Year);

        Email email = new Email(Constantes.EmailIntranet, "martin.duran@servaind.com", "", String.Format("Vencimientos {0:00}/{1:0000}", mes.Month, mes.Year),
            String.Format("Se adjunta un archivo con los vencimientos correspondientes al {0:00}/{1:0000}.", mes.Month, mes.Year), new List<AdjuntoEmail>() { new AdjuntoEmail(doc, String.Format("Vencimientos {0:00}-{1:0000}.xls", mes.Month, mes.Year)) });
        email.Enviar();
    }
}