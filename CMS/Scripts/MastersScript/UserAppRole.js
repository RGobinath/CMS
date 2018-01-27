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
        url: '/Common/UserAppRolejqgrid',
        //postData: { userid: usersrch, appcd: appcdsrch, rlcd: rlcdsrch, depcd: depcdsrch, brncd: brncdsrch },
        datatype: 'json',
        height: 200,
        colNames: ['User Id', 'Application Code', 'Role Code', 'Department Code', 'Branch', 'Email Id', ''],
        colModel: [
                          { name: 'UserId', index: 'UserId', width: 90, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, editoptions: { style: " font-size: 0.9em" }, sortable: true },
                          { name: 'AppCode', index: 'AppCode', width: 60, align: 'left', editable: true, edittype: 'select', editoptions: { dataUrl: '/Home/AppCodeddl', style: "width: 150px; height: 20px; font-size: 0.9em" }, editrules: { custom: true, custom_func: checkvalid }, sortable: true },
                          { name: 'RoleCode', index: 'RoleCode', width: 60, align: 'left', editable: true, edittype: 'select', editoptions: { dataUrl: '/Home/RoleCodeddl', style: "width: 150px; height: 20px; font-size: 0.9em", dataEvents: [{ type: 'change', fn: function (frm) { if ($(frm.target).val() == 'CSE') { $('#DeptCode').attr('disabled', 'disabled'); } else { $('#DeptCode').attr('disabled', false); } } }] }, editrules: { custom: true, custom_func: checkvalid }, sortable: true },
                          { name: 'DeptCode', index: 'DeptCode', width: 130, align: 'left', editable: true, edittype: 'select', editoptions: { dataUrl: '/Home/DeptCodeddl', style: "width: 150px; height: 20px; font-size: 0.9em" }, editrules: { custom: true, custom_func: checkvalid1 }, sortable: true },
                          { name: 'BranchCode', index: 'BranchCode', width: 90, align: 'center', editable: true, edittype: 'select', editoptions: { dataUrl: '/Home/BranchCodeddl', style: "width: 150px; height: 20px; font-size: 0.9em" }, editrules: { custom: true, custom_func: checkvalid }, sortable: true },
                          { name: 'Email', index: 'Email', width: 130, align: 'left', editable: true, edittype: 'text', editrules: { email: true }, sortable: true, search: false },
                          { name: 'Id', index: 'Id', width: 60, align: 'left', editable: true, edittype: 'text', hidden: true, key: true, sortable: false }

                          ],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid'); 
        },
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;User Approle"

    });
   // $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });


    function checkvalid(value, column) {

        if (value == 'nil') {
            return [false, column + ": Field is Required"];
        }
        else {
            return [true];
        }
    }

    function checkvalid1(value, column) {
        if (value == 'nil') {
            if ($('#RoleCode').val() == 'CSE' && $('#DeptCode').val() == 'nil') {
                //        alert($('#RoleCode').val());
                return [true];
            }
            else {
                // return [false, column + ": Field is Required"];
                return [true];
            }
        }
        else {
            return [true];
        }
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
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
            {
                url: '/Common/AddUserAppRole/?test=edit',
                beforeShowForm: function (form) { $('#UserId').attr('readonly', 'readonly'); if ($('#RoleCode').val() == 'CSE') { $('#DeptCode').attr('disabled', 'disabled'); } else { $('#DeptCode').attr('disabled', false); } }
            },
            {
                url: '/Common/AddUserAppRole',
                beforeShowForm: function (frm) { $('#UserId').removeAttr('readonly'); $('#DeptCode').attr('disabled', false); }
            }, {}, {})

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
    $("#appcodeddl").change(function () {
        $("#btnSearch").click();
    });
    $("#rolecodeddl").change(function () {
        $("#btnSearch").click();
    });
    $("#deptcodeddl").change(function () {
        $("#btnSearch").click();
    });
    $("#branchcodeddl").change(function () {
        $("#btnSearch").click();
    });

    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        usersrch = $('#userid').val();
        appcdsrch = $('#appcodeddl').val();
        rlcdsrch = $('#rolecodeddl').val();
        depcdsrch = $('#deptcodeddl').val();
        brncdsrch = $('#branchcodeddl').val();

        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Common/UserAppRolejqgrid',
                        postData: { userid: usersrch, appcd: appcdsrch, rlcd: rlcdsrch, depcd: depcdsrch, brncd: brncdsrch },
                        page: 1
                    }).trigger("reloadGrid");
    });

    $("#btnReset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        usersrch = $('#userid').val();
        appcdsrch = $('#appcodeddl').val();
        rlcdsrch = $('#rolecodeddl').val();
        depcdsrch = $('#deptcodeddl').val();
        brncdsrch = $('#branchcodeddl').val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Common/UserAppRolejqgrid',
                        postData: { userid: usersrch, appcd: appcdsrch, rlcd: rlcdsrch, depcd: depcdsrch, brncd: brncdsrch },
                        page: 1
                    }).trigger("reloadGrid");

    });
});



$.getJSON("/Base/FillAllBranchCode",
     function (fillcampus) {
         var ddlcam = $("#branchcodeddl");
         ddlcam.empty();
         ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

         $.each(fillcampus, function (index, itemdata) {
             ddlcam.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
     });