jQuery(function ($) {
    var grid_selector = "#VehicleReport";
    var pager_selector = "#VehicleReportPager";

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
    var Campus;
    jQuery(grid_selector).jqGrid({
        url: '/Transport/VehicleReportJqGrid',
        datatype: 'json',
        height: 250,
        colNames: ['Id', 'VehicleId', 'Type', 'Campus', 'Vehicle No', 'Distance Covered', 'Fuel Consumed', 'Fuel Cost', 'Mileage', 'FC', 'Insurance', 'Mechanical Maintenance', 'AC Maintenance', 'Electrical Maintenance', 'Body Maintenance', 'Tyre Maintenance'],
        colModel: [
                          { name: 'Id', index: 'Id', editable: true, hidden: true, key: true, search: false },
                          { name: 'VehicleId', index: 'VehicleId', search: false },
                          { name: 'Campus', index: 'Campus' },
                          { name: 'Type', index: 'Type' },
                          { name: 'VehicleNo', index: 'VehicleNo' },
                          
                          { name: 'DistanceCovered', index: 'DistanceCovered', search: false },
                          { name: 'FuelConsumed', index: 'FuelConsumed', search: false },
                          { name: 'FuelCost', index: 'FuelCost', search: false },
                          { name: 'Mileage', index: 'Mileage', search: false },
                          { name: 'FC', index: 'FC', search: false },
                          { name: 'Insurance', index: 'Insurance', search: false },
                          { name: 'MechanicalMaintenance', index: 'MechanicalMaintenance', search: false },
                          { name: 'ACMaintenance', index: 'ACMaintenance', search: false },
                          { name: 'ElectricalMaintenance', index: 'ElectricalMaintenance', search: false },
                          { name: 'BodyMaintenance', index: 'BodyMaintenance', search: false },
                          { name: 'TyreMaintenance', index: 'TyreMaintenance', search: false }
                          ],
        viewrecords: true,
        rowNum: 10,
        rowList: [25, 50, 100, 500],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-car'></i>&nbsp;Vehicle Report"

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
            var Campus = $("#ddlcampus").val();
            var Type = $("#gs_Type").val();
            var VehicleNo = $("#gs_VehicleNo").val();
            var Campus = $("#gs_Campus").val();
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            window.open("VehicleReportJqGrid" + '&Campus=' + Campus + + '?Type=' + Type + '&VehicleNo=' + VehicleNo +  '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&rows=9999' + '&ExportType=Excel');
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
        var Campus = $("#ddlcampus").val();
        var Type = $("#gs_Type").val();
        var VehicleNo = $("#gs_VehicleNo").val();
        
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleReportJqGrid',
                    postData: { Campus: Campus, Type: Type, VehicleNo: VehicleNo, FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#Reset").click(function () {
        $("input[type=text], textarea, select").val("");
        var Type = $("#gs_Type").val();
        var VehicleNo = $("#gs_VehicleNo").val();
        var Campus = $("#gs_Campus").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleReportJqGrid',
                    postData: {Campus: Campus, Type: Type, VehicleNo: VehicleNo, Campus: Campus, FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");
    });
});