jQuery(function () {
    var PopUpIdNumber = $('#PopUpIdNumber').val();
    var PopUpAttendanceFromDate = $('#PopUpAttendanceFromDate').val();
    var PopUpAttendanceToDate = $('#PopUpAttendanceToDate').val();
    var MonthYear = $('#txtMonthYear').val();
   // alert(MonthYear);
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#StaffDailyAttendanceIOSummaryNewGrid").jqGrid('setGridWidth', $(".page-content").width() - 290);
    });
    //resize on sidebar collapse/expand
    var parent_column = $("#StaffDailyAttendanceIOSummaryNewGrid").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#StaffDailyAttendanceIOSummaryNewGrid").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    });
    jQuery("#StaffDailyAttendanceIOSummaryNewGrid").jqGrid({
        url: '/BioMetricAttendance/StaffDailyAttendanceIOSummaryNewJqGrid?AttendanceFromDate=' + PopUpAttendanceFromDate + '&AttendanceToDate=' + PopUpAttendanceToDate + '&IdNumber=' + PopUpIdNumber,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Campus', 'Name', 'Staff Id', 'Attendance Date', 'Department', 'Designation', 'Group', 'Log In Time', 'Log Out Time', 'Working Hours','History'],
        colModel: [
        { name: 'Id', index: 'Id', hidden: true },
        { name: 'Campus', index: 'Campus', hidden: true },
        { name: 'Name', index: 'Name', hidden: true },
        { name: 'IdNumber', index: 'IdNumber', hidden: true },
        { name: 'AttendanceDate', index: 'AttendanceDate', hidden: false },
        { name: 'Department', index: 'Department', hidden: true },
        { name: 'Designation', index: 'Designation', hidden: true },
        { name: 'Programme', index: 'Programme', hidden: true },
        { name: 'LogInTime', index: 'LogInTime', hidden: false },
        { name: 'LogOutTime', index: 'LogOutTime', hidden: false },
        { name: 'WorkingHours', index: 'WorkingHours', hidden: false },
        { name: 'ShowDetails', index: 'ShowDetails', align:'center',hidden: false }
        ],
        viewrecords: true,
        altRows: true,
        autowidth: false,
        shrinkToFit: true,
        multiselect: true,
        multiboxonly: true,
        rowNum: 5000,
        rowList: [5000],
        pager: "#StaffDailyAttendanceIOSummaryNewGridPager",
        sortname: 'Id',
        sortorder: "Desc",
        height: 400,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Daily IO Summary',
    });

    $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size

    //navButtons Add, edit, delete

    jQuery("#StaffDailyAttendanceIOSummaryGrid").jqGrid('navGrid', "#StaffDailyAttendanceIOSummaryNewGridPager",
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
    $("#StaffDailyAttendanceIOSummaryNewGrid").jqGrid('navButtonAdd', "#StaffDailyAttendanceIOSummaryNewGridPager", {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("StaffDailyAttendanceIOSummaryNewJqGrid" + '?rows=9999' +
                        '&page=1' +
                        '&sidx=Name' +
                        '&sord=desc' +
                        '&Campus=' + $("#ddlCampus").val() +
                        '&StaffType=' + $("#ddlStaffType").val() +
                        '&IdNumber=' + $("#PopUpIdNumber").val() +
                        '&StaffName=' + $("#StaffName").val() +
                        '&MonthYear=' + $("#txtMonthYear").val() +
                        '&AttendanceFromDate=' + $("#PopUpAttendanceFromDate").val() +
                        '&AttendanceToDate=' + $("#PopUpAttendanceToDate").val() +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }

    });
    //$("#StaffDailyAttendanceIOSummaryGrid").jqGrid('navButtonAdd', "#StaffDailyAttendanceIOSummaryGridPager", {
    //    caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
    //    onClickButton: function () {
    //        window.open("StaffConsolidateSummaryJqGrid" + '?rows=9999' +
    //                    '&page=1' +
    //                    '&sidx=Name' +
    //                    '&sord=desc' +
    //                    '&Campus=' + $("#ddlCampus").val() +
    //                    '&StaffType=' + $("#ddlStaffType").val() +
    //                    '&IdNumber=' + $("#StaffId").val() +
    //                    '&StaffName=' + $("#StaffName").val() +
    //                    '&MonthYear=' + $("#txtMonthYear").val() +
    //                    '&AttendanceFromDate=' + $("#AttendanceFromDate").val() +
    //                    '&AttendanceToDate=' + $("#AttendanceToDate").val() +
    //                    '&ExportType=Excel'
    //                     );
    //    }

    //});
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
});
function ShowStaffAttendanceInOutStatusNew(IdNumber, AttendanceFromDate, AttendanceToDate,AttendanceDate) {
    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/ShowStaffAttendanceInOutStatusNew?IdNumber=" + IdNumber + '&AttendanceFromDate=' + AttendanceFromDate + '&AttendanceToDate=' + AttendanceToDate + '&AttendanceDate=' + AttendanceDate, $('#PopUpStaffAttendanceInOutDetails'),
          function () { }, function () { }, 1020, 560, "Daily Log In/Out Details");
}