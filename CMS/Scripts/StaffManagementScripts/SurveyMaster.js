
//--Grid Loading
jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({

        url: '/StaffManagement/SurveyMasterGridLoad',
        datatype: 'json',
        height: 300,
        mtype: 'GET',

        colNames: ['SurveyId', 'Survey Name', 'Is Active', 'Created By', 'Created Date'],
        colModel: [
                                  { name: 'SurveyId', index: 'SurveyId', hidden: true, editable: true, key: true },
                                  { name: 'SurveyName', index: 'SurveyName', sortable: true, editable: true, editrules: { required: true } },
                                  { name: 'IsActive', index: 'IsActive', editable: false },
                                  { name: 'CreatedBy', index: 'CreatedBy', sortable: true },
                                  { name: 'CreatedDate', index: 'CreatedBy', sortable: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'SurveyId',
        sortorder: 'desc',
        altRows: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Survey Master",
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {
                width: 'auto', url: '/StaffManagement/EditSurveyMaster', modal: false, closeAfterEdit: true
            }, //Edit
            {
                width: 'auto', url: '/StaffManagement/AddSurveyMaster', modal: false, closeAfterAdd: false
            }, //Add
            {
                width: 'auto', url: '/StaffManagement/DeleteSurveyMaster', beforeShowForm: function (params) {
                selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                return { Id: selectedrows }
                }
            },
            {},
            {});
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


    $('#btnSearch').click(function () {
       $(grid_selector).setGridParam(
                      {
                          datatype: "json",
                          url: "/StaffManagement/SurveyMasterGridLoad",
                          postData: { SurveyName: $('#txtSurveyName').val(), IsActive: $('#ddlIsActive').val() },
                          page: 1
                      }).trigger('reloadGrid');
    });

    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        reloadGrid();
    });

});


function reloadGrid() {
    $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/StaffManagement/SurveyMasterGridLoad',
             postData: { SurveyName: $('#txtSurveyName').val(), IsActive: $('#ddlIsActive').val() },
             page: 1
         }).trigger("reloadGrid");
}






