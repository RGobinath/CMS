jQuery(function ($) {
    var grid_selector = "#VehicleTyreMaintenanceList";
    var pager_selector = "#VehicleTyreMaintenanceListPager";

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

    jQuery(grid_selector).jqGrid({
        url: '/Transport/VehicleTyreMaintenanceReportJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Vehicle Id','Campus', 'Vehicle No', 'Maintenance Type', 'Location', 'Tyre Type', 'Make', 'Model', 'Size', 'Assigned Date', 'Milometer Reading', 'Reason For Removing',
             'Cost', 'Bill No', 'Date Of Alignment', 'Date Of Rotation', 'Date Of WheelService', 'Service Cost', 'Maintenance Bill No', 'Tyre Date Of Service', 'Tyre Service Provider', 'Cost Of Service', 'Tyre Serviced By', 'Tyre Service BillNo', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
            {name:'Campus',index:'Campus'},
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'TyreMaintenanceType', index: 'TyreMaintenanceType' },
            { name: 'TyreLocation', index: 'TyreLocation' },
            { name: 'TypeOfTyre', index: 'TypeOfTyre' },
            { name: 'TyreMake', index: 'TyreMake' },
            { name: 'TyreModel', index: 'TyreModel' },
            { name: 'TyreSize', index: 'TyreSize' },
            { name: 'TyreDateOfEntry', index: 'TyreDateOfEntry' },
            { name: 'TyreMilometerReading', index: 'TyreMilometerReading' },
            { name: 'TyreReasonForRemoving', index: 'TyreReasonForRemoving' },
            { name: 'TyreCost', index: 'TyreCost' },
            { name: 'TyreBillNo', index: 'TyreBillNo' },
            { name: 'TyreDateOfAlignment', index: 'TyreDateOfAlignment' },
            { name: 'TyreDateOfRotation', index: 'TyreDateOfRotation' },
            { name: 'TyreDateOfWheelService', index: 'TyreDateOfWheelService' },
            { name: 'TyreServiceCost', index: 'TyreServiceCost' },
            { name: 'TyreMaintenanceBillNo', index: 'TyreMaintenanceBillNo' },
            { name: 'TyreDateOfService', index: 'TyreDateOfService' },
            { name: 'TyreServiceProvider', index: 'TyreServiceProvider' },
            { name: 'CostOfService', index: 'CostOfService' },
            { name: 'TyreServicedBy', index: 'TyreServicedBy' },
            { name: 'TyreServiceBillNo', index: 'TyreServiceBillNo' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
            ],
        viewrecords: true,
        rowNum: 6,
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
        caption: "<i class='ace-icon fa fa-car'></i>&nbsp;Tyre Maintenance List"

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
            var VehicleNo = $("#gs_VehicleNo").val();
            var TyreMaintenanceType = $("#gs_TyreMaintenanceType").val();
            var TyreLocation = $("#gs_TyreLocation").val();
            var TypeOfTyre = $("#gs_TypeOfTyre").val();
            var TyreMake = $("#gs_TyreMake").val();
            var TyreModel = $("#gs_TyreModel").val();
            var TyreSize = $("#gs_TyreSize").val();
            var TyreDateOfEntry = $("#gs_TyreDateOfEntry").val();
            var TyreMilometerReading = $("#gs_TyreMilometerReading").val();
            var TyreCost = $("#gs_TyreCost").val();
            var TyreBillNo = $("#gs_TyreBillNo").val();
            var TyreDateOfAlignment = $("#gs_TyreDateOfAlignment").val();
            var TyreDateOfRotation = $("#gs_TyreDateOfRotation").val();
            var TyreDateOfWheelService = $("#gs_TyreDateOfWheelService").val();
            var TyreServiceCost = $("#gs_TyreServiceCost").val();
            var TyreMaintenanceBillNo = $("#gs_TyreMaintenanceBillNo").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/VehicleTyreMaintenanceReportJqGrid" + '?ExportType=Excel'
                    +'&Campus='+Campus
                    + '&VehicleNo=' + VehicleNo
                    + '&TyreMaintenanceType=' + TyreMaintenanceType
                    + '&TyreLocation=' + TyreLocation
                    + '&TypeOfTyre=' + TypeOfTyre
                    + '&TyreMake=' + TyreMake
                    + '&TyreModel=' + TyreModel
                    + '&TyreSize=' + TyreSize
                    + '&TyreDateOfEntry=' + TyreDateOfEntry
                    + '&TyreMilometerReading=' + TyreMilometerReading
                    + '&TyreCost=' + TyreCost
                    + '&TyreBillNo=' + TyreBillNo
                    + '&TyreDateOfAlignment=' + TyreDateOfAlignment
                    + '&TyreDateOfRotation=' + TyreDateOfRotation
                    + '&TyreDateOfWheelService=' + TyreDateOfWheelService
                    + '&TyreServiceCost=' + TyreServiceCost
                    + '&TyreMaintenanceBillNo=' + TyreMaintenanceBillNo
                    + '&CreatedDate=' + CreatedDate
                    + '&CreatedBy=' + CreatedBy
                    + '&rows=9999');
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
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleTyreMaintenanceReportJqGrid',
                    postData: { Campus: $("#ddlcampus").val(),VehicleNo: $("#VehicleNo").val(), TyreMaintenanceType: $("#TyreMaintenanceType").val(), TyreLocation: $("#TyreLocation").val(), TypeOfTyre: $("#TypeOfTyre").val(), TyreMake: $("#TyreMake").val(), TyreModel: $("#TyreModel").val(), TyreSize: $("#TyreSize").val(), TyreCost: $("#TyreCost").val(), TyreBillNo: $("#TyreBillNo").val(), TyreServiceCost: $("#TyreServiceCost").val(), TyreMaintenanceBillNo: $("#TyreMaintenanceBillNo").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleTyreMaintenanceReportJqGrid',
                    postData: { Campus: $("#ddlcampus").val(),VehicleNo: $("#VehicleNo").val(), TyreMaintenanceType: $("#TyreMaintenanceType").val(), TyreLocation: $("#TyreLocation").val(), TypeOfTyre: $("#TypeOfTyre").val(), TyreMake: $("#TyreMake").val(), TyreModel: $("#TyreModel").val(), TyreSize: $("#TyreSize").val(), TyreCost: $("#TyreCost").val(), TyreBillNo: $("#TyreBillNo").val(), TyreServiceCost: $("#TyreServiceCost").val(), TyreMaintenanceBillNo: $("#TyreMaintenanceBillNo").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});