$(document).ready(function() {
    $('#btnSave').click(Enviar);

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es',
        startDate: new Date()
    });
});

function Enviar() {
    var causasRaices = $('#txtCausasRaices').val();
    var accCorr = $('#txtAccCorr').val();
    var accCorrRespId = $('#cbAccCorrResp').val();
    var accCorrFecha = $('#txtAccCorrFechaFin').val();
    var accPrev = $('#txtAccPrev').val();
    var accPrevRespId = $('#cbAccPrevResp').val();
    var accPrevFecha = $('#txtAccPrevFechaFin').val();

    if ($.trim(causasRaices).length == 0) {
        ShowError('No se ha ingresado la Definicion de' +
            ' Causas Raices.');
        return;
    }

    if ($.trim(accCorr).length == 0) {
        ShowError('No se ha ingresado una Accion Correctiva.');
        return;
    }

    if (accCorrRespId.length == 0) {
        ShowError('No se ha seleccionado un Responsable para la Accion Correctiva.');
        return;
    }

    if ($('#divAccPrev').css('display') != 'none') {
        if ($.trim(accPrev).length == 0) {
            ShowError('No se ha ingresado una Accion Preventiva.');
            return;
        }

        if (accPrevRespId.length == 0) {
            ShowError('No se ha seleccionado un Responsable para la Accion Preventiva.');
            return;
        }
    } else {
        accPrevRespId = INVALID_PERSONA_ID;
    }

    ShowLoading();

    $.ajax({
        url: '/Sgi/FormFg005ProcesandoResponsable/' + formNumero,
        type: "POST",
        data: {
            causasRaices: causasRaices,
            accCorr: accCorr,
            accCorrRespId: accCorrRespId,
            accCorrFechaFin: accCorrFecha,
            accPrev: accPrev,
            accPrevRespId: accPrevRespId,
            accPrevFechaFin: accPrevFecha
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r.Success) ShowSuccess('Los cambios fueron guardados.', GoToList);
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}