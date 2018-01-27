jQuery(function ($) {
    //debugger;
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

        url: '/HostelManagement/HostelMasterPageJqgrid/',
        datatype: 'json',
        mtype: 'GET',
        shrinkToFit: true,
        height: 250,
        colNames: ['Id', 'Hostel Name', 'Hostel Type', 'Floor', 'Campus', 'InCharge'],
        colModel: [
                { name: 'HstlMst_Id', index: 'HstlMst_Id', width: 230, editable: true, hidden: true, key: true },
                { name: 'HostelName', index: 'HostelName', width: 260, editable: true },
                { name: 'HostelType', index: 'HostelType', width: 190, editable: true },
                 { name: 'Floor', index: 'Floor', width: 260, editable: true },
                //{ name: 'Campus', index: 'Campus', width: 260, editable: true },
                {
                    name: 'Campus', index: 'Campus',
                    //hidden: true,
                    viewable: true,
                   // editrules: { edithidden: true },
                    editable: true,
                    //stype: 'select',
                    edittype: 'select',
                    editoptions: {
                        dataUrl: "/Base/FillBranchCode",
                        buildSelect: function (data) {
                            debugger;
                            var response, s = '<select>', i = 0;
                            response = jQuery.parseJSON(data);
                            if (response && response.length) {
                                $.each(response, function (i) {
                                    s += '<option value="' + this.Value + '">' + this.Text + '</option>';
                                });
                            }
                            return s + '</select>';
                        },
                    }
                },
{ name: 'InCharge', index: 'InCharge', width: 190, hidden: true, editable: true }
        ],
        viewrecords: true,
        rowNum: 20,
        rowList: [20, 50, 100, 200, 500],
        pager: pager_selector,
        height: 300,
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
        caption: "&nbsp;Hostel Master",
        subGrid: true,
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (subgrid_id, ParentId) {
            var subgrid_table_id, pager_id;
            subgrid_table_id = subgrid_id + "_t";
            pager_id = "p_" + subgrid_table_id;
            $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: '/HostelManagement/RoomMasterPageJqgrid?hostelId=' + ParentId,
                datatype: "json",
                colNames: ['Id', 'HstlMst_Id', 'Room Number', 'Created By'],
                colModel: [
        { name: 'RoomMst_Id', index: 'RoomMst_Id', width: 290, hidden: true, key: true },
        { name: 'HstlMst_Id', index: 'HstlMst_Id', width: 290, editable: true, hidden: true },
        { name: 'RoomNumber', index: 'RoomNumber', width: 310, editable: true },
        { name: 'CreatedBy', index: 'CreatedBy', width: 300, editable: true, hidden: true }
                ],
                width: 1000,
                altRows: true,
                viewrecords: true,
                shrinkToFit: true,
                rowNum: 5,
                rowList: [5, 10, 20, 30],
                pager: pager_id,
                sortname: 'num',
                sortorder: "asc",
                height: 200,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                },
                multiselect: true,
                subGrid: true,
                subGridOptions: {
                    plusicon: "ace-icon fa fa-plus center bigger-110 blue",
                    minusicon: "ace-icon fa fa-minus center bigger-110 blue",
                    openicon: "ace-icon fa fa-chevron-right center orange",
                    // load the subgrid data only once // and the just show/hide 
                    "reloadOnExpand": false,
                    // select the row when the expand column is clicked 
                    "selectOnExpand": true
                },
                subGridRowExpanded: function (subgrid_id2, ParentId) {
                    var subgrid_table_id2, pager_id;
                    subgrid_table_id2 = subgrid_id2 + "_t";
                    pager_id = "p_" + subgrid_table_id2;
                    $("#" + subgrid_id2).html("<table id='" + subgrid_table_id2 + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
                    jQuery("#" + subgrid_table_id2).jqGrid({
                        url: '/HostelManagement/BedMasterPageJqgrid?RoomMst_Id=' + ParentId,
                        datatype: "json",
                        colNames: ['Id', 'BedMst_Id', 'Bed Number', 'Created By'],
                        colModel: [
                { name: 'BedMst_Id', index: 'BedMst_Id', width: 290, editable: true, hidden: true, key: true },
                { name: 'RoomMst_Id', index: 'RoomMst_Id', width: 290, hidden: true, hidden: true },
                { name: 'BedNumber', index: 'BedNumber', width: 310, editable: true },
                { name: 'CreatedBy', index: 'CreatedBy', width: 300, editable: true, hidden: true }
                        ],
                        width: 930,
                        altRows: true,
                        viewrecords: true,
                        shrinkToFit: true,
                        rowNum: 5,
                        rowList: [5, 10, 20, 30],
                        pager: pager_id,
                        sortname: 'num',
                        sortorder: "asc",
                        height: 200,
                        loadComplete: function () {
                            var table = this;
                            setTimeout(function () {
                                updatePagerIcons(table);
                                enableTooltips(table);
                            }, 0);
                        },
                        multiselect: true
                    });
                    jQuery("#" + subgrid_table_id2).jqGrid('navGrid', "#" + pager_id, { 	//navbar options
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
                    //Edit options
                    { width: 'auto', url: '/HostelManagement/AddThirdLevelMenus?ids=' + ParentId, closeAfterEdit: true, left: '10%', top: '10%', height: '50%', width: 400 },
                    //add options
                    { width: 'auto', url: '/HostelManagement/AddThirdLevelMenus?ids=' + ParentId, closeAfterAdd: true, left: '10%', top: '10%', height: '50%', width: 400 },
                    // Delete options
                    { width: 'auto', url: '/HostelManagement/DeleteThirdLevelSubMenus/', closeAfterDelete: true, left: '10%', top: '10%', height: '50%', width: 400, beforeShowForm: function (params) { var gsr = $(grid_selector).getGridParam('selrow'); var sdata = $(grid_selector).getRowData(gsr); return { Id: sdata.Id } } }
                    );
                }
            });

            jQuery("#" + subgrid_table_id).jqGrid('navGrid', "#" + pager_id, { 	//navbar options
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
            //Edit options
                    { width: 'auto', url: '/HostelManagement/AddSubMenus?ids=' + ParentId, closeAfterEdit: true, left: '10%', top: '10%', height: '50%', width: 400 },
            //add options
        { width: 'auto', url: '/HostelManagement/AddSubMenus?ids=' + ParentId, closeAfterAdd: true, left: '10%', top: '10%', height: '50%', width: 400 },
            // Delete options
                    {
                        width: 'auto', url: '/HostelManagement/DeleteSubMenus/',
                        closeAfterDelete: true,
                        left: '10%', top: '10%', height: '50%', width: 400,
                        beforeShowForm: function (params) {
                            var gsr = $(grid_selector).getGridParam('selrow');
                            var sdata = $(grid_selector).getRowData(gsr);
                            return { Id: sdata.Id }
                        },
                    }
                    );
        }
    });


    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
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
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/HostelManagement/AddMenu?edit=edit', closeAfterEdit: true }, //Edit
            { url: '/HostelManagement/AddMenu', closeAfterAdd: true }, //Add
              { url: '/HostelManagement/DeleteMenu/', closeAfterDelete: true, beforeShowForm: function (params) { var gsr = $(grid_selector).getGridParam('selrow'); var sdata = $(grid_selector).getRowData(gsr); return { Id: sdata.Id } } }, //Delete 
              {}, {})



    function style_edit_form(form) {
        //enable datepicker on "sdate" field and switches for "stock" field
        form.find('input[name=sdate]').datepicker({ format: 'yyyy-mm-dd', autoclose: true })
                .end().find('input[name=stock]')
                    .addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
        //don't wrap inside a label element, the checkbox value won't be submitted (POST'ed)
        //.addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');

        //update buttons classes
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-primary').prepend('<i class="ace-icon fa fa-check"></i>');
        buttons.eq(1).prepend('<i class="ace-icon fa fa-times"></i>')

        buttons = form.next().find('.navButton a');
        buttons.find('.ui-icon').hide();
        buttons.eq(0).append('<i class="ace-icon fa fa-chevron-left"></i>');
        buttons.eq(1).append('<i class="ace-icon fa fa-chevron-right"></i>');
    }

    function style_delete_form(form) {
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm btn-white btn-round').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-danger').prepend('<i class="ace-icon fa fa-trash-o"></i>');
        buttons.eq(1).addClass('btn-default').prepend('<i class="ace-icon fa fa-times"></i>')
    }

    function style_search_filters(form) {
        form.find('.delete-rule').val('X');
        form.find('.add-rule').addClass('btn btn-xs btn-primary');
        form.find('.add-group').addClass('btn btn-xs btn-success');
        form.find('.delete-group').addClass('btn btn-xs btn-danger');
    }
    function style_search_form(form) {
        var dialog = form.closest('.ui-jqdialog');
        var buttons = dialog.find('.EditTable')
        buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
        buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
        buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
    }

    function beforeDeleteCallback(e) {
        var form = $(e[0]);
        if (form.data('styled')) return false;

        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_delete_form(form);

        form.data('styled', true);
    }

    function beforeEditCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_edit_form(form);
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

    //var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $("#btnSearch").click(function () {
        debugger;
        jQuery(grid_selector).clearGridData();
        MenuName = $('#MenuName').val();
        Role = $('#Role').val();


        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Common/Menujqgrid',
                        postData: { MenuName: MenuName, Role: Role },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");

        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Common/Menujqgrid',
                        postData: { MenuName: MenuName, Role: Role },
                        page: 1
                    }).trigger("reloadGrid");

    });
});