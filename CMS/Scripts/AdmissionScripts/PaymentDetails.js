jQuery(function ($) {

    var grid_selector = "#payment-grid-table";
    var pager_selector = "#payment-grid-pager";

    //window.onload = $(window).triggerHandler('resize.jqGrid');
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
        jQuery(grid_selector).jqGrid({
            url: '/Admission/paymentjqgrid?preregno=' + $('#PreRegNum').val(),
            datatype: 'json',
            colNames: ['Fee Type', 'Mode Of Payment', 'Cheque Date', 'Reference No', 'Bank Name', 'Amount', 'Remarks', 'Paid Date', 'Cleared Date', 'Fee Status','','','','',''],
            colModel: [{ name: 'FeeType', index: 'FeeType', align: 'left', sortable: false, editable: true, width: 200 },
                       { name: 'ModeOfPayment', index: 'ModeOfPayment', align: 'left', sortable: false, editable: true, edittype: 'select', editoptions: { dataUrl: '/Admission/Modeofpmtddl', style: 'width: 150px; height: 20px; font-size: 0.9em'} },
                       { name: 'ChequeDate', index: 'ChequeDate', align: 'left', sortable: false, editable: true, editoptions: { dataInit: function (elem) {
                           $(elem).datepicker({
                               format: "dd/mm/yyyy",
                               autoclose: true
                           }).attr('readonly', 'readonly');
                       }
                       }
                       },
                       { name: 'ReferenceNo', index: 'ReferenceNo', align: 'left', sortable: false, editable: true, formatter: nildata },
                       { name: 'BankName', index: 'BankName', align: 'left', sortable: false, editable: true, edittype: 'select', editoptions: { dataUrl: '/Base/Bankmasterddl', style: 'width: 150px; height: 20px; font-size: 0.9em'} },
                       { name: 'Amount', index: 'Amount', align: 'left', sortable: false, editable: true },
                       { name: 'Remarks', index: 'Remarks', align: 'left', sortable: false, editable: true },
                       { name: 'PaidDate', index: 'PaidDate', align: 'left', sortable: false, editable: true, editoptions: { dataInit: function (elem) {
                           $(elem).datepicker({
                               format: "dd/mm/yyyy",
                               autoclose: true
                           }).attr('readonly', 'readonly');
                       }
                       }
                       },
                       { name: 'ClearedDate', index: 'ClearedDate', align: 'left', sortable: false, editable: true, editoptions: { dataInit: function (elem) {
                           $(elem).datepicker({
                               format: "dd/mm/yyyy",
                               autoclose: true
                           }).attr('readonly', 'readonly');}}},
                       { name: 'FeePaidStatus', index: 'FeePaidStatus', align: 'left', sortable: false, editable: true, edittype: 'select', editoptions: { sopt: ["eq", "ne"], value: ":Select;InProgress:InProgress;FeePaid:FeePaid"} },
                       { name: 'CreatedBy', index: 'CreatedBy', hidden: true, editable: false},
                       { name: 'CreatedDate', index: 'CreatedDate',hidden: true, editable: false},
                       { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true, editable: false },
                       { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true, editable: false },
                       { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true, editable: true }
                       ],
            viewrecords: true,
            rowNum: 10,
            //autowidth: true,
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
            caption: "Payment Details"
        });

    }
    //        $(grid_selector).GridUnload();
    window.onload = loadgrid();

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    $('#feetypeddl').change(function () {
        var feedetails = $('#feetypeddl').val();
        if (feedetails == "Pre-Registration Amount") {
            $('#Amount').val('500');
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
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/Admission/EditPaymentDetails/', closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {},
            { url: '/Admission/DeletePaymentDetails/' },
            {},
            {}
        )

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
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

function Paymentvalidate() {
    var feetype = document.getElementById("feetypeddl").value;
    var modofpmnt = document.getElementById("modeofpmtddl").value;
    var refno = document.getElementById("ReferenceNo").value;
    var amt = document.getElementById("Amount").value;
    var pdate = document.getElementById("PaidDate").value;
    var bank = document.getElementById("BankName").value;
    if (feetype == "") {
        ErrMsg("Please Select Fee Type!");
        return false;
    }
    else if (modofpmnt == "") {
        ErrMsg("Please Select Mode Of Payment!");
        return false;
    }
    else if ((modofpmnt != "Cash") && (refno == "")) {
        ErrMsg("Please Enter Reference No.!");
        return false;
    }
    else if ((modofpmnt != "Cash") && (bank == "")) {
        ErrMsg("Please Select Bank.!");
        return false;
    }
    else if (amt == "") {
        ErrMsg("Please Enter The Amount!");
        return false;
    }
    else if (pdate == "") {
        ErrMsg("Please select Paid date!");
        return false;
    }
    else {
        if (document.getElementById("ReferenceNo").value == "") {
            refno = "";
        }
        return true;
    }
}

function getpastschooldata(id1) {

    var grid = jQuery('#list5');
    var sel_id = grid.jqGrid('getGridParam', 'selrow');

    var fromdate = grid.jqGrid('getCell', sel_id, 'FromDate');
    var todate = grid.jqGrid('getCell', sel_id, 'ToDate');
    var schoolname = grid.jqGrid('getCell', sel_id, 'SchoolName');
    var City = grid.jqGrid('getCell', sel_id, 'City');
    var GradeStudied = grid.jqGrid('getCell', sel_id, 'GradeStudied');
    var Id = grid.jqGrid('getCell', sel_id, 'Id');

    document.getElementById("fromdate").value = fromdate;
    document.getElementById("todate").value = todate;
    document.getElementById("schoolname1").value = schoolname;
    document.getElementById("city").value = City;
    document.getElementById("grade1").value = GradeStudied;
}

function validate1() {
    if (document.getElementById("fromdate").value == "") {
        ErrMsg("Please Provide PastSchool Details FromDate");
        return false;
    }
    else if (document.getElementById("todate").value == "") {
        ErrMsg("Please Provide PastSchool Details ToDate");
        return false;
    }
    else {
        return true;
    }
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