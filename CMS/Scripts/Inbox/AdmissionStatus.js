$(function () {
    var grid_selector = "#AdmissionList";
    var pager_selector = "#AdmissionListPager";
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
    var UserType = $("#UserType").val();
    $(grid_selector).jqGrid({
        url: '/Inbox/AdmissionStatusJqgrid?Id=' + Id + '&UserType=' + UserType,
        datatype: 'json',
        mtype: 'GET',
        height: '240',
        //autowidth: true,
        //shrinkToFit: true,
        colNames: ['Id', 'ApplicationNo', 'Campus', 'PreRegNum', 'Name', 'Application Role'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'ApplicationNo', index: 'ApplicationNo', sortable: true },
              { name: "Campus", index: "Campus", sortable: false },
              { name: 'PreRegNum', index: 'PreRegNum', align: 'left', sortable: true },
              { name: 'Name', index: 'Name', align: 'left', sortable: true },
              { name: 'AdmissionStatus', index: 'AdmissionStatus', align: 'left', sortable: true },
              //{ name: 'Id', index: 'Id', align: 'left', sortable: false },
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

            },
            {},
            {}, {}, {})
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
});