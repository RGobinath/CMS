$(function () {
    var grid_selector = "#ActivitiesHstryList";
    var pager_selector = "#ActivitiesHstryPager";

    $(grid_selector).jqGrid({
        datatype: 'local',
        mtype: 'GET',
        height: '300',
        //width: 'auto',
        autowidth: true,
        //shrinkToFit: false,
        colNames: ['Id', 'Activity Name', 'Status', 'Performer', 'Created Date', 'Application Role', 'Reference Id'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'ActivityFullName', index: 'ActivityFullName', sortable: true },
              { name: "Status", index: "Status", sortable: false },
              { name: 'Performer', index: 'Performer', align: 'left', sortable: true },
              { name: 'CreatedDate', index: 'CreatedDate', align: 'left', sortable: true },
              { name: 'AppRole', index: 'AppRole', align: 'left', sortable: true },
              { name: 'Id', index: 'Id', align: 'left', sortable: false },
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: "Desc",
        viewrecords: true,
        caption: 'Activities List',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
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
            {}, {},
            {})
    function updatePagerIcons(table) {
        var replacement = {
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
});