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
        url: '/Admission/CommunicationDetailsUpdateJQGrid',
        datatype: 'json',
        type: 'POST',
        height: 320,
        colNames: ['Id', 'StudentTemplateId', 'PreRegNum', 'New Id', 'Name', 'Initial', 'Campus', 'Grade', 'Section', 'Second Language', 'Admission Status', 'Academic Year', 'General Email Id', 'Father Id', 'Father Mobile', 'Father EmailId', 'Mother Id', 'Mother Mobile', 'Mother EmailId', 'Edit'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, editable: true, width: 120, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'StudentTemplateId', index: 'StudentTemplateId', search: false, hidden: true, editable: true, width: 120, editoptions: { class: 'NoBorder'} },
            { name: 'PreRegNum', index: 'PreRegNum', search: true, hidden: true, editable: false, width: 120, editoptions: { disabled: true} },
            { name: 'NewId', index: 'NewId', search: true, editable: false, width: 120, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'Name', index: 'Name', search: true, editable: true, width: 120, editoptions: { class: 'NoBorder'} },
            { name: 'Initial', index: 'Initial', search: true, editable: true, width: 70, editoptions: { class: 'NoBorder'} },
            { name: 'Campus', index: 'Campus', search: true, editable: false, width: 120, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'Grade', index: 'Grade', search: true, width: 70, editable: false, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'Section', index: 'Section', search: true, width: 80, editable: true, sortable: true, edittype: 'text', editrules: { custom: true, custom_func: SectionValidation} },
            {
                name: 'SecondLanguage', index: 'SecondLanguage', search: true, width: 140, editable: true, sortable: true, edittype: 'select', editoptions: { dataUrl: '/Admission/getSecondLanguageMaster' },
                buildSelect: function (data) {
                    var response, s = '<select>', i;
                    response = jQuery.parseJSON(data);
                    if (response && response.length) {
                        $.each(response, function (i) {
                            s += '<option value="' + response[i].Text + '">' + response[i].Value + '</option>';
                        });
                    }
                    return s + '</select>';
                }
            },

            { name: 'AdmissionStatus', index: 'AdmissionStatus', search: true, hidden: true, editable: false, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'AcademicYear', index: 'AcademicYear', search: true, hidden: true, editable: false, editoptions: { disabled: true, border: 0} },
            { name: 'General_EmailId', index: 'General_EmailId', search: true, editable: true, edittype: 'text', editrules: { custom: true, custom_func: CheckValidEmail} },
            { name: 'Father_Id', index: 'Father_Id', search: false, hidden: true, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'Father_Mobile', index: 'Father_Mobile', search: true, editable: true, edittype: 'text', editrules: { custom: true, custom_func: MobileNumberValidation }, editoptions: { size: 10, maxlength: 10} },
            { name: 'Father_EmailId', index: 'Father_EmailId', search: true, editable: true, edittype: 'text', editrules: { custom: true, custom_func: CheckValidEmail} },
            { name: 'Mother_Id', index: 'Mother_Id', search: false, hidden: true, editable: true },
            { name: 'Mother_Mobile', index: 'Mother_Mobile', search: true, editable: true, edittype: 'text', editrules: { custom: true, custom_func: MobileNumberValidation }, editoptions: { size: 10, maxlength: 10} },
            { name: 'Mother_EmailId', index: 'Mother_EmailId', search: true, editable: true, edittype: 'text', editrules: { custom: true, custom_func: CheckValidEmail} },
            { name: 'Edit', index: 'Edit', search: false, formatoptions: { keys: false, editbutton: true, delbutton: false }, formatter: 'actions', width: 60, border: null }
            ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 25,50,100],
        pager: pager_selector,
        altRows: true,
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
        editurl: '/Admission/EditEmailDetails',
        caption: "<i class='fa fa-pencil'></i> &nbsp;Communication Details Update"
    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    function CheckValidEmail(value, column) {
        if (value == "") {
            return [value];
        }
        else if (!ValidateEmail(value)) {
            return [false, ' Is Incorrect Email ! The email you entered is not correct format.'];
        }
        else {
            return [value];
        }
    }

    function MobileNumberValidation(value, column) {
        if (value == "") {
            return [false, 'The mobile number columnn is Empty.'];
        }
        if (!$.isNumeric(value)) {
            return [false, ' Should be numeric'];
        }
        else {
            return [value];
        }
        var MobileNum = parseInt(value);
        var MobileNumberLength = MobileNum.toString().length;
        if (MobileNumberLength < 10) {
            return [false, ' Is less than 10 Digit Mobile Number!'];
        }
        else if (MobileNumberLength > 10) {
            return [false, ' Is greater than 10 Digit Mobile Number!'];
        }
        else return [MobileNum];
    }

    function SectionValidation(value, column) {
        if (value == "") {
            return [false, 'Assign any one section'];
        }
        if ($.isNumeric(value)) {
            return [false, ' Should be alphabets'];
        }
        else return [value];
    }
    function SecondLanguageValidation(value, column) {
        if (value == "") {
            return [false, 'Assign any one section'];
        }
        if ($.isNumeric(value)) {
            return [false, ' Should be alphabets'];
        }
        else return [value];
    }
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

    //var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
