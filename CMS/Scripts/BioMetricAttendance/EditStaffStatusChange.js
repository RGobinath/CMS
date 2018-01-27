$(function () {
    var PreRegNum = $("#PopUpPreRegNum").val();
    var Campus = $("#PopUpCampus").val();
    var Status = $("#PopUpStatus").val();
    var MonthYear = $("#hdnAttMonthYear").val();
    var AttendanceDate = $("#hdnAttDate").val();
    $('#txtDateOfLongLeaveAndResigned').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,
    });
    $("#txtStatus").change(function () {
        var selected = $("#txtStatus").val();
        if (selected == "LongLeave") {
            var BoldVal = $("#lblId").text("Date Of Long Leave");
            BoldVal.css("font-weight", "Bold");
        }
        else if (selected == "Resigned") {
            var BoldVal = $("#lblId").text("Date Of Resigning Staff");
            BoldVal.css("font-weight", "Bold");
        }
        else if (selected == "Registered") {
            var BoldVal = $("#lblId").text("Date Of Active Staff");
            BoldVal.css("font-weight", "Bold");
        }
        else if (selected == "Rejoining") {
            var BoldVal = $("#lblId").text("Date Of Rejoining Staff");
            BoldVal.css("font-weight", "Bold");
        }
        else {
            var BoldVal = $("#lblId").text("Date Of Others Staff");
            BoldVal.css("font-weight", "Bold");
        }
    });
    var grid_selector = "#StaffAttendanceNewStatusGridListJqGrid";
    var pager_selector = "#StaffAttendanceNewStatusGridListJqGridpager";
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

        url: '/StaffManagement/StaffAttendanceNewStatusGridListJqGrid?PreRegNum=' + PreRegNum,
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['StaffStatus_Id', 'Staff Name', 'IdNumber', 'PreRegNum', 'Staff Status', 'DateOfLongLeave/Resigned', 'ToDateOfLongLeave/Resigned', 'Remarks', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                    { name: 'StaffStatus_Id', index: 'StaffStatus_Id', hidden: true, editable: true, key: true },
                    { name: 'StaffName', index: 'StaffName', sortable: true, hidden: false },
                    { name: 'IdNumber', index: 'IdNumber', sortable: true, hidden: false },
                    { name: 'PreRegNum', index: 'PreRegNum', sortable: true, hidden: true },
                    { name: 'StaffStatus', index: 'StaffStatus', sortable: true, hidden: false },
                    { name: 'DateOfLongLeaveAndResigned', index: 'DateOfLongLeaveAndResigned', sortable: true, hidden: false },
                    { name: 'ToDateOfLongLeaveAndResigned', index: 'ToDateOfLongLeaveAndResigned', sortable: true, hidden: true },
                    { name: 'Remarks', index: 'Remarks', sortable: true, hidden: false },
                    { name: 'CreatedBy', index: 'CreatedBy', sortable: true, hidden: true },
                    { name: 'CreatedDate', index: 'CreatedBy', sortable: true, hidden: true },
                    { name: 'ModifiedBy', index: 'ModifiedBy', sortable: true, hidden: true },
                    { name: 'ModifiedDate', index: 'ModifiedDate', sortable: true, hidden: true },
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'StaffStatus_Id',
        sortorder: 'desc',
        altRows: true,
        autowidth: false,
        width: 875,
        shrinkToFit: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Staff Attendance Status ",
    });

    // $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
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
            {},
            {});
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

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function StaffDetailsStatusChange() {
    var PreRegNum = $("#PopUpPreRegNum").val();
    var Campus = $("#PopUpCampus").val();
    var Status = $("#txtStatus").val();
    var DateOfLongLeaveAndResigned = $("#txtDateOfLongLeaveAndResigned").val();
    var Remarks = $("#Remarks").val();
    var MonthYear = $("#hdnAttMonthYear").val();
    var AttendanceDate = $("#hdnAttDate").val();
    if (Status == '' || DateOfLongLeaveAndResigned == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/BioMetricAttendance/SaveStaffDetailsStatus",
        data: {
            Status: Status, DateOfLongLeaveAndResigned: DateOfLongLeaveAndResigned, Remarks: Remarks, PreRegNum: PreRegNum, MonthYear: MonthYear, AttendanceDate: AttendanceDate,
        },
        async: false,
        success: function (data) {
            if (data == "Updated") {
                $('#divStaffStatusChange').dialog('close');
                SucessMsg("Updated Sucessfully");
                reloadGrid();
                return true;
            }
        }

    });
}
$('#btnExit').click(function () {
    $('#divStaffStatusChange').dialog('close');
});
function reloadGrid() {

    $('#StaffConsolidateSummaryNewGridList').setGridParam(
         {
             datatype: "json",
             url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
             postData: {
                 Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), IdNumber: $("#StaffId").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val()
             },
         }).trigger("reloadGrid");

}