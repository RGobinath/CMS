$(function () {
    var grid_selector = "#ViewFAHWMarksList";
    var pager_selector = "#ViewFAHWMarksListPager";
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
    $(grid_selector).jqGrid({
        url: '/Assess360/ViewFAHWMarksListJqGrid?Id=' + Id,
        datatype: 'json',
        mtype: 'GET',
        height: '240',
        //autowidth: true,
        //shrinkToFit: true,
        colNames: ['Id', 'AssessId', 'IdNo', 'Name', 'Campus', 'AcademicYear', 'Section', 'Grade', 'Subject', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA', 'HW', 'FA','HW'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'AssessId', index: 'AssessId', hidden: true },
              { name: 'IdNo', index: 'IdNo', hidden: true },
              { name: 'Name', index: 'Name', hidden: true },
              { name: 'Campus', index: 'Campus', hidden: true },
              { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
              { name: 'Section', index: 'Section', hidden: true },
              { name: 'Grade', index: 'Grade', hidden: true },
              { name: 'Subject', index: 'Subject' },
              { name: 'FAJan', index: 'FAJan', width: 60 },
              { name: 'HWJan', index: 'HWJan', width: 60 },
              { name: 'FAFeb', index: 'FAFeb', width: 60 },
              { name: 'HWFeb', index: 'HWFeb', width: 60 },
              { name: 'FAMar', index: 'FAMar', width: 60 },
              { name: 'HWMar', index: 'HWMar', width: 60 },
              { name: 'FAApr', index: 'FAApr', width: 60 },
              { name: 'HWApr', index: 'HWApr', width: 60 },
              { name: 'FAJun', index: 'FAJun', width: 60 },
              { name: 'HWJun', index: 'HWJun', width: 60 },
              { name: 'FAJul', index: 'FAJul', width: 60 },
              { name: 'HWJul', index: 'HWJul', width: 60 },
              { name: 'FAAug', index: 'FAAug', width: 60 },
              { name: 'HWAug', index: 'HWAug', width: 60 },
              { name: 'FASep', index: 'FASep', width: 60 },
              { name: 'HWSep', index: 'HWSep', width: 60 },
              { name: 'FAOct', index: 'FAOct', width: 60 },
              { name: 'HWOct', index: 'HWOct', width: 60 },
              { name: 'FANov', index: 'FANov', width: 60 },
              { name: 'HWNov', index: 'HWNov', width: 60 },
              { name: 'FADec', index: 'FADec', width: 60 },
              { name: 'HWDec', index: 'HWDec', width: 60 },
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: "Desc",
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>FA Mark List',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(grid_selector).jqGrid('setGridWidth');
        }
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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

            }, {}, {}, {}, {});
    jQuery(grid_selector).jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
         { startColumnName: 'FAJan', numberOfColumns: 2, titleText: '<em>JAN<em>' },
         { startColumnName: 'FAFeb', numberOfColumns: 2, titleText: '<em>FEB</em>' },
         { startColumnName: 'FAMar', numberOfColumns: 2, titleText: '<em>MAR</em>' },
         { startColumnName: 'FAApr', numberOfColumns: 2, titleText: '<em>APR</em>' },
         { startColumnName: 'FAJun', numberOfColumns: 2, titleText: '<em>JUN</em>' },
         { startColumnName: 'FAJul', numberOfColumns: 2, titleText: '<em>JUL</em>' },
         { startColumnName: 'FAAug', numberOfColumns: 2, titleText: '<em>AUG</em>' },
         { startColumnName: 'FASep', numberOfColumns: 2, titleText: '<em>SEP</em>' },
         { startColumnName: 'FAOct', numberOfColumns: 2, titleText: '<em>OCT</em>' },
         { startColumnName: 'FANov', numberOfColumns: 2, titleText: '<em>NOV</em>' },
         { startColumnName: 'FADec', numberOfColumns: 2, titleText: '<em>DEC</em>' },
        ]
    });

    
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
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
