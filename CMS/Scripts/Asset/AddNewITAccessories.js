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
function FillModelByBrand() {
    var BrandId = $("#ddlBrand").val();
    var ddlbc = $("#ddlModel");
    if (BrandId != "" && BrandId != null) {
        $.getJSON("/Asset/GetITAccessoriesModelByBrand?BrandId=" + BrandId,
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
function FillProductTypeByName() {
    var ProductNameId = $("#ddlProductName").val();
    var ddlbc = $("#ddlProductType");
    if (ProductNameId != "" && ProductNameId != null) {
        $.getJSON("/Asset/GetAssetProductTypeByAssetProductName?ProductNameId=" + ProductNameId,
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
    GetVendorNameByVendorType();
    FillYear();
    FillMonth();
    $("#txtAmount").keypress(function (e) {
        debugger;
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    $('#btnAddAccessories').click(function () {
        debugger;
        var Warranty = 0;
        var InvoiceDetailsId = 0
        var Campus = $('#ddlCampus').val();
        var VendorName = $("#ddlVendorName").val();
        InvoiceDetailsId = $("#ddlInvoiceNo").val();
        var ProductType = $('#ddlProductType').val();
        var ProductName = $('#ddlProductName').val();
        var Model = $('#ddlModel').val();
        var Brand = $('#ddlBrand').val();
        var Year = $("#ddlYear").val();
        var Month = $("#ddlMonth").val();
        var Amount = $("#txtAmount").val();
        var Quantity = $("#txtQuantity").val();
        if (Year != "" && parseInt(Year) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Year) * 12;
        }
        if (Month != "" && parseInt(Month) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Month);
        }        
        if (Campus == "" || ProductName == "" || ProductType == "" || Brand == "" || Model == "" || Quantity == "" || Amount == "" || InvoiceDetailsId <= 0 || Warranty <= 0) {
            ErrMsg('All fields are required and it cannot be empty!!');
            return false;
        }
        else {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                async: false,
                url: '/Asset/AddAccessoriesDetails',
                data: { CampusId: Campus, ProductNameId: ProductName, ProductTypeId: ProductType, BrandId: Brand, ModelId: Model, Quantity: Quantity, InvoiceDetailsId: InvoiceDetailsId, Warranty: Warranty, Amount: Amount },
                success: function (data) {
                    if (data == "success") {
                        SucessMsg('Accessories added successfully!!');
                        $('#ddlCampus').val("");
                        $('#ddlBrand').val("");
                        $('#ddlProductName').val("");
                        $('#ddlProductType').val("");
                        $('#ddlModel').val("");
                        $("#ddlVendorName").val("");
                        $("#ddlInvoiceNo").val("");
                        $("#ddlYear").val("");
                        $("#ddlMonth").val("");
                        $("#txtAmount").val("");
                        $("#txtQuantity").val("");
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
        $('#newITAccessories').dialog('close');
        reloadGrid();
    });
    $("#ddlBrand").change(function () {
        FillModelByBrand();
    });
    $("#ddlProductName").change(function () {
        FillProductTypeByName();
    });
    $("#ddlVendorName").change(function () {
        GetInvoiceNoByVendorIdandDocumentType();
    });
    FillProductName();
    $.getJSON("/Base/FillCampus",
          function (fillig) {
              var ddlcam = $("#ddlCampus");
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
    $.getJSON("/Asset/GetITAccessoriesBrandName",
          function (fillig) {
              var ddlcam = $("#ddlBrand");
              ddlcam.empty();
              ddlcam.append("<option value=''>Select</option>");
              $.each(fillig, function (index, itemdata) {
                  ddlcam.append($('<option/>',
                      {
                          value: itemdata.Value,
                          text: itemdata.Text
                      }));
              });
          });
});
function FillProductName() {
    $.getJSON("/Asset/GetAssetProductName",
           function (fillig) {
               var ddlcam = $("#ddlProductName");
               ddlcam.empty();
               ddlcam.append("<option value=''>Select</option>");
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
             url: '/Asset/ITAccessoriesJqgrid',
             postData: { AssetType: "", Make: "", Model: "", SerialNumber: "", Location: "", FormId: "" },
             page: 1
         }).trigger("reloadGrid");
}