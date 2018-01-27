function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    FillCampusDll();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/StaffAttendance_vwJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Name', 'IdNumber', 'Department', 'Designation', 'Attendance Date', 'Check In', 'Check Out', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true, key: true },
                      {
                          name: 'Campus', index: 'Campus'
                      },
                      { name: 'Name', index: 'Name' },
                      { name: 'IdNumber', index: 'IdNumber' },
                      { name: 'Department', index: 'Department' },
                      { name: 'Designation', index: 'Designation' },
                      { name: 'AttendanceDate', index: 'AttendanceDate' },
                      { name: 'LogIn', index: 'LogIn' },
                      { name: 'LogOut', index: 'LogOut' },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Staff Attendance Report",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
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
            }, //Edit
            {

            }, //Add
              {
              },
               {},
                {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            var Campus = $("#ddlCampus").val();
            var Name = $("#txtName").val();
            var IdNumber = $("#txtIdNumber").val();
            var AttendanceDate = $("#txtAttendanceDate").val();
            var Department = $("#ddlDepartment").val();
            var Designation = $("#ddlDesignation").val();
            window.open("StaffAttendance_vwJqGrid" + '?Campus=' + Campus + '&Name=' + Name + '&IdNumber=' + IdNumber + '&Department=' + Department + '&Designation=' + Designation + '&AttendanceDate=' + AttendanceDate + '&rows=9999' + '&ExptXl=1');
        }
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffAttendance_vwJqGrid',
           postData: { Campus: "", Name: "", IdNumber: "", Department: "", Designation: "", AttendanceDate: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $('#txtAttendanceDate').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,        
        endDate: new Date(),
        autoclose: true
    });
    $("#btnSearch").click(function () {
        var Campus = $("#ddlCampus").val();
        var Name = $("#txtName").val();
        var IdNumber = $("#txtIdNumber").val();
        var AttendanceDate = $("#txtAttendanceDate").val();
        var Department = $("#ddlDepartment").val();
        var Designation = $("#ddlDesignation").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffAttendance_vwJqGrid',
           postData: { Campus: Campus, Name: Name, IdNumber: IdNumber, Department: Department, Designation: Designation, AttendanceDate: AttendanceDate },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#ddlCampus").change(function () {

        if ($("#ddlCampus").val() == "") {

            var designation = $("#ddlDesignation");
            designation.empty();
            designation.append($('<option/>',
        {
            value: "",
            text: "Select"
        }));

        } else {
            $.getJSON("/Base/DesignationByCampus?campus=" + $("#ddlCampus").val(),
     function (fillbc) {
         var designation = $("#ddlDesignation");
         designation.empty();
         designation.append($('<option/>',
        {
            value: "",
            text: "Select"
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
            text: "Select"
        }));

        } else {
            $.getJSON("/Base/DepartmentByCampus?campus=" + $("#ddlCampus").val(),
     function (fillbc) {
         var department = $("#ddlDepartment");
         department.empty();
         department.append($('<option/>',
        {
            value: "",
            text: "Select"
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


    //$(document).on('ajaxloadstart', function (e) {
    //    $(grid_selector).jqGrid('GridUnload');
    //    $('.ui-jqdialog').remove();
    //});
});