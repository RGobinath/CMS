$(function () {
    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var AttMonthYear = $('#txtMonthYear').val();

    var grid_selector = "#ReportList";
    var pager_selector = "#ReportListPager";
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
    $("#txtMonthYear").datepicker({
        format: "mm-yyyy",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    });
    var Status = ["Registered", "LongLeave", "Others", "Resigned"];
    var StaffCategory = ["Teaching", "Non Teaching-Admin"];
    jQuery(grid_selector).jqGrid({
        url: '/BioMetricAttendance/StaffAttendanceRegisterReportGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val() + '&MonthYear=' + $('#txtMonthYear').val() + '&StaffStatus=' + Status + '&StaffType=' + StaffCategory,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Name', 'Staff Id Number', '29', '30', '31', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', 'Total no.of Days Present [Punches + 2]', 'No Of Days Absent'],
        colModel: [
                   { name: 'Id', index: 'Id', hidden: true },
                   { name: 'Name', index: 'Name', width: 100 },
                   { name: 'IdNumber', index: 'IdNumber', width: 100 },
                   { name: 'Date29', index: 'Date29', align: 'center', sortable: false, width: 50 },
                   { name: 'Date30', index: 'Date30', align: 'center', sortable: false, width: 50 },
                   { name: 'Date31', index: 'Date31', align: 'center', sortable: false, width: 50 },
                   { name: 'Date1', index: 'Date1', align: 'center', sortable: false, width: 50 },
                   { name: 'Date2', index: 'Date2', align: 'center', sortable: false, width: 50 },
                   { name: 'Date3', index: 'Date3', align: 'center', sortable: false, width: 50 },
                   { name: 'Date4', index: 'Date4', align: 'center', sortable: false, width: 50 },
                   { name: 'Date5', index: 'Date5', align: 'center', sortable: false, width: 50 },
                   { name: 'Date6', index: 'Date6', align: 'center', sortable: false, width: 50 },
                   { name: 'Date7', index: 'Date7', align: 'center', sortable: false, width: 50 },
                   { name: 'Date8', index: 'Date8', align: 'center', sortable: false, width: 50 },
                   { name: 'Date9', index: 'Date9', align: 'center', sortable: false, width: 50 },
                   { name: 'Date10', index: 'Date10', align: 'center', sortable: false, width: 50 },
                   { name: 'Date11', index: 'Date11', align: 'center', sortable: false, width: 50 },
                   { name: 'Date12', index: 'Date12', align: 'center', sortable: false, width: 50 },
                   { name: 'Date13', index: 'Date13', align: 'center', sortable: false, width: 50 },
                   { name: 'Date14', index: 'Date14', align: 'center', sortable: false, width: 50 },
                   { name: 'Date15', index: 'Date15', align: 'center', sortable: false, width: 50 },
                   { name: 'Date16', index: 'Date16', align: 'center', sortable: false, width: 50 },
                   { name: 'Date17', index: 'Date17', align: 'center', sortable: false, width: 50 },
                   { name: 'Date18', index: 'Date18', align: 'center', sortable: false, width: 50 },
                   { name: 'Date19', index: 'Date19', align: 'center', sortable: false, width: 50 },
                   { name: 'Date20', index: 'Date20', align: 'center', sortable: false, width: 50 },
                   { name: 'Date21', index: 'Date21', align: 'center', sortable: false, width: 50 },
                   { name: 'Date22', index: 'Date22', align: 'center', sortable: false, width: 50 },
                   { name: 'Date23', index: 'Date23', align: 'center', sortable: false, width: 50 },
                   { name: 'Date24', index: 'Date24', align: 'center', sortable: false, width: 50 },
                   { name: 'Date25', index: 'Date25', align: 'center', sortable: false, width: 50 },
                   { name: 'Date26', index: 'Date26', align: 'center', sortable: false, width: 50 },
                   { name: 'Date27', index: 'Date27', align: 'center', sortable: false, width: 50 },
                   { name: 'Date28', index: 'Date28', align: 'center', sortable: false, width: 50 },
                   { name: 'TotalWorkedDays', index: 'TotalWorkedDays', align: 'center', sortable: false, width: 90 },
                   { name: 'NoOfDaysLeaveTaken', index: 'NoOfDaysLeaveTaken', align: 'center', sortable: false, width: 90 },

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
        width: 970,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);

            }, 0);
        },
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Staff Attendance Report',
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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
                viewicon: 'ace-icon fa fa-search-plus grey'

            },
            {},
            {}, {}, {})
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            var ExportType = "Export To Excel";
            window.open("StaffAttendanceRegisterReportGrid" + '?rows=9999' +
                        '&page=1' +
                        '&sidx=Name' +
                        '&sord=desc' +
                        '&Campus=' + $('#ddlCampus').val() +
                        '&MonthYear=' + $('#txtMonthYear').val() +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }
    });
    //Pager icons
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
        //$("#thumbnail").append(response + '&nbsp;')
    }
    $("#Search").click(function () {
        var AttMonthYear = $('#txtMonthYear').val();
        var Campus = $('#ddlCampus').val();
        if (AttMonthYear == "") {
            ErrMsg("Please fill month year!");
            return false;
        }
        if (Campus == "") {
            ErrMsg("Please fill campus");
            return false;
        }
        if (AttMonthYear != "") {
            var AttMonthAndYear = AttMonthYear.split('-');
            var Month = AttMonthAndYear[0] - 1;
            var PreMonth = monthNames[Month - 1];
            var Year = AttMonthAndYear[1];
            var LeapYear = Year % 4;
            if (PreMonth == "February") {
                if (LeapYear == 0) {
                    $("#ReportList").hideCol("Date30");
                    $("#ReportList").hideCol("Date31");
                }
                else {
                    $("#ReportList").hideCol("Date29");
                    $("#ReportList").hideCol("Date30");
                    $("#ReportList").hideCol("Date31");
                }
            }
            if (PreMonth == "April") {
                $("#ReportList").hideCol("Date31");
            }
            else if (PreMonth == "June") {
                $("#ReportList").hideCol("Date31");
            }
            else if (PreMonth == "September") {
                $("#ReportList").hideCol("Date31");
            }
            else if (PreMonth == "November") {
                $("#ReportList").hideCol("Date31");
            }
        }
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/BioMetricAttendance/StaffAttendanceRegisterReportGrid',
           postData: {
               Campus: $("#ddlCampus").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
    $("#Reset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).clearGridData();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid',
           postData: {
               Campus: $("#ddlCampus").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
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