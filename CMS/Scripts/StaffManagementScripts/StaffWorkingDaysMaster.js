jQuery(function ($) {
    var grid_selector = "#StaffWorkingDaysMasterGridList";
    var pager_selector = "#StaffWorkingDaysMasterGridListPager";
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
    getYearDdl();
    Monthddl();
    function getCampusMulti() {
        debugger;
        var spec;
        $.ajax(
            {
                url: "/Base/FillAllBranchCode",
                async: false,
                success: function (data, result) {
                    debugger;
                    if (data != "" && data != null) {
                        var tempData = '';
                        for (var index = 0; index < data.length; index++) {
                            tempData += data[index].Value + ':' + data[index].Text;
                            if (index != data.length - 1)
                                tempData += ';';
                        }
                        spec = tempData;
                    }
                    else {
                        spec = '';
                    }
                }
            });
        return spec;
    }
    function getMonth() {
        debugger;
        var spec;
        $.ajax(
            {
                url: "/StaffManagement/FillMonthDdl",
                async: false,
                success: function (data, result) {
                    debugger;
                    if (data != "" && data != null) {
                        var tempData = '';
                        for (var index = 0; index < data.length; index++) {
                            tempData += data[index].Value + ':' + data[index].Text;
                            if (index != data.length - 1)
                                tempData += ';';
                        }
                        spec = tempData;
                    }
                    else {
                        spec = '';
                    }
                }
            });
        return spec;
    }
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/StaffWorkingDaysMasterGridListJqGrid',
        datatype: 'json',
        type: 'GET',
        colNames: ['Staff_WorkingDaysMaster_Id', 'Campus', 'Staff Type', 'Month', 'Year', 'Total No. Of Working Days (per month)', 'Created By', 'Created Date', ' Modified By', 'Modified Date'],
        colModel: [
                { name: 'Staff_WorkingDaysMaster_Id', index: 'Staff_WorkingDaysMaster_Id', hidden: true, key: true, editable: true },
                {name: 'Campus', index: 'Campus', editable: true, hidden: true, width: 180},
                {name: 'StaffType', index: 'StaffType', editable: true, width: 180, hidden: false},
                {name: 'Month', index: 'Month', editable: true, width: 180, hidden: false, sortable: true},
                {name: 'Year', index: 'Year', editable: true, width: 180, sortable: true},       
                {name: 'NoOfworkingDays', index: 'NoOfworkingDays', editable: true, width: 180},
                { name: 'CreatedBy', index: 'CreatedBy', width: 110, hidden: true },
                { name: 'CreatedDate', index: 'CreatedDate', hidden: true, width: 100 },
                { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true, width: 150 },
                { name: 'ModifiedDate', index: 'ModifiedDate', width: 90, hidden: true }
        ],
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        pager: pager_selector,
        sortname: 'Staff_WorkingDaysMaster_Id',
        sortorder: "Desc",
        height: 300,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Staff Working Days Master',
    });

    $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size
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
        {
            url: '/StaffManagement/SaveOrUpdateStaffWorkingDaysMaster'
                , closeOnEscape: true
                , dataheight: 308, width: 930, height: 400, top: 250, left: 250
                , beforeShowForm: function (frm)
                { }
                , width: 'auto'
        }, {
            url: '/StaffManagement/SaveOrUpdateStaffWorkingDaysMaster'
                , closeOnEscape: true
                , dataheight: 308, width: 930, height: 400, top: 250, left: 250
                , beforeShowForm: function (frm)
                { }
                , width: 'auto'
        }, {
            url: '/StaffManagement/DeleteStaff_WorkingDaysMaster', closeOnEscape: true, beforeShowForm: function (frm)
            { selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows } }
        }, {}, {}
    );
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
    $("#btnSearch").click(function () {
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffWorkingDaysMasterGridListJqGrid',
           postData: {
               Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), Month: $("#ddlMonth").val(), Year: $("#ddlYear").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffWorkingDaysMasterGridListJqGrid',
           postData: {
               Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), Month: $("#ddlMonth").val(), Year: $("#ddlYear").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
});
function getCampus() {
    $.getJSON("/Base/FillAllBranchCode",
function (fillig) {
    var Campusddl = $("#ddlCampus");
    Campusddl.empty();
    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    });
});
}
function Monthddl() {
    $.getJSON("/StaffManagement/FillMonthDdl",
function (fillig) {
    var Campusddl = $("#ddlMonth");
    Campusddl.empty();
    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    });
});
}
function getYearDdl() {
    $.getJSON("/StaffManagement/FillYearDdl",
function (fillig) {
    var Campusddl = $("#ddlYear");
    Campusddl.empty();
    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    });
});
}