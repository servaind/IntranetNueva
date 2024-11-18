var currentId;
var validator;

var Bases = function () {

    var handleValidation = function () {
        var form = $('#form-base');

        validator = form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtNombre: {
                    required: true
                },
                cbResponsable: {
                    required: true
                },
                cbAlternate: {
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
        $('#btnBaseAdd').click(function() {
            ShowBaseDlg();
        });

        $('#btnBaseSave').click(function () {
            SaveBase();
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
   
    $.ajax({
        url: '/Bases/List',
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            var c = r.length;

            var rows = [];

            if (c == 0) rows.push('<tr><td colspan="6" class="center">No hay datos disponibles.</td></tr>');
            else {
                for (var i = 0; i < c; i++) rows.push(GetListRow(r[i]));
            }

            $('#list-container').html(rows.join(''));

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

    result += '<tr id="persona_' + i.Id + '" onclick="LoadBase(' + i.Id + ');">';
    result += '<td class="text-center">' + i.Id + '</td>';
    result += '<td class="text-center">' + i.Nombre + '</td>';
    result += '<td class="text-center">' + i.Responsable + '</td>';
    result += '<td class="text-center">' + i.Alternate + '</td>';
    result += '<td class="text-center"><label class="label label-sm label-' + (i.Activa ? 'success' : 'danger') + '" id="lblActivo_' + i.Id + '">' + (i.Activa ? 'Si' : 'No') + '</label></td>';
    result += '</tr>';

    return result;
}

function ShowBaseDlg(nombre, responsableId, alternateId, activa) {

    var c = arguments.length;
    if (c == 0) {
        nombre = '';
        responsableId = alternateId = INVALID_ID;
        activa = true;
    }

    $('#dlgBaseTitle').text((c == 0 ? 'Agregar' : 'Editar') + ' Base');

    $('#txtNombre').val(nombre);
    $('#cbResponsable').val(responsableId);
    $('#cbAlternate').val(alternateId);
    $('#cbActivo').val(activa ? 1 : 0);

    $('#form-base .form-group').removeClass('has-error has-success');

    ShowModal('dlgBase');
}

function LoadBase(id) {
    ShowLoading();

    $.ajax({
        url: '/Bases/Detalle/' + id,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            if (r != null) {
                currentId = id;
                ShowBaseDlg(r.Nombre, r.ResponsableId, r.AlternateId, r.Activa);
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

function SaveBase() {
    if (!$('#form-base').valid()) return;

    var target = '.modal-content';

    Block(target);

    $.ajax({
        url: '/Bases/Save/' + currentId,
        data: { nombre: $('#txtNombre').val(), responsableId: $('#cbResponsable').val(), alternateId: $('#cbAlternate').val(), activa: $('#cbActivo').val() == 1 },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                List(1);
                CloseModal('dlgBase');
                ShowSuccess('Los cambios fueron guardados.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}