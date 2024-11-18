var currentPath;

$(document).ready(function () {
    $('#btnFolderAdd').click(ShowCreateCarpeta);
    $('#btnFileAdd').click(ShowUploadFile);

    currentPath = [];

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        disableImageResize: true,
        autoUpload: false,
        maxFileSize: 500000000,
        maxChunkSize: 1024000,
        acceptFileTypes: /(\.|\/)(exe|txt|jp?g|png|gif|zip|rar|iso|xls|doc|xlsx|docx|pdf|ppt|pptx|pps|ppsx)$/i,
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},                
    });

    // Load & display existing files:
    $('#fileupload').addClass('fileupload-processing');
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

    $('#btnFileUploadOk').click(LoadFiles);

    LoadFiles();
});

function ShowCreateCarpeta() {
    bootbox.prompt("Nueva carpeta", function (result) {
        if (result == null) return;

        var path = currentPath.join('\\');

        $.ajax({
            url: '/Repositorio/CreateCarpeta/' + repositorioId,
            type: "POST",
            data: { path: path, nombre: result },
            dataType: "json",
            async: true,
            cache: false,
            success: function (r) {
                if (!r.Success) ShowError(r.Message);
                else ShowSuccess('Carpeta creada.', LoadFiles);
            },
            error: function (data, ajaxOptions, thrownError) {
                ShowError(ERR_SERVER);
            }
        });
    });
}

function ShowUploadFile() {
    var path = currentPath.join('\\');

    $('#fileupload').attr('action', '/Repositorio/UploadFiles/' + repositorioId + '?path=' + path);
    $('#files').html('');

    ShowModal('dlgUploadFile');
}

function LoadFiles() {
    ShowLoading();

    var path = currentPath.join('\\');

    $.ajax({
        url: '/Repositorio/List/' + repositorioId,
        type: "POST",
        data: { path: path },
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (!r.Success) {
                ShowError(r.Message);
                return;
            }

            var lista = [];
            var fila;
            for (var i in r.Items.Carpetas) {
                var nombre = r.Items.Carpetas[i]

                fila = '<tr><td onclick="Navegar(\'' + nombre + '\');"><i class="fa fa-folder-open font-yellow"></i> ' + nombre + '</td><td>Carpeta</td><td></td>';
                fila += '<td class="text-right">';
                if (r.Items.CanEditar) {
                    fila += '<a class="btn default btn-xs green-stripe" href="javascript:UpdateCarpetaNombre(\'' + nombre + '\');" title="Renombrar"><i class="fa fa-font"></i></a>';
                    fila += '<a class="btn default btn-xs red-stripe" href="javascript:DeleteCarpeta(\'' + nombre + '\');" title="Eliminar"><i class="fa fa-trash"></i></a>';
                }
                fila += '</td></tr>';
                lista.push(fila);
            }

            for (var i in r.Items.Archivos) {
                var f = r.Items.Archivos[i];

                fila = '<tr><td><i class="fa fa-' + GetFileIcon(f.Extension) + ' font-blue"></i> ' + f.Nombre + '</td><td>' + f.Tipo + '</td>';
                fila += '<td class="text-right">' + f.Tamano + '</td>';
                fila += '<td class="text-right">';
                if (f.Extension == 'pdf') {
                    fila += '<a class="btn default btn-xs green-stripe" href="javascript:ViewPdf(\'' + f.Nombre + '\');" title="Ver"><i class="fa fa-desktop"></i></a>';
                }
                fila += '<a class="btn default btn-xs blue-stripe" href="javascript:Download(\'' + f.Nombre + '\');" title="Descargar"><i class="fa fa-download"></i></a>';
                if (r.Items.CanEditar) {
                    fila += '<a class="btn default btn-xs red-stripe" href="javascript:DeleteArchivo(\'' + f.Nombre + '\');" title="Eliminar"><i class="fa fa-trash"></i></a>';
                }
                fila += '</td></tr>';
                lista.push(fila);
            }

            if (lista.length == 0) lista.push('<tr><td class="text-center" colspan="4">Carpeta vac&iacute;a</td></tr>');

            $('#repo-files').html(lista.join(''));

            UpdatePathBreadcumb();
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}

function Navegar(p) {
    currentPath.push(p);

    LoadFiles();
}

function NavegarPath(p) {
    currentPath = p.slice(0, -1).split('/');

    LoadFiles();
}

function NavegarHome() {
    currentPath = [];

    LoadFiles();
}

function UpdatePathBreadcumb() {
    var result = '<li><i class="fa fa-home"></i><a href="javascript:NavegarHome();">Inicio</a></li>';

    var path = '';
    for (var p in currentPath) {
        path += currentPath[p] + '/';
        result += '<li><a href="javascript:NavegarPath(\'' + path + '\');">' + currentPath[p] + '</a></li>';
    }

    $('.path-breadcrumb').html(result);
}

function Download(f) {
    var path = currentPath.join('\\');

    window.open('/Repositorio/Download/' + repositorioId + '?path=' + path + '&nombre=' + f);
}

function DeleteCarpeta(n) {
    bootbox.confirm("¿Est&aacute; seguro que desea eliminar la carpeta?", function (result) {
        if (!result) return;

        var path = currentPath.join('\\');

        $.ajax({
            url: '/Repositorio/DeleteCarpeta/' + repositorioId,
            type: "POST",
            data: { path: path, nombre: n },
            dataType: "json",
            async: true,
            cache: false,
            success: function (r) {
                if (!r.Success) ShowError(r.Message);
                else ShowSuccess('Carpeta eliminada.', LoadFiles);
            },
            error: function (data, ajaxOptions, thrownError) {
                ShowError(ERR_SERVER);
            }
        });
    });
}

function DeleteArchivo(n) {
    bootbox.confirm("¿Est&aacute; seguro que desea eliminar el archivo?", function (result) {
        if (!result) return;

        var path = currentPath.join('\\');

        $.ajax({
            url: '/Repositorio/DeleteArchivo/' + repositorioId,
            type: "POST",
            data: { path: path, nombre: n },
            dataType: "json",
            async: true,
            cache: false,
            success: function (r) {
                if (!r.Success) ShowError(r.Message);
                else ShowSuccess('Archivo eliminado.', LoadFiles);
            },
            error: function (data, ajaxOptions, thrownError) {
                ShowError(ERR_SERVER);
            }
        });
    });
}

function UpdateCarpetaNombre(v) {
    bootbox.prompt("Cambiar nombre", function (result) {
        if (result == null) return;

        var path = currentPath.join('\\');

        $.ajax({
            url: '/Repositorio/UpdateCarpetaNombre/' + repositorioId,
            type: "POST",
            data: { path: path, viejo: v, nuevo: result },
            dataType: "json",
            async: true,
            cache: false,
            success: function (r) {
                if (!r.Success) ShowError(r.Message);
                else ShowSuccess('Operación completada.', LoadFiles);
            },
            error: function (data, ajaxOptions, thrownError) {
                ShowError(ERR_SERVER);
            }
        });
    });
}

function ViewPdf(f) {
    var path = currentPath.join('\\');
    var url = 'http://' + window.location.host + '/Repositorio/Download/' + repositorioId + '?path=' + path + '&nombre=' + f;

    $('#pdf-container').html('<div id="pdf" style="height:1px"></div>');

    $('#pdf').attr('href', url).gdocsViewer({ width: 900, height: 450 });

    ShowModal('dlgViewPdf');
}

