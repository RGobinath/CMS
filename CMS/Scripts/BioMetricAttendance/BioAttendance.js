jQuery(function () {
    var grid_selector = "#AttendanceLogJqGrid";
    var pager_selector = "#AttendanceLogJqGridPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-9").width());
    });
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
    $(grid_selector).jqGrid({
        url: '/BioMetricAttendance/AttendanceLogJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'EmployeeName', 'Gender', 'Company Name', 'Department Name', 'Designation', 'Employement Type', 'Status', 'Attendance Date', 'Attendance Status'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true },
                   { name: 'EmployeeName', index: 'EmployeeName' },
                   { name: 'Gender', index: 'Gender' },
                   { name: 'CompanyFName', index: 'CompanyFName' },
                   { name: 'DepartmentFName', index: 'DepartmentFName' },
                   { name: 'Designation', index: 'Designation' },
                   { name: 'EmployementType', index: 'EmployementType' },
                   { name: 'Status', index: 'Status' },
                   { name: 'AttendanceDate', index: 'AttendanceDate' },
                   { name: 'AttendanceStatus', index: 'AttendanceStatus' }],
        pager: '#AttendanceLogJqGridPager',
        rowNum: '10',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'EmployeeName',
        sortorder: 'Asc',
        height: '230',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Attendance Log',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });


    //navButtons Add, edit, delete
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
            },{},{}, {},{})

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            var EmpName = $("#txtEmpName").val() || "";
            var Campus = $("#ddlCompany").val() || "";
            var AttDate = $("#txtAttDate").val() || "";
            window.open("AttendanceLogJqGrid" + '?rows=9999&page=1&sidx=EmployeeName&sord=desc' +
                        '&EmployeeName=' + EmpName +
                        '&CompanyCode=' + Campus +
                        '&AttendanceDate=' + AttDate +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }
    });

   $("#btnSearch").click(function () {
       jQuery(grid_selector).clearGridData();
       debugger;
        var EmpName = $("#txtEmpName").val() || "";
        var Campus = $("#ddlCompany").val() || "";
        var AttDate = $("#txtAttDate").val() || "";
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: "/BioMetricAttendance/AttendanceLogJqGrid",
                        postData: { EmployeeName: EmpName, CompanyCode: Campus, AttendanceDate: AttDate },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        window.location.href = "/BioMetricAttendance/BioAttendance";
    });
   


    function updatePagerIcons(table) {
        var replacement = {
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
   
});