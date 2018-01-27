$(function () {
    var ReqId = ""; var Cam = ""; var Grd = ""; var Sec = ""; var AcY = ""; var Sub = ""; var NofDaysTerm1 = ""; var NofDaysTerm2 = ""; var nofworkdays = 0;
    var param = ""; var CoCode = 0; Trm = "";

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
            window.location.href = "/Assess360/ReportCardCBSENew?RptRequestId=" + $("#Id").val() + '&Subject=' + $("#ddlSubject").val();
        }
        if ($("#ddlSubject").val() == "") {
            window.location.href = "/Assess360/ReportCardCBSENew?RptRequestId=" + $("#Id").val();
        }
    });
    $("#ddllanguage").change(function () {
        if ($("#ddlSubject").val() == "Language" && $("#ddllanguage").val() != "") {
            window.location.href = "/Assess360/ReportCardCBSENew?RptRequestId=" + $("#Id").val() + '&Subject=' + $("#ddlSubject").val() + '&Language=' + $("#ddllanguage").val();
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
    alert(Val);
    if ($('#Subject').val() == "") {
        if (Val == 1 || Val == 3 || Val == 5) {
            if (Val == 4) {
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
        url: '/Assess360/ReportCardCBSEValidationNew?PreRegNum=' + RegNum + '&AcademicYear=' + AcYear + '&ReqId=' + ReqId,
        success: function (data) {
            if (data == "Success") {
                window.open('/Assess360/GenerateCBSERptCardPdfNew?PreRegNum=' + RegNum + '&Terms=' + term);
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

$(window).on('resize.jqGrid', function () {
    $("#JqGridFA1").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridFA2").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridOverAll").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridCommonTerm1").jqGrid('setGridWidth', $(".tab-content").width());
    $("#JqGridCommonTerm2").jqGrid('setGridWidth', $(".tab-content").width());
})
//resize on sidebar collapse/expand
var parent_column3 = $("#JqGridFA1").closest('[class*="col-"]');
var parent_column4 = $("#JqGridFA2").closest('[class*="col-"]');
var parent_column7 = $("#JqGridOverAll").closest('[class*="col-"]');
var parent_column8 = $("#JqGridCommonTerm1").closest('[class*="col-"]');
var parent_column9 = $("#JqGridCommonTerm2").closest('[class*="col-"]');

$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
        setTimeout(function () {
            $("#JqGridFA1").jqGrid('setGridWidth', parent_column3.width());
            $("#JqGridFA2").jqGrid('setGridWidth', parent_column4.width());
            $("#JqGridOverAll").jqGrid('setGridWidth', parent_column7.width());
            $("#JqGridCommonTerm1").jqGrid('setGridWidth', parent_column8.width());
            $("#JqGridCommonTerm2").jqGrid('setGridWidth', parent_column9.width());
        }, 0);
    }
})

function FormativeAssessment1(ReqId, Cam, Grd, Sec, AcY, Sub) {
    //Formative Assessment 1
    var lastSel;
    $('#JqGridFA1').jqGrid({
        url: '/Assess360/JqGridFormativeAssessment1New',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'PA I (10)', 'NoteBook (5)', 'SEA (5)', 'HalfYearly (80)',  'Total (100)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
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
                      { name: 'PA1', index: 'PA1', width: 70, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'NoteBook1', index: 'NoteBook1', width: 70, align: 'center', editable: true, sortable: false },
                      { name: 'SEA1', index: 'SEA1', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'HalfYearly', index: 'HalfYearly', width: 50, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'Term1Total', index: 'Term1Total', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'Term1Grade', index: 'Term1Grade', width: 80, align: 'center', editable: false, sortable: false },
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
                        $("#JqGridFA2").trigger("reloadGrid");
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
        editurl: '/Assess360/SaveReportCardCBSEFA1New',
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
            window.open("JqGridFormativeAssessment1New" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });
}

function FormativeAssessment2(ReqId, Cam, Grd, Sec, AcY, Sub) {
    var lastSel;
    //Formative Assessment 2
    $('#JqGridFA2').jqGrid({
        url: '/Assess360/JqGridFormativeAssessment2New',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptId', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'PA II (10)', 'NoteBook (5)', 'SEA (5)', 'AnnualExam (80)',  'Total (100)', 'Grade', 'EditRowId', 'Created By', 'Created Date', 'Modified By', 'ModifiedDate'],
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
                      { name: 'PA2', index: 'PA2', width: 100, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'SEA2', index: 'SEA2', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'NoteBook2', index: 'NoteBook2', width: 70, align: 'center', editable: true, sortable: false },
                      { name: 'AnnualExam', index: 'FA2CTotal', width: 80, align: 'center', editable: true, sortable: false, editrules: { custom: true, custom_func: checkFAvalid } },
                      { name: 'Term2Total', index: 'Term2Total', width: 80, align: 'center', editable: false, sortable: false },
                      { name: 'Term2Grade', index: 'Term2Grade', width: 80, align: 'center', editable: false, sortable: false },
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
                        $("#JqGridFA1").trigger("reloadGrid");
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
        editurl: '/Assess360/SaveReportCardCBSEFA2New',

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
            window.open("JqGridFormativeAssessment2New" + '?RptRequestId=' + ReqId + '&campus=' + Cam + '&grade=' + Grd + '&section=' + Sec + '&academicyear=' + AcY + '&subject=' + Sub + '&rows=99999' + '&ExptXl=1');
        }
    });
}

function OverAllAssessment(ReqId, Cam, Grd, Sec, AcY, Sub) {
    //Formative Assessment 4
    var lastSel;
    $('#JqGridOverAll').jqGrid({
        url: '/Assess360/JqGridOverAllAssessmentNew',
        postData: { RptRequestId: ReqId, campus: Cam, grade: Grd, section: Sec, academicyear: AcY, subject: Sub },
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        colNames: ['Id', 'RptRequestId', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Section', 'Subject', 'Academic Year', 'PA', 'NoteBook', 'SEA1', 'HalfYearly',
            'TermTotal', 'TermGrade', 'PA', 'NoteBook2', 'SEA2', 'AnnualExam', 'TermTotal', 'TermGrade', 'T-I+T-II Total', 'T-I+T-II Avg', 'Result PDF'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true, sortable: false },
                       { name: 'RptRequestId', index: 'RptRequestId', hidden: true, sortable: false },
                       { name: 'PreRegNum', index: 'PreRegNum', hidden: true, width: 80, sortable: false },
                       { name: 'Name', index: 'Name', width: 120 },
                       { name: 'Campus', index: 'Campus', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'Grade', index: 'Grade', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'Section', index: 'Section', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'Subject', index: 'Subject', width: 80, align: 'center', sortable: false },
                       { name: 'AcademicYear', index: 'AcademicYear', width: 50, align: 'center', hidden: true, sortable: false },
                       { name: 'PA1', index: 'PA1', width: 80, align: 'center', sortable: false },
                       { name: 'NoteBook1', index: 'NoteBook1', width: 80, align: 'center', sortable: false },
                       { name: 'SEA1', index: 'SEA1', width: 80, align: 'center', sortable: false },
                       { name: 'HalfYearly', index: 'HalfYearly', width: 80, align: 'center', sortable: false },
                       { name: 'Term1Total', index: 'Term1Total', width: 80, align: 'center', sortable: false },
                       { name: 'Term1Grade', index: 'Term1Grade', width: 80, align: 'center', sortable: false },
                       { name: 'PA2', index: 'PA2', width: 80, align: 'center', sortable: false },
                       { name: 'NoteBook2', index: 'NoteBook2', width: 80, align: 'center', sortable: false },
                       { name: 'SEA2', index: 'SEA2', width: 80, align: 'center', sortable: false },
                       { name: 'AnnualExam', index: 'AnnualExam', width: 80, align: 'center', sortable: false },
                       { name: 'Term2Total', index: 'Term2Total', width: 80, align: 'center', sortable: false },
                       { name: 'Term2Grade', index: 'Term2Grade', width: 80, align: 'center', sortable: false },
                       { name: 'T1T2Total', index: 'T1T2Total', width: 80, align: 'center', sortable: false },
                       { name: 'T1T2Grade', index: 'T1T2Grade', width: 80, align: 'center', sortable: false },
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
                 { startColumnName: 'PA1', numberOfColumns: 6, titleText: '<em><center>Term I</center></em>' }, { startColumnName: 'PA2', numberOfColumns: 6, titleText: '<em><center>Term II</center></em>' }, ]
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

            if (column == "NoteBook (5)" || column == "SEA (5)" ) {
                if (value <= 5 && value >= 0)
                    return [true];
                else if (value < 0)
                    return [false, 'Should not less than 0'];
                else
                    return [false, 'Should be less than 5'];
            }
            else if (column == "PA II (10)" || column == "PA I (10)") {
                if (value <= 10 && value >= 0)
                    return [true];
                else if (value < 0)
                    return [false, 'Should not less than 0'];
                else
                    return [false, 'Should be less than 20'];
            }
            else if (column == "AnnualExam (80)" || column == "HalfYearly (80)") {
                if (value <= 80 && value >= 0)
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