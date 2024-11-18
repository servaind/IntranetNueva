var currentPage;

$(document).ready(function () {
    $('#btnFiltro').click(function() {
        ShowModal('dlgFiltro');
    });

    $('#btnFiltroAceptar').click(function () {
        List(1);
        CloseModal('dlgFiltro');
    });

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es'
    });

    $('#btnCreate').click(function() {
        location.href = '/Sgi/FormFg005';
    });

    List(1);
});

function List(page) {
    ShowLoading();

    currentPage = page;

    $.ajax({
        url: '/Sgi/FormsFg005',
        type: "POST",
        data: {
            pagina: currentPage,
            numero: $('#txtFiltroNumero').val(),
            categoria: $('#cbFiltroCategoria').val(),
            origen: $('#cbFiltroOrigen').val(),
            areaResponsabilidad: $('#cbFiltroArea').val(),
            asunto: $('#txtFiltroAsunto').val(),
            estado: $('#cbFiltroEstado').val(),
            fechaDesde: $('#txtFiltroFechaDesde').val(),
            fechaHasta: $('#txtFiltroFechaHasta').val(),
            cierreDesde: $('#txtFiltroCierreDesde').val(),
            cierreHasta: $('#txtFiltroCierreHasta').val(),
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            var c = r.Items.length;

            var rows = [];

            if (c == 0) rows.push('<tr><td colspan="7" class="text-center">No hay datos disponibles.</td></tr>');
            else for (var i = 0; i < c; i++) rows.push(GetListRow(r.Items[i]));

            $('#list-container').html(rows.join(''));

            $('#list-pager').html(DrawPager(page, r.TotalPages));

            CloseLoading();
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}

function GetListRow(i) {
    var result = '';

    result += '<tr onclick="OpenForm(' + i.Numero + ');">';
    result += '<td class="text-center">' + i.NumeroStr + '</td>';
    result += '<td class="text-center">' + i.Categoria + '</td>';
    result += '<td class="text-center">' + i.Asunto + '</td>';
    result += '<td class="text-center">' + i.AreaResponsabilidad + '</td>';
    result += '<td class="text-center">' + i.Fecha + '</td>';
    result += '<td class="text-center">' + i.FechaCierre + '</td>';
    result += '<td class="text-center"><label class="label label-sm label-' + (i.Cerrada ? 'success' : 'info') + '">' + i.EstadoStr + '</label></td>';
    result += '</tr>';

    return result;
}

function OpenForm(numero) {
    var url = 'Sgi/FormFg005/' + numero;
    OpenLink(url);
}

function ExportarCsv(all) {
    ShowSuccess('La operacion se esta procesando.');

    var url = 'Sgi/FormsFg005ToCsv/';
    url += '?numero=' + $('#txtFiltroNumero').val();
    url += '&categoria=' + $('#cbFiltroCategoria').val();
    url += '&origen=' + $('#cbFiltroOrigen').val();
    url += '&areaResponsabilidad=' + $('#cbFiltroArea').val();
    url += '&asunto=' + $('#txtFiltroAsunto').val();
    url += '&estado=' + $('#cbFiltroEstado').val();
    url += '&fechaDesde=' + $('#txtFiltroFechaDesde').val();
    url += '&fechaHasta=' + $('#txtFiltroFechaHasta').val();
    url += '&cierreDesde=' + $('#txtFiltroCierreDesde').val();
    url += '&cierreHasta=' + $('#txtFiltroCierreHasta').val();
    url += '&todos=' + (all ? 1 : 0);

    OpenLink(url);
}