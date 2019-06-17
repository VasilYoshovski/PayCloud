var token = $(".AntiForge" + " input").val();
$(document).ready(function () {

//    var date = new Date();
//    var currentDate = date.toISOString().slice(0, 10);
//    document.getElementById('StartDate').value = currentDate;
//    document.getElementById('EndDate').value = currentDate;

//    UsersOfClientModal('/admin/accounts/GetAccountsList', 'acountListPartial');

});

$("#clientListAUTCP").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/admin/clients/clientList',
            type: 'GET',
            cache: false,
            data: request,
            dataType: 'json',
            success: function (data) {
                response($.map(data, function (item) {
                    console.log(item.clientId + ':' + item.clientName);

                    return {
                        label: item.clientName,
                        value: item.clientId
                    }

                }))
            }
        });
    },
    minLength: 2,
    delay: 0,
    select: function (event, ui) {
        event.preventDefault();
        var resultid = ui.item.value;
        var resultname = ui.item.label;
        console.log(resultid);

        $('#clientList').data('clientid', resultid);
        $('#clientList').data('clientname', resultname);
        $('#clientList').val(resultname);
        console.log($('#clientList').data('clientid'));

        return false;
    }
});

//function getUsersOfClientsList(url, elementid, sort, filter, search, pagenum) {

//    if (url == null) {

//        url = '/admin/PayCloudUsers/GetPayCloudUsersList' + '?clientId=' + clientId;
//    }
//    if (elementid == null) {

//        elementid = 'payCloudUsersListPartial';
//    }
//    $.ajax({
//        url: url,
//        type: "get",
//        //data: {
//        //    sortOrder: sort,
//        //    currentFilter: filter,
//        //    searchString: search,
//        //    pageNumber: pagenum
//        //},
//        success: function (data) {
//            //console.log(data);
//            $('#' + elementid).html(data);
//        },
//        error: function (xhr) {
//            console.log(xhr);

//            $('#' + elementid).html('Error loading account list...');
//        }
//    });
//}

//$('#create-user-form').submit(
//    function (event) {
//        event.preventDefault();

//        //console.log($('#SelectedImageName').data());

//        var modelg = {
//            UserId: 0,
//            Name: $('#modalCreateUserName').val(),
//            Username: $('#modalCreateUserNickName').val(),
//            Password: $('#modalCreateUserPassword').val(),
//            Role: $("#modalCreateUserRole").val()
//        }
//        console.log(modelg);
//        if (modelg.Name.length < 1 || modelg.Username.length < 1) {
//            toastr.error("Select yututuytuytuytutuy first!");
//        }
//        else {
//            $.ajax({
//                method: 'post',
//                url: "/Admin/PayCloudUsers/Create",
//                headers: { 'RequestVerificationToken': token },
//                data: JSON.stringify(modelg),
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    console.log(data);

//                    toastr.success(data.value);
//                    //url = '/admin/banners/GetBannersList'
//                    //    + '?pageSize=' + $('#bannersListPartial').find('#accTableLength').val()
//                    //    + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
//                    //console.log(url);
//                    //getAccountList(url, 'bannersListPartial');

//                    //$('#clientList').val('');
//                },
//                error: function (data) {
//                    console.log('error');
//                    console.log(data);

//                    if (data.responseJSON.value == null) {
//                        toastr.error("Error! Banner was not created!");
//                    }
//                    else {
//                        toastr.error("Error! Banner was not created! " + data.responseJSON.value);
//                    }

//                    //$('#clientList').val('');
//                }
//            });
//        }
//    }
//);

//function createUserFromForm() {
//    var target = $(event.target);

//    var modelg = {
//        UserId: 0,
//        Name: $('#modalCreateUserName').val(),
//        Username: $('#modalCreateUserNickName').val(),
//        Password: $('#modalCreateUserPassword').val(),
//        Role: $("#modalCreateUserRole").val()
//    }
//    console.log(modelg);
//    if (modelg.Name.length < 1 || modelg.Username.length < 1) {
//        toastr.error("Select yututuytuytuytutuy first!");
//    }
//    else {
//        $.ajax({
//            method: 'post',
//            url: "/Admin/PayCloudUsers/Create",
//            headers: { 'RequestVerificationToken': token },
//            data: JSON.stringify(modelg),
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                console.log(data);

//                toastr.success(data.value);
//                //url = '/admin/banners/GetBannersList'
//                //    + '?pageSize=' + $('#bannersListPartial').find('#accTableLength').val()
//                //    + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
//                //console.log(url);
//                //getAccountList(url, 'bannersListPartial');

//                //$('#clientList').val('');
//            },
//            error: function (data) {
//                console.log('error');
//                console.log(data);

//                if (data.responseJSON.value == null) {
//                    toastr.error("Error! Banner was not created!");
//                }
//                else {
//                    toastr.error("Error! Banner was not created! " + data.responseJSON.value);
//                }

//                //$('#clientList').val('');
//            }
//        });
//    }
//};
