$(document).ready(function() {
    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        disableImageResize: true,
        autoUpload: false,
        maxFileSize: 8388608,
        acceptFileTypes: /(\.|\/)(mov|doc|docx|xls|xlsx|pdf|ppt|pptx|gif|jpe?g|png)$/i,
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},
        done: function (e, d) {
            Sent();
        }
    });

    // Load & display existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
            // Uncomment the following to send cross-domain cookies:
            //xhrFields: {withCredentials: true},
            url: $('#fileupload').attr("action"),
            dataType: 'json',
            context: $('#fileupload')[0]
        }).always(function() {
            $(this).removeClass('fileupload-processing');
        })
        .done(function(result) {
            $(this).fileupload('option', 'done')
                .call(this, $.Event('done'), { result: result });
        });

    $('#btnEnviar').click(Send);
});

function ParseUrl() {
    var url = '/Sgi/OportMejora/';
    url += '?areaId=' + $('#cbArea').val();
    url += '&responsableId=' + $('#cbResponsable').val();
    url += '&comentarios=' + $('#txtComentarios').val();
    url += '&urgenciaId=' + $('#cbUrgencia').val();

    return url;
}

function Send() {
    var desc = $('#txtComentarios').val();

    if ($.trim(desc).length == 0) {
        ShowError('No se ha ingresado un Cambio propuesto.');
        return;
    }

    if (desc.length > 3000) {
        ShowError('El Cambio propuesto supera los 3000 caracteres.');
        return;
    }

    if ($('.template-upload').length > 0 && $('.label-danger').text().length > 0) return;

    ShowLoading();
    
    if ($('.template-upload').length > 0) {
        $('#fileupload').attr('action', ParseUrl());

        $('#btnConfirmSend').click();
    }
    else SendManual();
}

function Sent() {
    CloseLoading();

    ShowSuccess('La propuesta de cambio fue enviada.', Finish);
}

function SendManual() {
    $.ajax({
        url: ParseUrl(),
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r.Success) Sent();
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            ShowError(ERR_SERVER);
        }
    });
}

function Finish() {
    location.href = '/Sgi';
}