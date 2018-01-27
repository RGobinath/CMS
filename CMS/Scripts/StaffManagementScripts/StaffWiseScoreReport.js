function getSubject() {
    ddlcam = $("#ddlCampus").val();
    ddlgrd = $("#ddlGrade").val();
    if (ddlcam != "", ddlgrd != "") {
        var url = '/Base/GetSubjectsByCampusGradeSec?Campus=' + ddlcam + '&Grade=' + ddlgrd + '&Section=';
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
//function getSemdll() {
//    Campus = $("#ddlCampus").val();
//    Grade = $("#ddlGrade").val();
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
function getMondll() {
    var AcademicYear = $("#ddlAcademicYear").val();
    var Grade = $("#ddlGrade").val();
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
function FillGradeByCampus() {
    var Campus = $("#ddlCampus").val();
    $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
      function (fillbc) {
          var ddlbc = $("#ddlGrade");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
$(function () {
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
    $(grid_selector).jqGrid({
        url: '/StaffManagement/StaffWiseScoreReportDetailsJqGrid',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Id', 'PreReg Number', 'Staff Name', 'Id Number', 'Campus', 'Grade', 'Academic Year', 'Month', 'Subject', 'Sec-A', 'Sec-B', 'Sec-C', 'Sec-D', 'Sec-E', 'Sec-F'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true },
                      { name: 'PreRegNum', index: 'PreRegNum', hidden: true },
                      { name: 'Name', index: 'Name' },
                      { name: 'IdNumber', index: 'IdNumber', width: 131 },
                      { name: 'Campus', index: 'Campus', width: 90 },
                      { name: 'Grade', index: 'Grade', width: 50 },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 80 },
                      { name: 'Month', index: 'Month', width: 80 },
                      { name: 'Subject', index: 'Subject' },
                      { name: 'A', index: 'A', width: 80, align: 'center' },
                      { name: 'B', index: 'B', width: 80, align: 'center' },
                      { name: 'C', index: 'C', width: 80, align: 'center' },
                      { name: 'D', index: 'D', width: 80, align: 'center' },
                      { name: 'E', index: 'E', width: 80, align: 'center' },
                      { name: 'F', index: 'F', width: 80, align: 'center' }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        shrinkToFit: false,
        autowidth: true,
        sortorder: "Asc",
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Staff Wise Score Report",
    })
    $("#grid-table").jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [
          { startColumnName: 'A', numberOfColumns: 6, titleText: 'Score' }
        ]
    });
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
            {
                //url: '/StaffManagement//', closeAfterEdit: true, closeOnEscape: true
            }, //Edit
            {

            }, //Add
              {
                  //width: 'auto', url: '/StaffManagement/DeleteCampusBasedStaffDetails', beforeShowForm: function (params) {
                  //    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                  //    return { Id: selectedrows }
                  //}
              },
               {},
                {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("StaffWiseScoreReportDetailsJqGrid" + '?Name=' + $("#txtStaffName").val() + '&Campus=' + $("#ddlCampus").val() + '&Grade=' + $("#ddlGrade").val() + '&IdNumber=' + $("#txtIdNumber").val() + '&Subject=' + $("#ddlSubject").val() + '&AcademicYear=' + $("#ddlAcademicYear").val() + '&Month=' + $("#ddlMonth").val() + '&rows=9999' + '&Expt=1');
        }
    });
    $("#ddlCampus").change(function () {
        FillGradeByCampus();
    });
    $("#ddlGrade").change(function () {
        getSubject();
        getMondll();
        //getSemdll();        
    });    
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffWiseScoreReportDetailsJqGrid',
           postData: { Campus: "", Grade: "", Subject: "", Name: "", IdNumber: "", AcademicYear: "", Month: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlGrade").val();
        var AcademicYear = $("#ddlAcademicYear").val();
        var StaffName = $("#txtStaffName").val();
        var Subject = $("#ddlSubject").val();
        var IdNumber = $("#txtIdNumber").val();
        var Month = $("#ddlMonth").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffWiseScoreReportDetailsJqGrid',
           postData: { Campus: Campus, Grade: Grade, Subject: Subject, Name: StaffName, IdNumber: IdNumber, Month: Month, AcademicYear: AcademicYear },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#txtStaffName").autocomplete({
        source: function (request, response) {
            if (request.term.length <= 3) {
                return false;
            }
            if (request.term.length > 3) {
                var Campus = $("#ddlCampus").val();
                $.ajax({
                    url: "/StaffManagement/StaffNameAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: request.term, Campus: Campus },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Name, value: item.Name }
                        }))
                    }
                })
            }
        },
        messages: {
            noResults: "", results: ""
        }
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
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function ShowCategoryWiseMarks(Cam, Gra, Sect, aYear, Sub, Mon, PreRegNum) {
    $('#CategoryWiseMarks').clearGridData();
    ModifiedLoadPopupDynamicaly("/StaffManagement/StaffEvaluationCategoryWise?cam=" + Cam + '&gra=' + Gra + '&acayear=' + aYear + '&sect=' + Sect + '&mon=' + Mon + '&sub=' + Sub + '&PreRegNum=' + PreRegNum, $('#CategoryWiseMarks'), function () {
        LoadSetGridParam($('#CategoryWiseMarksList'), "/StaffManagement/StaffEvaluationCategoryWiseJQGrid?cam=" + Cam + '&gra=' + Gra + '&acayear=' + aYear + '&sect=' + Sect + '&mon=' + Mon + '&sub=' + Sub + '&PreRegNum=' + PreRegNum);
    }, function () { }, 830, 487, "Category Wise Mark Report");
}
