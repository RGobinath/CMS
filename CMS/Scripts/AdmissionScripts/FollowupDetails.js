jQuery(function ($) {

    document.getElementById('followupstats').value = $("#AdminStatus").val();

    var grid_selector = "#followup-grid-table";
    var pager_selector = "#followup-grid-pager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        //$(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
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
        //alert($(".tab-pane").width());
        //alert($(".tab-content").width());
        //alert($(".page-content").width());
        
        //alert($(".row").width());
        //alert($("#test").width());

        
        jQuery(grid_selector).jqGrid({
            url: '/Admission/followupjqgrid',
            datatype: 'json',
            colNames: ['Id', 'Remarks', 'FollowupDate'],
            colModel: [
                    { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true, editable: true },
                    { name: 'Remarks', index: 'Remarks', align: 'left', sortable: false, editable: true },
                    { name: 'FollowupDate', index: 'FollowupDate', align: 'left', sortable: false, editable: true }
            ],
            viewrecords: true,
            rowNum: 10,
            rowList: [10, 20, 30],
            pager: pager_selector,
            altRows: true,
            //autowidth: true,
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
            caption: "followup Details"
        });
    }
   

    //$(grid_selector).GridUnload();
    window.onload = loadgrid();
    

    //trigger window resize to make the grid get the correct size
    $(window).triggerHandler('resize.jqGrid');

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
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/Admission/EditFollowup/', closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {},
            { url: '/Admission/DeleteFollowupDetails/' },
            {},
            {}
        )

    //var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#followupbtn").click(function () {
        if (document.getElementById("remarks").value == "") {
            ErrMsg('Please Enter Remarks!');
            return false;
        }
        else if (document.getElementById("followupdate").value == "") {
            ErrMsg('Please Select Followup Date!');
            return false;
        }
        else {
            var objfwp = {
                Remarks: $('#remarks').val(),
                FollowupDate: $('#followupdate').val()
            };
            $.ajax({
                url: '/Admission/AddFollowupdetails/',
                type: 'POST',
                dataType: 'json',
                data: objfwp,
                traditional: true,
                success: function (data) {
                    $(grid_selector).setGridParam({ url: '/Admission/followupjqgrid/' }).trigger("reloadGrid");
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });
            document.getElementById("remarks").value = "";
            document.getElementById("followupdate").value = "";

        }
    });

    $("#updatestatus").click(function () {
        var editid = $("#editid").val();
        var preregno = $("#preregno").val();
        var objstat = {
            Id: editid,
            PreRegNum: preregno,
            AdmissionStatus: $('#followupstats').val()
        };

        window.location.href = "/Admission/EditNewRegistrationStatus?AdmissionStatus=" + $('#followupstats').val();
    });
});

function nildata(cellvalue, options, rowObject) {

    if ((cellvalue == '') || (cellvalue == null) || (cellvalue == 0)) {
        return ''
    }
    else {
        return cellvalue
    }
}

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