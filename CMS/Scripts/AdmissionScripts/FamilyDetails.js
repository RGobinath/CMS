jQuery(function ($) {
    
    var grid_selector = "#family-grid-table";
    var pager_selector = "#family-grid-pager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        //$(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
        var page_width = $(".page-content").width();
        page_width = page_width - 27;
        $(grid_selector).jqGrid('setGridWidth', page_width);
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        alert();
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                //$(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    function loadgrid() {
        
        jQuery(grid_selector).jqGrid({
            url: '/Admission/familyjqgrid',
            datatype: 'json',
            colNames: ['Relation', 'Name', 'Education', 'Mobile', 'Age', 'Email', 'Employee Type', 'Occupation', 'Company Name', 'Company Address', 'Staying With Child', 'Pick Up Card', 'Id'],
            colModel: [

                          { name: 'FamilyDetailType', index: 'FamilyDetailType',  align: 'left', sortable: false, editable: true, editrules: { required: true} },
                          { name: 'Name', index: 'Name',  align: 'left', sortable: false, editable: true, formatter: nildata, editrules: { required: true} },
                          { name: 'Education', index: 'Education',  align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Mobile', index: 'Mobile',  align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Age', index: 'Age', align: 'left', sortable: false, editable: true, formatter: nildata },
                           { name: 'Email', index: 'Email', align: 'left', sortable: false, editable: true, formatter: nildata },

                          { name: 'EmpType', index: 'EmpType',  align: 'left', sortable: false, editable: true, formatter: nildata, editrules: { edithidden: true }, hidden: true, edittype: 'select', editoptions: { dataUrl: '/Admission/Employeetypeddl', style: 'width: 150px; height: 20px; font-size: 0.9em'} },
                          { name: 'Occupation', index: 'Occupation',  align: 'left', sortable: false, editable: true, formatter: nildata, editrules: { edithidden: true }, hidden: true },
                          { name: 'CompName', index: 'CompName', align: 'left', sortable: false, editable: true, formatter: nildata, editrules: { edithidden: true }, hidden: true },
                          { name: 'CompAddress', index: 'CompAddress', align: 'left', sortable: false, editable: true, formatter: nildata, editrules: { edithidden: true }, hidden: true },

                          { name: 'StayingWithChild', index: 'StayingWithChild', align: 'left', sortable: false, editable: true, formatter: nildata, edittype: 'select', formatter: PickupCard, editoptions: { dataUrl: '/Admission/Stayingwithchildddl', style: 'width: 150px; height: 20px; font-size: 0.9em'} },
                          { name: 'TransportReq', index: 'TransportReq', align: 'left', sortable: false, editable: true, edittype: 'select', formatter: PickupCard, editoptions: { dataUrl: '/Admission/pickupcardddl', style: 'width: 150px; height: 20px; font-size: 0.9em'} },
                          { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true, editable: true }

                          ],
            viewrecords: true,
            rowNum: 10,
            rowList: [10, 20, 30],
            pager: pager_selector,
            altRows: true,
            //width:1229,
            //autowidth: true,
            height: 150,
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                //$(window).triggerHandler('resize.jqGrid');
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: "Family Details"
        });

    }
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    $(grid_selector).GridUnload();
    window.onload = loadgrid();
    


    $("#relationtype").change(function () {
        //          alert('hss');
        document.getElementById("name1").value = "";
        document.getElementById("name2").value = "";
        document.getElementById("mobile").value = "";
        document.getElementById("age").value = "";
        document.getElementById("email").value = "";
        document.getElementById("occupation").value = "";
        document.getElementById("compname").value = "";
        document.getElementById("compadd").value = "";
    });

    $("#family").click(function () {
        
        var e = document.getElementById("relationtype");
        var eType = document.getElementById("EmpType");
        if (e.options[e.selectedIndex].value == "") {
            ErrMsg('Please Select RelationType');
            return false;
        }
        else if (document.getElementById("name1").value == "") {
            ErrMsg('Please Enter Name');
            return false;
        }
            //            else if (document.getElementById("name2").value == "") {
            //                ErrMsg('Please Enter Education');
            //                return false;
            //            }

        else if (eType.options[eType.selectedIndex].value == "") {
            ErrMsg('Please Select Employee Type!');
            return false;
        }
        //else if ((document.getElementById('Employee_true').checked == 1) && (document.getElementById('Employee_false').checked == 2) && (document.getElementById('Employee_Others').checked == 3)) {
        //    ErrMsg("Please Select Employee Type!");
        //    return false;
        //}
        else if ((document.getElementById('StayingWithChild_true').checked == false) && (document.getElementById('StayingWithChild_false').checked == false)) {
            ErrMsg("Please Select Whether Staying With Child Or Not!");
            return false;
        }
        else if ((document.getElementById('transportreq_true').checked == false) && (document.getElementById('transportreq_false').checked == false)) {
            ErrMsg("Please Select Whether Pickup Card Required Or Not!");
            return false;
        }
        else {
            if (document.getElementById("mobile").value == "") {
                document.getElementById("mobile").value = "";
            }
            if (document.getElementById("email").value == "") {
                document.getElementById("email").value = "";
            }
            if (document.getElementById("occupation").value == "") {
                document.getElementById("occupation").value = "";
            }
            if (document.getElementById("compname").value == "") {
                document.getElementById("compname").value = "";
            }
            if (document.getElementById("compadd").value == "") {
                document.getElementById("compadd").value = "";
            }
            if (document.getElementById("compadd").value == "") {
                document.getElementById("compadd").value = "";
            }

            var name1 = document.getElementById("name1").value;
            var name2 = document.getElementById("name2").value;
            var mobile = document.getElementById("mobile").value;
            var age = document.getElementById("age").value;
            var email = document.getElementById("email").value;
            var occupation = document.getElementById("occupation").value;
            var compname = document.getElementById("compname").value;
            var compadd = document.getElementById("compadd").value;
            var e = document.getElementById("relationtype");
            var relationtype = e.options[e.selectedIndex].value;

            var et = document.getElementById("EmpType");
            var emptype = et.options[et.selectedIndex].value;
            //if (document.getElementById('Employee_true').checked == true) {
            //    emptype = "Self Employed"
            //} else if (document.getElementById('Employee_true').checked == true) {
            //    emptype = "Salaried";
            //}
            //else {
            //    emptype = "Others";
            //}

            var stayinwithchild;
            if (document.getElementById('StayingWithChild_true').checked == true) {
                stayinwithchild = true;
            } else {
                stayinwithchild = false;
            }

            var pickupcard;
            if (document.getElementById('transportreq_true').checked == true) {
                pickupcard = true;
            } else {
                pickupcard = false;
            }

            $.ajax({
                //url: '/Admission/AddFamilydetails?relationtype=' + relationtype + '&name1=' + name1 + '&name2=' + name2 + '&mobile=' + mobile + '&age=' + age + '&email=' + email + '&occupation=' + occupation + '&compn=' + compname + '&compa=' + compadd + '&emptype=' + emptype + '&stayingwithchild=' + stayingwithchild + '&pickupcard=' + pickupcard,
                url: '/Admission/AddFamilydetails/',
                type: 'POST',
                dataType: 'json',
                data: { relationtype: relationtype, name1: name1, name2: name2, mobile: mobile, age: age, email: email, occupation: occupation, compn: compname, compa: compadd, emptype: emptype, stayingwithchild: stayinwithchild, pickupcard: pickupcard },
                traditional: true,
                success: function (data) {
                    $(grid_selector).setGridParam({ url: '/Admission/familyjqgrid/' }).trigger("reloadGrid");
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });

            document.getElementById("name1").value = "";
            document.getElementById("name2").value = "";
            document.getElementById("mobile").value = "";
            document.getElementById("age").value = "";
            document.getElementById("email").value = "";
            document.getElementById("occupation").value = "";
            document.getElementById("compname").value = "";
            document.getElementById("compadd").value = "";

            document.getElementById('StayingWithChild_true').checked = false;
            document.getElementById('StayingWithChild_false').checked = false;

            document.getElementById('transportreq_true').checked = false;
            document.getElementById('transportreq_false').checked = false;

            document.getElementById('Employee_true').checked = false;
            document.getElementById('Employee_false').checked = false;
            document.getElementById('Employee_Others').checked = false;

            var g = document.getElementById('relationtype');
            g.options[0].selected = true; // "Select";

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
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/Admission/EditFamily/', closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {},
            { url: '/Admission/DeleteFamilyDetails/' },
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
        cellvalue = cellvalue.replace('&', 'and');
        return cellvalue
    }
}
function PickupCard(cellvalue, options, rowObject) {
    //      alert(cellvalue);
    if (cellvalue == 'True') {
        return 'Yes'
    }
    else if (cellvalue == 'False') {
        return 'No'
    }
    //  alert(cellvalue);
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

//var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');