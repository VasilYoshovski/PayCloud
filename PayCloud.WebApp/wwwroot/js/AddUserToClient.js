var token = $(".AntiForge" + " input").val();
$(document).ready(function () {
    SetAddUserToClientMode();
});

function SetAddUserToClientMode() {
    //document.getElementById("SearchEntryAddUserToClient").value = $(document).getElementById("SearchEntryRemoveUserFromClient").value;

    document.getElementById("SearchEntryAddUserToClient").style.display = "block";
    document.getElementById("SearchEntryRemoveUserFromClient").style.display = "none";
    document.getElementById("AddUserToClientBtn").style.display = "block";
    document.getElementById("RemoveUserFromClientBtn").style.display = "none";
    document.getElementById("callCreateUserModalButton").style.display = "block";

    document.getElementById("AddUserToClientTitleButton").style.opacity = 1;
    document.getElementById("RemoveUserFromClientTitleButton").style.opacity = 0.4;
}

function SetRemoveUserFromClientMode() {
    //document.getElementById("SearchEntryRemoveUserFromClient").value = $(document).getElementById("SearchEntryAddUserToClient").value;

    document.getElementById("SearchEntryAddUserToClient").style.display = "none";
    document.getElementById("SearchEntryRemoveUserFromClient").style.display = "block";
    document.getElementById("AddUserToClientBtn").style.display = "none";
    document.getElementById("RemoveUserFromClientBtn").style.display = "block";
    document.getElementById("callCreateUserModalButton").style.display = "none";

    document.getElementById("AddUserToClientTitleButton").style.opacity = 0.4;
    document.getElementById("RemoveUserFromClientTitleButton").style.opacity = 1;
}

$("#clientListAUTCP").autocomplete({
    source: function (request, response) {
        $('#clientListAUTCP').data('clientid', "");
        $.ajax({
            url: '/admin/clients/clientList',
            headers: { 'RequestVerificationToken': token },
            type: 'post',
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
            },
            error: function (data) {
                toastr.error("Controller returned invalid data: " + data.responseText);
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

        $('#clientListAUTCP').data('clientid', resultid);
        $('#clientListAUTCP').data('clientname', resultname);
        $('#clientListAUTCP').val(resultname);
        console.log($('#clientListAUTCP').data('clientid'));

        return false;
    }
});

$("#listOfUsersNotRelatedToClientAUTCP").autocomplete(
    {
        source: function (request, response) {
            var clientIdTmp = $('#clientListAUTCP').data('clientid');
            if (1 > clientIdTmp) {
                toastr.error("Select a valid client first!");
                return;
            }
            $('#listOfUsersNotRelatedToClientAUTCP').data('userid', "");
            var requestInputData =  {
                ClientId: clientIdTmp,
                Term: request["term"]
            };
            $.ajax({
                url: '/admin/PayCloudUsers/UsersNotAssignedToClient',
                headers: { 'RequestVerificationToken': token },
                type: "post",
                contentType: "application/json",
                data: JSON.stringify(requestInputData),
                cache: false,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        console.log(item.userId + ':' + item.userName);

                        return {
                            label: item.userName,
                            value: item.userId
                        }

                    }))
                },
                error: function (data) {
                    toastr.error("Controller returned invalid data: " + data.responseText);
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

            $('#listOfUsersNotRelatedToClientAUTCP').data('userid', resultid);
            $('#listOfUsersNotRelatedToClientAUTCP').data('username', resultname);
            $('#listOfUsersNotRelatedToClientAUTCP').val(resultname);
            console.log($('#listOfUsersNotRelatedToClientAUTCP').data('userid'));

            return false;
        }
    }
);

$("#listOfUsersRelatedToClientAUTCP").autocomplete(
    {
        source: function (request, response) {
            var clientIdTmp = $('#clientListAUTCP').data('clientid');
            if (1 > clientIdTmp) {
                toastr.error("Select a valid client first!");
                return;
            }
            $('#listOfUsersRelatedToClientAUTCP').data('userid', "");
            //var ggg = $("#listOfClientsAATUP").data("clientname");
            var requestInputData = {
                ClientId: clientIdTmp,
                Term: request["term"]
            };
            $.ajax({
                url: '/admin/PayCloudUsers/UsersAssignedToClient',
                headers: { 'RequestVerificationToken': token },
                type: "post",
                contentType: "application/json",
                data: JSON.stringify(requestInputData),
                success: function (data) {
                    //response ? alert("It worked! " + response) : alert("It didn't work. " + response);
                    response($.map(data, function (item) {
                        console.log(item.userId + ':' + item.userName);

                        return {
                            label: item.userName,
                            value: item.userId
                        }

                    }))
                },
                error: function (data) {
                    toastr.error("Controller returned invalid data: " + data.responseText);
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

            $('#listOfUsersRelatedToClientAUTCP').data('userid', resultid);
            $('#listOfUsersRelatedToClientAUTCP').data('username', resultname);
            $('#listOfUsersRelatedToClientAUTCP').val(resultname);
            console.log($('#listOfUsersRelatedToClientAUTCP').data('userid'));

            return false;
        }
    }
);

//function getAccountList(url, elementid, sort, filter, search, pagenum) {

//    if (url == null) {

//        url = '/admin/accounts/GetAccountsList';
//    }
//    if (elementid == null) {

//        elementid = 'acountListPartial';
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

//$('#acountListPartial').click(function (event) {
//    var target = $(event.target)

//    if (target.hasClass('accTableClickHandler') && !target.is(':disabled')) {
//        console.log("part-----");

//        event.preventDefault();
//        var a = $('.accpaging');
//        console.log(a.length);

//        var d = $(event.target).attr('href');
//        console.log(d);
//        getAccountList(d, 'acountListPartial');
//    }
//});


//$('#createAccountPartial').change(function (event) {
//    var target = $(event.target)
//    console.log("accountpart-----change");

    
//    if (target.is('#clientList')) {
//        console.log(target);
//        if (target.data('clientname') !== target.val())
//        target.data('clientid', null);
//    }
//});


//$('#acountListPartial').change(function (event) {
//    var target = $(event.target)
//    console.log("part-----change");


//    if (target.is('#accTableLength') || (target.is('#accTableSearch'))) {
//        url = '/admin/accounts/GetAccountsList'
//            + '?pageSize=' + $('#acountListPartial').find('#accTableLength').val()
//            + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
//        console.log(url);
//        getAccountList(url, 'acountListPartial');
//    }
//});

$('#add-user-to-client-form').submit(
    function (event) {
        event.preventDefault();

        console.log($('#listOfUsersNotRelatedToClientAUTCP').data("userid"));

        var modelg = {
            ClientId: $('#clientListAUTCP').data('clientid'),
            UserId: $('#listOfUsersNotRelatedToClientAUTCP').data('userid')
        }
        console.log(modelg);
        if (1 > modelg.ClientId) {
            toastr.error("Select a valid client first!");
        }
        if (1 > modelg.UserId) {
            toastr.error("Select a valid user first!");
        }
        else {
            $.ajax({
                method: 'post',
                url: "/Admin/PayCloudUsers/AddUserToClient",
                headers: { 'RequestVerificationToken': token },
                data: JSON.stringify(modelg),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);

                    $('#listOfUsersNotRelatedToClientAUTCP').data('userid', "");
                    $('#listOfUsersNotRelatedToClientAUTCP').data('username', "");
                    $('#listOfUsersNotRelatedToClientAUTCP').val("");
                    toastr.success(data.value);
                    //url = '/admin/accounts/GetAccountsList'
                    //    + '?pageSize=' + $('#acountListPartial').find('#accTableLength').val()
                    //    + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
                    //console.log(url);
                    //getAccountList(url, 'acountListPartial');

                    //$('#clientList').val('');
                },
                error: function (data) {
                    console.log('error');
                    console.log(data);

                    if (data.responseJSON.value == null) {
                        toastr.error("Error! User not assigned to client!");
                    }
                    else {
                        toastr.error(data.responseJSON.value);
                    }

                    //$('#clientList').val('');
                }
            });
        }
    }
);

function confirmDialogModal(message, handler) {
    $(`<div class="modal fade" id="myConfirmModal" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-body" style="padding:10px;">
                <div class="text-center">
                    <div class="card mb-4">
                        <div class="card-header">${message}</div>
                        <div class="card-body">
                            <div class="col-xl-12 col-lg-12  col-sm-12">
                                <div class="row">
                                    <div class="col-6 text-left">
                                        <button type="button" class="btn btn-danger mt-2 btn-yes">yes</button>
                                    </div>
                                    <div class="col-6 text-right">
                                        <button type="button" class="btn btn-default mt-2 btn-no">no</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>`).appendTo('body');

    //Trigger the modal
    $("#myConfirmModal").modal({
        backdrop: 'static',
        keyboard: false
    });

    //Pass true to a callback function
    $(".btn-yes").click(function () {
        handler(true);
        $("#myConfirmModal").modal("hide");
    });

    //Pass false to callback function
    $(".btn-no").click(function () {
        handler(false);
        $("#myConfirmModal").modal("hide");
    });

    //Remove the modal once it is closed.
    $("#myConfirmModal").on('hidden.bs.modal', function () {
        $("#myConfirmModal").remove();
    });
}

function callRemoveUserFromClientAjax(bannerId) {
    event.preventDefault();

    var userNameTemp = document.getElementById("listOfUsersRelatedToClientAUTCP").value;
    var clientNameTemp = document.getElementById("clientListAUTCP").value;

    var modelg = {
        ClientId: $('#clientListAUTCP').data('clientid'),
        UserId: $('#listOfUsersRelatedToClientAUTCP').data('userid')
    }
    console.log(modelg);
    if (1 > modelg.ClientId) {
        toastr.error("Select a valid client first!");
    }
    if (1 > modelg.UserId) {
        toastr.error("Select a valid user first!");
    }
    else {
        confirmDialogModal("Do you really want to remove user with name: " + userNameTemp + " from client with name: " + clientNameTemp + "?", (ans) => {
            if (ans) {
                $.ajax({
                    method: 'post',
                    url: "/Admin/PayCloudUsers/RemoveUserFromClient",
                    headers: { 'RequestVerificationToken': token },
                    data: JSON.stringify(modelg),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);

                        $('#listOfUsersRelatedToClientAUTCP').data('userid', "");
                        $('#listOfUsersRelatedToClientAUTCP').data('username', "");
                        $('#listOfUsersRelatedToClientAUTCP').val("");
                        toastr.success(data.value);
                        //url = '/admin/accounts/GetAccountsList'
                        //    + '?pageSize=' + $('#acountListPartial').find('#accTableLength').val()
                        //    + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
                        //console.log(url);
                        //getAccountList(url, 'acountListPartial');

                        //$('#clientList').val('');
                    },
                    error: function (data) {
                        console.log('error');
                        console.log(data);

                        if (data.responseJSON.value == null) {
                            toastr.error("Error! User not removed from client!");
                        }
                        else {
                            toastr.error(data.responseJSON.value);
                        }
                    }
                });
            } else {
                //toastr.success("Delete operation has been cancelled");
            }
        });
    }
}
