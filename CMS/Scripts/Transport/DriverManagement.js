jQuery(function ($) {
    var grid_selector = "#DriverManagmentGrid";
    var pager_selector = "#DriverManagmentGridPager";

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
        url: '/Transport/DriverManagementListGrid?PageName=Saved',
        //postData: { userid: usersrch, appcd: appcdsrch, rlcd: rlcdsrch, depcd: depcdsrch, brncd: brncdsrch },
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Campus', 'Name', 'Driver Id No', 'Dob', 'Age', 'Gender', 'Contact No', 'Status', 'Created Date', 'Created By', 'Modified Date', 'Modified By','PDF'],
        colModel: [
        { name: 'Id', width: 30, index: 'Id', key: true, hidden: true },
            { name: 'Campus', index: 'Campus' },
            { name: 'Name', index: 'Name' },
            { name: 'DriverIdNo', index: 'DriverIdNo' },
            { name: 'Dob', index: 'Dob' },
            { name: 'Age', index: 'Age', width: 120 },
            { name: 'Sex', index: 'Sex' },
            { name: 'ContactNo', index: 'ContactNo' },
            { name: 'Status', index: 'Status' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
            { name: 'PDF', index: 'PDF' }],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-dot-circle-o'></i>&nbsp;Driver Details List"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

//    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
//        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
//        onClickButton: function () {
//            window.open("DriverMasterJqGrid" + '?Campus=' + $("#txtDrvrCampus").val() + '&Name=' + $("#txtDrvrName").val()
//            + '&Dob=' + $("#txtDrvrDob").val() + '&Age=' + $("#txtDrvrAge").val() + '&Sex=' + $("#txtDrvrSex").val()
//            + '&LicenseNo=' + $("#txtDrvrLicenseNo").val() + '&DriverIdNo=' + $("#txtDriverIdNo").val() + '&BatchNo=' + $("#txtDrvrBatchNo").val()
//            + '&LicenseValDate=' + $("#txtDrvrLicenseValDate").val() + '&NonTraLicenseValDate=' + $("#txtDrvrNonTraLicenseValDate").val() + '&Status=' + $("#txtDrvrStatus").val()
//             + ' &rows=9999 ' + '&ExportType=Excel');
//        }
//    });


    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
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
    $("#driverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#ddlCampus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        select: function (event, ui) {
            //debugger;
            FillDriverIdNo(ui.item.value);
        },
        minLength: 1,
        delay: 100
    });
    $("#gs_Name").autocomplete({
        source: function (request, response) {
            var Campus = $("#ddlCampus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $("#ddlCampus").change(function () {
        $("#btnSearch").click();
    });
    $("#ddlStatus").change(function () {
        $("#btnSearch").click();
    });
//    $("#deptcodeddl").change(function () {
//        $("#btnSearch").click();
//    });
//    $("#ddlCampus").change(function () {
//        $("#btnSearch").click();
//    });

    $("#btnSearch").click(function () {
    debugger;
        jQuery(grid_selector).clearGridData();
        namesrch = $('#driverName').val();
        idsrch = $('#driverIdNo').val();
        statussrch = $('#ddlStatus').val();
        //depcdsrch = $('#deptcodeddl').val();
        campsrch = $('#ddlCampus').val();

        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Transport/DriverManagementListGrid?PageName=Saved',
                        postData: { Campus: campsrch, Name: namesrch, DriverIdNo: idsrch, Status: statussrch },
                        page: 1
                    }).trigger("reloadGrid");
    });

    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).clearGridData();
        namesrch = $('#driverName').val();
        idsrch = $('#driverIdNo').val();
        statussrch = $('#ddlStatus').val();
        //depcdsrch = $('#deptcodeddl').val();
        campsrch = $('#ddlCampus').val();

        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Transport/DriverManagementListGrid?PageName=Saved',
                        postData: { Campus: campsrch, Name: namesrch, DriverIdNo: idsrch, Status: statussrch },
                        page: 1
                    }).trigger("reloadGrid");
    });


    $("#IdCard").click(function () {
        debugger;
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (GridIdList == "") { ErrMsg("Please select Driver List"); return false;}
        var rowData = [];
        var rowData1 = [];
        var MainrowData1 = "";
        //  alert(GridIdList.length);
        if (GridIdList.length > 0) {

            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].DriverIdNo;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
            window.location.href = "/Transport/PrintIdCard?PreRegNo=" + MainrowData1; // rowData1;
        }
    });


});

function FillDriverIdNo(DriverName) {
    debugger;
    var Campus = $("#ddlCampus").val();
    $.ajax({
        type: 'POST',
        url: "/Transport/GetDriverDetailsByNameAndCampus",
        data: { Campus: Campus, DriverName: DriverName },
        success: function (data) {
            $("#driverIdNo").val(data.DriverIdNo);
            $("#PreRegNum").val(data.DriverRegNo);
        }
    });
}



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


function DriverDetails(id1) {
    debugger;
    window.location.href = "/Transport/DriverProfilePDF?id=" + id1;
}