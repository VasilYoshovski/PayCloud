$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

$('#myModalPartial').click(function (event) {
    console.log("modal-----");
    var target = $(event.target)

    if (target.hasClass('accTableClickHandler') && !target.is(':disabled')) {

        event.preventDefault();
        var a = $('.accpaging');
        console.log(a.length);

        var d = $(event.target).attr('href');
        console.log(d);
        getAccountList(d, 'mymodalmain');
    }

    if (target.hasClass('clnTableClickHandler') && !target.is(':disabled')) {

        event.preventDefault();

        var d = $(event.target).attr('href');
        console.log(d);
        getAccountList(d, 'mymodalmain');
    }

    if (target.hasClass('trnTableClickHandler') && !target.is(':disabled')) {
        console.log("part-----");

        event.preventDefault();

        var url = $(event.target).attr('href');
        console.log(url);
        getTransactionsList(url, 'mymodalmain');
    }
});

$('#myModalPartial').change(function (event) {
    console.log("modal----- change");

    var target = $(event.target);
    console.log($('#myModalPartial').find('#clnTableLength').val());
    console.log(target.val());



    if (target.is('#accTableLength') || (target.is('#accTableSearch'))) {
        var clientId = $('#clientList').data('clientid');
        console.log(clientId);
        url = '/admin/accounts/GetAccountsList'
            + '?pageSize=' + $('#myModalPartial').find('#accTableLength').val()
            + '&currentFilter=' + $('#myModalPartial').find('#accTableSearch').val()
            + '&clientId=' + clientId;
        console.log(url);
        getAccountList(url, 'mymodalmain');
    }

    if (target.is('#clnTableLength') || (target.is('#clnTableSearch'))) {
        url = '/admin/clients/GetClientsList'
            + '?pageSize=' + $('#myModalPartial').find('#clnTableLength').val()
            + '&currentFilter=' + $('#myModalPartial').find('#clnTableSearch').val();
        console.log(url);
        getAccountList(url, 'mymodalmain');
    }

    if (target.is('#trnTableLength') || (target.is('#trnTableSearch'))) {
        var clientId = $('#clientList').data('clientid');

        url = '/user/transactions/GetTransactionsList'
            + '?pageSize=' + $('#transactionListPartial').find('#trnTableLength').val()
            + '&currentFilter=' + $('#transactionListPartial').find('#trnTableSearch').val();
        console.log(url);
        getTransactionsList(url, 'mymodalmain');
    }
});

function confirmCreateUserDialogModal(message, handler) {
    $(`<div class="modal fade" id="myCreateUserModal" role="dialog"> 
     <div class="modal-dialog"> 
       <!-- Modal content--> 
        <div class="modal-content"> 
           <div class="modal-body" style="padding:10px;"> 
				<div class="card mb-4">
					<div class="card-header">Create user</div>
					<div class="card-body">
						<form class="form-horizontal row mt-2" id="create-user-form">
							<div class="col-xl-12 col-lg-12  col-sm-12">
								<div class="form-group">
									<label>UserName</label>
									<input type="text" class="form-control" placeholder="Enter user name" value="" ID="modalCreateUserName" data-username="" required autofocus="">
								</div>
								<div class="form-group">
									<label>Name</label>
									<input type="text" class="form-control" placeholder="Enter name" value="" ID="modalCreateUserNickName" data-usernickname="" required autofocus="">
								</div>
								<div class="form-group">
									<label>Password</label>
									<input type="text" class="form-control" placeholder="Enter password" value="" ID="modalCreateUserPassword" data-userpassword="" required autofocus="">
								</div>
								<div class="form-group">
									<label>Role</label>
									<input type="text" class="form-control" placeholder="Enter role" value="User" ID="modalCreateUserRole" data-userrole="User" required autofocus="" disabled>
								</div>
							</div>
							<div class="col-6 text-left">
								<button type="button" class="btn btn-default mt-2 btn-yes">Create</button>
							</div>
							<div class="col-6 text-right">
								<button type="button" class="btn btn-default mt-2 btn-no">Close</button>
							</div>
						</form>
					</div>
				</div>
           </div> 
       </div> 
    </div> 
  </div>`).appendTo('body');

    //Trigger the modal
    $("#myCreateUserModal").modal({
        backdrop: 'static',
        keyboard: false
    });

    //Pass true to a callback function
    $(".btn-yes").click(function (event) {
        event.preventDefault();
        handler(true);

        //console.log($('#SelectedImageName').data());

        var modelg = {
            // UserId: 0,
            Username: $('#modalCreateUserName').val().trim(),
            Name: $('#modalCreateUserNickName').val().trim(),
            Password: $('#modalCreateUserPassword').val(),
            Role: $("#modalCreateUserRole").val().trim()
        }
        console.log(modelg);
        if (modelg.Username.length < 5 || modelg.Username.length > 16) {
            toastr.error("Enter a valid Username first! Length should be between 5 and 16 symbols");
            return;
        }
        if (modelg.Username.length < 5 || modelg.Username.length > 16) {
            toastr.error("Enter a valid Username first! Length should be between 5 and 16 symbols");
            return;
        }
        if (modelg.Name.length < 6 || modelg.Name.length > 35) {
            toastr.error("Enter a valid NickName first! Length should be between 6 and 35 symbols");
            return;
        }
        if (modelg.Password.length < 8 || modelg.Password.length > 32 || modelg.Password.trim().length === 0) {
            toastr.error("Enter a valid Password first! Length should be between 8 and 32 symbols.");
            return;
        }
        if (modelg.Role.length < 4 || modelg.Role.length > 16) {
            toastr.error("Enter a valid Role first! Length should be between 4 and 16 symbols");
            return;
        }
        else {
            $.ajax({
                method: 'post',
                url: "/Admin/PayCloudUsers/Create",
                headers: { 'RequestVerificationToken': token },
                data: JSON.stringify(modelg),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);

                    toastr.success(data.value);
                    //url = '/admin/banners/GetBannersList'
                    //    + '?pageSize=' + $('#bannersListPartial').find('#accTableLength').val()
                    //    + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
                    //console.log(url);
                    //getAccountList(url, 'bannersListPartial');

                    //$('#clientList').val('');
                },
                error: function (data) {
                    console.log('error');
                    console.log(data);

                    if (data.responseJSON.value == null) {
                        toastr.error("Error! User not created!");
                    }
                    else {
                        toastr.error("Error! User not created! " + data.responseJSON.value);
                    }

                    //$('#clientList').val('');
                }
            });
        }
        $("#myCreateUserModal").modal("hide");
    });

    //Pass false to callback function
    $(".btn-no").click(function () {
        handler(false);
        $("#myCreateUserModal").modal("hide");
    });

    //Remove the modal once it is closed.
    $("#myCreateUserModal").on('hidden.bs.modal', function () {
        $("#myCreateUserModal").remove();
    });
}

function callCreateUserModal() {
    $('#mymodaltitle').text('Create user')

    //var clientId = $('#clientListAUTCP').data('clientid');
    //if (1 > clientId) {
    //    toastr.error("Select a valid client first");
    //    return;
    //}

    //confirmCreateUserDialogModal("Create user for client ID: " + clientId, (ans) => {
    confirmCreateUserDialogModal("Create user", (ans) => {
        //alert(ans);
    });
};

//function UsersOfClientModal() {

//    clientId = $('#clientListAUTCP').data('clientid');
//    clientName = $('#clientListAUTCP').val();

//    if (clientId > 0) {

//        $('#mymodaltitle').text('List of "' + clientName + '" users')

//        url = '/admin/PayCloudUsers/GetPayCloudUsersList' + '?clientId=' + clientId;
//        console.log(url);
//        getUsersOfClientsList(url, 'mymodalmain');

//        $("#mymodal-close-button").toggle(true);

//        $("#myModal").modal('show');
//    }
//    else {
//        toastr.error("Please select client first!")
//    }
//    //url = '/admin/PayCloudUsers/GetPayCloudUsersList';
//    //if (clientId > 0) {
//    //    $('#mymodaltitle').text('List of "' + clientName + '" users')

//    //    url = url + '?clientId=' + clientId;
//    //}
//    //else {
//    //    $('#mymodaltitle').text('List of users')
//    //}
//    //console.log(url);
//    //getUsersOfClientsList(url, 'mymodalmain');
//    //$("#myModal").modal('show');
//};

function AccountsOfUserModal() {
    clientId = $('#listOfClientsAATUP').data('clientid');
    clientName = $('#listOfClientsAATUP').val();
    userId = $('#listOfUsersRelatedToClientAATUP').data('userid');
    userName = $('#listOfUsersRelatedToClientAATUP').val();

    //if (clientId > 0) {
        if (userId > 0) {

            $('#mymodaltitle').text('List of "' + userName + '" accounts')

            url = '/admin/accounts/GetAccountsOfUserList' + '?userId=' + userId;
            console.log(url);
            getAccountsOfUserList(url, 'mymodalmain');

            $("#mymodal-close-button").toggle(true);

            $("#myModal").modal('show');
        }
        else {
            toastr.error("Please select a valid user first!")
        }
    //}
    //else {
    //    toastr.error("Please select a valid client first!")
    //}
};

function ClientAccountsModal() {

    clientId = $('#clientList').data('clientid');
    clientName = $('#clientList').val();

    if (clientId > 0) {


        $('#mymodaltitle').text('List of "' + clientName + '" accounts')
        $('#myModalSize').attr('class', 'modal-dialog-cust-big');

        url = '/admin/accounts/GetAccountsList' + '?clientId=' + clientId;
        console.log(url);
        $("#mymodalmain").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
        getAccountList(url, 'mymodalmain');

        $("#mymodal-close-button").toggle(true);

        $("#myModal").modal('show');
    }
    else {
        toastr.error("Please select client first!")
    }
};

function ShowClientListModal() {

    $('#mymodaltitle').text('Pay Cloud - Clients')
    $('#myModalSize').attr('class', 'modal-dialog-cust-mid');

    url = '/admin/clients/GetClientsList';
    console.log(url);
    $("#mymodalmain").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    getClientsList(url, 'mymodalmain');
    $("#mymodal-close-button").toggle(true);

    $("#myModal").modal('show');
};

function getPartial(url, elementid, successFunction, successParam) {
    $.ajax({
        url: url,
        type: "get",
        success: function (data) {
            $('#' + elementid).html(data);
            if (successFunction) successFunction(successParam);
        },
        error: function (xhr) {
            $('#' + elementid).html('Error loading partial...');
        }
    });
}

function showPaymentModal(event, senderAccountId) {

    event.stopPropagation();
    $('#mymodaltitle').text('Pay Cloud - Payment')
    $('#myModalSize').attr('class', 'modal-dialog-cust-mid');
    url = '/user/transactions/ShowPaymentPartial'

    if (senderAccountId) {
        url = url + '?senderAccountId=' + senderAccountId;
    }

    $("#mymodalmain").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    getPartial(url, 'mymodalmain', initializeMySelect2, senderAccountId);
    console.log('sender acc id: ' + senderAccountId);
    $("#mymodal-close-button").toggle(true);

    $("#myModal").modal('show');
};


function showTransactionsModal(accountId, accountName) {
    modalAccountId = accountId;
    $('#mymodaltitle').text('List of "' + accountName + '" transactions')
    $('#myModalSize').attr('class', 'modal-dialog-cust-big');

    url = '/user/transactions/GetTransactionsList' + '?accountId=' + accountId;
    $("#mymodalmain").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    getPartial(url, 'mymodalmain');
    $("#mymodal-close-button").toggle(true);

    $("#myModal").modal('show');
};


function showChangeNicknameModal(event, accountId, accountName) {
    event.stopPropagation();
    modalAccountId = accountId;
    $('#mymodaltitle').text('Change nickname of  account number "' + accountName + '"');
    $('#myModalSize').attr('class', 'modal-dialog-cust-mid');

    url = '/user/accounts/ChangeNickname' + '?accountId=' + accountId;
    $("#mymodalmain").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');
    getPartial(url, 'mymodalmain');

    $("#mymodal-close-button").toggle(false);

    $("#myModal").modal('show');
};

function showAccountDetailsModal(event, accountId, accountName) {
    event.stopPropagation();
    modalAccountId = accountId;
    $('#mymodaltitle').text('Account "' + accountName + '" details and history');
    $('#myModalSize').attr('class', 'modal-dialog-cust-mid');


    url = '/user/accounts/AccountDetails' + '?accountId=' + accountId;
    $("#mymodalmain").html('<div class="center"><img src="/images/loading.gif" alt="Loading..."></div>');

    getPartial(url, 'mymodalmain', getLineChartData, accountId);

    $("#mymodal-close-button").toggle(true);

    $("#myModal").modal('show');
};




