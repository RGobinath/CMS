
$(function () {

    var Studentgrid_selector = "#StudentsList";
    var Studentpager_selector = "#StudentsListPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(Studentgrid_selector).jqGrid('setGridWidth', $('#StudentMarkAnalysis').width());
    })
    //resize on sidebar collapse/expand 
    var parent_column = $(Studentgrid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(Studentgrid_selector).jqGrid('setGridWidth', parent_column.width());
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
        $(Studentgrid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    $("#StudentsList").jqGrid({
        datatype: 'local',
        type: 'GET',
        colNames: ['Id','Name', 'Campus', 'Grade', 'Section', 'Semester', 'Subject', 'Calculated'],
        colModel: [
        //if any column added need to check formateadorLink
               { name: 'Id', index: 'Id', hidden: true, key: true },
               { name: 'Name', index: 'Name' },
               { name: 'Campus', index: 'Campus' },
               { name: 'Grade', index: 'Grade' },
               { name: 'Section', index: 'Section' },
               { name: 'Semester', index: 'Semester' },
               { name: 'Subject', index: 'Subject' },
               { name: 'Calculated', index: 'Calculated' },
        ],
        pager: '#StudentsListPager',
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '320',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Student List Report',
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    //$("#StudentsList").navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' });




});