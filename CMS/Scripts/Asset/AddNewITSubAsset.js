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
function FillModelByBrand(Brand) {
    var ddlbc = $("#assetModel");
    if (Brand != "" && Brand != null) {
        $.getJSON("/Asset/GetModelByBrand?Brand=" + Brand,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Text == $("#hiddenmodel").val()) {
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
function FillBlockByCampus() {
    var Campus = "";
    Campus = $("#ddlNewCampus option:selected").text();
    var ddlbc = $("#ddlNewBlock");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Text == $("#hdnblock").val()) {
                      ddlbc.append("<option value='" + itemdata.Text + "' selected='selected'>" + itemdata.Text + "</option>");
                      FillLocationByCampusWithBlock();
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
                if (itemdata.Text == $("#hdnlocation").val()) {
                    ddlbc.append("<option value='" + itemdata.Text + "' selected='selected'>" + itemdata.Text + "</option>");
                    FillLocationByCampusWithBlock();
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
jQuery(function ($) {
    FillYear();
    FillMonth();
    var spec;
    var AssetId = $('#ddlAssetType').val();    
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Asset/GetSpecificationlistByAsset?AssetId=' + AssetId,
        success: function (data) {
            spec = data;
        }
    });
    $("#ddlNewCampus,#ddlSubUserType,#ddlNewBlock,#ddlNewLocation,,#txtUserName").attr("disabled", true);
    $("#divassetcode").hide();
    $("#divusername").hide();
    $("#divassetspecs").hide()
    $("#assetMake").change(function () {
        FillModelByBrand();
    });
    $("#ddlSubAssetType").change(function () {
        if ($("#ddlSubAssetType").val() == "External") {
            $("#ddlNewCampus option:contains(" + $("#hdncampus").val() + ")").attr('selected', true);
            FillBlockByCampus();
            $("#ddlSubUserType").val($("#hdnusertype").val());
            if ($("#hdnusertype").val() == "Student" || $("#hdnusertype").val() == "Staff")
            {
                $("#divusername").show();
                $("#txtUserName").val($("#hdnusername").val());
            }
            $("#txtUserName").val();
            $("#ddlNewBlock").val($("#hdnblock").val());
            $("#ddlNewLocation").val($("#hdnlocation").val());
            $("#assetMake,#assetModel,#assetSerial,#ddlYear,#ddlMonth").attr("disabled", true);
            $("#divassetcode").show();
            $("#divassetspecs").hide()
        }
        else if ($("#ddlSubAssetType").val() == "Internal") {
            $("#ddlNewCampus option:contains(" + $("#hdncampus").val() + ")").attr('selected', true);
            $("#ddlSubUserType").val($("#hdnusertype").val());
            if ($("#hdnusertype").val() == "Student" || $("#hdnusertype").val() == "Staff") {
                $("#divusername").show();
                $("#txtUserName").val($("#hdnusername").val());
            }
            $("#ddlNewBlock").val($("#hdnblock").val());
            $("#ddlNewLocation").val($("#hdnlocation").val());
            $("#assetMake,#assetModel,#assetSerial,#ddlYear,#ddlMonth").attr("disabled", false);
            $("#assetMake,#assetModel,#assetSerial,#ddlYear,#ddlMonth,#txtAssetCode").val('');
            $("#divassetcode").hide();
            $("#divassetspecs").show()
            FillBlockByCampus();
            FillModelByBrand("");
        }
        else {
            $("#assetMake,#assetModel,#assetSerial,#ddlYear,#ddlMonth").attr("disabled", false);
            $("#assetMake,#assetModel,#assetSerial,#ddlYear,#ddlMonth,#txtAssetCode,#ddlNewCampus,#ddlSubUserType,#ddlNewBlock,#ddlNewLocation").val('');
            $("#divassetcode,#divusername").hide();
            $("#divassetspecs").hide()
            FillBlockByCampus();
            FillModelByBrand("");
        }
    });
    $("#assetMake").change(function () {
        FillModelByBrand($("#assetMake").val());
    });
    $("#btnAddSubAsset").click(function () {
        var vals = [];
        var Warranty = 0;
        var InvoiceDetailsId = 0
        var AssetId = $('#ddlAssetType').val();
        var Campus = $('#ddlNewCampus').val();
        //var VendorName = $("#ddlVendorName").val();
        //InvoiceDetailsId = $("#ddlInvoiceNo").val();
        //var AssetCode = $('#assetCode').val();        
        var Location = $('#ddlNewLocation').val();
        var AssetType = $('#assetNameId').val();
        var SerialNumber = $('#assetSerial').val();
        var Model = $('#assetModel').val();
        var Make = $('#assetMake').val();
        var Block = $('#ddlNewBlock').val();
        var UserType = $('#ddlSubUserType').val();
        var IdNum = $('#IdNum').val();
        var Year = $("#ddlYear").val();
        var Month = $("#ddlMonth").val();
        var Amount = 0;
        if ($("#ddlSubAssetType").val() == "Internal") {
            for (var i = 0; i < spec.length; i++) {
                vals.push($(document.getElementById(spec[i])).val());
                if (i > 0) {
                    if (vals[i] == "" || vals[i] == "undefined" || vals[i] == null) {
                        ErrMsg("All fields are required and it cannot be empty!!");
                        return false;
                    }
                }
            }
        }
        if (Year != "" && parseInt(Year) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Year) * 12;
        }
        if (Month != "" && parseInt(Month) > 0) {
            Warranty = parseInt(Warranty) + parseInt(Month);
        }
        if ($("#ddlSubAssetType").val() == "" || $("#assetMake").val() == "" || $("#assetModel").val() == "" || $("#assetSerial").val() == "" || $("#ddlYear").val() == "" || $("#ddlMonth").val() == "") {
            ErrMsg("All fields are required and it cannot be empty!!");
            return false;
        }        
        else if ($("#ddlSubAssetType").val() == "External" && $("#txtAssetCode").val() == "") {
            ErrMsg("Please Enter Asset Code");
            return false;
        }
        else if ($("#ddlSubAssetType").val() == "External" && $("#txtAssetCode").val() != "" && ($("#hdnAssetDet_Id").val() == "" || $("#hdnAssetDet_Id").val() <= 0)) {
            ErrMsg("Please Enter Valid Asset Code");
            return false;
        }
        else {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                async: false,
                url: '/Asset/AddSubAssetDetails?SpecDetails=' + vals + '&FormId=' + Campus,
                data: { Location: Location, Asset_Id: AssetId, AssetType: AssetType, SerialNo: SerialNumber, Model: Model, Make: Make, IdNum: IdNum, UserType: UserType, FromBlock: Block, InvoiceDetailsId: InvoiceDetailsId, Warranty: Warranty, Amount: Amount, AssetRefId: $("#assetId").val(), SubAssetType: $("#ddlSubAssetType").val(), AssetDet_Id: $("#hdnAssetDet_Id").val() },
                success: function (data) {
                    if (data == "success") {
                        SucessMsg('Sub Asset added successfully!!');                        
                        for (var i = 0; i < spec.length; i++) {
                            vals.push($('#' + spec[i]).val(""));
                        }
                        $("#ddlSubAssetType,#assetMake,#assetModel,#assetSerial,#ddlYear,#ddlMonth").val('');
                        jQuery("#grid-table").trigger("reloadGrid");
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
    var cache = {};
    $("#txtAssetCode").autocomplete({
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
            $.getJSON("/Asset/AssetCodeAutoComplete?Campus=" + $("#ddlNewCampus option:selected").text() + '&AssetType=', request, function (data, status, xhr) {
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
            $("#txtAssetCode").val(ui.item.label);
            FillDetailsByAssetDet_Id(ui.item.value);
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtAssetCode").val(ui.item.label);
        },
        messages: {
            noResults: "", results: ""
        }
    });

});
function FillDetailsByAssetDet_Id(AssetDet_Id) {
    $.ajax({
        type: 'POST',
        url: "/Asset/GetSubAssetDetails",
        data: { AssetDet_Id: AssetDet_Id },
        success: function (data) {
            $("#assetMake").val(data.Make);
            FillModelByBrand(data.Make);
            $("#hiddenmodel").val(data.Model);
            $("#ddlYear").val(data.Year);
            $("#ddlMonth").val(data.Month);
            $("#assetSerial").val(data.SerialNo);
        }
    });
}