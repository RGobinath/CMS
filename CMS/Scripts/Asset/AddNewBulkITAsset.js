function FillBlockByCampus() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var ddlbc = $("#ddlNewBlock");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStaffGroupByCampus() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var ddlbc = $("#ddlNewStaffGroup");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Base/StaffGroupByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillDepatmentByCampusandGroup() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var ddlbc = $("#ddlNewDepartment");
    if (Campus != "" && Campus != null && StaffGroup != "" && StaffGroup != null) {
        $.getJSON("/Base/DepatmentByCampusandGroup?Campus=" + Campus + '&GroupName=' + StaffGroup,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillLocationByCampusWithBlock() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var Block = $("#ddlNewBlock").val();
    var ddlbc = $("#ddlNewLocation");
    if (Campus != "" && Campus != null && Block != "" && Block != null) {
        $.getJSON("/Asset/GetLocationByCampusWithBlock?Campus=" + Campus + '&Block=' + Block,
        function (fillbc) {
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select" }));
            $.each(fillbc, function (index, itemdata) {
                ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
            });
        });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillGradeByCampus() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var ddlbc = $("#ddlNewGrade");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStaffNameByCampusWithGroupandDepartment() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var Department = $("#ddlNewDepartment").val();
    var ddlbc = $("#ddlNewStaffName");
    if (Campus != "" && Campus != null && StaffGroup != "" && StaffGroup != null) {
        $.getJSON("/Base/StaffNameByCampusWithGroupandDepartment?Campus=" + Campus + '&GroupName=' + StaffGroup + '&Department=' + Department,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStudNameByCampusWithGrade() {
    var Campus = $("#ddlNewCampus option:selected").text();
    var Grade = $("#ddlNewGrade").val();
    var ddlbc = $("#ddlNewStudentName");
    if (Campus != "" && Campus != null && Grade != "" && Grade != null) {
        $.getJSON("/Base/StudentNameByCampusWithGrade?Campus=" + Campus + '&Grade=' + Grade,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
jQuery(function ($) {
//    $("#StaffDiv").hide();
//    $("#StudentDiv").hide();
    var spec;
    var AssetId = $('#assetId').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Asset/GetSpecificationlistByAsset?AssetId=' + AssetId,
        success: function (data) {
            spec = data;
        }
    });
    $('#btnAddAsset').click(function () {
        debugger;
        var vals = [];
        var AssetId = $('#assetId').val();
        var Campus = $('#ddlNewCampus').val();
        //var AssetCode = $('#assetCode').val();
        var Location = $('#ddlNewLocation').val();
        var AssetType = $('#assetNameId').val();
        var SerialNumber = $('#assetSerial').val();
        var Model = $('#assetModel').val();
        var Make = $('#assetMake').val();
        var Block = $('#ddlNewBlock').val();
        var UserType = $('#ddlUserType').val();
        var IdNum = $('#IdNum').val();
        for (var i = 0; i < spec.length; i++) {
            vals.push($(document.getElementById(spec[i])).val());
            if (i > 0) {
                if (vals[i] == "" || vals[i] == "undefined" || vals[i] == null) {
                    ErrMsg("All fields are required and it cannot be empty!!");
                    return false;
                }
            }
        }
        if (UserType == "Student") {
            if ($("#ddlNewGrade").val() == "") {
                return ErrMsg("Please Fill Grade");
            }
            if ($("#ddlNewStudentName").val() == "") {
                return ErrMsg("Please Fill Student Name");
            }
            IdNum = $("#ddlNewStudentName").val();
        }
        if (UserType == "Staff") {
            if ($("#ddlNewStaffGroup").val() == "") {
                return ErrMsg("Please Fill Staff Group");
            }
            if ($("#ddlNewDepartment").val() == "") {
                return ErrMsg("Please Fill Department");
            }
            if ($("#ddlNewStaffName").val() == "") {
                return ErrMsg("Please Fill StaffName");
            }
            IdNum = $("#ddlNewStaffName").val();
        }
        if (AssetId == 'undefined' || AssetId == 'undefined' || Campus == " " || Location == "" || Model == "" || Make == "" || UserType == "" || Block == "" || spec.length != vals.length) {
            ErrMsg('All fields are required and it cannot be empty!!');
            return false;
        }
        else {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                async: false,
                url: '/Asset/AddAssetDetails?SpecDetails=' + vals + '&FormId=' + Campus,
                data: { Location: Location, Asset_Id: AssetId, AssetType: AssetType, SerialNo: SerialNumber, Model: Model, Make: Make, IdNum: IdNum, UserType: UserType, FromBlock: Block },
                success: function (data) {
                    if (data == true) {
                        SucessMsg('Asset added successfully!!');
                        $('#ddlNewCampus').val("");
                        $('#assetMake').val("");
                        $('#assetModel').val("");
                        $('#assetSerial').val("");
                        $('#assetLocation').val("");
                        $("#ddlNewStudentName").val("");
                        $("#ddlNewGrade").val("");
                        $("#ddlNewStaffGroup").val("");
                        $("#ddlNewDepartment").val("");
                        $("#ddlNewStaffName").val("");
                        $("#ddlNewBlock").val("");
                        for (var i = 0; i < spec.length; i++) {
                            vals.push($('#' + spec[i]).val(""));
                        }
                        reloadGrid();
                        return true;
                    }
                    else {
                        ErrMsg('Already Exist');
                        return false
                    }
                }
            });
        }
    });

    $('#btnExit').click(function () {
        $('#newITAsset').dialog('close');
        reloadGrid();
    });
    //$("#ddlUserType").change(function () {
    //    if ($("#ddlUserType").val() == "Staff") {
    //        $("#StudentDiv").hide();
    //        $("#StaffDiv").show();
    //    }
    //    else if ($("#ddlUserType").val() == "Student") {
    //        $("#StudentDiv").show();
    //        $("#StaffDiv").hide();
    //    }
    //    else {
    //        $("#StaffDiv").hide();
    //        $("#StudentDiv").hide();
    //    }
    //});
    $("#ddlNewCampus").change(function () {
        FillGradeByCampus();
        FillBlockByCampus();
        FillStaffGroupByCampus();
    });
    $("#ddlNewGrade").change(function () {
        FillStudNameByCampusWithGrade();
    });
    $("#ddlNewStaffGroup").change(function () {
        FillDepatmentByCampusandGroup();
    });
    $("#ddlNewDepartment").change(function () {
        FillStaffNameByCampusWithGroupandDepartment();
    });
    $("#ddlNewBlock").change(function () {
        FillLocationByCampusWithBlock();
    });
    //$("#StaffSearch").click(function () {
    //    GetStaffPopup();
    //})
    //function GetStaffPopup() {
    //    var grade = ''; // $('#Grade').val();
    //    ModifiedLoadPopupDynamicaly("/StaffManagement/StaffDetailsPopup", $('#DivStaffMasterSearch'),
    //          function () {
    //              $('#StaffGrade').val(grade);
    //              LoadSetGridParam($('#StaffMasterList'), "/StaffManagement/StaffDetailsPopupJqgrid");
    //          }, function (rdata) { $('#txtStaffName').val(rdata.Name); $('#StaffPreRegNum').val(rdata.PreRegNum); }, 600, 345, "Staff Name")
    //}
    FillAssetName();
    $.getJSON("/Base/FillCampus",
          function (fillig) {
              var ddlcam = $("#ddlNewCampus");
              ddlcam.empty();
              ddlcam.append("<option value=' '> Select </option>");
              $.each(fillig, function (index, itemdata) {
                  ddlcam.append($('<option/>',
                      {
                          value: itemdata.Value,
                          text: itemdata.Text
                      }));
              });
          });
});
function FillAssetName() {
    $.getJSON("/Asset/FillITAssetName",
           function (fillig) {
               var ddlcam = $("#ddlNewAsset");
               ddlcam.empty();
               ddlcam.append("<option value=' '> ----Select---- </option>");
               $.each(fillig, function (index, itemdata) {
                   ddlcam.append($('<option/>',
                       {
                           value: itemdata.Value,
                           text: itemdata.Text
                       }));
               });
           });
}
function reloadGrid() {
    $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/Asset/ITAssetManagementjqGrid',
             postData: { AssetType: "", Make: "", Model: "", SerialNumber: "", Location: "", FormId: "" },
             page: 1
         }).trigger("reloadGrid");
}