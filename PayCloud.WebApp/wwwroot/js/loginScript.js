$(document).ready(function () {
    var errorMessage = $('#error-container').val();
    if (errorMessage) {
        toastr.error(errorMessage)
    }
});