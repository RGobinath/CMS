-function FillGradeByCampus() {
    var Campus = [];
    var StaffPreRegNum = $('#StaffPreRegNum').val();
    var ToughtGrades = "";
    $.ajax({
        url: "/StaffManagement/GetToughtGradesDetailsByPreRegNum?StaffPreRegNumber=" + StaffPreRegNum,
        type: "GET",
        dataType: "json",
        traditional: true,
        success: function (Obj) {
            ToughtGrades = Obj;
            Campus = $("#ddlcampus").val();
            var ddlbc = $("#ddlgrade");
            if (Campus != null && Campus != "") {
                $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
                  function (fillbc) {
                      ddlbc.empty();
                      var ToughtGradesArr = ToughtGrades.split(',');
                      $.each(fillbc, function (index, GradeData) {
                          var checkVal = "" + GradeData.Value + "";
                          if (ToughtGradesArr.includes(checkVal) == true)
                              ddlbc.append("<option value='" + GradeData.Value + "'selected='selected'>" + GradeData.Text + "</option>");
                          else
                              ddlbc.append("<option value='" + GradeData.Value + "'>" + GradeData.Text + "</option>");
                      });
                      $('#ddlgrade').multiselect('enable');
                      $('#ddlgrade').multiselect('rebuild');
                  });
            }
            if (Campus == "" || Campus == null) {
                ddlbc.empty;
                $('#ddlgrade').multiselect('disable');
            }
        }
    });
}
//function FillGradeByCampus() {
//    var Campus = [];
//    Campus = $("#ddlcampus").val();
//    var ddlbc = $("#ddlgrade");
//    if (Campus != null && Campus != "") {
//        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
//          function (fillbc) {
//              ddlbc.empty();
//              $.each(fillbc, function (index, itemdata) {
//                  debugger;
//                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//              });
//              $('#ddlgrade').multiselect('enable');
//              $('#ddlgrade').multiselect('rebuild');
//          });
//    }
//    if (Campus == "" || Campus == null) {
//        ddlbc.empty;
//        $('#ddlgrade').multiselect('disable');
//    }
//}
function FillSubjectByCampusandGrade() {
    var Campus = $("#ddlcampus").val();
    var Grade = $("#ddlgrade").val();
    var ddlbc = $("#ddlSubject");
    if (Campus != "" && Grade != "") {
        $.getJSON("/Base/GetSubjectsByCampusGradeWithMultiplCriteria?Campus=" + Campus + '&Grade=' + Grade,
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  alert(itemdata.Value);
                  alert(itemdata.Text);
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  //for (var i = 0; itemdata.length >= 0; i++) {
                  //    if (itemdata[i].Value != null && itemdata[i].Text != null) {
                  //        ddlbc.append($('<option/>', { value: itemdata[i].Value, text: itemdata[i].Text }));
                  //    }
                  //}
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
    }
}
function FillAcademicYearDll() {
    $.getJSON("/Base/GetJsonAcademicYear",
      function (fillbc) {
          var ddlbc = $("#ddlacademicyear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillGradeByCampus1() {
    var Campus = $("#SrchddlCampus").val();
    $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
      function (fillbc) {
          var ddlbc = $("#SrchddlGrade");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillSubjectByCampusandGrade1() {
    var Campus = $("#SrchddlCampus").val();
    var Grade = $("#SrchddlGrade").val();
    var ddlbc = $("#SrchddlSubject");
    if (Campus != "" && Grade != "") {
        $.getJSON("/Base/GetSubjectsByCampusGrade?Campus=" + Campus + '&Grade=' + Grade,
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
function FillAcademicYearDll1() {
    $.getJSON("/Base/GetJsonAcademicYear",
      function (fillbc) {
          var ddlbc = $("#SrchddlAcademicYear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
$(function () {
    FillAcademicYearDll();
    FillAcademicYearDll1();
    FillSubDepartment();
    FillSubDepartmentForSearch();
    FillProgramme();
    //FillProgrammeForSearch();
    RefreshddlReportingDesignationDdl();
    FillProgrammeBasedOnCampusAndStaffType();
    FillReportingManager();
    //FillReportingDesignation();
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
        url: '/StaffManagement/CampusBasedStaffDetails_VwJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Staff PreReg Number', 'Campus', 'Staff Name', 'Department', 'Sub Department', 'Group', 'Designation', 'Academic Year', 'Show Details'],
        colModel: [
                      { name: 'Id', index: 'Id', width: 130, hidden: true, editable: true, key: true },
                      { name: 'StaffPreRegNumber', index: 'StaffPreRegNumber', hidden: true, editable: true },
                      { name: 'Campus', index: 'Campus', width: 90, editable: true },
                      { name: 'StaffName', index: 'StaffName', width: 240, editable: true },
                      { name: 'Department', index: 'Department', editable: true, hidden: true },
                      { name: 'SubDepartment', index: 'SubDepartment', editable: true, hidden: true },
                      { name: 'Programme', index: 'Programme', width: 50, editable: true },
                      { name: 'Designation', index: 'Designation', width: 90 },
                      { name: 'AcademicYear', index: 'AcademicYear', width: 70 },
                      { name: 'ShowStaffDetails', index: 'ShowStaffDetails', align: 'center', width: 50 }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 25, 50, 100, 250, 500, 1000],
        pager: pager_selector,
        altRows: true,
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Campus Based Staff Details",
        onSelectRow: function (id) {
            UpdateAddPanelById(id);
        }
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
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
            {//url: '/StaffManagement//', closeAfterEdit: true, closeOnEscape: true
            }, //Edit
            {}, //Add
              {
                  width: 'auto', url: '/StaffManagement/DeleteCampusBasedStaffDetails', beforeShowForm: function (params) {
                      selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                      return { Id: selectedrows }
                  }
              },
               {},
                {});
    $("#ddlcampus").change(function () {
        FillGradeByCampus();
        StaffNameAutoComplete();
        RefreshddlReportingDesignationDdl();
        FillReportingDesignation();
        FillReportingManager();
    });
    $("#SrchddlGrade").change(function () {
        FillSectionByCampusGrade();
    });


    $("#ddlcampus,#ddlgrade").change(function () {
        FillSubjectByCampusandGrade();
        FillSectionByCampusGrade();
    });
    //FillReportingManagersDesignation();
    $("#SrchddlCampus").change(function () {
        FillGradeByCampus1();
        //FillReportingManagersDesignation();
        FillProgrammeBasedOnCampusAndStaffType();
        FillReportingManager();
    });
    $("#SrchddlStaffType").change(function () {
        FillProgrammeBasedOnCampusAndStaffType();
    });
    $("#SrchddlCampus,#SrchddlGrade").change(function () {
        FillSubjectByCampusandGrade1();
        //FillReportingManagersDesignation();
    });
    $('#ddlgrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '280',
        numberDisplayed: 6,
        nonSelectedText: 'None Selected',
        includeSelectAllDivider: true
    });
    $('#ddlSection').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '280',
        numberDisplayed: 6,
        nonSelectedText: 'None Selected',
        includeSelectAllDivider: true
    });
    $('#ddlReportingDesignation').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '280',
        numberDisplayed: 2,
        nonSelectedText: 'None Selected',
        includeSelectAllDivider: true
    });
    $("#btnReset").click(function () {
        $("#ddlcampus").val('');
        $("#ddlgrade").val('');
        $("#ddlSection").val('');
        $("#ddlgrade").multiselect('rebuild');
        $("#ddlSection").multiselect('rebuild');
        $("#ddlReportingDesignation").val('');
        $("#ddlReportingDesignation").multiselect('rebuild');
        FillGradeByCampus();
        $("#ddlacademicyear").val('');
        $("#txtStaffName").val('');
        $("#ddlSubject").val('');
        $("#ddlDepartment").val('');
        $("#ddlProgramme").val('');
        FillSubjectByCampusandGrade();
    });
    $("#SrchbtnReset").click(function () {
        $("#SrchddlCampus").val('');
        $("#SrchddlStaffType").val('');
        //  $("#SrchddlGrade").val('');
        $("#SrchddlProgramme").val('');
        $("#SrchddlAcademicYear").val('');
        //$("#SrchtxtStaffName").val('');
        //$("#SrchddlSubject").val('');
        //$("#SrchddlSection").val('');
        $("#SrchddlConfigurationStatus").val('');
        $("#SrchddlReportingDesignation").val('');
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/CampusBasedStaffDetails_VwJqGrid',
           postData: {
               Campus: "", Department: "", SubDepartment: "", StaffName: "", Programme: "", Designation: "", StaffType: "", AcademicYear: "", ConfigurationStatus: ""
               , ReportingHeads: "", Grade: "", Section: ""
           },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#SrchddlCampus").val();
        var StaffName = $("#SrchtxtStaffName").val();
        //var SrchddlDepartment = $("#SrchddlDepartment").val();
        //var SrchddlSubDepartment = $("#SrchddlSubDepartment").val();
        var SrchddlProgramme = $("#SrchddlProgramme").val();
        //var SrchddlDesignation = $("#SrchddlDesignation").val();
        var SrchddlStaffType = $("#SrchddlStaffType").val();
        var SrchddlAcademicyear = $("#SrchddlAcademicYear").val();
        var SrchddlConfigurationStatus = $("#SrchddlConfigurationStatus").val();
        var SrchddlReportingDesignation = $("#SrchddlReportingDesignation").val();
        var Grade = $("#SrchddlGrade").val();
        var Section = $("#ScrchddlSection").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/CampusBasedStaffDetails_VwJqGrid',
           postData: {
               Campus: Campus
               //, StaffName: StaffName
               //, Department: SrchddlDepartment
               //, SubDepartment: SrchddlSubDepartment
               , Programme: SrchddlProgramme
               //, Designation: SrchddlDesignation
               , StaffType: SrchddlStaffType
               , AcademicYear: SrchddlAcademicyear
               , ConfigurationStatus: SrchddlConfigurationStatus
               , Grade: Grade
               , Section: Section
               , ReportingHeads: SrchddlReportingDesignation
           },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#SrchtxtStaffName").autocomplete({
        source: function (request, response) {
            if (request.term.length <= 3) {
                return false;
            }
            if (request.term.length > 3) {
                var Campus = $("#SrchddlCampus").val();
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
                });
            }
        },
        messages: {
            noResults: "", results: ""
        }
    });
    $('#btnSave').click(function () {
        debugger;
        var PreRegNum = $("#StaffPreRegNum").val();
        var Campus = $("#ddlcampus").val();
        var Grade = $("#ddlgrade").val();
        var StaffName = $("#txtStaffName").val();
        var AcademicYear = $("#ddlacademicyear").val();
        var Subject = $("#ddlSubject").val();
        var Section = $("#ddlSection").val();

        var Department = $("#ddlDepartment").val();
        //var SubDepartment = $("#ddlSubDepartment").val();
        //var Programme = $("#ddlProgramme").val();
        //var Designation = $("#ddlDesignation").val();
        //var ReportingLevel = $("#ddlReportingLevel").val();
        var ReportingHeadPreRegNumsArray = $("#ddlReportingDesignation").val();
        //if (ReportingHeadPreRegNumsArray != null && ReportingHeadPreRegNumsArray != "") {
        //    ReportingHeadPreRegNumsArray = ReportingHeadPreRegNumsArray[0].val();
        //    ReportingHeadPreRegNumsArray = ReportingHeadPreRegNumsArray.split(',')
        //}
        if (Campus == "" || Grade == "" || StaffName == "" || AcademicYear == "" || Subject == "" || Section == "" || Campus == null || Grade == null || StaffName == null || AcademicYear == null || Subject == null || Section == null) {
            ErrMsg("Please Fill the Required Details");
            return false;
        }

        $.ajax({
            Type: 'POST',
            dataType: 'json',
            url: '/StaffManagement/AddCampusBasedStaffDetails?Campus=' + Campus + '&Grade=' + Grade + '&AcademicYear=' + AcademicYear + '&StaffName=' + StaffName + '&Subject=' + Subject + '&Section=' + Section + '&StaffPreRegNumber=' + PreRegNum
            + '&Department=' + Department
            //+ '&SubDepartment=' + SubDepartment
            //+ '&Programme=' + Programme
            //+ '&Designation=' + Designation
            //+ '&ReportingLevel=' + ReportingLevel
            + '&ReportingHeadPreRegNums=' + ReportingHeadPreRegNumsArray,

            success: function (data) {
                if (data == "success") {
                    SucessMsg("Added Successfully");
                    $(grid_selector).trigger('reloadGrid');
                }
                else {
                    ErrMsg("Already Exist");
                }
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
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function StaffNameAutoComplete() {
    var Campus = $("#ddlcampus").val();
    if (Campus != "") {
        $("#txtStaffName").autocomplete({
            source: function (request, response) {
                if (request.term.length <= 3) {
                    return false;
                }
                if (request.term.length > 3) {
                    $.ajax({
                        url: "/StaffManagement/StaffNameAutoComplete",
                        type: "POST",
                        dataType: "json",
                        data: { term: request.term, Campus: Campus },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.Name, value: item.PreRegNum }
                            }))
                        }
                    })
                }
            },
            minLength: 1,
            delay: 100,
            select: function (event, ui) {
                event.preventDefault();
                $("#StaffPreRegNum").val(ui.item.value);
                $("#txtStaffName").val(ui.item.label);
                $.ajax({
                    url: "/StaffManagement/GetStaffDetailsViewByPreRegNum",
                    type: "POST",
                    dataType: "json",
                    data: { PreRegNum: ui.item.value },
                    success: function (data) {
                        $("#ddlDepartment").val(data.Department);
                        $("#ddlSubDepartment").val(data.SubDepartment);
                        $("#ddlProgramme").val(data.Programme);
                        $("#ddlDesignation").val(data.Designation);
                        $('#ddlDepartment').attr('disabled', true);
                        $('#ddlSubDepartment').attr('disabled', true);
                        $('#ddlProgramme').attr('disabled', true);
                        $('#ddlDesignation').attr('disabled', true);
                    }
                });
            },
            focus: function (event, ui) {
                event.preventDefault();
                $("#txtStaffName").val(ui.item.label);
            },
            messages: {
                noResults: "", results: ""
            }
        });
        return false;
    }
    else {
        return false;
    }
}

function FillSubDepartment() {
    $.getJSON("/Base/FillSubDepartment",
      function (fillbc) {
          var ddlbc = $("#ddlSubDepartment");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
} function FillSubDepartmentForSearch() {
    $.getJSON("/Base/FillSubDepartment",
      function (fillbc) {
          var ddlbc = $("#SrchddlSubDepartment");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}

function FillProgramme() {
    $.getJSON("/Base/FillProgramme",
      function (fillbc) {
          var ddlbc = $("#ddlProgramme");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillProgrammeForSearch() {
    $.getJSON("/Base/FillProgrammeByCampusAndStaffType",
      function (fillbc) {
          var ddlbc = $("#SrchddlProgramme");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function ShowCampusBasedStaffDetails(StaffPreRegNumber, Campus) {
    ModifiedLoadPopupDynamicaly("/StaffManagement/ShowCampusBasedStaffDetails?StaffPreRegNumber=" + StaffPreRegNumber + '&Campus=' + Campus, $('#divShowCampusBasedStaffDetails'),
            function () { }, function () { }, 960, 500, "Staff Details");
}
function FillReportingManager() {
    var Campus = $("#SrchddlCampus").val();
    $.getJSON("/StaffManagement/FillReportingManagersNameAndDesignationByCampus?campus=" + Campus,
      function (fillbc) {
          var ddlbc = $("#SrchddlReportingDesignation");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
//function FillReportingManager() {
//    debugger;
//    //$("#ddlReportingDesignation").multiselect('rebuild');
//    var Campus = $("#SrchddlCampus").val();
//    var ddlbc = $("#SrchddlReportingDesignation");
//    if (Campus != null && Campus != "") {
//        $.getJSON("/StaffManagement/FillReportingManagersNameAndDesignationByCampus?campus=" + Campus,
//          function (fillbc) {
//              ddlbc.empty();
//              $.each(fillbc, function (index, itemdata) {
//                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//              });
//          });
//    }
//}
function FillReportingDesignation() {
    //$("#ddlReportingDesignation").multiselect('rebuild');
    var Campus = $("#ddlcampus").val();
    var ddlbc = $("#ddlReportingDesignation");
    if (Campus != null && Campus != "") {
        $.getJSON("/StaffManagement/FillReportingManagersNameAndDesignationByCampus?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
              $('#ddlReportingDesignation').multiselect('enable');
              $('#ddlReportingDesignation').multiselect('rebuild');
          });
    }
}
function RefreshddlReportingDesignationDdl() {
    $('#ddlReportingDesignation').multiselect('rebuild');
}
function FillReportingManagersDesignation() {
    var Campus = $("#SrchddlCampus").val();
    $.getJSON("/StaffManagement/FillReportingManagersDesignation?Campus=" + Campus,
     function (fillbc) {
         var ddlbc = $("#SrchDdlReportingHeads");
         ddlbc.empty();
         ddlbc.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillbc, function (index, itemdata) {
             ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
         });
     });
}
function UpdateAddPanelById(Id) {
    $.ajax({
        url: "/StaffManagement/GetCampusBasedStaffDetailsById?Id=" + Id,
        type: "GET",
        dataType: "json",
        success: function (Obj) {
            //alert(Obj.Campus);
            $('#StaffPreRegNum').val(Obj.StaffPreRegNumber);
            $('#ddlcampus').val(Obj.Campus);
            FillGradeByCampus();
            FillSubjectByCampusandGrade();
            $('#txtStaffName').val(Obj.StaffName);
            $('#ddlDepartment').val(Obj.Department);
            $('#ddlProgramme').val(Obj.Programme);
            $('#ddlcampus').attr("disabled", true);
            $('#txtStaffName').attr("disabled", true);
            $('#ddlDepartment').attr("disabled", true);
            $('#ddlProgramme').attr("disabled", true);
        }
    });
}
function FillProgrammeBasedOnCampusAndStaffType() {
    var Campus = $("#SrchddlCampus").val();
    var StaffType = $("#SrchddlStaffType").val();
    $.getJSON("/StaffManagement/FillProgrammeByCampusAndStaffType?Campus=" + Campus + '&StaffType=' + StaffType,
     function (fillbc) {
         var ddlbc = $("#SrchddlProgramme");
         ddlbc.empty();
         ddlbc.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillbc, function (index, itemdata) {
             ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
         });
     });
}
function FillSectionByCampusGrade() {
    var Campus = $("#SrchddlCampus").val();
    var Grade = $("#SrchddlGrade").val();
    $.getJSON("/StaffManagement/FillSectionBasedCampusGrade?Campus=" + Campus + '&Grade=' + Grade,
     function (fillbc) {
         var ddlbc = $("#ScrchddlSection");
         ddlbc.empty();
         ddlbc.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillbc, function (index, itemdata) {
             ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
         });
     });
}