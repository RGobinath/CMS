function FillGradeByCampus() {
    var Campus = [];
    Campus = $("#ddlBranchCode").val();
    var ddlbc = $("#ddlGrade");
    if (Campus != null && Campus != "") {
        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              if ($("#Grade").val() == "All") {
                  ddlbc.append($('<option/>', { value: "All", text: "All", selected: true }));
              }
              else {
                  ddlbc.append($('<option/>', { value: "All", text: "All" }));
              }
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Text == $('#Grade').val()) {
                      ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                  }
                  else {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  }
              });
          });
    }
    if (Campus == "" || Campus == null) {
        ddlbc.empty;
        ddlbc.append($('<option/>',
    {
        value: "",
        text: "Select One"

    }));
    }
}
$(function () {
    jQuery('[data-toggle="tooltip"]').tooltip();
    //$('#File').ace_file_input();
    $('#cancel').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });
    //var backlink = '<a href="/Notify/ViewAlbumDetails?pgpreid=' + $("#PGPreId").val() + '" style="text-decoration:underline;color:blue;">Back</a>';
    //$('#back').append(backlink);
    $("#ddlBranchCode").change(function () {
        FillGradeByCampus();
    });
    window.onbeforeunload = function (e) { processBusy.dialog('open'); $(".ui-dialog-titlebar").hide(); };

    $(".ui-dialog-titlebar").hide();

    $.getJSON("/Notify/FillCampusList",
     function (fillbc) {
         var ddlbc = $("#ddlBranchCode");
         ddlbc.empty();
         ddlbc.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

         $.each(fillbc, function (index, itemdata) {
             if (itemdata.Text == $('#Campus').val()) {
                 ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
             }
             else {
                 ddlbc.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
             }
         });
         FillGradeByCampus();
     });

    $('.story-small img').each(function () {


        var maxWidth = 100; // Max width for the image
        var maxHeight = 100;    // Max height for the image
        var ratio = 0;  // Used for aspect ratio
        var width = $(this).width();    // Current image width
        var height = $(this).height();  // Current image height

        alert(width);
        alert(height);

        // Check if the current width is larger than the max
        if (width > maxWidth) {
            ratio = maxWidth / width;   // get ratio for scaling image
            $(this).css("width", maxWidth); // Set new width
            $(this).css("height", height * ratio);  // Scale height based on ratio
            height = height * ratio;    // Reset height to match scaled image
            width = width * ratio;    // Reset width to match scaled image
        }

        // Check if current height is larger than max
        if (height > maxHeight) {
            ratio = maxHeight / height; // get ratio for scaling image
            $(this).css("height", maxHeight);   // Set new height
            $(this).css("width", width * ratio);    // Scale width based on ratio
            width = width * ratio;    // Reset width to match scaled image
        }
    });



    $.ajax({
        url: "/Notify/TotalViewAlbumPhotos?PGPreId=" + $('#PGPreId').val(),

        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {

            $.each(data, function (key, imgid) {
                debugger;
                var imgSet = "";
                var str = imgid.includes("*");
                if (str == true) {
                    var NewViewids = imgid.split("*");
                    imgSet = '<a href="/Notify/ViewFolderDetails?pgPreId=' + $("#PGPreId").val() + '&Id=' + NewViewids[0] + '" data-toggle=tooltip data-placement=bottom title="' + NewViewids[1] + '"style="word-wrap:break-word;word-break: break-all;width:100px;height:100px" onmousedown=WhichEvent(event,"' + NewViewids[0] + '")><img src="../../Images/folder-icon.png"style="width:80px; height:80px; resize:both;" border="2"/><br><span>'
                    imgSet += NewViewids[2];
                    imgSet += '</span></a>';
                }
                else {
                    imgSet = '<span>&nbsp;<img src="ViewAlbumPhotos?id=';
                    imgSet += imgid;
                    imgSet += '&PGPreId=';
                    imgSet += $('#PGPreId').val();
                    imgSet += '" style="width:100px; height:100px; resize:both;" border="2" onclick="DeleteImage(';
                    imgSet += imgid;
                    imgSet += ')" /></span>&nbsp;&nbsp;';
                }
                $('#ViewAlbumImages').append(imgSet);

            });

        }

    });
    $("#UploadedPhotos").hide();
    $("#NewFolder").hide();
    $("#btnNewFolder").hide();
    //$("#btndelFolder").hide();
    $("#ddlAction").change(function () {
        var Action = $("#ddlAction").val();
        if (Action == "UploadPhotos") {
            $("#UploadedPhotos").show();
            $("#NewFolder").hide();
            $("#btnNewFolder").hide();
            //$("#btndelFolder").hide();
        }
        else if (Action == "NewFolder" || Action == "DeleteFolder") {
            $("#NewFolder").show();
            if (Action == "NewFolder") {
                $("#btnNewFolder").show();
                // $("#btndelFolder").hide();
            }
            //if (Action == "DeleteFolder") {
            //    $("#btnNewFolder").hide();
            //    $("#btndelFolder").show();
            //    FolderNameAutoComplete();
            //}
            $("#UploadedPhotos").hide();
        }
        else {
            $("#NewFolder").hide();
            $("#btnNewFolder").hide();
            //$("#btndelFolder").hide();
            $("#UploadedPhotos").hide();
        }
    });
    var mytext = document.getElementById("txtFolderName");
    mytext.addEventListener('keypress', function (evt) {
        if (evt.which == 58 || evt.which == 42 || evt.which == 47 || evt.which == 63 || evt.which == 60 || evt.which == 62 || evt.which == 92 || evt.which == 124 || evt.which == 34) {
            evt.preventDefault();
            return false;
        }

    });
});



function Checkfiles() {

    var inp = document.getElementById('File');

    var imgCount = inp.files.length;

    var cnt = 0;

    for (var i = 0; i < inp.files.length; i++) {

        var fname = inp.files.item(i).name;
        // alert("here is a file name: " + fname);

        if (fname != "") {
            // code to get File Extension..
            var arr1 = new Array;
            arr1 = fname.split("\\");
            var len = arr1.length;
            var img1 = arr1[len - 1];
            var filext = img1.substring(img1.lastIndexOf(".") + 1);
            // Checking Extension
            if (filext == "jpg" || filext == "jpeg" || filext == "gif" || filext == "bmp" || filext == "png" || filext == "JPG" || filext == "JPEG" || filext == "BMP" || filext == "GIF" || filext == "PNG") {
                cnt++;
            }
            else {
                ErrMsg("Invalid Image File Selected");
                return false;
            }
        }

    }

    if (imgCount == cnt) {
        return true;
    }
}


function DeleteImage(id) {


    var res = confirm("Do you really want to delete this Image?");

    if (res == true) {

        $.ajax({

            url: '/Notify/DeletePhotosFromAlbum?Id=' + id,
            type: 'POST',
            dataType: 'json',
            traditional: true,
            success: function (data) {

                window.location.href = "/Notify/ViewAlbumDetails?pgpreid=" + $('#PGPreId').val();
            }
        });

    }
    return false;
}

function uploaddoc2() {
    if (document.getElementById("File").value == "") {
        ErrMsg("Please Browse a Photo");
    } else if (Checkfiles() == false) {
        ErrMsg("Invalid Image File Selected");
    }
    else {
        splitstr = splitstr + $('#File').val().split('\\');

        $.ajaxFileUpload({

            url: "/Notify/PhotoGalleryFilesUpload?PGPreId=" + $('#PGPreId').val(),
            secureuri: false,
            fileElementId: 'File',
            dataType: 'json',
            success: function (data, success) {
                var div = document.getElementById('Uploadedfiles');

                if ((div.innerHTML == 'Uploaded Files &nbsp;&nbsp;&nbsp;  ')) {
                    div.innerHTML = div.innerHTML + data.result;
                }
                else {
                    div.innerHTML = div.innerHTML + ', ' + data.result;
                }

                $('#ViewAlbumImages').html("");

                $.ajax({
                    url: "/Notify/TotalViewAlbumPhotos?PGPreId=" + $('#PGPreId').val(),
                    mtype: 'GET',
                    async: false,
                    datatype: 'json',
                    success: function (data) {
                        $.each(data, function (key, imgid) {
                            var imgSet = "";
                            var str = imgid.includes("*");
                            if (str == true) {
                                var NewViewids = imgid.split("*");
                                imgSet = '<a href="/Notify/ViewFolderDetails?pgPreId=' + $("#PGPreId").val() + '&Id=' + NewViewids[0] + '" data-toggle=tooltip data-placement=bottom title="' + NewViewids[1] + '"style="word-wrap:break-word;word-break: break-all;width:100px;height:100px" onmousedown=WhichEvent(event,"' + NewViewids[0] + '")><img src="../../Images/folder-icon.png"style="width:80px; height:80px; resize:both;" border="2"/><br><span>'
                                imgSet += NewViewids[2];
                                imgSet += '</span></a>';
                            }
                            else {
                                imgSet = '<span>&nbsp;<img src="ViewAlbumPhotos?id=';
                                imgSet += imgid;
                                imgSet += '&PGPreId=';
                                imgSet += $('#PGPreId').val();
                                imgSet += '" style="width:100px; height:100px; resize:both;" border="2" onclick="DeleteImage(';
                                imgSet += imgid;
                                imgSet += ')" /></span>&nbsp;&nbsp;';
                            }
                            $('#ViewAlbumImages').append(imgSet);

                        });

                    }

                });

            }
        });

        $('#clear2').html($('#clear2').html());
    }
}

var splitstr = "";

function resethtml2() {
    //    alert('hre');
    $('#clear2').html($('#clear2').html());
    var div = document.getElementById('Uploadedfiles');
    div.innerHTML = 'Uploaded Files : &nbsp;&nbsp;&nbsp;  ';
    $.ajax({
        url: '/Notify/PhotoGalleryDeleteUploadedFiles/',
        type: 'POST',
        dataType: 'json',
        traditional: true
    });
}
function CreateNewFolder() {
    var FolderName = $("#txtFolderName").val();
    if (FolderName == "") {
        ErrMsg("Please Fill the Folder Name");
        return false;
    }
    $.ajax({
        url: "/Notify/CreateNewFolderPG?PGPreId=" + $('#PGPreId').val() + '&FolderName=' + $("#txtFolderName").val() + '&ParentPKId=' + $("#ParentPKId").val(),
        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {
            if (data == "success") {
                $.ajax({
                    url: "/Notify/TotalViewAlbumPhotos?PGPreId=" + $('#PGPreId').val(),
                    mtype: 'GET',
                    async: false,
                    datatype: 'json',
                    success: function (data) {
                        $("#txtFolderName").val("");
                        $('#ViewAlbumImages').html("");
                        $.each(data, function (key, imgid) {                            
                            var imgSet = "";
                            var str = imgid.includes("*");
                            if (str == true) {
                                var NewViewids = imgid.split("*");                                
                                imgSet = '<a href="/Notify/ViewFolderDetails?pgPreId=' + $("#PGPreId").val() + '&Id=' + NewViewids[0] + '" data-toggle=tooltip data-placement=bottom title="' + NewViewids[1] + '"style="word-wrap:break-word;word-break: break-all;width:100px;height:100px" onmousedown=WhichEvent(event,"' + NewViewids[0] + '")><img src="../../Images/folder-icon.png"style="width:80px; height:80px; resize:both;" border="2"/><br><span>'
                                imgSet += NewViewids[2];
                                imgSet += '</span></a>';
                            }
                            else {
                                imgSet = '<span>&nbsp;<img src="ViewAlbumPhotos?id=';
                                imgSet += imgid;
                                imgSet += '&PGPreId=';
                                imgSet += $('#PGPreId').val();
                                imgSet += '" style="width:100px; height:100px; resize:both;" border="2" onclick="DeleteImage(';
                                imgSet += imgid;
                                imgSet += ')" /></span>&nbsp;&nbsp;';
                            }
                            $('#ViewAlbumImages').append(imgSet);

                        });

                    }
                });

            }
            if (data == "failed") {
                return ErrMsg("The Folder is Already Exist");
            }
        }
    });
}
//function DeleteFolder() {
//    var FolderName = $("#txtFolderName").val();
//    if (FolderName == "") {
//        ErrMsg("Please Fill the Folder Name");
//        return false;
//    }
//    $.ajax({
//        url: "/Notify/DeleteFolder?PGPreId=" + $('#PGPreId').val() + '&FolderName=' + $("#txtFolderName").val(),
//        mtype: 'GET',
//        async: false,
//        datatype: 'json',
//        success: function (data) {
//            if (data == "success") {
//                $.ajax({
//                    url: "/Notify/TotalViewAlbumPhotos?PGPreId=" + $('#PGPreId').val(),
//                    mtype: 'GET',
//                    async: false,
//                    datatype: 'json',
//                    success: function (data) {
//                        $('#ViewAlbumImages').html("");
//                        $.each(data, function (key, imgid) {
//                            var Viewids = imgid.split(",");
//                            var IsFolder = [];
//                            IsFolder = imgid.split("-");
//                            var FolderName = imgid.slice(7);
//                            var imgSet = "";
//                            if (IsFolder[0] == "Folder") {
//                                //imgSet = '<a href=""><img src="../../Images/folder-icon.png"style="width:100px; height:100px; resize:both;" border="2"/><span>'
//                                imgSet = '<a href="/Notify/ViewFolderDetails?pgPreId=' + $("#PGPreId").val() + '&FolderName=' + FolderName + '" ><img src="../../Images/folder-icon.png"style="width:100px; height:100px; resize:both;" border="2"/><span>'
//                                imgSet += FolderName;
//                                imgSet += '</span></a>';
//                            }
//                            else {
//                                imgSet = '<span>&nbsp;<img src="ViewAlbumPhotos?id=';
//                                imgSet += Viewids;
//                                imgSet += '&PGPreId=';
//                                imgSet += $('#PGPreId').val();
//                                imgSet += '" style="width:100px; height:100px; resize:both;" border="2" onclick="DeleteImage(';
//                                imgSet += Viewids;
//                                imgSet += ')" /></span>&nbsp;&nbsp;';
//                            }
//                            $('#ViewAlbumImages').append(imgSet);
//                        });

//                    }
//                });

//            }
//            if (data == "failed") {
//                return ErrMsg("The Folder doesn't Exist");
//            }
//        }
//    });
//}
//function FolderNameAutoComplete() {
//    var Action = $("#ddlAction").val();
//    var PGPreId = $("#PGPreId").val();
//    if (Action == "DeleteFolder") {
//        $("#txtFolderName").autocomplete({
//            source: function (request, response) {
//                $.ajax({
//                    url: "/Notify/FolderNameAutoComplete",
//                    type: "POST",
//                    dataType: "json",
//                    data: { term: request.term, Id: PGPreId },
//                    success: function (data) {
//                        response($.map(data, function (item) {
//                            return { label: item.FolderName, value: item.FolderName };
//                        }))
//                    }
//                })
//            },
//            messages: {
//                noResults: "", results: ""
//            }
//        });
//        return false;
//    }
//}
function WhichEvent(event, Id) {
    debugger;
    if (event.button == 2) {
        var res = confirm("Do you really want to delete this Folder?");
        if (res == true) {
            $.ajax({

                url: '/Notify/DeleteFolder?PGPreId=' + $("#PGPreId").val() + '&Id=' + Id,
                type: 'POST',
                dataType: 'json',
                traditional: true,
                success: function (data) {

                    window.location.href = "/Notify/ViewAlbumDetails?pgpreid=" + $('#PGPreId').val();
                }
            });
        }
    }
    if (event.button == 0) {
        return true;
    }
    return false;
}