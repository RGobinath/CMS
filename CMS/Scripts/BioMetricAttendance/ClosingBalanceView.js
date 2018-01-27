$(function () {
    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var AttFromDate = $('#AttendanceFromDate').val();
    var AttToDate = $('#AttendanceToDate').val();
    var AttMonthYear = $('#txtMonthYear').val();
    if (AttMonthYear != "") {
        var AttMonthAndYear = AttMonthYear.split('-');
        var Month = monthNames[AttMonthAndYear[0] - 1];
        var Year = AttMonthAndYear[1]
    }
    if (AttToDate != "") {
        var Date = AttToDate.split('/');
        var Month = monthNames[Date[1] - 1];
        var Year = Date[2];
    }
    var grid_selector = "#StaffClosingBalanceJqGridList";
    var pager_selector = "#StaffClosingBalanceJqGridListPager";

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
    var Status = ["Registered", "LongLeave", "Others", "Resigned"];
    var StaffCategory = ["Teaching", "Non Teaching-Admin"];
    jQuery(grid_selector).jqGrid({
        url: '/BioMetricAttendance/StaffClosingBalanceJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val() + '&MonthYear=' + $('#txtMonthYear').val() + '&StaffStatus=' + Status + '&StaffType=' + StaffCategory,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Name', 'Staff Id', 'Total no.of working Days', 'Total no.of Days Present ', 'No Of Days Absent', 'Leave Closing Balance', 'New Status'],
        colModel: [
        { name: 'Id', index: 'Id', hidden: true },
        { name: 'Name', index: 'Name' },
        { name: 'IdNumber', index: 'IdNumber' },
        { name: 'TotalDays', index: 'TotalDays', width: 90, hidden: true },
        { name: 'TotalWorkedDays', index: 'TotalWorkedDays', width: 90, hidden: false },
        { name: 'NoOfDaysLeaveTaken', index: 'NoOfDaysLeaveTaken', width: 90, hidden: false },
        { name: 'ClosingBalance', index: 'ClosingBalance', width: 90, hidden: false },
        { name: 'NewStatus', index: 'NewStatus', width: 90, hidden: true },
        ],
        viewrecords: true,
        altRows: true,
        autowidth: false,
        shrinkToFit: true,
        multiselect: true,
        multiboxonly: true,
        rowNum: 5000,
        rowList: [5000],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: "Desc",
        height: 400,
        width: 970,
        gridComplete: function () {
            debugger;
            var rows = $("#StaffClosingBalanceJqGridList").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var NewStatus = $("#StaffClosingBalanceJqGridList").getCell(rows[i], "NewStatus");
                if (NewStatus == "LongLeave" || NewStatus == "Resigned") {
                    $("#StaffClosingBalanceJqGridList").jqGrid('setRowData', rows[i], false, { weightfont: 'bold', background: 'Silver' });
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
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Staff Attendance Report ' + Month + ' ' + Year,
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
           refresh: true,
           refreshicon: 'ace-icon fa fa-refresh green',
           view: false,
           viewicon: 'ace-icon fa fa-search-plus grey',
       },
       {}, {}, {}, {}, {}
   );
    //AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("StaffClosingBalanceJqGrid" + '?rows=9999' +
                        '&page=1' +
                        '&sidx=Name' +
                        '&sord=desc' +
                        '&MonthYear=' + $("#txtMonthYear").val() +
                        '&AttendanceFromDate=' + $("#AttendanceFromDate").val() +
                        '&AttendanceToDate=' + $("#AttendanceToDate").val() +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }

    });
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