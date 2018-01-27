jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";

    //resize to fit page size
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
    jQuery(grid_selector).jqGrid({
        url: '/Common/ApplicationMasterjqGrid/',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'App Code', 'App Name', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
                      { name: 'Id', index: 'Id', width: 190, key: true, hidden: true },
                      { name: 'AppCode', index: 'AppCode', width: 230, editable: true, search: true },
                      { name: 'AppName', index: 'AppName', width: 260, editable: true, search: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, editable: true, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, editable: true, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 20, editable: true, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, editable: true, hidden: true }
                     ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
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
        caption: " Application Master"
    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size


    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
        }, 0);
    }


    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
            { url: '/Common/AddApplicationMaster' }, //Edit
            {url: '/Common/AddApplicationMaster' }, //Add
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
    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        AppCode = $('#AppCode').val();
        AppName = $('#AppName').val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Common/ApplicationMasterjqGrid',
                        postData: { AppCode: AppCode, AppName: AppName },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");

        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Common/ApplicationMasterjqGrid',
                        page: 1
                    }).trigger("reloadGrid");

    });
});
