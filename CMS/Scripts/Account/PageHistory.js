jQuery(function ($) {
    debugger;
    var grid_selector = "#PageHistory";
    var pager_selector = "#PageHistoryPager";

    var grid_selector1 = "#ActionHistory";
    var pager_selector1 = "#ActionHistoryPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-6").width());
        $(grid_selector1).jqGrid('setGridWidth', $(".col-sm-6").width()); //For Second Grid
    })

    jQuery(grid_selector).jqGrid({
        url: '/Account/PageHistoryListJQGrid',
        height: 250,
        autowidth: true,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'User Id', 'Session Id', 'Visited Time', 'Action', 'Controller', 'Execution Time(Milli Seconds)'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true, key: true, width: 30 },
                      { name: 'UserId', index: 'UserId', width: 30 },
                      { name: 'SessionId', index: 'SessionId', hidden: true, width: 30 },
                      { name: 'VisitedTime', index: 'VisitedTime', width: 60 },
                      { name: 'Action', index: 'Action', width: 60 },
                      { name: 'Controller', index: 'Controller', width: 30 },
                      { name: 'ExecutionTime', index: 'ExecutionTime', width: 60 },
                     ],
        viewrecords: true,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: "Page History"

    });

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
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {}, {}, {}, {})

    $(grid_selector1).jqGrid({
        url: '/Account/ActionHistoryListJQGrid',
        height: 250,
        autowidth: true,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Action', 'Controller', 'Hit Count'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true },
                      { name: 'Action', index: 'Action' },
                      { name: 'Controller', index: 'Controller' },
                      { name: 'HitCount', index: 'HitCount', sortable: false },
                     ],
        pager: pager_selector1,
        rowNum: '10',
        rowList: [ 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        viewrecords: true,
        altRows: true,
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Action Hit Count'
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
});
