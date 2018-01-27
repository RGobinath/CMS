var sord = "";
var sidx = "";
function FillPerformerDll() {
    var ddlbc = $("#ddlPerformer");
    if ($("#ddlCampus").val() != "") {        
        $.getJSON("/Home/getCallManagementPerformerList?Campus=" + $("#ddlCampus").val(),
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
$(function () {
    //FillPerformerDll();
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
    //loadchart();
    var grid_selector = "#PerformerWiseIssueCountReportList";
    var pager_selector = "#PerformerWiseIssueCountReportListPager";
    $(grid_selector).jqGrid({
        url: '/Home/GetPerformerWiseIssueCountReportJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Performer', 'Assigned', 'Resolved', 'Completed', 'Rejected'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true, key: true },
                    { name: 'BranchCode', index: 'BranchCode' },
                    { name: 'Performer', index: 'Performer' },
                    { name: 'Assigned', index: 'Assigned', align: 'center' },
                    { name: 'Resolved', index: 'Resolved', align: 'center' },
                    { name: 'Completed', index: 'Completed', align: 'center' },
                    { name: 'Rejected', index: 'Rejected', align: 'center' }
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
        caption: '<i class="fa fa-list"></i> Performer Wise Issue Count Report'
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
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            sidx = jQuery(grid_selector).jqGrid('getGridParam', 'sortname');
            sord = $(grid_selector).jqGrid('getGridParam', 'sortorder');
            window.open("GetPerformerWiseIssueCountReportJqGrid" + '?Performer=' + $("#ddlPerformer").val() + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&BranchCode=' + $("#ddlCampus").val() + '&rows=9999999' + '&ExprtToExcel=Excel' + '&sidx=' + sidx + '&sord=' + sord);
        }
    });    
    $("#btnReset").click(function () {
        //$("input[type=text], textarea, select").val("");
        $("#txtFromDate").val('');
        $("#txtToDate").val('');
        $("#ddlPerformer").val('');
        $("#ddlCampus").val('');
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Home/GetPerformerWiseIssueCountReportJqGrid',
           postData: { Performer: "", FromDate: "", ToDate: "", BranchCode: "" },
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
    $("#btnSearch").click(function () {
        debugger;
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        var Performer = $("#ddlPerformer").val();
        var Campus = $("#ddlCampus").val();
        if (ToDate != "") {
            if (FromDate == "") {
                return ErrMsg("Please Select From Date");
            }
        }
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Home/GetPerformerWiseIssueCountReportJqGrid',
           postData: { Performer: Performer, FromDate: FromDate, ToDate: ToDate, BranchCode: Campus },
           page: 1
       }).trigger("reloadGrid");
        if ($("#ddlPerformer").val() != "" && $("#ddlCampus").val != "") {
            $("#PerformerWiseIssueCountChart1").hide();

            sidx = jQuery(grid_selector).jqGrid('getGridParam', 'sortname');
            sord = $(grid_selector).jqGrid('getGridParam', 'sortorder');
            loadchart();
        }
        if ($("#ddlPerformer").val() == "" && $("#ddlCampus").val != "") {
            sidx = jQuery(grid_selector).jqGrid('getGridParam', 'sortname');
            sord = $(grid_selector).jqGrid('getGridParam', 'sortorder');
            $("#PerformerWiseIssueCountChart1").show();            
            loadchart();
        }        
    });
$("#ddlCampus").change(function () {
    FillPerformerDll();
});
$.getJSON("/Base/FillAllBranchCode",
  function (fillbc) {
      var ddlbc = $("#ddlCampus");
      ddlbc.empty();
      ddlbc.append($('<option/>', { value: "", text: "Select" }));

      $.each(fillbc, function (index, itemdata) {
          ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
      });
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
function loadchart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/Home/GetPerformerWiseIssueCountReportJqGrid?Performer=' + $("#ddlPerformer").val() + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&BranchCode=' + $("#ddlCampus").val() + '&rows=9999' + '&sidx=' + sidx + '&sord=' + sord,
        success: function (data) {
            debugger;            
            var data_pie = [];
            if (data.rows.length > 0) {
                data_pie[0] = {
                    label: data != "" ? "Assigned" : "",
                    data: data != "" ? data.rows[0].cell[3] : "",
                    color: '#0B6BFD'
                }
                data_pie[1] = {
                    label: data != "" ? "Resolved" : "",
                    data: data != "" ? data.rows[0].cell[4] : "",
                    color: '#4A0093'
                }

                data_pie[2] = {
                    label: data != "" ? "Completed" : "",
                    data: data != "" ? data.rows[0].cell[5] : "",
                    color: '#007A01'
                }
                data_pie[3] = {
                    label: data != "" ? "Rejected" : "",
                    data: data != "" ? data.rows[0].cell[6] : "",
                    color: '#FE0000'
                }
            }
            if (data.rows.length == "undefined" || data.rows.length == undefined) {
                data_pie[0] = {
                    label: "Empty",
                    data: 1,
                    color: '#FE9900'
                }
            }
            $.plot($("#PerformerWiseIssueCountChart"), data_pie, {
                series: {
                    pie: {
                        show: true,
                        innerRadius: 0.5,
                        radius: 1,
                        label: {
                            show: true,
                            radius: 2 / 3,
                            formatter: function (label, series) {
                                if (label == "Empty") {
                                    return '<div style="font-size:11px;text-align:center;padding:4px;color:white;">%</div>';
                                }
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
