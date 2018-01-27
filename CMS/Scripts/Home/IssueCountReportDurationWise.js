var grid_selector = "#IssueCountReport";
var pager_selector = "#IssueCountReportJqGridPager";

$(window).on('resize.jqGrid', function () {
    $(grid_selector).jqGrid('setGridWidth', $(".col-sm-6").width());
})

$(function () {
    jQuery(grid_selector).jqGrid({
        url: '/Home/IssueCountReportDurationWiseJqGrid/',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Logged', 'Completed', 'Non Completed'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'Campus', index: 'Campus' },
              { name: 'Logged', index: 'Logged' },
              { name: 'Completed', index: 'Completed' },
              { name: 'NonCompleted', index: 'NonCompleted' },
               ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [50, 100, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '200',

        // autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="ace-icon fa fa-list"></i>Issue Count Report'
    });
    //$("#IssueCountReport").navGrid('#IssueCountReportJqGridPager', { add: false, edit: false, del: false, search: false, refresh: true });
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
            {}, {}, {});

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            var Campus = $("#ddlCampus").val();
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            window.open("IssueCountReportDurationWiseJqGrid" + '?Campus=' + Campus + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&rows=9999' + '&ExptXl=1');
        }
    });

    $("#btnSearch").click(function () {
        debugger;
        $(grid_selector).clearGridData();
        var Campus = $("#ddlCampus").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();

        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Home/IssueCountReportDurationWiseJqGrid/',
                    postData: { Campus: Campus, FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");

        LoadIssueStatusChart(Campus, FromDate, ToDate);
    });
    $.getJSON("/Base/FillBranchCode",
             function (fillig) {
                 var ddlcam = $("#ddlCampus");
                 ddlcam.empty();
                 ddlcam.append($('<option/>',
                {
                    value: "",
                    text: "Select One"

                }));

                 $.each(fillig, function (index, itemdata) {
                     ddlcam.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                 });
             });
    LoadIssueStatusChart('', '', '');

    $("#btnReset").click(function () {
        window.location.href = "IssueCountReportDurationWise", "Home";
    });
});
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}
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

function LoadIssueStatusChart(Campus, FromDate, ToDate) {
    $.ajax({
        type: 'Get',
        url: '/Home/IssueCountReportChartDurationWise?Campus=' + Campus + '&FromDate=' + FromDate + '&ToDate=' + ToDate,
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "600", "400");
            chart.setDataXML(data);
            chart.render("WeeklyIssueStatus1Div");
            if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                processBusy.dialog('close');
            }
        },
        async: false,
        dataType: "text"
    });
}