function PaymentDialogShow(event, transactId) {
    event.stopPropagation();
    $('#myDialogModal-title').text('Payment options');
    $('#myDialogModal-main').text('What do you want to do with this transaction?');


    $("#myDialogModal-button1").text('Send it');
    $("#myDialogModal-button2").text('Cancel it');
    $('#myDialogModal-button1').on('click', function () { paySavedPayment(transactId); });
    $('#myDialogModal-button2').on('click', function () { cancelSavedPayment(transactId); });


    $("#myDialogModal-button1").toggle(true);
    $("#myDialogModal-button2").toggle(true);

    $("#myDialogModal").modal('show');
}



