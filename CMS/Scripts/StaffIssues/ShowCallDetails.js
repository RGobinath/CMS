$(function () {
    //FillPerformerDll();
    var Result = $("#viewbagresult").val();
    var CountName = $("#viewbagcountname").val();
    var Perfomer = $("#viewbagperformer").val();
    var DateType = $("#ddlDateType").val();
    var FromDate = $("#txtFromDate").val()
    var ToDate = $("#txtToDate").val()
    var DueDateType = $("#ddlDueDateType").val();
    var Campus = $("#ddlCampus").val();
    var IssueGroup = $("#ddlIssueGroup").val();
    var IssueType = $("#ddlIssueType").val();
    var grid_selector = "#ShowCallDetailsList";
    var pager_selector = "#ShowCallDetailsListPager";
    $(grid_selector).jqGrid({
        url: '/StaffIssues/ShowCallDetailsListJqGrid?CountName=' + CountName + '&Result=' + Result + '&Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&DueDateType=' + DueDateType + '&DateType=' + DateType + '&Campus=' + Campus + '&IssueGroup=' + IssueGroup + '&IssueType=' + IssueType,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Performer', 'Issue Number', 'Campus', 'Issue Group', 'Issue Type', 'Description', 'Created By', 'Created Date', 'Status', 'Resolution', 'Resolved Date', 'Due Date', 'SLA Status', 'DueDateType', 'Issue Hours'],
        colModel: [
                    { name: 'Performer', index: 'Performer', width: 50 },
                    { name: 'IssueNumber', index: 'IssueNumber', key: true, width: 50 },
                    { name: 'BranchCode', index: 'BranchCode', width: 50 },
                    { name: 'IssueGroup', index: 'IssueGroup', width: 50 },
                    { name: 'IssueType', index: 'IssueType', width: 50 },
                    { name: 'Description', index: 'Description', width: 150 },
                    { name: 'CreatedBy', index: 'CreatedBy', width: 50 },
                    { name: 'CreatedDate', index: 'CreatedDate', width: 60 },
                    { name: 'Status', index: 'Status', width: 90 },
                    { name: 'Resolution', index: 'Resolution', width: 90 },
                    { name: 'ModifiedDate', index: 'ModifiedDate', width: 60 },
                    { name: 'DueDate', index: 'DueDate', width: 60 },
                    { name: 'SLAStatus', index: 'SLAStatus', formatter: DueDateStatus, width: 20 },
                    { name: 'DueDateType', index: 'DueDateType', width: 50, align: 'center', formatter: txtDueDateStatus, resizable: true, sortable: false },
                    { name: 'TotalDays', index: 'TotalDays', width: 60 }
        ],
        pager: pager_selector,
        rowNum: '10000',
        rowList: [10000, 20000, 50000, 100000, 150000, 200000],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1250,
        autowidth: true,
        height: 300,
        viewrecords: true,
        shrinkToFit: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {

                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        //    loadError: function (xhr, status, error) {
        //    $(grid_selector).clearGridData();
        //    ErrMsg($.parseJSON(xhr.responseText).Message);
        //},
        caption: '<i class="fa fa-list"></i> Staff Issue Management'
    });
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
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {})
    if ($("#showexcel").val() == "True") {
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
            onClickButton: function () {
                //window.open("ShowCallDetailsListJqGrid" + '?Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&DateType=' + DateType + '&DueDateType=' + DueDateType + '&Result=' + Result + '&CountName=' + CountName + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999' + '&ExptXl=1');
                window.open("ExportToExcelSIMR" + '?Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&DateType=' + DateType + '&DueDateType=' + DueDateType + '&Result=' + Result + '&CountName=' + CountName + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999');
            }
        });
    }
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-pdf-o'></i> Export To PDF",
        onClickButton: function () {
            //window.open("ShowCallDetailsListJqGrid1" + '?Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&Result=' + Result + '&HeaderName=' + HeaderName + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999' + '&ExptXl=1');
            window.open("StaffIssueManagementReportPDF" + '?Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&DateType=' + DateType + '&DueDateType=' + DueDateType + '&Result=' + Result + '&CountName=' + CountName + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999');
            //window.open("ExportToExcel");
        }
    });
    //$("#btnReset").click(function () {
    //    $("input[type=text], textarea, select").val("");
    //    jQuery(grid_selector).setGridParam(
    //   {
    //       datatype: "json",
    //       url: '/StaffIssues/StaffIssueManagementReportListJqGrid',
    //       postData: { Performer: "", FromDate: "", ToDate: "" },
    //       page: 1
    //   }).trigger("reloadGrid");
    //});
    //$('.date-picker').datepicker({
    //    format: 'dd/mm/yyyy',
    //    autoclose: true,
    //    endDate: new Date()
    //});
    //$("#btnSearch").click(function () {
    //    var FromDate = $("#txtFromDate").val();
    //    var ToDate = $("#txtToDate").val();
    //    var Performer = $("#ddlPerformer").val();
    //    var DateType = $("#ddlDateType").val();
    //    var DueDateType = $("#ddlDueDateType").val();
    //    if (DateType != "") {
    //        if (FromDate == "") {
    //            return ErrMsg("Please Select From Date");
    //        }
    //    }
    //    if (FromDate != "") {
    //        if (DateType == "") {
    //            return ErrMsg("Please Select Date Type");
    //        }
    //    }
    //    if (ToDate != "") {
    //        if (DateType == "") {
    //            return ErrMsg("Please Select Date Type");
    //        }
    //        if (FromDate == "") {
    //            return ErrMsg("Please Select From Date");
    //        }
    //    }
    //    jQuery(grid_selector).setGridParam(
    //   {
    //       datatype: "json",
    //       url: '/StaffIssues/StaffIssueManagementReportListJqGrid',
    //       postData: { Performer: Performer, FromDate: FromDate, ToDate: ToDate, DateType: DateType, DueDateType: DueDateType },
    //       page: 1
    //   }).trigger("reloadGrid");
    //});

});
function DueDateStatus(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseFloat(cellvalue);
    var cml = $("#ShowCallDetailsList").jqGrid();
    for (i = 0; i < cml.length; i++) {
        if (rowObject[11] == "") {
            if (cellValueInt <= 24) {
                return '<img src="../../Images/blue.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellvalue == 'Completed') {
                return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
        }
        else {
            if (cellValueInt <= 0) {
                return '<img src="../../Images/yellow.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellValueInt > 0 && cellValueInt <= 24) {
                return '<img src="../../Images/orange.jpg" height="10px" width="10px"  alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellValueInt > 24) {
                return '<img src="../../Images/redblink3.gif" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellvalue == 'Completed') {
                return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
        }
    }
}
function txtDueDateStatus(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseFloat(cellvalue);
    var cml = $("#ShowCallDetailsList").jqGrid();
    for (i = 0; i < cml.length; i++) {
        if (rowObject[11] == "") {
            return "Not Completed";
        }
        else {
            if (cellvalue == "NotCompleted") {
                return "Not Completed";
            }
            else if (cellValueInt <= 0) {
                return "Completed Before Due Date";
            }
            else if (cellValueInt > 0 && cellValueInt <= 24) {
                return "Completed Above Due Date within 24 hours";
            }
            else if (cellValueInt > 24) {
                return "Completed Above Due Date after 24 hours";
            }
        }
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
