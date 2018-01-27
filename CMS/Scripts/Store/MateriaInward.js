var grid_selector = "#MaterialSkuList";
var pager_selector = "#MaterialSkuListPager";

$(function () {
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    $('#InvoiceDate').datepicker({ format: "dd/mm/yyyy",
        numberOfMonths: 1,
        autoclose: true
    }).datepicker('setDate', $("#InvoiceDate").val());

    $('#DCDate').datepicker({ format: "dd/mm/yyyy",
        numberOfMonths: 1,
        autoclose: true
    }).datepicker('setDate', $("#DCDate").val());

    $('#ReceivedDate').datepicker({ format: "dd/mm/yyyy",
        numberOfMonths: 1,
        autoclose: true
    }).datepicker('setDate', $("#ReceivedDate").val());
    //    $('#InvoiceDate').attr('readonly', true);
    //    $('#DCDate').attr('readonly', true);
    //    $('#ReceivedDate').attr('readonly', true);

    $("#DriverContactNo").keydown(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
            event.keyCode == 13 || (event.keyCode == 65 && event.ctrlKey === true) || (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
        }
        else {
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });
    var cam = $("#Campus").val();
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Store/FillStore?Campus=' + cam,
        success: function (data) {
            $("#ddlStore").empty();
            for (var i = 0; i < data.rows.length; i++) {
                if (data.rows[i].Text == $('#Store').val()) {
                    $("#ddlStore").append("<option value='" + data.rows[i].Value + "' selected='selected'>" + data.rows[i].Text + "</option>");
                }
                else {
                    $("#ddlStore").append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                }
            }
        }
    });
    var Id = $("#Id").val();
    $("#txtReceivedBy").val($("#ProcessedBy").val()).attr("readonly", true).css("background-color", "#F5F5F5");
    $("#ddlRcvdBy").change(function () {
        if ($(this).val() == "Self") {
            $("#txtReceivedBy").val($("#ProcessedBy").val()).attr("readonly", true).css("background-color", "#F5F5F5");
        }
        else {
            $("#txtReceivedBy").val('').attr("readonly", false).css("background-color", "#FFFFFF");
        }
    });
    $("#txtSupplierName").attr("readonly", true).css("background-color", "#F5F5F5");
    //    $("#SupplierSearch").button({ icons: { primary: "fa-icon-search"} });
    //    $("#MaterialSearch").button({ icons: { primary: "fa-icon-search"} });
    $("#btnComplete").click(function () {
        var Id = $("#Id").val();
        if (Id == 0) {
            ErrMsg("Please Save");
            return false;
        }
        if (skuValidation() == false) {
            return false;
        }
        else {
            if (confirm("Are you sure to Complete?")) {
                var objMatInward1 = {
                    Id: $("#Id").val()
                };
                $.ajax({
                    url: '/Store/CompleteMaterialInward',
                    type: 'POST',
                    dataType: 'json',
                    data: objMatInward1,
                    traditional: true,
                    success: function (data) {
                        if (data != "") {
                            InfoMsg(data + " is completed successfully", function () { window.location.href = '/Store/MaterialInwardList'; });
                        }
                    },
                    error: function (xhr, status, error) {
                        ErrMsg($.parseJSON(xhr.responseText).Message);
                    }
                });
            }
        }
    });
    $.getJSON("/Store/FillStoreUnits",
             function (fillig) {
                 var ddlUnits = $("#ddlUnits");
                 ddlUnits.empty();
                 ddlUnits.append($('<option/>', { value: "", text: "Select One" }));

                 $.each(fillig, function (index, itemdata) {
                     ddlUnits.append($('<option/>', { value: itemdata.Value, text: itemdata.Text
                     }));
                 });
             });

    $("#SupplierSearch").click(function () {
        ModifiedLoadPopupDynamicaly("/Store/StoreSupplier", $('#DivSupplierSearch'), function () { LoadSetGridParam($('#StoreSupplierList'), "/Store/StoreSupplierListJqGrid") },
        function () { }, 700, 400, "Supplier Details");
    });

    var lastsel2;
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialSkuListJqGrid?Id=' + Id + '&Store=' + $("#ddlStore").val(),
        datatype: 'json',
        mtype: 'POST',
        colNames: ['SKU Id', 'Material Ref Id', 'Materials', 'Material Group', 'Material Sub Group', 'Ord.Units', 'Rcvd.Units', 'Old Prices', 'Ord.Qty', 'Rcvd.Qty', 'Dmg.Qty', 'Unit Price', 'Total Price', 'Dmg.Desc / Remarks', ''],
        colModel: [
              { name: 'SkuId', index: 'SkuId', hidden: true, key: true, editable: true },
              { name: 'MaterialRefId', index: 'MaterialRefId', hidden: true, editable: true },
              { name: 'Material', index: 'Material', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
              { name: 'OrderedUnits', index: 'OrderedUnits', width: 100, align: 'left', sortable: false, editable: true, editoptions: { readonly: true, class: 'NoBorder'} },
              { name: 'ReceivedUnits', index: 'ReceivedUnits', width: 100, align: 'left', sortable: false, editable: true, editoptions: { readonly: true, class: 'NoBorder'} },
              { name: 'OldPrices', index: 'OldPrices', width: 100 },
              { name: 'OrderQty', index: 'OrderQty', width: 90, editable: true, editrules: { integer: true} },
              { name: 'ReceivedQty', index: 'ReceivedQty', width: 90, editable: true, editrules: { required: true, integer: true },
                  editoptions: {
                      dataInit: function (element) {
                          $(element).keyup(function () {
                              // alert("anbu");
                              var rowId = parseInt($(this).attr("id"));
                              CalculateTotalPrice(rowId);
                          })
                      }
                  }
              },
              { name: 'DamagedQty', index: 'DamagedQty', width: 90, editable: true, editrules: { integer: true} },
              { name: 'UnitPrice', index: 'UnitPrice', width: 90, editable: true, editoptions: {
                  dataInit: function (element) {
                      $(element).keyup(function () {
                          var rowId = parseInt($(this).attr("id"));
                          CalculateTotalPrice(rowId);
                      })
                  }
              }
              },
              { name: 'TotalPrice', index: 'TotalPrice', width: 90, editable: true },
              { name: 'DamageDescription', index: 'DamageDescription', width: 200, editable: true, edittype: "textarea", editoptions: { rows: "1", cols: "25"} },
              { name: 'Delete', index: 'Delete', width: 30, align: "center", sortable: false, formatter: frmtrDel }
              ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [50, 100, 150, 200],
        sortname: 'SkuId',
        sortorder: 'asc',
        height: '230',
        autowidth: true,
        //shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Material SKU List',
        forceFit: true,
        gridview: true,
        editurl: '/Store/UpdateSKU',
        gridComplete: function () {
            var rdata = $(grid_selector).getRowData();
            if (rdata.length > 0) {
                $('.T1CompDel').click(function () { DeleteComponentDtls($(this).attr('rowid')); });
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            var $this = $(this), rows = this.rows, l = rows.length, i, row;
            $(this).hide();
            for (i = 1; i < l; i++) {
                row = rows[i];
                if ($(row).hasClass("jqgrow")) {
                    $this.jqGrid('editRow', row.id);
                }
            }
            $(this).show();
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
        edit: false,
        editicon: 'ace-icon fa fa-pencil blue',
        add: false,
        addicon: 'ace-icon fa fa-plus-circle purple',
        del: false,
        delicon: 'ace-icon fa fa-trash-o red',
        search: false,
        searchicon: 'ace-icon fa fa-search orange',
        refresh: true,
        refreshicon: 'ace-icon fa fa-refresh green',
        view: false,
        viewicon: 'ace-icon fa fa-search-plus grey'
    }, {}, {}, {}, {})

    $("#MaterialSearch").click(function () {
        var Id = $("#Id").val();
        if (Id == 0) {
            ErrMsg("Please Save");
            return false;
        }
        $("#StoreMaterialsList").jqGrid('resetSelection');
        ModifiedLoadPopupDynamicaly("/Store/MaterialSearch", $('#DivMaterialSearch'),
            function () {
                LoadSetGridParam($('#StoreMaterialsList'), "/Store/StoreSKUListJqGridForMaterialInward")
            }, function () { }, 900, 700, "Material Details");
    });
    $("#btnBack").click(function () {
        window.location.href = $('#BackUrl').val(); // 'Url.Action("MaterialInwardList", "Store")';
    });
    //    function LoadSetGridParam(GridId, brUrl) {
    //        GridId.setGridParam({
    //            url: brUrl,
    //            datatype: 'json',
    //            mtype: 'GET'
    //        }).trigger("reloadGrid");
    //    }
    //    function LoadSetGridParam1(GridId1, brUrl1) {

    //        GridId1.setGridParam({
    //            url: brUrl1,
    //            datatype: 'json',
    //            mtype: 'GET'
    //        }).trigger("reloadGrid");
    //    }

    $("#txtOrderQuantity").keyup(function () {
        NumbersOnly($(this).val());
    });
    $("#txtOrderQuantity").keyup(function () {
        NumbersOnly($(this).val());
    });
    $("#txtReceivedQty").keyup(function () {
        NumbersOnly($(this).val());
    });
    $("#txtDamagedQty").keyup(function () {
        NumbersOnly($(this).val());
    });

});

function CalculateTotalPrice(rowId) {
    $("#" + rowId + "_TotalPrice").val($("#" + rowId + "_UnitPrice").val() * $("#" + rowId + "_ReceivedQty").val());
}

function frmtrDel(cellvalue, options, rowdata) {
    var delBtn = "";
    delBtn = "<span id='T1btnDel_" + options.rowId + "'class='ace-icon fa fa-trash-o red T1CompDel' rowid='" + options.rowId + "' title='Delete'></span>";
    return delBtn;
}
function DeleteComponentDtls(id) {
    if (confirm("Are you sure you want to delete this item?")) {
        DeleteComponentIds(
                '/Store/DeleteSKU?id=' + id, //delURL, 
                '/Store/MaterialSkuListJqGrid?Id=' + $('#Id').val(), //reloadURL, 
                $(grid_selector) //GridId, 
                );
    }
}
function DeleteComponentIds(delURL, reloadURL, GridId) {
    $.ajax({
        url: delURL,
        type: 'POST',
        dataType: 'json',
        traditional: true,
        success: function (data) {
            LoadSetGridParam(GridId, reloadURL);
        },
        loadError: function (xhr, status, error) {
            msgError = $.parseJSON(xhr.responseText).Message;
            ErrMsg(msgError, function () { });
        }
    });
}

function SaveValidation() {
    var objMatInward = {
        Id: $("#Id").val(),
        //  RequestedDate: $("#RequestedDate").val(),
        ProcessedBy: $("#ProcessedBy").val(),
        UserRole: $("#UserRole").val(),
        Status: $("#Status").val(),
        Campus: $("#Campus").val(),
        PoNumber: $("#txtPoNumber").val(),
        Supplier: $("#txtSupplierName").val(),
        SuppRefNo: $("#txtSuppRefNum").val(),
        ReceivedBy: $("#txtReceivedBy").val(),
        ReceivedDateTime: $("#ReceivedDate").val(),
        Store: $("#ddlStore").val(),
        InvoiceDate: $("#InvoiceDate").val(),
        InvoiceAmount: $("#InvoiceAmount").val(),
        DCNumber: $("#DCNumber").val(),
        DCDate: $("#DCDate").val(),
        VehicleType: $("#VehicleType").val(),
        VehicleNo: $("#VehicleNo").val(),
        DriverName: $("#DriverName").val(),
        DriverContactNo: $("#DriverContactNo").val()

    };
    var Supplier = $("#txtSupplierName").val();
    var SuppRefNo = $("#txtSuppRefNum").val();
    var ReceivedBy = $("#txtReceivedBy").val();
    var ReceivedDateTime = $("#ReceivedDate").val();
    var Store = $("#ddlStore").val();
    var InvoiceDate = $("#InvoiceDate").val();
    var InvoiceAmount = $("#InvoiceAmount").val();
    var DCNumber = $("#DCNumber").val();
    var DCDate = $("#DCDate").val();
    var PoNumber = $("#txtPoNumber").val();
    var VehicleType = $("#VehicleType").val();
    var VehicleNo = $("#VehicleNo").val();
    var DriverName = $("#DriverName").val();
    var DriverContactNo = $("#DriverContactNo").val();

    if (Supplier == "") {
        ErrMsg("Please Select Supplier Name");
        return false;
    }

    //        if (SuppRefNo == "") {
    //            ErrMsg("Please type Supplier Reference Number");
    //            return false;
    //        }
    if (ReceivedBy == "") {
        ErrMsg("Please type received by");
        return false;
    }


    if (Store == "") {
        ErrMsg("Please select Store.");
        return false;
    }
    if (InvoiceDate == "") {
        ErrMsg("Please select Invoice Date.");
        return false;
    }
    //        if (DCNumber == "") {
    //            ErrMsg("Please type DC Number.");
    //            return false;
    //        }
    if (DCDate == "") {
        ErrMsg("Please select DC Date.");

        return false;
    }
    if (ReceivedDateTime == "") {
        ErrMsg("Please select received date.");
        return false;
    }
    //        if (VehicleType == "") {
    //            ErrMsg("Please type Vehicle Type.");
    //            return false;
    //        }
    //        if (VehicleNo == "") {
    //            ErrMsg("Please type Vehicle No.");
    //            return false;
    //        }
    //        if (DriverName == "") {
    //            ErrMsg("Please type Driver Name.");
    //            return false;
    //        }
    if (PoNumber.length > 25) {
        ErrMsg("PO Number length should not exceed 25 characters.");
        $("#txtPoNumber").focus();
        $("#txtPoNumber").val('');
        return false;
    }
    if (SuppRefNo.length > 50) {
        ErrMsg("SuppRefNo length should not exceed 50 characters.");
        $("#txtSuppRefNum").focus();
        $("#txtSuppRefNum").val('');
        return false;
    }

    if (isNaN(InvoiceAmount)) {
        ErrMsg("Invoice Amount should be in number.");
        $("#InvoiceAmount").focus();
        $("#InvoiceAmount").val('');
        return false;
    }
    if (InvoiceAmount.length > 12) {
        ErrMsg("Invoice Amount length should not exceed 12 characters.");
        $("#InvoiceAmount").focus();
        $("#InvoiceAmount").val('');
        return false;
    }

    if (DCNumber.length > 50) {
        ErrMsg("DCNumber length should not exceed 50 characters.");
        $("#DCNumber").focus();
        $("#DCNumber").val('');
        return false;
    }
    $.ajax({
        url: '/Store/StartMaterialInward',
        type: 'POST',
        dataType: 'json',
        data: objMatInward,
        traditional: true,
        success: function (data) {
            $("#InwardNumber").val(data);
            $('#Id').val(data.substring(data.lastIndexOf("-") + 1, data.length));
            //$("#btnSave").hide();
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}

function skuValidation() {

    var Id = $("#Id").val();
    if (Id == 0) {
        ErrMsg("Please Save");
        return false;
    }
    if (SaveValidation() == false) {
        return false;
    }
    else {
        var RowList1;
        var selectedData;
        var skuLst1 = '';
        var Id = $("#Id").val();
        var dataIds = $('#MaterialSkuList').jqGrid('getDataIDs');
        if (dataIds == "") {
            ErrMsg("Please add Material. Empty Inward cannot be created.");
            return false;
        }
        for (var i = 0, list = dataIds.length; i < list; i++) {
            var ordqty = $("#" + dataIds[i] + "_OrderQty").val();
            var rcvdqty = $("#" + dataIds[i] + "_ReceivedQty").val();
            var dmgqty = $("#" + dataIds[i] + "_DamagedQty").val();
            var dmgdesc = $("#" + dataIds[i] + "_DamageDescription").val();

            var unitprice = $("#" + dataIds[i] + "_UnitPrice").val();
            var totprice = $("#" + dataIds[i] + "_TotalPrice").val();

            if (unitprice == '' || totprice == '' || unitprice == '0' || totprice == '0') {
                debugger;
                ErrMsg("Unit Price and Total Price are mandatory");
                return fals;
            }

            if (dmgqty != "") {
                if (parseInt(dmgqty) != 0 && dmgdesc == "") {
                    ErrMsg("Please type Damage description", function () {
                        $("#" + dataIds[i] + "_DamageDescription").focus();
                        jQuery('#MaterialSkuList').editRow(dataIds[i], true);
                    });
                    return false;
                    break;
                }
                else if (parseInt(dmgqty) == 0 && dmgdesc != "") {
                    //                                   ErrMsg("Damage description not needed since there is no damaged quantity", function () {
                    //                                       $("#" + dataIds[i] + "_DamageDescription").focus();
                    //                                       jQuery('#MaterialSkuList').editRow(dataIds[i], true);
                    //                                   });
                    //                                   // $("#" + dataIds[i] + "_DamageDescription").val('');
                    //                                   return false;
                    //                                   break;
                }
                else if (parseInt(dmgqty) > parseInt(rcvdqty)) {
                    ErrMsg("Damaged Qty should not exceed Received Qty", function () {
                        $("#" + dataIds[i] + "_DamagedQty").focus();
                        jQuery(grid_selector).editRow(dataIds[i], true);
                    });
                    return false;
                    break;
                }
            }
            else {
                if (dmgqty == "" && dmgdesc != "") {
                }
            }
            jQuery(grid_selector).saveRow(dataIds[i], true);
            jQuery(grid_selector).editRow(dataIds[i], true);
        }
        $(grid_selector).setGridParam({ url: '/Store/MaterialSkuListJqGrid?Id=' + Id }).trigger("reloadGrid");
        return true;
    }
}
function updatePagerIcons(table) {
    var replacement =
            {
                'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
                'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
                'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
                'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
            };
    $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

        if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
    })
}

function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}