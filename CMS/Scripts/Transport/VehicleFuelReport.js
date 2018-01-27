jQuery(function ($) {
    var grid_selector = "#vehicleFuelReport";
    var pager_selector = "#vehicleFuelReportPager";

    //resize to fit page size
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
    var Campus = $("#ddlcampus").val();
    var Month = $("#Month").val();
    var Year = $("#Year").val();
    var Type;
    var VehicleNo;

    $("#ddlyear").val(Year);
    $.getJSON("/Base/FillMonth",
             function (fillig) {
                 var mior = $("#ddlmonth");
                 mior.empty();
                 mior.append("<option value=' '> Select </option>");
                 $.each(fillig, function (index, itemdata) {
                     if (itemdata.Value != Month) {
                         mior.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                     }
                     else {
                         mior.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text,
                         selected: "Selected"

                     }));
                     }
                 });
             });

    jQuery(grid_selector).jqGrid({
        url: '/Transport/VehicleFuelReportJqGrid?CurrMonth=' + Month + '&CurrYear=' + Year,
        datatype: 'json',
        height: 250,
        colNames: ['Id', 'Campus', 'Vehicle No', 'Vehicle Id', 'Vehicle Type', 'Fuel Type', 'Type', 'FromDate', 'Todate', 'Fuel Quantity', 'Total Price'],
        colModel: [
              { name: 'Id', index: 'Id',hidden: true  },
              { name: 'Campus', index: 'Campus', width: '250px' },
              { name: 'VehicleNo', index: 'VehicleNo', width: '250px' },
              { name: 'VehicleId', index: 'VehicleId', width: '250px', hidden: true },
              { name: 'VehicleType', index: 'VehicleType', width: '250px' },
              { name: 'FuelType', index: 'FuelType', width: '250px' },
              { name: 'Type', index: 'Type', width: '250px' },
              { name: 'FromDate', index: 'FromDate' },
              { name: 'Todate', index: 'Todate' },
              { name: 'FuelQty', index: 'FuelQty' },
              { name: 'TotalPrice', index: 'TotalPrice' },
              ],
        viewrecords: true,
        rowNum: 99999,
        //rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'desc',
        multiselect: true,
        pgbuttons : false,
        pgtext : "",
        pginput : false,
        //loadComplete: function () {
        //    var table = this;
            //setTimeout(function () {
            //    //styleCheckbox(table);
            //    //updateActionIcons(table);
            //    //updatePagerIcons(table);
            //    //enableTooltips(table);
            //}, 0);
        //},
        caption: "<i class='ace-icon fa fa-tint'></i>&nbsp;Vehicle Fuel Report"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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
            }, {}, {}, {}, {})

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            Campus = $("#ddlcampus").val();
            Type = $("#txtType").val();
            VehicleNo = $("#txtVehicleNo").val();
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            window.open("/Transport/VehicleFuelReportJqGrid" + '?ExptXl=1'
                + '&Campus=' + Campus
                + '&Type=' + Type
                + '&VehicleNo=' + VehicleNo
                + '&FromDate=' + FromDate
                + '&ToDate=' + ToDate
                + ' &rows=9999 ');
        }
    });

    //For pager Icons
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
    $("#btnSearch").click(function () {
        debugger;
        jQuery(grid_selector).clearGridData();
        Campus = $("#ddlcampus").val();
        Type = $("#txtType").val();
        VehicleNo = $("#txtVehicleNo").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleFuelReportJqGrid',
                    postData: { Campus: Campus, Type: Type, VehicleNo: VehicleNo,  FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");

    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        Campus = $("#ddlcampus").val();
        Type = $("#txtType").val();
        VehicleNo = $("#txtVehicleNo").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleFuelReportJqGrid',
                    postData: { Campus: Campus, Type: Type, VehicleNo: VehicleNo, FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    

    $("#txtToDate").click(function () {
        var SDate = $("#txtFromDate").val();
        if (SDate == "") {
            ErrMsg("Please select From Date");
            return false;
        }
    });
    $("#btnSearch").click(function () {
        var SDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        if (SDate == "" && ToDate=="") {
            ErrMsg("Please select From and To Date");
            return false;
        }
    });
});