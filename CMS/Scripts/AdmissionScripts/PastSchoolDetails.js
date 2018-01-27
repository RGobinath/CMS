﻿jQuery(function ($) {

    var grid_selector = "#PastSchool-grid-table";
    var pager_selector = "#PastSchool-grid-pager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        //var page_width = $(grid_selector).closest('.tab-pane').width();
        var page_width = $(".page-content").width();
        page_width = page_width - 27;
        $(grid_selector).jqGrid('setGridWidth', page_width);
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
    function loadgrid() {
        jQuery(grid_selector).jqGrid({
            url: '/Admission/pastschooljqgrid',
            datatype: 'json',
            colNames: ['From Date', 'To Date', 'School Name', 'City', 'From Grade', 'To Grade', ''],
            colModel: [
                              { name: 'FromDate', index: 'FromDate', align: 'left', sortable: false, editable: true, editrules: { date: true }, formatoptions: { "newformat": "m/d/Y" } },
                              { name: 'ToDate', index: 'ToDate', align: 'left', sortable: false, editable: true, editrules: { date: true }, formatoptions: { "newformat": "m/d/Y" } },
                              { name: 'SchoolName', index: 'SchoolName', align: 'left', sortable: false, editable: true },
                              { name: 'City', index: 'City', align: 'left', sortable: false, editable: true },
                              { name: 'FromGrade', index: 'FromGrade', align: 'left', sortable: false, editable: true, edittype: 'select', editoptions: { dataUrl: '/Admission/Gradeddl', style: 'width: 150px; height: 20px; font-size: 0.9em' } },
                               { name: 'ToGrade', index: 'ToGrade', align: 'left', sortable: false, editable: true, edittype: 'select', editoptions: { dataUrl: '/Admission/Gradeddl', style: 'width: 150px; height: 20px; font-size: 0.9em' } },
                              { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true, editable: true }
            ],
            viewrecords: true,
            rowNum: 7,
            rowList: [7, 20, 30],
            pager: pager_selector,
            altRows: true,
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                $(window).triggerHandler('resize.jqGrid');
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: 'Past School Grid'
        });
    }

    window.onload = loadgrid();
    

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size


    $('.date-picker').datepicker({
        //            showOn: "button",
        //            buttonImage: "../../Images/date.gif",
        buttonImageOnly: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'yy',
        onSelect: function () {
            $('.datepicker').hide();
        },
        onClose: function (dateText, inst) {
            //   var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, 1));
        }
    });

    $("#pastschool").click(function () {
        if (document.getElementById("fromdate").value == "") {
            ErrMsg("Please Provide PastSchool Details FromDate");
            return false;
        }
        else if (document.getElementById("todate").value == "") {
            ErrMsg("Please Provide PastSchool Details ToDate");
            return false;
        }
        else if (document.getElementById("schoolname1").value == "") {
            ErrMsg("Please Provide PastSchool Name");
            return false;
        }
        else if (document.getElementById("city").value == "") {
            ErrMsg("Please Provide PastSchool City");
            return false;
        }
        else if (document.getElementById("grade1").value == "") {
            ErrMsg("Please Provide PastSchool From Grade");
            return false;
        }
        else if (document.getElementById("grade2").value == "") {
            ErrMsg("Please Provide PastSchool To Grade");
            return false;
        } else {

            var frmdate = document.getElementById("fromdate").value;
            var todate = document.getElementById("todate").value;
            var schoolname = document.getElementById("schoolname1").value;
            var city = document.getElementById("city").value;
            var fgrade = document.getElementById("grade1").value;
            var tgrade = document.getElementById("grade2").value;

            $.ajax({
                url: '/Admission/AddPastschooldetails/',
                type: 'POST',
                dataType: 'json',
                data: { frmdate: frmdate, todate: todate, schlname: schoolname, city: city, fgrade: fgrade, tgrade: tgrade },
                traditional: true,
                success: function (data) {
                    jQuery(grid_selector).setGridParam({ url: '/Admission/pastschooljqgrid' }).trigger("reloadGrid");
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });

            document.getElementById("fromdate").value = "";
            document.getElementById("todate").value = "";
            document.getElementById("schoolname1").value = "";
            document.getElementById("city").value = "";

            var g = document.getElementById('grade1');
            g.options[0].selected = true; // "Select";

            var h = document.getElementById('grade2');
            h.options[0].selected = true; // "Select";

            return true;
        }
    });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                //navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/Admission/EditPastSchool/', left: '10%', top: '10%', height: '50%', labelswidth: 60, closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {},
            { url: '/Admission/DeletePastSchoolDetails/' },
            {},
            {}
        )
    

    //var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    var startDate = new Date('01/01/1947');
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    //ToEndDate.setDate(ToEndDate.getDate() + 365);

    $('.from_date').datepicker({
        weekStart: 1,
        startDate: '01/01/1947',
        endDate: FromEndDate,
        autoclose: true
    })
    .on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('.to_date').datepicker('setStartDate', startDate);
    });
    $('.to_date')
        .datepicker({
            weekStart: 1,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        }).on('changeDate', function (selected) {
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $('.from_date').datepicker('setEndDate', FromEndDate);
        });


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