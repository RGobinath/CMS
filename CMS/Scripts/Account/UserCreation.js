jQuery(function ($) {
    var grid_selector = "#StudentManagementList";
    var pager_selector = "#StudentManagementListPager";

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
        mtype: 'GET',
        url: '/Account/GetStudentListForUserCreationJqGrid',
        datatype: 'json',
        colNames: ['Id', 'Application No', 'Pre-Reg No', 'Student Name', 'Gender', 'Campus', 'Grade', 'Section', 'Is Hosteller', 'Student Id', 'Fee Structure Year', 'Admission Status', 'Academic Year'],
        colModel: [
                        { name: 'Id', index: 'Id', sortable: false, hidden: true },
                      { name: 'ApplicationNo', index: 'ApplicationNo', align: 'left' },
                      { name: 'PreRegNo', index: 'PreRegNum', align: 'left' },
                      { name: 'Name', index: 'Name', align: 'left' },
                      { name: 'Gender', index: 'Gender', align: 'left' },
                      { name: 'Campus', index: 'Campus', align: 'left' },
                      { name: 'Grade', index: 'Grade', align: 'left' },
                      { name: 'Section', index: 'Section', align: 'left' },
                      { name: 'IsHosteller', index: 'IsHosteller', align: 'left', formatter: ShowYesOrNo },
                      { name: 'NewId', index: 'NewId', align: 'left' },
                      { name: 'FeeStructYear', index: 'FeeStructYear', align: 'left' },
                      { name: 'AdmissionStatus', index: 'AdmissionStatus', align: 'left' },
                      { name: 'AcademicYear', index: 'AcademicYear', align: 'left' },
                      ],
        pager: pager_selector,
        rowNum: '100',
        rowList: [100, 150, 200, 250, 300],
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
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
        caption: 'Edit Students'

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
            }, {}, {}, {}, {})

    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Account/GetStudentListForUserCreationJqGrid/',
                    postData: { Campus: $("#Campus").val(), Grade: $("#Grade").val(), Section: $("#Section").val(), UserType: $("#UserType").val(), IsHosteller: $("#IsHosteller").val(), AcademicYear: $("#AcademicYear").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnCreate").click(function () {
        if (confirm("Are you sure to create User Ids ?")) {
            if ($("#AcademicYear").val() == '') {
                ErrMsg("Please select Academic Year");
                return false;
            }
            if ($("#UserType").val() == '') {
                ErrMsg("Please select User Type");
                return false;
            }
            $.ajax({
                type: 'POST',
                url: '/Account/UserIdCreation/',
                data: { Campus: $("#Campus").val(), Grade: $("#Grade").val(), Section: $("#Section").val(), UserType: $("#UserType").val(), IsHosteller: $("#IsHosteller").val(), AcademicYear: $("#AcademicYear").val() },
                success: function (data) {
                    InfoMsg(data);
                }
            });
        }
    });
    $("#Campus").change(function () {
        var e = document.getElementById('Campus');
        var campus = e.options[e.selectedIndex].value;
        //     alert(campus);
        $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#Grade");
                select.empty();
                select.append($('<option/>'
                               , {
                                   value: "",
                                   text: "Select Grade"
                               }));
                $.each(modelData, function (index, itemData) {

                    select.append($('<option/>',
                                  {
                                      value: itemData.gradcod,
                                      text: itemData.gradcod
                                  }));
                });
            });
    });

    $("#btnReset").click(function () {
        window.location.href = '/Account/UserCreation';
    });
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