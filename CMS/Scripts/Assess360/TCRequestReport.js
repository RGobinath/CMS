jQuery(function ($) {
    var grid_selector1 = "#JqGridreason";
    var pager_selector1 = "#JqGridreasonPager";
    var grid_selector2 = "#JqGridgrade";
    var pager_selector2 = "#JqGridgradePager";
    $(window).on('resize.jqGrid', function () {
        $(grid_selector1).jqGrid('setGridWidth', $(".tab-content").width());
        $(grid_selector2).jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand 
    var parent_column = $(grid_selector2).closest('[class*="tab-content"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector1).jqGrid('setGridWidth', parent_column.width());
                $(grid_selector2).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector1).jqGrid({
        url: '/Admission/TCRequestReportByReasonListJqGrid',
        datatype: "json",
        colNames: ['Id', 'Academic Year', 'ReasonFor TCRequest', 'IB Main', 'IB KG', 'Chennai Main', 'Chennai City', 'Ernakulam', 'Ernakulam KG', 'Karur', 'Karur KG', 'Tirupur', 'Tirupur KG', 'Tips Saran'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'AcademicYear', index: 'AcademicYear' },
              { name: 'ReasonForTCRequest', index: 'ReasonForTCRequest' },
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
              { name: 'TipsSaran', index: 'TipsSaran' }],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector1,
        sortname: 'Id',
        multiselect: true,
        autowidth: true,
        height: 250,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);
        },
        viewrecords: true,
        sortorder: "Asc",

        caption: "TCReport By Reason"
    });
    jQuery(grid_selector1).jqGrid('navGrid', pager_selector1, { edit: false, add: false, del: false, search: false, refresh: false },
    {},{},{}).navButtonAdd(pager_selector1, {
        caption: "Export To Excel",
        buttonicon: "ui-icon-add",
        onClickButton: function () {
            acayr = $('#txtacayear2').val();
            window.open("/Admission/TCRequestReportByReasonListJqGrid" + '?rows=9999&acadyr=' + acayr + '&ExprtToExcel=Excel');
        },
        position: "last"
    })
    $('#btnreset2,#btnreset1').click(function () {
        $("input[type=text], textarea, select").val("");
        LoadSetGridParam($(grid_selector1), "/Admission/TCRequestReportByReasonListJqGrid?acadyr=");
        LoadSetGridParam($(grid_selector2), "/Admission/TCRequestReportByReasonListJqGrid?acadyr=");
    })
    $('#btnsrch2').click(function () {
        $(grid_selector1).clearGridData();
        acayr = $('#txtacayear2').val();
        LoadSetGridParam($(grid_selector1), "/Admission/TCRequestReportByReasonListJqGrid?acadyr=" + acayr);

    });
    jQuery(grid_selector2).jqGrid({
        url: '/Admission/TCRequestReportByGradeListJqGrid',
        datatype: "json",
        colNames: ['Id', 'Academic Year', 'Grade', 'IB Main', 'IB KG', 'Chennai Main', 'Chennai City', 'Ernakulam', 'Ernakulam KG', 'Karur', 'Karur KG', 'Tirupur', 'Tirupur KG', 'Tips Saran'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'AcademicYear', index: 'AcademicYear' },
              { name: 'Grade', index: 'Grade' },
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
              { name: 'TipsSaran', index: 'TipsSaran' }],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector2,
        sortname: 'Id',
        multiselect: true,
        autowidth: true,
        height: 250,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);
        },
        viewrecords: true,
        sortorder: "Asc",
        caption: "TCReport By Grade"
    });
    jQuery(grid_selector2).jqGrid('navGrid', pager_selector1, { edit: false, add: false, del: false, search: false, refresh: false },
    {},
    {},
    {}).navButtonAdd(pager_selector2, {
        caption: "Export To Excel",
        buttonicon: "ui-icon-add",
        onClickButton: function () {
            acayr = $('#txtacayear1').val();
            window.open("/Admission/TCRequestReportByGradeListJqGrid" + '?rows=9999&acadyr=' + acayr + '&ExprtToExcel=Excel');
        },
        position: "last"
    })
    $('#btnsrch1').click(function () {
        $(grid_selector1).clearGridData();
        acayr = $('#txtacayear1').val();
        LoadSetGridParam($(grid_selector2), "/Admission/TCRequestReportByGradeListJqGrid?acadyr=" + acayr);

    });
});

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
