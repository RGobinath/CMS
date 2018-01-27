$(function () {

    var grid_selector = "#CampusWiseModuleUsageReport_vwGrid";
    var pager_selector = "#CampusWiseModuleUsageReport_vwPager";

    jQuery(grid_selector).jqGrid({
        url: '/StudentsReport/GetCampusWiseModuleUsageReport_vwListJqGrid',
        datatype: "json",
        colNames: ['Id','Module','IB Main','IB KG','Chennai Main','Chennai City','Ernakulam','Ernakulam KG','Karur','Karur KG','Tirupur','Tirupur KG','Tips Saran' ],
        //colNames: ['Id', 'Module', 'MCS ANTHIYUR', 'MHSS ANTHIYUR', 'MMS ANTHIYUR', 'MCOE ANTHIYUR', 'MTTI ANTHIYUR', 'RPS KOTAGIRI'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'Module', index: 'Module' },
              { name: 'IBMain', index: 'IBMain' },
              { name: 'IBKG', index: 'IBKG' },
              { name: 'ChennaiMain', index: 'ChennaiMain' },
              { name: 'ChennaiCity', index: 'ChennaiCity' },
              { name: 'Ernakulam', index: 'Ernakulam' },
              { name: 'ErnakulamKG', index: 'ErnakulamKG' },
              { name: 'Karur', index: 'Karur' },
              { name: 'KarurKG', index: 'KarurKG' },
              { name: 'Tirupur', index: 'Tirupur' },
              { name: 'TirupurKG', index: 'TirupurKG' },
              { name: 'TipsSaran', index: 'TipsSaran' }
          ],  

        //colModel: [
        //{ name: 'Id', index: 'Id', hidden: true, key: true },
        //{ name: 'Module', index: 'Module' },
        //{ name: 'MCS_ANTHIYUR', index: 'MCS_ANTHIYUR' },
        //{ name: 'MHSS_ANTHIYUR', index: 'MHSS_ANTHIYUR' },
        //{ name: 'MMS_ANTHIYUR', index: 'MMS_ANTHIYUR' },
        //{ name: 'MCOE_ANTHIYUR', index: 'MCOE_ANTHIYUR' },
        //{ name: 'MTTI_ANTHIYUR', index: 'MTTI_ANTHIYUR' },
        //{ name: 'RPS_KOTAGIRI', index: 'RPS_KOTAGIRI' }

        //],

        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        sortname: 'Id',
        multiselect: true,
        autowidth: true,
        height:250,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
               // styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                //enableTooltips(table);
            }, 0);
        },
        viewrecords: true,
        sortorder: "Asc",

        caption: "Campus Wise Module Usage Report"
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, { edit: false, add: false, del: false, search: false, refresh: false },
    {},
    {},
    {}).navButtonAdd(pager_selector, {
        caption: "Export To Excel",
        buttonicon: "ui-icon-add",
        onClickButton: function () {
            //alert("Export to Excel");
            window.open("/StudentsReport/GetCampusWiseModuleUsageReport_vwListJqGrid" + '?rows=9999&ExprtToExcel=Excel');
        },
        position: "last"
    })

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
    $("#btnSearch").click(function () {
        var Module = $("#Module").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StudentsReport/GetCampusWiseModuleUsageReport_vwListJqGrid/',
           postData: { Module: Module },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        var Module = $("#Module").val();
        jQuery(grid_selector).setGridParam(
        {
            datatype: "json",
            url: '/StudentsReport/GetCampusWiseModuleUsageReport_vwListJqGrid/',
            postData: { Module: Module },
            page: 1
        }).trigger("reloadGrid");

    });
    //$("#Module").autocomplete({
    //    source: function (request, response) {
    //        $.ajax({
    //            url: "/Reports/PageCountModuleAutoComplete",
    //            type: "POST",
    //            dataType: "json",
    //            data: { term: request.term },
    //            success: function (data) {
    //                response($.map(data, function (item) {
    //                    return { label: item.Module, value: item.Module };
    //                }))

    //            }
    //        })
    //    },
    //    messages: {
    //        noResults: "", results: ""
    //    }
    //});
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