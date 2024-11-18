// Constants.
var APP_NAME = 'SME';
var ERR_SERVER = 'Se produjo un error al intentar completar la operación. Por favor, intente nuevamente.';

var dlg_opts = { show: true, keyboard: false, modalOverflow: true };
var chartColours = ['#88bbc8', '#ed7a53', '#9FC569', '#bbdce3', '#9a3b1b', '#5a8022', '#2c7282'];

function ShowError(msg) {
    bootbox.dialog({
        message: msg,
        title: "Error",
        className: "dlg-error-box",
        buttons: {
            main: {
                label: "Aceptar",
                className: "btn-danger"
            }
        }
    });
}

function ShowSuccess(msg, action) {
    bootbox.dialog({
        message: msg,
        className: "dlg-error-box",
        buttons: {
            main: {
                label: "Aceptar",
                className: "btn-success",
                callback: action
            }
        }
    });
}

function ShowAlert(msg, primaryText, secondaryText, action) {
    bootbox.dialog({
        message: msg,
        className: "dlg-error-box",
        buttons: {
            cancel: {
                label: secondaryText,
                className: "btn-default"
            },
            main: {
                label: primaryText,
                className: "btn-primary",
                callback: action
            }
        }
    });
}

function ShowLoading() {
    Metronic.blockUI({
        boxed: true,
        message: 'Cargando...'
    });
}

function CloseLoading() {
    Metronic.unblockUI();
}

function Pad(str, max) {
    return str.length < max ? Pad("0" + str, max) : str;
}

function RemoveArrayElement(array, element) {
    if (array) {
        var idx = array.indexOf(element);
        if (idx != -1) {
            array.splice(idx, 1);
        }
    }
}

function GetDate(d) {
    var curr_date = d.getDate().toString();
    var curr_month = (d.getMonth() + 1).toString();
    var curr_year = d.getFullYear().toString();

    return Pad(curr_date, 2) + "/" + Pad(curr_month, 2) + "/" + Pad(curr_year, 2);
}

function GetTime(d) {
    var h = d.getHours().toString();
    var m = d.getMinutes().toString();

    return Pad(h, 2) + ':' + Pad(m, 2);
}

function OpenLink(relativeUrl) {
    window.open(
      ('http://' + window.location.host + '/' + relativeUrl),
      '_self'
    );
}

function RefreshPage() {
    location.reload();
}

bootbox.setDefaults({
    locale: "es"
});

$('.number-float').change(function () {
    var n = SetDecimal($(this).val());
    var aux;

    if (isNaN(n)) n = 0;
    aux = parseFloat(n);

    $(this).val(aux.toFixed(1));
});

$('.number-float-range').change(function () {
    var min = parseFloat(SetDecimal($(this).attr('min')));
    var max = parseFloat(SetDecimal($(this).attr('max')));
    var n = SetDecimal($(this).val());
    var aux;

    if (isNaN(n)) n = 0;
    aux = parseFloat(n);
    if (aux < min) aux = min;
    if (aux > max) aux = max;

    $(this).val(aux.toFixed(1));
});

$('.number-int').change(function () {
    var n = $(this).val();
    var aux;

    if (isNaN(n)) n = 0;
    aux = parseInt(n);

    $(this).val(aux);
});

function SetDecimal(v) {
    var result = arguments.length == 0 ? $(this).val() : v;

    return result.replace(',', '.');
}

function GetDecimalNumber(str, decimals) {
    var result;

    if (decimals > 0) {
        result = parseFloat(str).toFixed(decimals);
    } else {
        result = str;
    }

    return result;
}

function FormatDate(d) {
    var result = GetDate(d) + ' ' + GetTime(d);

    return result;
}

function Pad(str, max) {
    return str.length < max ? Pad("0" + str, max) : str;
}

function GetDate(d) {
    var curr_date = d.getDate().toString();
    var curr_month = (d.getMonth() + 1).toString();
    var curr_year = d.getFullYear().toString();

    return Pad(curr_date, 2) + "/" + Pad(curr_month, 2) + "/" + Pad(curr_year, 2);
}

function GetTime(d) {
    var h = d.getHours().toString();
    var m = d.getMinutes().toString();

    return Pad(h, 2) + ':' + Pad(m, 2);
}

function Block(t) {
    Metronic.blockUI({
        target: t,
        boxed: true,
        message: 'Procesando...'
    });
}

function Unblock(t) {
    window.setTimeout(function () {
        Metronic.unblockUI(t);
    }, 200);
}

function ShowModal(t) {
    $('#' + t).modal(dlg_opts);
}

function CloseModal(t) {
    $('#' + t).modal('hide');
}

function RefreshCheckboxUI() {
    $('input[type=checkbox]').each(function () {
        var t = $(this).parents('span:first');
        t.removeClass('checked');

        if ($(this).is(':checked')) t.addClass('checked');
    });
}

function ClearFormStatus(f) {
    $('#' + f + ' .form-group').removeClass('has-error has-success');
}

$(document).ready(function() {
    $('textarea[maxlength]').keyup(function () {
        var limit = parseInt($(this).attr('maxlength'));
        var text = $(this).val();
        var chars = text.length;

        if (chars > limit) {
            var new_text = text.substr(0, limit);
            $(this).val(new_text);
        }
    });
});

function GetFileIcon(e) {
    if (e == 'pdf') return 'file-pdf-o';
    if (e == 'txt') return 'file-text-o';
    if (e == 'exe') return 'file-desktop';
    if (e == 'iso') return 'file-hdd-o';
    if (e == 'xls' || e == 'xlsx') return 'file-excel-o';
    if (e == 'doc' || e == 'docx') return 'file-word-o';
    if (e == 'ppt' || e == 'pptx' || e == 'pps' || e == 'ppsx') return 'file-powerpoint-o';
    if (e == 'zip' || e == 'rar') return 'file-zip-o';
    if (e == 'jpg' || e == 'png' || e == 'gif') return 'file-image-o';

    return 'file';
}