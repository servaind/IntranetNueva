var currentPage;
var currentId;
var validator;

var Personas = function () {

    var handleValidation = function () {
        var form = $('#form-persona');

        validator = form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtNombre: {
                    required: true
                },
                txtUsuario: {
                    required: true
                },
                txtEmail: {
                    required: true
                },
                txtLegajo: {
                    required: true
                },
                txtCuil: {
                    required: true
                },
                cbResponsable: {
                    required: true
                },
                cbBase: {
                    required: true
                },
                txtHoraEntrada: {
                    required: true
                },
                txtHoraSalida: {
                    required: true
                },
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
        $('#btnPersonaAdd').click(function() {
            ShowPersonaDlg();
        });

        $('#btnPersonaSave').click(function () {
            SavePersona();
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

    var personaId = $('#txtFilterId').val();
    if ($.trim(personaId).length == 0 || isNaN(personaId)) {
        personaId = INVALID_ID;
        $('#txtFilterId').val('');
    }
    
    $.ajax({
        url: '/Personas/List',
        type: "POST",
        data: { pagina: currentPage, id: personaId, nombre: $('#txtFilterNombre').val(), usuario: $('#txtFilterUsuario').val(), responsableId: $('#cbFilterResponsable').val() },
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

    result += '<tr id="persona_' + i.Id + '" onclick="LoadPersona(' + i.Id + ');">';
    result += '<td class="text-center">' + i.Id + '</td>';
    result += '<td class="text-center" id="lblNombre_' + i.Id + '">' + i.Nombre + '</td>';
    result += '<td class="text-center" id="lblUsuario_' + i.Id + '">' + i.Usuario + '</td>';
    result += '<td class="text-center" id="lblResponsable_' + i.Id + '">' + i.Responsable + '</td>';
    result += '<td class="text-center"><label class="label label-sm label-' + (i.EnPc ? 'success' : 'danger') + '" id="lblEnPc_' + i.Id + '">' + (i.EnPc ? 'Si' : 'No') + '</label></td>';
    result += '<td class="text-center"><label class="label label-sm label-' + (i.Activo ? 'success' : 'danger') + '" id="lblActivo_' + i.Id + '">' + (i.Activo ? 'Si' : 'No') + '</label></td>';
    result += '</tr>';

    return result;
}

function ShowPersonaDlg(nombre, usuario, email, legajo, cuil, responsableId, baseId, horaEntrada, horaSalida,
    enPc, activo) {

    var c = arguments.length;
    if (c == 0) {
        nombre = usuario = email = legajo = cuil = '';
        responsableId = baseId = INVALID_ID;
        horaEntrada = '08:30';
        horaSalida = '17:30';
        enPc = activo = true;
    }

    $('#dlgPersonaTitle').text((c == 0 ? 'Agregar' : 'Editar') + ' Persona');

    $('#txtNombre').val(nombre);
    $('#txtUsuario').val(usuario);
    $('#txtEmail').val(email);
    $('#txtLegajo').val(legajo);
    $('#txtCuil').val(cuil);
    $('#cbResponsable').val(responsableId);
    $('#cbBase').val(baseId);
    $('#txtHoraEntrada').val(horaEntrada);
    $('#txtHoraSalida').val(horaSalida);
    $('#cbEnPc').val(enPc ? 1 : 0);
    $('#cbActivo').val(activo ? 1 : 0);

    $('#form-persona .form-group').removeClass('has-error has-success');

    ShowModal('dlgPersona');
}

function LoadPersona(id) {
    ShowLoading();

    $.ajax({
        url: '/Personas/Detalle/' + id,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            if (r != null) {
                currentId = id;
                ShowPersonaDlg(r.Nombre, r.Usuario, r.Email, r.Legajo, r.Cuil, r.ResponsableId, r.BaseId, r.HoraEntrada, r.HoraSalida, r.EnPanelControl, r.Activo);
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

function SavePersona() {
    if (!$('#form-persona').valid()) return;

    var target = '.modal-content';

    Block(target);

    $.ajax({
        url: '/Personas/Save/' + currentId,
        data: { nombre: $('#txtNombre').val(), usuario: $('#txtUsuario').val(), email: $('#txtEmail').val(), legajo: $('#txtLegajo').val(), cuil: $('#txtCuil').val(), responsableId: $('#cbResponsable').val(), baseId: $('#cbBase').val(), horaEntrada: $('#txtHoraEntrada').val(), horaSalida: $('#txtHoraSalida').val(), enPc: $('#cbEnPc').val() == 1, activo: $('#cbActivo').val() == 1 },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            Unblock(target);

            if (r.Success) {
                UpdatePersona(r.Items);
                CloseModal('dlgPersona');
                ShowSuccess('Los cambios fueron guardados.');
            } else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            Unblock(target);
            ShowError(ERR_SERVER);
        }
    });
}

function UpdatePersona(nombre) {
    $('#lblNombre_' + currentId).text(nombre);
    $('#lblUsuario_' + currentId).text($('#txtUsuario').val());
    $('#lblResponsable_' + currentId).text($('#cbResponsable option:selected').text());

    var enPc = $('#cbEnPc').val() == 1;
    var activo = $('#cbActivo').val() == 1;
    $('#lblEnPc_' + currentId).removeClass('label-success label-danger').addClass(enPc ? 'label-success' : 'label-danger').text(enPc ? 'Si' : 'No');
    $('#lblActivo_' + currentId).removeClass('label-success label-danger').addClass(activo ? 'label-success' : 'label-danger').text(activo ? 'Si' : 'No');

    currentId = INVALID_ID;
}