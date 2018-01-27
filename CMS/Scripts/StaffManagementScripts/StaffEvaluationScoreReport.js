function FillGradeByCampus() {
    Campus = $("#ddlcampus").val();
    var ddlgrd = $("#ddlgrade");
    getSubject();
    if (Campus != null && Campus != "") {
        $.getJSON("/Admission/CampusGradeddl?campus=" + Campus,
          function (fillbc) {
              ddlgrd.empty();
              ddlgrd.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlgrd.append($('<option/>', { value: itemdata.gradcod, text: itemdata.gradcod }));
              });
          });
    }
    else {
        ddlgrd.empty;
        ddlgrd.append($('<option/>', { value: "", text: "Select One" }));
    }
}

function getSubject() {
    ddlcam = $("#ddlcampus").val();
    ddlgrd = $("#ddlgrade").val();
    ddlsec = $("#ddlSection").val();
    if (ddlcam != "", ddlgrd != "", ddlsec != "") {
        var url = '/Base/GetSubjectsByCampusGradeSec?Campus=' + ddlcam + '&Grade=' + ddlgrd + '&Section=' + ddlsec;
        $.getJSON(url, function (fillSubject) {
            var ddlSub = $("#ddlSubject");
            ddlSub.empty();
            ddlSub.append($('<option />', { value: "", text: "Select One" }));
            $.each(fillSubject, function (index, itemdata) {
                debugger;
                ddlSub.append($('<option />', { value: itemdata.Value, text: itemdata.Text }));
            });
        });
    }
    else {
        $("#ddlSubject").empty();
        $("#ddlSubject").append($('<option />', { value: "", text: "Select One" }));
    }
}
function getMondll() {
    var AcademicYear = $("#ddlacademicyear").val();
    var Grade = $("#ddlgrade").val();
    if (Grade != "") {
        $.getJSON("/Base/GetMonthValbyAcademicYearandGrade?academicYear=" + AcademicYear + '&grade=' + Grade,
              function (fillig) {
                  $("#ddlMonth").empty();
                  $("#ddlMonth").append($('<option />', { value: "", text: "Select One" }));
                  $.each(fillig, function (index, itemdata) {
                      $("#ddlMonth").append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                  });
              });
    }
    else {
        $("#ddlMonth").empty();
        $("#ddlMonth").append($('<option/>', { value: "", text: "Select One" }));
    }
}
//function getSemdll() {
//    Campus = $("#ddlcampus").val();
//    Grade = $("#ddlgrade").val();
//    if (Campus != "" && Grade != "") {
//        $.getJSON("/Assess360/GetSemester?Campus=" + Campus + "&Grade=" + Grade,
//              function (fillig) {
//                  $("#ddlSemester").empty();
//                  $("#ddlSemester").append($('<option />', { value: "", text: "Select One" }));
//                  $.each(fillig, function (index, itemdata) {
//                      $("#ddlSemester").append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//                  });
//              });
//    }
//    else {
//        $("#ddlSemester").empty();
//        $("#ddlSemester").append($('<option/>', { value: "", text: "Select One" }));
//    }
//}

$(function () {
    var Campus = ""; var Grade = ""; var Section = "";
    var AcademicYear = ""; var Subject = ""; var Month = "";
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
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
        url: '/StaffManagement/StaffEvaluationScoreReportJqgrid',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Section', 'Academic Year', 'Month', 'Subject Name', 'PreRegNum', 'Name', 'IdNumber', 'No. of Students Attended', 'Total No. of Students', 'Average Score'],
        colModel: [{ name: 'Id', index: 'Id', key: true, width: 130, hidden: true, editable: true },
                   { name: 'Campus', index: 'Campus' },
                   { name: 'Grade', index: 'Grade' },
                   { name: 'Section', index: 'Section' },
                   { name: 'AcademicYear', index: 'AcademicYear' },
                   { name: 'Month', index: 'Month' },
                   { name: 'SubjectName', index: 'SubjectName' },
                   { name: 'PreRegNum', index: 'PreRegNum', hidden: true },
                   { name: 'Name', index: 'Name' },
                   { name: 'IdNumber', index: 'IdNumber', hidden: true },
                   { name: 'Entered', index: 'Entered',align:'center' },
                   { name: 'TotalStudents', index: 'TotalStudents', align: 'center' },
                   { name: 'TotalScore', index: 'TotalScore', align: 'center' },
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
                //styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Staff Evaluation Score",
    })
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size            
    //navButtons
    $(grid_selector).jqGrid('navGrid', pager_selector,
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
            {}, {}, {});
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            Campus = $("#ddlcampus").val();
            Grade = $("#ddlgrade").val();
            Section = $("#ddlSection").val();
            AcademicYear = $("#ddlacademicyear").val();
            Subject = $("#ddlSubject").val();
            Month = $("#ddlMonth").val();
            staff = $("#ddlStaffName").val();
            window.open("/StaffManagement/StaffEvaluationScoreReportJqgrid" + '?ExportType=Excel'
                + '&Campus=' + Campus
                + '&Grade=' + Grade
                + '&Section=' + Section
                + '&AcademicYear=' + AcademicYear
                + '&Subject=' + Subject
                + '&Month=' + Month
                + '&staff=' + staff
                    + '&rows=99999');
        }
    });
    //$(grid_selector).jqGrid('filterToolbar', {
    //    searchOnEnter: true, enableClear: false,
    //    afterSearch: function () { $(grid_selector).clearGridData(); }
    //});

    $("#SrchbtnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffEvaluationScoreReportJqgrid',
           postData: { Campus: "", Grade: "", Section: "", AcademicYear: "", Subject: "", Month: "", staff: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        Campus = $("#ddlcampus").val();
        Grade = $("#ddlgrade").val();
        Section = $("#ddlSection").val();
        AcademicYear = $("#ddlacademicyear").val();
        Subject = $("#ddlSubject").val();
        Month = $("#ddlMonth").val();
        staff = $("#ddlStaffName").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffEvaluationScoreReportJqgrid',
           postData: { Campus: Campus, Grade: Grade, Section: Section, AcademicYear: AcademicYear, Subject: Subject, Month: Month, staff: staff },
           page: 1
       }).trigger("reloadGrid");
    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function getStaffName() {
    var Campus = $("#ddlcampus").val();
    var Grade = $("#ddlgrade").val();
    var Section = $("#ddlSection").val();
    var sub = $('#ddlSubject').val();
    if (sub != "") {
        var url = '/Base/GetStaffName?Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section + '&Subject=' + sub;
        $.getJSON(url, function (fillStaff) {
            var ddlSub = $("#ddlStaffName");
            ddlSub.empty();
            if (fillStaff != null && fillStaff.length == 1) {
                ddlSub.append("<option value='" + fillStaff[0].Text + "'selected='selected' >" + fillStaff[0].Text + "</option>");
            }
            else {
                ddlSub.append($('<option />', { value: "", text: "Select One" }));
                $.each(fillStaff, function (index, itemdata) {
                    ddlBoard.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                });
            }
        });
    }
    else {        
        $("#ddlStaffName").empty();
        $("#ddlStaffName").append($('<option />', { value: "", text: "Select One" }));
    }

}
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