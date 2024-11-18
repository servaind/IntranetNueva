using System;
using System.ComponentModel;

namespace Servaind.Intranet.Core
{
    public enum ElementType
    {
        ImagenInstrumento = 0
    }



    public enum EstadosLicencia
    {
        NoRecibida = 0, RechazadaResponsable = 1, AprobadaResponsable = 2, RechazadaRRHH = 3, Confirmada = 4
    }
    public enum TiposLicencia
    {
        Vacaciones = 0, Franco = 1, Examen = 2, SinGoceHaberes = 3, Casamiento = 4, Nacimiento = 5, 
        ModificacionHorario = 6, LicenciaSinAviso = 7, Presente = 99
    }
    public enum EstadosParteDiario
    {
        Presente = 0, Licencia = 1
    }
    public enum EstadosVDM
    {
        Enviada = 0, RecibidaResponsable = 1, AprobadaResponsable = 2, RecibidaDeposito = 3, EntregadaDeposito = 4, 
        RechazadaResponsable = 5
    }
    public enum EstadosNC
    {
        ProcesandoSGI = 0, ProcesandoImputado = 1, EsperandoCierre = 2,
        Cerrada = 3, NoCorresponde = 4
    }
    public enum ConclusionesNC
    {
        Satisfactoria = 1,
        EnProceso = 3,
        NoCorresponde = 2
    }
    public enum CategoriasNC
    {
        NotaNoConformidad = 0,
        Observacion = 1,
        OportunidadMejora = 2,
        Stock = 3,
        NoCorresponde = 4
    }
    public enum FiltrosNC
    {
        Asunto = 0, Categoria = 1, Area = 2, Estado = 3, EmitidaPor = 4, Numero = 5
    }
    public enum FiltrosVDM
    {
        Codigo = 0, Imputacion = 1, Solicito = 2, Estado = 3
    }
    public enum TIPO_FILTRO_SEM
    {
        Todas = 0
    }
    public enum TIPO_FILTRO_SECCION
    {
        Todas,
        Personal
    }
    public enum FiltrosHerramienta
    {
        Marca = 0,
        Descripcion = 1,
        PersonaCargo = 2,
        Tipo = 3,
        NumeroHerramienta = 4,
        NumeroInstrumento = 5,
        Clasificacion = 6
    }
    public enum FrecCalHerramienta
    {
        Mensual = 0,
        Bimestral = 1,
        UnAno = 2,
        DosAnos = 3
    }
    public enum TiposCalHerramienta
    {
        Externa = 0,
        Interna = 1
    }
    public enum FiltrosSolViaje
    {
        Solicito = 0,
        Destino = 1,
        Imputacion = 2,
        Vehiculo = 3,
        Estado = 4
    }
    public enum VehiculosSolViaje 
    { 
        Moto = 0, Taxi = 1, Flete = 2, Auto = 3
    }
    public enum ImporanciasSolViaje 
    { 
        Baja = 0, Normal = 1, Alta = 2 
    }
    public enum EstadosSolViaje
    { 
        Enviada = 0, Leida = 1, Aprobada = 2, Confirmada = 3, Cancelada = 4 
    }
    public enum EstadosCodArt
    {
        Revision = 0,
        Rechazado = 1,
        Aprobado = 2,
        NoCorresponde = 3
    }
    public enum FiltrosCodArt
    {
        Solicito = 0,
        Estado = 1
    }
    public enum FiltrosImputacion
    {
        Numero = 0,
        Descripcion = 1,
        Estado = 2
    }
    public enum EstadosImputacion
    {
        Activa = 1,
        Inactiva = 0
    }
    public enum EstadosPersona
    {
        [Description("Activa")]
        Activa = 1,
        [Description("Inactiva")]
        Inactiva = 0
    }
    public enum FiltrosPersona
    {
        Id = 0,
        Nombre = 1,
        Usuario = 2,
        Responsable = 3,
        EnPanelControl = 4,
        Estado = 5
    }
    public enum PermisosPersona
    {
        // Administración del sistema.
        Administrador = 0,
        AdminImputaciones = 1,
        AdminPanelesControlPD = 2,
        AdminPersonal = 3,
        AdminPartesDiarios = 4,
        ControlAcceso = 5,

        Publico = 0x0A,

        // Vale de Materiales.
        ValeMaterialesRecibeResp = 10,
        ValeMaterialesApruebaResp = 11,
        ValeMaterialesRecibeDep = 12,
        ValeMaterialesEntrega = 13,
        ValeMaterialesVer = 14,
        ValeMaterialesInforme = 15,

        // Solicitud de Envío de Materiales.
        SolEnvMatVer = 20,
        SolEnvMatGuardar = 21,
        SolEnvMatConfirmar = 22,
        SolEnvMatCerrar = 23,

        // Nota de No Conformidad.
        NNCAdministrador = 30,
        NNCVer = 31,
        NNCEditar = 32,
        NNCResponsable = 33,
        
        // Solicitud de Viajes.
        SolViajeVer = 40,
        SolViajeEditar = 41,

        // Repositorio de archivos.
        RDACrear = 50,
        RDAVer = 51,

        // Stock.
        CotizadorOnLine = 60,
        CotizadorOnLineEquipos = 61,
        ControlHerramientas = 62,
        StockVer = 63,
        StockIngreso = 64,
        StockEgreso = 65,

        // Administración de personal.
        LicRRHH = 70,  // RR.HH.
        ADP_CargaEntrada,
        ADP_PanelControlAsistencia,
        ADP_CargaSalida,
        ADP_PcResponsable,

        // Solicitud de alta de artículos.
        SAAResponsable = 80,

        // Herramientas.
        HerramientaAdministrador = 91,
        InstrumentoSeguimiento = 92,
        InstrumentoNotifCambio = 93,

        // Paneles de control.
        PCVerGeneral = 100,

        // Información interna de obra.
        IIOGenerar = 110,
        IIOVer = 111,

        // Roles.
        RolDireccion = 120,
        RolGerencia = 121,

        // SSM.
        SSM_Admin = 130,

        // Vehículos.
        VehicAdmin = 140,
        VehicVer = 141,
        VehicAlerta = 142,

        // Autorizaciones.
        AutorizAdministrar = 150,

        // Sistema de Notificación de Ventas.
        SNV_Visualizacion = 160,
        SNV_Vendedor,
        SNV_AltaFacRem,
        SNV_AltaCliente,
        SNV_AltaImputacion,
        SNV_NotifOC,
        SNV_NotifCierre,
        SNV_NotifRecordatorio,
        SNV_AltaTransporte,
        SNV_NotifProducto,
        SNV_NotifRMA,

        // General.
        GEN_CargaParteDiario = 180,

        // SGI.
        SGI_Responsable = 190,

        // Calidad
        CAL_Responsable = 191
    }
    public enum ESTADO_SEM 
    { 
        EsperandoConfirmacion = 0, Confirmada = 1, Cerrada = 2, Rechazada = 3
    }
    public enum TIPO_TRANSP_SEM 
    { 
        Expreso = 0, Interno =1 
    }
    public enum TIPO_CAUSA_SEM
    {
        Reemplazo = 0,
        Falla = 1,
        Otros = 2
    }
    public enum ORIGEN_ITEM_SEM
    {
        SM = 0,
        VDM = 1
    }
    public enum BDConexiones
    {
        Intranet,
        Tango,
        Proser
    }
    public enum PermisoRepositorio
    {
        Lectura = 0,
        Escritura = 1
    }
    public enum TiposTrabajoObra
    {
        Obra = 0,
        Mantenimiento = 1,
        Auditoria = 2
    }
    public enum FiltrosInformeObra
    {
        NumObra = 0,
        Cliente = 1,
        Responsable = 2,
        Informante = 3,
        OrdenCompra = 4,
        Imputacion = 5
    }
    public enum TiposBinario
    {
        No = 0,
        Si = 1
    }
    public enum EstadosSitio
    {
        NoCumplido = 0,
        Cumplido = 1,
        NoAplica = 2
    }
    public enum FrecuenciasSitio
    {
        Mensual = 0,
        Anual = 1
    }
    public enum RepositoriosArchivos
    {
        [Description(@"\\" + Constantes.SERVIDOR1 + @"\repositorio")]
        Comun = 0,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI")]
        SGI_Multisitio = 1,
        [Description(@"\\" + Constantes.SERVIDOR1 + @"\repositorio")]
        SGI_BuenosAires = 2,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\GAS")]
        SGI_Gas = 3,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\LIQUIDOS")]
        SGI_Liquidos = 4,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\VÁLVULAS")]
        SGI_Valvulas = 5,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\BOLIVIA")]
        SGI_Bolivia = 6,
        [Description(@"\\" + Constantes.SERVIDOR1 + @"\Repositorio\Publico\petrobras")]
        Petrobras = 7,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\Manual de SGI")]
        Manual_SGI = 8,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\Política SGI")]
        Politica_SGI = 9,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\Política Alcohol y drogas")]
        Politica_Alcohol_Drogas = 10,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\Certificaciones")]
        Certificaciones = 11,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\f.Compras")]
        SGI_BsAs_Compras = 12,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\e.Desarrollo")]
        SGI_BsAs_Desarrollo = 13,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\i.Informatica")]
        SGI_BsAs_Informatica = 14,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\c.Ingeniería")]
        SGI_BsAs_Ingenieria = 15,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\d.GAS Y LÍQUIDOS")]
        SGI_BsAs_Mantenimiento = 16,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\h.Obras")]
        SGI_BsAs_Obras = 17,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\a.RRHH")]
        SGI_BsAs_RRHH = 18,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\g.SEGURIDAD E HIGIENE")]
        SGI_BsAs_Seguridad_Higiene = 19,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\b.COMERCIAL")]
        SGI_BsAs_Ventas = 20,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\l.LABORATORIO")]
        SGI_BsAs_Metrologia = 21,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\Organigrama")]
        SGI_BsAs_Organigramas = 22,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\i. Proyectos")]
        SGI_BsAs_Proyectos = 23,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\j.ADM. Y FINANZAS")]
        SGI_BsAs_AdminFinanz = 24,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\K.DEPÓSITO")]
        SGI_BsAs_Deposito = 25,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI\a. Políticas\POLÍTICA SGI")]
        SGI_Politica_SGI = 26,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI\b. Manual de SGI")]
        SGI_Manual_SGI = 27,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI\a. Políticas\POLÍTICA ALCOHOL Y DROGAS")]
        SGI_Politica_Alcohol_Drogas = 28,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI\d. Certificaciones")]
        SGI_Certificaciones = 29,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI\e. Normas")]
        SGI_Normas = 30,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\a.SGI\f. Procedimientos SGI")]
        SGI_Procedimientos_SGI = 31,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\b.Medio Ambiente\MANEJO DE RESIDUOS")]
        MA_ControlResiduos = 32,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\b.Medio Ambiente\EMERGENCIAS AMBIENTALES")]
        MA_Emergencias_Ambientales = 33,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\b.Medio Ambiente\ACTUACIÓN ANTE DERRAMES")]
        MA_Actuacion_Derrames = 34,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\c.Seguridad\MATRIZ DE RIESGOS")]
        SEG_Matriz = 35,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\c.Seguridad\INVESTIGACIÓN DE INCIDENTES")]
        SEG_Investigacion_Incidentes = 36,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\c.Seguridad\EPP")]
        SEG_EPP = 37,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\c.Seguridad\SEGURIDAD Y SALUD EN LAS OPERACIONES")]
        SEG_Seguridad_Salud_Operaciones = 38,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\c.Seguridad\PLAN DE EMERGENCIAS")]
        SEG_Plan_Emergencias = 39,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\d.RRHH\Manual del Empleado")]
        RRHH_Manual_Empleado = 40,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\d.RRHH\Organigrama")]
        RRHH_Organigrama = 41,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\d.RRHH\Capacitaciones")]
        RRHH_Registro_Capacitacion = 42,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\e.Doc Externos\Lista de Doc Externos (FG-004)")]
        DE_Lista_Doc_Ext = 43,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\e.Doc Externos\Documentos Externos")]
        DE_Doc_Ext = 44,
        [Description(@"\\" + Constantes.SERVIDOR1 + @"\Repositorio\ITR")]
        ITR = 45,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\m.SGI")]
        SGI = 46,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\o.MEDIO AMBIENTE")]
        MedioAmbiente = 47,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\g.SEGURIDAD E HIGIENE")]
        Seguridad = 48,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\a.RRHH")]
        RRHH = 49,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\h. MATRIZ LEGAL INTERNA")]
        Mat_Leg_Int = 50,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\m.SGI")]
        DOC_SGI = 51,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\o.MEDIO AMBIENTE")]
        DOC_MedioAmbiente = 52,
        [Description(@"\\" + Constantes.SERVER_STORAGE + @"\Calidad\f. DOCUMENTACION AREAS\a.RRHH")]
        DOC_RRHH = 53
    }
    public enum ClasifHerramienta
    {
        Herramienta = 0,
        Instrumento = 1
    }
    public enum PathImage
    {
        ListadoInstrumentos = 0
    }
    public enum PathDescargas
    {
        ListadoInstrumentos = 0,
        ITR,
        OC
    }
    public enum TipoListadoHerramientas
    {
        Herramientas = 0,
        Instrumentos = 1
    }
    public enum TipoAlertaVencimiento
    {
        Precaucion,
        Vencido,
        Recordatorio
    }
    public enum EstadoAutorizacion
    {
        Pendiente = 0,
        Aprobada,
        Rechazada
    }
    public enum SeccionAutorizacion
    {
        InformacionInternaObra = 0
    }
    public enum FiltrosAutorizacion
    {
        Solicito = 0,
        Estado,
        Referencia,
        Responsable
    }
    public enum StockOperaciones
    {
        Ingreso = 0,
        Egreso
    }
    public enum FiltrosArticuloStock
    {
    
    }
    public enum FrecuenciaCalibracion
    {
        Mensual = 0,
        Anual,
        DosAnios
    }
    public enum FiltroInstrumento
    {
        Activo,
        Todo,
        Calib,
        CalibProx,
        CalibVencido,
        Mant,
        MantProx,
        MantVencido,
        BusquedaExtendida
    }

    public enum EstadoRegInstrumento
    {
        [Description("Vigente")]
        Activo = 1,
        [Description("Próx. a vencer")]
        ProxVencer,
        [Description("Vencida")]
        Vencido
    }

    public enum EstadoAsistencia
    {
        Ausente = 0,
        Presente,
        Licencia,
        AusenteArt,
        AusentePmc,
        AusenteFall,
        AusenteFeriado,
        FrancoPersonalObras
    }
    public enum ProcesoAsistencia
    {
        SinProcesar = 0,
        Procesada
    }
    public enum Moneda
    {
        Peso,
        Dolar,
        Euro,
        Real
    }
    public enum EstadoNotifVenta
    {
        CargandoDatos = 0,
        CargandoRemito,
        EsperandoAprobacion,
        ConfeccionRem,
        ConfeccionFac,
        EsperandoITR,
        Cerrada,
        Rechazada
    }
    public enum FiltroNotifVenta
    {
        ID,
        Vendedor,
        Cliente,
        OC,
        Imputacion,
        Fecha,
        Factura,
        Remito,
        Estado
    }
    public enum TipoNotifVenta
    {
        Producto = 0,
        Servicio,
        RMA,
        RemitoOficial,
        RemitoInterno,
        Obra
    }
    public enum OportMejoraUrgencia
    {
        [Description("Alta")]
        Alta = 0,
        [Description("Media")]
        Media,
        [Description("Baja")]
        Baja
    }

//FIN TIPOS DE DATO----------------------------------------------------------------------->
}