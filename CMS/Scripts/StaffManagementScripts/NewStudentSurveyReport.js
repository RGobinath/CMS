$(function () {
    var Campus = ""; var Grade = ""; var Section = "";
    var AcademicYear = ""; var surveyno = "";
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    var grid_selector1 = "#JqGridgrade";
    var pager_selector1 = "#JqGridgradePager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-pane").width());
        $(grid_selector1).jqGrid('setGridWidth', $(".tab-pane").width());
    })
    //resize on sidebar collapse/expand 
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    var parent_column1 = $(grid_selector1).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                $(grid_selector1).jqGrid('setGridWidth', parent_column1.width());
            }, 0);
        }
    })
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $(grid_selector1).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    $(grid_selector1).jqGrid({
        datatype: 'json',
        type: 'GET',
        autowidth: true,
        height: 250,
        colNames: [],
        colModel: [],
        rowNum: 50,
        rowList: [50, 100, 150, 200, 500, 1000,5000],
        sortname: 'Total',
        sortroder: 'Desc',
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>All Student Survey Report'
    });

    $(grid_selector).jqGrid({
        url: '/StaffManagement/StaffWiseSurveyReportJqgrid',
        //postData: { Campus: Campus, Grade: Grade, Section: Section, Surveyno: Surveyno, AcademicYear: AcademicYear },
        datatype: 'json',
        height: 230,
        mtype: 'GET',
        colNames: ['Id', 'Academic Year', 'Campus', 'Staff Name', 'Staff PreRegNumber', 'Department', 'CategoryName', 'No. of Students evaluated', 'No. of Question in Survey', 'Score', 'Average Score'],
        colModel: [{ name: 'Id', index: 'Id', key: true, hidden: true },
                   { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
                   { name: 'Campus', index: 'Campus', hidden: false, width: 100 },
                   { name: 'StaffName', index: 'StaffName', align: 'center', width: 300 },                   
                   { name: 'StaffPreRegNumber', index: 'StaffPreRegNumber', hidden: true },
                   { name: 'Subject', index: 'Subject', align: 'center', hidden: true },
                   { name: 'CategoryName', index: 'CategoryName', align: 'center', hidden: true },
                   { name: 'StudentCount', index: 'StudentCount', align: 'center' },
                   { name: 'QuestionCount', index: 'QuestionCount', align: 'center', hidden: true },
                   { name: 'Score', index: 'Score', align: 'center', hidden: true },
                   { name: 'Average', index: 'Average', align: 'center' }
        ],
        viewrecords: true,
        rowNum: 25,
        shrinkToFit: true,
        rowList: [25, 50, 100],
        sortname: 'StaffName',
        sortorder: 'Asc',
        viewrecords: true,
        pager: pager_selector,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Student Survey Report",
        multiselect: true,
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
        subGridRowExpanded: function (StudentSurey, Id) {
            debugger;
            var surveystaffListTable, surveystaffListPager;
            surveystaffListTable = StudentSurey + "_t";
            surveystaffListPager = "p_" + surveystaffListTable;
            $("#" + StudentSurey).html("<table id='" + surveystaffListTable + "' ></table><div id='" + surveystaffListPager + "' ></div>");
            jQuery("#" + surveystaffListTable).jqGrid({
                url: '/StaffManagement/SurveyStaffwiseReportJqgrid?Id=' + Id + '&Survey=' + $('#ddlSurveyNo').val(),
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'Academic Year', 'Campus', 'Grade', 'Section', 'Subject','Survey Number', 'Survey Date', 'No. of Students Attended', 'No. of Question in Survey', 'Score', 'Average Score'],
                colModel: [{ name: 'Id', index: 'Id', key: true, hidden: true },
                    { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
                    { name: 'Campus', index: 'Campus', hidden: true },
                    { name: 'Grade', index: 'Grade', align: 'center' },
                    { name: 'Section', index: 'Section', align: 'center' },
                    { name: 'Subject', index: 'Subject', align: 'center' },
                    { name: 'CategoryName', index: 'CategoryName', align: 'center' },
                   { name: 'EvaluationDate', index: 'EvaluationDate', align: 'center' },
                   { name: 'StudentCount', index: 'StudentCount', align: 'center' },
                   { name: 'QuestionCount', index: 'QuestionCount', align: 'center', hidden: true },
                   { name: 'Score', index: 'Score', align: 'center', hidden: true },
                   { name: 'Average', index: 'Average', align: 'center', formatter: formateadorLink }
                ],

                pager: surveystaffListPager,
                rowNum: 100,
                rowList: [50, 100, 150, 200, 500, 1000, 5000],
                sortname: 'Id',
                sortorder: 'Desc',
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
            jQuery("#" + surveystaffListTable).jqGrid('navGrid', "#" + surveystaffListPager,
                {
                    edit: false, add: false, del: false, search: false,
                    searchicon: 'ace-icon fa fa-search orange',
                    refresh: true, refreshicon: 'ace-icon fa fa-refresh green'
                });
            jQuery("#" + surveystaffListTable).jqGrid('navButtonAdd', "#" + surveystaffListPager, {
                caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
                onClickButton: function () {
                    window.open("/StaffManagement/SurveyStaffwiseReportJqgrid?ExportType=Excel&Id=" + Id + '&rows=99999');
                }
            });
            // jQuery("#" + surveystaffListTable).jqGrid('jqPivot', "data.json", // pivot options
            //{

            //    aggregates: [
            //        { member: 'Price', aggregator: 'sum', width: 80, formatter: 'number', align: 'right', summaryType: 'sum' }
            //    ],
            //    rowTotals: true, colTotals: true
            //}, // grid options 
            //{ width: 700, rowNum: 10, pager: "#VehicleCostDetailsJqGridPager" });
        }
    });
    jQuery(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green', search: false });
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            Campus = $("#ddlcampus").val();
            Grade = $("#ddlgrade").val();
            Section = $("#ddlSection").val();
            AcademicYear = $("#ddlacademicyear").val();
            Surveyno = $("#ddlSurveyNo").val();
            window.open("/StaffManagement/StaffWiseSurveyReportJqgrid?ExportType=Excel&Campus=" + Campus + '&Grade=' + Grade + '&Section=' + Section + '&AcademicYear=' + AcademicYear + '&Surveyno=' + Surveyno + '&rows=99999');
        }
    });

    function Allloadgrid(Campus, Grade, Section, AcademicYear, Surveyno) {
        if (Section == "ALL") {
            colNames = "['Id', 'Academic Year','Campus', 'Grade',  'Survey Number', 'Survey Date', 'Staff Name', 'No. of Students Attended', 'No. of Question in Survey','Score',  'Average Score']";
            colModel = "[{ name: 'Id', index: 'Id', key: true,hidden: true },{ name: 'AcademicYear', index: 'AcademicYear',hidden: true },{ name: 'Campus', index: 'Campus',hidden: true },{ name: 'Grade', index: 'Grade',hidden: true },{ name: 'CategoryName', index: 'CategoryName' },";
            colModel = colModel + "{ name: 'EvaluationDate', index: 'EvaluationDate' },{ name: 'StaffName', index: 'StaffName', align: 'center',width:300 },{ name: 'StudentCount', index: 'StudentCount', align: 'center' },{ name: 'QuestionCount', index: 'QuestionCount', align: 'center',hidden:true },{ name: 'Score', index: 'Score', align: 'center',hidden:true },{ name: 'Average', index: 'Average', align: 'center' }]"
        }
        else {
            colNames = "['Id', 'Academic Year', 'Campus', 'Grade', 'Section', 'Survey Number', 'Survey Date', 'Staff Name', 'No. of Students Attended', 'No. of Question in Survey','Score',  'Average Score']";
            colModel = "[{ name: 'Id', index: 'Id', key: true,hidden: true },{ name: 'AcademicYear', index: 'AcademicYear',hidden: true },{ name: 'Campus', index: 'Campus',hidden: true },{ name: 'Grade', index: 'Grade',hidden: true },{ name: 'Section', index: 'Section',hidden: true },{ name: 'CategoryName', index: 'CategoryName', align: 'center' },";
            colModel = colModel + "{ name: 'EvaluationDate', index: 'EvaluationDate', align: 'center' },{ name: 'StaffName', index: 'StaffName', align: 'center',width:300 },{ name: 'StudentCount', index: 'StudentCount', align: 'center' },{ name: 'QuestionCount', index: 'QuestionCount', align: 'center',hidden:true },{ name: 'Score', index: 'Score', align: 'center',hidden:true },{ name: 'Average', index: 'Average', align: 'center' }]"
        }
        columnName = eval(colNames);
        columnModel = eval(colModel);
        $(grid_selector1).jqGrid({
            url: '/StaffManagement/SurveyReportAllJqgrid',
            postData: { Campus: Campus, Grade: Grade, Section: Section, Surveyno: Surveyno, AcademicYear: AcademicYear },
            datatype: 'json',
            height: 230,
            mtype: 'GET',
            colNames: columnName,
            colModel: columnModel,
            viewrecords: true,
            sortname: 'StaffName',
            sortorder: 'Asc',
            rowNum: 25,
            shrinkToFit: true,
            rowList: [50, 100, 150, 200, 500, 1000, 5000],
            viewrecords: true,
            pager: pager_selector1,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            },
            caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Student Survey Report",
        });
        jQuery(grid_selector1).navGrid(pager_selector1, { add: false, edit: false, del: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green', search: false });
        $(grid_selector1).jqGrid('navButtonAdd', pager_selector1, {
            caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
            onClickButton: function () {
                Campus = $("#ddlcampus1").val();
                Grade = $("#ddlgrade1").val();
                Section = $("#ddlSection1").val();
                AcademicYear = $("#ddlacademicyear1").val();
                Surveyno = $("#ddlSurveyNo1").val();
                window.open("/StaffManagement/SurveyReportAllJqgrid?ExportType=Excel&Campus=" + Campus + '&Grade=' + Grade + '&Section=' + Section + '&AcademicYear=' + AcademicYear + '&Surveyno=' + Surveyno + '&rows=99999');
            }
        });
    }

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size            
    //navButtons


    //$(grid_selector).jqGrid('filterToolbar', {
    //    searchOnEnter: true, enableClear: false,
    //    afterSearch: function () { $(grid_selector).clearGridData(); }
    //});
    function formateadorLink(cellvalue, options, rowObject) {
        return "<a href=/Assess360/Assess360?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }

    $("#SrchbtnReset").click(function () {
        //var url = $("#BackUrl").val();
        //window.location.href = url;
        $("input[type=text], textarea, select").val("");
        $(grid_selector).GridUnload();

        $(grid_selector).jqGrid({
            //url: "/Assess360/SummativeAssessmentJqGridNew",
            datatype: 'json',
            type: 'GET',
            autowidth: true,
            height: 250,
            colNames: [],
            colModel: [],
            rowNum: 50,
            rowList: [50, 100, 150, 200],
            sortname: 'Total',
            sortroder: 'Desc',
            viewrecords: true,
            caption: '<i class="fa fa-list">&nbsp;</i>Student Survey Report'
        });
    });
    $("#btnallReset").click(function () {
        //var url = $("#BackUrl").val();
        //window.location.href = url;
        $("input[type=text], textarea, select").val("");
        $(grid_selector1).GridUnload();

        $(grid_selector1).jqGrid({
            //url: "/Assess360/SummativeAssessmentJqGridNew",
            datatype: 'json',
            type: 'GET',
            autowidth: true,
            height: 250,
            colNames: [],
            colModel: [],
            rowNum: 50,
            rowList: [50, 100, 150, 200],
            sortname: 'Total',
            sortroder: 'Desc',
            viewrecords: true,
            caption: '<i class="fa fa-list">&nbsp;</i>All Student Survey Report'
        });
    });

    $("#btnSearch").click(function () {
        var AcademicYear = $("#ddlacademicyear").val();
        var Campus = $("#ddlcampus").val();
        var Surveyno = $("#ddlSurveyNo").val();
        var subject = $("#ddlSubject").val();
        var staffpreregnum = $("#StaffPreRegNum").val();
        if (!isEmptyorNull(AcademicYear)) {
            $(grid_selector).setGridParam(
             {
                 datatype: "json",
                 url: "/StaffManagement/StaffWiseSurveyReportJqgrid",
                 type: 'POST',
                 postData: { Campus: Campus, AcaYear: AcademicYear, StaffPreRegNum: staffpreregnum, Subject: subject, surveyNo: Surveyno },
                 page: 1
             }).trigger("reloadGrid");
        }
        else {
            ErrMsg('Please select AcademicYear.');
            return false;
        }
    });
    $("#btnallSearch").click(function () {
        var Campus = $("#ddlcampus1").val();
        var Grade = $("#ddlgrade1").val();
        var Section = $("#ddlSection1").val();
        var AcademicYear = $("#ddlacademicyear1").val();
        var Surveyno = $("#ddlSurveyNo1").val();
        if (!isEmptyorNull(AcademicYear) && !isEmptyorNull(Campus) && !isEmptyorNull(Grade) && !isEmptyorNull(Section) && !isEmptyorNull(Surveyno)) {
            $(grid_selector1).GridUnload();
            window.onload = Allloadgrid(Campus, Grade, Section, AcademicYear, Surveyno);
        }
        else {
            ErrMsg('Please Select Mandatory Fields.');
            return false;
        }
    });
    $("#txtStaffName").keyup(function (event) {
        if ($("#txtStaffName").val() == "") {
            $("#StaffPreRegNum").val("");
        }
    }).keydown(function (event) {
        if ($("#txtStaffName").val() == "")
            $("#StaffPreRegNum").val("");
    });
    var cache = {};
    $("#txtStaffName").autocomplete({
        source: function (request, response) {
            var term = request.term;
            if (term in cache) {
                response($.map(cache[term], function (item) {
                    return { label: item.Name, value: item.PreRegNum }
                }))
                return;
            }
            $.getJSON("/StaffManagement/StaffNameAutoComplete", request, function (data, status, xhr) {
                cache[term] = data;
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.PreRegNum }
                }))
            });
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            event.preventDefault();
            $("#StaffPreRegNum").val(ui.item.value);
            $("#txtStaffName").val(ui.item.label);
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtStaffName").val(ui.item.label);
        },
        messages: {
            noResults: "", results: ""
        }
    });
    //$("#txtStaffName").autocomplete({
    //    source: function (request, response) {
    //        if (request.term.length <= 3) {
    //            return false;
    //        }
    //        if (request.term.length > 3) {
    //            var Campus = $("#ddlcampus").val();
    //            $.ajax({
    //                url: "/StaffManagement/StaffNameAutoComplete",
    //                type: "POST",
    //                dataType: "json",
    //                data: { term: request.term, Campus: Campus },
    //                success: function (data) {
    //                    response($.map(data, function (item) {
    //                        return { label: item.Name, value: item.Name }
    //                    }))
    //                }
    //            })
    //        }
    //    },
    //    messages: {
    //        noResults: "", results: ""
    //    }
    //});

    $("#ddlcampus").change(function () {
        getSurveyno();
        FillSubjectByCampus();
    });

});
function FillGradeByCampus() {
    Campus = $("#ddlcampus").val();
    var ddlgrd = $("#ddlgrade");
    if (Campus != null && Campus != "") {
        $.getJSON("/StaffManagement/GetStaffEvaluationCategoryGradeByCampus?Active=true&campus=" + Campus,
          function (fillbc) {
              ddlgrd.empty();
              ddlgrd.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlgrd.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlgrd.empty;
        ddlgrd.append($('<option/>', { value: "", text: "Select One" }));
    }
}


function FillAllGradeByCampus() {
    Campus = $("#ddlcampus1").val();
    var ddlgrd = $("#ddlgrade1");
    if (Campus != null && Campus != "") {
        $.getJSON("/StaffManagement/GetStaffEvaluationCategoryGradeByCampus?campus=" + Campus,
          function (fillbc) {
              ddlgrd.empty();
              ddlgrd.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlgrd.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlgrd.empty;
        ddlgrd.append($('<option/>', { value: "", text: "Select One" }));
    }
}

function getSurveyno() {
    ddlcam = $("#ddlcampus").val();
    if (ddlcam != "") {
        var url = '/StaffManagement/GetStaffEvaluationCategoryByCampusGrade?Campus=' + ddlcam;
        $.getJSON(url, function (fillSubject) {
            var ddlSub = $("#ddlSurveyNo");
            ddlSub.empty();
            ddlSub.append($('<option />', { value: "", text: "Select One" }));
            $.each(fillSubject, function (index, itemdata) {
                ddlSub.append($('<option />', { value: itemdata.Value, text: itemdata.Text }));
            });
        });
    }
    else {
        $("#ddlSurveyNo").empty();
        $("#ddlSurveyNo").append($('<option />', { value: "", text: "Select One" }));
    }
}
function FillSubjectByCampus() {
    var Campus = $("#ddlcampus").val();
    var ddlbc = $("#ddlSubject");
    if (Campus != "") {
        $.getJSON("/Base/GetSubjectsByCampusGrade?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  for (var i = 0; itemdata.length >= 0; i++) {
                      if (itemdata[i].Value != null && itemdata[i].Text != null) {
                          ddlbc.append($('<option/>', { value: itemdata[i].Value, text: itemdata[i].Text }));
                      }
                  }
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
    }
}

function getAllSurveyno() {
    ddlcam = $("#ddlcampus1").val();
    ddlgrd = $("#ddlgrade1").val();
    ddlsec = $("#ddlSection1").val();
    var ddlSub = $("#ddlSurveyNo1");
    if (ddlcam != "", ddlgrd != "") {
        var url = '/StaffManagement/GetStaffEvaluationCategoryByCampusGrade?Campus=' + ddlcam + '&Grade=' + ddlgrd + '&Section=' + ddlsec;
        $.getJSON(url, function (fillSubject) {
            ddlSub.empty();
            ddlSub.append($('<option />', { value: "", text: "Select One" }));
            $.each(fillSubject, function (index, itemdata) {
                ddlSub.append($('<option />', { value: itemdata.Value, text: itemdata.Text }));
            });
        });
    }
    else {
        ddlSub.empty();
        ddlSub.append($('<option />', { value: "", text: "Select One" }));
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

//function ShowWeightageMarkswithQuestion(rowid, aYear, Cam, Gra, preRegnum, surveyId) {
//    $('#SurveyMarkswsecListJqgrid').clearGridData();
//    ModifiedLoadPopupDynamicaly("/StaffManagement/ShowQuestionMarksWithoutSection?RowId=" + rowid, $('#SurveyMarksList'), function () {
//        LoadSetGridParam($('#SurveyMarkswsecListJqgrid'), "/StaffManagement/ShowQuestionMarksListJqGrid?acayear=" + aYear + '&cam=' + Cam + '&gra=' + Gra + '&preRegNum=' + preRegnum + '&SurveyId=' + surveyId);
//    }, function () { }, 1000, 500, "Staff wise Detailed Report");
//}

function ShowWeightageMarkwithQuestionSec(rowid, aYear, Cam, Gra, Sect, preRegnum, surveyId, CampusBasedStaffDetails_Id) {
    $('#SurveyMarkssecListJqgrid').clearGridData();    
    ModifiedLoadPopupDynamicaly("/StaffManagement/ShowSurveyQuestionMarksWithSection?RowId=" + rowid , $('#SurveyMarkssecList'), function () {
        LoadSetGridParam($('#SurveyMarkssecListJqgrid'), "/StaffManagement/ShowSurveyQuestionMarksListJqGrid?acayear=" + aYear + '&cam=' + Cam + '&gra=' + Gra + '&preRegNum=' + preRegnum + '&sect=' + Sect + '&SurveyId=' + surveyId + '&sidx=SurveyQuestionId' + '&CampusBasedStaffDetails_Id=' + CampusBasedStaffDetails_Id);
    }, function () { }, 1000, 500, "Staff wise Detailed Report");
}

function ShowQuestionWeightageMarkinStaff(rowid, aYear, Cam, Gra, Sect, preRegnum, surveyId, CampusBasedStaffDetails_Id) {
    $('#SurveyMarkswsecListJqgrid').clearGridData();
    ModifiedLoadPopupDynamicaly("/StaffManagement/ShowSurveyQuestionMarksInStaff?RowId=" + rowid + '&Cam=' + Cam + '&grad=' + Gra + '&sec=' + Sect + '&acayr=' + aYear + '&surveyid=' + surveyId + '&preregnum=' + preRegnum + '&CampusBasedStaffDetails_Id=' + CampusBasedStaffDetails_Id, $('#SurveyMarksList'), function () {
        LoadSetGridParam($('#SurveyMarkswsecListJqgrid'), "/StaffManagement/ShowSurveyQuestionMarksListJqGrid?acayear=" + aYear + '&cam=' + Cam + '&gra=' + Gra + '&preRegNum=' + preRegnum + '&sect=' + Sect + '&SurveyId=' + surveyId + '&CampusBasedStaffDetails_Id=' + CampusBasedStaffDetails_Id);
    }, function () { }, 1000, 500, "Staff wise Detailed Report");
}