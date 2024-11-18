$(document).ready(function() {
    $('#btnSave').click(Enviar);

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es',
        startDate: new Date()
    });

    $('input[type=checkbox]').change(function () {
        var target = $('#' + $(this).attr('id').replace('Nor', 'Apa').replace('chk', 'txt'));
        var enabled = $(this).is(':checked');

        if (enabled) target.removeAttr('disabled');
        else target.attr('disabled', 'disabled');

        target.val('');
    }).change();
});

function Enviar() {
    var areaResponsabilidadId = $('#cbAreaResponsabilidad').val();
    var categoriaId = $('#cbCategoria').val();
    

    if (areaResponsabilidadId.length == 0) {
        ShowError('No se ha seleccionado un Area de Responsabilidad.');
        return;
    }

    if (categoriaId.length == 0) {
        ShowError('No se ha seleccionado una Categoria.');
        return;
    }

    ShowLoading();

    $.ajax({
        url: '/Sgi/FormFg005ProcesandoSgi/' + formNumero,
        type: "POST",
        data: {
            norIso9001: $('#chkNorIso9001').is(':checked'),
            apaIso9001: $('#txtApaIso9001').val(),
            norIso14001: $('#chkNorIso14001').is(':checked'),
            apaIso14001: $('#txtApaIso14001').val(),
            norOhsas18001: $('#chkNorOhsas18001').is(':checked'),
            apaOhsas18001: $('#txtApaOhsas18001').val(),
            norIram301: $('#chkNorIram301').is(':checked'),
            apaIram301: $('#txtApaIram301').val(),
            areaResponsabilidadId: parseInt(areaResponsabilidadId),
            categoriaId: parseInt(categoriaId)
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