var currentId;

function GetEstadoCelda(ultima, frec, vencida, proxVencer) {
    var c = vencida ? 'danger' : (proxVencer ? 'warning' : 'success');
    var t = vencida ? 'vencida' : (proxVencer ? 'próx. a vencer' : 'vigente');

    var result = 'Último: ' + ultima + '<br/>';
    result += 'Frecuencia: ' + frec + ' mes' + (frec != 1 ? 'es' : '') + '.';
    result += '<br/><div class="clearfix margin-bottom-5"></div><center><span class="label label-sm label-' + c + '">' + t + '</span></center>';

    return result;
}

function ShowDetalle(id) {
    var s = $('#show-detalle-' + id);
    var t = $('#vehiculo-detalle-' + id);

    if (s.hasClass('row-details-close')) {
        s.removeClass('row-details-close').addClass('row-details-open');
        t.show();
    } else {
        s.removeClass('row-details-open').addClass('row-details-close');
        t.hide();
    }
}

function FinishEdit(id) {
    $.ajax({
        url: '/Vehiculos/Info/' + id,
        type: "POST",
        async: true,
        cache: false,
        success: function (r) {
            if (r != null) {
                $('#vehiculo-detalle-' + id).remove();
                $('#vehiculo-' + id).replaceWith(r);
            }

            CloseLoading();
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
        }
    });
}

function Edit(id) {
    ShowLoading();

    $.ajax({
        url: '/Vehiculos/?id=' + id,
        type: "POST",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r == null) {
                return;
            }

            $('#dlgVehiculoTitle').text('Editar ' + r.Patente);
            $("#cbUbicacion option").filter(function () {
                return this.text == r.Ubicacion;
            }).attr('selected', true);
            $("#cbResponsable option").filter(function () {
                return this.text == r.Responsable;
            }).attr('selected', true);

            UpdateDateInput(r.VtoCertIzaje, 'txtVtoCertIzaje');
            UpdateDateInput(r.VtoCedulaVerde, 'txtVtoCedulaVerde');
            $('#txtNroRuta').val(r.NroRuta);
            UpdateDateInput(r.VtoRuta, 'txtVtoRuta');
            UpdateDateInput(r.VtoVtv, 'txtVtoVtv');
            UpdateDateInput(r.VtoPatente, 'txtVtoPatente');
            UpdateDateInput(r.VtoStaCruz, 'txtVtoStaCruz');
            $('#txtCiaSeguro').val(r.CiaSeguro);
            $('#txtPolizaSeguro').val(r.PolizaSeguro);
            UpdateDateInput(r.VtoSeguro, 'txtVtoSeguro');

            $('.chkNoAplica').change();

            ShowModal('dlgVehiculo');
            RefreshCheckboxUI();

            currentId = id;
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
        }
    });
}

$(document).ready(function() {
    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es'
    });

    $('.chkNoAplica').change(function () {
        var t = $('#' + $(this).attr('target')).parent('.input-group');
        
        if ($(this).is(':checked')) {
            t.hide();
        } else {
            t.val(fechaHoy);
            t.show();
        }
    });

    $('#btnVehiculoSave').click(Save);

    $('#cbFiltroUbicacion').change(function () {
        location.href = '/Vehiculos/?uid=' + $(this).val();
    });

    ShowNotif();
});

function UpdateDateInput(f, t) {
    var vto = f != fechaInvalida;
    $('.chkNoAplica[target=' + t + ']').prop('checked', !vto);
    $('#' + t).val(!vto ? fechaHoy : f);
}

function GetDateInput(t) {
    var chk = $('.chkNoAplica[target=' + t + ']');
    return chk.is(':checked') ? fechaInvalida : $('#' + t).val();
}

function Save() {
    ShowLoading();

    $.ajax({
        url: '/Vehiculos/Update/' + currentId,
        data: {
            ubicacion: $('#cbUbicacion').val(), responsableId: $('#cbResponsable').val(),
            vtoCertIzaje: GetDateInput('txtVtoCertIzaje'), vtoCedulaVerde: GetDateInput('txtVtoCedulaVerde'),
            nroRuta: $('#txtNroRuta').val(), vtoRuta: GetDateInput('txtVtoRuta'),
            vtoVtv: GetDateInput('txtVtoVtv'), vtoPatente: GetDateInput('txtVtoPatente'),
            vtoStaCruz: GetDateInput('txtVtoStaCruz'), ciaSeguro: $('#txtCiaSeguro').val(),
            polizaSeguro: parseInt($('#txtPolizaSeguro').val()), vtoSeguro: GetDateInput('txtVtoSeguro'),
        },
        type: "POST",
        async: true,
        cache: false,
        success: function (r) {
            if (r.Success) {
                CloseModal('dlgVehiculo');
                FinishEdit(currentId);
                ShowSuccess('Los cambios fueron guardados.');
            } else {
                CloseLoading();
                ShowError(r.Message);
            }

            currentId = null;
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
        }
    });
}

function ShowNotif() {
    var settings = {
        theme: 'ruby',
        sticky: true,
        horizontalEdge: 'right',
        verticalEdge: 'top',
        heading: 'IMPORTANTE'
    };

    $.notific8('zindex', 11500);
    $.notific8('Al renovar RUTA (nacional), renovar de forma unificada con RUTA provincial.', settings);
}