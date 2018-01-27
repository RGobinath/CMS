$(function () {
    var ReqId = ""; var Cam = ""; var Grd = ""; var Sec = ""; var AcY = ""; var Sub = ""; var NofDaysTerm1 = ""; var NofDaysTerm2 = ""; var nofworkdays = 0;
    var param = ""; var CoCode = 0; Trm = "";
    GetSubject();
    GetLanguage();
    HideUnwantedDiv();
    $("#lifeSkillTabs").tabs({});
    $("#visualperformingartTabs").tabs({});
    $("#attitudeTabs").tabs({});
    $("#valuesTabs").tabs({});

    $('#saveWorkingDayTerm2').click(function () {
        if ($("#txtWorkingDayTerm2").val() != 0) {
            NofDaysTerm2 = $("#txtWorkingDayTerm2").val();
            SaveTotalWorkingDay("Term2", NofDaysTerm2);
            ReqId = $("#Id").val();
            Cam = $("#Campus").val();
            Grd = $("#Grade").val();
            Sec = $("#Section").val();
            AcY = $("#AcademicYear").val();
            $("#JqGridCommonTerm2").setGridParam(
                {
                    datatype: "json",
                    url: '/Assess360/JqGridCommonTerm2',
                    postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, NofDays: NofDaysTerm2 },
                    page: 1
                }).trigger("reloadGrid");
        }
        else { ErrMsg("Please Enter Term2 Total no of Workingday"); return false; }
        $("#JqGridCommonTerm2").trigger("reloadGrid");
    });
    $('#btnBack').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url; //'Url.Action("ReportCardRequest", "Assess360")';
    });
    $("#ddlSubject").change(function () {
        if ($("#ddlSubject").val() == "Language") {
            $("#languageddl").show();
            $("#languagelbl").show();
        }
        else {
            $("#languageddl").hide();
            $("#languagelbl").hide();
        }
        if (($("#ddlSubject").val() != "") && ($("#ddlSubject").val() != "Language")) {
            window.location.href = "/Assess360/ReportCardCBSE?RptRequestId=" + $("#Id").val() + '&Subject=' + $("#ddlSubject").val();
        }
        if ($("#ddlSubject").val() == "") {
            window.location.href = "/Assess360/ReportCardCBSE?RptRequestId=" + $("#Id").val();
        }
    });
    $("#ddllanguage").change(function () {
        if ($("#ddlSubject").val() == "Language" && $("#ddllanguage").val() != "") {
            window.location.href = "/Assess360/ReportCardCBSE?RptRequestId=" + $("#Id").val() + '&Subject=' + $("#ddlSubject").val() + '&Language=' + $("#ddllanguage").val();
        }
    });
    $("#getCoScholastic").click(function () {
        Grd = $("#Grade").val();
        if ($("#ddlCoScholastic").val() == "")
        { ErrMsg("Please Select CoScholastic!.."); return false; }
        else if ($("#ddlTerm").val() == "")
        { ErrMsg("Please Select Term!.."); return false; }
        ReqId = $("#Id").val();
        Cam = $("#Campus").val();
        Sec = $("#Section").val();
        AcY = $("#AcademicYear").val();
        Trm = $("#ddlTerm").val();


        if ($("#ddlCoScholastic").val() == 1) {
            HideUnwantedDiv();
            $("#lifeSkill").show();
            LifeSkills1(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills2(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills3(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills4(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills5(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills6(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills7(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills8(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills9(ReqId, Cam, Grd, Sec, AcY, Trm);
            LifeSkills10(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 2) {
            HideUnwantedDiv();
            $("#workeducation").show();
            WorkEducation(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 3) {
            HideUnwantedDiv();
            $("#visualperformingart").show();
            VisualArts(ReqId, Cam, Grd, Sec, AcY, Trm);
            PerformingArts(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 4) {
            HideUnwantedDiv();
            $("#attitude").show();
            Attitude1(ReqId, Cam, Grd, Sec, AcY, Trm);
            Attitude2(ReqId, Cam, Grd, Sec, AcY, Trm);
            Attitude3(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 5) {
            HideUnwantedDiv();
            $("#values").show();
            ValuesSystem1(ReqId, Cam, Grd, Sec, AcY, Trm);
            ValuesSystem2(ReqId, Cam, Grd, Sec, AcY, Trm);
            ValuesSystem3(ReqId, Cam, Grd, Sec, AcY, Trm);
            ValuesSystem4(ReqId, Cam, Grd, Sec, AcY, Trm);
            ValuesSystem5(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 6) {
            HideUnwantedDiv();
            $("#litandcreskills").show();
            LiteraryandCreSkill(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 7) {
            HideUnwantedDiv();
            $("#ICT").show();
            ICT(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 8) {
            HideUnwantedDiv();
            $("#healthandpet").show();
            HealthPET(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else if ($("#ddlCoScholastic").val() == 9) {
            HideUnwantedDiv();
            $("#ScientificSkills").show();
            ScientificSkills(ReqId, Cam, Grd, Sec, AcY, Trm);
        }
        else {
            HideUnwantedDiv();
        }
    });
    $('#saveWorkingDayTerm1').click(function () {
        if ($("#txtWorkingDayTerm1").val() != 0) {
            NofDaysTerm1 = $("#txtWorkingDayTerm1").val();
            SaveTotalWorkingDay("Term1", NofDaysTerm1);
            ReqId = $("#Id").val();
            Cam = $("#Campus").val();
            Grd = $("#Grade").val();
            Sec = $("#Section").val();
            AcY = $("#AcademicYear").val();
            $("#JqGridCommonTerm1").setGridParam(
                {
                    datatype: "json",
                    url: '/Assess360/JqGridCommonTerm1',
                    postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, NofDays: NofDaysTerm1 },
                    page: 1
                }).trigger("reloadGrid");
        }
        else { ErrMsg("Please Enter Term1 Total no of Workingday"); return false; }
    });


});
function ValidationTabs(Val) {
    if ($('#ddlSubject').val() == "") {
        if (Val == 3 || Val == 7 || Val == 8) {
            if (Val == 8) {
            }
            else {
                return true;
            }
        }
        else { ErrMsg("Please fill the Subject"); return false; }
    }
}
function GeneratePDF(RegNum, AcYear, ReqId, term) {
    $.ajax({
        type: 'POST',
        async: false,
        url: '/Assess360/ReportCardCBSEValidation?PreRegNum=' + RegNum + '&AcademicYear=' + AcYear + '&ReqId=' + ReqId,
        success: function (data) {
            if (data == "Success") {
                window.open('/Assess360/GenerateCBSERptCardPdf?PreRegNum=' + RegNum + '&Terms=' + term);
            }
            else {
                ErrMsg("Marks are not available");
                return false;
            }
        }
    });
}
function SaveTotalWorkingDay(Term, nofworkdays) {
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/SaveWorkingDay?RequestId=' + $("#Id").val() + '&Term1or2=' + Term + '&NofDays=' + nofworkdays,
        success: function (data) {
        }
    });
}

function GetLanguage() {
    var campus = $("#Campus").val();
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Base/GetLanguageByCampus?Campus=' + campus,
        success: function (data) {
            $("#ddllanguage").empty();
            $("#ddllanguage").append("<option value=''>--Select Language-- </option>");
            for (var k = 0; k < data.rows.length; k++) {
                $("#ddllanguage").append("<option value='" + data.rows[k].Value + "'>" + data.rows[k].Text + "</option>");
            }
        }
    });
}

function GetSubject() {
    var campus = $("#Campus").val();
    var grade = $("#Grade").val();
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Base/GetSubjectsByCampusGrade?Campus=' + campus + '&Grade=' + grade,
        success: function (data) {
            $("#ddlSubject").empty();
            $("#ddlSubject").append("<option value=''>--Select Subject-- </option>");
            for (var k = 0; k < data.rows.length; k++) {
                $("#ddlSubject").append("<option value='" + data.rows[k].Value + "'>" + data.rows[k].Text + "</option>");
            }
        }
    });
}
$(window).on('resize.jqGrid', function () {
    $("#JqGridSA1").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridSA2").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridFA1").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridFA2").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridFA3").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridFA4").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridOverAll").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridCommonTerm1").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridCommonTerm2").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridHealthPET").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridICT").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridWorkEdu").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLitandCreSkill").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridVS_ToABC").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridVS_ToCFNI").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridVS_ToUPSUI").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridVS_ToRNSWCU").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridVS_ToPHUB").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridAT_AToT").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridAT_AToSM").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridAT_AToSPE").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridPerformingArts").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridVisualArts").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_SA").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_PS").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_DM").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_CriT").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_CreT").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_IR").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_EC").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_Emp").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_ME").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridLS_MwthS").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridSciSkills").jqGrid('setGridWidth', $(".tab-content").width());

})
//resize on sidebar collapse/expand
var parent_column1 = $("#JqGridSA1").closest('[class*="col-"]');
var parent_column2 = $("#JqGridSA2").closest('[class*="col-"]');
var parent_column3 = $("#JqGridFA1").closest('[class*="col-"]');
var parent_column4 = $("#JqGridFA2").closest('[class*="col-"]');
var parent_column5 = $("#JqGridFA3").closest('[class*="col-"]');
var parent_column6 = $("#JqGridFA4").closest('[class*="col-"]');
var parent_column7 = $("#JqGridOverAll").closest('[class*="col-"]');
var parent_column8 = $("#JqGridCommonTerm1").closest('[class*="col-"]');
var parent_column9 = $("#JqGridCommonTerm2").closest('[class*="col-"]');
var parent_column10 = $("#JqGridHealthPET").closest('[class*="col-"]');
var parent_column11 = $("#JqGridICT").closest('[class*="col-"]');

var parent_column12 = $("#JqGridWorkEdu").closest('[class*="col-"]');
var parent_column13 = $("#JqGridLitandCreSkill").closest('[class*="col-"]');
var parent_column14 = $("#JqGridVS_ToABC").closest('[class*="col-"]');
var parent_column15 = $("#JqGridVS_ToCFNI").closest('[class*="col-"]');
var parent_column16 = $("#JqGridVS_ToUPSUI").closest('[class*="col-"]');
var parent_column17 = $("#JqGridVS_ToRNSWCU").closest('[class*="col-"]');
var parent_column18 = $("#JqGridVS_ToPHUB").closest('[class*="col-"]');
var parent_column19 = $("#JqGridAT_AToT").closest('[class*="col-"]');
var parent_column20 = $("#JqGridAT_AToSM").closest('[class*="col-"]');
var parent_column21 = $("#JqGridAT_AToSPE").closest('[class*="col-"]');
var parent_column22 = $("#JqGridPerformingArts").closest('[class*="col-"]');
var parent_column23 = $("#JqGridVisualArts").closest('[class*="col-"]');
var parent_column24 = $("#JqGridLS_SA").closest('[class*="col-"]');
var parent_column25 = $("#JqGridLS_PS").closest('[class*="col-"]');
var parent_column26 = $("#JqGridLS_DM").closest('[class*="col-"]');
var parent_column27 = $("#JqGridLS_CriT").closest('[class*="col-"]');
var parent_column28 = $("#JqGridLS_CreT").closest('[class*="col-"]');
var parent_column29 = $("#JqGridLS_IR").closest('[class*="col-"]');
var parent_column30 = $("#JqGridLS_EC").closest('[class*="col-"]');
var parent_column31 = $("#JqGridLS_Emp").closest('[class*="col-"]');
var parent_column32 = $("#JqGridLS_ME").closest('[class*="col-"]');
var parent_column33 = $("#JqGridLS_MwthS").closest('[class*="col-"]');
var parent_column34 = $("#JqGridSciSkills").closest('[class*="col-"]');

$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
        setTimeout(function () {
            $("#JqGridSA1").jqGrid('setGridWidth', parent_column1.width());
            $("#JqGridSA2").jqGrid('setGridWidth', parent_column2.width());
            $("#JqGridFA1").jqGrid('setGridWidth', parent_column3.width());
            $("#JqGridFA2").jqGrid('setGridWidth', parent_column4.width());
            $("#JqGridFA3").jqGrid('setGridWidth', parent_column5.width());
            $("#JqGridFA4").jqGrid('setGridWidth', parent_column6.width());
            $("#JqGridOverAll").jqGrid('setGridWidth', parent_column7.width());
            $("#JqGridCommonTerm1").jqGrid('setGridWidth', parent_column8.width());
            $("#JqGridCommonTerm2").jqGrid('setGridWidth', parent_column9.width());
            $("#JqGridHealthPET").jqGrid('setGridWidth', parent_column10.width());
            $("#JqGridICT").jqGrid('setGridWidth', parent_column11.width());
            $("#JqGridWorkEdu").jqGrid('setGridWidth', parent_column12.width());
            $("#JqGridLitandCreSkill").jqGrid('setGridWidth', parent_column13.width());
            $("#JqGridVS_ToABC").jqGrid('setGridWidth', parent_column14.width());
            $("#JqGridVS_ToCFNI").jqGrid('setGridWidth', parent_column15.width());
            $("#JqGridVS_ToUPSUI").jqGrid('setGridWidth', parent_column16.width());
            $("#JqGridVS_ToRNSWCU").jqGrid('setGridWidth', parent_column17.width());
            $("#JqGridVS_ToPHUB").jqGrid('setGridWidth', parent_column18.width());
            $("#JqGridAT_AToT").jqGrid('setGridWidth', parent_column19.width());
            $("#JqGridAT_AToSM").jqGrid('setGridWidth', parent_column20.width());
            $("#JqGridAT_AToSPE").jqGrid('setGridWidth', parent_column21.width());
            $("#JqGridPerformingArts").jqGrid('setGridWidth', parent_column22.width());
            $("#JqGridVisualArts").jqGrid('setGridWidth', parent_column23.width());
            $("#JqGridLS_SA").jqGrid('setGridWidth', parent_column24.width());
            $("#JqGridLS_PS").jqGrid('setGridWidth', parent_column25.width());
            $("#JqGridLS_DM").jqGrid('setGridWidth', parent_column26.width());
            $("#JqGridLS_CriT").jqGrid('setGridWidth', parent_column27.width());
            $("#JqGridLS_CreT").jqGrid('setGridWidth', parent_column28.width());
            $("#JqGridLS_IR").jqGrid('setGridWidth', parent_column29.width());
            $("#JqGridLS_EC").jqGrid('setGridWidth', parent_column30.width());
            $("#JqGridLS_Emp").jqGrid('setGridWidth', parent_column31.width());
            $("#JqGridLS_ME").jqGrid('setGridWidth', parent_column32.width());
            $("#JqGridLS_MwthS").jqGrid('setGridWidth', parent_column33.width());
            $("#JqGridSciSkills").jqGrid('setGridWidth', parent_column34.width());
        }, 0);
    }
})
function SummativeAssessment1(ReqId, Cam, Grd, Sec, AcY, Sub) {
    var lastSel;
    //Summative Assessment 1
    $('#JqGridSA1').jqGrid({
        url: '/Assess360/JqGridSummativeAssessment1',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',

        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'SA1Total (90)', 'SA1 (30)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
            { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 15, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 20, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 70, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Subject', index: 'Subject', width: 15, align: 'center', editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'SA1Total', index: 'SA1Total', width: 25, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkSAvalid } },
                      { name: 'SA1', index: 'SA1', width: 15, align: 'center', editable: false, sortable: false },
                      { name: 'SA1Grade', index: 'SA1Grade', width: 10, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridSA1').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== lastSel) {
                jQuery('#JqGridSA1').restoreRow(lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridSA1').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridSA1").trigger("reloadGrid");
                        $("#JqGridSA2").trigger("reloadGrid");
                        $("#JqGridFA1").trigger("reloadGrid");
                        $("#JqGridFA2").trigger("reloadGrid");
                        $("#JqGridFA3").trigger("reloadGrid");
                        $("#JqGridFA4").trigger("reloadGrid");
                        $('#JqGridOverAll').trigger("reloadGrid");
                    });
                }
                lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSESA1',

        height: 160,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridSA1Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Term 1 Summative Assessment 1'
    });
    $("#JqGridSA1").navGrid('#JqGridSA1Pager',
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
            {},
            {}, {}, {});
    $('#JqGridSA1').jqGrid('navButtonAdd', '#JqGridSA1Pager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });
}

//resize on sidebar collapse/expand
function SummativeAssessment2(ReqId, Cam, Grd, Sec, AcY, Sub) {
    var lastSel;
    //Summative Assessment 2
    $('#JqGridSA2').jqGrid({
        url: '/Assess360/JqGridSummativeAssessment2',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'SA2Total (90)', 'SA2 (30)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
            { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 120, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Subject', index: 'Subject', width: 80, align: 'center', editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'SA2Total', index: 'SA2Total', width: 100, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkSAvalid } },
                      { name: 'SA2', index: 'SA2', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'SA2Grade', index: 'SA2Grade', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridSA2').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== lastSel) {
                jQuery('#JqGridSA2').restoreRow(lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridSA2').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridSA2").trigger("reloadGrid");
                        $("#JqGridSA1").trigger("reloadGrid");
                        $("#JqGridFA1").trigger("reloadGrid");
                        $("#JqGridFA2").trigger("reloadGrid");
                        $("#JqGridFA3").trigger("reloadGrid");
                        $("#JqGridFA4").trigger("reloadGrid");
                        $('#JqGridOverAll').trigger("reloadGrid");
                    });
                }
                lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSESA2',

        height: 160,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        shrinkToFit: true,
        viewrecords: true,
        pager: 'JqGridSA2Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Term 2 Summative Assessment 2'
    });
    $("#JqGridSA2").navGrid('#JqGridSA2Pager',
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
            {},
            {}, {}, {});

    $('#JqGridSA2').jqGrid('navButtonAdd', '#JqGridSA2Pager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });
}

function FormativeAssessment1(ReqId, Cam, Grd, Sec, AcY, Sub) {
    //Formative Assessment 1
    var lastSel;
    debugger;
    $('#JqGridFA1').jqGrid({
        url: '/Assess360/JqGridFormativeAssessment1',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'SlipTest (20)', 'SlipTestTotal (5)', 'B (5)', 'C (5)', 'D (5)', 'Total(A+B+C+D)', 'FA1 (10)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 80, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 120, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Subject', index: 'Subject', width: 80, align: 'center', editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'FA1ASlip', index: 'FA1ASlip', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA1ASlipTotal', index: 'FA1ASlipTotal', width: 70, align: 'center', editable: false, sortable: false },
                      { name: 'FA1BTotal', index: 'FA1BTotal', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA1CTotal', index: 'FA1CTotal', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA1DTotal', index: 'FA1DTotal', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA1Total', index: 'FA1Total', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'FA1', index: 'FA1', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'FA1Grade', index: 'FA1Grade', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridFA1').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== lastSel) {
                jQuery('#JqGridFA1').restoreRow(lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridFA1').editRow(id, true, '', '', '', '', function reload(rowid, result) {
                        $("#JqGridFA1").trigger("reloadGrid");
                        $("#JqGridSA1").trigger("reloadGrid");
                        $("#JqGridSA2").trigger("reloadGrid");
                        $("#JqGridFA2").trigger("reloadGrid");
                        $("#JqGridFA3").trigger("reloadGrid");
                        $("#JqGridFA4").trigger("reloadGrid");
                        $('#JqGridOverAll').trigger("reloadGrid");
                    });
                }
                lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEFA1',

        height: 160,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        shrinkToFit: true,
        viewrecords: true,
        pager: 'JqGridFA1Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Term 1 Formative Assessment 1'
    });
    $("#JqGridFA1").navGrid('#JqGridFA1Pager',
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
            {},
            {}, {}, {});

    $('#JqGridFA1').jqGrid('navButtonAdd', '#JqGridFA1Pager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridFormativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });
    jQuery("#JqGridFA1").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
                 { startColumnName: 'FA1ASlip', numberOfColumns: 2, titleText: '<em><center>A (5)</center></em>' }, ]
    });
}


function FormativeAssessment2(ReqId, Cam, Grd, Sec, AcY, Sub) {
    var lastSel;
    //Formative Assessment 2
    $('#JqGridFA2').jqGrid({
        url: '/Assess360/JqGridFormativeAssessment2',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'SlipTest (20)', 'SlipTestTotal (5)', 'B (5)', 'C (5)', 'D (5)', 'Total(A+B+C+D)', 'FA2 (10)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
             { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 150, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Subject', index: 'Subject', width: 100, align: 'center', editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'FA2ASlip', index: 'FA2ASlip', width: 100, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA2ASlipTotal', index: 'FA2ASlipTotal', width: 100, align: 'center', editable: false, sortable: false },
                      { name: 'FA2BTotal', index: 'FA2BTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA2CTotal', index: 'FA2CTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA2DTotal', index: 'FA2DTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA2Total', index: 'FA2Total', width: 120, align: 'center', editable: false, sortable: false },
                      { name: 'FA2', index: 'FA2', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'FA2Grade', index: 'FA2Grade', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridFA2').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== lastSel) {
                jQuery('#JqGridFA2').restoreRow(lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridFA2').editRow(id, true, '', '', '', '', function reload(rowid, result) {
                        $("#JqGridFA2").trigger("reloadGrid");
                        $("#JqGridSA1").trigger("reloadGrid");
                        $("#JqGridSA2").trigger("reloadGrid");
                        $("#JqGridFA1").trigger("reloadGrid");
                        $("#JqGridFA3").trigger("reloadGrid");
                        $("#JqGridFA4").trigger("reloadGrid");
                        $('#JqGridOverAll').trigger("reloadGrid");
                    });
                }
                lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEFA2',

        height: 160,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridFA2Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Term 1 Formative Assessment 2'
    });
    $("#JqGridFA2").navGrid('#JqGridFA2Pager',
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
            {},
            {}, {}, {});

    $('#JqGridFA2').jqGrid('navButtonAdd', '#JqGridFA2Pager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridFormativeAssessment2" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });

    jQuery("#JqGridFA2").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
                 { startColumnName: 'FA2ASlip', numberOfColumns: 2, titleText: '<em><center>A (5)</center></em>' }, ]
    });

}


function FormativeAssessment3(ReqId, Cam, Grd, Sec, AcY, Sub) {
    var lastSel;
    //Formative Assessment 3
    $('#JqGridFA3').jqGrid({
        url: '/Assess360/JqGridFormativeAssessment3',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'SlipTest (20)', 'SlipTestTotal (5)', 'B (5)', 'C (5)', 'D (5)', 'Total(A+B+C+D)', 'FA3 (10)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
            { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 60, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 70, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Subject', index: 'Subject', width: 80, align: 'center', editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'FA3ASlip', index: 'FA3ASlip', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA3ASlipTotal', index: 'FA3ASlipTotal', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'FA3BTotal', index: 'FA3BTotal', width: 60, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA3CTotal', index: 'FA3CTotal', width: 60, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA3DTotal', index: 'FA3DTotal', width: 60, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'FA3Total', index: 'FA3Total', width: 70, align: 'center', editable: false, sortable: false },
                      { name: 'FA3', index: 'FA3', width: 60, align: 'center', editable: false, sortable: false },
                      { name: 'FA3Grade', index: 'FA3Grade', width: 60, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridFA3').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== lastSel) {
                jQuery('#JqGridFA3').restoreRow(lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridFA3').editRow(id, true, '', '', '', '', function reload(rowid, result) {
                        $("#JqGridFA3").trigger("reloadGrid");
                        $("#JqGridSA1").trigger("reloadGrid");
                        $("#JqGridSA2").trigger("reloadGrid");
                        $("#JqGridFA1").trigger("reloadGrid");
                        $("#JqGridFA2").trigger("reloadGrid");
                        $("#JqGridFA4").trigger("reloadGrid");
                        $('#JqGridOverAll').trigger("reloadGrid");
                    });
                }
                lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEFA3',

        height: 160,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridFA3Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i>  Term 2 Formative Assessment 3'
    });
    $("#JqGridFA3").navGrid('#JqGridFA3Pager',
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
            {},
            {}, {}, {});

    $('#JqGridFA3').jqGrid('navButtonAdd', '#JqGridFA3Pager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridFormativeAssessment3" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });

    jQuery("#JqGridFA3").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
                 { startColumnName: 'FA3ASlip', numberOfColumns: 2, titleText: '<em><center>A (5)</center></em>' }, ]
    });

}

function FormativeAssessment4(ReqId, Cam, Grd, Sec, AcY, Sub) {
    //Formative Assessment 4
    var lastSel;
    $('#JqGridFA4').jqGrid({
        url: '/Assess360/JqGridFormativeAssessment4',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'SlipTest (20)', 'SlipTestTotal (5)', 'B (5)', 'C (5)', 'D (5)', 'Total(A+B+C+D)', 'FA4 (10)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
            { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                       { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                       { name: 'Name', index: 'Name', width: 200, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                       { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                       { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                       { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                       { name: 'Subject', index: 'Subject', width: 80, align: 'center', editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                       { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                       { name: 'FA4ASlip', index: 'FA4ASlip', width: 100, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                       { name: 'FA4ASlipTotal', index: 'FA4ASlipTotal', width: 100, align: 'center', editable: false, sortable: false },
                       { name: 'FA4BTotal', index: 'FA4BTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                       { name: 'FA4CTotal', index: 'FA4CTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                       { name: 'FA4DTotal', index: 'FA4DTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                       { name: 'FA4Total', index: 'FA4Total', width: 80, align: 'center', editable: false, sortable: false },
                       { name: 'FA4', index: 'FA4', width: 80, align: 'center', editable: false, sortable: false },
                       { name: 'FA4Grade', index: 'FA4Grade', width: 80, align: 'center', editable: false, sortable: false },
                        { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridFA4').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== lastSel) {
                jQuery('#JqGridFA4').restoreRow(lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridFA4').editRow(id, true, '', '', '', '', function reload(rowid, result) {
                        $("#JqGridFA4").trigger("reloadGrid");
                        $("#JqGridSA1").trigger("reloadGrid");
                        $("#JqGridSA2").trigger("reloadGrid");
                        $("#JqGridFA1").trigger("reloadGrid");
                        $("#JqGridFA2").trigger("reloadGrid");
                        $("#JqGridFA3").trigger("reloadGrid");
                        $('#JqGridOverAll').trigger("reloadGrid");
                    });
                }
                lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEFA4',

        height: 160,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridFA4Pager', loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Term 2 Formative Assessment 4'
    });
    $("#JqGridFA4").navGrid('#JqGridFA4Pager',
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
            {},
            {}, {}, {});
    $('#JqGridFA4').jqGrid('navButtonAdd', '#JqGridFA4Pager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridFormativeAssessment4" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });

    jQuery("#JqGridFA4").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
                 { startColumnName: 'FA4ASlip', numberOfColumns: 2, titleText: '<em><center>A (5)</center></em>' }, ]
    });
}

function OverAllAssessment(ReqId, Cam, Grd, Sec, AcY, Sub) {
    //Formative Assessment 4
    var lastSel;
    $('#JqGridOverAll').jqGrid({
        url: '/Assess360/JqGridOverAllAssessment',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'FA1Grade', 'FA2Grade', 'SA1Grade', 'Term1Total', 'Term1Grade', 'FA3Grade', 'FA4Grade', 'SA2Grade', 'Term2Total', 'Term2Grade', 'Result PDF'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false },
                       { name: 'PreRegNum', index: 'PreRegNum', width: 80, sortable: false },
                       { name: 'Name', index: 'Name', width: 90 },
                       { name: 'Campus', index: 'Campus', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'Grade', index: 'Grade', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'Section', index: 'Section', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'Subject', index: 'Subject', width: 80, align: 'center', sortable: false },
                       { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'FA1Grade', index: 'FA1Grade', width: 80, align: 'center', sortable: false },
                       { name: 'FA2Grade', index: 'FA2Grade', width: 80, align: 'center', sortable: false },
                       { name: 'SA1Grade', index: 'SA1Grade', width: 80, align: 'center', sortable: false },
                       { name: 'Term1Total', index: 'Term1Total', width: 80, align: 'center', sortable: false },
                       { name: 'Term1Grade', index: 'Term1Grade', width: 80, align: 'center', sortable: false },
                       { name: 'FA3Grade', index: 'FA3Grade', width: 80, align: 'center', sortable: false },
                       { name: 'FA4Grade', index: 'FA4Grade', width: 80, align: 'center', sortable: false },
                       { name: 'SA2Grade', index: 'SA2Grade', width: 80, align: 'center', sortable: false },
                       { name: 'Term2Total', index: 'Term2Total', width: 80, align: 'center', sortable: false },
                       { name: 'Term2Grade', index: 'Term2Grade', width: 80, align: 'center', sortable: false },
                       { name: 'GetPdf', index: 'GetPdf', width: 40, align: 'center' }],


        height: 160,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridOverAllPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i>  Over All'
    });
    $("#JqGridOverAll").navGrid('#JqGridOverAllPager',
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
            {},
            {}, {}, {});
    jQuery("#JqGridOverAll").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
                 { startColumnName: 'FA1Grade', numberOfColumns: 4, titleText: '<em><center>Term I</center></em>' }, { startColumnName: 'FA3Grade', numberOfColumns: 4, titleText: '<em><center>Term II</center></em>' }, ]
    });
}


function CommonEntry1(ReqId, Cam, Grd, Sec, AcY, NofDaysTerm1) {
    var C1lastSel;
    //Common Entry Term 1
    $('#JqGridCommonTerm1').jqGrid({
        url: '/Assess360/JqGridCommonTerm1',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, NofDays: NofDaysTerm1 },
        datatype: 'Json',
        type: 'POST',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Total Working Day', 'Total Attendence', 'Height (cm)', 'Weight (Kg)', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate', 'Result PDF'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
            { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 200, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'TofWorkingDayT1', index: 'TofWorkingDayT1', width: 80, align: 'center', editable: true, sortable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'TotalAttendenceT1', index: 'TotalAttendenceT1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCommonValue } },
                      { name: 'HeightT1', index: 'HeightT1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCommonValue } },
                      { name: 'WeightT1', index: 'WeightT1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCommonValue } },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true },
                      { name: 'GetPdf', index: 'GetPdf', width: 80, align: 'center' }],


        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridCommonTerm1').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== C1lastSel) {
                jQuery('#JqGridCommonTerm1').restoreRow(C1lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridCommonTerm1').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridCommonTerm1").trigger("reloadGrid");
                        $("#JqGridCommonTerm2").trigger("reloadGrid");
                    });
                }
                C1lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidateCommon1(postdata) == false) { return false; }
            else { return postdata; }
            //return true;
        },
        editurl: '/Assess360/SaveReportCardCBSECommonTerm1',
        height: 150,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridCommonTerm1Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i>  ReportCard Common Entry Term1'
    });
    $("#JqGridCommonTerm1").navGrid('#JqGridCommonTerm1Pager',
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
            {},
            {}, {}, {});
}

function CommonEntry2(ReqId, Cam, Grd, Sec, AcY, NofDaysTerm2) {
    var C2lastSel;
    //Common Entry Term 1
    $('#JqGridCommonTerm2').jqGrid({
        url: '/Assess360/JqGridCommonTerm2',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, NofDays: NofDaysTerm2 },
        datatype: 'Json',
        type: 'POST',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Total Working Day', 'Total Attendence', 'Height (cm)', 'Weight (Kg)', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate', 'Result PDF'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
            { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Name', index: 'Name', width: 200, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'TofWorkingDayT2', index: 'TofWorkingDayT2', width: 80, align: 'center', editable: true, sortable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'TotalAttendenceT2', index: 'TotalAttendenceT2', width: 100, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCommonValue } },
                      { name: 'HeightT2', index: 'HeightT2', width: 60, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCommonValue } },
                      { name: 'WeightT2', index: 'WeightT2', width: 60, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCommonValue } },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true },
                      { name: 'GetPdf', index: 'GetPdf', width: 80, align: 'center' }],

        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridCommonTerm2').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== C2lastSel) {
                jQuery('#JqGridCommonTerm2').restoreRow(C2lastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridCommonTerm2').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridCommonTerm2").trigger("reloadGrid");
                        $("#JqGridCommonTerm1").trigger("reloadGrid");
                    });
                }
                C2lastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidateCommon2(postdata) == false) { return false; }
            else { return postdata; }
        },
        editurl: '/Assess360/SaveReportCardCBSECommonTerm2',
        height: 150,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridCommonTerm2Pager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> ReportCard Common Entry Term2'
    });
    $("#JqGridCommonTerm2").navGrid('#JqGridCommonTerm2Pager',
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
            {},
            {}, {}, {});
}


function HealthPET(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Work Education
    $('#JqGridHealthPET').jqGrid({
        url: '/Assess360/JqGridHealthPET',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',

        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Demonstrates physical fitness and agility', 'Displays courage and determination', 'Demonstrates flexibility of the body', 'Demonstrates sportsmanship', 'Follows all safety norms of games and sports', 'Follows rules of the games',
        'Has undergone training and coaching in the chosen sports and games items', 'Makes strategic decisions within the games', 'Organizes and provides leadership in this area', 'Takes initiative and interestin physical Education and Wellness', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'HPET_1', index: 'HPET_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_2', index: 'HPET_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_3', index: 'HPET_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_4', index: 'HPET_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_5', index: 'HPET_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_6', index: 'HPET_6', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_7', index: 'HPET_7', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_8', index: 'HPET_8', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_9', index: 'HPET_9', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_10', index: 'HPET_10', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'HPET_Total', index: 'HPET_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'HPET_Average', index: 'HPET_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'HPET_Grade', index: 'HPET_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridHealthPET').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridHealthPET').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridHealthPET').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridHealthPET").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEHealthAndPet',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,

        pager: 'JqGridHealthPETPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i>  Health and PET'
    });
    $("#JqGridHealthPET").navGrid('#JqGridHealthPETPager',
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
            {},
            {}, {}, {});
    //    $('#JqGridHealthPET').jqGrid('navButtonAdd', '#JqGridHealthPETPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function ICT(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Information & Communication Technology (ICT)
    $('#JqGridICT').jqGrid({
        url: '/Assess360/JqGridICT',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Is efficient in handling IT equipments & gadgets', 'Has a step by step approach to solving problem', 'Is able to apply theoritical knowledge into practical usage', 'Adheres to activity and project time lines', 'Takes initiative to organise & participate in IT activities', 'Takes keen interest in computer related activities',
        'Is helpful, guides & facilitates others', 'Is a keen observer & is able to make decisions', 'Is innovative in ideas', 'Adheres to ethical norms of using technology', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'ICT_1', index: 'ICT_1', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_2', index: 'ICT_2', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_3', index: 'ICT_3', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_4', index: 'ICT_4', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_5', index: 'ICT_5', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_6', index: 'ICT_6', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_7', index: 'ICT_7', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_8', index: 'ICT_8', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_9', index: 'ICT_9', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_10', index: 'ICT_10', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'ICT_Total', index: 'ICT_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'ICT_Average', index: 'ICT_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'ICT_Grade', index: 'ICT_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridICT').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridICT').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridICT').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridICT").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEICT',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        pager: 'JqGridICTPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Information & Communication Technology (ICT)'
    });
    $("#JqGridICT").navGrid('#JqGridICTPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridICT').jqGrid('navButtonAdd', '#JqGridICTPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function ScientificSkills(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Information & Communication Technology (ICT)
    $('#JqGridSciSkills').jqGrid({
        url: '/Assess360/JqGridSciSkills',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Verifies existing knowledge before accepting', 'Does not get carried away by rumours and media reports', 'Tries to find new and more effective solutions to problems', 'Conducts experiments with efficiency and effectiveness', 'Takes keen interest in scientific activities in laboratory and feild-based experiment at school, inter-school, state, national and international level',
            'Takes the initiative to plan, organise and evaluate various science-related events like quizzes,Seminars,model making etc', 'Shows a high degree of curiosity and reads science related literature', 'Is a keen observer & is able to make decisions', 'Displays good experiment skills and a practical knowledge of every day phenomena', 'Makes use of technology in marking projects and models', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                   { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                   { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                   { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                   { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                   { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                   { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                   { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                   { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                   { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                   { name: 'SciSkills_1', index: 'SciSkills_1', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_2', index: 'SciSkills_2', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_3', index: 'SciSkills_3', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_4', index: 'SciSkills_4', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_5', index: 'SciSkills_5', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_6', index: 'SciSkills_6', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_7', index: 'SciSkills_7', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_8', index: 'SciSkills_8', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_9', index: 'SciSkills_9', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_10', index: 'SciSkills_10', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                   { name: 'SciSkills_Total', index: 'SciSkills_Total', width: 40, align: 'center', editable: false, sortable: false },
                   { name: 'SciSkills_Average', index: 'SciSkills_Average', width: 50, align: 'center', editable: false, sortable: false },
                   { name: 'SciSkills_Grade', index: 'SciSkills_Grade', width: 40, align: 'center', editable: false, sortable: false },
                   { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                   { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                   { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                   { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                   { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridSciSkills').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridSciSkills').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridSciSkills').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridSciSkills").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSESciSkills',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        pager: 'JqGridSciSkillsPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Scientific Skills'
    });
    $("#JqGridSciSkills").navGrid('#JqGridSciSkillsPager',
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
            {},
            {}, {}, {});
}

function WorkEducation(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Work Education
    $('#JqGridWorkEdu').jqGrid({
        url: '/Assess360/JqGridCoSchWorkandEducation',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Has collabrative approach towards the process of learning', 'Is innovative in ideas', 'Plans and adhers to timeline', 'Is involved and motivated', 'Demonstrates a positive attitude', 'Is helpful,guides and help others', 'Demonstrates understanding of correction with real life situation', 'Has a step by step approach to solve a problem', 'Has clear understanding of output to be generated', 'Is able to apply the theoritical knowleadgento practical usage', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'WE_1', index: 'WE_1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_2', index: 'WE_2', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_3', index: 'WE_3', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_4', index: 'WE_4', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_5', index: 'WE_5', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_6', index: 'WE_6', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_7', index: 'WE_7', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_8', index: 'WE_8', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_9', index: 'WE_9', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_10', index: 'WE_10', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'WE_Total', index: 'WE_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'WE_Average', index: 'WE_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'WE_Grade', index: 'WE_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridWorkEdu').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridWorkEdu').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridWorkEdu').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridWorkEdu").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEWE',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        pager: 'JqGridWorkEduPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Work Education'
    });
    $("#JqGridWorkEdu").navGrid('#JqGridWorkEduPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridWorkEdu').jqGrid('navButtonAdd', '#JqGridWorkEduPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function LiteraryandCreSkill(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Literary and Creative Skills 
    $('#JqGridLitandCreSkill').jqGrid({
        url: '/Assess360/JqGridCoSchLiteraryCreSkill',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Compose poems or lyrics', ' Writes short stories', 'Writes literary criticism', 'Participates actively in literary and creative activities at school, interschool, state, national and international levels', 'Plans and organizes literary events like debates, recitation, book clubs etc'
 , 'Reads books and shows a high degree of awareness in the field of literature', 'Appreciates well written or spoken pieces  representing various genres', 'Expresses ideas and opinions creatively in different forms',
 'Displays originality of ideas and opinions', 'Is able to inspire others and involve a large part of the school and community in different events ', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LndCS_1', index: 'LndCS_1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_2', index: 'LndCS_2', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_3', index: 'LndCS_3', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_4', index: 'LndCS_4', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_5', index: 'LndCS_5', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_6', index: 'LndCS_6', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_7', index: 'LndCS_7', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_8', index: 'LndCS_8', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_9', index: 'LndCS_9', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_10', index: 'LndCS_10', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LndCS_Total', index: 'LndCS_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LndCS_Average', index: 'LndCS_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'LndCS_Grade', index: 'LndCS_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLitandCreSkill').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLitandCreSkill').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLitandCreSkill').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLitandCreSkill").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELitandCreSkill',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        shrinkToFit: true,
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,
        pager: 'JqGridLitandCreSkillPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i>  Literary and Creative Skills'
    });
    $("#JqGridLitandCreSkill").navGrid('#JqGridLitandCreSkillPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridLitandCreSkill').jqGrid('navButtonAdd', '#JqGridLitandCreSkillPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function ValuesSystem1(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///To Abide by the Constitution in Values System
    $('#JqGridVS_ToABC').jqGrid({
        url: '/Assess360/JqGridVS_ToABC',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Is aware of the Directive Principle and Fundamental Rights', 'Sings National anthem with de corum', 'Attends hoisting of National Flag with respect & decorum', 'Understands the meaning of tri-colour & the Ashok Chakra', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 70, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'VS_ToABC_1', index: 'VS_ToABC_1', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToABC_2', index: 'VS_ToABC_2', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToABC_3', index: 'VS_ToABC_3', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToABC_4', index: 'VS_ToABC_4', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToABC_Total', index: 'VS_ToABC_Total', width: 20, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToABC_Average', index: 'VS_ToABC_Average', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToABC_Grade', index: 'VS_ToABC_Grade', width: 20, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridVS_ToABC').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridVS_ToABC').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridVS_ToABC').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridVS_ToABC").trigger("reloadGrid");
                        $("#JqGridVS_ToCFNI").trigger("reloadGrid");
                        $("#JqGridVS_ToUPSUI").trigger("reloadGrid");
                        $("#JqGridVS_ToRNSWCU").trigger("reloadGrid");
                        $("#JqGridVS_ToPHUB").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSETOABC',

        height: 210,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridVS_ToABCPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> To Abide by the Constitution'
    });
    $("#JqGridVS_ToABC").navGrid('#JqGridVS_ToABCPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridVS_ToABC').jqGrid('navButtonAdd', '#JqGridVS_ToABCPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function ValuesSystem2(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///To Cherish and Follow the Noble Ideas in Values System
    $('#JqGridVS_ToCFNI').jqGrid({
        url: '/Assess360/JqGridVS_ToCFNI',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Takes interest in the National Freedom Struggle', 'Displays pride in being an Indian Citizen', 'Celebrates Republic Day & Independence day with enthusiasm', 'Reads biographies of freedom fighters', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 15, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'VS_ToCFNI_1', index: 'VS_ToCFNI_1', width: 15, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToCFNI_2', index: 'VS_ToCFNI_2', width: 15, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToCFNI_3', index: 'VS_ToCFNI_3', width: 15, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToCFNI_4', index: 'VS_ToCFNI_4', width: 15, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToCFNI_Total', index: 'VS_ToCFNI_Total', width: 5, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToCFNI_Average', index: 'VS_ToCFNI_Average', width: 10, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToCFNI_Grade', index: 'VS_ToCFNI_Grade', width: 5, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridVS_ToCFNI').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridVS_ToCFNI').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridVS_ToCFNI').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridVS_ToCFNI").trigger("reloadGrid");
                        $("#JqGridVS_ToUPSUI").trigger("reloadGrid");
                        $("#JqGridVS_ToRNSWCU").trigger("reloadGrid");
                        $("#JqGridVS_ToPHUB").trigger("reloadGrid");
                        $("#JqGridVS_ToABC").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSETOCFNI',
        height: 210,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridVS_ToCFNIPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> To Cherish and Follow the Noble Ideas'
    });
    $("#JqGridVS_ToCFNI").navGrid('#JqGridVS_ToCFNI',
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
            {},
            {}, {}, {});

    //    $('#JqGridVS_ToCFNI').jqGrid('navButtonAdd', '#JqGridVS_ToCFNIPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function ValuesSystem3(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///To Upload & Project the Sovereignty, Unity & Intergrity in Values Systems
    $('#JqGridVS_ToUPSUI').jqGrid({
        url: '/Assess360/JqGridVS_ToUPSUI',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Raises voice against divisive forces', 'Respects armed forces & paramilitary forces', 'Respects Indian diversity', 'Maintains peace and love', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 40, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 15, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 15, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'VS_ToUPSUI_1', index: 'VS_ToUPSUI_1', width: 10, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToUPSUI_2', index: 'VS_ToUPSUI_2', width: 10, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToUPSUI_3', index: 'VS_ToUPSUI_3', width: 10, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToUPSUI_4', index: 'VS_ToUPSUI_4', width: 10, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToUPSUI_Total', index: 'VS_ToUPSUI_Total', width: 5, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToUPSUI_Average', index: 'VS_ToUPSUI_Average', width: 9, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToUPSUI_Grade', index: 'VS_ToUPSUI_Grade', width: 5, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridVS_ToUPSUI').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridVS_ToUPSUI').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridVS_ToUPSUI').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridVS_ToUPSUI").trigger("reloadGrid");
                        $("#JqGridVS_ToRNSWCU").trigger("reloadGrid");
                        $("#JqGridVS_ToPHUB").trigger("reloadGrid");
                        $("#JqGridVS_ToABC").trigger("reloadGrid");
                        $("#JqGridVS_ToCFNI").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSETOUPSUI',

        height: 210,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,

        pager: 'JqGridVS_ToUPSUIPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> To Upload & Project the Sovereignty, Unity & Intergrity'
    });
    $("#JqGridVS_ToUPSUI").navGrid('#JqGridVS_ToUPSUIPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridVS_ToUPSUI').jqGrid('navButtonAdd', '#JqGridVS_ToUPSUIPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function ValuesSystem4(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///To Render National Service when Called Upon in Values Systems
    $('#JqGridVS_ToRNSWCU').jqGrid({
        url: '/Assess360/JqGridVS_ToRNSWCU',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Shows a procative and responsible behaviour during crises', 'Helpful towards disadvantaged section of the society', 'Renders social work enthusiastically', 'Actively participates in community development programmes of the school', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 150, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'VS_ToRNSWCU_1', index: 'VS_ToRNSWCU_1', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToRNSWCU_2', index: 'VS_ToRNSWCU_2', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToRNSWCU_3', index: 'VS_ToRNSWCU_3', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToRNSWCU_4', index: 'VS_ToRNSWCU_4', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToRNSWCU_Total', index: 'VS_ToRNSWCU_Total', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToRNSWCU_Average', index: 'VS_ToRNSWCU_Average', width: 60, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToRNSWCU_Grade', index: 'VS_ToRNSWCU_Grade', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridVS_ToRNSWCU').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridVS_ToRNSWCU').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridVS_ToRNSWCU').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridVS_ToRNSWCU").trigger("reloadGrid");
                        $("#JqGridVS_ToPHUB").trigger("reloadGrid");
                        $("#JqGridVS_ToABC").trigger("reloadGrid");
                        $("#JqGridVS_ToCFNI").trigger("reloadGrid");
                        $("#JqGridVS_ToUPSUI").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSETORNSWCU',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: false,
        autowidth: true,
        pager: 'JqGridVS_ToRNSWCUPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> To Render National Service when Called Upon'
    });
    $("#JqGridVS_ToRNSWCU").navGrid('#JqGridVS_ToRNSWCUPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridVS_ToRNSWCU').jqGrid('navButtonAdd', '#JqGridVS_ToRNSWCUPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function ValuesSystem5(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///To Promote Harmony, Unity & BrotherHood in Values Systems
    $('#JqGridVS_ToPHUB').jqGrid({
        url: '/Assess360/JqGridVS_ToPHUB',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Respects opposite gender', 'Respects teacher from different religious & linguistic communities', 'Takes up issues in case of indignity to women', 'Kind & helpful to others', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 150, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'VS_ToPHUB_1', index: 'VS_ToPHUB_1', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToPHUB_2', index: 'VS_ToPHUB_2', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToPHUB_3', index: 'VS_ToPHUB_3', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToPHUB_4', index: 'VS_ToPHUB_4', width: 200, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VS_ToPHUB_Total', index: 'VS_ToPHUB_Total', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToPHUB_Average', index: 'VS_ToPHUB_Average', width: 60, align: 'center', editable: false, sortable: false },
                      { name: 'VS_ToPHUB_Grade', index: 'VS_ToPHUB_Grade', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridVS_ToPHUB').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridVS_ToPHUB').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridVS_ToPHUB').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridVS_ToPHUB").trigger("reloadGrid");
                        $("#JqGridVS_ToABC").trigger("reloadGrid");
                        $("#JqGridVS_ToCFNI").trigger("reloadGrid");
                        $("#JqGridVS_ToUPSUI").trigger("reloadGrid");
                        $("#JqGridVS_ToRNSWCU").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSETOPHUB',
        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: false,
        autowidth: true,
        pager: 'JqGridVS_ToPHUBPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> To Promote Harmony, Unity & BrotherHood'
    });
    $("#JqGridVS_ToPHUB").navGrid('#JqGridVS_ToPHUBPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridVS_ToPHUB').jqGrid('navButtonAdd', '#JqGridVS_ToPHUBPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function Attitude1(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Attitude Towards Teachers in attitudes
    $('#JqGridAT_AToT').jqGrid({
        url: '/Assess360/JqGridAT_AToT',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Shows decency and courtesy to teachers inside and outside the class', 'Demonstrates positive attitudes towards learning', 'Takes suggestions and criticism in the right spirit', 'Respects teachers’ instructions', 'Accepts norms and rules of the school',
         'Communicates his / her thoughts with teachers', 'Confides his / her problems with teachers', 'Shows honesty and sincerity towards teachers', 'Feels free to ask questions', 'Helpful to teachers', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                   { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                  { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                  { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                  { name: 'Name', index: 'Name', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                  { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'AT_AToT_1', index: 'AT_AToT_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_2', index: 'AT_AToT_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_3', index: 'AT_AToT_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_4', index: 'AT_AToT_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_5', index: 'AT_AToT_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_6', index: 'AT_AToT_6', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_7', index: 'AT_AToT_7', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_8', index: 'AT_AToT_8', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_9', index: 'AT_AToT_9', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_10', index: 'AT_AToT_10', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToT_Total', index: 'AT_AToT_Total', width: 40, align: 'center', editable: false, sortable: false },
                  { name: 'AT_AToT_Average', index: 'AT_AToT_Average', width: 45, align: 'center', editable: false, sortable: false },
                  { name: 'AT_AToT_Grade', index: 'AT_AToT_Grade', width: 40, align: 'center', editable: false, sortable: false },
                  { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                  { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                  { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                  { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                  { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridAT_AToT').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridAT_AToT').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridAT_AToT').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridAT_AToT").trigger("reloadGrid");
                        $("#JqGridAT_AToSM").trigger("reloadGrid");
                        $("#JqGridAT_AToSPE").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEAT_AToT',

        height: 210,
        //rowNum: 50,
        autowidth: true,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        pager: 'JqGridAT_AToTPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Attitude Towards Teachers'
    });
    $("#JqGridAT_AToT").navGrid('#JqGridAT_AToTPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridAT_AToT').jqGrid('navButtonAdd', '#JqGridAT_AToTPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function Attitude2(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Attitude Towards School Mates in Attitude
    $('#JqGridAT_AToSM').jqGrid({
        url: '/Assess360/JqGridAT_AToSM',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Is friendly with most of the classmates', 'Expresses ideas and opinionsfreely in a group', 'Is receptive to ideas and opinions of others', 'Treats classmates as equals without any sense of superiority or inferiority',
                    'Sensitive and supporative towards peers and differently abled schoolmates', 'Treats peers from different school,religion and economic background with out any discrimination', 'Respect opposite gender and is comfortable in their company', 'Does not bully others', 'Deals with aggressive behaviour by peers tactfully',
                    'Shares credit and praise with team members and peers ', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                   { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                  { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                  { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                  { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                  { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                  { name: 'AT_AToSM_1', index: 'AT_AToSM_1', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_2', index: 'AT_AToSM_2', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_3', index: 'AT_AToSM_3', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_4', index: 'AT_AToSM_4', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_5', index: 'AT_AToSM_5', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_6', index: 'AT_AToSM_6', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_7', index: 'AT_AToSM_7', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_8', index: 'AT_AToSM_8', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_9', index: 'AT_AToSM_9', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_10', index: 'AT_AToSM_10', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                  { name: 'AT_AToSM_Total', index: 'AT_AToSM_Total', width: 30, align: 'center', editable: false, sortable: false },
                  { name: 'AT_AToSM_Average', index: 'AT_AToSM_Average', width: 35, align: 'center', editable: false, sortable: false },
                  { name: 'AT_AToSM_Grade', index: 'AT_AToSM_Grade', width: 30, align: 'center', editable: false, sortable: false },
                  { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                  { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                  { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                  { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                  { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridAT_AToSM').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridAT_AToSM').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridAT_AToSM').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridAT_AToSM").trigger("reloadGrid");
                        $("#JqGridAT_AToSPE").trigger("reloadGrid");
                        $("#JqGridAT_AToT").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEAT_AToSM',

        height: 210,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        pager: 'JqGridAT_AToSMPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Attitude Towards School Mates'
    });
    $("#JqGridAT-AToSM").navGrid('#JqGridAT_AToSMPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridAT_AToSM').jqGrid('navButtonAdd', '#JqGridAT_AToSMPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            // window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function Attitude3(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Attitude Towards School Programme & Environment in Attitude
    $('#JqGridAT_AToSPE').jqGrid({
        url: '/Assess360/JqGridAT_AToSPE',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Attaches a lot of importance to school activities and programmes', 'Participates in school activities relating to improvement of environment', 'Enthusiastically participates in school programmes', 'Shoulders responsibility happily', 'Confronts anyone who criticizes school and school programmes',
                       'Insists on parents to participate/witness school programmes', 'Participates in community activities relating to environment', 'Takes care of school property', 'Sensitive and concerned about environmental degradation', 'Takes initiative in planning activities for the betterment of the environment', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 60, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AT_AToSPE_1', index: 'AT_AToSPE_1', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_2', index: 'AT_AToSPE_2', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_3', index: 'AT_AToSPE_3', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_4', index: 'AT_AToSPE_4', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_5', index: 'AT_AToSPE_5', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_6', index: 'AT_AToSPE_6', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_7', index: 'AT_AToSPE_7', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_8', index: 'AT_AToSPE_8', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_9', index: 'AT_AToSPE_9', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_10', index: 'AT_AToSPE_10', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'AT_AToSPE_Total', index: 'AT_AToSPE_Total', width: 20, align: 'center', editable: false, sortable: false },
                      { name: 'AT_AToSPE_Average', index: 'AT_AToSPE_Average', width: 25, align: 'center', editable: false, sortable: false },
                      { name: 'AT_AToSPE_Grade', index: 'AT_AToSPE_Grade', width: 20, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridAT_AToSPE').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridAT_AToSPE').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridAT_AToSPE').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridAT_AToSPE").trigger("reloadGrid");
                        $("#JqGridAT_AToT").trigger("reloadGrid");
                        $("#JqGridAT_AToSM").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEAT_AToSPE',

        height: 210,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        pager: 'JqGridAT_AToSPEPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Attitude Towards School Programme & Environment'
    });
    $("#JqGridAT_AToSPE").navGrid('#JqGridAT_AToSPEPager',
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
            {},
            {}, {}, {});

    $("#JqGridAT_AToSPE").navGrid('#JqGridAT_AToSPEPager', { add: false, edit: false, del: false, search: false, refresh: false });
    //    $('#JqGridAT_AToSPE').jqGrid('navButtonAdd', '#JqGridAT_AToSPEPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel", buttonicon: "ui-icon-print",
    //        onClickButton: function () {
    //            // window.open("/Assess360/JqGridAT_AToSPE" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&rows=99999' + '&ExportType=Excel');
    //            // alert();
    //        }
    //    });
}


function PerformingArts(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Performing Arts
    $('#JqGridPerformingArts').jqGrid({
        url: '/Assess360/JqGridPerformingArts',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Signs and plays insrtumental music', 'Dances and acts in drama', 'Awarenessand appreciation of works of artists', 'Demonstrates appreciation skills', 'Participates actively in aesthestic activities of various level',
                       'Takes intiative to plan, create and direct various creative events', 'Reads and shows degree of awareness of particular domain of Art', 'Experiments with art forms', 'Shows a high degree of imagination and innovation', 'Displays artistic temperament in all of his/her action in school and outside', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 70, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'PA_1', index: 'PA_1', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_2', index: 'PA_2', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_3', index: 'PA_3', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_4', index: 'PA_4', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_5', index: 'PA_5', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_6', index: 'PA_6', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_7', index: 'PA_7', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_8', index: 'PA_8', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_9', index: 'PA_9', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_10', index: 'PA_10', width: 75, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'PA_Total', index: 'PA_Total', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'PA_Average', index: 'PA_Average', width: 35, align: 'center', editable: false, sortable: false },
                      { name: 'PA_Grade', index: 'PA_Grade', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridPerformingArts').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridPerformingArts').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridPerformingArts').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridPerformingArts").trigger("reloadGrid");
                        $("#JqGridVisualArts").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEPerformingArts',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        shrinkToFit: true,
        viewrecords: true,
        autowidth: true,
        pager: 'JqGridPerformingArtsPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Performing Arts'
    });
    $("#JqGridPerformingArts").navGrid('#JqGridPerformingArtsPager',
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
            {},
            {}, {}, {});

    $("#JqGridPerformingArts").navGrid('#JqGridAT_AToSPEPager', { add: false, edit: false, del: false, search: false, refresh: false });
    //    $('#JqGridPerformingArts').jqGrid('navButtonAdd', '#JqGridPerformingArtsPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            // window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function VisualArts(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Visual Arts
    $('#JqGridVisualArts').jqGrid({
        url: '/Assess360/JqGridVisualArts',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Takes an intiative and creative approach', 'Shows ashethetic sensibility', 'Displays observational skills', 'Demonstrate interprotation and originality', 'Correlates with real life',
                       'Shows willingness to experiements with different arts modes and medium', 'Sketches and paints', 'Generates computer animation', 'Demonstrates preparation in size and clarity', 'Understands the importance of colour balance , bright and brightness', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 85, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'VA_1', index: 'VA_1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_2', index: 'VA_2', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_3', index: 'VA_3', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_4', index: 'VA_4', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_5', index: 'VA_5', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_6', index: 'VA_6', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_7', index: 'VA_7', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_8', index: 'VA_8', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_9', index: 'VA_9', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_10', index: 'VA_10', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'VA_Total', index: 'VA_Total', width: 20, align: 'center', editable: false, sortable: false },
                      { name: 'VA_Average', index: 'VA_Average', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'VA_Grade', index: 'VA_Grade', width: 20, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridVisualArts').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridVisualArts').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridVisualArts').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridVisualArts").trigger("reloadGrid");
                        $("#JqGridPerformingArts").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSEVisualArts',

        height: 180,
        autowidth: true,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,

        pager: 'JqGridVisualArtsPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Visual Arts'
    });
    $("#JqGridVisualArts").navGrid('#JqGridVisualArtsPager',
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
            {},
            {}, {}, {});

    $('#JqGridVisualArts').jqGrid('navButtonAdd', '#JqGridVisualArtsPager', {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });
}

function LifeSkills1(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///self Awareness in soft skills
    $('#JqGridLS_SA').jqGrid({
        url: '/Assess360/JqGridLS_SA',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Is aware of his/her physical/social and emotional self', 'Self Respecting', 'Aware of his / her strength and weakness', 'Adopts optimistic approach', 'Has the confidence to face challenges', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 30, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_SA_1', index: 'LS_SA_1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_SA_2', index: 'LS_SA_2', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_SA_3', index: 'LS_SA_3', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_SA_4', index: 'LS_SA_4', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_SA_5', index: 'LS_SA_5', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_SA_Total', index: 'LS_SA_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_SA_Average', index: 'LS_SA_Average', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_SA_Grade', index: 'LS_SA_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_SA').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_SA').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_SA').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_SA',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        pager: 'JqGridLS_SAPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Self Awareness'
    });
    $("#JqGridLS_SA").navGrid('#JqGridLS_SAPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridLS_SA').jqGrid('navButtonAdd', '#JqGridLS_SAPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            // window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function LifeSkills2(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Problem solving in soft skills
    $('#JqGridLS_PS').jqGrid({
        url: '/Assess360/JqGridLS_PS',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Finds the workable solution to the problem', 'Handles various problem effectively', 'Identifies and states the problem', 'Views problems as a steping stone to success', 'Finds ways to solve different kinds of conflicts', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_PS_1', index: 'LS_PS_1', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_PS_2', index: 'LS_PS_2', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_PS_3', index: 'LS_PS_3', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_PS_4', index: 'LS_PS_4', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_PS_5', index: 'LS_PS_5', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_PS_Total', index: 'LS_PS_Total', width: 35, align: 'center', editable: false, sortable: false },
                      { name: 'LS_PS_Average', index: 'LS_PS_Average', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_PS_Grade', index: 'LS_PS_Grade', width: 35, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_PS').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_PS').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_PS').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_PS',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        shrinkToFit: true,
        viewrecords: true,
        autowidth: true,
        pager: 'JqGridLS_PSPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Problem Solving'
    });
    $("#JqGridLS_PS").navGrid('#JqGridLS_PSPager',
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
            {},
            {}, {}, {});

    $("#JqGridLS_PS").navGrid('#JqGridLS_PSPager', { add: false, edit: false, del: false, search: false, refresh: false });
    //    $('#JqGridLS_PS').jqGrid('navButtonAdd', '#JqGridLS_PSPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            // window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function LifeSkills3(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Decision Making in soft skills
    $('#JqGridLS_DM').jqGrid({
        url: '/Assess360/JqGridLS_DM',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Is decisive and convincing', 'Is able to find different alternative to olve problems', 'Analyse the alternactive critically', 'Take decision logically', 'Shows readiness to face challenges', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 75, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_DM_1', index: 'LS_DM_1', width: 65, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_DM_2', index: 'LS_DM_2', width: 65, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_DM_3', index: 'LS_DM_3', width: 65, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_DM_4', index: 'LS_DM_4', width: 65, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_DM_5', index: 'LS_DM_5', width: 65, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_DM_Total', index: 'LS_DM_Total', width: 35, align: 'center', editable: false, sortable: false },
                      { name: 'LS_DM_Average', index: 'LS_DM_Average', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_DM_Grade', index: 'LS_DM_Grade', width: 35, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_DM').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_DM').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_DM').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_DM',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        pager: 'JqGridLS_DMPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Decision Making'
    });
    $("#JqGridLS_DM").navGrid('#JqGridLS_DMPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridLS_DM').jqGrid('navButtonAdd', '#JqGridLS_DMPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function LifeSkills4(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Critical Thinking in soft skills
    $('#JqGridLS_CriT').jqGrid({
        url: '/Assess360/JqGridLS_CriT',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Assesses the statement and arguments', 'Examines the problems closely', 'Listens carefully and gives feedback', 'Tries to find out alternatives and solutions ', 'Questions relevantly', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 10, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_CriT_1', index: 'LS_CriT_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CriT_2', index: 'LS_CriT_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CriT_3', index: 'LS_CriT_3', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CriT_4', index: 'LS_CriT_4', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CriT_5', index: 'LS_CriT_5', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CriT_Total', index: 'LS_CriT_Total', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'LS_CriT_Average', index: 'LS_CriT_Average', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_CriT_Grade', index: 'LS_CriT_Grade', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_CriT').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_CriT').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_CriT').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_CriT',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        autowidth: true,
        shrinkToFit: true,
        pager: 'JqGridLS_CriTPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Critical Thinking'
    });
    $("#JqGridLS_CriT").navGrid('#JqGridLS_CriTPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridLS_CriT').jqGrid('navButtonAdd', '#JqGridLS_CriTPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });

}

function LifeSkills5(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Creative Thinking in soft skills
    $('#JqGridLS_CreT').jqGrid({
        url: '/Assess360/JqGridLS_CreT',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Ability  to find creative and constructive solutions to problem', 'Is independent in thinking', 'Has fluency inexpression', 'Has rich imagination and is able to think out of the box ', 'Can make Independent judgement in crucial matters', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 100, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_CreT_1', index: 'LS_CreT_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CreT_2', index: 'LS_CreT_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CreT_3', index: 'LS_CreT_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CreT_4', index: 'LS_CreT_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CreT_5', index: 'LS_CreT_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_CreT_Total', index: 'LS_CreT_Total', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'LS_CreT_Average', index: 'LS_CreT_Average', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_CreT_Grade', index: 'LS_CreT_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_CreT').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_CreT').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_CreT').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_CreT',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        pager: 'JqGridLS_CreTPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Creative Thinking'
    });
    $("#JqGridLS_CreT").navGrid('#JqGridLS_CreTPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridLS_CreT').jqGrid('navButtonAdd', '#JqGridLS_CreTPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function LifeSkills6(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Interpersonal Relationalship in soft skills
    $('#JqGridLS_IR').jqGrid({
        url: '/Assess360/JqGridLS_IR',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Interacts effectively with peers and teachers', 'Is very cheerful and friendly', 'Exhibits fine etiquettes and other social skills', 'Finds it natural and easy to share and discuss the feelings with other', 'Responsive to other interest and concerns', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 70, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 90, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_IR_1', index: 'LS_IR_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_IR_2', index: 'LS_IR_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_IR_3', index: 'LS_IR_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_IR_4', index: 'LS_IR_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_IR_5', index: 'LS_IR_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_IR_Total', index: 'LS_IR_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_IR_Average', index: 'LS_IR_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'LS_IR_Grade', index: 'LS_IR_Grade', width: 30, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_IR').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_IR').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_IR').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_IR',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        pager: 'JqGridLS_IRPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Interpersonal Relationalship'
    });
    $("#JqGridLS_IR").navGrid('#JqGridLS_IRPager',
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
            {},
            {}, {}, {});

    //    $('#JqGridLS_IR').jqGrid('navButtonAdd', '#JqGridLS_IRPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            // window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function LifeSkills7(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Effective Communication in soft skills
    $('#JqGridLS_EC').jqGrid({
        url: '/Assess360/JqGridLS_EC',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Contribute frequently to group conversions', 'knows the difference  between assertive, aggressive,and submissivemanners of communication', 'Is able to make use of speech,action and expression while', 'Exhibts  good listening skills', 'user gestures,facial expressions and vioce intonation to emphasize points', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_EC_1', index: 'LS_EC_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_EC_2', index: 'LS_EC_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_EC_3', index: 'LS_EC_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_EC_4', index: 'LS_EC_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_EC_5', index: 'LS_EC_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_EC_Total', index: 'LS_EC_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_EC_Average', index: 'LS_EC_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'LS_EC_Grade', index: 'LS_EC_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_EC').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_EC').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_EC').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_EC',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        shrinkToFit: true,
        viewrecords: true,
        autowidth: true,
        pager: 'JqGridVS_ToABCPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Effective Communication'
    });
    $("#JqGridLS_EC").navGrid('#JqGridLS_ECPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridLS_EC').jqGrid('navButtonAdd', '#JqGridLS_ECPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //  window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function LifeSkills8(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Empathy in soft skills
    $('#JqGridLS_Emp').jqGrid({
        url: '/Assess360/JqGridLS_Emp',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Demonstrates ability to respect others', 'Is concerned about the problems in the society/community', 'Is able to reach outto the friends who are in need of extra help', 'Is tolerant with diversites', 'Sensitive towards the environment', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_Emp_1', index: 'LS_Emp_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_Emp_2', index: 'LS_Emp_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_Emp_3', index: 'LS_Emp_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_Emp_4', index: 'LS_Emp_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_Emp_5', index: 'LS_Emp_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_Emp_Total', index: 'LS_Emp_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_Emp_Average', index: 'LS_Emp_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'LS_Emp_Grade', index: 'LS_Emp_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_Emp').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_Emp').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_Emp').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_Emp',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        width: true,
        pager: 'JqGridLS_EmpPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Empathy'
    });
    $("#JqGridLS_Emp").navGrid('#JqGridLS_EmpPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridLS_Emp').jqGrid('navButtonAdd', '#JqGridLS_EmpPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function LifeSkills9(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Managing Emotions in soft skills
    $('#JqGridLS_ME').jqGrid({
        url: '/Assess360/JqGridLS_ME',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Identify his/her emotions', 'Manages his/her emotions', 'Shares his/her feelings with peer groups,teacher and parent', 'Can express his or her feelings in healthy manner', 'Remains cool and calm under adverse conditions', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                   { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_ME_1', index: 'LS_ME_1', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_ME_2', index: 'LS_ME_2', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_ME_3', index: 'LS_ME_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_ME_4', index: 'LS_ME_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_ME_5', index: 'LS_ME_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_ME_Total', index: 'LS_ME_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_ME_Average', index: 'LS_ME_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'LS_ME_Grade', index: 'LS_ME_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_ME').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_ME').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_ME').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_ME").trigger("reloadGrid");
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_ME',

        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'Desc',
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        pager: 'JqGridLS_MEPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> To Abide by the Constitution'
    });
    $("#JqGridLS_ME").navGrid('#JqGridLS_MEPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridLS_ME').jqGrid('navButtonAdd', '#JqGridLS_MEPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}


function LifeSkills10(ReqId, Cam, Grd, Sec, AcY, Trm) {
    var ColastSel = 0;
    ///Managing with Stress in soft skills
    $('#JqGridLS_MwthS').jqGrid({
        url: '/Assess360/JqGridLS_MwthS',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, Term: Trm },
        datatype: 'Json',
        type: 'GET',
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Academic Year', 'Term', 'Identifies the different stress related situations', 'Copes with stress in effective manner', 'Is optimistic in handling different stress inducing situation', 'Able to react positively under critical situation', 'Remains composed and collected in stressful situations', 'Total', 'Average', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptId', index: 'RptId', hidden: true, sortable: false, editable: true },
                      { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false, editable: true },
                      { name: 'PreRegNum', index: 'PreRegNum', width: 90, editable: true, sortable: false, hidden: true },
                      { name: 'Name', index: 'Name', width: 80, editable: true, editoptions: { disabled: true, class: 'NoBorder', style: " background-color:inherit; border-color:#dddddd;" } },
                      { name: 'Campus', index: 'Campus', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Grade', index: 'Grade', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Section', index: 'Section', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'Term', index: 'Term', width: 50, align: 'center', editable: true, hidden: true, sortable: false },
                      { name: 'LS_MwthS_1', index: 'LS_MwthS_1', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_MwthS_2', index: 'LS_MwthS_2', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_MwthS_3', index: 'LS_MwthS_3', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_MwthS_4', index: 'LS_MwthS_4', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_MwthS_5', index: 'LS_MwthS_5', width: 90, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: CheckCo_Scholastic } },
                      { name: 'LS_MwthS_Total', index: 'LS_MwthS_Total', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'LS_MwthS_Average', index: 'LS_MwthS_Average', width: 50, align: 'center', editable: false, sortable: false },
                      { name: 'LS_MwthS_Grade', index: 'LS_MwthS_Grade', width: 40, align: 'center', editable: false, sortable: false },
                      { name: 'EditRowId', index: 'EditRowId', width: 80, align: 'center', hidden: true, editable: false, sortable: true },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', width: 10, align: 'center', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 10, align: 'center', hidden: true }],
        onSelectRow: function (id) {
            var EditRole = $("#IsEdit").val();
            var RptEditId = jQuery('#JqGridLS_MwthS').jqGrid('getCell', id, 'EditRowId');
            if (id && id !== ColastSel) {
                jQuery('#JqGridLS_MwthS').restoreRow(ColastSel);
                if (RptEditId == 0 || EditRole == 'Yes') {
                    jQuery('#JqGridLS_MwthS').editRow(id, true, '', function reload(rowid, result) {
                        $("#JqGridLS_MwthS").trigger("reloadGrid");
                        $("#JqGridLS_SA").trigger("reloadGrid");
                        $("#JqGridLS_PS").trigger("reloadGrid");
                        $("#JqGridLS_DM").trigger("reloadGrid");
                        $("#JqGridLS_CriT").trigger("reloadGrid");
                        $("#JqGridLS_CreT").trigger("reloadGrid");
                        $("#JqGridLS_IR").trigger("reloadGrid");
                        $("#JqGridLS_EC").trigger("reloadGrid");
                        $("#JqGridLS_Emp").trigger("reloadGrid");
                        $("#JqGridLS_ME").trigger("reloadGrid");
                    });
                }
                ColastSel = id;
            }
        },
        serializeRowData: function (postdata) {
            if (CheckValidationFunc(postdata) == false) { return false; }
            else
                return postdata;
        },
        editurl: '/Assess360/SaveReportCardCBSELS_MwthS',
        autowidth: true,
        height: 180,
        //rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        shrinkToFit: true,
        sortorder: 'Desc',
        viewrecords: true,
        pager: 'JqGridLS_MwthSPager',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list"></i> Managing with Stress'
    });
    $("#JqGridLS_Mwths").navGrid('#JqGridLS_MwthsPager',
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
            {},
            {}, {}, {});


    //    $('#JqGridLS_MwthS').jqGrid('navButtonAdd', '#JqGridLS_MwthSPager', {
    //        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
    //        onClickButton: function () {
    //            //window.open("JqGridSummativeAssessment1" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
    //        }
    //    });
}

function CheckValidateCommon1(postdata) {
    if (postdata.TotalAttendenceT1 != "" && postdata.HeightT1 != "" && postdata.WeightT1 != "") {
        if (parseInt(postdata.TofWorkingDayT1) >= parseInt(postdata.TotalAttendenceT1)) { return true; }
        else {
            ErrMsg("Total Attendence Should be less than Total no of Workingdays"); return false;
        }
    }
    else return false;
}

function CheckValidateCommon2(postdata) {
    if (postdata.TotalAttendenceT2 != "" && postdata.HeightT2 != "" && postdata.WeightT2 != "") {
        if (parseInt(postdata.TofWorkingDayT2) >= parseInt(postdata.TotalAttendenceT2)) { return true; }
        else {
            ErrMsg("Total Attendence Should be less than Total no of Workingdays"); return false;
        }
    }
    else return false;
}

function CheckValidationFunc(postdata) {
    return true;
}

function CheckCo_Scholastic(value, column) {
    if (value == "") {
        return [false, 'Please fill the ' + column + ' value'];
    }
    if (!$.isNumeric(value)) {
        return [false, ' Should be numeric'];
    } else {
        if (value <= 5 && value >= 0)
            return [true];
        else if (value < 0)
            return [false, 'Should not less than 0'];
        else
            return [false, ' Should be less than 5'];
    }
}

function CheckCommonValue(value, column) {
    if (value == "") {
        return [false, 'Please fill the ' + column + ' value'];
    }
    if (!$.isNumeric(value)) {
        return [false, ' Should be numeric'];
    } else {
        return [true];
    }
}

function checkFAvalid(value, column) {
    if ($('#' + column).val() == "") {
        return [false, column + 'Please fill the Value. '];
    } else {
        if (value == "") {
            return [false, 'Please fill the ' + column + 'value'];
        }
        if (!$.isNumeric(value)) {
            if (value == "ABS" || value == "abs")
                return [true];
            else
                return [false, 'Should be numeric'];
        } else {
            if (column == "B (5)" || column == "C (5)" || column == "D (5)") {
                if (value <= 5 && value >= 0)
                    return [true];
                else if (value < 0)
                    return [false, 'Should not less than 0'];
                else
                    return [false, 'Should be less than 5'];
            }
            else if (column == "SlipTest (20)") {
                if (value <= 20 && value >= 0)
                    return [true];
                else if (value < 0)
                    return [false, 'Should not less than 0'];
                else
                    return [false, 'Should be less than 20'];
            }
        }
    }
}

function checkSAvalid(value, column) {
    if ($('#' + column).val() == "") {
        return [false, column + 'Please fill the Value. '];
    } else {
        if (value == "") {
            return [false, 'Please fill the ' + column + 'value'];
        }
        if (!$.isNumeric(value)) {
            if (value == "ABS" || value == "abs")
                return [true];
            else
                return [false, 'Should be numeric'];
        } else {
            if (column == "SA1Total (90)" || column == "SA2Total (90)") {
                if (value <= 90 && value >= 0)
                    return [true];
                else if (value < 0)
                    return [false, 'Should not less than 0'];
                else
                    return [false, 'Should be less than 90'];
            }
        }
    }
}
function checkvalid(value, column) {
    if ($('#' + column).val() == "") {
        return [false, column + 'Please fill the Value. '];
    } else {
        if (value == "") {
            return [false, 'Please fill the ' + column + 'value'];
        }
        if (!$.isNumeric(value)) {
            return [false, 'Should be numeric'];
        } else {
            if (column == "ObtainMarks") {
                if (value <= 5 && value >= 0)
                    return [true];
                else if (value < 0)
                    return [false, 'Should not less than 0'];
                else
                    return [false, 'Should be less than 5'];
            }
        }
    }
}
function checkTerm1Workingday(value, column) {
    if (value == "") {
        return [false, 'Please fill the ' + column];
    }
    else { }
}

function HideUnwantedDiv() {
    $("#lifeSkill").hide();
    $("#workeducation").hide();
    $("#visualperformingart").hide();
    $("#attitude").hide();
    $("#values").hide();
    $("#litandcreskills").hide();
    $("#ICT").hide();
    $("#ScientificSkills").hide();
    $("#healthandpet").hide();
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