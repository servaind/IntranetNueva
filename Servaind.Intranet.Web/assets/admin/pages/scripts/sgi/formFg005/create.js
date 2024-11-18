$(document).ready(function() {
    $('#btnEnviar').click(Enviar);
	$('#btnCancel').click(GoToList);
});

function Enviar() {
    var origenId = $('#cbOrigen').val();
    var asunto = $('#txtAsunto').val();
    var hallazgo = $('#txtHallazgo').val();
    var accInmediata = $('#txtAccInmediata').val();
    var comentarios = $('#txtComentarios').val();

    if ($('#cbOrigen option:selected').text().length == 0 || asunto.length == 0 || hallazgo.length == 0 ||
        accInmediata.length == 0) {
        ShowError('Todos los campos (excepto Comentarios) deben ser completados.');
        return;
    }

    ShowLoading();

    $.ajax({
        url: '/Sgi/FormFg005Create/',
        type: "POST",
        data: {
            origenId : origenId, asunto: asunto, hallazgo: hallazgo, accInmediata: accInmediata, comentarios: comentarios
        },
        dataType: "json",
        async: true,
        cache: false,
        traditional: true,
        success: function (r) {
            CloseLoading();

            if (r.Success) {
                ShowSuccess('La solicitud fue enviada con el numero ' + r.Message + '.', GoToList);
            }
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });

    GoToList();
}

function GoToList() {
    location.href = '/Sgi/FormsFg005';
}
