var grid_selector = "#IssueNoteReportList";
var pager_selector = "#IssueNoteReportListListPager";
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
        url: '/Store/MaterialIssueNoteReportListJqGrid',
        datatype: 'json',
        mtype: 'POST',
        colNames: ['Id', 'IssNoteNumber', 'Request Number', 'Requested By', 'Issued By', 'Issue Date', 'Required For Campus', 'Required For Store', 'Delivered Through', 'Delivery Details', 'Request Type',
             'Required For Grade', 'Required For', 'Material Group', 'Material SubGroup', 'Material', 'Units', 'Issued.Qty', 'Total Price'],
        colModel: [
            { name: 'Id', index: 'SkuId', hidden: true, key: true, editable: true },
            { name: 'IssNoteNumber', index: 'IssNoteNumber', editable: true },
            { name: 'RequestNumber', index: 'RequestNumber', editable: true },
            { name: 'ProcessedBy', index: 'ProcessedBy', editable: true },
            { name: 'IssuedBy', index: 'IssuedBy', editable: true },
            { name: 'IssueDate', index: 'IssueDate', width: 230, editable: true },
            { name: 'RequiredForCampus', index: 'RequiredForCampus', editable: true },
            { name: 'RequiredForStore', index: 'RequiredForStore', editable: true },
            { name: 'DeliveredThrough', index: 'DeliveredThrough', editable: true },
            { name: 'DeliveryDetails', index: 'DeliveryDetails', editable: true },
            { name: 'RequestType', index: 'RequestType', editable: true },
            { name: 'RequiredForGrade', index: 'RequiredForGrade', editable: true },
            { name: 'RequiredFor', index: 'RequiredFor', width: 200, editable: true },
            { name: 'MaterialGroup', index: 'MaterialGroup' },
            { name: 'MaterialSubGroup', index: 'MaterialSubGroup' },
            { name: 'Material', index: 'Material', editable: true },
            { name: 'Units', index: 'Units', editable: true },
            { name: 'IssueQty', index: 'IssueQty', editable: true },
            { name: 'TotalPrice', index: 'TotalPrice', editable: true },
            ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'IssueDate',
        sortorder: 'desc',
        height: '130',
        //width: 1225,
        autowidth: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },

        viewrecords: true,
        caption: ' <i class="fa fa-shopping-cart"></i> Material Issue Note List',
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
            var fromDate = $("#txtFromIssueDate").val();
            var toDate = $("#txtToIssueDate").val();
            window.open("MaterialIssueNoteReportListJqGrid" + '?fromDate=' + fromDate + '&toDate=' + toDate
                + '&IssNoteNumber=' + $("#gs_IssNoteNumber").val()
                + '&RequestNumber=' + $("#gs_RequestNumber").val()
                + '&ProcessedBy=' + $("#gs_ProcessedBy").val()
                + '&IssuedBy=' + $("#gs_IssuedBy").val()
                + '&RequiredForCampus=' + $("#gs_RequiredForCampus").val()
                + '&RequiredForStore=' + $("#gs_RequiredForStore").val()
                + '&DeliveredThrough=' + $("#gs_DeliveredThrough").val()
                + '&DeliveryDetails=' + $("#gs_DeliveryDetails").val()
                + '&RequestType=' + $("#gs_RequestType").val()
                + '&RequiredForGrade=' + $("#gs_RequiredForGrade").val()
                + '&RequiredFor=' + $("#gs_RequiredFor").val() 
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
    $("#btnSearch2").click(function () {

        $(grid_selector).clearGridData();
        var fromDate = $("#txtFromIssueDate").val();
        var toDate = $("#txtToIssueDate").val();
        if (fromDate == "") {
            ErrMsg("Please select From Date");
            $("#txtFromIssueDate").focus();
            return false;
        }
        if (toDate == "") {
            ErrMsg("Please select To Date");
            $("#txtToIssueDate").focus();
            return false;
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialIssueNoteReportListJqGrid/',
                    postData: { fromDate: fromDate, toDate: toDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset2").click(function () {

        var fromDate = $("#txtFromIssueDate").val('');
        var toDate = $("#txtToIssueDate").val('');
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '~/Store/MaterialIssueNoteReportListJqGrid/',
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
