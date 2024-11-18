var Instrumentos = function () {

    var handleValidationInst = function () {
        var form = $('#form-inst');
        form.validate({
            errorElement: 'span',
            errorClass: 'help-block help-block-error',
            focusInvalid: false,
            ignore: "",
            rules: {
                txtNumero: {
                    required: true,
                    digits: true,
                    min: 0
                },
                cbTipo: {
                    required: true
                },
                txtDescripcion: {
                    required: true
                },
                cbMarca: {
                    required: true
                },
                txtModelo: {
                    required: true
                },
                txtNumSerie: {
                    required: true
                },
                txtRango: {
                    required: true
                },
                txtResolucion: {
                    required: true
                },
                txtClase: {
                    required: true
                },
                txtIncertidumbre: {
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
                txtMantFrec: {
                    required: true,
                    min: 1
                },
                txtMantFecha: {
                    required: true
                },
                cbMantPersona: {
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
                txtCalibFrec: {
                    required: true,
                    digits: true,
                    min: 0
                },
                txtCalibFecha: {
                    required: true
                },
                cbCalibPersona: {
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

    var handleEvents = function() {
        $('#btnCancel').click(function() {
            ShowAlert("&iquest;Est&aacute; seguro que desea salir?", "Salir", "Cancelar", Discard);
        });

        $('.date-picker').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            language: 'es',
            endDate: new Date()
        });

        $('#btnSave').click(Save);
    };

    return {
        init: function () {
            handleValidationInst();
            handleValidationCalib();
            handleValidationMant();
            handleValidationComprob();
            handleEvents();
        }
    };
}();

function Discard() {
    location.href = '/Instrumentos/';
}

function Save() {
    if (!$('#form-inst').valid() || !$('#form-calib').valid() || !$('#form-mant').valid() || !$('#form-comprob').valid()) {
        ShowError('Los campos marcados con rojo, no est&aacute;n completos o contienen datos inv&aacute;lidos.');
        return;
    }

    ShowLoading();

    $.ajax({
        url: '/Instrumentos/Create/',
        data: {
            numero: numero,
            tipoId: $('#cbTipo').val(),
            descripcion: $('#txtDescripcion').val(),
            marcaId: $('#cbMarca').val(),
            modelo: $('#txtModelo').val(),
            numSerie: $('#txtNumSerie').val(),
            rango: $('#txtRango').val(),
            resolucion: $('#txtResolucion').val(),
            clase: $('#txtClase').val(),
            incertidumbre: $('#txtIncertidumbre').val(),
            frecCalib: $('#txtCalibFrec').val(),
            ultCalib: $('#txtCalibFecha').val(),
            ultCalibPersonaId: $('#cbCalibPersona').val(),
            frecMant: $('#txtMantFrec').val(),
            ultMant: $('#txtMantFecha').val(),
            ultMantPersonaId: $('#cbMantPersona').val(),
            ubicacion: $('#txtComprobUbicacion').val(),
            ultComprob: $('#txtComprobFecha').val(),
            grupoId: $('#cbComprobGrupo').val(),
            ultComprobPersonaId: $('#cbComprobPersona').val()
        },
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        success: function (r) {
            CloseLoading();

            if (r.Success) ShowSuccess('Se agregó el instrumento.', Discard);
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}