function GetVendorNameByVendorType1() {
    var VendorType = "";
    if ($("#SrchddlDocumentType").val() == "PurchaseInvoice") {
        VendorType = "Purchase";
    }
    if ($("#SrchddlDocumentType").val() == "ServiceInvoice") {
        VendorType = "Service";
    }
    var ddlbc = $("#SrchddlVendor");
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
//function FillYear() {
//    var ddlyear = $("#ddlYear");
//    ddlyear.empty();
//    ddlyear.append($('<option/>', { value: "", text: "Year" }));
//    var len = 10;
//    for (var i = 0; i <= len; i++) {
//        ddlyear.append($('<option/>', { value: i, text: i }));
//    }
//}
//function FillMonth() {
//    var ddlmonth = $("#ddlMonth");
//    ddlmonth.empty();
//    ddlmonth.append($('<option/>', { value: "", text: "Month" }));
//    var len = 11;
//    for (var i = 0; i <= len; i++) {
//        ddlmonth.append($('<option/>', { value: i, text: i }));
//    }
//}
function GetVendorNameByVendorType() {
    var VendorType = "";
    if ($("#ddlDocumentType").val() == "PurchaseInvoice") {
        VendorType = "Purchase";
    }
    if ($("#ddlDocumentType").val() == "ServiceInvoice") {
        VendorType = "Service";
    }
    var ddlbc = $("#ddlVendor");
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
jQuery(function ($) {
    $.extend({
        handleError: function (s, xhr, status, e) {
            // If a local callback was specified, fire it
            if (s.error)
                s.error(xhr, status, e);
                // If we have some XML response text (e.g. from an AJAX call) then log it in the console
            else if (xhr.responseText)
                console.log(xhr.responseText);
        }
    });
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    //FillYear();
    //FillMonth();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/Asset/AssetInvoiceDetailsJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['InvoiceDetailsId', 'Vendor Name', 'Invoice No', 'Document Type', 'Document Name', 'Document Size', 'Document Data', 'Uploaded Date', 'Invoice Date', 'Amount', 'Number Of Assets', 'Asset Count', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'InvoiceDetailsId', index: 'InvoiceDetailsId', hidden: true },
                      { name: 'VendorId', index: 'VendorId' },
                      { name: 'InvoiceNo', index: 'InvoiceNo' },
                      { name: 'DocumentType', index: 'DocumentType' },
                      { name: 'DocumentName', index: 'DocumentName' },
                      { name: 'DocumentSize', index: 'DocumentSize', hidden: true },
                      { name: 'DocumentData', index: 'DocumentData', hidden: true },
                      { name: 'UploadedDate', index: 'UploadedDate', hidden: true },
                      { name: 'InvoiceDate', index: 'InvoiceDate' },
                      { name: 'Amount', index: 'Amount', formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                      //{ name: 'Warranty', index: 'Warranty' },
                      { name: 'TotalAsset', index: 'TotalAsset' },                                            
                      { name: 'AssetCount', index: 'AssetCount', hidden: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        altRows: true,
        footerrow: true,
        userDataOnFooter: true,
        sortname: 'InvoiceDetailsId',
        sortorder: 'Asc',
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            $(grid_selector).footerData('set', { InvoiceDate: 'Total:' });
            //var colAmount = $(grid_selector).jqGrid('getCol', 'Amount', false, 'sum');
            //$(grid_selector).footerData('set', { Amount: colAmount });
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Asset Invoice Details",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
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
            },
            {
                //width: 'auto', url: '/Asset/EditAssetModelMaster', modal: false, closeAfterEdit: true
                //url: '/Common/AddAcademicMaster/?test=edit', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                // width: 'auto', url: '/Asset/AddAssetModelMaster', modal: false, closeAfterAdd: true
                //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
              {
                  //width: 'auto', url: '/Asset/DeleteAssetModelMaster', beforeShowForm: function (params) {
                  //    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                  //    return { Id: selectedrows }
                  //}
              },
               {},
                {})
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        //FillBlockByCampus();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/AssetInvoiceDetailsJqGrid',
           postData: { DocumentType: "", InvoiceDate: "", InvoiceNo: "", VendorId: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        //var Campus = $("#ddlCampus").val();
        var DocumentType = $("#SrchddlDocumentType").val();
        var VendorId = $("#SrchddlVendor").val();
        var InvoiceNo = $("#SrchtxtInvoiceNo").val();
        var InvoiceDate = $("#SrchtxtInvoiceDate").val();
        //var Warranty = $("#SrchtxtWarranty").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/AssetInvoiceDetailsJqGrid',
           postData: { DocumentType: DocumentType, VendorId: VendorId, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#ddlDocumentType").change(function () {
        GetVendorNameByVendorType();
    });
    $("#SrchddlDocumentType").change(function () {
        GetVendorNameByVendorType1();
    });
    //$("#txtWarranty").keyup(function () {
    //    var value = $(this).val();
    //    if (isNaN(value)) {
    //        ErrMsg("Numbers only allowed.");
    //        $("#txtWarranty").val('');
    //        return false;
    //    }
    //});
    $("#txtTotalAsset").keyup(function () {
        var value = $(this).val();
        if (isNaN(value)) {
            ErrMsg("Numbers only allowed.");
            $("#txtTotalAsset").val('');
            return false;
        }
    });
    $('.date-picker').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        endDate: new Date(),
    });
    //replace icons with FontAwesome icons like above
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
    function SpecialCharacters(strValidate) {
        if (strValidate.indexOf('&') != -1 || strValidate.indexOf("'") != -1 ||
            strValidate.indexOf(";") != -1 || strValidate.indexOf("#") != -1) {
            return true;
        } else {
            return false;
        }
    }

    function validateDocReq(msg, reqField, isValid) {
        var fieldVal = $('#' + reqField).val();
        if ((typeof isValid != 'undefined' && isValid) || fieldVal == null || fieldVal == "") {
            ErrMsg(msg, function () { $('#' + reqField).focus(); });
            return false;
        } else {
            return true;
        }
    }

    $('#Upload').click(function () {
        var splitstr = $('#file1').val().split('\\');
        var fileName = splitstr[splitstr.length - 1];
        var DocTypeText = $('#ddlDocumentType option:selected').text();
        if (!validateDocReq("No document found. Please select a file to upload!!!", 'file1')) { }
        else if (!validateDocReq("Special characters (&,#,;') are not supported in document file names. Please amend the file name before upload!!!", 'file1', SpecialCharacters(fileName))) { }
        else if (!validateDocReq("The attached file does not contain file extension.", 'file1', (fileName.lastIndexOf('.') == -1))) { }
        else {
            ajaxUploadDocs();
            return false;
        }
    });
    $("#txtInvoiceAmount").keypress(function (e) {
        debugger;
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    function ajaxUploadDocs() {
        debugger;
        //var Warranty = 0;
        var DocTypeText = $('#ddlDocumentType option:selected').text();
        var VendorId = $("#ddlVendor").val();
        var InvoiceNo = $("#txtInvoiceNo").val();
        var InvoiceDate = $("#txtInvoiceDate").val();
        var InvoiceAmount = $("#txtInvoiceAmount").val();
        //var Year = $("#ddlYear").val();
        //var Month = $("#ddlMonth").val();       
        //if (parseInt(Year) > 0) {
        //    Warranty = parseInt(Warranty) + parseInt(Year) * 12;                     
        //}
        //if (parseInt(Month) > 0) {
        //    Warranty = parseInt(Warranty) + parseInt(Month);                        
        //}
        if ($('#ddlDocumentType').val() == "" || $("#ddlVendor").val() == "" || $("#txtInvoiceNo").val() == "" || $("#txtInvoiceDate").val() == "" || $("#txtTotalAsset").val() == "" || $("#txtInvoiceAmount").val() == "") {
            ErrMsg("Please Fill Required Fields!!");
            return false;
        }
        else if (InvoiceAmount == 0 || InvoiceAmount < 0) {
            ErrMsg("Invoice Amount Must be Greater than zero!!");
            return false;
        }
        else if ($('#file1').val() == " " || $('#file1').val() == "") {
            ErrMsg("Please Upload a Document!!");
            return false;
        }
        var TotalAsset = $("#txtTotalAsset").val();
        $.ajaxFileUpload({
            url: '/Asset/AddAssetInvoiceDetails?DocumentType=' + DocTypeText + '&VendorId=' + VendorId + '&InvoiceNo=' + InvoiceNo + '&InvoiceDate=' + InvoiceDate + '&TotalAsset=' + TotalAsset + '&Amount=' + InvoiceAmount,
            secureuri: false,
            fileElementId: 'file1',
            dataType: 'json',
            success: function (data, status) {
                debugger;
                $('#grid-table').trigger("reloadGrid");
                $('#file1').val('');
                $("input[type=text], textarea, select").val("");

                if (typeof data.result != 'undefined' && data.result != '') {
                    debugger;
                    if (typeof data.success != 'undefined' && data.success == true) {
                        $('#file1').val('');
                        SucessMsg(data.result);
                    } else {
                        ErrMsg(data.result);
                    }
                }
            },
            error: function (data, status, e) {
            }
        });
    }
});
//function uploadvalidate() {
//    alert($("#ddlYear").val());
//    if ($('#ddlDocumentType').val() == "" || $("#ddlVendor").val() == "" || $("#txtInvoiceNo").val() == "" || $("#txtInvoiceDate").val() == "" || $("#txtTotalAsset").val() == "" || $("#ddlYear").val() == "" || $("#ddlMonth").val() == "") {
//        ErrMsg("Please Fill Required Fields!!");
//        return false;
//    }
//    else if ($('#file1').val() == " " || $('#file1').val() == "") {
//        ErrMsg("Please Upload a Document!!");
//        return false;
//    }
//    else {
//        return true;
//    }
//}
function NumbersOnly(value) {
    if (isNaN(value)) {
        ErrMsg("Numbers only allowed.");
        return false;
    }
}
function uploaddat(id1) {    
    window.location.href = "/Asset/uploaddisplay?Id=" + id1;    
}