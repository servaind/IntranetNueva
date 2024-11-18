var currentPage;
var currentId;
var validator;

var Imputaciones = function () {

    var handleValidation = function () {
        var form = $('#form-imputacion');

        validator = form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtNumero: {
                    required: true,
                    digits: true,
                    min: 1
                },
                txtDescripcion: {
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
        $('#btnImputacionAdd').click(function() {
            ShowImputacionDlg();
        });

        $('#btnImputacionSave').click(function () {
            SaveImputacion();
        });

        $('.filter-field').change(function() {
            List(1);
        });
    };

    return {
        init: function () {
            currentId = INVALID_ID;

            handleValidation();
            handleList();
            handleEvents();
        }
    };
}();

function List(page) {
    ShowLoading();

    currentPage = page;

    var numero = $('#txtFilterNumero').val();
    if ($.trim(numero).length == 0 || isNaN(numero)) {
        numero = INVALID_ID;
        $('#txtFilterNumero').val('');
    }
    
    $.ajax({
        url: '/Imputaciones/List',
        type: "POST",
        data: { pagina: currentPage, numero: numero, descripcion: $('#txtFilterDescripcion').val() },
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

    result += '<tr id="imputacion_' + i.Id + '" onclick="LoadImputacion(' + i.Id + ');">';
    result += '<td class="text-center">' + i.Numero + '</td>';
    result += '<td class="text-center" id="lblDescripcion_' + i.Id + '">' + i.Descripcion + '</td>';
    result += '<td class="text-center"><label class="label label-sm label-' + (i.Activa ? 'success' : 'danger') + '" id="lblActiva_' + i.Id + '">' + (i.Activa ? 'Si' : 'No') + '</label></td>';
    result += '</tr>';

    return result;
}

function ShowImputacionDlg(numero, descripcion, activa) {

    var c = arguments.length;
    if (c == 0) {
        numero = descripcion = '';
        activa = true;
    }

    $('#dlgImputacionTitle').text((c == 0 ? 'Agregar' : 'Editar') + ' Imputación');

    $('#txtNumero').val(numero);
    $('#txtDescripcion').val(descripcion);
    $('#cbActiva').val(activa ? 1 : 0);

    $('#form-imputacion .form-group').removeClass('has-error has-success');

    ShowModal('dlgImputacion');
}

function LoadImputacion(id) {
    ShowLoading();

    $.ajax({
        url: '/Imputaciones/Detalle/' + id,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            if (r != null) {
                currentId = id;
                ShowImputacionDlg(r.Numero, r.Descripcion, r.Activa);
            } else {
                currentId = INVALID_ID;
            }

            CloseLoading();
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}

function SaveImputacion() {
    if (!$('#form-imputacion').valid()) return;

    var target = '.modal-content';

    Block(target);

    $.ajax({
        url: '/Imputaciones/Save/' + currentId,
        data: {
            numero: $('#txtNumero').val(), descripcion: $('#txtDescripcion').val(),
            activa: $('#cbActiva').val() == 1
        },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                UpdateImputacion();
                CloseModal('dlgImputacion');
                ShowSuccess('Los cambios fueron guardados.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function UpdateImputacion() {
    $('#lblNombre_' + currentId).text($('#txtNombre').val());
    $('#lblDescripcion_' + currentId).text($('#txtDescripcion').val());

    var activa = $('#cbActiva').val() == 1;
    $('#lblActiva_' + currentId).removeClass('label-success label-danger').addClass(activa ? 'label-success' : 'label-danger').text(activa ? 'Si' : 'No');

    currentId = INVALID_ID;
}