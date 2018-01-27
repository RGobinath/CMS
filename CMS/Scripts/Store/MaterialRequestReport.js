var grid_selector = "#MaterialRequestReportList";
var pager_selector = "#MaterialRequestReportListPager";

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

$(document).ready(function () {

    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialRequestReportListJqGrid',
        datatype: 'json',
        mtype: 'POST',
        colNames: ['Id', 'Request Number', 'Required For Campus', 'Processed By', 'Requested Date', 'Request Type', 'Required For Grade', 'Required For',
             'Material Group', 'Material SubGroup', 'Material', 'Units', 'Required Date', 'Req.Qty', 'App.Qty', 'Issd.Qty', 'Status'],
        colModel: [
            { name: 'Id', index: 'SkuId', hidden: true, key: true, editable: true },
            { name: 'RequestNumber', index: 'RequestNumber', editable: true },
            { name: 'RequiredForCampus', index: 'RequiredForCampus', editable: true },
            { name: 'ProcessedBy', index: 'ProcessedBy', editable: true },
            { name: 'RequestedDate', index: 'RequestedDate', width: 240, editable: true },
            { name: 'RequestType', index: 'RequestType', editable: true, stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: ":Select;Student:Student;General:General" } },
            { name: 'RequiredForGrade', index: 'RequiredForGrade', editable: true },
            { name: 'RequiredFor', index: 'RequiredFor', editable: true },
            { name: 'MaterialGroup', index: 'MaterialGroup' },
            { name: 'MaterialSubGroup', index: 'MaterialSubGroup' },
            { name: 'Material', index: 'Material', editable: true },
            { name: 'Units', index: 'Units', editable: true },
            { name: 'RequiredDate', index: 'RequiredDate', width: 240, editable: true },
            { name: 'Quantity', index: 'Quantity', editable: true },
            { name: 'ApprovedQty', index: 'ApprovedQty', editable: true },
            { name: 'IssuedQty', index: 'IssuedQty', editable: true },
            { name: 'Status', index: 'Status', editable: true },
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'RequestedDate',
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
        caption: '<i class="fa fa-hand-o-up"></i> Material Request List',
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
            var fromDate = $("#txtFromReqDate").val();
            var toDate = $("#txtToReqDate").val();
            window.open("MaterialRequestReportListJqGrid" + '?fromDate=' + fromDate + '&toDate=' + toDate +
                '&RequestNumber=' + $("#gs_RequestNumber").val() +
                '&RequiredForCampus=' + $("#gs_RequiredForCampus").val() +
                '&ProcessedBy=' + $("#gs_ProcessedBy").val() +
                '&RequestType=' + $("#gs_RequestType").val() +
                '&RequiredForGrade=' + $("#gs_RequiredForGrade").val() +
                '&RequiredFor=' + $("#gs_RequiredFor").val() +
                '&MaterialGroup=' + $("#gs_MaterialGroup").val() +
                '&MaterialSubGroup=' + $("#gs_MaterialSubGroup").val() +
                '&Material=' + $("#gs_Material").val() +
                '&Units=' + $("#gs_Units").val() +
                '&Quantity=' + $("#gs_Quantity").val() +
                '&ApprovedQty=' + $("#gs_ApprovedQty").val() +
                '&IssuedQty=' + $("#gs_IssuedQty").val() +
                '&Status=' + $("#gs_Status").val() +
                '&rows=9999' + '&Expt=Excel');
        }
    });
    $(grid_selector).jqGrid('filterToolbar', {
        stringResult: false, searchOnEnter: true, beforeSearch: function () {
            $(grid_selector).clearGridData();
            return false;
        }
    });
    $("#btnSearch1").click(function () {

        $(grid_selector).clearGridData();
        var fromDate = $("#txtFromReqDate").val();
        var toDate = $("#txtToReqDate").val();
        if (fromDate == "") {
            ErrMsg("Please select From Date");
            $("#txtFromReqDate").focus();
            return false;
        }
        if (toDate == "") {
            ErrMsg("Please select To Date");
            $("#txtToReqDate").focus();
            return false;
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialRequestReportListJqGrid/',
                    postData: { fromDate: fromDate, toDate: toDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset1").click(function () {
        var fromDate = $("#txtFromReqDate").val('');
        var toDate = $("#txtToReqDate").val('');
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialRequestReportListJqGrid/',
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