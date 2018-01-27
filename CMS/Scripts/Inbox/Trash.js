jQuery(function ($) {
    var grid_selector = "#jqGridTrashMaster";
    var pager_selector = "#jqGridTrashPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#jqGridTrashMaster").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#jqGridTrashMaster").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/Inbox/TrashJqgrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id',  'Module', 'Description', 'Status', 'Created Date','undo'],
        colModel: [
        //if any column added need to check formateadorLink
             { name: 'Id', index: 'Id', hidden: true, key: true },
          //    { name: 'IsRead', index: 'IsRead', hidden: true },
               { name: 'Module', index: 'Module', width: 40, hidden: true },
             { name: 'Description', index: 'Description' },
              { name: 'Status', index: 'Status', width: 20, search: false, align: 'center' },
                { name: 'CreatedDate', index: 'CreatedDate', width: 40, search: false, align: 'center' },
                 { name: 'Undo', index: 'Undo', width: 20, align: 'center' },
        ],
        pager: '#jqGridTrashPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 440,
        //autowidth:true,
        //shrinktofit: true,
        viewrecords: true,
        altRows: true,
        multiselect: true,
        viewrecords: true,
        loadComplete: function () {
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            $("tr.jqgrow:odd").addClass('RowBackGroundColor');
            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.IsRead == "True") {
                    $(grid_selector).setCell(ids[i], "Description", "", { "background-color": "#cccccc" });
                }
            }
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Inbox'
    });

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
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {}, {}, {
                
            }, {}, {})

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
        //
    }

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }

});
function getdata(id1) {
    window.location.href = "/inbox/Inboxdata?id=" + id1;
    //'@ViewBag.editid' == id1;
}

function ShowComments(ActivityId, UserType) {
    debugger;
    ModifiedLoadPopupDynamicaly("/Inbox/Activities?Id=" + ActivityId + "&UserType=" + UserType, $('#Activities1'),
            function () {
                LoadSetGridParam($('#ActivitiesList'), "/Inbox/ActivitiesListJqGrid?Id=" + ActivityId)
            }, function () { }, 925, 410, "ActivitiesList");
}

function LoadSetGridParam(GridId, brUrl) {
    GridId.setGridParam({
        url: brUrl,
        datatype: 'json',
        mtype: 'GET',
        page: 1
    }).trigger("reloadGrid");
}

function LoadPopupDynamicaly1(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
    if (width == undefined) { width = 800; }
    //if (ModalId.html().length == 0) {
    $.ajax({
        url: dynURL,
        type: 'GET',
        async: false,
        dataType: 'html', // <-- to expect an html response
        success: function (data) {
            ModalId.dialog({
                height: 'auto',
                width: width,
                modal: true,
                title: '<i class="fa fa-list"></i>&nbsp;<label>History</label>',
                buttons: {}
            });
            ModalId.html(data);
        }
    });
    clbPupGrdSel = onSelcalbck;
    ModalId.dialog('open');
    CallBackFunction(loadCalBack);
}
