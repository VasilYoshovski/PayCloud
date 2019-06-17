var token = $(".AntiForge" + " input").val();
//$(document).ready(
//    function () {

//    }
//);

$(document).ready(function () {

//    getAccountList('/admin/accounts/GetAccountsList', 'acountListPartial');

//    $("#clientList").autocomplete({
//        source: function (request, response) {
//            $.ajax({
//                url: '/admin/clients/clientList',
//                type: 'GET',
//                cache: false,
//                data: request,
//                dataType: 'json',
//                success: function (data) {
//                    response($.map(data, function (item) {
//                        console.log(item.clientId + ':' + item.clientName);

//                        return {
//                            label: item.clientName,
//                            value: item.clientId
//                        }

//                    }))
//                }
//            });
//        },
//        minLength: 2,
//        delay: 0,
//        select: function (event, ui) {
//            event.preventDefault();
//            var resultid = ui.item.value;
//            var resultname = ui.item.label;
//            console.log(resultid);

//            $('#clientList').data('clientid', resultid);
//            $('#clientList').data('clientname', resultname);
//            $('#clientList').val(resultname);
//            console.log($('#clientList').data('clientid'));

//            return false;
//        }
//    });
});

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

//function getPayCloudClientsList(url, elementid, sort, filter, search, pagenum) {

//    if (url == null) {

//        url = '/admin/Clients/GetClientsList';
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

//function getPayCloudUsersList(url, elementid, sort, filter, search, pagenum) {

//    if (url == null) {

//        url = '/admin/PayCloudUsers/GetPayCloudUsersList';
//    }
//    if (elementid == null) {

//        elementid = 'usersListPartial';
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

//$('#createBannerPartial').change(function (event) {
//    var target = $(event.target)
//    console.log("accountpart-----change");


//    if (target.is('#clientList')) {
//        console.log(target);
//        if (target.data('clientname') !== target.val())
//            target.data('clientid', null);
//    }
//});

//$document.getElementById("#modalButonSave").on("click", function () {
//    alert("save")
//});

//$document.getElementById("#modalButonDelete").on("click", function () {
//    alert("delete")
//});

//$(".MoreButtonModal").on("click", function () {
//$('#createBannerPartial1').getElementById(".MoreButtonModal").on("click", function () {
//    var buton_value = $(this).val();
//    var modalType = buton_value.substring(0, 1);
//    var temIndex = buton_value.substring(2);
//    $("#modalButonSave").val(temIndex);
//    $("#modalButonDelete").val(temIndex);
//    $("#modalButonClose").val(temIndex);
//    if ("M" === modalType) {
//        $("#editImage").attr("src", $(this).data("imagelocationpath"));
//        //var image = document.getElementById("editImage");
//        //image.src = "image1.jpg"
//        //document.getElementById("editImage").src="../template/save.png";
//        $("#editBanerID").val($(this).data("bannerid"));
//        $("#editUrlLink").val($(this).data("url"));
//        $("#editStartDate").val($(this).data("startdate"));
//        //alert($("#editStartDate").val());
//        $("#editEndDate").val($(this).data("enddate"));
//        //alert($("#editEndDate").val());
//        $("#editImageLocationPath").val($(this).data("imagelocationpath"));
//        document.getElementById("editModal_Buton").click();
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

//$('#create-account-form').submit(

//    function (event) {
//        event.preventDefault();

//        console.log($('#accountBalance').data());

//        var modelg = {
//            ClientId: $('#clientList').data('clientid'),
//            Balance: $('#accountBalance').data('accbalance')
//        }
//        console.log(modelg);
//        if (modelg.ClientId.length == 0 || modelg.Balance == 0) {
//            toastr.error("Select client first!");
//        }
//        else {
//            $.ajax({
//                method: 'post',
//                url: "/Admin/Accounts/Create",
//                data: JSON.stringify(modelg),
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    console.log(data);

//                    toastr.success(data.value);
//                    url = '/admin/accounts/GetAccountsList'
//                        + '?pageSize=' + $('#acountListPartial').find('#accTableLength').val()
//                        + '&currentFilter=' + $('#acountListPartial').find('#accTableSearch').val();
//                    console.log(url);
//                    getAccountList(url, 'acountListPartial');

//                    $('#clientList').val('');
//                },
//                error: function (data) {
//                    console.log('error');
//                    console.log(data);

//                    if (data.value == null) {
//                        toastr.error("Error! Account was not created!");
//                    }
//                    else {
//                        toastr.error(data.value);
//                    }

//                    $('#clientList').val('');
//                }
//            });
//        }
//});
