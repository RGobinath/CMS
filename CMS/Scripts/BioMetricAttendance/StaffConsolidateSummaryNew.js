jQuery(function () {
    $("#LoadEditButton").hide();
    var Flag = $("#hdnFlag").val();
    var hdnCampus = $("#hdnCampus").val();
    var grid_selector = "#StaffConsolidateSummaryNewGridList";
    var pager_selector = "#StaffConsolidateSummaryNewGridListPager";

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
    });
    getCampus();
    GetCurrentDate();
    var startDate = new Date('01/01/1947');
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    ToEndDate.setDate(ToEndDate.getDate() + 365);
    $('#AttendanceFromDate').datepicker({
        weekStart: 1,
        startDate: '01/01/1947',
        format: "dd/mm/yyyy",
        todayHighlight: true,
        endDate: FromEndDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#AttendanceToDate').datepicker('setStartDate', startDate);
    });
    $('#AttendanceToDate').datepicker({
        weekStart: 1,
        format: "dd/mm/yyyy",
        todayHighlight: true,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#AttendanceFromDate').datepicker('setEndDate', FromEndDate);
    });
    $("#txtMonthYear").datepicker({
        format: "mm-yyyy",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    });
    $("#AttendanceToDate").focus(function () {
        if ($("#AttendanceFromDate").val() == "") {
            $("#AttendanceToDate").val('');
            return ErrMsg("Please Select FromDate");
        }
    });
    $("#AttendanceFromDate,#AttendanceToDate,#txtMonthYear").focus(function (e) {
        e.preventDefault();
    });
    $("#txtMonthYear").focus(function () {
        if ($("#AttendanceFromDate").val() != "") {
            $("#AttendanceFromDate,#AttendanceToDate").val('');
        }
    });
    $("#AttendanceFromDate").focus(function () {
        if ($("#txtMonthYear").val() != "") {
            $("#txtMonthYear").val('');
        }
    });
    $("#txtMonthYear,#AttendanceFromDate,#AttendanceToDate").ready(function () {
        $("#txtMonthYear,#AttendanceFromDate,#AttendanceToDate").bind("cut copy paste", function (e) {
            e.preventDefault();
        });
    });
    var Status = ["Registered", "LongLeave", "Others", "Resigned"];
    var StaffCategory = ["Teaching", "Non Teaching-Admin"];
    var rowsToColor = [];
    jQuery(grid_selector).jqGrid({
        url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val() + '&MonthYear=' + $('#txtMonthYear').val() + '&StaffStatus=' + Status + '&StaffType=' + StaffCategory + '&Campus=' + hdnCampus,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Campus', 'Name', 'Staff Id', 'Department', 'Staff Type', 'Group', 'Designation', 'Staff Status', 'Total no.of working Days', 'Total no.of Days Present [Punches + 2] ', 'No Of Days Absent', 'No Of Days Leave Taken', 'Total no.of Hours Worked', 'Opening Balance', 'Closing Balance', 'Show Log Details', 'Edit Attendance', 'Total Available Balance', 'Edit Status'],
        colModel: [
        { name: 'Id', index: 'Id', hidden: true },
        { name: 'Campus', index: 'Campus', hidden: false },
        { name: 'Name', index: 'Name', width: 200 },
        { name: 'IdNumber', index: 'IdNumber' },
        { name: 'Department', index: 'Department', width: 180, hidden: true },
        { name: 'StaffCategoryForAttendane', index: 'StaffCategoryForAttendane', width: 110, hidden: false },
        { name: 'Programme', index: 'Programme', hidden: true, width: 100 },
        { name: 'Designation', index: 'Designation', hidden: false, width: 150 },
        { name: 'NewStatus', index: 'NewStatus', width: 120, hidden: false },
        { name: 'TotalDays', index: 'TotalDays', width: 90, hidden: false },
        { name: 'TotalWorkedDays', index: 'TotalWorkedDays', width: 90, hidden: false },
        { name: 'NoOfDaysLeaveTaken', index: 'NoOfDaysLeaveTaken', width: 90, hidden: false },
        { name: 'LeaveToBeCalculated', index: 'LeaveToBeCalculated', width: 90, hidden: false },
        { name: 'TotalWorkedHours', index: 'TotalWorkedHours', width: 90, hidden: false },
        { name: 'OpeningBalance', index: 'OpeningBalance', width: 80, hidden: false },
        { name: 'ClosingBalance', index: 'ClosingBalance', width: 80, hidden: false },
        { name: 'ShowStaffAttendanceDetails', index: 'ShowStaffAttendanceDetails', width: 80, align: 'center', hidden: false },
        { name: 'Edit', index: 'Edit', align: 'center', width: 80, hidden: true },
        { name: 'TotalAvailableBalance', index: 'TotalAvailableBalance', width: 80, hidden: true },
        { name: 'EditStaffDetailsChange', index: 'EditStaffDetailsChange', align: 'center', width: 120, hidden: true },

        ],
        viewrecords: true,
        altRows: true,
        autowidth: true,
        shrinkToFit: false,
        multiselect: true,
        multiboxonly: true,
        rowNum: 5000,
        rowList: [5000],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: "Desc",
        height: 400,
        gridComplete: function () {
            var rows = $("#StaffConsolidateSummaryNewGridList").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var NewStatus = $("#StaffConsolidateSummaryNewGridList").getCell(rows[i], "NewStatus");
                if (NewStatus == "LongLeave" || NewStatus == "Resigned") {
                    $("#StaffConsolidateSummaryNewGridList").jqGrid('setRowData', rows[i], false, { weightfont: 'bold', background: 'Silver' });
                }
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
                if (Flag == "Show-Finance") {                    
                    $("#StaffConsolidateSummaryNewGridList").hideCol("Edit");
                    $("#StaffConsolidateSummaryNewGridList").hideCol("EditStaffDetailsChange");
                }
                else if (Flag == "Show-HR") {
                    $("#StaffConsolidateSummaryNewGridList").hideCol("Edit");
                    $("#StaffConsolidateSummaryNewGridList").hideCol("EditStaffDetailsChange");
                }
                else if (Flag == "Show-ALL") {
                    ColumnHide();
                }
                else {
                    $("#StaffConsolidateSummaryNewGridList").hideCol("Edit");
                    $("#StaffConsolidateSummaryNewGridList").hideCol("EditStaffDetailsChange");
                }
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Staff In/Out Summary',
    });
    $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size
    //navButtons Add, edit, delete
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
            viewicon: 'ace-icon fa fa-search-plus grey',
        },
        {}, {}, {}, {}, {}
    );
    //AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
    debugger;
    if (Flag == "Show-ALL") {
        $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel (HR)<u/>',
            onClickButton: function () {
                window.open("StaffConsolidateSummaryNewJqGrid" + '?rows=99999' +
                            '&page=1' +
                            '&sidx=Name' +
                            '&sord=desc' +
                            '&Campus=' + $("#ddlCampus").val() +
                            '&StaffType=' + $("#ddlStaffType").val() +
                            '&IdNumber=' + $("#StaffId").val() +
                            '&StaffName=' + $("#StaffName").val() +
                            '&MonthYear=' + $("#txtMonthYear").val() +
                            '&AttendanceFromDate=' + $("#AttendanceFromDate").val() +
                            '&AttendanceToDate=' + $("#AttendanceToDate").val() +
                            '&ExportType=ExcelForHR'
                             );
                $("#thumbnail").append(response + '&nbsp;')
            }
        });
        $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel (Finance)<u/>',
            onClickButton: function () {
                window.open("StaffConsolidateSummaryNewJqGrid" + '?rows=99999' +
                            '&page=1' +
                            '&sidx=Name' +
                            '&sord=desc' +
                            '&Campus=' + $("#ddlCampus").val() +
                            '&StaffType=' + $("#ddlStaffType").val() +
                            '&IdNumber=' + $("#StaffId").val() +
                            '&StaffName=' + $("#StaffName").val() +
                            '&MonthYear=' + $("#txtMonthYear").val() +
                            '&AttendanceFromDate=' + $("#AttendanceFromDate").val() +
                            '&AttendanceToDate=' + $("#AttendanceToDate").val() +
                            '&ExportType=ExcelForFinance'
                             );
                $("#thumbnail").append(response + '&nbsp;')
            }
        });
    }
    else if (Flag == "Show-Finance") {
        $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel (Finance)<u/>',
            onClickButton: function () {
                window.open("StaffConsolidateSummaryNewJqGrid" + '?rows=99999' +
                            '&page=1' +
                            '&sidx=Name' +
                            '&sord=desc' +
                            '&Campus=' + $("#ddlCampus").val() +
                            '&StaffType=' + $("#ddlStaffType").val() +
                            '&IdNumber=' + $("#StaffId").val() +
                            '&StaffName=' + $("#StaffName").val() +
                            '&MonthYear=' + $("#txtMonthYear").val() +
                            '&AttendanceFromDate=' + $("#AttendanceFromDate").val() +
                            '&AttendanceToDate=' + $("#AttendanceToDate").val() +
                            '&ExportType=ExcelForFinance'
                             );
                $("#thumbnail").append(response + '&nbsp;')
            }
        });
    }
    else if (Flag == "Show-HR") {
        debugger;
        $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel (HR)<u/>',
            onClickButton: function () {
                window.open("StaffConsolidateSummaryNewJqGrid" + '?rows=99999' +
                            '&page=1' +
                            '&sidx=Name' +
                            '&sord=desc' +
                            '&Campus=' + $("#ddlCampus").val() +
                            '&StaffType=' + $("#ddlStaffType").val() +
                            '&IdNumber=' + $("#StaffId").val() +
                            '&StaffName=' + $("#StaffName").val() +
                            '&MonthYear=' + $("#txtMonthYear").val() +
                            '&AttendanceFromDate=' + $("#AttendanceFromDate").val() +
                            '&AttendanceToDate=' + $("#AttendanceToDate").val() +
                            '&ExportType=ExcelForHR'
                             );
                $("#thumbnail").append(response + '&nbsp;')
            }
        });
    }
    else {
    }
    //For pager Icons
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }
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
    function checkvalid(value, column) {
        if (value == 'nil' || value == "") {
            return [false, column + ": Field is Required"];
        }
        else {
            return [true];
        }
    }
    //url: '/BioMetricAttendance/StaffInOutSummaryJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val() + '&MonthYear=' + $('#txtMonthYear').val(),
    $("#btnGetSearch").click(function () {
        var MonthYear = $("#txtMonthYear").val();
        if (MonthYear == "") {
            ErrMsg("Please Select MonthYear");
            return false;
        }
        $("#LoadEditButton").show();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            async: false,
            url: '/BioMetricAttendance/CalculateClosingBalanceGenerate',
            data: {
                MonthYear: MonthYear
            },
            success: function (data) {
                if (data == "Success") {
                    SucessMsg("Attendance Generate Sucessfully");
                    reloadGrid();
                    return true;
                }
                if (data == "Failed") {
                    ErrMsg("Attendance cannot be generated or already exist!");
                    return false;
                }
                if (data == "AttGeneratFailed") {
                    ErrMsg("Attendance cannot be generated or already exist!");
                    return false;
                }

            },
            complete: function () {
                $('#LoadEditButton').hide();
            }
        });

    });

    $("#Search").click(function () {
        var AttFromDate = $('#AttendanceFromDate').val();
        var AttToDate = $('#AttendanceToDate').val();
        var AttMonthYear = $('#txtMonthYear').val();
        if ((AttFromDate != null && AttFromDate != "") && (AttToDate == null || AttToDate == "")) {
            ErrMsg("Please fill To Date!");
            return false;
        }
        if ((AttFromDate == null || AttFromDate == "") && (AttToDate != null && AttToDate != "")) {
            ErrMsg("Please fill From Date!");
            return false;
        }
        if ((AttFromDate != null && AttFromDate != "") && (AttToDate != null && AttToDate != "") && (AttMonthYear != null && AttMonthYear != "")) {
            ErrMsg("Please use Date wise search (or) Month Year search!");
            return false;
        }
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
           postData: {
               Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), IdNumber: $("#StaffId").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val(), StaffStatus: $("#Status").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
    $("#Reset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).clearGridData();
        GetCurrentDate();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
           postData: {
               Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), IdNumber: $("#StaffId").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val(), StaffStatus: $("#Status").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
    $('#btnOverView').click(function () {
        var AttFromDate = $('#AttendanceFromDate').val();
        var AttToDate = $('#AttendanceToDate').val();
        var AttMonthYear = $('#txtMonthYear').val();
        $.ajax({
            dataType: 'json',
            url: '/BioMetricAttendance/AttendanceGenerateCheck',
            data: {
                AttendanceToDate: AttToDate, MonthYear: AttMonthYear
            },
            success: function (data) {
                if (data == "Success") {
                    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/ClosingBalanceView?AttendanceFromDate=" + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val() + '&MonthYear=' + $('#txtMonthYear').val(), $('#divClosingBalanceView'),
         function () { LoadSetGridParam($('#divClosingBalanceView')) }, function () { }, 1000, 580, "Closing Balance Details");
                    return true;
                }
                else {
                    ErrMsg("Attendance doesn't exist for selected month!");
                    return false
                }
            }
        });

        //}
    });

});
function getCampus() {
    $.getJSON("/Base/FillCampusName",
function (fillig) {
    var Campusddl = $("#ddlCampus");
    Campusddl.empty();
    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    });
});
}
function GetCurrentDate() {
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
    // alert(today);
    document.getElementById("AttendanceFromDate").value = today;
    document.getElementById("AttendanceToDate").value = today;
}
function ShowStaffAttendanceDetailsNew(IdNumber, AttendanceFromDate, AttendanceToDate, TotalDays) {
    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/ShowStaffAttendanceDetailsNew?IdNumber=" + IdNumber + '&AttendanceFromDate=' + AttendanceFromDate + '&AttendanceToDate=' + AttendanceToDate + '&TotalDays=' + TotalDays, $('#divShowStaffAttendanceDetailsNew'),
          function () { }, function () { }, 1020, 560, "Daily Log Details");
}
function EditStaffAttendanceDetails(PreRegNum, TotalWorkedDays, StaffType, AttendanceToDate, TotalDays, Campus, NoOfLeaveTaken) {
    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/EditStaffBioAttendanceDetails?PreRegNum=" + PreRegNum + '&TotalWorkedDays=' + TotalWorkedDays + '&StaffType=' + StaffType + '&AttendanceToDate=' + AttendanceToDate + '&TotalDays=' + TotalDays + '&Campus=' + Campus + '&NoOfLeaveTaken=' + NoOfLeaveTaken, $('#divEditStaffAttendanceDetails'),
          function () { }, function () { }, 900, 500, "Edit Staff Attendance Details");
}
function EditStaffStatusChange(PreRegNum, Campus, MonthYear, AttendancToDate) {
    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/EditStaffStatusChange?PreRegNum=" + PreRegNum + '&Campus=' + Campus + '&MonthYear=' + MonthYear + '&AttendanceDate=' + AttendancToDate, $('#divStaffStatusChange'),
          function () { }, function () { }, 900, 550, "Staff Details Status Update");
}
$('#btnStatusRpt').click(function () {
    ModifiedLoadPopupDynamicaly("/StaffManagement/StaffStatusReport", $('#divStaffDetailsStatusReport'),
            function () { LoadSetGridParam($('#divStaffDetailsStatusReport')) }, function () { }, 900, 380, "Staff Status Report");
});
function ColumnHide() {
    var AttToDate = $('#AttendanceToDate').val();
    var AttMonthYear = $('#txtMonthYear').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        //traditional: true,
        url: '/BioMetricAttendance/ColumnHide',
        data: {
            AttendanceDate: AttToDate, MonthYear: AttMonthYear
        },
        success: function (data) {
            if (data == "Hide") {
                $("#StaffConsolidateSummaryNewGridList").hideCol("Edit");
                $("#StaffConsolidateSummaryNewGridList").hideCol("EditStaffDetailsChange");
                $("#hdnEmptyValue").val(true);
                return true;
            }
            else {
                $("#StaffConsolidateSummaryNewGridList").showCol("Edit");
                $("#StaffConsolidateSummaryNewGridList").showCol("EditStaffDetailsChange");
                $("#hdnEmptyValue").val(false);
                return false;
            }
        },
    });
    return false;
}
function reloadGrid() {
    $('#StaffConsolidateSummaryNewGridList').setGridParam(
         {
             datatype: "json",
             url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
             postData: {
                 Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), IdNumber: $("#StaffId").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val(), StaffStatus: $("#Status").val()
             },
         }).trigger("reloadGrid");

}