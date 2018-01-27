jQuery(function ($) {
    GetCampus();

    var grid_selector = "#StaffProgrammeMasterJqgrid";
    var pager_selector = "#StaffProgrammeMasterJqgridpager";

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
    debugger;
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/StaffProgrammeMasterJqgrid',
        datatype: 'json',
        height: 250,
        colNames: ['StaffProgrammeMatserId', 'Campus', 'Staff Type', 'Group Name','IsActive', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                          { name: 'StaffProgrammeMatserId', index: 'StaffProgrammeMatserId', sortable: false, editable: true, key: true, hidden: true },
                          {
                              name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true, }, 

                              editoptions: {
                                  style: "width: 140px; height: 25px; font-size: 0.9em" ,
                                    dataUrl: '/Base/FillAllBranchCode',
                                    buildSelect: function (data) {
                                        Campus = jQuery.parseJSON(data);
                                        var s = '<select>';
                                        s += '<option value=" ">Select</option>';
                                        if (Campus && Campus.length) {
                                            for (var i = 0, l = Campus.length; i < l; i++) {
                                                s += '<option value="' + Campus[i].Value + '">' + Campus[i].Text + '</option>';
                                            }
                                        }
                                        return s + "</select>";
                                    }
                                }, search: true,
                          },
                          { name: 'StaffType', index: 'StaffType', editable: true, edittype: "select", editoptions: { sopt: ['eq'], value: { '': 'Select', 'Teaching': 'Teaching', 'Non Teaching': 'Non Teaching' }, style: "width: 140px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                          { name: 'ProgrammeName', index: 'ProgrammeName', sortable: true, editable: true, editrules: { required: true } },
                          { name: 'IsActive', index: 'IsActive', editable: false, hidden: true, editrules: { required: true }, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                          { name: 'CreatedBy', index: 'CreatedBy', sortable: false, editable: true, hidden: true },
                          { name: 'CreatedDate', index: 'CreatedDate', sortable: false, editable: true, hidden: true },
                          { name: 'ModifiedBy', index: 'ModifiedBy', sortable: false, hidden: true, editable: true },
                          { name: 'ModifiedDate', index: 'ModifiedDate', sortable: false, hidden: true, editable: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        sortname: 'StaffProgrammeMatserId',
        sortorder: 'Asc',
        altRows: true,
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Staff Programme Master",
    });


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
                url: '/StaffManagement/SaveOrUpdateStaffProgrammeMaster', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                url: '/StaffManagement/SaveOrUpdateStaffProgrammeMaster', closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
                { url: '/StaffManagement/DeleteStaffProgrammeMaster' },
              {},
               {},
                {})



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
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }
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
        debugger;
        var Campus = $("#Campus").val();
        var StaffType = $("#ddlStaffType").val();
        var ProgrammeName = $("#txtProgrammeName").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StaffManagement/StaffProgrammeMasterJqgrid',
                    postData: { Campus: Campus, StaffType: StaffType, ProgrammeName: ProgrammeName, },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StaffManagement/StaffProgrammeMasterJqgrid',
                    postData: { Campus: $("#Campus").val(), StaffType: $("#ddlStaffType").val(), ProgrammeName: $("#txtProgrammeName").val(), },
                    page: 1
                }).trigger("reloadGrid");
    });
});
function GetCampus() {
    var ddlcam = $("#Campus");
    $.ajax({
        type: 'POST',
        async: true,
        dataType: "json",
        url: "/Base/FillBranchCode",
        success: function (data) {
            ddlcam.empty();
            ddlcam.append("<option value=''> Select </option>");
            for (var i = 0; i < data.length; i++) {
                ddlcam.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}