var grid_selector = "#loginCountReport";
var pager_selector = "#loginCountReportJqGridPager";

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
    var UserType;
    var Year;
    LogInFusionChartLoading(UserType, CurrYear);
    var CurrYear = $('#CurrYear').val();
    $("#ddlyear").val(CurrYear);
    $.getJSON("/Account/FillUserType",
                 function (fillusertype) {
                     var ddlusrtyp = $("#ddlUserType");
                     ddlusrtyp.empty();
                     ddlusrtyp.append($('<option/>',
                    {
                        value: "",
                        text: "Select One"

                    }));

                     $.each(fillusertype, function (index, itemdata) {
                         ddlusrtyp.append($('<option/>',
                             {
                                 value: itemdata.Value,
                                 text: itemdata.Text
                             }));
                     });
                 });


    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        UserType = $("#ddlUserType").val();
        Year = $("#ddlyear").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Home/loginCountReportJqGrid/',
                    postData: { UserType: UserType, CountYear: Year },
                    page: 1
                }).trigger("reloadGrid");
        LogInFusionChartLoading(UserType, Year);
    });

    $('#btnReset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });

    var UserType = $("#ddlUserType").val();
    var Year = $("ddlyear").val();
    jQuery(grid_selector).jqGrid({
        url: '/Home/loginCountReportJqGrid?UserType=' + UserType + '&CountYear=' + CurrYear,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'UserType', 'Year', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'UserType', index: 'UserType', width: '200px' },
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
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '200',
        //width: 750,
        //  autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="menu-icon fa fa-pencil-square-o"></i>&nbsp;&nbsp;Login Count Report',
        forceFit: true
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
            {},
            {}, {}, {});

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            Campus = $("#ddlCampus").val();
            Year = $("#ddlyear").val();
            window.open("loginCountReportJqGrid" + '?UserType=' + UserType + '&CountYear=' + Year + '&rows=9999' + '&ExptXl=1');
        }
    });

    function LogInFusionChartLoading(UserType, Year) {
        $.ajax({
            type: 'Get',
            url: '/Home/GetloginCountReportChart/',
            data: { UserType: UserType, CountYear: Year },
            success: function (data) {
                var chart = new FusionCharts("../../Charts/FCF_Column3D.swf", "productSales", "600", "320");
                chart.setDataXML(data);
                chart.render("LogInCountChart");
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
