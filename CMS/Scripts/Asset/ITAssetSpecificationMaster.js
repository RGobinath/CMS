jQuery(function ($) {
    var grid_selector = "#ITAssetSpecificationMasterjqGrid";
    var pager_selector = "#ITAssetSpecificationMasterjqGridPager";
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
        url: '/Asset/ITAssetSpecificationjqGrid',
        datatype: 'json',
        height: 200,
        width: 1270,
        colNames: ['Id', 'Description', 'Specification', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'Spec_Id', index: 'Spec_Id', key: true, hidden: true, editable: true },
                      { name: 'Specification', index: 'Specification', editable: false, search: true, width: 80, editoptions: { style: "width: 167px; height: 20px; font-size: 0.9em" }, editrules: { custom: true, custom_func: checkvalid },hidden:true },
                      { name: 'Description', index: 'Description', editable: true, search: true, width: 80, editoptions: { style: "width: 167px; height: 20px; font-size: 0.9em" }, editrules: { custom: true, custom_func: checkvalid } },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 60, hidden: false },
                      { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true }
        ],
        sortname: 'Spec_Id',
        sortorder: 'Desc',
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Specification Master"
    });
    $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: false,
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
            {
                url: '/Asset/EditITAssetSpecificationMaster', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm) { }
            }, //Edit
            {
                url: '/Asset/AddITAssetSpecificationMaster', closeOnEscape: true, beforeShowForm: function (frm) { }
            }, //Add
              { url: '/Asset/DeleteITAssetSpecificationMaster', beforeShowForm: function (params) { var gsr = $(grid_selector).getGridParam('selrow'); var sdata = $(grid_selector).getRowData(gsr); return { Id: sdata.Id } } },
              {}, {})
    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Asset/ITAssetSpecificationjqGrid',
                        postData: { Specification: $('#txtSpecification').val() },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Asset/ITAssetSpecificationjqGrid',
                        postData: { Specification: "" },
                        page: 1
                    }).trigger("reloadGrid");

    });
    function checkvalid(value, column) {
        if (value == 'nil' || value == "") {
            return [false, column + ": Field is Required"];
        }
        else {
            return [true];
        }
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