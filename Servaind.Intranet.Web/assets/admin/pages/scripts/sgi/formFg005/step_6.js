function DownloadFile(n) {
    location.href = '/Sgi/FormFg005Download/' + formNumero + '?nombre=' + n;
}

$(document).ready(function() {
    $('#btnPrint').click(function() {
        OpenLink('Sgi/FormFg005Print/' + formNumero);
    });
});