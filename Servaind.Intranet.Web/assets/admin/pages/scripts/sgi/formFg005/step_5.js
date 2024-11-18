var fCount;
var fCurrent;
var fFails;

$(document).ready(function () {
    $('#btnClose').click(Close);

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        disableImageResize: true,
        autoUpload: false,
        maxFileSize: 500000000,
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},                
    });

    // Load & display existing files:
    $('#fileupload').addClass('fileupload-processing')
        .bind('fileuploadfail', UploadFail)
        .bind('fileuploaddone', UploadDone);
    $.ajax({
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},
        url: $('#fileupload').attr("action"),
        dataType: 'json',
        context: $('#fileupload')[0]
    }).always(function () {
        $(this).removeClass('fileupload-processing');
    }).done(function (result) {
        $(this).fileupload('option', 'done')
        .call(this, $.Event('done'), { result: result });
    });
});

function Close() {
    fCount = $('.template-upload').size();
    fCurrent = 0;
    fFails = 0;

    var evResultados = $('#txtEvRes').val();
    var evRevision = $('#txtEvRev').val();

    if ($.trim(evResultados).length == 0) {
        ShowError('No se ha ingresado la Evidencia de resultados de acciones tomadas.');
        return;
    }

    if ($.trim(evRevision).length == 0) {
        ShowError('No se ha ingresado la Evidencia de revisión de eficacia de acciones tomadas.');
        return;
    }

    ShowLoading();

    $.ajax({
        url: '/Sgi/FormFg005Cerrar/' + formNumero,
        type: "POST",
        data: {
            evResultados: evResultados,
            evRevision: evRevision,
            cierreId: $('#cbCierre').val(),
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r.Success) {
                if (fCount != 0) UploadFiles();
                else ShowSuccess('Los cambios fueron guardados.', GoToList);
            }
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}

function UploadFiles() {
    $('#btnStartUpload').click();
}

function UploadFail() {
    fFails++;
    if (++fCurrent == fCount) UploadFinished();
}

function UploadDone() {
    if (++fCurrent == fCount) UploadFinished();
}

function UploadFinished() {
    if (fFails == 0) ShowSuccess('Los cambios fueron guardados.', GoToList);
    else ShowError('Algunos archivos no pudieron ser cargados, por favor, intente nuevamente.');
}