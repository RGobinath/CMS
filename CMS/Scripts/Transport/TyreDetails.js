jQuery(function ($) {
    var grid_selector = "#TyreDetailsList";
    var pager_selector = "#TyreDetailsListPager";

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
        url: '/Transport/ShowTyreDetailsFromStock',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Invoice Id', 'InvoiceNo', 'Tyre No', 'Make', 'Model', 'Size', 'Type', 'Tube Cost', 'Tyre Cost', 'Total Cost', 'Is Assigned', 'Assigned To'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'InvoiceId', index: 'InvoiceId', hidden: true },
              { name: 'InvoiceNo', index: 'InvoiceNo' },
              { name: 'TyreNo', index: 'TyreNo' },
              { name: 'Make', index: 'Make' },
              { name: 'Model', index: 'Model' },
              { name: 'Size', index: 'Size' },
              { name: 'Type', index: 'Type' },
              { name: 'TubeCost', index: 'TubeCost' },
              { name: 'TyreCost', index: 'TyreCost' },
              { name: 'TotalCost', index: 'TotalCost' },
              { name: 'IsAssigned', index: 'IsAssigned', stype: 'select', editoptions: { value: ":Select;True:Yes;False:No"} },
              { name: 'AssignedTo', index: 'AssignedTo' },
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
        caption: "<i class='ace-icon fa fa-bullseye'></i>&nbsp;Tyre Details"
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

    $('#btnSearch').click(function () {
        var InvoiceNo = $('#txtInvoiceNo').val();
        var TyreNo = $('#TyreNo').val();
        var Make = $('#Make').val();
        var Model = $('#Model').val();
        var Size = $('#Size').val();
        var Type = $('#Type').val();
        var TubeCost = $('#TubeCost').val();
        var TyreCost = $('#TyreCost').val();
        var TotalCost = $('#TotalCost').val();
        var IsAssigned = $('#IsAssigned').val();
        var AssignedTo = $('#AssignedTo').val();
        var TyreNo = $('#TyreNo').val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/ShowTyreDetailsFromStock',
                    postData: { InvoiceNo: InvoiceNo, TyreNo: TyreNo, Make: Make, Model: Model, Size: Size, Type: Type, TubeCost: TubeCost, TyreCost: TyreCost, TotalCost: TotalCost, IsAssigned: IsAssigned, AssignedTo: AssignedTo, TyreNo: TyreNo },
                    page: 1
                }).trigger("reloadGrid");
    });

    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        var InvoiceNo = $('#txtInvoiceNo').val();
        var TyreNo = $('#TyreNo').val();
        var Make = $('#Make').val();
        var Model = $('#Model').val();
        var Size = $('#Size').val();
        var Type = $('#Type').val();
        var TubeCost = $('#TubeCost').val();
        var TyreCost = $('#TyreCost').val();
        var TotalCost = $('#TotalCost').val();
        var IsAssigned = $('#IsAssigned').val();
        var AssignedTo = $('#AssignedTo').val();
        var TyreNo = $('#TyreNo').val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/ShowTyreDetailsFromStock',
                    postData: { InvoiceNo: InvoiceNo, TyreNo: TyreNo, Make: Make, Model: Model, Size: Size, Type: Type, TubeCost: TubeCost, TyreCost: TyreCost, TotalCost: TotalCost, IsAssigned: IsAssigned, AssignedTo: AssignedTo, TyreNo: TyreNo },
                    page: 1
                }).trigger("reloadGrid");
    });
});