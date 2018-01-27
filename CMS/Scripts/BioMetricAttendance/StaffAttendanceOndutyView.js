$(function () {
    var grid_selector = "#StaffAttendanceOndutyReportListJqGrid";
    var pager_selector = "#StaffAttendanceOndutyReportListJqGridPager";
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
    var dt = new Date();
    var curyear = dt.getFullYear();
    var ddlYear = $("#ddlYear");
    ddlYear.empty();
    ddlYear.append("<option value=' '>Select</option>");
    for (var i = 2015; i <= curyear; i++) {
        ddlYear.append($('<option/>', { value: i, text: i }));
    }
    $(grid_selector).jqGrid({
        url: '/BioMetricAttendance/StaffAttendanceOndutyReportListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Staff Name', 'IdNumber', 'PreRegNum', 'Month', 'Year', 'OnDuty'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true },
                   { name: 'Name', index: 'Name' },
                   { name: 'IdNumber', index: 'IdNumber' },
                   { name: 'PreRegNum', index: 'PreRegNum', hidden: true },
                   { name: 'Month', index: 'Month' },
                   { name: 'Year', index: 'Year' },
                   { name: 'OnDuty', index: 'OnDuty' },
        ],
        pager: '#StaffAttendanceOndutyReportListJqGridPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Asc',
        height: '230',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Staff Attendance OnDuty Report',
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
            }, {}, {}, {}, {})

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            var Month = $("#ddlMonth").val() || "";
            var Year = $("#ddlYear").val() || "";
            window.open("StaffAttendanceOndutyReportListJqGrid" +
                       '?rows=9999&page=1&sidx=Name&sord=Asc' +
                        '&Month=' + Month +
                        '&Year=' + Year +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }
    });


    $('#btnSearch').click(function () {
        $(grid_selector).setGridParam(
                       {
                           datatype: "json",
                           url: "/BioMetricAttendance/StaffAttendanceOndutyReportListJqGrid",
                           postData: { Month: $('#ddlMonth').val(), Year: $('#ddlYear').val()},
                           page: 1
                       }).trigger('reloadGrid');
    });

    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                       {
                           datatype: "json",
                           url: "/BioMetricAttendance/StaffAttendanceOndutyReportListJqGrid",
                           postData: { Month: $('#ddlMonth').val(), Year: $('#ddlYear').val() },
                           page: 1
                       }).trigger('reloadGrid');
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