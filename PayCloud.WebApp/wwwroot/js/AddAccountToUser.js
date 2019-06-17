var token = $(".AntiForge" + " input").val();
$(document).ready(function () {
    SetAddAccountToUserMode();
});

function SetAddAccountToUserMode() {
    //document.getElementById("SearchEntryAddAccountToUser").value = document.getElementById("SearchEntryRemoveAccountFromUser").value;

    document.getElementById("SearchEntryAddAccountToUser").style.display = "block";
    document.getElementById("SearchEntryRemoveAccountFromUser").style.display = "none";
    document.getElementById("AddAccountToUserBtn").style.display = "block";
    document.getElementById("RemoveAccountFromUserBtn").style.display = "none";
    document.getElementById("callAccountsOfUserModalButton").style.display = "block";

    document.getElementById("AssignAccountToUserTitleButton").style.opacity = 1;
    document.getElementById("UnassignAccountFromUserTitleButton").style.opacity = 0.4;
}

function SetRemoveAccountFromUserMode() {
    //document.getElementById("SearchEntryRemoveAccountFromUser").value = document.getElementById("SearchEntryAddAccountToUser").value;

    document.getElementById("SearchEntryAddAccountToUser").style.display = "none";
    document.getElementById("SearchEntryRemoveAccountFromUser").style.display = "block";
    document.getElementById("AddAccountToUserBtn").style.display = "none";
    document.getElementById("RemoveAccountFromUserBtn").style.display = "block";
    document.getElementById("callAccountsOfUserModalButton").style.display = "none";

    document.getElementById("AssignAccountToUserTitleButton").style.opacity = 0.4;
    document.getElementById("UnassignAccountFromUserTitleButton").style.opacity = 1;
}

$("#listOfClientsAATUP").autocomplete({
    source: function (request, response) {
        $('#listOfClientsAATUP').data('clientid', "");
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

        $('#listOfClientsAATUP').data('clientid', resultid);
        $('#listOfClientsAATUP').data('clientname', resultname);
        $('#listOfClientsAATUP').val(resultname);
        console.log($('#listOfClientsAATUP').data('clientid'));

        return false;
    }
});

$("#listOfUsersRelatedToClientAATUP").autocomplete(
    {
        source: function (request, response) {
            var clientIdTmp = $('#listOfClientsAATUP').data('clientid');
            if (1 > clientIdTmp) {
                //toastr.error("Select a valid client first!");
                //return;
                clientIdTmp = null;
            }
            $('#listOfUsersRelatedToClientAATUP').data('userid', "");
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

            $('#listOfUsersRelatedToClientAATUP').data('userid', resultid);
            $('#listOfUsersRelatedToClientAATUP').data('username', resultname);
            $('#listOfUsersRelatedToClientAATUP').val(resultname);
            console.log($('#listOfUsersRelatedToClientAATUP').data('userid'));

            return false;
        }
    }
);

$("#listOfAccountsOfClientNotRelatedToUserAATUP").autocomplete(
    {
        source: function (request, response) {
            var clientIdTmp = $('#listOfClientsAATUP').data('clientid');
            if (1 > clientIdTmp) {
                toastr.error("Select a valid client first!");
                return;
            }
            var userIdTmp = $("#listOfUsersRelatedToClientAATUP").data("userid");
            if (1 > userIdTmp) {
                toastr.error("Select a valid user first!");
                return;
            }
            $('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountid', "");
            var requestInputData = {
                ClientId: clientIdTmp,
                UserId: userIdTmp,
                Term: request["term"]
            };
            $.ajax({
                //url: "@Url.Action("ProcessData", "Home")",
                url: '/admin/PayCloudUsers/AccountsNotAssignedToUserOfClient',
                headers: { 'RequestVerificationToken': token },
                type: "post",
                contentType: "application/json",
                data: JSON.stringify(requestInputData),
                success: function (data) {
                    //response ? alert("It worked! " + response) : alert("It didn't work. " + response);
                    response($.map(data, function (item) {
                        console.log(item.accountId + ':' + item.accountNumber);

                        return {
                            label: item.accountNumber,
                            value: item.accountId
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

            $('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountid', resultid);
            $('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountname', resultname);
            $('#listOfAccountsOfClientNotRelatedToUserAATUP').val(resultname);
            console.log($('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountid'));

            return false;
        }
    }
);

$("#listOfAccountsOfClientRelatedToUserAATUP").autocomplete(
    {
        source: function (request, response) {
            var clientIdTmp = $('#listOfClientsAATUP').data('clientid');
            if (1 > clientIdTmp) {
                toastr.error("Select a valid client first!");
                return;
            }
            var userIdTmp = $("#listOfUsersRelatedToClientAATUP").data("userid");
            if (1 > userIdTmp) {
                toastr.error("Select a valid user first!");
                return;
            }
            $('#listOfAccountsOfClientRelatedToUserAATUP').data('accountid', "");
            var requestInputData = {
                ClientId: clientIdTmp,
                UserId: userIdTmp,
                Term: request["term"]
            };
            $.ajax({
                //url: "@Url.Action("ProcessData", "Home")",
                url: '/admin/PayCloudUsers/AccountsAssignedToUserOfClient',
                headers: { 'RequestVerificationToken': token },
                type: "post",
                contentType: "application/json",
                data: JSON.stringify(requestInputData),
                success: function (data) {
                    //response ? alert("It worked! " + response) : alert("It didn't work. " + response);
                    response($.map(data, function (item) {
                        console.log(item.accountId + ':' + item.accountNumber);

                        return {
                            label: item.accountNumber,
                            value: item.accountId
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

            $('#listOfAccountsOfClientRelatedToUserAATUP').data('accountid', resultid);
            $('#listOfAccountsOfClientRelatedToUserAATUP').data('accountname', resultname);
            $('#listOfAccountsOfClientRelatedToUserAATUP').val(resultname);
            console.log($('#listOfAccountsOfClientRelatedToUserAATUP').data('accountid'));

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

$('#add-account-to-user-form').submit(
    function (event) {
        event.preventDefault();

        console.log($('#listOfUsersRelatedToClientAATUP').data("userid").value);

        var modelg = {
            ClientId: $('#listOfClientsAATUP').data('clientid'),
            UserId: $('#listOfUsersRelatedToClientAATUP').data('userid'),
            AccountId: $('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountid')
        }
        console.log(modelg);
        if (1 > modelg.ClientId) {
            toastr.error("Select a valid client first!");
            return;
        }
        if (1 > modelg.UserId) {
            toastr.error("Select a valid user first!");
            return;
        }
        if (1 > modelg.AccountId) {
            toastr.error("Select a valid account first!");
        }
        else {
            $.ajax({
                method: 'post',
                url: "/Admin/PayCloudUsers/AddAccountToUser",
                headers: { 'RequestVerificationToken': token },
                data: JSON.stringify(modelg),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);

                    $('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountid', "");
                    $('#listOfAccountsOfClientNotRelatedToUserAATUP').data('accountname', "");
                    $('#listOfAccountsOfClientNotRelatedToUserAATUP').val("");
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
                        toastr.error("Error! Account is not assigned to User!");
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

function callRemoveAccountFromUserAjax() {
    event.preventDefault();

    var userNameTemp = document.getElementById("listOfUsersRelatedToClientAATUP").value;
    var accountNumberTemp = document.getElementById("listOfAccountsOfClientRelatedToUserAATUP").value;

    var modelg = {
        ClientId: $('#listOfClientsAATUP').data('clientid'),
        UserId: $('#listOfUsersRelatedToClientAATUP').data('userid'),
        AccountId: $('#listOfAccountsOfClientRelatedToUserAATUP').data('accountid')
    }
    console.log(modelg);
    if (1 > modelg.ClientId) {
        toastr.error("Select a valid client first!");
        return;
    }
    if (1 > modelg.UserId) {
        toastr.error("Select a valid user first!");
        return;
    }
    if (1 > modelg.AccountId) {
        toastr.error("Select a valid account first!");
    }
    else {
        confirmDialogModal("Do you really want to remove account with name: " + accountNumberTemp + " from user with name: " + userNameTemp + "?", (ans) => {
            if (ans) {
                $.ajax({
                    method: 'post',
                    url: "/Admin/PayCloudUsers/RemoveAccountFromUser",
                    headers: { 'RequestVerificationToken': token },
                    data: JSON.stringify(modelg),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);

                        $('#listOfAccountsOfClientRelatedToUserAATUP').data('accountid', "");
                        $('#listOfAccountsOfClientRelatedToUserAATUP').data('accountname', "");
                        $('#listOfAccountsOfClientRelatedToUserAATUP').val("");
                        toastr.success(data.value);
                    },
                    error: function (data) {
                        console.log('error');
                        console.log(data);

                        if (data.responseJSON.value == null) {
                            toastr.error("Error! Account is not removed from User!");
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
