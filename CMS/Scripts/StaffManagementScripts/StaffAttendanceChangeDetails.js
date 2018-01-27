$(function ($) {
    var grid_selector = "#StaffAttendanceChangeDetailsGridListJqGrid";
    var pager_selector = "#StaffAttendanceChangeDetailsGridListJqGrid";

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
    jQuery(grid_selector).jqGrid({
        url: '/BioMetricAttendance/StaffAttendanceChangeDetailsGridListJqGrid',
        datatype: 'json',
        type: 'GET',
        colNames: ['Staff_AttendanceChangeDetails_Id', 'PreRegNum', 'Month', 'Year', 'AllowedCashualLeaves', 'TotalNoOfDaysWorkedByLogs', 'TotalNoOfDaysWorkedByChange', 'TotalNoOfLeavesTakenByLogs', 'TotalNoOfLeavesTakenByChange', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate', 'IsActive', 'OnDuty', 'NoOfPermissionsTaken', 'NoOfLeavesCalculatedByPermissions'],
        colModel: [
        { name: 'Staff_AttendanceChangeDetails_Id', index: 'Staff_AttendanceChangeDetails_Id', hidden: true },
        { name: 'PreRegNum', index: 'PreRegNum', hidden: false, width: 130 },
        { name: 'Month', index: 'Month', width: 200 },
        { name: 'Year', index: 'Year' },
        { name: 'AllowedCashualLeaves', index: 'AllowedCashualLeaves', width: 180, hidden: true },
        { name: 'TotalNoOfDaysWorkedByLogs', index: 'TotalNoOfDaysWorkedByLogs', width: 180, hidden: true },
        { name: 'TotalNoOfDaysWorkedByChange', index: 'TotalNoOfDaysWorkedByChange', width: 110, hidden: false },
        { name: 'TotalNoOfLeavesTakenByLogs', index: 'TotalNoOfLeavesTakenByLogs', hidden: false, width: 100 },
        { name: 'TotalNoOfLeavesTakenByChange', index: 'Designation', hidden: false, width: 150 },
        { name: 'CreatedBy', index: 'CreatedBy', width: 80, hidden: false },
        { name: 'CreatedDate', index: 'CreatedDate', width: 80, hidden: false },
        { name: 'ModifiedBy', index: 'ModifiedBy', width: 80, hidden: false },
        { name: 'ModifiedDate', index: 'ModifiedDate', align: 'center', width: 80 },
        { name: 'IsActive', index: 'IsActive', width: 180, hidden: true },
        { name: 'OnDuty', index: 'OnDuty', width: 180, hidden: true },
        { name: 'NoOfPermissionsTaken', index: 'NoOfPermissionsTaken', width: 180, hidden: true },
        { name: 'NoOfLeavesCalculatedByPermissions', index: 'NoOfLeavesCalculatedByPermissions', width: 180, hidden: true },
        { name: 'NoOfHolidays', index: 'NoOfHolidays', width: 180, hidden: true },
        { name: 'CasualLeavesTaken', index: 'CasualLeavesTaken', width: 180, hidden: true },
        { name: 'RemainingCasualLeaves', index: 'RemainingCasualLeaves', width: 180, hidden: true }
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
        sortname: 'Staff_AttendanceChangeDetails_Id',
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
    //alert(NoOfDaysLeaveTaken);

});
