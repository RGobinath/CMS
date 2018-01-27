var grid_selector = "#jqGridStudentSurveyAnswerList";
var pager_selector = "#jqGridStudentSurveyAnswerListPager";

function GetAcdemicYearCampusGradeAddPanel() {

    var AcademicYear = $("#ddlAcademicYearmaster").val();
    var Campus = $("#ddlCampusmaster").val();
    var Grade = $("#ddlgrademaster").val();
    if (AcademicYear != "" && Campus != "" && Grade != "") {
        $.getJSON("/StaffManagement/GetStudentAcdemicYearCampusGrade/", { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade },
                    function (modelData) {
                        var select = $("#ddlStudentSurveyGroupMaster");
                        select.empty();
                        select.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
                    });
    }
}
function GetGroupandQuestionAddPanel() {
    var AcademicYear = $("#ddlAcademicYearmaster").val();
    var Campus = $("#ddlCampusmaster").val();
    var Grade = $("#ddlgrademaster").val();
    var StudentSurveyGroup = $('#ddlStudentSurveyGroupMaster').val();
    if (AcademicYear != "" && Campus != "" && Grade != "" && StudentSurveyGroup != "") {
        $.getJSON("/StaffManagement/GetStudentAcdemicYearCampusGradeandGroup/", { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, StudentSurveyGroupId: StudentSurveyGroup },
                    function (modelData) {
                        var select = $("#ddlStudentSurveyQuestionMaster");
                        select.empty();
                        select.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
                    });
    }
}
function GetGroupandQuestionSearchPanel() {
    var AcademicYear = $("#ddlAcademicYear").val();
    var Campus = $("#ddlCampus").val();
    var Grade = $("#ddlgrade").val();
    var StudentSurveyGroup = $('#ddlStudentSurveyGroup').val();
    if (AcademicYear != "" && Campus != "" && Grade != "" && StudentSurveyGroup != "") {
        $.getJSON("/StaffManagement/GetStudentAcdemicYearCampusGradeandGroup/", { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, StudentSurveyGroupId: StudentSurveyGroup },
                    function (modelData) {
                        var select = $("#ddlStudentSurveyQuestion");
                        select.empty();
                        select.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
                    });
    }
}
function GetAnswerMasterSearchPanel() {

    var AcademicYear = $("#ddlAcademicYear").val();
    var Campus = $("#ddlCampus").val();
    var Grade = $("#ddlgrade").val();
    if (AcademicYear != "" && Campus != "" && Grade != "") {
        $.getJSON("/StaffManagement/GetStudentAcdemicYearCampusGrade/", { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade },
                    function (modelData) {
                        var temp = $("#ddlStudentSurveyGroup");
                        temp.empty();
                        debugger;
                        temp.append($('<option/>', { value: "", text: "Select One" }));
                        debugger;
                        $.each(modelData, function (index, itemData) {
                            debugger;
                            temp.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                            debugger;
                        });
                    });
    }
}
function gradeddl() {

    var e = document.getElementById('ddlCampusmaster');
    var campus = e.options[e.selectedIndex].value;
    //alert(campus);
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrademaster");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "Select Grade" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
}
function gradeddlmaster() {

    var e = document.getElementById('ddlCampus');
    var campus = e.options[e.selectedIndex].value;
    //alert(campus);
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrade");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "Select Grade" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
}


$(function () {
    $('#ddlAcademicYearmaster,#ddlCampusmaster,#ddlgrademaster').change(function () {
        GetAcdemicYearCampusGradeAddPanel();

    });
    $('#ddlAcademicYear,#ddlCampus,#ddlgrade').change(function () {
        GetAnswerMasterSearchPanel();

    });
    $('#ddlCampus').change(function () {
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        gradeddlmaster();
    });
    $('#ddlCampusmaster').change(function () {

        if ($('#ddlCampusmaster').val() == "") { ErrMsg("Please fill the Campus"); return false; }

        gradeddl();

    });
    $('#ddlAcademicYearmaster,#ddlCampusmaster,#ddlgrademaster,#ddlStudentSurveyGroupMaster').change(function () {
        GetGroupandQuestionAddPanel();

    });
    $('#ddlAcademicYear,#ddlCampus,#ddlgrade,#ddlStudentSurveyGroup').change(function () {
        GetGroupandQuestionSearchPanel();

    });
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
    debugger;
    $(grid_selector).jqGrid({
        url: '/StaffManagement/GetJqGridstudsurveyAnswerList',
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['Student Survey Answer  view Id', 'Academic Year', 'Campus', 'Grade', 'Student Survey Group', 'Student Survey Question', 'Student Survey Answer Id', 'Student Survey Answer', 'Student Survey Mark', 'Is Positive', 'Is Active', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate', ],
        colModel: [
                     { name: 'StudentSurveyAnswerViewId', index: 'StudentSurveyAnswerViewId', key: true, hidden: true, editable: true },
                     { name: 'AcademicYear', index: 'AcademicYear', sortable: false, editable: false, hidden: true },
                     { name: 'Campus', index: 'Campus', sortable: false, editable: false, hidden: true },
                     { name: 'Grade', index: 'Grade', sortable: false, editable: false, hidden: true },
                     { name: 'StudentSurveyGroup', index: 'StudentSurveyGroup', sortable: false, editable: false, hidden: false,width:100 },
                     {
                         name: 'StudentSurveyQuestionId', index: 'StudentSurveyQuestionId', sortable: true, editable: true, edittype: 'select', editoptions: {
                             dataUrl: '/StaffManagement/GetStudentAnswerddl',

                             buildSelect: function (data) {
                                 debugger;
                                 var SurveyQuestionMaster = jQuery.parseJSON(data);
                                 var s = '<select>';
                                 s += '<option value="">Select One</option>';
                                 if (SurveyQuestionMaster && SurveyQuestionMaster.length) {

                                     for (var i = 0, l = SurveyQuestionMaster.length; i < l; i++) {

                                         s += '<option value="' + SurveyQuestionMaster[i].Value + '">' + SurveyQuestionMaster[i].Text + '</option>';
                                     }
                                 }
                                 return s + "</select>";
                             },
                             style: "width: 175px;"
                         }, editrules: { required: true }, sortable: true
                     },
                     { name: 'StudentSurveyAnswerId', index: 'StudentSurveyAnswerId', sortable: false, editable: true, hidden: true },
                     { name: 'StudentSurveyAnswer', index: 'StudentSurveyAnswer', sortable: false, editable: true, width: 100 },
                     { name: 'StudentSurveyMark', index: 'StudentSurveyMark', sortable: false, editable: true, width: 50 },
                     {
                         name: 'IsPositive', index: 'IsPositive', sortable: true, editable: true, edittype: 'select', editoptions: {
                             value: {
                                 'true': 'Yes',
                                 'false': 'No',

                             },
                         }, width: 50
                     },
                    {
                        name: 'IsActive', index: 'IsActive', sortable: true, editable: true, edittype: 'select', editoptions: {
                            value: {
                                'true': 'Yes',
                                'false': 'No',

                            },
                        }, width: 50
                    },
                     { name: 'CreatedBy', index: 'CreatedBy', sortable: false, editable: false, hidden: true },
                    { name: 'CreatedDate', index: 'CreatedDate', sortable: false, editable: false, hidden: true },
                    { name: 'ModifiedBy', index: 'ModifiedBy', sortable: false, editable: false, hidden: true },
                    { name: 'ModifiedDate', index: 'ModifiedDate', sortable: false, editable: false, hidden: true },
        ],

        viewrecords: true,
        altRows: true,
        autowidth: true,
        autoheight: true,
        multiselect: true,
        // multiboxonly: true,
        height: 250,
        rowNum: 10,
        rowList: [5, 10, 20],
        sortName: 'StudentSurveyAnswerViewId',
        sortOrder: 'Asc',
        pager: pager_selector,


        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-th-list"></i>&nbsp; Student Survey Answer Details'
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
            {
                // url: '/StaffManagement/EditStudentSurveyAnswerMaster/',
                closeAfterEdit: true,
                closeOnEscape: true,
                width: 480,
                height: 275
            }, //Edit
            {

            }, //Add
              {
              },
               {},
                {})
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    $('#btnSave').click(function () {
        var AcademicYear = $("#ddlAcademicYearmaster").val();
        var Campus = $("#ddlCampusmaster").val();
        var Grade = $("#ddlgrademaster").val();
        var StudentSurveyGroup = $('#ddlStudentSurveyGroupMaster').val();
        var StudentSurveyQuestion = $("#ddlStudentSurveyQuestionMaster").val();
        var StudentSurveyAnswer = $("#txtStudentSurveyAnswerMaster").val();
        var StudentSurveyMark = $("#txtStudentSurveyMarkMaster").val();
        var IsPositive = $("#ddlIsPositiveMaster").val();
        var IsActive = $("#ddlIsActiveMaster").val();
        if (AcademicYear == "" || Campus == "" || Grade == "" || StudentSurveyQuestion == "" || StudentSurveyAnswer == "" || StudentSurveyMark == "" || IsPositive == "" || IsActive == "") {
            ErrMsg("Please Fill the Required Details");
            return false;
        }
        $.ajax({
            Type: 'POST',
            dataType: 'json',
            url: '/StaffManagement/AddOrEditStudentSurveyAnswer?StudentSurveyGroupId=' + StudentSurveyGroup + '&StudentSurveyQuestionId=' + StudentSurveyQuestion + '&AcademicYear=' + AcademicYear + '&Campus=' + Campus + '&Grade=' + Grade + '&StudentSurveyAnswer=' + StudentSurveyAnswer + '&StudentSurveyMark=' + StudentSurveyMark + '&IsPositive=' + IsPositive + '&IsActive=' + IsActive,
            success: function (data) {
                debugger;
                $(grid_selector).trigger('reloadGrid');
                if (data == "insert") {
                    $("input[type=text], textarea, select").val("");
                    SucessMsg("Added Successfully");
                    return true;
                }
                else {
                    $("input[type=text]").val("");
                    ErrMsg("Already Exist");
                    return false;
                }


            }
        });

    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: '/StaffManagement/GetJqGridstudsurveyAnswerList',
                postData: { StudentSurveyQuestionId: "", StudentSurveyAnswer: "", StudentSurveyMark: "", IsPositive: "", IsActive: "" },
                page: 1
            }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {

        var AcademicYear = $("#ddlAcademicYear").val();
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlgrade").val();
        var MatarialSubGroup = $('#ddlStudentSurveyGroup option:selected').text();
        var StudentSurveyQuestion = $("#ddlStudentSurveyQuestion").val();
        var StudentSurveyAnswer = $("#txtStudentSurveyAnswer").val();
        var StudentSurveyMark = $("#txtStudentSurveyMark").val();
        var IsPositive = $("#ddlIsPositive").val();
        var IsActive = $("#ddlIsActive").val();
        if (AcademicYear == "" || Campus == "" || Grade == "") {
            ErrMsg("Please Fill the Required Details");
            return false;
        }
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StaffManagement/GetJqGridstudsurveyAnswerList',
                    postData: { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, StudentSurveyGroup: MatarialSubGroup, StudentSurveyQuestionId: StudentSurveyQuestion, StudentSurveyAnswer: StudentSurveyAnswer, StudentSurveyMark: StudentSurveyMark, IsPositive: IsPositive, IsActive: IsActive },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#SrchbtnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: '/StaffManagement/GetJqGridstudsurveyAnswerList',
                postData: { AcademicYear: "", Campus: "", Grade: "", StudentSurveyGroup: "", StudentSurveyQuestionId: "", StudentSurveyAnswer: "", StudentSurveyMark: "", IsPositive: "", IsActive: "" },
                page: 1
            }).trigger("reloadGrid");
    });
    $("#txtStudentSurveyMarkMaster,#txtStudentSurveyMark").keydown(function (e) {

        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
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


