//Scrpipts only for User area!

$(document).ready(function () {
    $("#acountListPartial").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    getAccountList('/user/accounts/GetAccountsList', 'acountListPartial');
});


function getAccountList(url, elementid) {

    if (url == null) {

        url = '/user/accounts/GetAccountsList';
    }
    if (elementid == null) {

        elementid = 'acountListPartial';
    }
    $.ajax({
        url: url,
        type: "get",
        success: function (data) {
            $('#' + elementid).html(data);
        },
        error: function (xhr) {
            $('#' + elementid).html('Error loading account list...');
        }
    });
};


$('#acountListPartial').click(function (event) {
    var target = $(event.target)

    if (target.hasClass('accTableClickHandler') && !target.is(':disabled')) {
        console.log("part-----");

        event.preventDefault();
        var a = $('.accpaging');
        console.log(a.length);

        var d = $(event.target).attr('href');
        console.log(d);
        getAccountList(d, 'acountListPartial');
    }
});

//$('#acountListPartial').on('click', "tr", function () {
//    console.log($(this).text());
//});)

$('#acountListPartial').change(function (event) {
    var target = $(event.target)
    console.log("part-----change");


    if (target.is('#accTableLength') || (target.is('#accTableSearch'))) {
        url = '/user/accounts/GetAccountsList'
            + '?pageSize=' + $('#acountListPartial').find('#accTableLength').val()
            + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
        console.log(url);
        getAccountList(url, 'acountListPartial');
    }
});
