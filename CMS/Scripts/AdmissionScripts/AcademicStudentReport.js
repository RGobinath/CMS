jQuery(function ($) {
    var grid_selector = "#AcademicStudentReport";
    var pager_selector = "#AcademicStudentReportPager";

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
        url: '/Admission/AcademicStudentReportJqgrid',
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        height: 200,
        colNames: ['Id', 'PreRegNum', 'Campus', 'Grade', 'Section', 'Name', 'NewId', 'Student Photo'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true },
            { name: 'PreRegNum', index: 'PreRegNum',hidden:true },
            { name: 'Campus', index: 'Campus' },
            { name: 'Grade', index: 'Grade' },
            { name: 'Section', index: 'Section' },
            { name: 'Name', index: 'Name' },
            { name: 'NewId', index: 'NewId' },
            { name: 'StudentPhoto', index: 'StudentPhoto' },
        ],
        rowNum: 50,
        rowList: [50, 100, 150,200], // disable page size dropdown
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
        caption: 'Academic Student Report'
    });

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

    //$(grid_selector).jqGrid('navButtonAdd', pager_selector, {
    //    caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
    //    onClickButton: function () {
    //        debugger;
    //        window.open("AcademicYearReportJqgrid" + '?rows=9999 ' + '&ExportType=Excel');
    //    }
    //});

    //replace icons with FontAwesome icons like above

    $('#campusddl').change(function () {
        gradeddl();
    });

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        if ($('#campusddl').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        else if ($('#gradeddl').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        else {
            jQuery(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: '/Admission/AcademicStudentReportJqgrid',
                            postData: { Campus: $('#campusddl').val(), Grade: $('#gradeddl').val(),Section: $('#Section').val() },
                            page: 1
                        }).trigger("reloadGrid");
        }
    });

    $("#btnReset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Admission/AcademicStudentReportJqgrid',
                        postData: { Campus: $('#campusddl').val(), Grade: $('#gradeddl').val(), Section: $('#Section').val() },
                        page: 1
                    }).trigger("reloadGrid");

    });
    $("#btnPdf").click(function () {
        if ($('#campusddl').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        else if ($('#gradeddl').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        else {
            window.open('/Admission/GenerateStudentProfilePDF?Campus=' + $('#campusddl').val() + '&Grade=' + $('#gradeddl').val() + '&Section=' + $('#Section').val());
        }
    });
});

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
//enable datepicker
function pickDate(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=text]')
                    .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
    }, 0);
}
//switch element when editing inline
function aceSwitch(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=checkbox]')
                .addClass('ace ace-switch ace-switch-5')
                .after('<span class="lbl"></span>');
    }, 0);
}

function gradeddl() {
    var e = document.getElementById('campusddl');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#gradeddl");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "Select Grade" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
}

