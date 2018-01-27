jQuery(function ($) {
    var grid_selector = "#MarkAnalysisJqgrid";
    var pager_selector = "#MarkAnalysisPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
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
    var Campus = $("#Campus").val();
    var Grade = $("#Grade").val();
    var Semester = $("#ddlSemester").val();
    jQuery(grid_selector).jqGrid({
        url: '/Assess360/SummativeMarkAnalysisJqGrid?Campus=' + Campus + "&Grade=" + Grade + "&Semester=" + Semester,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Section', 'semester', 'Subject','100-90','80-90','70-80','60-70','50-60','40-50','<40','Total Student'],
        colModel: [
        //if any column added need to check formateadorLink
               { name: 'Id', index: 'Id', hidden: true, key: true },
               { name: 'Campus', index: 'Campus' },
               { name: 'Grade', index: 'Grade' },
               { name: 'Section', index: 'Section'},
               { name: 'semester', index: 'semester'},
               { name: 'Subject', index: 'Subject' },
               { name: 'ntytohund', index: 'ntytohund', width: 60, search: false, align: 'center' },
               { name: 'etytonty', index: 'etytonty', width: 60, search: false, align: 'center' },
               { name: 'svtytoety', index: 'svtytoety', width: 60, search: false, align: 'center' },
               { name: 'sxtytosvty', index: 'sxtytosvty', width: 60, search: false, align: 'center' },
               { name: 'ftytosxty', index: 'ftytosxty', width: 60, search: false, align: 'center' },
               { name: 'frtytofty', index: 'frtytofty', width: 60, search: false, align: 'center' },
               { name: 'blwfrty', index: 'blwfrty', width: 60, search: false, align: 'center' },
               { name: 'TotalStudents', index: 'TotalStudents', width: 60, search: false, align: 'center' },
        ],
        pager: pager_selector,
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 275,
        autowidth: true,
        multiselect:true,
        //shrinktofit: true,
        viewrecords: true,
        altRows: true,
        select: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
            //jQuery('#jqGridTrash').jqGrid('clearGridData');
            //$('#jqGridTrash').trigger('reloadGrid');
        },
        caption: 'Inbox'
    });

    jQuery(grid_selector).jqGrid('navGrid',pager_selector,
            {
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
            }, {}, {}, {
            }, {}, {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            var Campus = $("#Campus").val();
            var Grade = $("#Grade").val();
            var Semester = $("#ddlSemester").val();
            window.open("/Assess360/SummativeMarkAnalysisJqGrid" + '?ExportType=Excel'
                  + '&Campus=' + Campus
                    + '&Grade=' + Grade
                    + '&Semester=' + Semester
                    + '&rows=9999');
        }
    });
    $("#Campus").change(function () {
        getSemdll();
    });
    $("#Grade").change(function () {
        getSemdll();
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
        //
    }

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        var Campus = $("#Campus").val();
        var Grade = $("#Grade").val();
        var Semester = $("#ddlSemester").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Assess360/SummativeMarkAnalysisJqGrid',
                    postData: {  Campus: Campus, Grade: Grade,Semester:Semester },
                    page: 1
                }).trigger("reloadGrid");

    });

    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        var Campus = $("#Campus").val();
        var Grade = $("#Grade").val();
        var Semester = $("#ddlSemester").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Assess360/SummativeMarkAnalysisJqGrid',
                    postData: { Campus: Campus, Grade: Grade, Semester: Semester },
                    page: 1
                }).trigger("reloadGrid");
    });
    function getSemdll() {
        if (Campus != "" && Grade != "") {
            var Campus = $("#Campus").val();
            var Grade = $("#Grade").val();
            $.getJSON("/Assess360/GetSemester?Campus=" + Campus + "&Grade=" + Grade,
                  function (fillig) {
                      $("#ddlSemester").empty();
                      $("#ddlSemester").append($('<option/>', { value: "", text: "Select One" }));
                      $.each(fillig, function (index, itemdata) {
                          $("#ddlSemester").append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                      });
                  });
        }
    }
   
});
function MarkAnalysis(Cam, Gra, Sect, Sub, Marks) {
 //   var modalid = $('#StudentMarkAnalysis');
    $('#StudentMarkAnalysis').clearGridData();
    ModifiedLoadPopupDynamicaly("/Assess360/StudentsMarkDetails", $('#StudentMarkAnalysis'), function () {
        LoadSetGridParam($('#StudentsList'), "/Assess360/StudentMarkDetailsJqgrid?Campus=" + Cam + '&grade=' + Gra + '&section=' + Sect + '&sub=' + Sub + '&marks=' + Marks);
    }, function () { }, 930, 488, "Sumative Student details");

    //ModifiedLoadPopupDynamicaly("/Assess360/StudentsMarkDetails", $('#StudentMarkAnalysis'), function () {
    //    LoadSetGridParam($('#StudentsList'), "/Assess360/StudentMarkDetailsJqgrid?Campus=" + Cam + '&grade=' + Gra + '&section=' + Sect + '&sub=' + encodeURIComponent(Sub) + '&marks=' + Marks);
    //}, function () { }, 918, 487, "Students List Report");
}
function LoadSetGridParam(GridId, brUrl) {
    GridId.setGridParam({
        url: brUrl,
        datatype: 'json',
        mtype: 'GET',
        page: 1
    }).trigger("reloadGrid");
}

