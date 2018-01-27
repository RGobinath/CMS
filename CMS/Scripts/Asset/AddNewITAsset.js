function GetVendorNameByVendorType() {
    var VendorType = "Purchase";
    var ddlbc = $("#ddlVendorName");
    $.getJSON("/Asset/GetVendorNameWithIdByVendorType?VendorType=" + VendorType,
      function (fillbc) {
          debugger;
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillYear() {
    var ddlyear = $("#ddlYear");
    ddlyear.empty();
    ddlyear.append($('<option/>', { value: "", text: "Year" }));
    var len = 10;
    for (var i = 0; i <= len; i++) {
        ddlyear.append($('<option/>', { value: i, text: i }));
    }
}
function FillMonth() {
    var ddlmonth = $("#ddlMonth");
    ddlmonth.empty();
    ddlmonth.append($('<option/>', { value: "", text: "Month" }));
    var len = 11;
    for (var i = 0; i <= len; i++) {
        ddlmonth.append($('<option/>', { value: i, text: i }));
    }
}
function GetInvoiceNoByVendorIdandDocumentType() {
    var VendorId = $("#ddlVendorName").val();
    var ddlbc = $("#ddlInvoiceNo");
    if (VendorId > 0) {
        $.getJSON("/Asset/GetInvoiceNoByVendorIdandDocumentType?VendorId=" + VendorId + '&DocumentType=PurchaseInvoice',
          function (fillbc) {
              debugger;
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
function FillBlockByCampus() {
    var Campus = "";
    if ($('#ddlUserType').val() == "Not Applicable") {
        Campus = "Stock"
    }
    else {
        Campus = $("#ddlNewCampus option:selected").text();
    }
    var ddlbc = $("#ddlNewBlock");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              if ($('#ddlUserType').val() == "Not Applicable") {
                  //ddlbc.append($('<option/>', { value: "Stock", text: "Stock" }));
                  ddlbc.append("<option value='Stock' selected='selected'>Stock</option>");
                  FillLocationByCampusWithBlock();
              }
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
            if ($('#ddlUserType').val() == "Not Applicable") {
                //ddlbc.append($('<option/>', { value: "Stock", text: "Stock" }));
                ddlbc.append("<option value='Stock' selected='selected'>Stock</option>");
            }
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
function FillModelByBrand() {
    var Brand = $("#assetMake").val();
    var ddlbc = $("#assetModel");
    if (Brand != "" && Brand != null) {
        $.getJSON("/Asset/GetModelByBrand?Brand=" + Brand,
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
jQuery(function ($) {
    $("#StaffDiv").hide();
    $("#StudentDiv").hide();
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
    GetVendorNameByVendorType();
    FillYear();
    FillMonth();
    $("#txtAmount").keypress(function (e) {
        debugger;
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });    
    if ($("#IsSubAsset").val() == "True")
    {
        $("#ddlUserType option[value=Staff]").hide();
        $("#ddlUserType option[value=Student]").hide();
        $("#ddlUserType option[value=Common]").hide();
        $("#ddlUserType").val('Not Applicable');
        $("#ddlUserType").attr("disabled", true);
        FillBlockByCampus();
    }
    $('#btnAddAsset').click(function () {
        debugger;
        var vals = [];
        var Warranty = 0;
        var InvoiceDetailsId = 0
        var AssetId = $('#assetId').val();
        var Campus = $('#ddlNewCampus').val();
        var VendorName = $("#ddlVendorName").val();
        InvoiceDetailsId = $("#ddlInvoiceNo").val();
        //var AssetCode = $('#assetCode').val();        
        var Location = $('#ddlNewLocation').val();
        var AssetType = $('#assetNameId').val();
        var SerialNumber = $('#assetSerial').val();
        var Model = $('#assetModel').val();
        var Make = $('#assetMake').val();
        var Block = $('#ddlNewBlock').val();
        var UserType = $('#ddlUserType').val();
        var IdNum = $('#IdNum').val();
        var Year = $("#ddlYear").val();
        var Month = $("#ddlMonth").val();
        var Amount = $("#txtAmount").val();
        if (Year != "" && parseInt(Year) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Year) * 12;
        }
        if (Month != "" && parseInt(Month) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Month);
        }
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
            //|| VendorName == "" || InvoiceDetailsId == "" || Amount == ""
            ErrMsg('All fields are required and it cannot be empty!!');
            return false;
        }
        else if (SerialNumber == null || SerialNumber.trim() === ''  && $('#assetNameId').val() != "MOUSE")
        {
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
                data: { Location: Location, Asset_Id: AssetId, AssetType: AssetType, SerialNo: SerialNumber, Model: Model, Make: Make, IdNum: IdNum, UserType: UserType, FromBlock: Block, InvoiceDetailsId: InvoiceDetailsId, Warranty: Warranty, Amount: Amount },
                success: function (data) {
                    if (data == true) {
                        SucessMsg('Asset added successfully!!');
                        $('#ddlNewCampus').val("");
                        $('#assetMake').val("");
                        $('#assetModel').val("");
                        $('#assetSerial').val("");
                        $('#ddlNewLocation').val("");
                        $("#ddlNewStudentName").val("");
                        $("#ddlNewGrade").val("");
                        $("#ddlNewStaffGroup").val("");
                        $("#ddlNewDepartment").val("");
                        $("#ddlNewStaffName").val("");
                        $("#ddlNewBlock").val("");
                        $("#ddlUserType").val("");
                        $("#ddlVendorName").val("");
                        $("#ddlInvoiceNo").val("");
                        $("#ddlYear").val("");
                        $("#ddlMonth").val("");
                        $("#txtAmount").val("");
                        for (var i = 0; i < spec.length; i++) {
                            vals.push($('#' + spec[i]).val(""));
                        }
                        if ($("#IsSubAsset").val() === "True") {
                            reloadGrid1();
                        }
                        else {
                            reloadGrid();
                        }
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
        if ($("#IsSubAsset").val() == "True")
        {
            reloadGrid1();
        }
        else
        {
            reloadGrid();
        }
    });
    $("#ddlUserType").change(function () {
        if ($("#ddlUserType").val() == "Staff") {
            $("#StudentDiv").hide();
            $("#StaffDiv").show();
            FillBlockByCampus();
        }
        else if ($("#ddlUserType").val() == "Student") {
            $("#StudentDiv").show();
            $("#StaffDiv").hide();
            FillBlockByCampus();
        }
        else if ($("#ddlUserType").val() == "Stock") {
            $("#StudentDiv").show();
            $("#StaffDiv").hide();
            FillBlockByCampus();
        }
        else {
            $("#StaffDiv").hide();
            $("#StudentDiv").hide();
            FillBlockByCampus();
        }
    });    
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
    $("#assetMake").change(function () {
        FillModelByBrand();
    });
    $("#ddlVendorName").change(function () {
        GetInvoiceNoByVendorIdandDocumentType();
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
    $.getJSON("/Asset/GetBrandName",
          function (fillig) {
              var ddlcam = $("#assetMake");
              ddlcam.empty();
              ddlcam.append("<option value=''> Select </option>");
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
function reloadGrid1() {
    $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/Asset/ITSubAssetManagementjqGrid',
             postData: { AssetType: "", Make: "", Model: "", SerialNumber: "", Location: "", FormId: "" },
             page: 1
         }).trigger("reloadGrid");
}