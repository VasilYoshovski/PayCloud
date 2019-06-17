var token = $(".AntiForge" + " input").val();



function changeNickname(accountid) {
    var nickname = $('#nickname-input').val();
    if (nickname) {

        $.ajax({
            method: 'post',
            url: '/user/accounts/ChangeNickname',
            headers: { 'RequestVerificationToken': token },
            data: JSON.stringify({
                accountId: accountid,
                nickname: nickname
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log('#nickname-field-' + accountid + ":" + nickname);
                getChartData(true);
                $('#nickname-field-' + accountid).text(nickname);
                $("#myModal").modal('hide');

                toastr.success(data.value);
            },
            error: function (data) {
                console.log('error');
                console.log(data);

                if (data.responseJSON.value == null) {
                    toastr.error("Error! Nickname was not changed!");
                }
                else {
                    toastr.error(data.responseJSON.value);
                }
            }
        });
    }
    else {
        toastr.error("Error! Nickname can not be empty!");

    }
};
