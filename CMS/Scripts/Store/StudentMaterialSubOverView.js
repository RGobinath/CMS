jQuery(function ($) {
    var grid_selector = "#GetJqGridStudentMaterialSubOverView_vwList";
    var pager_selector = "#GetJqGridStudentMaterialSubOverView_vwListPager";

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
        url: '/Store/GetJqGridStudentMaterialSubOverView_vwList',
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        height: 350,
        colNames: ['Id', 'Academic Year', 'MaterialSubGroup', 'MaterialSubGroupId', 'MaterialId', 'Material', 'IB MAIN', 'IB KG', 'Karur', 'Karur KG', 'TIPS SARAN', 'Tirupur', 'Tirupur KG', 'Ernakulam', 'Chennai Main', 'Chennai City', 'CBSE MAIN'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, key: true },
            { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
             { name: 'MaterialSubGroup', index: 'MaterialSubGroup', widt: 180 },
            { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', hidden: true },
            { name: 'MaterialId', index: 'MaterialId', hidden: true },
            { name: 'Material', index: 'Material' },
            { name: 'IB_MAIN', index: 'IB_MAIN' },
            { name: 'IB_KG', index: 'IB_KG' },
            { name: 'Karur', index: 'Karur' },
            { name: 'Karur_KG', index: 'Karur_KG' },
            { name: 'Tips_Saran', index: 'Tips_Saran', hidden: true },
            { name: 'Tirupur', index: 'Tirupur' },
            { name: 'Tirupur_KG', index: 'Tirupur_KG' },
            { name: 'Ernakulam', index: 'Ernakulam' },
            { name: 'Chennai_Main', index: 'Chennai_Main' },
            { name: 'Chennai_City', index: 'Chennai_City' },
            { name: 'CBSE_Main', index: 'CBSE_Main' },
        ],
        rowNum: 20,
        rowList: [20, 50, 100, 150], // disable page size dropdown
        sortname: 'Id',
        sortorder: 'Asc',
        viewrecords: true,
        pager: pager_selector,
        altRows: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: 'Student Material Over View',
        subGrid: true,
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (StudentSubGroup, Id) {
            debugger;
            var StudentSubGroupListTable, StudentSubGroupListPager;
            StudentSubGroupListTable = StudentSubGroup + "_t";
            StudentSubGroupListPager = "p_" + StudentSubGroupListTable;
            $("#" + StudentSubGroup).html("<table id='" + StudentSubGroupListTable + "' ></table><div id='" + StudentSubGroupListPager + "' ></div>");
            jQuery("#" + StudentSubGroupListTable).jqGrid({
                url: '/Store/GetJqGridStudentMaterialSubGroup_vwList?Id=' + Id,
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'Campus', 'Academic Year', 'MaterialSubGroupId', 'MaterialId', 'MaterialSubGroup', 'Type/Size', 'Total'],
                colModel: [
                    { name: 'Id', index: 'Id', hidden: true, key: true },
                    { name: 'Campus', index: 'Campus' },
                    { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
                    { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', hidden: true },
                    { name: 'MaterialId', index: 'MaterialId', hidden: true },
                    { name: 'MaterialSubGroup', index: 'MaterialSubGroup', hidden: true },
                    { name: 'Material', index: 'Material' },
                    { name: 'Total', index: 'Total' },
                ],
                pager: StudentSubGroupListPager,
                rowNum: 100,
                rowList: [50, 100, 150, 200, 500, 1000, 5000],
                sortname: 'Campus',
                sortorder: 'Asc',
                height: '130',
                autowidth: true,
                viewrecords: true,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                }

            });
            jQuery("#" + StudentSubGroupListTable).jqGrid('navGrid', "#" + StudentSubGroupListPager,
                {
                    edit: false, add: false, del: false, search: false,
                    searchicon: 'ace-icon fa fa-search orange',
                    refresh: true, refreshicon: 'ace-icon fa fa-refresh green'
                });
            jQuery("#" + StudentSubGroupListTable).jqGrid('navButtonAdd', "#" + StudentSubGroupListPager, {
                caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
                onClickButton: function () {
                    var AcademicYear = $("#ddlacademicyear").val();
                    //  window.open("GetJqGridStudentMaterialSubGroup_vwList" + '?ExportType=Excel&rows=9999');
                    window.open("/Store/GetJqGridStudentMaterialSubGroup_vwList?ExportType=Excel&Id=" + Id + '&AcademicYear=' + AcademicYear + '&rows=99999');
                }
            });
        }

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
    $("#btnrptSearch1").click(function () {
        //alert();
        jQuery(grid_selector).clearGridData();
        AcademicYear = $('#ddlacademicyear').val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Store/GetJqGridStudentMaterialOverView_vwList',
                        postData: { AcademicYear: AcademicYear },
                        page: 1
                    }).trigger("reloadGrid");
    });

    $("#btnrptReset1").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        AcademicYear = $('#ddlacademicyear').val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Store/GetJqGridStudentMaterialOverView_vwList',
                        postData: { AcademicYear: AcademicYear },
                        page: 1
                    }).trigger("reloadGrid");

    });

});