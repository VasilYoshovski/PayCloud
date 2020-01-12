
var token = $(".AntiForge" + " input").val();

function transferPayment(url, paymentDto, successfunc) {


    console.log(paymentDto);

    $.ajax({
        method: 'post',
        url: url,
        headers: { 'RequestVerificationToken': token },
        data: JSON.stringify(paymentDto),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (successfunc != undefined) successfunc();
            toastr.success(data.value);
        },
        error: function (data) {
            console.log('error');
            console.log(data);

            
            if (data.responseText == null) {
                toastr.error("Error! Transaction canceled!");
            }
            else {
                toastr.error(data.responseText);
            }
        }
    });
};

function getPaymentDto() {
    var amount = $('#paymentAmount').val();
    var senderId = $('#senderaccount').data('senderaccount');
    var receiverId = $('#receiveraccount').data('receiveraccount');
    var description = $('#paymentDescription').val();
    console.log(senderId + "/" + receiverId + "/" + description);

    if (!description || !receiverId || !senderId) {
        toastr.error("All payment fields are required");
        return false;
    }

    if (amount <= 0) {
        toastr.error("Amount must be greater than 0!");
        return false;
    }

    if (senderId === receiverId) {
        toastr.error("Sender and receiver accounts can not be same!");
        return false;
    }

    var model = {
        SenderAccountId: parseInt(senderId),
        ReceiverAccountId: parseInt(receiverId),
        Amount: parseFloat(amount),
        Description: description
    }
    console.log(model);

    return model;
};

function makePayment() {
    $('#make-payment-button').prop("disabled", true);
    $('#make-payment-button').addClass('running');
    var paymentDto = getPaymentDto();
    if (paymentDto) {
        transferPayment('/user/transactions/makePayment', paymentDto, makePaymentCallBack);
    }

    $('#make-payment-button').prop("disabled", false);
    $('#make-payment-button').removeClass('running');

};

function savePayment() {
    var paymentDto = getPaymentDto();
    if (paymentDto) {
        transferPayment('/user/transactions/savePayment', paymentDto)
    }
};

function makePaymentCallBack() {
    console.log("clear->")
    clearPaymentFields();
    console.log("update->")
    updatePartialsAccountPage();
}

function updatePartialsAccountPage() {
    getAccountList('/user/accounts/GetAccountsList', 'acountListPartial');
    getChartData(true);
}

function clearPaymentFields() {

    if (!$('#senderaccount').is(':disabled')) {

        $('#senderaccount').val(null).trigger("change")
        $('#senderaccount').data('senderaccount', null);
        $('#senderaccount').data('balance', null);
        $('#receiveraccount').data('receiveraccount', null);
        $('#receiveraccount').val(null).trigger("change");
        $('#paymentAmount').val("");
        $('#paymentDescription').val(null);
    }
    else {

        var url = '/user/transactions/ShowPaymentPartial' + '?senderAccountId=' + modalAccountId;
        getPartial(url, 'mymodalmain', initializeMySelect2, modalAccountId);
    }
};

function paySavedPayment(transactionId) {
    transferPayment('/user/transactions/PaySavedPayment', transactionId, PaymentUpdate);

};

function cancelSavedPayment(transactionId) {
    transferPayment('/user/transactions/CancelSavedPayment', transactionId, PaymentUpdate);

};

function PaymentUpdate() {

    $("#transactionListPartial").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    $("#mytransactionListPartial").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');

    getTransactionsList('/user/transactions/GetTransactionsList', 'transactionListPartial');

    getTransactionsList('/user/transactions/GetMyTransactionsList', 'mytransactionListPartial');

    $("#myDialogModal").modal('hide');
}