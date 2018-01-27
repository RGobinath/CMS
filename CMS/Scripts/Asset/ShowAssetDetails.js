$(function () {
    var AssetType = $("#viewbagAssetType").val();
    var TransactionType = $("#viewbagTransactionType").val();
    var grid_selector = "#ShowAssetDetailsList";
    var pager_selector = "#ShowAssetDetailsListPager";
    $(grid_selector).jqGrid({
        url: '/Asset/ITAssetDetailsJqgrid?AssetType=' + AssetType + '&TransactionType=' + TransactionType,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['AssetDet_Id', 'Asset Code', 'Asset Id', 'Asset Type', 'Brand', 'Model', 'Serial Number', 'Transaction Type', 'Campus', 'Block', 'Location', 'User Type', 'Name'],
        colModel: [
                      { name: 'AssetDet_Id', index: 'AssetDet_Id', hidden: true },
                      { name: 'AssetCode', index: 'AssetCode' },
                      { name: 'Asset_Id', index: 'Asset_Id', hidden: true, search: false },
                      { name: 'AssetType', index: 'AssetType' },
                      { name: 'Make', index: 'Make' },
                      { name: 'Model', index: 'Model' },
                      { name: 'SerialNo', index: 'SerialNo' },
                      { name: 'TransactionType', index: 'TransactionType' },
                      { name: 'CurrentCampus', index: 'CurrentCampus' },
                      { name: 'CurrentBlock', index: 'CurrentBlock' },
                      { name: 'CurrentLocation', index: 'CurrentLocation' },
                      { name: 'UserType', index: 'UserType' },
                      { name: 'IdNum', index: 'IdNum', sortable: true },
        ],
        pager: pager_selector,
        rowNum: '10000',
        rowList: [10000, 20000, 50000, 100000, 150000, 200000],
        sortname: 'AssetDet_Id',
        sortorder: 'Desc',
        //        width: 1250,
        autowidth: true,
        height: 300,
        viewrecords: true,
        shrinkToFit: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {

                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        //    loadError: function (xhr, status, error) {
        //    $(grid_selector).clearGridData();
        //    ErrMsg($.parseJSON(xhr.responseText).Message);
        //},
        caption: '<i class="fa fa-list"></i> Asset Details'
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
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("ITAssetDetailsJqgrid" + '?AssetType=' + AssetType + '&TransactionType=' + TransactionType + '&rows=9999999' + '&ExptXl=1');
        }
    });
    //jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
    //    caption: "<i class='fa fa-file-pdf-o'></i> Export To PDF",
    //    onClickButton: function () {
    //        //window.open("ShowCallDetailsListJqGrid1" + '?Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&Result=' + Result + '&HeaderName=' + HeaderName + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999' + '&ExptXl=1');
    //        window.open("StaffIssueManagementReportPDF" + '?Performer=' + Perfomer + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&DateType=' + DateType + '&DueDateType=' + DueDateType + '&Result=' + Result + '&CountName=' + CountName + '&Campus=' + $("#ddlCampus").val() + '&IssueType=' + $("#ddlIssueType").val() + '&IssueGroup=' + $("#ddlIssueGroup").val() + '&rows=9999');
    //        //window.open("ExportToExcel");
    //    }
    //});    
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
