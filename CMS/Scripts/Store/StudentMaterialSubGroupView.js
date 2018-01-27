jQuery(function ($) {
    
    var grid_selector = "#GetJqGridStudentMaterialSubGroup_vwList";
    var pager_selector = "#GetJqGridStudentMaterialSubGroup_vwListPager";

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
        url: '/Store/GetJqGridStudentMaterialSubGroup_vwList',
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        height: 200,
        colNames: ['Id', 'Campus', 'Academic Year', 'MaterialSubGroupId', 'MaterialId', 'MaterialSubGroup', 'Material', 'Total'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, key: true },
            { name: 'Campus', index: 'Campus' },
            { name: 'AcademicYear', index: 'AcademicYear'},
            { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', hidden: true },
            { name: 'MaterialId', index: 'MaterialId', hidden: true },
            { name: 'MaterialSubGroup', index: 'MaterialSubGroup' },
            { name: 'Material', index: 'Material' },
            { name: 'Total', index: 'Total' },
        ],
        rowNum: 20,
        rowList: [20, 50, 100, 150], // disable page size dropdown
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        viewrecords: true,
        shrinktofit: true,
        pager: pager_selector,
        altRows: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: 'Student Material Over View',
       
});
    //});

   // $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
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
            {}, //Edit
            {}, //Add
            {}, //Delete
            {},
            {})

    //jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
    //    caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
    //    onClickButton: function () {
    //        debugger;
    //        window.open("/Admission/AcademicYearReportJqgrid" + '?ExportType=Excel'
    //                + '&rows=9999');
    //    }
    //});

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("GetJqGridStudentMaterialOverView_vwList" + '?ExportType=Excel&rows=9999');
        }
    });
    //$(grid_selector).jqGrid('navButtonAdd', pager_selector, {
    //    caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
    //    onClickButton: function () {
    //        window.open("/Stroe/GetJqGridStudentMaterialOverView_vwList" + '?ExportType=Excel&rows=9999 ');
    //    }
    //});


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
    //$("#btnrptSearch1").click(function () {
    //    alert();
    //    jQuery(grid_selector).clearGridData();
    //    AcademicYear = $('#ddlacademicyear').val();
    //    jQuery(grid_selector).setGridParam(
    //                {
    //                    datatype: "json",
    //                    url: '/Store/GetJqGridStudentMaterialOverView_vwList',
    //                    postData: { AcademicYear: AcademicYear },
    //                    page: 1
    //                }).trigger("reloadGrid");
    //});

    //$("#btnrptReset1").click(function () {
    //    $(grid_selector).clearGridData();
    //    $("input[type=text], textarea, select").val("");
    //    AcademicYear = $('#ddlacademicyear').val();
    //    jQuery(grid_selector).setGridParam(
    //                {
    //                    datatype: "json",
    //                    url: '/Store/GetJqGridStudentMaterialOverView_vwList',
    //                    postData: { AcademicYear: AcademicYear },
    //                    page: 1
    //                }).trigger("reloadGrid");

    //});
    
});
