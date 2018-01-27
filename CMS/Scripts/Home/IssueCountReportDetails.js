function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          var ddlbc1 = $("#ddlCampus1");
          ddlbc.empty();
          ddlbc1.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          ddlbc1.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              ddlbc1.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
    var grid_selector1 = "#JqGridCampus";
    var pager_selector1 = "#JqGridCampusPager";
    var grid_selector2 = "#JqGridIssueGroup";
    var pager_selector2 = "#JqGridIssueGroupPager";
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
        url: '/Home/GetIssueCountReportByCampusJqGrid',
        datatype: "json",
        colNames: ['Id', 'Campus', 'Logged', 'Completed', 'Non Completed', 'Resolve Issue', 'Approve Issue', 'Complete'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'BranchCode', index: 'BranchCode', width: 180 },
              { name: 'Logged', index: 'Logged',align: 'center', width: 170 },
              { name: 'Completed', index: 'Completed', align: 'center', width: 170 },
              { name: 'NonCompleted', index: 'NonCompleted', align: 'center', width: 170 },
              { name: 'ResolveIssue', index: 'ResolveIssue', align: 'center', width: 170 },
              { name: 'ApproveIssue', index: 'ApproveIssue', align: 'center', width: 170 },
              { name: 'Complete', index: 'Complete', align: 'center', width: 170 }],
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
            $(grid_selector1).footerData('set', { BranchCode: 'Total:' });
            var colLogged = $(grid_selector1).jqGrid('getCol', 'Logged', false, 'sum');
            $(grid_selector1).footerData('set', { Logged: colLogged });
            var colCompleted = $(grid_selector1).jqGrid('getCol', 'Completed', false, 'sum');
            $(grid_selector1).footerData('set', { Completed: colCompleted });
            var colNonCompleted = $(grid_selector1).jqGrid('getCol', 'NonCompleted', false, 'sum');
            $(grid_selector1).footerData('set', { NonCompleted: colNonCompleted });
            var colResolveIssue = $(grid_selector1).jqGrid('getCol', 'ResolveIssue', false, 'sum');
            $(grid_selector1).footerData('set', { ResolveIssue: colResolveIssue });
            var colApproveIssue = $(grid_selector1).jqGrid('getCol', 'ApproveIssue', false, 'sum');
            $(grid_selector1).footerData('set', { ApproveIssue: colApproveIssue });
            var colComplete = $(grid_selector1).jqGrid('getCol', 'Complete', false, 'sum');
            $(grid_selector1).footerData('set', { Complete: colComplete });            
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);
        },
        viewrecords: true,
        sortorder: "Asc",
        caption: "Issue Count Report By Campus"
    });
    jQuery(grid_selector1).jqGrid('navGrid', pager_selector1, { edit: false, add: false, del: false, search: false, refresh: false },
    {},
    {},
    {}).navButtonAdd(pager_selector1, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        //buttonicon: "ui-icon-add",
        onClickButton: function () {
            //acayr = $('#txtacayear2').val();
            var sidx = jQuery(grid_selector1).jqGrid('getGridParam', 'sortname');
            var sord = $(grid_selector1).jqGrid('getGridParam', 'sortorder');
            var Campus = $("#ddlCampus").val();
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            window.open("/Home/GetIssueCountReportByCampusJqGrid" + '?rows=9999&Campus=' + Campus + '&ExprtToExcel=Excel' + '&sord=' + sord + '&sidx=' + sidx);
        },
        position: "last"
    })
    $('#btnsrch2').click(function () {
        $(grid_selector1).clearGridData();
        var Campus = $('#ddlCampus').val();
        //var Status = $("#ddlstatus").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        LoadSetGridParam($(grid_selector1), "/Home/GetIssueCountReportByCampusJqGrid?Campus=" + Campus + '&FromDate=' + FromDate + '&ToDate=' + ToDate);

    });
    $("#btnreset2").click(function () {
        //$("input[type=text], textarea, select").val("");
        $('#ddlCampus').val('');
        $("#txtFromDate").val('');
        $("#txtToDate").val('');
        jQuery(grid_selector1).setGridParam(
       {
           datatype: "json",
           url: '/Home/GetIssueCountReportByCampusJqGrid',
           postData: { Campus: "", FromDate: "", ToDate: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    jQuery(grid_selector2).jqGrid({
        url: '/Home/GetIssueCountReportByIssueGroupJqGrid',
        datatype: "json",
        colNames: ['Id', 'Campus','Issue Group', 'Logged', 'Completed', 'Non Completed', 'Resolve Issue', 'Approve Issue', 'Complete'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'BranchCode', index: 'BranchCode', width: 210 },
              { name: 'IssueGroup', index: 'IssueGroup', width: 210},
              { name: 'Logged', index: 'Logged',align: 'center', width: 130 },
              { name: 'Completed', index: 'Completed', align: 'center', width: 130 },
              { name: 'NonCompleted', index: 'NonCompleted', align: 'center', width: 130 },
              { name: 'ResolveIssue', index: 'ResolveIssue', align: 'center', width: 130 },
              { name: 'ApproveIssue', index: 'ApproveIssue', align: 'center', width: 130 },
              { name: 'Complete', index: 'Complete', align: 'center', width: 130 }
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
            $(grid_selector2).footerData('set', { IssueGroup: 'Total:' });
            var colLogged = $(grid_selector2).jqGrid('getCol', 'Logged', false, 'sum');
            $(grid_selector2).footerData('set', { Logged: colLogged });
            var colCompleted = $(grid_selector2).jqGrid('getCol', 'Completed', false, 'sum');
            $(grid_selector2).footerData('set', { Completed: colCompleted });
            var colNonCompleted = $(grid_selector2).jqGrid('getCol', 'NonCompleted', false, 'sum');
            $(grid_selector2).footerData('set', { NonCompleted: colNonCompleted });
            var colResolveIssue = $(grid_selector2).jqGrid('getCol', 'ResolveIssue', false, 'sum');
            $(grid_selector2).footerData('set', { ResolveIssue: colResolveIssue });
            var colApproveIssue = $(grid_selector2).jqGrid('getCol', 'ApproveIssue', false, 'sum');
            $(grid_selector2).footerData('set', { ApproveIssue: colApproveIssue });
            var colComplete = $(grid_selector2).jqGrid('getCol', 'Complete', false, 'sum');
            $(grid_selector2).footerData('set', { Complete: colComplete });
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);

        },
        viewrecords: true,
        sortorder: "Asc",
        caption: "Issue Count Report By Issue Group"
    });
    jQuery(grid_selector2).jqGrid('navGrid', pager_selector2, { edit: false, add: false, del: false, search: false, refresh: false },
    {},
    {},
    {}).navButtonAdd(pager_selector2, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        //buttonicon: "fa fa-file-excel-o",
        onClickButton: function () {
            var sidx = jQuery(grid_selector2).jqGrid('getGridParam', 'sortname');
            var sord = $(grid_selector2).jqGrid('getGridParam', 'sortorder');
            var Campus = $("#ddlCampus1").val();
            var IssueGroup = $("#ddlIssueGroup1").val();
            var FromDate = $("#txtFromDate1").val();
            var ToDate = $("#txtToDate1").val();
            window.open("/Home/GetIssueCountReportByIssueGroupJqGrid" + '?rows=9999&Campus=' + Campus + '&IssueGroup=' + IssueGroup + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&ExprtToExcel=Excel' + '&sidx=' + sidx + '&sord=' + sord);
        },
        position: "last"
    })
    $('#btnsrch1').click(function () {
        $(grid_selector2).clearGridData();        
        var Campus = $("#ddlCampus1").val();
        var IssueGroup = $("#ddlIssueGroup1").val();
        var FromDate = $("#txtFromDate1").val();
        var ToDate = $("#txtToDate1").val();
        LoadSetGridParam($(grid_selector2), "/Home/GetIssueCountReportByIssueGroupJqGrid?Campus=" + Campus + '&IssueGroup=' + IssueGroup + '&FromDate=' + FromDate + '&ToDate=' + ToDate);

    });
    $("#btnreset1").click(function () {
        //$("input[type=text], textarea, select").val("");
        $("#ddlCampus1").val('');
        $("#ddlIssueGroup1").val('');
        $("#txtFromDate1").val('');
        $("#txtToDate1").val('');
        jQuery(grid_selector2).setGridParam(
       {
           datatype: "json",
           url: '/Home/GetIssueCountReportByIssueGroupJqGrid',
           postData: { Campus: "", IssueGroup: "", FromDate: "", ToDate: "" },
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
    $.getJSON("/Home/FillIssueGroup",
     function (fillig) {
         var ddlig = $("#ddlIssueGroup1");
         ddlig.empty();
         ddlig.append($('<option/>', { value: "", text: "Select" }));
         $.each(fillig, function (index, itemdata) {             
                 ddlig.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));             
         });

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
