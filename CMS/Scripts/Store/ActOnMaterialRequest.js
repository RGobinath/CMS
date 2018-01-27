var grid_selector = "#MaterialRequestList";
var pager_selector = "#MaterialRequestListPager";
var commentgrid_selector = "#CommentList";
var commentpager_selector = "#CommentListpager";
$(function () {
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
        $(commentgrid_selector).jqGrid('setGridWidth', $(".col-xs-8").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    var parent_column1 = $(commentgrid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                $(commentgrid_selector).jqGrid('setGridWidth', parent_column1.width());
            }, 0);
        }
    })
    var Id = $("#Id").val();
    var FromStore;
    if ($("#RequiredFromStore").val() != '')
        FromStore = $("#RequiredFromStore").val();
    else {
        var cam = $("#RequiredForCampus").val();
        $.ajax({
            type: 'POST',
            async: false,
            dataType: "json",
            url: '/Store/FillStore?Campus=' + cam,
            success: function (data) {
                FromStore = data.rows[0].Value;
            }
        });
    }
    jQuery(grid_selector).jqGrid({
        url: '/Store/ActOnMaterialRequestJqGrid?Id=' + Id + '&Store=' + FromStore,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Req.Type', 'Grade', 'Section', 'Req.For', 'Material', 'Material Group', 'Material Sub Group', 'Units', 'Req.Date', 'Status', 'Inward Ids', 'Available Qtys', 'Unit Prices', 'Tax', 'Discount', 'Req.Qty', 'App.Qty', 'Iss.Qty', ''],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'RequestType', index: 'RequestType', width: 90, sortable: true },
              { name: 'RequiredForGrade', index: 'RequiredForGrade', width: 40, sortable: true },
              { name: 'Section', index: 'Section', width: 40, sortable: true },
              { name: 'RequiredFor', index: 'RequiredFor', width: 110, sortable: true },
              { name: 'Material', index: 'Material', width: 90, sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[6] + ', Material Sub Group:' + rawObject[7] + '"' } },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true, hidden: true },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, sortable: true, hidden: true },
              { name: 'Units', index: 'Units', width: 40, sortable: true },
              { name: 'RequiredDate', index: 'RequiredDate', width: 90, sortable: true },
              { name: 'Status', index: 'Status', width: 90, sortable: true },
              { name: 'InwardIds', index: 'InwardIds', width: 90 },
              { name: 'ClosingBalance', index: 'ClosingBalance', width: 90, cellattr: function (rowId, cellValue, rawObject, cm, rdata) {
                  if (cellValue == 0) {
                      return 'class="ui-state-error ui-state-error-text"';
                  }
              }
              },
              { name: 'UnitPrices', index: 'UnitPrices', width: 90 },
              { name: 'Taxes', index: 'Taxes', width: 90 },
              { name: 'Discounts', index: 'Discounts', width: 90 },
              { name: 'Quantity', index: 'Quantity', width: 90, sortable: true, editrules: { required: true, integer: true }, editable: true },
              { name: 'ApprovedQty', index: 'ApprovedQty', width: 90, sortable: true, editrules: { required: true, integer: true }, editable: true },
              { name: 'IssuedQty', index: 'IssuedQty', width: 90, sortable: true, editable: false, editrules: { required: true, integer: true} },
              { name: 'Delete', index: 'Delete', width: 30, align: "center", sortable: false, formatter: frmtrDel },
              ],
        pager: '#MaterialRequestListPager',
        rowNum: '100',
        rowList: [100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '180',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: 'Material Request List',
        forceFit: true,
        //  multiselect: true,
        // cellEdit: true,
        // cellurl: '/Store/UpdateQty',
        editurl: '/Store/UpdateQty',
        onCellSelect: function (id) {
            if ($("#RequestStatus").val() != "ApproveMatRequest") {
                jQuery(grid_selector).setColProp('ApprovedQty', { editable: false });
            }
            else {
                jQuery(grid_selector).setColProp('ApprovedQty', { editable: true });
            }
            if ($("#RequestStatus").val() != "ApproveMatRequestRejection") {
                jQuery(grid_selector).setColProp('Quantity', { editable: false });
            }
            else {
                jQuery(grid_selector).setColProp('Quantity', { editable: true });
            }
            var selectedData = $('#MaterialRequestList').jqGrid('getRowData', id);
            if (selectedData.Status != "Approved") {
                jQuery(grid_selector).setColProp('IssuedQty', { editable: false });
            }
        },
        afterSaveCell: function () {
            $(this).trigger('reloadGrid');
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
                    var rdata = $("#MaterialRequestList").getRowData();
                    $this.jqGrid('editRow', row.id);
                }
            }
            $(this).show();
        },
        gridComplete: function () {
            var rdata = $("#MaterialRequestList").getRowData();
            if ($("#RequestStatus").val() == "ApproveMatRequest") {
                jQuery(grid_selector).setColProp('IssuedQty', { hidden: true });
            }
            if ($("#RequestStatus").val() != "ApproveMatRequestRejection") {
                jQuery(grid_selector).setColProp('Quantity', { editable: false });
            }
            if ($("#RequestStatus").val() != "ApproveMatRequest") {
                jQuery(grid_selector).setColProp('ApprovedQty', { editable: false });
            }
            if (rdata.length > 0) {
                $('.T1CompDel').click(function () { DeleteComponentDtls($(this).attr('rowid')); });
            }
            if ($('#Role').val() == "MRC") {
                var myGrid = $(grid_selector);
                myGrid.jqGrid('hideCol', myGrid.getGridParam("colModel")[12].name);
                myGrid.jqGrid('hideCol', myGrid.getGridParam("colModel")[13].name);
                myGrid.jqGrid('hideCol', myGrid.getGridParam("colModel")[14].name);
                myGrid.jqGrid('setGridWidth', $(".page-content").width());
            }
        },
        subGrid: true,
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (MatIssueList, Id) {
            var MatIssueListTable, MatIssueListPager;
            MatIssueListTable = MatIssueList + "_t";
            MatIssueListPager = "p_" + MatIssueListTable;
            $("#" + MatIssueList).html("<table id='" + MatIssueListTable + "' ></table><div id='" + MatIssueListPager + "' ></div>");
            jQuery("#" + MatIssueListTable).jqGrid({
                url: '/Store/MaterialIssueListJqGrid?Id=' + Id,
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'MRL Id', 'Iss Note Number', 'Issue Date', 'Issued Qty', 'Issued By', 'Status'],
                colModel: [
                       { name: 'Id', index: 'Id', hidden: true },
                       { name: 'MRLId', index: 'MRLId', width: 90, sortable: true, hidden: true },
                       { name: 'IssNoteNumber', index: 'IssNoteNumber', width: 90, sortable: true },
                       { name: 'IssueDate', index: 'IssueDate', width: 90, sortable: true },
                       { name: 'IssueQty', index: 'IssueQty', width: 90, sortable: true },
                       { name: 'IssuedBy', index: 'IssuedBy', width: 90, sortable: true },
                       { name: 'Status', index: 'Status', width: 90, sortable: true },
                       ],
                pager: MatIssueListPager,
                rowNum: '5',
                rowList: [5, 10, 20, 50, 100, 150, 200],
                sortname: 'Id',
                sortorder: "Asc",
                height: '130',
                autowidth: true,
                shrinkToFit: true,
                viewrecords: true,
                forceFit: true,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                }
            });
            jQuery("#" + MatIssueListTable).jqGrid('navGrid', "#" + MatIssueListPager, { edit: false, add: false, del: false, search: false, searchicon: 'ace-icon fa fa-search orange', refresh: true, refreshicon: 'ace-icon fa fa-refresh green' })
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

    $("#gs_Material").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $("#btnIdReject").click(function () {
        if ($("#txtRejDescription").val() == "") {
            ErrMsg("Please enter rejection comments", function () { $("#txtRejDescription").focus(); });
            return false;
        }
        if (rejectValidation() == false) {
            return false;
        }
        else {
            var Id = $("#Id").val();
            var MR = {
                Id: Id,
                RejComments: $("#txtRejDescription").val()
            }
            $.ajax({
                url: '/Store/RejectRequest',
                type: 'GET',
                dataType: 'json',
                data: MR,
                traditional: true,
                success: function (data) {
                    InfoMsg("Material Request has been rejected", function () { window.location.href = $('#BackUrl').val(); });
                }
            });
        }
    });
    //        $("#btnIdReject1").click(function () {
    //            if ($("#txtRejDescription").val() == "") {
    //                ErrMsg("Please enter rejection comments", function () { $("#txtRejDescription").focus(); });
    //                return false;
    //            }
    //        });
    $("#btnIdReply").click(function () {
        if ($("#txtReplyDescription").val() == "") {
            ErrMsg("Please enter reply comments", function () { $("#txtReplyDescription").focus(); });
            return false;
        }
        if (replyValidation() == false) {
            return false;
        }
        else {
            var Id = $("#Id").val();
            var MR = {
                Id: Id,
                ReplyComments: $("#txtReplyDescription").val()
            }
            $.ajax({
                url: '/Store/ReplyRequest',
                type: 'GET',
                dataType: 'json',
                data: MR,
                traditional: true,
                success: function (data) {
                    InfoMsg("Material Request submitted successfully", function () { window.location.href = $('#BackUrl').val(); });
                }
            });
        }
    });

    $("#btnIdApprove").click(function () {
        var Id = $("#Id").val();
        if ($("#txtResDescription").val() == "") {
            ErrMsg("Please enter Approve  comments", function () { $("#txtResDescription").focus(); });
            return false;
        }
        if (approveValidation() == false) {
            return false;
        }
        else {
            var MR = {
                Id: Id,
                ApproverComments: $("#txtResDescription").val()
            }
            $.ajax({
                url: '/Store/ApproveRequest',
                type: 'GET',
                dataType: 'json',
                data: MR,
                traditional: true,
                success: function (data) {
                    InfoMsg("Material Request has been approved", function () { window.location.href = $('#BackUrl').val(); });
                }
            });
        }
    });

    $("#btnBack").click(function () {
        window.location.href = $('#BackUrl').val();
    });

    $("#CommentList").jqGrid({
        url: '/Store/DescriptionForSelectedIdJqGrid?Id=' + $('#Id').val(),
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
        colModel: [
        // { name: 'Issue Number', index: 'EntityRefId', sortable: false },
              {name: 'CommentedBy', index: 'CommentedBy', sortable: false },
              { name: 'CommentedOn', index: 'CommentedOn', sortable: false },
              { name: 'RejectionComments', index: 'RejectionComments', sortable: false },
              { name: 'ResolutionComments', index: 'ResolutionComments', sortable: false }
             ],
        rowNum: -1,
        pager: CommentListpager,
        //width: 1160,
        autowidth: true,
        height: 110,
        sortname: 'EntityRefId',
        sortorder: "desc",
        viewrecords: true,
        caption: 'Discussion Forum'
    });
    jQuery(commentgrid_selector).jqGrid('navGrid', pager_selector, {
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
    $("#MaterialSearch1").click(function () {
        var Id = $("#Id").val();
        var BranchCode = $("#ddlReqForCamp").val();
        $('#StoreMaterialsList').clearGridData();
        ModifiedLoadPopupDynamicaly("/Store/MaterialSearchOnRejection?Campus=" + BranchCode, $('#DivMaterialSearch'),
            function () { LoadSetGridParam($('#StoreMaterialsList'), "/Store/StoreSKUListJqGrid") }, function () { }, 1100, 700, "Material Details");
    });

    $("#btnbkToAvailable").click(function () {
        $.ajax({
            url: '/Store/MoveBackToAvailable?ActivityId=' + $('#ActivityId').val(),
            type: 'GET',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                if (data & data == true) {
                    SucessMsg("MRF-" + $('#Id').val() + " is moved back to available.", function () { window.location.href = $('#BackUrl').val(); });
                } else { }
            },
            error: function (xhr, status, error) {
                ErrMsg($.parseJSON(xhr.responseText).Message);
            }
        });
    });
});
function approveValidation() {
    var dataIds = $('#MaterialRequestList').jqGrid('getDataIDs');
    if (dataIds == "") {
        ErrMsg("Empty items cannot be approved.");
        return false;
    }
    for (var i = 0, list = dataIds.length; i < list; i++) {
        selectedData = $('#MaterialRequestList').jqGrid('getRowData', dataIds[i]);
        //var ReqQty = $("#" + dataIds[i] + "_Quantity").val(); 
        var AppQty = $("#" + dataIds[i] + "_ApprovedQty").val();
        if (AppQty == "") {
            ErrMsg("Please type Approved Qty");
            $("#" + dataIds[i] + "_ApprovedQty").focus();
            return false;
            break;
        }
        if (isNaN(AppQty)) {
            ErrMsg("Numbers only allowed");
            // $("#" + dataIds[i] + "_IssQty").val('');
            $("#" + dataIds[i] + "_ApprovedQty").focus();
            return false;
            break;
        }
        if (parseInt(AppQty) > parseInt(selectedData.Quantity)) {
            ErrMsg("Approved Quantity should not exceed Requested Quantity");
            //$("#" + dataIds[i] + "_IssQty").val('');
            $("#" + dataIds[i] + "_ApprovedQty").focus();
            return false;
            break;
        }
        var AppList = {
            Id: selectedData.Id,
            ApprovedQty: AppQty
        }
        $.ajax({
            url: '/Store/UpdateQty',
            type: 'POST',
            dataType: 'json',
            data: AppList,
            traditional: true,
            success: function (data) {
            }
        });
    }

}

function rejectValidation() {
    var dataIds = $('#MaterialRequestList').jqGrid('getDataIDs');
    if (dataIds == "") {
        ErrMsg("Empty items cannot be approved.");
        return false;
    }
    for (var i = 0, list = dataIds.length; i < list; i++) {
        selectedData = $('#MaterialRequestList').jqGrid('getRowData', dataIds[i]);
        //var ReqQty = $("#" + dataIds[i] + "_Quantity").val(); 
        var AppQty = $("#" + dataIds[i] + "_ApprovedQty").val();
        //                if (AppQty == "") {
        //                    ErrMsg("Please type Approved Qty");
        //                    $("#" + dataIds[i] + "_ApprovedQty").focus();
        //                    return false;
        //                    break;
        //                }
        if (isNaN(AppQty)) {
            ErrMsg("Numbers only allowed");
            // $("#" + dataIds[i] + "_IssQty").val('');
            $("#" + dataIds[i] + "_ApprovedQty").focus();
            return false;
            break;
        }
        if (parseInt(AppQty) > parseInt(selectedData.Quantity)) {
            ErrMsg("Approved Quantity should not exceed Requested Quantity");
            //$("#" + dataIds[i] + "_IssQty").val('');
            $("#" + dataIds[i] + "_ApprovedQty").focus();
            return false;
            break;
        }
        var AppList = {
            Id: selectedData.Id,
            ApprovedQty: AppQty
        }
        $.ajax({
            url: '/Store/UpdateQty',
            type: 'POST',
            dataType: 'json',
            data: AppList,
            traditional: true,
            success: function (data) {
            }
        });
    }

}

function replyValidation() {
    var dataIds = $('#MaterialRequestList').jqGrid('getDataIDs');
    if (dataIds == "") {
        ErrMsg("Empty items cannot be requested.");
        return false;
    }
    for (var i = 0, list = dataIds.length; i < list; i++) {
        selectedData = $('#MaterialRequestList').jqGrid('getRowData', dataIds[i]);
        var ReqQty = $("#" + dataIds[i] + "_Quantity").val();
        var AppQty = selectedData.ApprovedQty;
        if (ReqQty == "") {
            ErrMsg("Please type Request Quantity");
            $("#" + dataIds[i] + "_Quantity").focus();
            return false;
            break;
        }
        if (isNaN(ReqQty)) {
            ErrMsg("Numbers only allowed");
            // $("#" + dataIds[i] + "_IssQty").val('');
            $("#" + dataIds[i] + "_Quantity").focus();
            return false;
            break;
        }
        //                if (ReqQty!="" && !isNaN(AppQty)) {
        //                    if (parseInt(ReqQty) > parseInt(AppQty)) {
        //                        ErrMsg("Current Request Quantity should not exceed Previously Approved Quantity");
        //                        //$("#" + dataIds[i] + "_IssQty").val('');
        //                        $("#" + dataIds[i] + "_Quantity").focus();
        //                        return false;
        //                        break;
        //                    }
        //                }
        var AppList = {
            Id: selectedData.Id,
            Quantity: ReqQty
        }
        $.ajax({
            url: '/Store/UpdateQty',
            type: 'POST',
            dataType: 'json',
            data: AppList,
            traditional: true,
            success: function (data) {
            }
        });
    }

}

function frmtrDel(cellvalue, options, rowdata) {
    var delBtn = "";
    if ($("#RequestStatus").val() == "ApproveMatRequestRejection") {
        delBtn = "<span id='T1btnDel_" + options.rowId + "'class='ace-icon fa fa-trash-o red T1CompDel' rowid='" + options.rowId + "' title='Delete'></span>";
        return delBtn;
    }
    else {
        return '';
    }
}
function DeleteComponentDtls(id) {

    if (confirm("Are you sure you want to delete this item?")) {
        DeleteComponentIds(
                '/Store/DeleteMaterialRequestList?id=' + id, //delURL, 
                '/Store/MaterialRequestJqGrid?Id=' + $('#Id').val(), //reloadURL, 
                $("#MaterialRequestList") //GridId, 
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
            //  $('#MaterialRequestList').jqGrid('delRowData', data);
            //   return true;
            LoadSetGridParam(GridId, reloadURL);
            //  InfoMsg("Material Request deleted Successfully", function () { });
        },
        loadError: function (xhr, status, error) {
            msgError = $.parseJSON(xhr.responseText).Message;
            ErrMsg(msgError, function () { });
        }
    });
}

function updatePagerIcons(table) {
    var replacement = {
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