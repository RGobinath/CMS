jQuery(function ($) {
    var grid_selector = "#VehicleElectricalMaintenanceList";
    var pager_selector = "#VehicleElectricalMaintenanceListPager";

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
        url: '/Transport/VehicleElectricalMaintenanceReportJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Vehicle Id', 'Vehicle No','Campus', 'Date Of Service', 'Service Provider', 'Service Cost', 'Bill No', 'SpareParts Used', 'Description', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
            { name: 'Campus', index: 'Campus' },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'EDateOfService', index: 'EDateOfService', search: false },
            { name: 'EServiceProvider', index: 'EServiceProvider' },
            { name: 'EServiceCost', index: 'EServiceCost' },
            { name: 'EBillNo', index: 'EBillNo' },
            { name: 'ESparePartsUsed', index: 'ESparePartsUsed' },
            { name: 'EDescription', index: 'EDescription' },
            { name: 'CreatedDate', index: 'CreatedDate', search: false },
            { name: 'CreatedBy', index: 'CreatedBy' }
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
        caption: "<i class='ace-icon fa fa-cogs'></i>&nbsp;Electrical Maintenance List"

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
            var EDateOfService = $("#gs_EDateOfService").val();
            var EServiceProvider = $("#gs_EServiceProvider").val();
            var EServiceCost = $("#gs_EServiceCost").val();
            var EBillNo = $("#gs_EBillNo").val();
            var ESparePartsUsed = $("#gs_ESparePartsUsed").val();
            var EDescription = $("#gs_EDescription").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/VehicleElectricalMaintenanceReportJqGrid" + '?ExportType=Excel'
                +'&Campus='+Campus
                    + '&VehicleNo=' + VehicleNo
                    + '&EDateOfService=' + EDateOfService
                    + '&EServiceProvider=' + EServiceProvider
                    + '&EServiceCost=' + EServiceCost
                    + '&EBillNo=' + EBillNo
                    + '&ESparePartsUsed=' + ESparePartsUsed
                    + '&EDescription=' + EDescription
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
                    url: '/Transport/VehicleElectricalMaintenanceReportJqGrid',
                    postData: { Campus: $("#ddlcampus").val(), VehicleNo: $("#VehicleNo").val(), EServiceProvider: $("#EServiceProvider").val(), EServiceCost: $("#EServiceCost").val(), EBillNo: $("#EBillNo").val(), ESparePartsUsed: $("#ESparePartsUsed").val(), EDescription: $("#EDescription").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleElectricalMaintenanceReportJqGrid',
                    postData: { Campus: $("#ddlcampus").val(), VehicleNo: $("#VehicleNo").val(), EServiceProvider: $("#EServiceProvider").val(), EServiceCost: $("#EServiceCost").val(), EBillNo: $("#EBillNo").val(), ESparePartsUsed: $("#ESparePartsUsed").val(), EDescription: $("#EDescription").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});