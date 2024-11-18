$(document).ready(function() {
    $('#btnReject').click(function() {
        Enviar(false);
    });
    $('#btnAprove').click(function () {
        Enviar(true);
    });
});

function Enviar(aprobar) {
    ShowLoading();

    $.ajax({
        url: '/Sgi/FormFg005ValidandoSgi/' + formNumero,
        type: "POST",
        data: {
            aprobar: aprobar
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r.Success) ShowSuccess('Los cambios fueron guardados.', aprobar ? RefreshPage : GoToList);
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}