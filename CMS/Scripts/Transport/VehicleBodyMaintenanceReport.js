jQuery(function ($) {
    var grid_selector = "#VehicleBodyMaintenanceList";
    var pager_selector = "#VehicleBodyMaintenanceListPager";

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
        url: '/Transport/VehicleBodyMaintenanceReportJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id','Campus', 'Vehicle Id', 'Vehicle No', 'Type Of Body', 'Date Of Repair', 'Type Of Repair', 'Parts Required', 'Service Provider', 'Service Cost', 'Bill No', 'Description', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
             { name: 'Campus', index: 'Campus' },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'BTypeOfBody', index: 'BTypeOfBody' },
            { name: 'BDateOfRepair', index: 'BDateOfRepair', search: false },
            { name: 'BTypeOfRepair', index: 'BTypeOfRepair' },
            { name: 'BPartsRequired', index: 'BPartsRequired' },
            { name: 'BServiceProvider', index: 'BServiceProvider' },
            { name: 'BServiceCost', index: 'BServiceCost' },
            { name: 'BBillNo', index: 'BBillNo' },
            { name: 'BDescription', index: 'BDescription' },
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
        caption: "<i class='ace-icon fa fa-shield'></i>&nbsp;Body Maintenance List"

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
            var BTypeOfBody = $("#gs_BTypeOfBody").val();
            var BDateOfRepair = $("#gs_BDateOfRepair").val();
            var BTypeOfRepair = $("#gs_BTypeOfRepair").val();
            var BPartsRequired = $("#gs_BPartsRequired").val();
            var BServiceProvider = $("#gs_BServiceProvider").val();
            var BServiceCost = $("#gs_BServiceCost").val();
            var BBillNo = $("#gs_BBillNo").val();
            var BDescription = $("#gs_BDescription").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/VehicleBodyMaintenanceReportJqGrid" + '?ExportType=Excel'
                + '&Campus=' + Campus
                    + '&VehicleNo=' + VehicleNo
                    + '&BTypeOfBody=' + BTypeOfBody
                    + '&BDateOfRepair=' + BDateOfRepair
                    + '&BTypeOfRepair=' + BTypeOfRepair
                    + '&BPartsRequired=' + BPartsRequired
                    + '&BServiceProvider=' + BServiceProvider
                    + '&BServiceCost=' + BServiceCost
                    + '&BServiceCost=' + BServiceCost
                    + '&BBillNo=' + BBillNo
                    + '&BDescription=' + BDescription
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
                    url: '/Transport/VehicleBodyMaintenanceReportJqGrid',
                    postData: { Campus: $("#ddlcampus").val(), VehicleNo: $("#VehicleNo").val(), BTypeOfBody: $("#BTypeOfBody").val(), BTypeOfRepair: $("#BTypeOfRepair").val(), BPartsRequired: $("#BPartsRequired").val(), BServiceProvider: $("#BServiceProvider").val(), BServiceCost: $("#BServiceCost").val(), BBillNo: $("#BBillNo").val(), BDescription: $("#BDescription").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleBodyMaintenanceReportJqGrid',
                    postData: { Campus: $("#ddlcampus").val(), VehicleNo: $("#VehicleNo").val(), BTypeOfBody: $("#BTypeOfBody").val(), BTypeOfRepair: $("#BTypeOfRepair").val(), BPartsRequired: $("#BPartsRequired").val(), BServiceProvider: $("#BServiceProvider").val(), BServiceCost: $("#BServiceCost").val(), BBillNo: $("#BBillNo").val(), BDescription: $("#BDescription").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});