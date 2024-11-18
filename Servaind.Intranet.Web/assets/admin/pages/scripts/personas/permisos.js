var Permisos = function () {

    var handleEvents = function() {
        $('.btnSave').click(function () {
            Save();
        });

        $('.td-permiso').click(function() {
            var target = $('#chkPermiso_' + $(this).attr('target'));

            $(target).prop('checked', !$(target).is(':checked'));

            RefreshCheckboxUI();
        });

        $('#cbPersona').change(function() {
            var id = $(this).val();

            if (id == INVALID_ID) {
                ClearSelection();
                $('.btnSave').hide();
            } else {
                LoadPermisos(id);
                $('.btnSave').show();
            }
        }).change();
    };

    return {
        init: function () {
            handleEvents();
        }
    };
}();

function ClearSelection() {
    $('.chkPermiso').prop('checked', false);

    RefreshCheckboxUI();
}

function LoadPermisos(id) {
    ClearSelection();

    ShowLoading();

    $.ajax({
        url: '/Personas/Permisos/' + id,
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            var c = r.length;
            for (var i = 0; i < c; i++) $('#chkPermiso_' + r[i]).prop('checked', true);

            RefreshCheckboxUI();

            CloseLoading();
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}

function Save() {
    ShowLoading();

    var permisos = [];
    $('.chkPermiso:checked').each(function () {
        permisos.push($(this).val());
    });

    $.ajax({
        url: '/Personas/SavePermisos/' + $('#cbPersona').val(),
        data: { permisos: permisos },
        type: "POST",
        dataType: "json",
        traditional: true,
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r.Success) ShowSuccess('Los cambios fueron guardados.');
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}