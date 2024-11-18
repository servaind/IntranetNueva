$(document).ready(function() {
    $('.date-picker').datepicker({
        autoclose: true,
        format: 'mm/yyyy',
        language: 'es',
        viewMode: 1,
        minViewMode: 1,
    });

    $('#txtPeriodo').change(function() {
        location.href = '/Vehiculos/Vencimientos/?mes=' + $(this).val();
    });
});