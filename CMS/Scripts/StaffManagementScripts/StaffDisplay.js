jQuery(function ($) {
    var campus = "";
    var designation = "";
    var department = "";
    var appname = "";
    var idno = "";
    var stat = "";

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
        mtype: 'GET',
        height: 250,
        url: '/StaffManagement/StaffListJqGrid',
        postData: { campus: campus, designation: designation, department: department, stat: stat, appname: appname, idno: idno, type: 'old' },
        datatype: 'json',
        colNames: ['Request No', 'Name', 'Id Number', 'Campus', 'Designation', 'Department', 'Gender', 'Status', 'Id'],
        colModel: [
              { name: 'PreRegNum', index: 'PreRegNum', width: 30, align: 'left' },
              { name: 'Name', index: 'Name', width: 80, align: 'left' },
              { name: 'IdNumber', index: 'IdNumber', width: 60, align: 'left' },
              { name: 'Campus', index: 'Campus', width: 50, align: 'left', sortable: true },
              { name: 'Designation', index: 'Designation', width: 50, align: 'left', sortable: true },
              { name: 'Department', index: 'Department', width: 50, align: 'left', sortable: true },
              { name: 'Gender', index: 'Gender', width: 30, align: 'left', sortable: true },
              { name: 'Status', index: 'Status', width: 50, align: 'left', sortable: true, formatter: ChangetoActive },
               { name: 'Id', index: 'Id', width: '30%', align: 'left', sortable: false, hidden: true, key: true }
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        multiselect: true,
        multiboxonly: true,
        viewrecords:true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-inbox'></i>&nbsp;Inbox"
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

    function ChangetoActive(cellvalue, options, rowObject) {

        if (cellvalue == 'Registered') {
            return 'Active'
        }
        else {
            return cellvalue
        }
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


    // written by felix kinoniya

    $("#ddlcampus").change(function () {

        if ($("#ddlcampus").val() == "") {

            var designation = $("#designation");
            designation.empty();
            designation.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));

        } else {
            $.getJSON("/Base/DesignationByCampus?campus=" + $("#ddlcampus").val(),
     function (fillbc) {
         var designation = $("#designation");
         designation.empty();
         designation.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));
         $.each(fillbc, function (index, itemdata) {
             designation.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });

     });
        }

        // for Department
        if ($("#ddlcampus").val() == "") {

            var designation = $("#designation");
            designation.empty();
            designation.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));

        } else {
            $.getJSON("/Base/DepartmentByCampus?campus=" + $("#ddlcampus").val(),
     function (fillbc) {
         var department = $("#department");
         department.empty();
         department.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));
         $.each(fillbc, function (index, itemdata) {
             department.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });

     });
        }

    });


    // end 
    $("#Search").click(function () {
        jQuery(grid_selector).clearGridData();
        campus = $('#ddlcampus').val();
        designation = $('#designation').val();
        department = $('#department').val();
        stat = $('#stat').val();
        appname = $('#appname').val();
        idno = $('#idno').val();
        //alert(appname);
        //alert(idno);
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/StaffManagement/StaffListJqGrid',
                        postData: { campus: campus, designation: designation, department: department, stat: stat, appname: appname, idno: idno, type: 'old' },
                        page: 1
                    }).trigger("reloadGrid");
    });

    $("#reset").click(function () {
        jQuery(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        campus = $('#ddlcampus').val();
        designation = $('#designation').val();
        department = $('#department').val();
        stat = $('#stat').val();
        appname = $('#appname').val();
        idno = $('#idno').val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/StaffManagement/StaffListJqGrid',
                        postData: { campus: campus, designation: designation, department: department, stat: stat, appname: appname, idno: idno, type: 'old' },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $('#btnPromotion').click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) {
                ErrMsg("Only 1 Staff can be Mark", function () {
                    jQuery(grid_selector).jqGrid('resetSelection');
                }); return false;
            }
        }
        else {
            ErrMsg("Please select the Staff");
            return false;
        }

        ModifiedLoadPopupDynamicaly("/StaffManagement/PromotionOfStaff?StaffID=" + GridIdList, $('#PromotionDiv'),
            function () {
            }, "", 610, 250, "Promotion Information");

    });
    $('#btnCertfy').click(function () {
        if (!$('#ddlCertificate').val()) { ErrMsg("Please select Certificate"); return false; }
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) { ErrMsg("Only 1 Staff can be Mark"); return false; }
        }
        else { ErrMsg("Please select the Staff"); return false; }


        if ($('#ddlCertificate').val() == "AddressProofLetter" || $('#ddlCertificate').val() == "AppointmentLetter") {
            CallGetCertificateFunction(GridIdList[0], $('#ddlCertificate').val(), "undefined", "undefined", "undefined", "undefined");
        } else {

            LoadPopupDynamicaly(
"/StaffManagement/StaffCredentialsPartialView?PartialViewName=StaffCredentials&certify=" + $('#ddlCertificate').val() + '&PreRegNum=' + GridIdList[0],
$('#DivStudentSearch'),
function () {
},
function (rdata) {
}, 500);

        }
    });
});

function CallGetCertificateFunction(PreRegNum, CertifyType, course, PvsDesg, DateofRelv, Amount) {
    window.location.href = '/StaffManagement/GenerateCertificate?staffId=' + PreRegNum + '&certFy=' + CertifyType + '&course=' + $('#txtCourse').val() + '&PvsDesg=' + $('#txtPvsDesg').val() + '&DateofRelv=' + $('#txtDateofRelv').val() + '&Amount=' + $('#txtAmount').val();
}
