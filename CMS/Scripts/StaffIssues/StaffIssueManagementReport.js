function FillPerformerDll() {
    debugger;
    var IssueGroup = $("#ddlIssueGroup").val();
    if (IssueGroup != "") {
        $.getJSON('/StaffIssues/getCallPerformerList?IssueGroup=' + IssueGroup + '&Performer='+"",
          function (fillbc) {
              var ddlbc = $("#ddlPerformer");
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));

              $.each(fillbc, function (index, itemdata) {
                  // ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  if (itemdata.Text == $('#viewbagPerformer').val()) {
                      ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                      //GetIssueType();
                  }
                  else {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  }
              });
              
          });
    }
    else {
        $("#ddlPerformer").empty();
        $("#ddlPerformer").append("<option value=''> Select One </option>");
    }
}
function FillPerformerDllByPerformer() {
    debugger;
    var IssueGroup = $("#ddlIssueGroup").val();
    var Performer = $("#viewbagPerformer").val();
    if (IssueGroup != "") {
        $.getJSON("/StaffIssues/getCallPerformerList?IssueGroup=" + IssueGroup + '&Performer=' + Performer,
          function (fillbc) {
              debugger;
              var ddlbc = $("#ddlPerformer");
              ddlbc.empty();
              //ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        $("#ddlPerformer").empty();
        $("#ddlPerformer").append("<option value=''> Select One </option>");
    }
}
function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}

$(function () {
    var DeptCode = $("#viewbagDeptCode").val();
    var $chrt_border_color = "#efefef";
    var $chrt_grid_color = "#DDD"
    var $chrt_main = "#E24913";
    /* red       */
    var $chrt_second = "#6595b4";
    /* blue      */
    var $chrt_third = "#FF9F01";
    /* orange    */
    var $chrt_fourth = "#7e9d3a";
    /* green     */
    var $chrt_fifth = "#BD362F";
    /* dark red  */
    var $chrt_mono = "#000";

    var $colrNewEnq = "#6595b4";
    var $colrClrnc = "#FF4500";//OrangeRed
    var $colrAprvl = "#708090";//SlateGray 
    var $colrReg = "#808000";//olive
    FillCampusDll();
    if ($("#showexcel").val() == "True") {
        //FillPerformerDll();
        $.getJSON("/StaffIssues/FillStaffIssueGroupByDeptCode?DeptCode=" + DeptCode,
     function (fillig) {
         debugger;
         var ddlig = $("#ddlIssueGroup");
         ddlig.empty();
         if (fillig.length > 1) {
             ddlig.append($('<option/>', { value: "", text: "Select One" }));
         }
         $.each(fillig, function (index, itemdata) {
             //if (itemdata.Text == $('#IssueGroup').val()) {
             //    ddlig.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
             //    GetIssueType();
             //}

             ddlig.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));

         });
         GetIssueType();
         FillPerformerDll();
     });
    }
    else {
        $.getJSON("/StaffIssues/FillStaffIssueGroupByDeptCode?DeptCode=" + DeptCode,
     function (fillig) {
         debugger;
         var ddlig = $("#ddlIssueGroup");
         ddlig.empty();
         if (fillig.length > 1) {
             ddlig.append($('<option/>', { value: "", text: "Select One" }));
         }
         $.each(fillig, function (index, itemdata) {
             //if (itemdata.Text == $('#IssueGroup').val()) {
             //    ddlig.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
             //    GetIssueType();
             //}

             ddlig.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));

         });
         GetIssueType();
         FillPerformerDllByPerformer();
     });
    }
    loadchart();
    var grid_selector = "#StaffIssueManagementReportList";
    var pager_selector = "#StaffIssueManagementReportListPager";
    debugger;
    var IssueGroup1 = $("#viewbagDeptCode").val();
    var Performer1 = $("#viewbagPerformer").val();
    $(grid_selector).jqGrid({
        url: '/StaffIssues/StaffIssueManagementReportListJqGrid?Performer=' + Performer1 + '&IssueGroup=' + IssueGroup1,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'InstanceId', 'ProcessRefId', 'Performer', 'Available', 'Assigned', 'Resolved', 'Total'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true, key: true },
                    { name: 'InstanceId', index: 'InstanceId', hidden: true },
                    { name: 'ProcessRefId', index: 'ProcessRefId', hidden: true },
                    { name: 'Performer', index: 'Performer' },
                    { name: 'Available', index: 'Available', align: 'center' },
                    { name: 'Assigned', index: 'Assigned', align: 'center' },
                    { name: 'Resolved', index: 'Resolved', align: 'center' },
                    { name: 'Total', index: 'Total', align: 'center' }
        ],
        pager: pager_selector,
        rowNum: '100',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1250,
        autowidth: true,
        height: 170,
        viewrecords: true,
        shrinkToFit: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {

                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        //    loadError: function (xhr, status, error) {
        //    $(grid_selector).clearGridData();
        //    ErrMsg($.parseJSON(xhr.responseText).Message);
        //},
        caption: '<i class="fa fa-list"></i> Staff Issue Management Report'
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
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {})
    if ($("#showexcel").val() == "True") {
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
            onClickButton: function () {
                window.open("StaffIssueManagementReportListJqGrid" + '?Performer=' + $("#ddlPerformer").val() + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&DateType=' + $("#ddlDateType").val() + '&DueDateType=' + $("#ddlDueDateType").val() + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999999' + '&ExptXl=1');
            }
        });
    }
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-pdf-o'></i> Export To PDF",
        onClickButton: function () {
            window.open("StaffIssueManagementCountReportPDF" + '?Performer=' + $("#ddlPerformer").val() + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&DateType=' + $("#ddlDateType").val() + '&DueDateType=' + $("#ddlDueDateType").val() + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999999');
        }
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");                
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffIssues/StaffIssueManagementReportListJqGrid',
           postData: { Performer: $("#viewbagPerformer").val(), FromDate: "", ToDate: "", DateType: "", DueDateType: "", Campus: "", IssueGroup: $("#viewbagDeptCode").val(), IssueType: "" },
           page: 1
       }).trigger("reloadGrid");
        if ($("#ddlPerformer").val() == "") {
            $("#StaffIssueMgmtRptChart1").show();
            loadchart();
        }
    });
    //$('.date-picker').datepicker({
    //    format: 'dd/mm/yyyy',
    //    autoclose: true,
    //    endDate:new Date()
    //});
    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);
    $('#txtFromDate').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        //startDate: startDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#txtToDate').datepicker('setStartDate', startDate);
    });
    $('#txtToDate').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#txtFromDate').datepicker('setEndDate', FromEndDate);
    });
    $("#txtToDate").focus(function () {
        if ($("#txtFromDate").val() == "") {
            return ErrMsg("Please Select FromDate");
        }
    });
    $("#ddlIssueGroup").change(function () {
        GetIssueType();
        if ($("#showexcel").val() == "True") {
            FillPerformerDll();
        }
        else {
            FillPerformerDllByPerformer();
        }
    });
    $("#btnSearch").click(function () {
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        var Performer = $("#ddlPerformer").val();
        var DateType = $("#ddlDateType").val();
        var DueDateType = $("#ddlDueDateType").val();

        var IssueGroup = $("#ddlIssueGroup").val();
        var IssueType = $("#ddlIssueType").val();
        var Campus = $("#ddlCampus").val();
        if (DateType != "") {
            if (FromDate == "") {
                return ErrMsg("Please Select From Date");
            }
        }
        if (FromDate != "") {
            if (DateType == "") {
                return ErrMsg("Please Select Date Type");
            }
        }
        if (ToDate != "") {
            if (DateType == "") {
                return ErrMsg("Please Select Date Type");
            }
            if (FromDate == "") {
                return ErrMsg("Please Select From Date");
            }
        }
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffIssues/StaffIssueManagementReportListJqGrid',
           postData: { Performer: Performer, FromDate: FromDate, ToDate: ToDate, DateType: DateType, DueDateType: DueDateType, Campus: Campus, IssueGroup: IssueGroup, IssueType: IssueType },
           page: 1
       }).trigger("reloadGrid");
        if ($("#ddlPerformer").val() != "") {
            $("#StaffIssueMgmtRptChart1").hide();
            loadchart();
        }
        if ($("#ddlPerformer").val() == "") {
            $("#StaffIssueMgmtRptChart1").show();
            loadchart();
        }
    });

});

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
function ShowCallDetails(TotalCount, CountName, Performer) {
    ModifiedLoadPopupDynamicaly("/StaffIssues/ShowCallDetails?Totalcount=" + TotalCount + '&CountName=' + CountName + '&Performer=' + Performer, $('#ShowCallDetails'),
                   function () { }, function () { }, 1200, 500, "Staff Issue Management");
}
function GetIssueType() {
    debugger;
    var value = $('#ddlIssueGroup').val();
    if (value != "") {
        $.ajax({
            type: 'POST',
            async: false,
            url: '/StaffIssues/FillStaffIssueType/?IssueGroup=' + value,
            success: function (data) {
                $("#ddlIssueType").empty();
                $("#ddlIssueType").append("<option value=''> Select One </option>");
                for (var i = 0; i < data.rows.length; i++) {
                    //if (data.rows[i].Text == $('#IssueType').val()) {
                    //    $("#ddlIssueType").append("<option value='" + data.rows[i].Value + "' selected='selected'>" + data.rows[i].Text + "</option>");
                    //} else {
                    $("#ddlIssueType").append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                    //}
                }
            },
            dataType: "json"
        });
    }
    else {
        $("#ddlIssueType").empty();
        $("#ddlIssueType").append("<option value=''> Select One </option>");
    }
}
function loadchart() {
    debugger;
    $.ajax({
        type: 'Get',
        async: false,
        url: '/StaffIssues/GetStaffIssueManagementReportChart?Performer=' + $("#ddlPerformer").val() + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&DueDateType=' + $("#ddlDueDateType").val() + '&DateType=' + $("#ddlDateType").val() + '&Campus=' + $("#ddlCampus").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&IssueType=' + $("#ddlIssueType").val(),
        success: function (ReportCount) {
            var data_pie = [];

            data_pie[0] = {
                label: ReportCount != "" ? "Assigned" : "",
                data: ReportCount != "" ? ReportCount[0].cell[5] : "",
                color: 'red'
            }
            data_pie[1] = {
                label: ReportCount != "" ? "Resolved" : "",
                data: ReportCount != "" ? ReportCount[0].cell[6] : "",
                color: 'green'
            }

            //data_pie[2] = {
            //    label: "Sent For Approval",
            //    data: AdmsnStatus.SentForApproval
            //}
            //data_pie[3] = {
            //    label: "Registered",
            //    data: AdmsnStatus.Registered
            //}

            $.plot($("#StaffIssueMgmtRptChart"), data_pie, {
                series: {
                    pie: {
                        show: true,
                        innerRadius: 0.5,
                        radius: 1,
                        label: {
                            show: true,
                            radius: 2 / 3,
                            formatter: function (label, series) {
                                return '<div style="font-size:11px;text-align:center;padding:4px;color:white;">' + Math.round(series.percent) + '%</div>';
                                //return '<div class="tooltip">Hover over me<span class="tooltiptext">Tooltip text</span></div>';
                            },
                            threshold: 0.1
                        }
                    }
                },
                legend: {
                    show: true,
                    noColumns: 1, // number of colums in legend table
                    labelFormatter: null, // fn: string -> string
                    labelBoxBorderColor: "#000", // border color for the little label boxes
                    container: null, // container (as jQuery object) to put legend in, null means default on top of graph
                    position: "ne", // position of default legend container within plot
                    margin: [5, 10], // distance from grid edge to default legend container within plot
                    backgroundColor: "#efefef", // null means auto-detect
                    backgroundOpacity: 1 // set to 0 to avoid background
                },
                tooltip: true,
                tooltipOpts: {
                    content: '<div></div><span>%y</span>',
                    defaultTheme: false
                },
                grid: {
                    hoverable: true,
                    clickable: true
                },
            });
        }
    });
}
