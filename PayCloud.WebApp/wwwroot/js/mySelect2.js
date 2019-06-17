$(document).ready(function () {

});

function initializeMySelect2(sender) {
    console.log(sender);
    showSelect2('receiveraccount', "Select receiver account", '/user/accounts/AllAccountsList', false);
    if (!sender) showSelect2('senderaccount', "Select your account", '/user/accounts/UserAccountsList', true);
};

var myAccounts = [];

function showSelect2(elementid, placeholder, url, isMyAccounts) {
    $("#" + elementid).select2({
        minimumInputLength: 0,
        width: '100%',
        placeholder: placeholder,
        allowClear: true,
        ajax: {
            url: url,
            type: 'GET',
            cache: false,
            data: function (params) {
                return {
                    term: params.term
                }
            },
            dataType: 'json',
            processResults: function (data) {

                if (isMyAccounts) {
                    myAccounts = data;
                }

                return {
                    results: $.map(data, function (item) {
                        if (isMyAccounts) {
                            var mytext = item.clientName + ": " + item.accountNumber + " (balance: " + item.balance + ")";
                        }
                        else {
                            var mytext = item.clientName + ": " + item.accountNumber;
                        }
                        return {
                            id: item.accountId,
                            text: mytext
                        }
                    })
                };
            },
        },
        delay: 1000
    }).on("change", function (e) {
        $(this).data(elementid, e.target.value);
    });
}