jQuery(function ($) {
    var grid_selector = "#AttendanceConfigurationsReportList";
    var pager_selector = "#AttendanceConfigurationsReportListPager";
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
    });
    getCampus();
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/GetAttendanceConfigurationsReportListJqGrid',
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'PreRegNum', 'Campus', 'Id Number', 'Name', 'Department', 'Designation', 'Status', ' Staff Type', 'Staff Category', 'Staf fUser Name', 'IdNumber To Employee Code', 'Staff Bio Metric Id', ' Is Having ICMS Account', 'Is Attendance[ESSL] Configured', 'Is Attendance[ESSL] Mapped In ICMS'],
        colModel: [
        { name: 'Id', index: 'Id', hidden: true, key: true, editable: true },
        { name: 'PreRegNum', index: 'PreRegNum', width: 110, hidden: true },
        { name: 'Campus', index: 'Campus', width: 90, hidden: false },
        { name: 'IdNumber', index: 'IdNumber', hidden: false, width: 150 },
        { name: 'Name', index: 'Name', hidden: false, width: 100 },
        { name: 'Department', index: 'Department', width: 110, hidden: false },
        { name: 'Designation', index: 'Designation', hidden: false },
        { name: 'Status', index: 'Status', hidden: true, width: 100 },
        { name: 'StaffType', index: 'StaffType', hidden: true, width: 150 },
        { name: 'StaffCategoryForAttendane', index: 'StaffCategoryForAttendane', hidden: false, width: 150 },
        { name: 'StaffUserName', index: 'StaffUserName', width: 90, hidden: true },
        { name: 'IdNumberToEmployeeCode', index: 'IdNumberToEmployeeCode', hidden: true },
        { name: 'StaffBioMetricId', index: 'StaffBioMetricId', width: 110, hidden: true },
        { name: 'IsHavingICMSAccount', index: 'IsHavingICMSAccount', hidden: true, width: 100 },
        { name: 'IsAttendanceConfigured', index: 'IsAttendanceConfigured', hidden: false, width: 150 },
        { name: 'IsAttendanceMappedInICMS', index: 'IsAttendanceMappedInICMS', width: 90, hidden: false }
        ],
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        rowNum: 10,
        rowList: [10, 20, 30, 50, 100, 500, 1000],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        pager: pager_selector,
        sortname: 'Id',
        sortorder: "Desc",
        height: 300,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Attendance Configurations Settings',
    });

    $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size
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
            viewicon: 'ace-icon fa fa-search-plus grey',
        },
        {}, {}, {}, {}, {});
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("GetAttendanceConfigurationsReportListJqGrid" + '?rows=9999' +
                        '&page=1' +
                        '&sidx=Id' +
                        '&sord=Desc' +
                        '&Campus=' + $("#ddlCampus").val() +
                        '&IdNumber=' + $("#txtIdNumber").val() +
                        '&Name=' + $("#txtName").val() +
                        '&Designation=' + $("#ddlDesignation").val() +
                        '&Department=' + $("#ddlDepartment").val() +
                        '&StaffCategoryForAttendane=' + $("#ddlStaffCategoryForAttendane").val() +
                        '&IsAttendanceConfigured=' + $("#ddlIsAttendanceConfigured").val() +
                        '&IsAttendanceMappedInICMS=' + $("#ddlIsAttendanceMappedInICMS").val() +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }

    });
    //For pager Icons
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
        if (value == 'nil' || value == "") {
            return [false, column + ": Field is Required"];
        }
        else {
            return [true];
        }
    }
    $("#btnSearch").click(function () {
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetAttendanceConfigurationsReportListJqGrid',
           postData: {
               Campus: $("#ddlCampus").val()
               , IdNumber: $("#txtIdNumber").val()
               , Name: $("#txtName").val()
               , Designation: $("#ddlDesignation").val()
               , Department: $("#ddlDepartment").val()
               , StaffCategoryForAttendane: $("#ddlStaffCategoryForAttendane").val()
               //, IsHavingICMSAccount: $("#ddlIsHavingICMSAccount").val()
               , IsAttendanceConfigured: $("#ddlIsAttendanceConfigured").val()
               , IsAttendanceMappedInICMS: $("#ddlIsAttendanceMappedInICMS").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetAttendanceConfigurationsReportListJqGrid',
           postData: {
               Campus: $("#ddlCampus").val()
               , IdNumber: $("#txtIdNumber").val()
               , Name: $("#txtName").val()
               , Designation: $("#ddlDesignation").val()
               , Department: $("#ddlDepartment").val()
               , StaffCategoryForAttendane: $("#ddlStaffCategoryForAttendane").val()
               //, IsHavingICMSAccount: $("#ddlIsHavingICMSAccount").val()
               , IsAttendanceConfigured: $("#ddlIsAttendanceConfigured").val()
               , IsAttendanceMappedInICMS: $("#ddlIsAttendanceMappedInICMS").val()
           },
           page: 1

       }).trigger("reloadGrid");
    });

    $('#btnMapIntoICMS').click(function () {
        var selectedRows = [];
        var selectedEmployeeCodes = [];
        selectedRows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
        for (var i = 0; i < selectedRows.length; i++) {
            var selectedRowData = $(grid_selector).jqGrid("getRowData", selectedRows[i]);
            if (i == 0)
                selectedEmployeeCodes = selectedRowData.IdNumberToEmployeeCode;
            else
                selectedEmployeeCodes = selectedEmployeeCodes + "," + selectedRowData.IdNumberToEmployeeCode;
        }
        if (selectedEmployeeCodes.length > 0) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                async: true,
                url: '/StaffManagement/UpdateStaffBiometricIdByIdNumberToEmployeeCode',
                data: {
                    IdNumberToEmployeeCodeArray: selectedEmployeeCodes
                },
                success: function (ReturnValue) {
                    if (ReturnValue == true)
                        InfoMsg("Mapped Successfully!");
                    else
                        ErrMsg("Mapping function UnSuccessfull!");
                }
            });
            jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetAttendanceConfigurationsReportListJqGrid',
           postData: {
               Campus: $("#ddlCampus").val()
               , IdNumber: $("#txtIdNumber").val()
               , Name: $("#txtName").val()
               , Designation: $("#ddlDesignation").val()
               , Department: $("#ddlDepartment").val()
               , StaffCategoryForAttendane: $("#ddlStaffCategoryForAttendane").val()
               //, IsHavingICMSAccount: $("#ddlIsHavingICMSAccount").val()
               , IsAttendanceConfigured: $("#ddlIsAttendanceConfigured").val()
               , IsAttendanceMappedInICMS: $("#ddlIsAttendanceMappedInICMS").val()
           },
           page: 1

       }).trigger("reloadGrid");
        }
        else { ErrMsg("Please select anyone staff!"); }
    });

    $("#ddlCampus").change(function () {
        if ($("#ddlCampus").val() == "") {
            var designation = $("#ddlDesignation");
            designation.empty();
            designation.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));

        } else {
            $.getJSON("/Base/DesignationByCampus?campus=" + $("#ddlCampus").val(),
     function (fillbc) {
         var designation = $("#ddlDesignation");
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
        if ($("#ddlCampus").val() == "") {

            var designation = $("#ddlDesignation");
            designation.empty();
            designation.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));

        } else {
            $.getJSON("/Base/DepartmentByCampus?campus=" + $("#ddlCampus").val(),
     function (fillbc) {
         var department = $("#ddlDepartment");
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
});
function getCampus() {
    $.getJSON("/Base/FillCampusNameByMapping",
function (fillig) {
    var Campusddl = $("#ddlCampus");
    Campusddl.empty();
    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    });
});
}