var Login = function () {

    var handleLogin = function() {

        $('.login-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                },
                remember: {
                    required: false
                }
            },

            messages: {
                username: {
                    required: "Ingrese un usuario."
                },
                password: {
                    required: "Ingrese una contraseña."
                }
            },

            invalidHandler: function(event, validator) { //display error alert on form submit   
                $('.alert-danger', $('.login-form')).show();
            },

            highlight: function(element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function(label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function(error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                $('.alert-danger', $('.login-form')).hide();
                DoLogin();
            }
        });

        $('.login-form input').keypress(function(e) {
            if (e.which == 13) {
                DoLogin();
                return false;
            }
        });
    };

    return {
        init: function () {
            handleLogin();
        }
    };
}();

function DoLogin() {
    if (!$('.login-form').valid()) return;

    ShowLoading();

    $.ajax({
        url: '/Usuario/Login',
        type: "POST",
        data: {
            username: $('#txtUsername').val(),
            password: $('#txtPassword').val(),
            remember: $('#chkRemember').is(':checked')
        },
        dataType: "json",
        async: true,
        cache: false,
        traditional: true,
        success: function (r) {
            CloseLoading();

            if (r.Success) location.href = urlRedirect.length > 0 ? urlRedirect : '/';
            else ShowError(r.Message);
        },
        error: function (data, ajaxOptions, thrownError) {
            CloseLoading();
            ShowError(ERR_SERVER);
        }
    });
}