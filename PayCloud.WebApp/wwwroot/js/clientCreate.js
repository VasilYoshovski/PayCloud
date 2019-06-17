var token = $(".AntiForge" + " input").val();

function getClientsList(url, elementid) {

    if (url == null) {

        url = '/admin/clients/GetClientsList';
    }
    if (elementid == null) {

        elementid = 'clientListPartial';
    }
    $.ajax({
        url: url,
        type: "get",
        success: function (data) {
            $('#' + elementid).html(data);
        },
        error: function (xhr) {
            console.log(xhr);

            $('#' + elementid).html('Error loading clients list...');
        }
    });
}




$('#create-client-form').submit(

    function (event) {
        event.preventDefault();

        //var modelg = {
        //    ClientId: $('#clientList').data('clientid'),
        //    Balance: $('#accountBalance').data('accbalance')
        //}
        //console.log(modelg);
        var clientName = $('#clientName').val();
        if (clientName.length < 3 || clientName.length > 35) {
            toastr.error("Client name must be between 3 and 35 symbols");
        }
        else {
            $.ajax({
                method: 'post',
                url: "/Admin/Clients/Create",
                headers: { 'RequestVerificationToken': token },
                data: JSON.stringify(clientName),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);

                    toastr.success(data);

                    $('#clientName').val('');
                },
                error: function (data) {
                    console.log('error');
                    console.log(data);

                    if (data.responseJSON.value == null) {
                        toastr.error("Error! Client was not created!");
                    }
                    else {
                        toastr.error(data.responseJSON.value);
                    }

                    $('#clientName').val('');
                }
            });
        }
    }
);

