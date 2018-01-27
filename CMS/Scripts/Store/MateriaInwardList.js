$(function () {
    var url;
    var grid_selector = "#MaterialInwardList";
    var pager_selector = "#MaterialInwardListPager";
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
    var status = $("#ddlStatus").val();

    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialInwardListJqGrid?status=' + status,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Inward Number', 'Items Count', 'Supplier', 'PO Number', 'SuppRefNo', 'Invoice Date', 'ReceivedBy', 'Rcvd Date Time', 'Created Date', 'Created By', 'Status'],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'InwardNumber', index: 'InwardNumber', formatter: formateadorLink, sortable: true },
             { name: 'TotalCount', index: 'TotalCount', sortable: true, cellattr: function (rowId, cellValue, rawObject, cm, rdata) {
                 if (cellValue == 0) {
                     return 'class="ui-state-error ui-state-error-text"';
                 }}
             },
             { name: 'Supplier', index: 'Supplier', sortable: true },
             { name: 'PONumber', index: 'PONumber', sortable: true },
             { name: 'SuppRefNo', index: 'SuppRefNo', sortable: true },
             { name: 'InvoiceDate', index: 'InvoiceDate', sortable: true },
             { name: 'ReceivedBy', index: 'ReceivedBy', sortable: true },
             { name: 'ReceivedDateTime', index: 'ReceivedDateTime', sortable: true, width: 170 },
             { name: 'CreatedDate', index: 'CreatedDate', sortable: true, width: 170 },
             { name: 'ProcessedBy', index: 'ProcessedBy', sortable: true },
             { name: 'Status', index: 'Status', sortable: true },
             ],
        pager: '#MaterialInwardListPager',
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '230',
        autowidth: true,
        viewrecords: true,
        subGrid: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-magnet'></i>&nbsp;Material Inward List",
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (SKUList, Id) {
            var selectedData = $(grid_selector).jqGrid('getRowData', Id);
            if (parseInt(selectedData.TotalCount) == 0) {
                InfoMsg("No Items to display");
                return false;
            }
            else {
                var SKUListTable, SKUListPager;
                SKUListTable = SKUList + "_t";
                SKUListPager = "p_" + SKUListTable;
                $("#" + SKUList).html("<table id='" + SKUListTable + "' ></table><div id='" + SKUListPager + "' ></div>");
                jQuery("#" + SKUListTable).jqGrid({
                    url: '/Store/MaterialSkuListJqGrid?Id=' + Id,
                    datatype: 'json',
                    mtype: 'GET',
                    colNames: ['SKU Id', 'MaterialRefId', 'Material', 'Material Group', 'Material Sub Group', 'Ord.Units', 'Rcvd.Units', 'Ord.Qty', 'Rcvd.Qty', 'Dmg.Qty', 'Unit Price', 'Total Price', 'Dmg.Desc / Remarks'],
                    colModel: [
                    { name: 'SkuId', index: 'SkuId', hidden: true, key: true },
                    { name: 'MaterialRefId', index: 'MaterialRefId', hidden: true },
                    { name: 'Material', index: 'Material', width: 90, sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[3] + ', Material Sub Group:' + rawObject[4] + '"' } },
                    { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true, hidden: true },
                    { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, sortable: true, hidden: true },
                    { name: 'OrderedUnits', index: 'OrderedUnits', width: 90, sortable: true },
                    { name: 'ReceivedUnits', index: 'ReceivedUnits', width: 90, sortable: true },
                    { name: 'OrderQty', index: 'OrderQty', width: 90, sortable: true },
                    { name: 'ReceivedQty', index: 'ReceivedQty', width: 90, sortable: true },
                    { name: 'DamagedQty', index: 'DamagedQty', width: 90, sortable: true },
                    { name: 'UnitPrice', index: 'UnitPrice', width: 90, sortable: true },
                    { name: 'TotalPrice', index: 'TotalPrice', width: 90, sortable: true },
                    { name: 'DamageDescription', index: 'DamageDescription', width: 150, sortable: true }
                    ],
                    pager: SKUListPager,
                    rowNum: '5',
                    rowList: [5, 10, 20, 50, 100, 150, 200],
                    sortname: 'SkuId',
                    sortorder: 'Desc',
                    height: '130',
                    autowidth: true,
                    viewrecords: true,
                    loadComplete: function () {
                        var table = this;
                        setTimeout(function () {
                            updatePagerIcons(table);
                            enableTooltips(table);
                        }, 0);
                    }
                });
                jQuery("#" + SKUListTable).jqGrid('navGrid', "#" + SKUListPager, { edit: false, add: false, del: false })
            }
        }
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
            {}, {}, {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "Export To Excel",
        onClickButton: function () {
            var Supplier = $("#Supplier").val();
            var SuppRefNo = $("#SuppRefNo").val();
            var InvoiceDate = $("#InvoiceDate").val();
            var DCDate = $("#DCDate").val();
            var PONumber = $("#PONumber").val();
            var status = $("#ddlStatus").val();
            window.open("MaterialInwardListJqGrid" + '?Supplier=' + Supplier + '&SuppRefNo=' + SuppRefNo + '&InvoiceDate=' + InvoiceDate + '&DCDate=' + DCDate + '&PONumber=' + PONumber + '&status=' + status + '&rows=9999' + '&ExptXl=1');
        }
    });

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        var Supplier = $("#Supplier").val();
        var SuppRefNo = $("#SuppRefNo").val();
        var InvoiceDate = $("#InvoiceDate").val();
        var DCDate = $("#DCDate").val();
        var PONumber = $("#PONumber").val();
        var status = $("#ddlStatus").val();

        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Store/MaterialInwardListJqGrid',
                        postData: { Supplier: Supplier, SuppRefNo: SuppRefNo, InvoiceDate: InvoiceDate, DCDate: DCDate, PONumber: PONumber, status: status },
                        page: 1
                    }).trigger("reloadGrid");
    });

    $("#btnNewMaterialInward").click(function () {
        url = $('#NewMINUrl').val();
        window.location.href = url;
    });
    $("#btnReset").click(function () {
        url = $('#BackUrl').val();
        window.location.href = url;
    });
});

function formateadorLink(cellvalue, options, rowObject) {
    if (rowObject[11] == "Open") {
        return "<a href=/Store/MaterialInward?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
    else {
        return "<a href=/Store/ShowMaterialInward?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
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
