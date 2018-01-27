$(function () {
    var ComposeId = $("#viewbagComposeId").val();
    var Status = $("#viewbagStatus").val();
    var grid_selector = "#ShowRecipientsDetailsList";
    var pager_selector = "#ShowRecipientsListPager";
    $(grid_selector).jqGrid({
        url: '/Communication/ShowEmailRecipientsDetailsJqgrid?ComposeId=' + ComposeId + '&Status=' + Status,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Compose Request Id', 'NewId', 'Name', 'Campus', 'Grade', 'Section', 'Fee StructYear', 'Admission Status', 'Academic Year', 'Is Hosteller', 'Route No', 'Status', 'Family Type', 'EmailId', 'Applied Date', 'Recipients Created Date', 'Recipients Modified Date'],
        colModel: [
                      { name: 'ComposeId', index: 'ComposeId', search: false },
                      { name: 'NewId', index: 'NewId', width: '125' },
                      { name: 'Name', index: 'Name', width: '250' },
                      { name: 'Campus', index: 'Campus', width: '170', search: false },
                      { name: 'Grade', index: 'Grade', width: '100' },
                      { name: 'Section', index: 'Section', width: '80' },
                      { name: 'FeeStructYear', index: 'FeeStructYear', hidden: true, },
                      { name: 'AdmissionStatus', index: 'AdmissionStatus', width: '130', search: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: '120', search: false },
                      { name: 'IsHosteller', index: 'IsHosteller', width: '90', edittype: "select", editoptions: { sopt: ['eq'], value: { '': 'Select', 'True': 'Yes', 'False': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, sortable: true, stype: 'select', search: false },
                      { name: 'VanNo', index: 'VanNo', width: '60', search: false },
                      { name: 'Status', index: 'Status', width: '95', search: false }, //,formatter: statusview,  },
                      { name: 'FamilyDetailType', index: 'FamilyDetailType', width: '70', search: false },
                      { name: 'U_EmailId', index: 'U_EmailId', search: false },
                      { name: 'AppliedDate', index: 'AppliedDate', hidden: false, search: false },
                      { name: 'RecipientsCreatedDate', index: 'RecipientsCreatedDate', hidden: true },
                      { name: 'RecipientsModifiedDate', index: 'RecipientsModifiedDate', hidden: true },
        ],
        pager: pager_selector,
        rowNum: '100',
        rowList: [100, 200, 300, 400, 500],
        sortname: 'ComposeId',
        sortorder: 'Asc',
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
        caption: '<i class="fa fa-list"></i> Email Recipients List'
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
            var colsidx = jQuery(grid_selector).jqGrid('getGridParam', 'sortname');
            var sortorder = $(grid_selector).jqGrid('getGridParam', 'sortorder');
            window.open("ShowEmailRecipientsDetailsJqgrid" + '?ComposeId=' + ComposeId +
                '&NewId=' + $("#gs_NewId").val() +
                '&Name=' + $("#gs_Name").val() +
                '&Grade=' + $("#gs_Grade").val() +
                '&Section=' + $('#gs_Section').val() +
                '&Status=' + Status +
                '&sidx=' + colsidx +
                '&sortorder=' + sortorder +
                '&rows=9999999' +
                '&ExptXl=1');
        }
    });
    jQuery(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, defaultSearch: "cn" });
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
