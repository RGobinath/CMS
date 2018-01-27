jQuery(function () {

    var grid_selector = "#StaffSummaryRptList";
    var pager_selector = "#StaffSummaryRptListPager";

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
    $('#ddlcampus').change(function () {
        ChangeDepartmentByCampus();
    });
    GetCurrentDate();
   
    var startDate = new Date('01/01/1947');
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);
    $('.AttFromdate').click(function () {
        $('#AttendanceFromDate').datepicker({
            weekStart: 1,
            startDate: '01/01/1947',
            format: "dd/mm/yyyy",
            todayHighlight: true,
            endDate: FromEndDate,
            autoclose: true
        });
        $('#AttendanceFromDate').datepicker('show').off('focus');
    });
    $('.AttTodate').click(function () {
        $('#AttendanceToDate').datepicker({
            weekStart: 1,
            format: "dd/mm/yyyy",
            todayHighlight: true,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        });
        $('#AttendanceToDate').datepicker('show').off('focus');
    });

    jQuery(grid_selector).jqGrid({
        url: '/BioMetricAttendance/GetStaffInOutSummaryJqGrid?date=' + $("#AttendanceFromDate").val() ,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Campus', 'Name', 'Staff Id', 'Designation','','','','','','Attendance Date','InTime','OutTime'],
        colModel: [
        { name: 'Id', index: 'Id', hidden: true },
        { name: 'Campus', index: 'Campus'  },
        { name: 'Name', index: 'Name' },
        { name: 'IdNumber', index: 'IdNumber'},
        { name: 'Designation', index: 'Designation'},
        { name: 'EmployeeID', index: 'EmployeeID',hidden:true },
        { name: 'EmployeeName', index: 'EmployeeName', width: 100,hidden:true },
        { name: 'EmployeeIdNumber', index: 'EmployeeIdNumber', width: 90, hidden: true },
        { name: 'DeviceId', index: 'DeviceId', width: 90, hidden: true },
        { name: 'UserId', index: 'UserId', width: 100, hidden: true },
        { name: 'LogDate', index: 'LogDate' },
        { name: 'InTime', index: 'InTime' },
        { name: 'OutTime', index: 'OutTime' },
        ],
        viewrecords: true,
        altRows: true,
        autowidth: true,
        shrinkToFit: false,
        multiselect: true,
        multiboxonly: true,
        rowNum: 30,
        rowList: [30, 50, 100, 250, 500],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: "Desc",
        height: 650,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Attendance InOut Summary',
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

    //$(grid_selector).jqGrid('navButtonAdd', pager_selector, {
    //    caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
    //    onClickButton: function () {
    //        window.open("GetStaffInOutSummaryJqGrid" + '?rows=9999' +
    //                    '&page=1' +
    //                    '&sidx=Name' +
    //                    '&sord=desc' +
    //                    '&Campus=' + document.getElementById('ddlCampus').value +
    //                    '&StaffId=' + document.getElementById('StaffId').value +
    //                    '&Name=' + document.getElementById('StaffName').value +
    //                    '&ReportDate=' + document.getElementById('ReportDate').value +
    //                    '&Type=Excel'
    //                     );
    //        $("#thumbnail").append(response + '&nbsp;')
    //    }

    //});

   function checkvalid(value, column) {
        if (value == 'nil' || value == "") {
            return [false, column + ": Field is Required"];
        }
        else {
            return [true];
        }
   }
   $("#Search").click(function () {
       $(grid_selector).clearGridData();
       jQuery(grid_selector).setGridParam(
      {
          datatype: "json",
          url: '/BioMetricAttendance/GetStaffInOutSummaryJqGrid?date=' + $("#AttendanceFromDate").val(),
          postData: {
              campus: $("#ddlcampus").val(), staffName: $("#StaffName").val(), staffId: $("#StaffId").val()
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
          url: '/BioMetricAttendance/GetStaffInOutSummaryJqGrid?date=' + $("#AttendanceFromDate").val(),
          postData: {
              campus: $("#ddlcampus").val(), staffName: $("#StaffName").val(), staffId: $("#StaffId").val()
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
    document.getElementById("AttendanceFromDate").value = today;
    //document.getElementById("AttendanceToDate").value = today;
}