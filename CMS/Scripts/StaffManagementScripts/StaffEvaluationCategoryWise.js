$(function () {

    var grid_selector = "#CategoryWiseMarksList";
    var pager_selector = "#CategoryWiseMarksListPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $('#grid').width());
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

    //pager icon
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
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    $(grid_selector).jqGrid({
        datatype: 'local',
        type: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Section', 'Academic Year', 'Month', 'Subject', 'Staff Name', 'Category Name', 'Marks'],
        colModel: [
                   { name: 'Id', indes: 'Id', hidden: true },
                   { name: 'Campus', indes: 'Campus', sortable: true },
                   { name: 'Grade', indes: 'Grade', sortable: true, width: 80 },
                   { name: 'Section', indes: 'Section', sortable: true, width: 80 },
                   { name: 'AcademicYear', indes: 'AcademicYear', sortable: true, width: 90 },
                   { name: 'Month', indes: 'Month', sortable: true, width: 80 },
                   { name: 'Subject', index: 'Subject', sortable: true },
                   { name: 'StaffName', index: 'StaffName', sortable: true },
                   { name: 'CategoryName', index: 'CategoryName', sortable: true },
                   { name: 'AvgScore', index: 'AvgScore', sortable: true, align: 'center', width: 80 }
        ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Asc',
        height: '320',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Category Wise Marks',
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    $(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' });
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("StaffEvaluationCategoryWiseJQGrid" + '?cam=' + $("#Campus").val() + '&gra=' + $("#Grade").val() + '&sect=' + $("#Section").val() + '&sub=' + $("#Subject").val() + '&acayear=' + $("#AcademicYear").val() + '&mon=' + $("#Month").val() + '&PreRegNum=' + $("#StaffPreRegNum").val() + '&rows=9999' + '&ExptXl=1');
        }
    });
});