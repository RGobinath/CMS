function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlcampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
          //$('#ddlcampus').multiselect('rebuild');
      });
}
function FillCampusDll1() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#Srchddlcampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
          //$('#ddlcampus').multiselect('rebuild');
      });
}
function FillCategoryName() {
    var Campus = $("#ddlcampus").val();
    var Grade = $("#ddlgrade").val();
    var AcademicYear = $("#ddlacademicyear").val();
    var Month = $("#ddlmonth").val();
    var ddlbc = $("#ddlCategoryName");
    if (Campus != "" && Campus != null && Grade != null && Grade != "" && AcademicYear != null && AcademicYear != "" && Month != null && Month != "") {
        $.getJSON("/StaffManagement/FillCategoryName?Campus=" + Campus + '&Grade=' + Grade + '&AcademicYear=' + AcademicYear + '&Month=' + Month,
                  function (fillbc) {
                      debugger;
                      ddlbc.empty();
                      ddlbc.append($('<option/>', { value: "", text: "Select One" }));
                      $.each(fillbc, function (index, itemdata) {
                          ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                      });
                      //$('#ddlgrade').multiselect('rebuild');
                  });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
    }
}
function FillCategoryName1() {
    var Campus = $("#Srchddlcampus").val();
    var Grade = $("#Srchddlgrade").val();
    var AcademicYear = $("#Srchddlacademicyear").val();
    var Month = $("#Srchddlmonth").val();
    var ddlbc = $("#SrchddlCategoryName");
    if (Campus != "" && Campus != null && Grade != null && Grade != "" && AcademicYear != null && AcademicYear != "" && Month != "" && Month != null) {
        $.getJSON("/StaffManagement/FillCategoryName?Campus=" + Campus + '&Grade=' + Grade + '&AcademicYear=' + AcademicYear + '&Month=' + Month,
                  function (fillbc) {
                      debugger;
                      ddlbc.empty();
                      ddlbc.append($('<option/>', { value: "", text: "Select One" }));
                      $.each(fillbc, function (index, itemdata) {
                          ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                      });
                      //$('#ddlgrade').multiselect('rebuild');
                  });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
    }
}
function FillGradeByCampus() {
    var Campus = $("#ddlcampus").val();
    var ddlbc = $("#ddlgrade");
    if (Campus != null && Campus != "") {
        $.getJSON("/Admission/CampusGradeddl?Campus=" + Campus,
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.gradcod, text: itemdata.gradcod }));
              });
              //$('#ddlgrade').multiselect('rebuild');
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
        //$('#ddlgrade').multiselect('refresh');
    }
}
function FillGradeByCampus1() {
    var Campus = $("#Srchddlcampus").val();
    var ddlbc = $("#Srchddlgrade");
    if (Campus != null && Campus != "") {
        $.getJSON("/Admission/CampusGradeddl?Campus=" + Campus,
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.gradcod, text: itemdata.gradcod }));
              });
              //$('#ddlgrade').multiselect('rebuild');
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
        //$('#ddlgrade').multiselect('refresh');
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
function FillAcademicYearDll1() {
    $.getJSON("/Base/GetJsonAcademicYear",
      function (fillbc) {
          var ddlbc = $("#Srchddlacademicyear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function ddlMonth() {
    debugger;
    var monthVal = $("#ddlmonth");
    if ($("#ddlgrade").val() != "") {
        $.getJSON("/Base/GetMonthValbyAcademicYearandGrade?academicYear=" + $("#ddlacademicyear").val() + '&grade=' + $("#ddlgrade").val(),
          function (fillbc) {
              monthVal.empty();
              monthVal.append($('<option/>',
             {
                 value: "",
                 text: "Select One"

             }));

              $.each(fillbc, function (index, itemdata) {
                  monthVal.append($('<option/>',
                      {
                          value: itemdata.Text,
                          text: itemdata.Text
                      }));

              });
          });
    }
    else {
        monthVal.empty();
        monthVal.append($('<option/>',
       {
           value: "",
           text: "Select One"

       }));
    }
}
function SrchddlMonth() {
    var monthVal = $("#Srchddlmonth");
    if ($("#Srchddlgrade").val() != "") {
        $.getJSON("/Attendance/GetMonthValbyAcademicYearandGrade?academicYear=" + $("#Srchddlacademicyear").val() + '&grade=' + $("#Srchddlgrade").val(),
          function (fillbc) {
              monthVal.empty();
              monthVal.append($('<option/>',
             {
                 value: "",
                 text: "Select One"

             }));

              $.each(fillbc, function (index, itemdata) {
                  monthVal.append($('<option/>',
                      {
                          value: itemdata.Text,
                          text: itemdata.Text
                      }));

              });
          });
    }
    else {
        monthVal.empty();
        monthVal.append($('<option/>', { value: "", text: "Select One" }));
    }
}
//jQuery(function ($) {
$(function () {
    FillCampusDll1();
    FillCampusDll();
    FillAcademicYearDll();
    FillAcademicYearDll1()
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
        url: '/StaffManagement/StaffEvaluationQuestionnairesJqgrid',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Id', 'Staff Evaluation Parameter Id', 'StaffEvaluationCategoryId', 'Campus', 'Grade', 'Academic Year', 'Month', 'Category Name', 'Questions','Is Positive', 'Is Active'],
        colModel: [
                      { name: 'Id', index: 'Id', width: 130, hidden: true, editable: true, key: true },
                      { name: 'StaffEvaluationParameterId', index: 'StaffEvaluationParameterId', width: 130, hidden: true, editable: true },
                      { name: 'StaffEvaluationCategoryId', index: 'StaffEvaluationCategoryId', width: 130, hidden: true, editable: true },
                      {
                          name: 'Campus', index: 'Campus', editable: false, edittype: 'select', stype: 'select', sortable: true,
                          searchoptions: {
                              dataUrl: '/Assess360/GetCampusddl',
                              dataEvents: [{
                                  type: 'change',
                                  fn: function (e) {
                                      var Campus = $(e.target).val();
                                      if (Campus != '') {
                                          $.getJSON('@Url.Action("GetGradeByCampus", "Assess360")',
                                                                      { Campus: Campus },
                                                                      function (cmp) {
                                                                          debugger;
                                                                          var selectHtml1 = "";
                                                                          selectHtml1 += '<option value="">--select--</option>';
                                                                          $.each(cmp, function (jdIndex1, jdData1) {
                                                                              selectHtml1 = selectHtml1 + "<option value='" + jdData1.Value + "'>" + jdData1.Text + "</option>";
                                                                          });
                                                                          $("#gs_Grade").html(selectHtml1);
                                                                      });
                                      }
                                  }
                              }],
                              style: "width: 265px; height: 25px; font-size: 0.9em"
                          }
                      },
                       {
                           name: 'Grade', index: 'Grade', editable: false, search: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/GetGradeddl' }
                       },
                      { name: 'AcademicYear', index: 'AcademicYear', editable: false, search: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/AcademicYearddl' } },
                      { name: 'Month', index: 'Month', },
                      { name: 'CategoryName', index: 'CategoryName', editable: false, search: true, stype: 'select', searchoptions: { dataUrl: '/StaffManagement/CategoryNameddl' } },
                      { name: 'StaffEvaluationParameters', index: 'StaffEvaluationParameters', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "55" } },
                      { name: 'IsPositive', index: 'IsPositive', editable: true, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      { name: 'IsActive', index: 'IsActive', editable: true, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      //{ name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      //{ name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      //{ name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      //{ name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortorder: "Asc",
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Staff Evaluation Questionnaires",
    })
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
    $(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
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
                width: '500', modal: false,
                url: '/StaffManagement/EditStaffEvaluationParameter/', closeAfterEdit: true, closeOnEscape: true
            }, //Edit
            {

            }, //Add
              {
                  //width: 'auto', url: '//', beforeShowForm: function (params) {
                  //    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                  //    return { Id: selectedrows }
                  //}
              },
               {},
                {})
    //$(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });
    $("#ddlcampus").change(function () {
        debugger;
        FillGradeByCampus();
    });
    $("#ddlgrade").change(function () {
        ddlMonth();
    });
    $("#Srchddlgrade").change(function () {
        SrchddlMonth();
    });
    $("#ddlcampus,#ddlacademicyear,#ddlgrade,#ddlmonth").change(function () {
        FillCategoryName();
    });
    $("#Srchddlcampus").change(function () {
        debugger;
        FillGradeByCampus1();
    });
    $("#Srchddlcampus,#Srchddlacademicyear,#Srchddlgrade,#Srchddlmonth").change(function () {
        debugger;
        FillCategoryName1();
    });
    $("#btnReset").click(function () {
        $("#ddlcampus").val('');
        FillGradeByCampus();
        FillCategoryName();
        $("#ddlacademicyear").val('');
        $("#txtareaQuestion").val('');
        $("#ddlmonth").val('');
        ddlMonth();
    });
    $("#SrchbtnReset").click(function () {
        $("#SrchtxtQuestion").val('');
        $("#ddlIsActive").val('');
        $("#Srchddlcampus").val('');
        $("#Srchddlacademicyear").val('');
        $("#Srchddlmonth").val('');
        $("#SrchddlsPositive").val('');
        SrchddlMonth();
        FillGradeByCampus1();
        FillCategoryName1();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffEvaluationQuestionnairesJqgrid',
           postData: { StaffEvaluationParameters: "", IsActive: "", Campus: "", Grade: "", AcademicYear: "", CategoryName: "", Month: "", IsPositive: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        var Question = $("#SrchtxtQuestion").val();
        var IsActive = $("#ddlIsActive").val();
        var Campus = $("#Srchddlcampus").val();
        var Grade = $("#Srchddlgrade").val();
        var AcademicYear = $("#Srchddlacademicyear").val();
        var CategoryName = $("#SrchddlCategoryName").val();
        var Month = $("#Srchddlmonth").val();
        var IsPositive = $("#SrchddlIsPositive").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffEvaluationQuestionnairesJqgrid',
           postData: { StaffEvaluationParameters: Question, IsActive: IsActive, Campus: Campus, Grade: Grade, AcademicYear: AcademicYear, CategoryName: CategoryName, Month: Month, IsPositive: IsPositive },
           page: 1
       }).trigger("reloadGrid");
    });
    $('#btnSave').click(function () {
        var StaffEvaluationParameters = $("#txtareaQuestion").val();
        var CategoryName = $("#ddlCategoryName").val();
        var IsPositive = $("#ddlIsPositive").val();
        if (CategoryName == "" || StaffEvaluationParameters == "" || IsPositive == "") {
            ErrMsg("Please Fill Details");
            return false;
        }
        $.ajax({
            Type: 'POST',
            dataType: 'json',
            url: '/StaffManagement/AddStaffEvaluationParameter?StaffEvaluationParameters=' + StaffEvaluationParameters + '&StaffEvaluationCategoryId=' + CategoryName + '&IsPositive=' + IsPositive,
            success: function (data) {
                if (data == "success") {
                    SucessMsg("Added Successfully");
                    $(grid_selector).trigger('reloadGrid');
                    $("#txtareaQuestion").val('');
                }
                else {
                    ErrMsg("Failed");
                }
            }
        });

    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});
//replace icons with FontAwesome icons like above
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