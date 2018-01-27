jQuery.browser = {};
$(function () {
    //jQuery.curCSS = function (element, prop, val) {
    //    return jQuery(element).css(prop, val);
    //};
    //    if ($("#SubmitSuccessMsg").val() != null & $("#SubmitSuccessMsg").val() != "") {
    //        alert($("#SubmitSuccessMsg").val());
    //        InfoMsg("Issue created Successfully.\n Issue Number is:" + $("#SubmitSuccessMsg").val(), function () { $("#SubmitSuccessMsg").val(""); });
    //    }

    //    $("#ddlSearchBy").change(function () {
    //        if ($("#ddlSearchBy").val() == "") {
    //            $("#txtSearch").val("");
    //            $("#txtSearch").attr("disabled", true);
    //        }
    //        else {
    //            $("#txtSearch").attr("disabled", false);
    //        }
    //    });
    //$("#txtSearch").datepicker("destroy");

    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
    $("#ddlSearchBy").change(function () {
        if ($("#ddlSearchBy").val() == "") {
            $("#txtSearch").val("");
            $("#txtSearch").attr("disabled", true);
        }
        else {
            $('#txtSearch').val('');
            if ($(this).val() == "CreatedDate") {
                $('#txtSearch').datepicker({
                    format: "dd/mm/yyyy",
                    autoclose: true,
                    changeYear: true,
                    changeMonth: true,
                    maxDate: 0,
                    numberOfMonths: 1,
                    timeFormat: 'hh:mm:ss',
                    autowidth: true
                });
                $("#txtSearch").focus();
            }
            else {
                $("#txtSearch").datepicker("remove");
            }
            $("#txtSearch").attr("disabled", false);
        }
    });
    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "blind",
            duration: 1000
        },

    });
    //Added by Thamizhmani
    var grid_selector = "#StaffManagementList";
    var pager_selector = "#StaffManagementListPager";
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
    if ($("#SubmitSuccessMsg").val() != null & $("#SubmitSuccessMsg").val() != "") {
        InfoMsg("Issue created Successfully.\n Issue Number is:" + $("#SubmitSuccessMsg").val(), function () { $("#SubmitSuccessMsg").val(""); });
    }
    $("#btnCreateNewIssue").click(function () {
        window.location.href = '/StaffIssues/StaffIssueCreation';
    });
    $("#ddlStatus").change(function () {
        //var status = $("#ddlStatus").val();
        //if (status == "Sent" || status == "Completed") {
        //    $("#btnBulkComplete").hide();
        //}
        //else {
        //    $("#btnBulkComplete").show();
        //}
        $("#btnSearch").click();
    });
    //        var count = "@ViewBag.count";    
    if ($('#viewbagcount').val() == "0") {
        $("#btnCreateNewIssue").hide();
        //$("#btnBulkComplete").hide();
    }

    $("#btnSearch").click(function () {
        debugger;
        //$(grid_selector).clearGridData();
        var txtSearch = $("#txtSearch").val();
        var ddlSearchBy = $("#ddlSearchBy").val();
        var fromDate = $("#txtFrmDate").val();
        var fromIssueNum = $("#txtFromIssueNum").val();
        var toIssueNum = $("#txtToIssueNum").val();
        var status = $("#ddlStatus").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StaffIssues/StaffIssueManagementListJqGrid/',
                    postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, fromIssueNum: fromIssueNum, toIssueNum: toIssueNum, status: status },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnreset").click(function () {
        $("input[type=text], textarea, select").val("");
        var txtSearch = $("#txtSearch").val();
        var ddlSearchBy = $("#ddlSearchBy").val();
        var fromDate = $("#txtFrmDate").val();
        var fromIssueNum = $("#txtFromIssueNum").val();
        var toIssueNum = $("#txtToIssueNum").val();
        var status = $("#ddlStatus").val();
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: '/StaffIssues/StaffIssueManagementListJqGrid/',
                postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, fromIssueNum: fromIssueNum, toIssueNum: toIssueNum, status: status },
                page: 1
            }).trigger("reloadGrid");
    });
    $("#btnBulkComplete").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var Template = "";
        var userId = "";
        var rowData = [];
        var rowData1 = [];
        var activityName = [];
        var findString = "";
        var i = 0;
        if (GridIdList.length > 0) {
            for (i; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].Id;
                activityName[i] = rowData[i].Status;
                if (activityName[i] != "Complete") {
                    ErrMsg("Please select complete items only to complete");
                    return false;
                }
            }
            $.ajax({
                url: '/StaffIssues/BulkIssueComplete/',
                type: 'POST',
                dataType: 'json',
                data: { ActivityId: rowData1, Template: Template, userId: userId },
                traditional: true,
                // page: 1,
                success: function (data) {
                    if (data == true) {
                        $(grid_selector).clearGridData();
                        var txtSearch = $("#txtSearch").val();
                        var ddlSearchBy = $("#ddlSearchBy").val();
                        var fromDate = $("#txtFrmDate").val();
                        var fromIssueNum = $("#txtFromIssueNum").val();
                        var toIssueNum = $("#txtToIssueNum").val();
                        var status = $("#ddlStatus").val();
                        $(grid_selector).setGridParam(
                             {
                                 datatype: "json",
                                 url: '/StaffIssues/StaffIssueManagementListJqGrid/',
                                 type: 'POST',
                                 postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, fromIssueNum: fromIssueNum, toIssueNum: toIssueNum, status: status },
                                 page: 1
                             }).trigger("reloadGrid");
                    }
                    InfoMsg("Issue completed Successfully");
                }
            });
        }
        if (rowData1 == "") {
            ErrMsg("Please select atleast one row to complete.");
            return false;
        }

    });

    //$("#ddlStatus").val($("#SIMGTStatus").val());   
    //var status = $("#ddlStatus").val();
    if ($("#SIMGTStatus").val() != null && $("#SIMGTStatus").val() != "undefined" && $("#SIMGTStatus").val() != "") {
        $("#ddlStatus").val($("#SIMGTStatus").val());
    }
    var status = $("#ddlStatus").val();
    $(grid_selector).jqGrid({
        url: '/StaffIssues/StaffIssueManagementListJqGrid/?status=' + status,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'ProcessRefId', 'Support Number', 'Campus', 'Issue Group', 'Issue Type', 'Description', 'Created By', 'Created Date', 'Status', 'History', 'Due Date', 'SLA Status', 'SLA Status'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'ProcessRefId', index: 'ProcessRefId', hidden: true },
              { name: 'IssueNumber', index: 'IssueNumber', width: 90, sortable: false },
              { name: 'Campus', index: 'Campus', width: 90, sortable: false },
              { name: 'IssueGroup', index: 'IssueGroup', width: 90, sortable: false },
              { name: 'IssueType ', index: 'IssueType', width: 90, sortable: false },
              { name: 'Description', index: 'Description', width: 90, sortable: false, formatter: frmtrlink },
              { name: 'CreatedBy', index: 'CreatedBy', width: 90, sortable: false },
              { name: 'CreatedDate', index: 'CreatedDate', width: 50, sortable: false },
              { name: 'Status', index: 'Status', width: 90, sortable: false },
              { name: 'History', index: 'History', width: 40, align: 'center', sortable: false },
              { name: 'DueDate', index: 'DueDate', width: 90,sortable:false },
              { name: 'Stat', index: 'Stat', width: 30, align: 'center', formatter: statusimage, resizable: true, sortable: false },
              { name: 'DueDateStatus', index: 'DueDateStatus', width: 30, align: 'center', formatter: DueDateStatus, resizable: true, sortable: false },

        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1250,
        autowidth: true,
        //height: 250,
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
            var ShowDueDate = $("#viewbagShowDueDate").val();
            if (ShowDueDate == "0") {
                $(grid_selector).hideCol("DueDate").setGridWidth(1270);
                $(grid_selector).hideCol("DueDateStatus").setGridWidth(1270);
            }
            if (ShowDueDate == "1") {
                $(grid_selector).hideCol("Stat").setGridWidth(1270);
            }
        },
        loadError: function (xhr, status, error) {
            $(grid_selector).clearGridData();
            //ErrMsg($.parseJSON(xhr.responseText).Message);
        },
        caption: '<i class="fa fa-list"></i> Staff Issue List'
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    $(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, txtSearch: false, refresh: false });
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            var txtSearch = $("#txtSearch").val();
            var ddlSearchBy = $("#ddlSearchBy").val();
            var fromDate = $("#txtFrmDate").val();
            var status = $("#ddlStatus").val();
            var ExptType = 'Excel';
            window.open("/StaffIssues/StaffIssueManagementListJqGrid" + '?ddlSearchBy=' + ddlSearchBy + '&txtSearch=' + txtSearch + '&fromDate=' + fromDate + '&status=' + status
                + '&rows=9999' + '&ExptType=' + ExptType);
        }
    })

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
//enable datepicker
function pickDate(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=text]')
                    .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
    }, 0);
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

function frmtrlink(cellvalue, options, rowObject) {
    var delBtn = "";
    delBtn = "<a onclick=ShowDescription('" + rowObject[1] + "');>" + cellvalue + "...</a>";
    return delBtn;
}
function ShowDescription(Id) {

    $.ajax({
        url: '/StaffIssues/ShowDescription/?Id=' + Id,
        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {
            if (data != 0) {
                $("#dialog").dialog("open");
                $('#showDescription').html(data);
            }
        }
    });
}
function statusimage(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseInt(cellvalue);
    var cml = $("#StaffManagementList").jqGrid();
    for (i = 0; i < cml.length; i++) {
        if (cellValueInt <= 24) {
            return '<img src="../../Images/yellow.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
        else if (cellValueInt > 24 && cellValueInt <= 48) {
            return '<img src="../../Images/orange.jpg" height="10px" width="10px"  alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
        else if (cellValueInt > 48) {
            return '<img src="../../Images/redblink3.gif" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
        else if (cellvalue == 'Completed') {
            return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
    }
}
function DueDateStatus(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseInt(cellvalue);
    var cml = $("#StaffManagementList").jqGrid();
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

function ShowComments(ActivityId) {
    modalid = $('#Activities');
    $('#ActivitiesList').clearGridData();
    ModifiedLoadPopupDynamicaly("/StaffIssues/LoadUserControl/Activities", modalid, function () {
        LoadSetGridParam($('#ActivitiesList'), "/StaffIssues/ActivitiesListJqGrid?Id=" + ActivityId);
    }, function () { }, 800, 500, "History");
}
function ShowDueDate(Id) {
    AddModifiedLoadPopupDynamicaly("/StaffIssues/ShowDueDate?Id=" + Id, $('#DivDueDate'),
                   function () { }, function () { }, 400, 300, "Due Date");
}
var clbPupGrdSel1 = null;
function AddModifiedLoadPopupDynamicaly(dynURL, ModalId, loadCalBack, onSelcalbck, width, height, title) {
    if (width == undefined) { width = 800; }
    if (height == undefined) { height = 800; }
    if (title == undefined) { title = "List" }
    // if (ModalId.html().length == 0) {
    $.ajax({
        url: dynURL,
        type: 'GET',
        async: false,
        dataType: 'html', // <-- to expect an html response
        success: function (data) {
            ModalId.dialog({
                width: width,
                height: height,
                position: { my: 'top', at: 'top+150' },
                dialogClass:'myClass',
                modal: true,
                buttons: {},
                open: function (event, ui) {
                    $(".ui-icon-closethick").hide();
                },
                close: function () {
                    $('#txtDueDate').data("DateTimePicker").destroy();
                }
            }).prev(".ui-dialog-titlebar").css("color", "#a94442");
            
            ModalId.html(data);
        }
    });    
    clbPupGrdSel1 = onSelcalbck;
    ModalId.dialog('open');
    CallBackFunction(loadCalBack);
}