 var grid_selector = "#jqGridStudentIssueDetailsList";
var Pager_selector = "#jqGridStudentIssueDetailsListPager";

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
$(function () {
    $(grid_selector).jqGrid({
        url: '/Store/GetJqGridMaterialIssueDetailsList?StudId=' + $("#hdnstudid").val(),
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['MaterialviewId', 'Academic Year', 'Campus', 'Grade', 'Section', 'StudId', 'NewId', 'Name', 'Gender', 'IsHosteller', 'Material', 'Quantity', 'MaterialSubGroupId', 'Size', 'IssueId', 'StudentId', 'MaterialId', 'Issued Qty', 'Received Qty', 'Pending Items', 'Extra Qty', 'TotalQty', 'MaterialDistributionId'],
        colModel: [
                    { name: 'MaterialviewId', index: 'MaterialviewId', key: true, hidden: true, editable: true },
                    //{ name: 'CategoryNameLink', index: 'CategoryNameLink', sortable: true, editable: false, search: true, hidden: false },
                    { name: 'AcademicYear', index: 'AcademicYear', sortable: true, editable: true, search: true, hidden: true },
                    { name: 'Campus', index: 'Campus', sortable: true, editable: false, search: true, hidden: true, editrules: { required: true } },
                    { name: 'Grade', index: 'Grade', sortable: true, editable: false, search: true, hidden: true, editrules: { required: true } },
                    { name: 'Section', index: 'Section', sortable: true, editable: false, search: true, hidden: true, editrules: { required: true } },
                    { name: 'StudId', index: 'StudId', sortable: true, editable: false, search: true, hidden: true, editrules: { required: true } },
                    { name: 'NewId', index: 'NewId', sortable: true, editable: false, hidden: true, search: true, editrules: { required: true } },
                    { name: 'Name', index: 'Name', sortable: false, editable: false, hidden: true },
                    { name: 'Gender', index: 'Gender', sortable: false, editable: false, hidden: true },
                    { name: 'IsHosteller', index: 'IsHosteller', sortable: false, editable: false, hidden: true },
                    { name: 'MaterialSubGroup', index: 'MaterialSubGroup', sortable: false, editable: false },
                    { name: 'Quantity', index: 'Quantity', sortable: true, editable: false, search: true, hidden: true, editrules: { required: true } },
                    { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', sortable: false, editable: false, hidden: true },
                    { name: 'Material', index: 'Material', sortable: false, editable: false },
                    { name: 'IssueId', index: 'IssueId', sortable: false, editable: false, hidden: true },
                    { name: 'StudentId', index: 'StudentId', sortable: false, editable: false, hidden: true },
                    { name: 'MaterialId', index: 'MaterialId', sortable: false, editable: false, hidden: true },
                    { name: 'IssuedQty', index: 'IssuedQty', sortable: false, editable: false },
                    { name: 'ReceivedQty', index: 'ReceivedQty', sortable: false, editable: false,hidden:true },
                    { name: 'PendingItems', index: 'PendingItems', sortable: false, editable: false,hidden:true },
                    { name: 'ExtraQty', index: 'ExtraQty', sortable: false, editable: false },
                    { name: 'TotalQty', index: 'TotalQty', sortable: false, editable: false },
                    { name: 'MaterialDistributionId', index: 'MaterialDistributionId', sortable: false, editable: false,hidden:true },
        ],

        viewrecords: true,
        altRows: true,
        autowidth: true,
        multiselect: true,
        // multiboxonly: true,
        height: '220',
        rowNum: 1000,
        rowList: [5, 10, 20],
        sortName: 'Id',
        sortOrder: 'Asc',
        pager: Pager_selector,

        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },

        caption: '<i class="fa fa-th-list"></i>&nbsp; Meterial Issue Details'
    });
    $("#btnBack").click(function () {
        window.location.href = "/Store/StudentMaterialDistribution";
    });

    jQuery(grid_selector).jqGrid('navGrid', Pager_selector,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {})    
    
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
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}

