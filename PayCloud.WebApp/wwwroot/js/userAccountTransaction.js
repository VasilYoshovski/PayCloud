$(document).ready(function () {

    $("#transactionListPartial").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    $("#mytransactionListPartial").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');

    getTransactionsList('/user/transactions/GetTransactionsList', 'transactionListPartial');

    getTransactionsList('/user/transactions/GetMyTransactionsList', 'mytransactionListPartial');
});

function getTransactionsList(url, elementid) {

    if (url == null) {

        url = '/user/transactions/GetTransactionsList';
    }
    if (elementid == null) {

        elementid = 'transactionListPartial';
    }
    $.ajax({
        url: url,
        type: "get",
        success: function (data) {
            $('#' + elementid).html(data);
        },
        error: function (xhr) {
            console.log(xhr);

            $('#' + elementid).html('Error loading transactions list...');
        }
    });
}

$('#transactionListPartial').click(function (event) {
    var target = $(event.target)

    if (target.hasClass('trnTableClickHandler') && !target.is(':disabled')) {
        console.log("part-----");

        event.preventDefault();

        var url = $(event.target).attr('href');
        console.log(url);
        getTransactionsList(url, 'transactionListPartial');
    }
});

$('#transactionListPartial').change(function (event) {
    var target = $(event.target)
    console.log("part-----change");


    if (target.is('#trnTableLength') || (target.is('#trnTableSearch'))) {
        url = '/user/transactions/GetTransactionsList'
            + '?pageSize=' + $('#transactionListPartial').find('#trnTableLength').val()
            + '&currentFilter=' + $('#transactionListPartial').find('#trnTableSearch').val();
        console.log(url);
        getTransactionsList(url, 'transactionListPartial');
    }
});

$('#mytransactionListPartial').click(function (event) {
    var target = $(event.target)

    if (target.hasClass('utrnTableClickHandler') && !target.is(':disabled')) {
        console.log("part-----");

        event.preventDefault();

        var url = $(event.target).attr('href');
        console.log(url);
        getTransactionsList(url, 'mytransactionListPartial');
    }
});

$('#mytransactionListPartial').change(function (event) {
    var target = $(event.target)
    console.log("part-----change");


    if (target.is('#utrnTableLength') || (target.is('#utrnTableSearch'))) {
        url = '/user/transactions/GetMyTransactionsList'
            + '?pageSize=' + $('#mytransactionListPartial').find('#utrnTableLength').val()
            + '&currentFilter=' + $('#mytransactionListPartial').find('#utrnTableSearch').val();
        console.log(url);
        getTransactionsList(url, 'mytransactionListPartial');
    }
});

function accountTransactions() {
    $('#mytransactionListPartial').addClass('non-visible');
    $('#transactionListPartial').removeClass('non-visible');
}

function myTransactions() {
    $('#transactionListPartial').addClass('non-visible');
    $('#mytransactionListPartial').removeClass('non-visible');
}

