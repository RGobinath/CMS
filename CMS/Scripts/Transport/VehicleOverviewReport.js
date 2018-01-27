var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
$(function () {
    var grid_selector = "#VehicleOverviewReportListJqGrid";
    var pager_selector = "#VehicleOverviewReportListJqGridPager";
    $(grid_selector).jqGrid({
        url: '/Transport/VehicleOverviewReportListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Vehicle No', 'Vehicle Type', 'DriverOt', 'Helper Ot', 'Fuel', 'Maintenance', 'Service', 'FC', 'Others', 'Total No Of Trips', 'Total No Of Trip Kms', 'Total Expenses'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true, key: true },
                    { name: 'Campus', index: 'Campus' },
                    { name: 'VehicleNo', index: 'VehicleNo' },
                    { name: 'VehicleType', index: 'VehicleType' },
                    { name: 'DriverOt', index: 'DriverOt', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'HelperOt', index: 'HelperOt', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'Fuel', index: 'Fuel', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'Maintenance', index: 'Maintenance', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'Service', index: 'Service', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'FC', index: 'FC', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'Others', index: 'Others', hidden: true, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
                    { name: 'TotalNoOfTrip', index: 'TotalNoOfTrip' },
                    { name: 'TotalDistance', index: 'TotalDistance' },
                    { name: 'Expenses', index: 'Expenses', formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },

        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'TypeOrder',
        sortorder: 'Asc',
        autowidth: true,
        height: 230,
        viewrecords: true,
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
        caption: '<i class="fa fa-list"></i> Vehicle Overview Report'
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
            var VehicleTravelDate = $("#txtrptMonthYear").val();
            var TravelDate = VehicleTravelDate;
            if (VehicleTravelDate != '') {
                //$("#financeyear").hide();
                VehicleTravelDate = VehicleTravelDate.split(' ');
                TravelDate = monthNames.indexOf(VehicleTravelDate[0]) + 1;
                if (TravelDate <= 9) {
                    TravelDate = "0" + TravelDate;
                }
                TravelDate = TravelDate + "-" + VehicleTravelDate[1];
            }
            var sidx = jQuery(grid_selector).jqGrid('getGridParam', 'sortname');
            var sord = $(grid_selector).jqGrid('getGridParam', 'sortorder');
            window.open("VehicleOverviewReportListJqGrid" + '?Campus=' + $("#ddlrptCampus").val() + '&VehicleNo=' + $("#txtrptVehicleNo").val() + '&VehicleType=' + $("#ddlrptVehicleType").val() + '&MonthYear=' + TravelDate + '&rows=9999999' + '&ExportType=Excel' + '&sord=' + sord + '&sidx=' + sidx + '&page=0');
        }
    });
    $("#btnrptReset").click(function () {
        $("#ddlrptCampus").val('');
        $("#txtrptVehicleNo").val('');
        $("#ddlrptVehicleType").val('');
        $("#txtrptMonthYear").val('');
        $("#financeyear").show();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Transport/VehicleOverviewReportListJqGrid',
           postData: { Campus: "", VehicleNo: "", VehicleType: "", MonthYear: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnrptSearch").click(function () {
        var Campus = $("#ddlrptCampus").val();
        var VehicleNo = $("#txtrptVehicleNo").val();
        var VehicleType = $("#ddlrptVehicleType").val();
        var VehicleTravelDate = $("#txtrptMonthYear").val();
        var TravelDate = VehicleTravelDate;
        if (VehicleTravelDate != '') {
            $("#financeyear").hide();
            VehicleTravelDate = VehicleTravelDate.split(' ');
            TravelDate = monthNames.indexOf(VehicleTravelDate[0]) + 1;
            if (TravelDate <= 9) {
                TravelDate = "0" + TravelDate;
            }
            TravelDate = TravelDate + "-" + VehicleTravelDate[1];
        }
        else {
            $("#financeyear").show();
        }
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Transport/VehicleOverviewReportListJqGrid',
           postData: { Campus: Campus, VehicleNo: VehicleNo, VehicleType: VehicleType, MonthYear: TravelDate },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#txtrptMonthYear").datepicker({
        format: "MM yyyy",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    });
    $("#txtrptMonthYear").keypress(function (e) {
        e.preventDefault();
    });
    $(this).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlrptCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
    $.getJSON("/Base/GetVehicleType",
      function (fillbc) {
          var ddlbc = $("#ddlrptVehicleType");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
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