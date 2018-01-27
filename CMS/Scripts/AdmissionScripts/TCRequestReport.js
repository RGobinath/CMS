jQuery(function ($) {
    var grid_selector1 = "#JqGridreason";
    var pager_selector1 = "#JqGridreasonPager";
    var grid_selector2 = "#JqGridgrade";
    var pager_selector2 = "#JqGridgradePager";
    $(window).on('resize.jqGrid', function () {
        //$(grid_selector1).jqGrid('setGridWidth', $(".tab-content").width());
        $(grid_selector1).jqGrid('setGridWidth', $(".page-content").width());
        $(grid_selector2).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand 
    //var parent_column = $(grid_selector2).closest('[class*="tab-content"]');
    //$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    //    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
    //        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
    //        setTimeout(function () {
    //            //$(grid_selector1).jqGrid('setGridWidth', parent_column.width());

    //            $(grid_selector2).jqGrid('setGridWidth', parent_column.width());
    //        }, 0);
    //    }
    //})
    var parent_column1 = $(grid_selector1).closest('[class*="col-"]');
    var parent_column2 = $(grid_selector2).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector1).jqGrid('setGridWidth', parent_column.width());
                jQuery(grid_selector2).jqGrid('setGridWidth', parent_column2.width());
            }, 0);
        }
    })
    jQuery(grid_selector1).jqGrid({
        url: '/Admission/TCRequestReportByReasonListJqGrid',
        datatype: "json",
        colNames: ['Id', 'Academic Year', 'Reason For TCRequest', 'IB Main', 'IB KG', 'Chennai Main', 'Chennai City', 'Ernakulam', 'Ernakulam KG', 'Karur', 'Karur KG', 'Tirupur', 'Tirupur KG', 'Tips Saran', 'Total'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'AcademicYear', index: 'AcademicYear', width: 80 },
              { name: 'ReasonForTCRequest', index: 'ReasonForTCRequest', width: 170 },
              { name: 'IBMain', index: 'IBMain', align: 'center', width: 80 },
              { name: 'IBKG', index: 'IBKG', align: 'center', width: 80 },
              { name: 'ChennaiMain', index: 'ChennaiMain', align: 'center', width: 80 },
              { name: 'ChennaiCity', index: 'ChennaiCity', align: 'center', width: 80 },
              { name: 'Ernakulam', index: 'Ernakulam', align: 'center', width: 80 },
              { name: 'ErnakulamKG', index: 'ErnakulamKG', align: 'center', width: 80 },
              { name: 'Karur', index: 'Karur', align: 'center', width: 80 },
              { name: 'KarurKG', index: 'KarurKG', align: 'center', width: 80 },
              { name: 'Tirupur', index: 'Tirupur', align: 'center', width: 80 },
              { name: 'TirupurKG', index: 'TirupurKG', align: 'center', width: 80 },
              { name: 'TipsSaran', index: 'TipsSaran', align: 'center', width: 80 },
              { name: 'Total', index: 'Total', align: 'center', width: 80 }],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector1,
        sortname: 'Id',
        multiselect: true,
        //autowidth: true,
        //shrinkToFit: true,
        height: 190,
        footerrow: true,
        loadComplete: function () {
            var table = this;
            $(grid_selector1).footerData('set', { AcademicYear: 'Total:' });
            var colIBMain = $(grid_selector1).jqGrid('getCol', 'IBMain', false, 'sum');
            $(grid_selector1).footerData('set', { IBMain: colIBMain });
            var colIBKG = $(grid_selector1).jqGrid('getCol', 'IBKG', false, 'sum');
            $(grid_selector1).footerData('set', { IBKG: colIBKG });
            var colChennaiMain = $(grid_selector1).jqGrid('getCol', 'ChennaiMain', false, 'sum');
            $(grid_selector1).footerData('set', { ChennaiMain: colChennaiMain });
            var colChennaiCity = $(grid_selector1).jqGrid('getCol', 'ChennaiCity', false, 'sum');
            $(grid_selector1).footerData('set', { ChennaiCity: colChennaiCity });
            var colErnakulam = $(grid_selector1).jqGrid('getCol', 'Ernakulam', false, 'sum');
            $(grid_selector1).footerData('set', { Ernakulam: colErnakulam });
            var colErnakulamKG = $(grid_selector1).jqGrid('getCol', 'ErnakulamKG', false, 'sum');
            $(grid_selector1).footerData('set', { ErnakulamKG: colErnakulamKG });
            var colKarur = $(grid_selector1).jqGrid('getCol', 'Karur', false, 'sum');
            $(grid_selector1).footerData('set', { Karur: colKarur });
            var colKarurKG = $(grid_selector1).jqGrid('getCol', 'KarurKG', false, 'sum');
            $(grid_selector1).footerData('set', { KarurKG: colKarurKG });
            var colTirupur = $(grid_selector1).jqGrid('getCol', 'Tirupur', false, 'sum');
            $(grid_selector1).footerData('set', { Tirupur: colTirupur });
            var colTirupurKG = $(grid_selector1).jqGrid('getCol', 'TirupurKG', false, 'sum');
            $(grid_selector1).footerData('set', { TirupurKG: colTirupurKG });
            var colTipsSaran = $(grid_selector1).jqGrid('getCol', 'TipsSaran', false, 'sum');
            $(grid_selector1).footerData('set', { TipsSaran: colTipsSaran });
            var colTotal = $(grid_selector1).jqGrid('getCol', 'Total', false, 'sum');
            $(grid_selector1).footerData('set', { Total: colTotal });
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);
        },
        viewrecords: true,
        sortorder: "Asc",
        caption: "TC Report By Reason"
    });
    jQuery(grid_selector1).jqGrid('navGrid', pager_selector1, { edit: false, add: false, del: false, search: false, refresh: false },
    {},
    {},
    {}).navButtonAdd(pager_selector1, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        //buttonicon: "ui-icon-add",
        onClickButton: function () {
            acayr = $('#txtacayear2').val();
            var sidx = jQuery(grid_selector1).jqGrid('getGridParam', 'sortname');
            var sord = $(grid_selector1).jqGrid('getGridParam', 'sortorder');
            var Status = $("#ddlstatus").val();
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            window.open("/Admission/TCRequestReportByReasonListJqGrid" + '?rows=9999&acadyr=' + acayr + '&ExprtToExcel=Excel' + '&sord=' + sord + '&sidx=' + sidx);
        },
        position: "last"
    })
    $('#btnsrch2').click(function () {
        $(grid_selector1).clearGridData();
        acayr = $('#txtacayear2').val();
        var Status = $("#ddlstatus").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        LoadSetGridParam($(grid_selector1), "/Admission/TCRequestReportByReasonListJqGrid?acadyr=" + acayr + '&Status=' + Status + '&FromDate=' + FromDate + '&ToDate=' + ToDate);

    });
    $("#btnreset2").click(function () {
        //$("input[type=text], textarea, select").val("");
        $('#txtacayear2').val('');
        $("#ddlstatus").val('');
        $("#txtFromDate").val('');
        $("#txtToDate").val('');
        jQuery(grid_selector1).setGridParam(
       {
           datatype: "json",
           url: '/Admission/TCRequestReportByReasonListJqGrid',
           postData: { acadyr: "", Status: "", FromDate: "", ToDate: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    jQuery(grid_selector2).jqGrid({
        url: '/Admission/TCRequestReportByGradeListJqGrid',
        datatype: "json",
        colNames: ['Id', 'Academic Year', 'Grade', 'IB Main', 'IB KG', 'Chennai Main', 'Chennai City', 'Ernakulam', 'Ernakulam KG', 'Karur', 'Karur KG', 'Tirupur', 'Tirupur KG', 'Tips Saran', 'Total'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'AcademicYear', index: 'AcademicYear', width: 80 },
              { name: 'Grade', index: 'Grade', width: 80 },
              { name: 'IBMain', index: 'IBMain', width: 80, align: 'center' },
              { name: 'IBKG', index: 'IBKG', width: 80, align: 'center' },
              { name: 'ChennaiMain', index: 'ChennaiMain', width: 80, align: 'center' },
              { name: 'ChennaiCity', index: 'ChennaiCity', width: 80, align: 'center' },
              { name: 'Ernakulam', index: 'Ernakulam', width: 90, align: 'center' },
              { name: 'ErnakulamKG', index: 'ErnakulamKG', width: 93, align: 'center' },
              { name: 'Karur', index: 'Karur', width: 90, align: 'center' },
              { name: 'KarurKG', index: 'KarurKG', width: 90, align: 'center' },
              { name: 'Tirupur', index: 'Tirupur', width: 90, align: 'center' },
              { name: 'TirupurKG', index: 'TirupurKG', width: 90, align: 'center' },
              { name: 'TipsSaran', index: 'TipsSaran', width: 90, align: 'center' },
              { name: 'Total', index: 'Total', width: 90, align: 'center' }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector2,
        sortname: 'Id',
        multiselect: true,
        //autowidth: true,
        height: 190,
        //shrinkToFit: true,
        footerrow: true,
        loadComplete: function () {
            var table = this;
            $(grid_selector2).footerData('set', { AcademicYear: 'Total:' });
            var colIBMain = $(grid_selector2).jqGrid('getCol', 'IBMain', false, 'sum');
            $(grid_selector2).footerData('set', { IBMain: colIBMain });
            var colIBKG = $(grid_selector2).jqGrid('getCol', 'IBKG', false, 'sum');
            $(grid_selector2).footerData('set', { IBKG: colIBKG });
            var colChennaiMain = $(grid_selector2).jqGrid('getCol', 'ChennaiMain', false, 'sum');
            $(grid_selector2).footerData('set', { ChennaiMain: colChennaiMain });
            var colChennaiCity = $(grid_selector2).jqGrid('getCol', 'ChennaiCity', false, 'sum');
            $(grid_selector2).footerData('set', { ChennaiCity: colChennaiCity });
            var colErnakulam = $(grid_selector2).jqGrid('getCol', 'Ernakulam', false, 'sum');
            $(grid_selector2).footerData('set', { Ernakulam: colErnakulam });
            var colErnakulamKG = $(grid_selector2).jqGrid('getCol', 'ErnakulamKG', false, 'sum');
            $(grid_selector2).footerData('set', { ErnakulamKG: colErnakulamKG });
            var colKarur = $(grid_selector2).jqGrid('getCol', 'Karur', false, 'sum');
            $(grid_selector2).footerData('set', { Karur: colKarur });
            var colKarurKG = $(grid_selector2).jqGrid('getCol', 'KarurKG', false, 'sum');
            $(grid_selector2).footerData('set', { KarurKG: colKarurKG });
            var colTirupur = $(grid_selector2).jqGrid('getCol', 'Tirupur', false, 'sum');
            $(grid_selector2).footerData('set', { Tirupur: colTirupur });
            var colTirupurKG = $(grid_selector2).jqGrid('getCol', 'TirupurKG', false, 'sum');
            $(grid_selector2).footerData('set', { TirupurKG: colTirupurKG });
            var colTipsSaran = $(grid_selector2).jqGrid('getCol', 'TipsSaran', false, 'sum');
            $(grid_selector2).footerData('set', { TipsSaran: colTipsSaran });
            var colTotal = $(grid_selector2).jqGrid('getCol', 'Total', false, 'sum');
            $(grid_selector2).footerData('set', { Total: colTotal });
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);

        },
        viewrecords: true,
        sortorder: "Asc",
        caption: "TC Report By Grade"
    });
    jQuery(grid_selector2).jqGrid('navGrid', pager_selector2, { edit: false, add: false, del: false, search: false, refresh: false },
    {},
    {},
    {}).navButtonAdd(pager_selector2, {
        caption:'&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        //buttonicon: "fa fa-file-excel-o",
        onClickButton: function () {
            var sidx = jQuery(grid_selector2).jqGrid('getGridParam', 'sortname');
            var sord = $(grid_selector2).jqGrid('getGridParam', 'sortorder');
            acayr = $('#txtacayear1').val();
            var Status = $("#ddlstatus1").val();
            var FromDate = $("#txtFromDate1").val();
            var ToDate = $("#txtToDate1").val();
            window.open("/Admission/TCRequestReportByGradeListJqGrid" + '?rows=9999&acadyr=' + acayr + '&Status=' + Status + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&ExprtToExcel=Excel' + '&sidx=' + sidx + '&sord=' + sord);
        },
        position: "last"
    })
    $('#btnsrch1').click(function () {
        $(grid_selector2).clearGridData();
        acayr = $('#txtacayear1').val();
        var Status = $("#ddlstatus1").val();
        var FromDate = $("#txtFromDate1").val();
        var ToDate = $("#txtToDate1").val();
        LoadSetGridParam($(grid_selector2), "/Admission/TCRequestReportByGradeListJqGrid?acadyr=" + acayr + '&Status=' + Status + '&FromDate=' + FromDate + '&ToDate=' + ToDate);

    });
    $("#btnreset1").click(function () {
        //$("input[type=text], textarea, select").val("");
        $('#txtacayear1').val('');
        $("#ddlstatus1").val('');
        $("#txtFromDate1").val('');
        $("#txtToDate1").val('');
        jQuery(grid_selector2).setGridParam(
       {
           datatype: "json",
           url: '/Admission/TCRequestReportByGradeListJqGrid',
           postData: { acadyr: "", Status: "", FromDate: "", ToDate: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    var startDate1 = new Date();
    var FromEndDate1 = new Date();
    var ToEndDate1 = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);
    ToEndDate1.setDate(ToEndDate1.getDate() + 365);
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
    $('#txtFromDate1').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        //startDate: startDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        startDate1 = new Date(selected.date.valueOf());
        startDate1.setDate(startDate1.getDate(new Date(selected.date.valueOf())));
        $('#txtToDate1').datepicker('setStartDate', startDate1);
    });
    $('#txtToDate1').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        startDate: startDate1,
        endDate: ToEndDate1,
        autoclose: true
    }).on('changeDate', function (selected) {
        FromEndDate1 = new Date(selected.date.valueOf());
        FromEndDate1.setDate(FromEndDate1.getDate(new Date(selected.date.valueOf())));
        $('#txtFromDate1').datepicker('setEndDate', FromEndDate1);
    });
    $("#txtToDate1").focus(function () {
        if ($("#txtFromDate1").val() == "") {
            return ErrMsg("Please Select FromDate");
        }
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
