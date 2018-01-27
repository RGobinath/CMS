
var grid_selector = '#jqGridStudentSurveyQuestionList';
var Pager_selector = '#jqGridStudentSurveyQuestionListPager';

$(function () {
   
    var grid_selector = "#jqGridStudentSurveyQuestionList";
    var pager_selector = "#jqGridStudentSurveyQuestionListPager";

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
        url: '/StaffManagement/GetJqGridstudsurveyList',
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['SurveyQuestionId', 'StudentSurveyQuestionId', 'Academic Year', 'Campus', 'Grade', 'Student Survey Group', 'Student Survey Question', 'Is Active', ],
        colModel: [
                     { name: 'SurveyQuestionId', index: 'SurveyQuestionId', key: true, hidden: true, editable: true },
                     { name: 'StudentSurveyQuestionId', index: 'StudentSurveyQuestionId', hidden: true, editable: false },
                    { name: 'AcademicYear', index: 'AcademicYear', sortable: true, editable: false, hidden: true },
                    { name: 'Campus', index: 'Campus', sortable: true, editable: false, hidden: true },
                    { name: 'Grade', index: 'Grade', sortable: true, editable: false, hidden: true },
                     {
                         name: 'StudentSurveyGroupId', index: 'StudentSurveyGroup', sortable: true, editable: false, edittype: 'select', editoptions: {
                             dataUrl: '/StaffManagement/GetStudentQuestionddl',

                             buildSelect: function (data) {
                               
                                 var SurveyGroupMaster = jQuery.parseJSON(data);
                                 var s = '<select>';
                                 s += '<option value="">Select One</option>';
                                 if (SurveyGroupMaster && SurveyGroupMaster.length) {

                                     for (var i = 0, l = SurveyGroupMaster.length; i < l; i++) {

                                         s += '<option value="' + SurveyGroupMaster[i].Value + '">' + SurveyGroupMaster[i].Text + '</option>';
                                     }
                                 }
                                 return s + "</select>";
                             },
                             style: "width: 175px;"
                         }, editrules: { required: true }, sortable: true
                     },
                     { name: 'StudentSurveyQuestion', index: 'StudentSurveyQuestion', sortable: true, editable: false },
                    {
                        name: 'IsActive', index: 'IsActive', sortable: true, editable: false, edittype: 'select', editoptions: {
                            value: {
                                'true': 'Yes',
                                'false': 'No',

                            },
                        },width:50
                    },

        ],

        viewrecords: true,
        altRows: true,
        autowidth: true,
        multiselect: true,
        // multiboxonly: true,
        height: 250,
        rowNum: 10,
        rowList: [5, 10, 20],
        sortName: 'StudentSurveyQuestionId',
        sortOrder: 'Asc',
        pager: Pager_selector,

        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-th-list"></i>&nbsp; Student Survey Details'
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
                // url: '/StaffManagement/EditStudentSurveyQuestionMaster/',
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
        var StudentSurvayGroup = $("#ddlStudentSurveyGroupMaster").val();
        var StudentSurveyQuestion = $("#addStudentSurveyQuestionMaster").val();
        var AcademicYear = $("#ddlAcademicYearmaster").val();
        var Campus = $("#ddlCampusmaster").val();
        var Grade = $("#ddlgrademaster").val();
       
        if (AcademicYear == "" || Campus == "" || Grade == "" || StudentSurvayGroup == "" || StudentSurveyQuestion == "") {
            ErrMsg("Please Fill the Required Details");
            return false;
        }
        $.ajax({
            Type: 'POST',
            dataType: 'json',
            url: '/StaffManagement/AddOrEditStudentSurveyQuestion?StudentSurveyGroupId=' + StudentSurvayGroup + '&AcademicYear=' + AcademicYear + '&Campus=' + Campus + '&Grade=' + Grade + '&StudentSurveyQuestion=' + StudentSurveyQuestion + '&IsActive=' + "",
            success: function (data) {
              
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
                url: '/StaffManagement/GetJqGridstudsurveyList',
                postData: { AcademicYear: "", Campus: "", Grade: "", StudentSurveyGroupId: "", StudentSurveyQuestion: "", IsActive: "" },
                page: 1
            }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        
        var StudentSurveyGoup = $("#ddlStudentSurveyGroup option:selected").val();

        var StudentSurveyQuestion = $("#addStudentSurveyQuestion").val();


        var IsActive = $("#ddlIsActive").val();
        var AcademicYear = $("#ddlAcademicYear").val();
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlgrade").val();
        if (AcademicYear == "" || Campus == "" || Grade == "" ) {
            ErrMsg("Please Fill the Required Details");
            return false;
        }
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StaffManagement/GetJqGridstudsurveyList',
                    postData: { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, StudentSurveyGroupId: StudentSurveyGoup, StudentSurveyQuestion: StudentSurveyQuestion, IsActive: IsActive },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#SrchbtnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: '/StaffManagement/GetJqGridstudsurveyList',
                postData: { AcademicYear: "", Campus: "", Grade: "", StudentSurveyGroupId: "", StudentSurveyQuestion: "", IsActive: "" },
                page: 1
            }).trigger("reloadGrid");
    });
    $('#ddlCampus').change(function () {
       
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        gradeddl();

    });
    $('#ddlCampusmaster').change(function () {
      
        if ($('#ddlCampusmaster').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        gradeddlmaster();

    });
    $('#ddlAcademicYearmaster,#ddlCampusmaster,#ddlgrademaster').change(function () {
        GetAcdemicYearCampusGradeAddPanel();

    });
    $('#ddlAcademicYear,#ddlCampus,#ddlgrade').change(function () {
        GetAcdemicYearCampusGradeSearchPanel();

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


function gradeddl() {
   
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
function gradeddlmaster() {
  
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
function GetAcdemicYearCampusGradeSearchPanel() {
  
    var AcademicYear = $("#ddlAcademicYear").val();
    var Campus = $("#ddlCampus").val();
    var Grade = $("#ddlgrade").val();
    if (AcademicYear != "" && Campus != "" && Grade != "") {
        $.getJSON("/StaffManagement/GetStudentAcdemicYearCampusGrade/", { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade },
                    function (modelData) {
                        var select = $("#ddlStudentSurveyGroup");
                        select.empty();
                        select.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
                    });
    }
}