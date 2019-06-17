var token = $(".AntiForge" + " input").val();
$(document).ready(function () {

    if (document.getElementById("createBannerPartial").style.display === "none") {
        document.getElementById("ShowHideCreatebannerPartial").innerText = "Show Create banner form";
    }
    else {
        ShowHideCreatebannerPartialForm();
    }

    getBannersList('/admin/banners/GetBannersList', 'bannersListPartial');
});


$('#bannersListPartial').click(function (event) {
    var target = $(event.target)
    console.log(event);

    if (target.hasClass('bnrTableClickHandler') && !target.is(':disabled')) {
        event.stopPropagation();
        event.preventDefault();
        console.log("part-----");
        var d = $(event.target).attr('href');
        console.log(d);
        getBannersList(d, 'bannersListPartial');
    }
});

function CallMoreModal(param) {
    var thisTarget = event.target;
    var modalType = param.substring(0, 1);
    var tempIndex = param.substring(2);
    console.log('-----mm--')
    $("#modalButonSave").val(tempIndex);
    $("#modalButonDelete").val(tempIndex);
    $("#modalButonClose").val(tempIndex);
    if ("M" === modalType) {
        $("#editImage").attr("src", $(thisTarget).data("imagelocationpath"));
        $("#editBanerID").val($(thisTarget).data("bannerid"));
        $("#editUrlLink").val($(thisTarget).data("url"));
        var startovaData = new Date($(thisTarget).data("startdate"));
        $("#editStartDate").val(startovaData);
        //alert($("#editStartDate").val());
        var krainaData = new Date($(thisTarget).data("enddate"));
        $("#editEndDate").val(krainaData);
        //alert($("#editEndDate").val());
        $("#editImageLocationPath").val($(thisTarget).data("imagelocationpath"));
        $("#editModal_Buton").click();
    }
}

document.getElementById("Banner-Type-Selector").onchange = function () {
    getBannersList('/admin/banners/GetBannersList', 'bannersListPartial');
};

function ShowHideCreatebannerPartialForm() {
    var x = document.getElementById("createBannerPartial");
    if (x.style.display === "none") {
        document.getElementById("ShowHideCreatebannerPartial").innerText = "Hide Create banner form";
        x.style.display = "block";
    } else {
        document.getElementById("ShowHideCreatebannerPartial").innerText = "Show Create banner form";
        x.style.display = "none";
    }
}

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

function deleteBannerAjax(bannerId) {
    //var thisTarget = $(event.target);
    //var bannerId = $(thisTarget).data("bannerid");

    //editBannerDialogModal("brei " + bannerId, (ans) => {
    //    alert(ans);
    //});

    if (0 < bannerId) {
        confirmDialogModal("Do you really want to delete banner with ID: " + bannerId + "?", (ans) => {
            if (ans) {
                $.ajax({
                    url: '/admin/banners/DeleteBanner',
                    headers: { 'RequestVerificationToken': token },
                    type: 'post',
                    cache: false,
                    data: {
                        id: bannerId
                    },
                    dataType: 'json',
                    success: function (data) {
                        getBannersList('/admin/banners/GetBannersList', 'bannersListPartial');
                        toastr.success(data);
                    },
                    error: function (xhr) {
                        getBannersList('/admin/banners/GetBannersList', 'bannersListPartial');
                        toastr.error("Banner not deleted! Invalid baner ID: " + bannerId + xhr.responseText);
                    }
                });
            } else {
                //toastr.success("Delete operation has been cancelled");
            }
        });
    } else {
        getBannersList('/admin/banners/GetBannersList', 'bannersListPartial');
        toastr.error("Banner not deleted! Invalid baner ID: " + bannerId);
    }
}

function getBannersList(url, elementid, sort, filter, search, pagenum) {
    console.log('ajax ban')
    if (url == null) {
        url = '/admin/banners/GetBannersList';
    }
    if (elementid == null) {
        elementid = 'bannersListPartial';
    }
    $.ajax({
        url: url,
        type: "post",
        headers: { 'RequestVerificationToken': token },
        data: {
            term: search,
            bannerTypeSelector: document.getElementById("Banner-Type-Selector").value
        },
        //data: {
        //    sortOrder: sort,
        //    currentFilter: filter,
        //    searchString: search,
        //    pageNumber: pagenum
        //},
        success: function (data) {
            console.log(data);
            console.log($('#' + elementid))
            $('#' + elementid).html(data);
        },
        error: function (xhr) {
            //console.log(xhr);

            $('#' + elementid).html('Error loading account list...');
        }
    });
}

//Preview image before upload
$("#fileUpload").on('change', function () {
    if (typeof (FileReader) != "undefined") {

        var image_holder = $("#banner-image-holder");
        image_holder.empty();

        var reader = new FileReader();

        reader.onload = function (e) {
            if (e.target.result.length < (1500 * 1340)) {
                var image = new Image();
                image.src = e.target.result;
                image.onload = function () {
                    // access image size here
                    const sizeW = 100;
                    const sizeH = sizeW;
                    if (sizeW > this.width || sizeH > this.height) {
                        document.getElementById("fileUpload").value = null;
                        document.getElementById("SelectedImageName").value = "";
                        infoDialogModal("Image should have width and height that are at least " + sizeH + "px! Current image has width of " + this.width + "px and height of " + this.height + "px", (ans) => {
                            // do nothhing
                        });
                        return;
                    } else {
                        if (((18 * this.width) > (25 * this.height)) || (this.width < this.height)) {
                            document.getElementById("fileUpload").value = null;
                            document.getElementById("SelectedImageName").value = "";
                            infoDialogModal("Height of the image should not be bigger than the width and should not be more than 28% smaller than the width.", (ans) => {
                                // do nothhing
                            });
                            return;
                        } else {
                            $("<img />", {
                                "src": e.target.result,
                                "class": "thumb-image",
                                "width": 200,
                                "height": 200
                            }).appendTo(image_holder);
                        }
                    }
                };
            } else {
                document.getElementById("fileUpload").value = null;
                document.getElementById("SelectedImageName").value = "";
                infoDialogModal("File' size should be less than 1500kB, but it is " + Math.round(e.target.result.length / 1340) + "kB!", (ans) => {
                    // do nothhing
                });
                return;
            }
        }

        image_holder.show();
        const file = $(this)[0].files[0];
        const fileType = file['type'];
        const validImageTypes = ['image/gif', 'image/jpeg', 'image/png', 'image/svg+xml'];
        if (!validImageTypes.includes(fileType)) {
            document.getElementById("fileUpload").value = null;
            document.getElementById("SelectedImageName").value = "";
            infoDialogModal("Unsupported image type! Supported types are .gif .jpeg .png .svg", (ans) => {
                // do nothhing
            });
            return;
        }

        var text1 = document.getElementById("fileUpload").value.split("\\");
        if (Array.isArray(text1)) {
            document.getElementById("SelectedImageName").value = text1[text1.length - 1];
            console.log("e masiv");
        } else {
            document.getElementById("SelectedImageName").value = text1;
            console.log("ne e masiv");
        }
        reader.readAsDataURL(file);
    } else {
        infoDialogModal("This browser does not support FileReader.", (ans) => {
            // do nothhing
        });
    }
});

function infoDialogModal(message, handler) {
    $(`<div class="modal fade" id="myInfoConfirmModal" role="dialog"> 
     <div class="modal-dialog"> 
       <!-- Modal content--> 
        <div class="modal-content"> 
           <div class="modal-body" style="padding:10px;"> 
             <h4 class="text-center">${message}</h4> 
             <div class="text-center"> 
               <a class="btn btn-default btn-close">close</a> 
             </div> 
           </div> 
       </div> 
    </div> 
  </div>`).appendTo('body');

    //Trigger the modal
    $("#myInfoConfirmModal").modal({
        backdrop: 'static',
        keyboard: false
    });

    //Pass close to callback function
    $(".btn-close").click(function () {
        handler("close");
        $("#myInfoConfirmModal").modal("hide");
    });

    //Remove the modal once it is closed.
    $("#myInfoConfirmModal").on('hidden.bs.modal', function () {
        $("#myInfoConfirmModal").remove();
    });
}
