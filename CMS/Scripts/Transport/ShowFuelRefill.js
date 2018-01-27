$(function () {
    var grid_selector = "#FuelRefillList";
    var pager_selector = "#FuelRefillListPager";
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
    var Id = $("#Id").val();
    jQuery(grid_selector).jqGrid({
        url: "/Transport/FuelRefillListBulkEntryJqGrid?RefId=" + Id,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'RefId', 'Vehicle Id', 'Type', 'Vehicle No', 'Fuel Type', 'Fuel Fill Type', 'Fuel Quantity', 'Litre Price', 'Total Price', 'Last MM Reading', 'Current MM Reading',
            'Mileage', 'Filled By', 'Filled Date', 'Bunk Name', 'Created By', 'CreatedDate'],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'RefId', index: 'RefId', hidden: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'Type', index: 'Type' },
             { name: 'VehicleNo', index: 'VehicleNo' },
             { name: 'FuelType', index: 'FuelType' },
             { name: 'FuelFillType', index: 'FuelFillType' },
             { name: 'FuelQuantity', index: 'FuelQuantity' },
             { name: 'LitrePrice', index: 'LitrePrice' },
             { name: 'TotalPrice', index: 'TotalPrice' },
             { name: 'LastMilometerReading', index: 'LastMilometerReading' },
             { name: 'CurrentMilometerReading', index: 'CurrentMilometerReading' },
             { name: 'Mileage', index: 'Mileage' },
             { name: 'FilledBy', index: 'FilledBy' },
             { name: 'FilledDate', index: 'FilledDate' },
             { name: 'BunkName', index: 'BunkName', editable: true },
             { name: 'CreatedBy', index: 'CreatedBy' },
             { name: 'CreatedDate', index: 'CreatedDate' }
                ],
        pager: pager_selector,
        rowNum: '100',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Asc',
        height: '230',
        autowidth: true,
        //shrinktofit: true,
        viewrecords: true,
        caption: 'Fuel Refill List',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(grid_selector).jqGrid('setGridWidth');
        }
    });
    //navButtons Add, edit, delete
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
            {},
            {}, {}, {})
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
    $("#btnBack").click(function () {
        window.location.href = "/Transport/FuelRefillBulkEntry/";
    });
});
