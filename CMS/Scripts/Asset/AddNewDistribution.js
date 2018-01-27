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
       
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });    
    $('#btnAddAsset').click(function () {

        var vals = [];
        var Warranty = 0;
        var InvoiceDetailsId = 0
        var AssetId = $('#assetId').val();
        var Campus = $('#ddlNewCampus').val();
        var VendorName = $("#ddlVendorName").val();
        InvoiceDetailsId = $("#ddlInvoiceNo").val();
        var Location = $('#ddlNewLocation').val();
        var AssetType = $('#assetNameId').val();
        var SerialNumber = $('#assetSerial').val();
        var Model = $('#assetModel').val();
        var Make = $('#assetMake').val();
        var Block = $('#ddlNewBlock').val();
        var Year = $("#ddlYear").val();
        var Month = $("#ddlMonth").val();
        var Amount = $("#txtAmount").val();
        var OperatingSystemDtls = $("#ddlOperatingSystem").val();
        var SSize = $("#ddlSS").val();

        if (Year != "" && parseInt(Year) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Year) * 12;
        }
        if (Month != "" && parseInt(Month) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Month);
        }

        for (var i = 0; i < spec.length; i++) {
            vals.push($(document.getElementById(spec[i])).val());
            //---Adding Screen Size
            if (i == 2)
                ScreenSize = $(document.getElementById(spec[2])).val();

            if (i > 0) {
                if (vals[i] == "" || vals[i] == "undefined" || vals[i] == null) {
                    ErrMsg("All fields are required and it cannot be empty!!");
                    return false;
                }
            }
        }
        if (AssetId == 'undefined' || AssetId == 'undefined' || Campus == " " || Location == "" || Model == "" || Make == "" || Block == "" || spec.length != vals.length || OperatingSystemDtls == "" || ScreenSize == "") {
            ErrMsg('All fields are required and it cannot be empty!!');
            return false;
        }
        else if (SerialNumber == null || SerialNumber.trim() === '') {
            ErrMsg('All fields are required and it cannot be empty!!');
            return false;
        }
        else {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                async: false,
                url: '/Asset/AddSTAssetDetails?SpecDetails=' + vals + '&FormId=' + Campus,
                data: { Location: "Stock", Asset_Id: AssetId, AssetType: AssetType, SerialNo: SerialNumber, Model: Model, Make: Make, IdNum: IdNum, FromBlock: "Stock", InvoiceDetailsId: InvoiceDetailsId, Warranty: Warranty, Amount: Amount, OperatingSystemDtls: OperatingSystemDtls, LTSize: SSize },
                success: function (data) {
                    if (data == true) {
                        SucessMsg('Asset added successfully!!');
                        $('#ddlNewCampus').val("");
                        $('#assetMake').val("");
                        $('#assetModel').val("");
                        $('#assetSerial').val("");
                        $('#ddlNewLocation').val("");
                        $("#ddlNewBlock").val("");
                        $("#ddlVendorName").val("");
                        $("#ddlInvoiceNo").val("");
                        $("#ddlYear").val("");
                        $("#ddlMonth").val("");
                        $("#txtAmount").val("");
                        $("#ddlOperatingSystem").val("");
                        $("#ddlSS").val("");

                        for (var i = 0; i < spec.length; i++) {
                            vals.push($('#' + spec[i]).val(""));
                        }
                        //if ($("#IsSubAsset").val() === "True") {
                        //    reloadGrid1();
                        //}
                        //else {
                        reloadGrid1();
                        //}
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
        $('#newDistribution').dialog('close');
        if ($("#IsSubAsset").val() == "True") {
            reloadGrid1();
        }
        else {
            reloadGrid();
        }
    });               
    $("#assetMake").change(function () {
        FillModelByBrand();
    });

    $("#ddlVendorName").change(function () {

        GetInvoiceNoByVendorIdandDocumentType();
    });
 
    FillAssetName();
    $.getJSON("/Base/FillCampus",
          function (fillig) {
              var ddlcam = $("#ddlNewCampus");
              ddlcam.empty();
              ddlcam.append("<option value=' '> Select </option>");
              $.each(fillig, function (index, itemdata) {
                  debugger;
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
             url: '/Asset/LoadGridLaptopDtlsInvoiceNoWiseJqGrid',
             postData: { lInvoiceNo: $('#ddlInvoiceNo1').val() },
             page: 1
         }).trigger("reloadGrid");
}

