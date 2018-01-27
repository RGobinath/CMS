var AssetUserType = $("#UserType").val();
function CalculatePendingAge() {
    var DCDate = $("#viewbagDCDate").val();
    if (DCDate == "") {
        $("#txtPendingAge").val('');
    }
    else {
        var Today = new Date();
        var oneDay = 24 * 60 * 60 * 1000;
        DCDate = DCDate.split('-');
        var firstDate = new Date(DCDate[2], DCDate[1], DCDate[0]);
        var secondDate = new Date(Today.getFullYear(), (Today.getMonth() + 1), Today.getDate());
        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        $("#txtPendingAge").val(diffDays);
    }
}
function GetVendorNameByVendorType() {
    var VendorType = "Service";
    var ddlbc = $("#ddlVendorName");
    $.getJSON("/Asset/GetVendorNameWithIdByVendorType?VendorType=" + VendorType,
      function (fillbc) {
          debugger;
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              if (itemdata.Text == $("#PreviousLocation").val()) {
                  ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                  GetInvoiceNoByVendorIdandDocumentType();
              }
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
        $.getJSON("/Asset/GetInvoiceNoByVendorIdandDocumentType?VendorId=" + VendorId + '&DocumentType=ServiceInvoice',
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
    if (AssetUserType == "Student") {
        Campus = $("#FromCampus").val();
    }
    else if ($("#ddlUserType").val() == "Not Applicable") {
        Campus = "Stock";
    }
    else {
        Campus = $("#ddlToCampus").val();
    }
    var ddlbc = $("#ddlToBlock");
    if (Campus != "" && Campus != null) {
        debugger;
        $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              if (AssetUserType == "Student") {
                  ddlbc.append("<option value='Stock'>Stock</option>");
              }
              else if ($("#ddlUserType").val() == "Not Applicable") {
                  ddlbc.append("<option value='Stock' selected='selected'>Stock</option>");
              }
              FillLocationByCampusWithBlock("");
              //ddlbc.append("<option value='Stock' selected='selected'>Stock</option>");
              $.each(fillbc, function (index, itemdata) {
                  //if (itemdata.Text == $("#FromBlock").val()) {
                  //    ddlbc.append("<option value='" + itemdata.Text+ "' selected='selected'>" + itemdata.Text + "</option>");
                  //    //GetIssueType();
                  //}

                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));

              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillLocationByCampusWithBlock(Block) {
    debugger;
    var Campus = "";
    if (AssetUserType == "Student") {
        Campus = $("#FromCampus").val();
    }
    else {
        Campus = $("#ddlToCampus").val();
    }
    if ($("#IsSubAsset").val() == "True" && $("#ddlUserType").val() != "Not Applicable" && AssetUserType != "Student") {
    }
    else {
        Block = $("#ddlToBlock").val();
    }
    var ddlbc = $("#ddlToLocation");
    if (Campus != "" && Campus != null && Block != "" && Block != null) {
        debugger;
        $.getJSON("/Asset/GetLocationByCampusWithBlock?Campus=" + Campus + '&Block=' + Block,
        function (fillbc) {
            debugger;
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select" }));
            //ddlbc.append($('<option/>', { value: "Stock", text: "Stock" }));
            if (Block == "Stock") {
                ddlbc.append("<option value='Stock' selected='selected'>Stock</option>");
            }
            $.each(fillbc, function (index, itemdata) {
                if (itemdata.Text == $("#AssetHistory_FromLocation").val()) {
                    ddlbc.append("<option value='" + itemdata.Text + "' selected='selected'>" + itemdata.Text + "</option>");
                }
                else {
                    ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                }
            });
            if ($("#ddlTransactionType").val() == "IntraCampus") {
                if ($("#txtFromBlock").val() == $("#ddlToBlock").val()) {
                    $("#ddlToLocation option[value='" + $("#txtFromLocation").val() + "']").hide();
                }
            }
        });

    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStaffGroupByCampus() {
    debugger;
    var Campus = $("#ddlToCampus").val();
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
    var Campus = $("#ddlToCampus").val();
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var ddlbc = $("#ddlNewDepartment");
    if (Campus != "" && Campus != null && StaffGroup != "" && StaffGroup != null) {
        $.getJSON("/Base/DepatmentByCampusandGroup?Campus=" + Campus + '&GroupName=' + StaffGroup,
          function (fillbc) {
              ddlbc.empty();

              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Text == $("#hiddenNewDepartment").val()) {
                      ddlbc.append("<option value='" + itemdata.Text + "' selected='selected'>" + itemdata.Text + "</option>");
                  }
                  else {
                      ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                  }
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStaffNameByCampusWithGroupandDepartment() {
    var Campus = $("#ddlToCampus").val();
    //if ($("#ddlTransactionType").val() == "IntraCampus") {
    //    Campus = $("#txtFromCampus").val();
    //}
    //if ($("#ddlTransactionType").val() == "InterCampus") {
    //    Campus = $("#ddlToCampus option:selected").text();
    //}
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var Department = $("#ddlNewDepartment").val();
    var ddlbc = $("#ddlNewStaffName");
    if (Campus != "" && Campus != null && StaffGroup != "" && StaffGroup != null) {
        $.getJSON("/Base/StaffNameByCampusWithGroupandDepartment?Campus=" + Campus + '&GroupName=' + StaffGroup + '&Department=' + Department,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Value == $("#hiddenIdNum").val()) {
                      ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                  }
                  else {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  }
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
jQuery(function ($) {
    //var spec;
    //var AssetId = $('#assetId').val();
    //$.ajax({
    //    type: 'POST',
    //    dataType: 'json',
    //    url: '/Asset/GetSpecificationlistByAsset?AssetId=' + AssetId,
    //    success: function (data) {
    //        spec = data;
    //    }
    //});   
    $("#divassetcode").hide();
    FillBlockByCampus();
    FillStaffGroupByCampus();
    CalculatePendingAge();
    GetVendorNameByVendorType();
    FillYear();
    FillMonth();
    //$("#ddlToCampus").val('');
    $("#ddlToCampus").prop("disabled", true);
    $("#ddlVendorName").prop("disabled", true);
    $("#ddlUserType").val('');
    $("#StaffDiv").hide();
    if (AssetUserType == "Staff" || AssetUserType == "Common") {
        $("#ddlUserType option[value=Student]").hide();
    }
    $("#txtAmount").keypress(function (e) {
        debugger;
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    var cache = {};
    $("#txtITAssetCode").autocomplete({
        source: function (request, response) {
            debugger;
            var term = request.term;
            if (term in cache) {
                debugger;
                response($.map(cache[term], function (item) {
                    debugger;
                    return { label: item.Text, value: item.Value }
                }))
                return;
            }
            //else {
            $.getJSON("/Asset/ITAssetCodeAutoComplete?Campus=" + $("#ddlToCampus option:selected").text() + '&UserType=' + $("#ddlUserType").val(), request, function (data, status, xhr) {
                cache[term] = data;
                //response(data);
                response($.map(data, function (item) {
                    return { label: item.Text, value: item.Value }
                }))
            });
            //}
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            debugger;
            event.preventDefault();
            $("#hdnAssetDet_Id").val(ui.item.value);
            $("#txtITAssetCode").val(ui.item.label);
            $("#ddlToBlock,#ddlToLocation,#ddlNewDepartment,#ddlNewStaffGroup,#ddlNewStaffName").attr("disabled", true);
            FillDetailsByAssetDet_Id(ui.item.value);
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtITAssetCode").val(ui.item.label);
        },
        messages: {
            noResults: "", results: ""
        }
    });
    $("#txtITAssetCode").focus(function () {
        if ($("#ddlUserType").val() == "") {
            ErrMsg("Please Select User Type");
            return false;
        }
        else {
            return true;
        }
    });
    $('#btnAddAsset').click(function () {
        debugger;
        var Warranty = 0;
        var AssetRefId = 0;
        var InWardDate = $("#txtInwardDate").val();
        var Amount = $("#txtAmount").val();
        var VendorName = $("#ddlVendorName").val();
        var InvoiceDetailsId = $("#ddlInvoiceNo").val();
        var PendingAge = $("#txtPendingAge").val();
        var Year = $("#ddlYear").val();
        var Month = $("#ddlMonth").val();
        if (Year != "" && parseInt(Year) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Year) * 12;
        }
        if (Month != "" && parseInt(Month) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Month);
        }
        if ($("#IsSubAsset").val() == "True") {
            AssetRefId = $("#hdnAssetDet_Id").val();
        }
        var IdNum = 0;
        var Campus = "", Location = "", Block = "", UserType = "", AssetDet_Id = 0, Department = "", StaffGroup = "";
        if (AssetUserType == "Student") {
            Campus = $("#FromCampus").val();
            if ($("#ddlToBlock").val() == "" || $("#ddlToLocation").val() == "") {
                ErrMsg("Please Fill Required Fields!!");
                return false;
            }
            else if ($("#ddlToBlock").val() != "Stock") {
                if ($("#ddlToLocation").val() != $("#AssetHistory_FromLocation").val()) {
                    ErrMsg("Previous Location Mismatched!!" + " Please Select " + $("#AssetHistory_FromLocation").val());
                    return false;
                }
            }
            Block = $("#ddlToBlock").val();
            Location = $("#ddlToLocation").val();
            IdNum = $("#IdNum").val();
            UserType = $("#UserType").val();
            AssetDet_Id = $("#AssetDet_Id").val();
        }
        else {
            Campus = $("#ddlToCampus").val();
            Location = $("#ddlToLocation").val();
            Block = $("#ddlToBlock").val();
            AssetDet_Id = $("#AssetDet_Id").val();
            Department = $("#ddlNewDepartment").val();
            StaffGroup = $("#ddlNewStaffGroup").val();
            UserType = $("#ddlUserType").val();
            if ($("#IsSubAsset").val() == "True" && $("#txtITAssetCode").val() == "") {
                $("#hdnAssetDet_Id").val('');
                ErrMsg("Please Enter Asset Code");
                return false;
            }
            if ($("#txtITAssetCode").val() != "" && AssetRefId < 0) {
                ErrMsg("Please Enter Valid Asset Code");
                return false;
            }
            if (UserType == "Staff") {
                var StaffName = $("#ddlNewStaffName").val();
                if (Department == "" || StaffGroup == "" || StaffName == "") {
                    ErrMsg("Please Fill Required Fields!!");
                    return false;
                }
                IdNum = $("#ddlNewStaffName").val();
            }
        }
        if (Campus == "" || Location == "" || Block == "" || InvoiceDetailsId == 0 || VendorName == "" || UserType == "" || Amount == "") {
            ErrMsg("Please Fill Required Fields");
            return false;
        }
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            async: false,
            url: '/Asset/EditAssetandServiceDetails',
            data: { CurrentLocation: Location, AssetDet_Id: AssetDet_Id, CurrentBlock: Block, CurrentCampus: Campus, InwardDate: InWardDate, IdNum: IdNum, UserType: UserType, PendingAge: PendingAge, InvoiceDetailsId: InvoiceDetailsId, Warranty: Warranty, Amount: Amount, AssetRefId: AssetRefId, IsSubAsset: $("#IsSubAsset").val() },
            success: function (data) {
                if (data == true) {
                    SucessMsg('Updated sucessfully!!');
                    window.location.href = "/Asset/ITAssetService";
                    return true;

                }
                if (data == false) {
                    ErrMsg("Not Updated Successfully.");
                    reloadGrid();
                    return true;
                }
            }
        });
    });
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = dd + '/' + mm + '/' + yyyy;
    document.getElementById("txtInwardDate").value = today;
    $('#btnExit').click(function () {
        $('#MoveToBack').dialog('close');
        reloadGrid();
    });
    $("#ddlUserType").change(function () {
        debugger;
        if ($("#ddlUserType").val() == "Staff") {
            $("#StaffDiv").show();
            $("#txtITAssetCode,#hdnAssetDet_Id").val('');
            cache = {};
            $("#divassetcode").show();
            FillBlockByCampus();
        }
        else if ($("#ddlUserType").val() == "Not Applicable") {
            $("#StaffDiv").hide();
            cache = {};
            $("#txtITAssetCode,#hdnAssetDet_Id").val('');
            $("#divassetcode").hide();
            FillBlockByCampus();
        }
        else if ($("#ddlUserType").val() == "Common") {
            $("#ddlNewDepartment").val('');
            $("#ddlNewStaffGroup").val('');
            $("#ddlNewStaffName").val('');
            cache = {};
            $("#txtITAssetCode,#hdnAssetDet_Id").val('');
            $("#StaffDiv").hide();
            $("#divassetcode").show();
            FillBlockByCampus();
        }
        else {
            $("#ddlNewDepartment").val('');
            $("#ddlNewStaffGroup").val('');
            $("#ddlNewStaffName").val('');
            cache = {};
            $("#txtITAssetCode,#hdnAssetDet_Id").val('');
            $("#StaffDiv").hide();
            $("#divassetcode").hide();
            FillBlockByCampus();
        }
    });
    //$("#ddlVendorName").change(function () {
    //    GetInvoiceNoByVendorIdandDocumentType();
    //});
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
    //$("#ddlToCampus").change(function () {
    //    $("#ddlToBlock").val('');
    //    $("#ddlToLocation").val('');
    //    $("#ddlNewStaffGroup").val('');
    //    FillBlockByCampus();
    //    FillLocationByCampusWithBlock();
    //    FillStaffGroupByCampus();
    //});
    //$("#ddlNewGrade").change(function () {
    //    FillStudNameByCampusWithGrade();
    //});
    $("#ddlNewStaffGroup").change(function () {
        FillDepatmentByCampusandGroup();
    });
    $("#ddlNewDepartment").change(function () {
        FillStaffNameByCampusWithGroupandDepartment();
    });
    $("#ddlToBlock").change(function () {
        FillLocationByCampusWithBlock("");
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
             url: '/Asset/ITAssetServicejqGrid',
             postData: { AssetType: "", Make: "", Model: "", SerialNumber: "", Location: "", FormId: "" },
             page: 1
         }).trigger("reloadGrid");
}
function FillDetailsByAssetDet_Id(AssetDet_Id) {
    $.ajax({
        type: 'POST',
        url: "/Asset/GetAssetDetails",
        data: { AssetDet_Id: AssetDet_Id },
        success: function (data) {
            $("#ddlToBlock").val(data.Block);
            $("#AssetHistory_FromLocation").val(data.Location);
            FillLocationByCampusWithBlock(data.Block);
            $("#ddlNewStaffGroup").val(data.StaffGroup);
            $("#hiddenNewDepartment").val(data.Department);
            FillDepatmentByCampusandGroup();
            FillStaffNameByCampusWithGroupandDepartment();
            $("#hiddenIdNum").val(data.IdNum);
        }
    });
}