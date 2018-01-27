jQuery(function ($) {
    var grid_selector = "#AgeCutOffMastergrid";
    var pager_selector = "#AgeCutOffMasterPager";

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
        url: '/Admission/AdmissionAgeCutOffJqgrid',
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        height: 300,
        colNames: ['Id', 'Academic Year', 'Grade', 'Normal Upto', 'CutOff Upto', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, key: true },
            { name: 'AcademicYear', index: 'AcademicYear', editable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/AcademicYearddl' }, edittype: 'select', editoptions: { dataUrl: '/Assess360/AcademicYearddl', style: "width: 170px; height: 20px; font-size: 0.9em" }, editrules: { required: true, custom: true, custom_func: checkvalid }, sortable: true },
            { name: 'Grade', index: 'Grade', editable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/GetGradeddl' }, edittype: 'select', editoptions: { dataUrl: '/Assess360/GetGradeddl', style: "width: 170px; height: 20px; font-size: 0.9em" }, editrules: { required: true, custom: true, custom_func: checkvalid} },
            { name: 'FromDate', index: 'FromDate', editable: true, editrules: { required: true }, editoptions: { dataInit: function (elem) {
                $(elem).datepicker({
                    dateFormat: "dd/mm/yy",
                    buttonImage: "../../Images/date.gif",
                    buttonImageOnly: true,
                    changeMonth: true,
                    timeFormat: 'hh:mm:ss',
                    autowidth: true,
                    changeYear: true,
                    showButtonPanel: false,
                    onSelect: function (selected) { $("#ToDate").datepicker("option", "minDate", selected) }
                }).attr('readonly', 'readonly');
            }
            }
            },
            { name: 'ToDate', index: 'ToDate', editable: true, editrules: { required: true }, editoptions: { dataInit: function (elem) {
                $(elem).datepicker({
                    dateFormat: "dd/mm/yy",
                    buttonImage: "../../Images/date.gif",
                    buttonImageOnly: true,
                    changeMonth: true,
                    timeFormat: 'hh:mm:ss',
                    autowidth: true,
                    changeYear: true,
                    showButtonPanel: false
                    // onSelect: function (selected) { $("#ToDate").datepicker("option", "minDate", selected) } 
                }).attr('readonly', 'readonly');
            }
            }
            },
            { name: 'CreatedBy', index: 'CreatedBy', search: false },
            { name: 'CreatedDate', index: 'CreatedDate', search: false },
            { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true}],
        rowNum: 20,
        rowList: [20, 50, 100, 150], // disable page size dropdown
        pgbuttons: false, // disable page control like next, back button
        pgtext: null, // disable pager text like 'Page 0 of 10'
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        viewrecords: true,
        shrinktofit: true,
        pager: pager_selector,
        altRows: true,
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
        caption: 'Age CutOff Master List'
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
                edit: false,
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
            { url: '/Admission/AddAdmissionAgeCutOff?edit=edit', modal: false, beforeSubmit: function (postdata, formid) {
                return [true, ''];
            }
            }, //Edit
            {url: '/Admission/AddAdmissionAgeCutOff', modal: false, beforeSubmit: function (postdata, formid) {
                return [true, ''];
            }
        }, //Add
              {url: '/Admission/DeleteAdmissionAgeCutOffMaster/', left: '10%', top: '10%', height: '50%', width: 400,
              beforeShowForm: function (params) { selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows} }
          }, //Delete
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



    //it causes some flicker when reloading or navigating grid
    //it may be possible to have some custom formatter to do this as the grid is being created to prevent this
    //or go back to default browser checkbox styles for the grid
    function styleCheckbox(table) {
        /**
        $(table).find('input:checkbox').addClass('ace')
        .wrap('<label />')
        .after('<span class="lbl align-top" />')
        
        
        $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
        .find('input.cbox[type=checkbox]').addClass('ace')
        .wrap('<label />').after('<span class="lbl align-top" />');
        */
    }


    //unlike navButtons icons, action icons in rows seem to be hard-coded
    //you can change them like this in here if you want
    function updateActionIcons(table) {
        /**
        var replacement = 
        {
        'ui-ace-icon fa fa-pencil' : 'ace-icon fa fa-pencil blue',
        'ui-ace-icon fa fa-trash-o' : 'ace-icon fa fa-trash-o red',
        'ui-icon-disk' : 'ace-icon fa fa-check green',
        'ui-icon-cancel' : 'ace-icon fa fa-times red'
        };
        $(table).find('.ui-pg-div span.ui-icon').each(function(){
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
        if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
        })
        */
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});