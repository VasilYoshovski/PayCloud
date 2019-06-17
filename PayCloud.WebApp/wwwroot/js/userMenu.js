$(document).ready(function () {
    var selectedItem = sessionStorage.getItem("usermenuselected");

    if (!selectedItem) {

        sessionStorage.setItem("usermenuselected", 'menu-item1');
    }

    var text = sessionStorage.getItem("usermenuselected");
    var item = $('#' + text);

    var prevselectedItem = item.parent().find('.active');
    prevselectedItem.removeClass('active');

    item.addClass('active');
});

function menuClick(caller) {

    sessionStorage.setItem("usermenuselected", caller);

    return true;
};

function userRemoveSessionSelect() {
    sessionStorage.removeItem("usermenuselected");
   // window.location.href = '/user/identity/logout';
};

//function userLogout() {
//    sessionStorage.removeItem("usermenuselected");
//   // window.location.href = '/admin/identity/logout';
//};