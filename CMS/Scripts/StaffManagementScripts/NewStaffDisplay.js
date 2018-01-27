jQuery(function ($) {
    var campus = "";
    var designation = "";
    var department = "";
    var appname = "";
    var idno = "";

    var grid_selector = "#StaffList";
    var pager_selector = "#StaffListPager";

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
    debugger;
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/StaffListJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Req No', 'Name', 'Temp Id Number', 'Campus', 'Designation', 'Department', 'Gender', 'Status', 'Id'],
        colModel: [
             { name: 'PreRegNum', index: 'PreRegNum', width: 30, editable: true, search: true },
                      { name: 'Name', index: 'Name', width: 130, editable: true, search: true },
                      { name: 'TempIdNumber', index: 'TempIdNumber', width: 60, editable: true, search: false },
                      { name: 'Campus', index: 'Campus', width: 50, editable: true, search: true },
                      { name: 'Designation', index: 'Designation', width: 120 },
                      { name: 'Department', index: 'Department', width: 90 },
                      { name: 'Gender', index: 'Gender', width: 40 },
                      { name: 'Status', index: 'Status', width: 110 },
                      { name: 'Id', index: 'Id', width: 90, key: true, hidden: true },
              ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
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
        caption: "<i class='ace-icon fa fa-inbox'></i>&nbsp;Inbox"
    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    // written by felix kinoniya
    $("#ddlcampus").change(function () {

        if ($("#ddlcampus").val() == "") {
            var designation = $("#designation");
            designation.empty();
            designation.append($('<option/>', { value: "", text: "Select One" }));

        } else {
            $.getJSON("/Base/DesignationByCampus?campus=" + $("#ddlcampus").val(),
     function (fillbc) {
         var designation = $("#designation");
         designation.empty();
         designation.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillbc, function (index, itemdata) {
             designation.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
         });

     });
        }

        // for Department
        if ($("#ddlcampus").val() == "") {

            var designation = $("#department");
            department.empty();
            department.append($('<option/>', { value: "", text: "Select One" }));

        } else {
            $.getJSON("/Base/DepartmentByCampus?campus=" + $("#ddlcampus").val(),
     function (fillbc) {
         var department = $("#department");
         department.empty();
         department.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillbc, function (index, itemdata) {
             department.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
         });
     });
        }
    });
    //end
    function loadgrid() {
        // alert(campus);
        jQuery(grid_selector).jqGrid({
            mtype: 'GET',
            url: '/StaffManagement/StaffListJqGrid',
            //postData: { campus: campus, designation: designation, department: department, appname: appname, idno: idno, type: 'new' },
            datatype: 'json',
            colNames: ['Request No', 'Name', 'Id Number', 'Campus', 'Designation', 'Department', 'Gender', 'Status', 'Id'],
            colModel: [
                { name: 'PreRegNum', index: 'PreRegNum', width: 30, align: 'left' },
              { name: 'Name', index: 'Name', width: 100, align: 'left' },
              { name: 'IdNumber', index: 'IdNumber', width: 30, align: 'left' },
              { name: 'Campus', index: 'Campus', width: 40, align: 'left', sortable: true },
              { name: 'Designation', index: 'Designation', width: 50, align: 'left', sortable: true },
              { name: 'Department', index: 'Department', width: 60, align: 'left', sortable: true },
              { name: 'Gender', index: 'Gender', width: 30, align: 'left', sortable: true },
                            { name: 'Status', index: 'Status', width: 60, align: 'left', sortable: true },
               { name: 'Id', index: 'Id', width: '30%', align: 'left', sortable: false, hidden: true, key: true }
              ],
            pager: '#NewStaffListPager',
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            multiselect: true,
            viewrecords: true,
            //                 sortname: 'Id',
            //                 sortorder: 'desc',
            height: '200',
            caption: 'Inbox'
        });

    }

    $("#Search").click(function () {

        //$(grid_selector).GridUnload();
        jQuery(grid_selector).clearGridData();

        var e = document.getElementById("ddlcampus");
        campus = e.options[e.selectedIndex].value;
        designation = document.getElementById("designation").value;
        var g = document.getElementById("department");
        department = g.options[g.selectedIndex].value;
        appname = document.getElementById('appname').value;
        idno = document.getElementById("idno").value;
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/StaffManagement/StaffListJqGrid',
                        postData: { campus: campus, designation: designation, department: department, appname: appname, idno: idno, type: 'new' },
                        page: 1
                    }).trigger("reloadGrid");

    });
    window.onload = loadgrid();
    $("#reset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        campus = $('#ddlcampus').val();
        designation = $('#designation').val();
        department = $('#department').val();
        appname = $('#appname').val();
        idno = $('#idno').val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/StaffManagement/StaffListJqGrid',
                        postData: { campus: campus, designation: designation, department: department, appname: appname, idno: idno, type: 'new' },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#AddNew").click(function () {
        var url = $("#AddNewUrl").val();
        window.location.href = url;
    });
});
