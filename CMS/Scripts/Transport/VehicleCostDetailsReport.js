jQuery(function ($) {
    debugger;
    GetCampus();
    var grid_selector = "#VehicleCostDetailsReportJqGrid";
    var pager_selector = "#VehicleCostDetailsReportJqGridPager";

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
    $('#txtFromDate1').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,


    });
    $('#txtToDate1').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,


    });
    jQuery(grid_selector).jqGrid({
        url: '/Transport/VehicleCostDetailsReportJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Campus', 'VehicleNo', 'Trip Count ', 'Total Driver OT', 'Total Helper OT', 'Total Diesel', 'Total Maintenance', 'Total Service', 'Total FC', 'Total Others'],
        colModel: [
             { name: 'Id', index: 'Id', width: 50, align: 'left', hidden: true, key: true },
             { name: 'Campus', index: 'Campus', width: 30, editable: true, sortable: false },
             { name: 'VehicleNo', index: 'VehicleNo', width: 30, editable: true, sortable: false },
             { name: 'TypeOfTrip', index: 'TypeOfTrip', width: 30, editable: true, sortable: false },
             { name: 'DriverOT', index: 'DriverOT', width: 30, editable: true, sortable: false },
             { name: 'HelperOT', index: 'HelperOT', width: 30, editable: true, sortable: false },
             { name: 'Diesel', index: 'Diesel', width: 30, editable: true, sortable: false },
             { name: 'Maintenance', index: 'Maintenance', width: 30, editable: true, sortable: false },
             { name: 'Service', index: 'Service', width: 30, editable: true, sortable: false },
             { name: 'FC', index: 'FC', width: 30, editable: true, sortable: false },
             { name: 'Others', index: 'Others', width: 30, editable: true, sortable: false },
        ],
        viewrecords: true,
        rowNum: 100,
        rowList: [100, 150, 200],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: "Asc",
        altRows: true,
        autowidth: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },


        caption: "<i class='ace-icon fa fa-truck'></i>&nbsp;Vehicle Cost Report"

    });
    $(window).triggerHandler('resize.jqGrid');
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, { edit: false, add: false, del: false, search: false, refresh: false },
       {},
       {},
       {}).navButtonAdd(pager_selector, {
           caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
           //buttonicon: "ui-icon-add",
           onClickButton: function () {
               Campus = $('#Campus').val();
               var sidx = jQuery(grid_selector).jqGrid('getGridParam', 'sortname');
               var sord = $(grid_selector).jqGrid('getGridParam', 'sortorder');
               var VehicleNo = $("#VehicleNo").val();
               var FromDate = $("#txtFromDate1").val();
               var ToDate = $("#txtToDate1").val();
               window.open("/Transport/VehicleCostDetailsReportJqGrid" + '?rows=9999&acadyr=' + Campus + '&ExprtToExcel=Excel' + '&sord=' + sord + '&sidx=' + sidx + '&VehicleNo=' + VehicleNo + '&FromDate=' + FromDate + '&ToDate=' + ToDate);
           },
           position: "last"
       })
    $('#btnSearch').click(function () {
        $(grid_selector).clearGridData();
        Campus = $('#Campus').val();
        var VehicleNo = $("#VehicleNo").val();
        var FromDate = $("#txtFromDate1").val();
        var ToDate = $("#txtToDate1").val();
        LoadSetGridParam($(grid_selector), "/Transport/VehicleCostDetailsReportJqGrid?Campus=" + Campus + '&VehicleNo=' + VehicleNo + '&FromDate=' + FromDate + '&ToDate=' + ToDate);

    });
    $("#btnReset").click(function () {
        //$("input[type=text], textarea, select").val("");
        $('#Campus').val('');
        $("#VehicleNo").val('');
        $("#txtFromDate1").val('');
        $("#txtToDate1").val('');
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Transport/VehicleCostDetailsReportJqGrid',
           postData: { Campus: "", VehicleNo: "", FromDate: "", ToDate: "" },
           page: 1
       }).trigger("reloadGrid");
    });
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

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});
function GetCampus() {
    var ddlcam = $("#Campus");
    $.ajax({
        type: 'POST',
        async: true,
        dataType: "json",
        url: "/Base/FillBranchCode",
        success: function (data) {
            ddlcam.empty();
            ddlcam.append("<option value=''> Select </option>");
            for (var i = 0; i < data.length; i++) {
                ddlcam.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}