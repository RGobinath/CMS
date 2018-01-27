var grid_selector = "#IssueCountReport";
var pager_selector = "#IssueCountReportJqGridPager";


$(window).on('resize.jqGrid', function () {
    $(grid_selector).jqGrid('setGridWidth', $(".col-sm-6").width());
})
var parent_column = $(grid_selector).closest('[class*="col-"]');
$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
        setTimeout(function () {
            $(grid_selector).jqGrid('setGridWidth', parent_column.width());
        }, 0);
    }
})

$(function () {
    ;
    var Campus;
    var Year;
    var CurrYear = $('#CurrYear').val();
    IssueFusionChartLoading(Campus, CurrYear);
    $("#ddlyear").val(CurrYear);
    $.getJSON("/Base/FillAllBranchCode",
             function (fillig) {
                 var ddlcam = $("#ddlCampus");
                 ddlcam.empty();
                 ddlcam.append("<option value=' '> Select </option>");
                 $.each(fillig, function (index, itemdata) {
                     ddlcam.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                 });
             });

    $("#btnSearch").click(function () {
        ;
        $(grid_selector).clearGridData();
        Campus = $("#ddlCampus").val();
        Year = $("#ddlyear").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Home/IssueCountReportJqGrid/',
                    postData: { Campus: Campus, CountYear: Year },
                    page: 1
                }).trigger("reloadGrid");
        IssueFusionChartLoading(Campus, Year);
    });

    $('#btnReset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });

    Campus = $("#ddlCampus").val();
    Year = $("#ddlyear").val();
    jQuery(grid_selector).jqGrid({
        url: '/Home/IssueCountReportJqGrid/?Campus=' + Campus + '&CountYear=' + CurrYear,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Year', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'Campus', index: 'Campus', width: '250px' },
              { name: 'Year', index: 'Year' },
              { name: 'January', index: 'January' },
              { name: 'February', index: 'February' },
              { name: 'March', index: 'March' },
              { name: 'April', index: 'April' },
              { name: 'May', index: 'May' },
              { name: 'June', index: 'June' },
              { name: 'July', index: 'July' },
              { name: 'August', index: 'August' },
              { name: 'September', index: 'September' },
              { name: 'October', index: 'October' },
              { name: 'November', index: 'November' },
              { name: 'December', index: 'December' },
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '200',
        // width: 750,
        // autowidth: true,
        // shrinkToFit: true,
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
        caption: '<i class="menu-icon fa fa-pencil-square-o"></i>&nbsp;&nbsp;Issue Count Report',
        forceFit: true
    });
    //$("#IssueCountReport").navGrid('#IssueCountReportJqGridPager', { add: false, edit: false, del: false, search: false, refresh: false });

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
            Campus = $("#ddlCampus").val();
            Year = $("#ddlyear").val();
            window.open("IssueCountReportJqGrid" + '?Campus=' + Campus + '&CountYear=' + Year + '&rows=9999' + '&ExptXl=1');
        }
    });
    function IssueFusionChartLoading(campus, Year) {
        $.ajax({
            type: 'Get',
            url: '/Home/GetIssueCountReportChart/',
            data: { Campus: campus, CountYear: Year },
            success: function (data) {
                var chart = new FusionCharts("../../Charts/FCF_Column3D.swf", "productSales", "600", "320");
                chart.setDataXML(data);
                chart.render("IssueCountChart");
            },

            async: false,
            dataType: "text"
        });
    }
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