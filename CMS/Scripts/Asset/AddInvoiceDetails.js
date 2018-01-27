function GetVendorNameByVendorType() {
    var ddlbc = $("#ddlVendorName1");
    if ($("#ddlDocumentType1").val() == "") {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
    else {
        var VendorType = "";
        if ($("#ddlDocumentType1").val() == "PurchaseInvoice") {
            VendorType = "Purchase";
        }
        else {
            VendorType = "Service";
        }
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
}
function FillYear() {
    var ddlyear = $("#ddlYear1");
    ddlyear.empty();
    ddlyear.append($('<option/>', { value: "", text: "Year" }));
    var len = 10;
    for (var i = 0; i <= len; i++) {
        ddlyear.append($('<option/>', { value: i, text: i }));
    }
}
function FillMonth() {
    var ddlmonth = $("#ddlMonth1");
    ddlmonth.empty();
    ddlmonth.append($('<option/>', { value: "", text: "Month" }));
    var len = 11;
    for (var i = 0; i <= len; i++) {
        ddlmonth.append($('<option/>', { value: i, text: i }));
    }
}
function GetInvoiceNoByVendorIdandDocumentType() {
    var VendorId = $("#ddlVendorName1").val();
    var ddlbc = $("#ddlInvoiceNo1");
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
jQuery(function ($) {
    $("#ddlDocumentType1").val("PurchaseInvoice");
    GetVendorNameByVendorType();
    FillYear();
    FillMonth();
    $("#txtAmount1").keypress(function (e) {
        debugger;
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    $("#ddlDocumentType1").change(function () {
        GetVendorNameByVendorType();
    });
    $("#ddlVendorName1").change(function () {
        GetInvoiceNoByVendorIdandDocumentType();
    });
    $('#btnAddInvoice').click(function () {
        debugger;
        var Warranty = 0;
        var InvoiceDetailsId = 0
        var VendorName = $("#ddlVendorName1").val();
        InvoiceDetailsId = $("#ddlInvoiceNo1").val();
        var Year = $("#ddlYear1").val();
        var Month = $("#ddlMonth1").val();
        var Amount = $("#txtAmount1").val();
        if (Year != "" && parseInt(Year) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Year) * 12;
        }
        if (Month != "" && parseInt(Month) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Month);
        }        
        if (Warranty <= 0 || InvoiceDetailsId <= 0 || VendorName == "" || Amount.trim() == "" || Amount <= 0) {
            ErrMsg('All fields are required and it cannot be empty!!');
            return false;
        }        
        else {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                async: false,
                url: '/Asset/AddNewInvoiceDetails?Warranty=' + Warranty,
                data: { InvoiceDetailsId: InvoiceDetailsId, Amount: Amount, AssetDet_Id: $("#viewbagAssetDet_Id").val() },
                success: function (data) {
                    if (data == true) {
                        SucessMsg('Invoice added successfully!!');
                        $("#ddlDocumentType1").val("");
                        $("#ddlVendorName1").val("");
                        $("#ddlInvoiceNo1").val("");
                        $("#ddlYear1").val("");
                        $("#ddlMonth1").val("");
                        $("#txtAmount1").val("");                                              
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
    $('#btnExit1').click(function () {
        $('#newAssetInvoiceDetails').dialog('close');
        reloadGrid();
    });
});
function reloadGrid() {
    $('#Uploadedfileslist').setGridParam(
         {
             datatype: "json",
             url: '/Asset/Documentsjqgrid',
             postData: { id: $("#viewbagAssetDet_Id").val() },
             page: 1
         }).trigger("reloadGrid");
}