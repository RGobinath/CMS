
var grid_selector = "#MaterialInwardReportList";
var pager_selector = "#MaterialInwardReportListPager";

$(window).on('resize.jqGrid', function () {
    $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
})

var parent_column = $(grid_selector).closest('[class*="col-"]');
$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
        setTimeout(function () {
            $(grid_selector).jqGrid('setGridWidth', parent_column.width());
        }, 0);
    }
})
$(function () {

    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialInwardReportListJqGrid',
        datatype: 'json',
        mtype: 'POST',
        colNames: ['Id', 'Inward Number', 'Campus', 'Store', 'Supplier', 'Supp Ref No', 'Invoice Date', 'Received DateTime', 'Received By', 'Created Date', 'Material Group', 'Material SubGroup', 'Material', 'Order.Qty', 'Ordered Units', 'Received.Qty', 'Received Units', 'Unit Price', 'Total Price', 'Damage Description'],
        colModel: [
            { name: 'Id', index: 'SkuId', hidden: true, key: true, editable: true },
            { name: 'InwardNumber', index: 'InwardNumber', editable: true },
            { name: 'Campus', index: 'Campus', editable: true },
            { name: 'Store', index: 'Store', editable: true },
            { name: 'Supplier', index: 'Supplier', editable: true },
            { name: 'SuppRefNo', index: 'SuppRefNo', editable: true },
            { name: 'InvoiceDate', index: 'InvoiceDate', editable: true },
            { name: 'ReceivedDateTime', index: 'ReceivedDateTime', width: 230, editable: true },
            { name: 'ReceivedBy', index: 'ReceivedBy', editable: true },
            { name: 'CreatedDate', index: 'CreatedDate', width: 230, editable: true },
            { name: 'MaterialGroup', index: 'MaterialGroup'},
            { name: 'MaterialSubGroup', index: 'MaterialSubGroup'},
            { name: 'Material', index: 'Material', editable: true },
            { name: 'OrderQty', index: 'OrderQty', editable: true },
            { name: 'OrderedUnits', index: 'OrderedUnits', editable: true },
            { name: 'ReceivedQty', index: 'ReceivedQty', editable: true },
            { name: 'ReceivedUnits', index: 'ReceivedUnits', editable: true },
            { name: 'UnitPrice', index: 'UnitPrice', editable: true },
            { name: 'TotalPrice', index: 'TotalPrice', editable: true },
            { name: 'DamageDescription', index: 'DamageDescription', editable: true },
            ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'CreatedDate',
        sortorder: 'desc',
        height: '130',
        //width: 1225,
        autowidth: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-sort-amount-asc"></i> Material Inward List',
        forceFit: true
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
            {},
            {}, {}, {});
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            var fromDate = $("#txtFromCreatedDate").val();
            var toDate = $("#txtToCreatedDate").val();
            window.open("MaterialInwardReportListJqGrid" + '?fromDate=' + fromDate + '&toDate=' + toDate
                 + '&InwardNumber=' + $("#gs_InwardNumber").val()
                 + '&Campus=' + $("#gs_Campus").val()
                 + '&Store=' + $("#gs_Store").val()
                 + '&Supplier=' + $("#gs_Supplier").val()
                 + '&InwardNumber=' + $("#gs_InwardNumber").val()
                 + '&ReceivedBy=' + $("#gs_ReceivedBy").val()
                 + '&MaterialGroup=' + $("#gs_MaterialGroup").val() 
                 + '&MaterialSubGroup=' + $("#gs_MaterialSubGroup").val()
                 + '&Material=' + $("#gs_Material").val()
                 + '&rows=9999' + '&Expt=Excel');
        }
    });
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(grid_selector).clearGridData();
        return false;
    }
    });
    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        var fromDate = $("#txtFromCreatedDate").val();
        var toDate = $("#txtToCreatedDate").val();
        if (fromDate == "") {
            ErrMsg("Please select From Date");
            $("#txtFromCreatedDate").focus();
            return false;
        }
        if (toDate == "") {
            ErrMsg("Please select To Date");
            $("#txtToCreatedDate").focus();
            return false;
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialInwardReportListJqGrid/',
                    postData: { fromDate: fromDate, toDate: toDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        var fromDate = $("#txtFromCreatedDate").val('');
        var toDate = $("#txtToCreatedDate").val('');
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialInwardReportListJqGrid/',
                    postData: { fromDate: fromDate, toDate: toDate },
                    page: 1
                }).trigger("reloadGrid");
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
