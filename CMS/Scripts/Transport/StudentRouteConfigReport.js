jQuery(function ($) {
    var grid_selector = "#StudentRouteConfigReport";
    var pager_selector = "#StudentRouteConfigReportPager";

    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
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
        url: '/Transport/StudentRouteConfigReportJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id','PreRegNum','Name','Grade','Section','LocationName'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'PreRegNum', index: 'PreRegNum', editable: false, search: false, hidden: true },
            { name: 'Name', index: 'Name', editable: true },
            { name: 'Grade', index: 'Grade', editable: true },
            { name: 'Section', index: 'Section', editable: false, search: true },
             { name: 'LocationName', index: 'LocationName', editable: false, search: true, hidden: false }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortname: 'Id',
        sortorder: 'Asc',
        autowidth: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-th-list'></i>&nbsp;Students List"
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
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
            {}, //Edit
            {}, //Add
              {
              width: 'auto', url: '/Transport/DeleteRouteConfiguration/', left: '10%', top: '10%', height: '50%', width: 400,
              beforeShowForm: function (params) { selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows} }
          }, {}, {})
    // For Pager Icons
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }

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
function gradeddl() {
    var e = document.getElementById('ddlcampus');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { Campus: campus },
            function (modelData) {
                var select = $("#ddlgrade");
                select.empty();
                select.append($('<option/>', { value: "", text: "Select Grade" }));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                });
            });
}