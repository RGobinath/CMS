﻿jQuery(function () {

    //var lastDateofTheMonth = new Date(2017, 6, 0)
    //alert(lastDateofTheMonth);
    //var t = lastDateofTheMonth.getMonth("MM");

    //var firstDay = new t(y, m, 1);
    //alert(t);
    //alert(firstDay);

    var grid_selector = "#KioskEnquiryGridList";
    var pager_selector = "#KioskEnquiryGridListPager";

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
    //getCampus();
    //$('#ddlcampus').change(function () {
    //    ChangeDepartmentByCampus();
    //});
    //GetCurrentDate();

    var startDate = new Date('01/01/1947');
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);
    $('.AttFromdate').click(function () {
        $('#Enquirydatefrom').datepicker({
            weekStart: 1,
            startDate: '01/01/1947',
            format: "dd/mm/yyyy",
            todayHighlight: true,
            endDate: FromEndDate,
            autoclose: true
        });
        $('#Enquirydatefrom').datepicker('show').off('focus');
    });
    $('.AttTodate').click(function () {
        $('#Enquirytodate').datepicker({
            weekStart: 1,
            format: "dd/mm/yyyy",
            todayHighlight: true,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        });
        $('#Enquirytodate').datepicker('show').off('focus');
        });
        //$("#txtMonthYear").datepicker({
        //    format: "mm-yyyy",
        //    viewMode: "months",
        //    minViewMode: "months",
        //    autoclose: true
        //});
        jQuery(grid_selector).jqGrid({
            url: '/Admission/GetKioskEnquiryList',
            datatype: 'json',
            type: 'GET',
            colNames: ['Enq_Id', 'Enq_Number', 'StudentName', 'AcademicYear', 'Grade', 'Campus', 'DOB', 'EmailId', 'FatherName', 'MotherName ', 'Gender', 'Phone', 'Address', 'Locality'],
            colModel: [
            { name: 'Enq_Id', index: 'Enq_Id', hidden: true },
            { name: 'Enq_Number', index: 'Enq_Number', hidden: false },
            { name: 'StudentName', index: 'StudentName', width: 200 },
            { name: 'AcademicYear', index: 'AcademicYear' },
            { name: 'Grade', index: 'Grade' },
            { name: 'Campus', index: 'Campus', hidden: true },
            { name: 'DOB', index: 'DOB', hidden: false },
            { name: 'EmailId', index: 'EmailId', hidden: false },
            { name: 'FatherName', index: 'FatherName', width: 90, hidden: false },
            { name: 'MotherName', index: 'MotherName', width: 90, hidden: false },
            { name: 'Gender', index: 'Gender', width: 90, hidden: false },
            { name: 'Phone', index: 'Phone', width: 90, hidden: false },
            { name: 'Address', index: 'Address', width: 80, align: 'center', hidden: false },
            { name: 'Locality', index: 'Locality', width: 80, hidden: true },
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
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;EnquiryDetails',

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
        $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
            onClickButton: function () {
                window.open("StaffConsolidateSummaryJqGrid" + '?rows=9999' +
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
        //url: '/BioMetricAttendance/StaffInOutSummaryJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val() + '&MonthYear=' + $('#txtMonthYear').val(),
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
            var AttendanceRpt = "Rept";
            //if ((AttFromDate != null && AttFromDate != "") && (AttToDate != null || AttToDate != ""))
            //    ErrMsg("Please fill both From Date and To Date!");

          

        });
        $("#Reset").click(function () {
            $("input[type=text], textarea, select").val("");
            $(grid_selector).clearGridData();
            GetCurrentDate();
            jQuery(grid_selector).setGridParam(
           {
               datatype: "json",
               url: '/BioMetricAttendance/StaffConsolidateSummaryJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
               postData: {
                   Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), IdNumber: $("#StaffId").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val(), AttendanceRept: AttendanceRpt
               },
               page: 1

           }).trigger("reloadGrid");
        });

    });
    //function getCampus() {
    //    $.getJSON("/Base/FillAllBranchCode",
    //function (fillig) {
    //    var Campusddl = $("#ddlCampus");
    //    Campusddl.empty();
    //    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    //    $.each(fillig, function (index, itemdata) {
    //        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    //    });
    //});
    //}
    //function GetCurrentDate() {
    //    var today = new Date();
    //    var dd = today.getDate();
    //    var mm = today.getMonth() + 1; //January is 0!

    //    var yyyy = today.getFullYear();
    //    if (dd < 10) {
    //        dd = '0' + dd
    //    }
    //    if (mm < 10) {
    //        mm = '0' + mm
    //    }
    //    var today = dd + '/' + mm + '/' + yyyy;
    //    // alert(today);
    //    document.getElementById("AttendanceFromDate").value = today;
    //    document.getElementById("AttendanceToDate").value = today;
    //}
    //function ShowStaffAttendanceDetails(IdNumber, AttendanceFromDate, AttendanceToDate, TotalDays) {
    //    debugger;
    //    //alert(PreRegNum+AttendanceFromDate+ AttendanceToDate);
    //    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/ShowStaffAttendanceDetails?IdNumber=" + IdNumber + '&AttendanceFromDate=' + AttendanceFromDate + '&AttendanceToDate=' + AttendanceToDate + '&TotalDays=' + TotalDays, $('#divShowStaffAttendanceDetails'),
    //          function () { }, function () { }, 1020, 560, "Daily Log Details");
    //}
    //function EditStaffAttendanceDetails(PreRegNum, TotalWorkedDays, StaffType, AttendanceToDate, TotalDays) {
    //    debugger;
    //    ModifiedLoadPopupDynamicaly("/BioMetricAttendance/EditStaffBioAttendanceDetails?PreRegNum=" + PreRegNum + '&TotalWorkedDays=' + TotalWorkedDays + '&StaffType=' + StaffType + '&AttendanceToDate=' + AttendanceToDate + '&TotalDays=' + TotalDays, $('#divEditStaffAttendanceDetails'),
    //          function () { }, function () { }, 850, 300, "Edit Staff Attendance Details");
    //}