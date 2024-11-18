var currentId;
var currentPage;
var currentAction;

var Instrumentos = function () {

    var handleValidationComprob = function () {
        var form = $('#form-comprob');
        form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtComprobFecha: {
                    required: true
                },
                cbComprobPersona: {
                    required: true
                },
                cbComprobGrupo: {
                    required: true
                },
                txtComprobUbicacion: {
                    required: true
                },
                txtComprobDescripcion: {
                    required: true
                }
            },

            invalidHandler: function (event, validator) {
                //ShowError('Algunos campos contienen datos inv&aacute;lidos.');
            },

            errorPlacement: function (error, element) {
                var icon = $(element).parent('.input-icon').children('i');
                icon.removeClass('fa-check').addClass("fa-warning");
                icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
            },

            highlight: function (element) {
                $(element)
                    .closest('.form-group').removeClass("has-success").addClass('has-error');
            },

            unhighlight: function (element) {

            },

            success: function (label, element) {
                var icon = $(element).parent('.input-icon').children('i');
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
                icon.removeClass("fa-warning").addClass("fa-check");
            },

            submitHandler: function (form) {

            }
        });
    };

    var handleValidationMant = function () {
        var form = $('#form-mant');
        form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtMantFecha: {
                    required: true
                },
                cbMantPersona: {
                    required: true
                },
                txtMantDescripcion: {
                    required: true
                }
            },

            invalidHandler: function (event, validator) {
                //ShowError('Algunos campos contienen datos inv&aacute;lidos.');
            },

            errorPlacement: function (error, element) {
                var icon = $(element).parent('.input-icon').children('i');
                icon.removeClass('fa-check').addClass("fa-warning");
                icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
            },

            highlight: function (element) {
                $(element)
                    .closest('.form-group').removeClass("has-success").addClass('has-error');
            },

            unhighlight: function (element) {

            },

            success: function (label, element) {
                var icon = $(element).parent('.input-icon').children('i');
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
                icon.removeClass("fa-warning").addClass("fa-check");
            },

            submitHandler: function (form) {

            }
        });
    };

    var handleValidationCalib = function () {
        var form = $('#form-calib');
        form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtCalibFecha: {
                    required: true
                },
                cbCalibPersona: {
                    required: true
                },
                txtCalibDescripcion: {
                    required: true
                }
            },

            invalidHandler: function (event, validator) {
                //ShowError('Algunos campos contienen datos inv&aacute;lidos.');
            },

            errorPlacement: function (error, element) {
                var icon = $(element).parent('.input-icon').children('i');
                icon.removeClass('fa-check').addClass("fa-warning");
                icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
            },

            highlight: function (element) {
                $(element)
                    .closest('.form-group').removeClass("has-success").addClass('has-error');
            },

            unhighlight: function (element) {

            },

            success: function (label, element) {
                var icon = $(element).parent('.input-icon').children('i');
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
                icon.removeClass("fa-warning").addClass("fa-check");
            },

            submitHandler: function (form) {

            }
        });
    };

    var handleValidationAction = function () {
        var form = $('#form-accion');
        form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtAccionDesc: {
                    required: true
                }
            },

            invalidHandler: function (event, validator) {
                //ShowError('Algunos campos contienen datos inv&aacute;lidos.');
            },

            errorPlacement: function (error, element) {
                var icon = $(element).parent('.input-icon').children('i');
                icon.removeClass('fa-check').addClass("fa-warning");
                icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
            },

            highlight: function (element) {
                $(element)
                    .closest('.form-group').removeClass("has-success").addClass('has-error');
            },

            unhighlight: function (element) {

            },

            success: function (label, element) {
                var icon = $(element).parent('.input-icon').children('i');
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
                icon.removeClass("fa-warning").addClass("fa-check");
            },

            submitHandler: function (form) {

            }
        });
    };

    var handleList = function () {
        List(1);
    };

    var handleEvents = function() {
        $('#btnInstrumentoAdd').click(function() {
            location.href = '/Instrumentos/Create';
        });

        $('.filter-field, #chkBusquedaExt').change(function() {
            List(1);
        });

        $('#btnHistComprobAdd').click(ShowHistComprobCreate);
        $('#btnHistComprobSave').click(CreateComprob);

        $('#btnHistMantAdd').click(ShowHistMantCreate);
        $('#btnHistMantSave').click(CreateMant);

        $('#btnHistCalibAdd').click(ShowHistCalibCreate);
        $('#btnHistCalibSave').click(CreateCalib);

        $('#btnAccionDo').click(DoAction);

        $('#btnExportCsv').click(ExportToCsv);
    };

    return {
        init: function () {
            handleValidationComprob();
            handleValidationMant();
            handleValidationCalib();
            handleValidationAction();
            handleList();
            handleEvents();
        }
    };
}();

function List(page) {
    ShowLoading();

    currentPage = page;
    
    $.ajax({
        url: '/Instrumentos/List',
        type: "POST",
        data: {
            pagina: currentPage, descripcion: $('#txtFilterDesc').val(),
            mantStatus: $('#cbFilterMant').val(),
            calibStatus: $('#cbFilterCalib').val(),
            busquedaExtendida: $('#chkBusquedaExt').is(':checked')
        },
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            var c = r.Items.length;

            var rows = [];

            if (c == 0) rows.push('<tr><td colspan="6" class="center">No hay datos disponibles.</td></tr>');
            else {
                for (var i = 0; i < c; i++) rows.push(GetListRow(r.Items[i]));
            }

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

    result += '<tr id="instrumento_' + i.Id + '">';
    result += '<td class="text-center"><span class="row-details row-details-close" id="inst-show-detalle-' + i.Id + '" onclick="ShowDetalle(' + i.Id + ');"></span></td>';
    result += '<td class="text-center"><img src="/Common/Image/?hash=' + i.ImagePathHash + '" class="instrumento-img-sm hand" onclick="ShowImagenes(' + i.Numero + ');"></td>';
    result += '<td>';
    result += '<strong class="' + (i.Activo ? '' : 'text-danger') + '">' + i.Tipo + '</strong><br/>';
    result += 'Marca: ' + i.Marca + '<br/>';
    result += 'Modelo: ' + i.Modelo + '<br/>';
    result += 'Nº de serie: ' + i.NumeroSerie + '<br/>';
    result += 'Número: ' + i.Numero;
    result += '</td>';
    result += '<td><center><a href="javascript:;" onclick="ShowHistComprob(' + i.Id + ');" class="btn btn-xs default margin-bottom-5"><i class="fa fa-calendar"></i> Historial</a></center>Última: ' + i.ComprobFecha + '</td>';
    result += '<td><center><a href="javascript:;" onclick="ShowHistMant(' + i.Id + ');" class="btn btn-xs default margin-bottom-5"><i class="fa fa-calendar"></i> Historial</a></center>' + GetEstadoCelda(i.MantFecha, i.MantFrec, i.MantVencido, i.MantProxVencer) + '</td>';
    if (i.CalibFrec > 0) {
        result += '<td><center><a href="javascript:;" onclick="ShowHistCalib(' + i.Id + ');" class="btn btn-xs default margin-bottom-5"><i class="fa fa-calendar"></i> Historial</a></center>' + GetEstadoCelda(i.CalibFecha, i.CalibFrec, i.CalibVencida, i.CalibProxVencer) + '</td>';
    } else result += '<td class="text-center"><br/>No aplica</td>';
    result += '<td>';
    if (i.HasCertCalib) {
        result += '<button type="button" class="btn btn-xs blue margin-bottom-5" onclick="DownloadCertCalib(' + i.Numero + ');"><i class="fa fa-file-pdf-o"></i> Cert. calibración</button>';
        result += '<br/>';
    }
    if (i.HasManuales) {
        result += '<button type="button" class="btn btn-xs blue margin-bottom-5" onclick="ShowManuales(' + i.Numero + ');"><i class="fa fa-file-pdf-o"></i> Manuales</button>';
        result += '<br/>';
    }
    if (i.HasEac) {
        result += '<button type="button" class="btn btn-xs blue margin-bottom-5" onclick="DownloadEac(' + i.Numero + ');"><i class="fa fa-file-pdf-o"></i> EAC</button>';
    }
    if (i.HasComprobMant) {
        result += '<button type="button" class="btn btn-xs blue margin-bottom-5" onclick="ShowComprobMant(' + i.Numero + ');"><i class="fa fa-file-pdf-o"></i> Imágenes</button>';
        result += '<br/>';
    }
    result += '</td>';
    result += '</tr>';
    result += '<tr id="inst-detalle-' + i.Id + '" class="instrumento-detalle">';
    result += '<td colspan="2"></td>';
    result += '<td colspan="4">';
    result += 'Ubicación: ' + i.ComprobUbicacion + '<br/>';
    result += 'Grupo: ' + i.ComprobGrupo + '<br/>';
    result += 'Responsable: ' + i.ComprobPersona + (false ? (' (' + i.ComprobPersonaEmail + ')') : '') + '<br/>';
    result += 'Resolución: ' + i.Resolucion + '<br/>';
    result += 'Clase: ' + i.Clase + '<br/>';
    result += 'Rango: ' + i.Rango + '<br/>';
    result += 'Incertidumbre: ' + i.Incertidumbre;
    result += '</td>';
    if (i.Activo) {
        result += '<td><button type="button" class="btn btn-xs red" onclick="ConfirmDesactivar(' + i.Id + ');"><i class="fa fa-trash"></i> Desactivar</button></td>';
    } else {
        result += '<td><button type="button" class="btn btn-xs green" onclick="ConfirmActivar(' + i.Id + ');"><i class="fa fa-refresh"></i> Activar</button></td>';
    }
    result += '</tr>';

    return result;
}

function GetEstadoCelda(ultima, frec, vencida, proxVencer) {
    var c = vencida ? 'danger' : (proxVencer ? 'warning' : 'success');
    var t = vencida ? 'vencida' : (proxVencer ? 'próx. a vencer' : 'vigente');

    var result = 'Último: ' + ultima + '<br/>';
    result += 'Frecuencia: ' + frec + ' mes' + (frec != 1 ? 'es' : '') + '.';
    result += '<br/><div class="clearfix margin-bottom-5"></div><center><span class="label label-sm label-' + c + '">' + t + '</span></center>';

    return result;
}

function ShowDetalle(id) {
    var s = $('#inst-show-detalle-' + id);
    var t = $('#inst-detalle-' + id);

    if (s.hasClass('row-details-close')) {
        s.removeClass('row-details-close').addClass('row-details-open');
        t.show();
    } else {
        s.removeClass('row-details-open').addClass('row-details-close');
        t.hide();
    }
}

function UpdateDetalleInstrumento(i) {
    $('#inst-detalle-' + currentId).remove();
    $('#instrumento_' + currentId).replaceWith(GetListRow(i));
}

function ShowAction(action, title) {
    currentAction = action;
    $('#dlgAccionTitle').text(title);
    $('#txtAccionDesc').val('');

    ClearFormStatus('form-accion');

    ShowModal('dlgAccion');
}

function DoAction() {
    if (!$('#form-accion').valid()) return;

    if (currentAction == 'activate') {
        Activar();
        return;
    }
    if (currentAction == 'deactivate') {
        Desactivar();
        return;
    }
}

function ConfirmDesactivar(id) {
    currentId = id;

    ShowAction('deactivate', 'Desactivar instrumento');
}

function Desactivar() {
    var target = '#dlgAccion .modal-content';

    Block(target);

    $.ajax({
        url: '/Instrumentos/Delete/' + currentId,
        data: { descripcion: $('#txtAccionDesc').val() },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                CloseModal('dlgAccion');

                $('#instrumento_' + currentId + ', #inst-detalle-' + currentId).remove();

                ShowSuccess('Se desactiv&oacute; el instrumento.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function ConfirmActivar(id) {
    currentId = id;
    ShowAction('activate', 'Activar instrumento');
}

function Activar() {
    var target = '#dlgAccion .modal-content';

    Block(target);

    $.ajax({
        url: '/Instrumentos/Activate/' + currentId,
        data: { descripcion: $('#txtAccionDesc').val() },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                CloseModal('dlgAccion');

                UpdateDetalleInstrumento(r.Items);

                ShowSuccess('Se activ&oacute; el instrumento.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function DownloadCertCalib(numero) {
    location.href = '/Instrumentos/CertCalib/' + numero;
}

function DownloadEac(numero) {
    location.href = '/Instrumentos/Eac/' + numero;
}

// Comprobaciones.
// --------------------------------------------------------------------------------

function ShowHistComprob(id) {
    currentId = id;
    ShowModal('dlgHistComprob');

    LoadHistComprob();
}

function LoadHistComprob() {
    var target = 'hist-comprob-body';

    $('#comprob-list-container').html('');

    Block(target);

    $.ajax({
        url: '/Instrumentos/ListComprob/' + currentId,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r != null) {
                var c = r.length;
                var rows = [];
                for (var i = 0; i < c; i++) {
                    var o = r[i];

                    var row = '<tr>';
                    row += '<td class="text-center">' + o.Fecha + '</td>';
                    row += '<td>' + o.Grupo + '</td>';
                    row += '<td>' + o.Ubicacion + '</td>';
                    row += '<td>' + o.Persona + '</td>';
                    row += '<td>' + o.Descripcion + '</td>';
                    row += '</tr>';
                    rows.push(row);
                }
                $('#comprob-list-container').html(rows.join(''));
            }
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function ShowHistComprobCreate() {
    ClearFormStatus('form-comprob');

    $('#txtComprobFecha').val(fechaHoy);
    $('#txtComprobUbicacion, #txtComprobDescripcion').val('');

    ShowModal('dlgHistComprobCreate');

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es',
        endDate: new Date()
    });
}

function CreateComprob() {
    if (!$('#form-comprob').valid()) return;

    var target = '#dlgHistComprobCreate .modal-content';

    Block(target);

    $.ajax({
        url: '/Instrumentos/CreateComprob/' + currentId,
        data: {
            fecha: $('#txtComprobFecha').val(), personaId: $('#cbComprobPersona').val(), grupoId: $('#cbComprobGrupo').val(),
            ubicacion: $('#txtComprobUbicacion').val(), descripcion: $('#txtComprobDescripcion').val()
        },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                UpdateDetalleInstrumento(r.Items);

                LoadHistComprob();

                CloseModal('dlgHistComprobCreate');

                ShowSuccess('Se agregó el registro.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

// Mantenimientos.
// --------------------------------------------------------------------------------

function ShowHistMant(id) {
    currentId = id;
    ShowModal('dlgHistMant');

    LoadHistMant();
}

function LoadHistMant() {
    var target = 'hist-mant-body';

    $('#mant-list-container').html('');

    Block(target);

    $.ajax({
        url: '/Instrumentos/ListMant/' + currentId,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r != null) {
                var c = r.length;
                var rows = [];
                for (var i = 0; i < c; i++) {
                    var o = r[i];

                    var row = '<tr>';
                    row += '<td class="text-center">' + o.Fecha + '</td>';
                    row += '<td>' + o.Persona + '</td>';
                    row += '<td>' + o.Descripcion + '</td>';
                    row += '</tr>';
                    rows.push(row);
                }
                $('#mant-list-container').html(rows.join(''));
            }
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function ShowHistMantCreate() {
    ClearFormStatus('form-mant');

    $('#txtMantFecha').val(fechaHoy);
    $('#txtMantDescripcion').val('');

    ShowModal('dlgHistMantCreate');

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es',
        endDate: new Date()
    });
}

function CreateMant() {
    if (!$('#form-mant').valid()) return;

    var target = '#dlgHistMantCreate .modal-content';

    Block(target);

    $.ajax({
        url: '/Instrumentos/CreateMant/' + currentId,
        data: {
            fecha: $('#txtMantFecha').val(), personaId: $('#cbMantPersona').val(),
            descripcion: $('#txtMantDescripcion').val()
        },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                UpdateDetalleInstrumento(r.Items);

                LoadHistMant();

                CloseModal('dlgHistMantCreate');

                ShowSuccess('Se agregó el registro.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

// Calibraciones.
// --------------------------------------------------------------------------------

function ShowHistCalib(id) {
    currentId = id;
    ShowModal('dlgHistCalib');

    LoadHistCalib();
}

function LoadHistCalib() {
    var target = 'hist-calib-body';

    $('#calib-list-container').html('');

    Block(target);

    $.ajax({
        url: '/Instrumentos/ListCalib/' + currentId,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r != null) {
                var c = r.length;
                var rows = [];
                for (var i = 0; i < c; i++) {
                    var o = r[i];

                    var row = '<tr>';
                    row += '<td class="text-center">' + o.Fecha + '</td>';
                    row += '<td>' + o.Persona + '</td>';
                    row += '<td>' + o.Descripcion + '</td>';
                    row += '</tr>';
                    rows.push(row);
                }
                $('#calib-list-container').html(rows.join(''));
            }
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function ShowHistCalibCreate() {
    ClearFormStatus('form-calib');

    $('#txtCalibFecha').val(fechaHoy);
    $('#txtCalibDescripcion').val('');

    ShowModal('dlgHistCalibCreate');

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'es',
        endDate: new Date()
    });
}

function CreateCalib() {
    if (!$('#form-calib').valid()) return;

    var target = '#dlgHistCalibCreate .modal-content';

    Block(target);

    $.ajax({
        url: '/Instrumentos/CreateCalib/' + currentId,
        data: {
            fecha: $('#txtCalibFecha').val(), personaId: $('#cbCalibPersona').val(),
            descripcion: $('#txtCalibDescripcion').val()
        },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                UpdateDetalleInstrumento(r.Items);

                LoadHistCalib();

                CloseModal('dlgHistCalibCreate');

                ShowSuccess('Se agregó el registro.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

// Manuales.
// --------------------------------------------------------------------------------

function ShowManuales(numero) {
    $('#manuales-list-container').html('');
    ShowModal('dlgManuales');

    var target = 'manuales-body';
    
    Block(target);

    $.ajax({
        url: '/Instrumentos/Manuales/' + numero,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r != null) {
                var c = r.length;
                var rows = [];
                for (var i = 0; i < c; i++) {
                    var o = r[i];

                    var row = '<tr onclick="DownloadManual(\'' + o.PathHash + '\');">';
                    row += '<td>' + o.Nombre + '</td>';
                    row += '<td>' + o.Tipo + '</td>';
                    row += '<td class="text-right">' + o.Tamano + '</td>';
                    row += '</tr>';
                    rows.push(row);
                }
                $('#manuales-list-container').html(rows.join(''));
            }
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function DownloadManual(hash) {
    location.href = '/Instrumentos/Manual/?hash=' + hash;
}

// Imágenes.
// --------------------------------------------------------------------------------

function ShowImagenes(numero) {
    $('#manuales-list-container').html('');
    ShowModal('dlgImagenes');

    var target = 'imagenes-body';

    Block(target);

    $.ajax({
        url: '/Instrumentos/Imagenes/' + numero,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r != null) {
                var c = r.length;
                var rows = [];
                for (var i = 0; i < c; i++) {
                    var o = r[i];

                    var src = '/Common/Image/?hash=' + o.PathHash;

                    var row = '<div class="tile image"><div class="tile-body"><img onclick="ShowImagen(\'' + src + '\');" src="' + src + '" title="Click para agrandar"></div></div>';
                    rows.push(row);
                }
                $('#imagenes-list-container').html(rows.join(''));
            }
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function ShowImagen(src) {
    $('#instrumento-imagen-container').attr('src', src);
    ShowModal('dlgImagen');
}

// Comprobación / Mantenimiento.
// --------------------------------------------------------------------------------

function ShowComprobMant(numero) {
    ShowFiles('Comprobación / Mantenimiento', '/Instrumentos/ComprobMantFiles/' + numero);
}

// Files
// --------------------------------------------------------------------------------

function ShowFiles(title, url) {
    var container = $('#dlg-files-list-container');
    container.html('');
    $('#dlgFilesTitle').text(title);
    ShowModal('dlgFiles');

    var target = 'dlg-files-body';

    Block(target);

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r != null) {
                var c = r.length;
                var rows = [];
                for (var i = 0; i < c; i++) {
                    var o = r[i];

                    var row = '<tr onclick="DownloadFile(\'' + o.PathHash + '\');">';
                    row += '<td>' + o.Nombre + '</td>';
                    row += '<td>' + o.Tipo + '</td>';
                    row += '<td class="text-right">' + o.Tamano + '</td>';
                    row += '</tr>';
                    rows.push(row);
                }
                container.html(rows.join(''));
            }
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function DownloadFile(hash) {
    location.href = '/Instrumentos/DownloadFile/?hash=' + hash;
}

// Export to CSV.
// --------------------------------------------------------------------------------
function ExportToCsv() {
    var url = '/Instrumentos/Csv?';
    url += 'descripcion=' + $('#txtFilterDesc').val();
    url += '&mantStatus=' + $('#cbFilterMant').val();
    url += '&calibStatus=' + $('#cbFilterCalib').val();
    url += '&busquedaExtendida=' + $('#chkBusquedaExt').is(':checked');
    location.href = url;
}