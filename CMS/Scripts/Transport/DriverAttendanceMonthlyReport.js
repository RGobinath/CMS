jQuery(function ($) {

    var grid_selector = "#DriverAttendanceMonthReport";
    var pager_selector = "#DriverAttendanceMonthReportPager";

    var Month = ($('#CurMonth')).val();
    var Year = ($('#CurYear')).val();
    var DriverName;
    var campus;
    var DriverIdNo;
    $("#ddlyear").val(Year);
    $.getJSON("/Base/FillMonth",
         function (fillig) {
             var mior = $("#ddlmonth");
             mior.empty();
             mior.append("<option value=' '> Select </option>");
             $.each(fillig, function (index, itemdata) {
                 if (itemdata.Value != Month) {
                     mior.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text
                     }));
                 }
                 else {
                     mior.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text,
                     selected: "Selected"

                 }));
                 }
             });
         });

    $("#btnSearch").click(function () {
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#FromDate').val() == " ") { ErrMsg("Please fill the FromDate"); return false; }
        if ($('#ToDate').val() == "") { ErrMsg("Please fill the ToDate"); return false; }
        DriverName = $("#txtDriverName").val();
        DriverIdNo = $("#txtDriverNo").val();
        campus = $("#Campus").val();
        fromDate = $("#FromDate").val();
        toDate = $("#ToDate").val();
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: "/Transport/DriverAttendanceMonthlyReportJqGrid",
                postData: { DriverName: DriverName, DriverIdNo: DriverIdNo, Campus: campus, FromDate: fromDate, ToDate: toDate },
                page: 1
            }).trigger("reloadGrid");
    });

    $("#btnReset").click(function () {
      
        $('#Campus').val('Select');
        $('#txtDriverName').val('');
        $('#txtDriverNo').val('');
        $('#ddlmonth').val('Select');
        $('#ddlyear').val('Select');
        LoadSetGridParam($(grid_selector), '/Transport/DriverAttendanceMonthlyReportJqGrid?CurrMonth=' + Month + '&CurrYear=' + Year);
    });

   

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


    //pager icon


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


    $(grid_selector).jqGrid({
        url: '/Transport/DriverAttendanceMonthlyReportJqGrid?CurrMonth=' + Month + '&CurrYear=' + Year,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Driver Name', 'DriverId No', 'From Date', 'To Date', 'No.of Leave', 'No.of Absent', 'Total Absent & Leave', 'No.of Present', 'Total Working days'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true },
            { name: 'Campus', index: 'Campus', align: 'center' },
            { name: 'DriverName', index: 'DriverName', width: '250px', align: 'center', sortable: false },
            { name: 'DriverIdNo', index: 'DriverIdNo', align: 'center' },
            { name: 'FromDate', index: 'FromDate', align: 'center', sortable: false },
            { name: 'ToDate', index: 'ToDate', align: 'center', sortable: false },
            { name: 'Leave', index: 'Leave', align: 'center', sortable: false },
            { name: 'Absent', index: 'Absent', align: 'center', sortable: false },
            { name: 'TotalLandA', index: 'TotalLandA', align: 'center', sortable: false },
            { name: 'noOfPre', index: 'noOfPre', align: 'center', sortable: false },
            { name: 'TotalWorkingDay', index: 'TotalWorkingDay', align: 'center', sortable: false },
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '230',
        autowidth: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Driver Attendance Monthly Report',
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });
    $(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: false });

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            debugger;
            DriverName = $("#txtDriverName").val();
            DriverIdNo = $("#txtDriverNo").val();
            Campus = $("#Campus").val();
            from = $("#FromDate").val();
            to = $("#ToDate").val();
            window.open("DriverAttendanceMonthlyReportJqGrid" + '?DriverName=' + DriverName + '&DriverIdNo=' + DriverIdNo + '&Campus=' + Campus + '&FromDate=' + from + '&ToDate=' + to + ' &rows=9999 ' + '&ExptXl=1');
        }
    });






});

function MonthName(cellvalue, options, rowObject) {

    switch (cellvalue) {
        case '1': return "January";
        case '2': return "February";
        case '3': return 'March';
        case '4': return 'April';
        case '5': return 'May';
        case '6': return 'June';
        case '7': return 'July';
        case '8': return 'August';
        case '9': return 'September';
        case '10': return 'October';
        case '11': return 'November';
        case '12': return 'December';
        default: return "";
    }
}

//   Autocomplete Search Example....
$("#txtDriverName").autocomplete({
    source: function (request, response) {
        var Campus = $("#Campus").val();
        $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
            response(data);
        });
    },
    select: function (event, ui) {
        debugger;
        FillDriverIdNo(ui.item.value);
    },
    minLength: 1,
    delay: 100
});
$("#Campus").change(function () {
    $("#DriverIdNo").val('');
    $.getJSON("/Transport/GetDriverByCampus?Campus=" + $(this).val(),
     function (fillig) {
         var Dri = $("#txtDriverName");
         Dri.empty();
         Dri.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));
         $.each(fillig, function (index, itemdata) {
             Dri.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });

     });
});


function FillDriverIdNo(DriverName) {
    debugger;
    var Campus = $("#Campus").val();
    $.ajax({
        type: 'POST',
        url: "/Transport/GetDriverDetailsByNameAndCampus",
        data: { Campus: Campus, DriverName: DriverName },
        success: function (data) {
            $("#txtDriverNo").val(data.DriverIdNo);
        }
    });
}