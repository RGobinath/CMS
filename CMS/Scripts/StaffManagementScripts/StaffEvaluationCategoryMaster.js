function FillSurveyNameDll() {
    $.getJSON("/StaffManagement/GetSurveyNameddl",
      function (fillbc) {
          var ddlbc = $("#ddlCategoryName");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
          });
          //$('#ddlcampus').multiselect('rebuild');
      });
}
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
function FillGradeByCampus() {
    debugger;
    var Campus = [];
    Campus = $("#ddlcampus").val();
    var ddlbc = $("#ddlgrade");
    if (Campus != null && Campus != "") {
        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
              //$('#ddlgrade').multiselect('rebuild');
          });
    }
    if (Campus == "" || Campus == null) {
        ddlbc.empty;
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

function getAllSurveyno() {
    ddlacyear = $("#Srchddlacademicyear").val();
    ddlcam = $("#Srchddlcampus").val();
    ddlgrd = $("#Srchddlgrade").val();
    ddlsec = $("#SrchddlSection").val();
    var ddlSub = $("#SrchddlSurveyNo");
    if (ddlcam != "", ddlgrd != "") {
        var url = '/StaffManagement/GetStaffEvaluationCategoryByCampusGrade?AcaYear=' + ddlacyear + '&Campus=' + ddlcam + '&Grade=' + ddlgrd + '&Section=' + ddlsec;
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
//jQuery(function ($) {
$(function () {
    FillCampusDll();
    FillSurveyNameDll();
    FillAcademicYearDll();
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
        url: '/StaffManagement/StaffEvaluationCategoryMasterJqgrid',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Staff Evaluation Category Id', 'Academic Year', 'Campus', 'Grade','Section', 'Survey Number', 'Survey Date', 'Survey OTP', 'Valid/In Valid', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
            { name: 'StaffEvaluationCategoryId', index: 'StaffEvaluationCategoryId', key: true, width: 130, hidden: true, editable: true },
            { name: 'AcademicYear', index: 'AcademicYear', editable: false, search: true, sortable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/AcademicYearddl' } },
            { name: 'Campus', index: 'Campus', editable: false, edittype: 'select', stype: 'select', sortable: true,
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
                      { name: 'Grade', index: 'Grade', editable: false, search: true, sortable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/GetGradeddl' } },
                      { name: 'Section', index: 'Section', editable: false, search: true, sortable: true },
                      { name: 'CategoryName', index: 'CategoryName', editable: false },
                      { name: 'EvaluationDate', index: 'EvaluationDate', editable: false },
                      { name: 'OTP', index: 'OTP', editable: false },
                      { name: 'IsActive', index: 'IsActive', editable: true, width: 60, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Valid', 'false': 'In Valid' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        sortname: 'StaffEvaluationCategoryId',
        sortorder: 'Desc',
        multiboxonly: true,
        onSelectRow: function (id) {
            var myGrid = $(grid_selector),
            selRowId = myGrid.jqGrid('getGridParam', 'selrow'),
            celValue = myGrid.jqGrid('getCell', selRowId, 'IsActive');
            if (celValue == "In Valid") {
                $("#edit_grid-table").hide();
                $('#jqg_grid-table_' + id).attr('checked', false)
                myGrid.jqGrid('resetSelection');
                return false;
            }
            if (celValue == "Valid") {
                if ($("#showedit").val() == "True") {
                    $("#edit_grid-table").show();
                    return true;
                }
                else {
                    $("#edit_grid-table").hide();
                    $('#jqg_grid-table_' + id).attr('checked', false)
                    myGrid.jqGrid('resetSelection');
                    return false;
                }

            }
        },
        loadComplete: function () {
            $("#edit_grid-table").hide();
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;OTP Configuration List",
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
                url: '/StaffManagement/EditStaffEvaluationCategory/', closeAfterEdit: true, closeOnEscape: true
            }, //Edit
            {

            }, //Add
              {
              },
               {},
                {})

    //$(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });
    $("#ddlcampus").change(function () {
        FillGradeByCampus();
    });
    //$("#ddlgrade").change(function () {
    //    ddlMonth();
    //});

    $('#txtSurveyDate').datepicker({
        format: "dd/mm/yyyy",
        numberOfMonths: 1,
        autoclose: true,
        startDate:new Date(),
        //endDate:new Date(),
        //        onSelect: function () {
        //            $('.date-picker').hide();
        //        }
    });
    //$('#ddlgrade').multiselect({
    //    includeSelectAllOption: true,
    //    selectAllText: ' Select all',
    //    enableCaseInsensitiveFiltering: true,
    //    enableFiltering: true,
    //    maxHeight: '280',
    //    numberDisplayed: 1,
    //    nonSelectedText: 'None Selected',
    //    includeSelectAllDivider: true
    //});
    //$('#ddlcampus').multiselect({
    //    includeSelectAllOption: true,
    //    selectAllText: ' Select all',
    //    enableCaseInsensitiveFiltering: true,
    //    enableFiltering: true,
    //    maxHeight: '280',
    //    numberDisplayed: 1,
    //    nonSelectedText: 'None Selected',
    //    includeSelectAllDivider: true
    //});
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        //$("#ddlcampus").val('');
        //$("#ddlgrade").val('');
        //FillGradeByCampus();
        //$("#ddlacademicyear").val('');
        //$("#txtCategoryName").val('');
    });
    $("#SrchbtnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        //$("#Srchddlcampus").val('');
        //$("#Srchddlgrade").val('');
        //$("#Srchddlacademicyear").val('');
        //$("#SrchtxtCategoryName").val('');
        //$("#ddlIsActive").val('');
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffEvaluationCategoryMasterJqgrid',
           postData: { Campus: "", Grade: "", Section: "", AcademicYear: "", CategoryName: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        var Campus = $("#Srchddlcampus").val();
        var Grade = $("#Srchddlgrade").val();
        var AcademicYear = $("#Srchddlacademicyear").val();
        var Section = $("#SrchddlSection").val();
        var CategoryName = $("#SrchtxtCategoryName").val();
        //var IsActive = $("#ddlIsActive").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffEvaluationCategoryMasterJqgrid',
           postData: { Campus: Campus, Grade: Grade, Section: Section, AcademicYear: AcademicYear, CategoryName: CategoryName },
           //postData: { Campus: Campus, Grade: Grade, Section: Section, AcademicYear: AcademicYear, CategoryName: CategoryName, IsActive: IsActive },
           page: 1
       }).trigger("reloadGrid");
    });
    $('#btnSave').click(function () {
        var Campus = $("#ddlcampus").val();
        var Section = $("#ddlSection").val();
        
        var Grade = $("#ddlgrade").val();
        var CategoryName = $("#ddlCategoryName").val();
        var AcademicYear = $("#ddlacademicyear").val();
        var date = $("#txtSurveyDate").val();
        if (Section =="" || Campus == "" || Grade == "" || CategoryName == "" || AcademicYear == "" || date == "" || Campus == null || Grade == null || CategoryName == null || AcademicYear == null || date == null) {
            ErrMsg("Please Fill the Required Details");
            return false;
        }
        $.ajax({
            Type: 'POST',
            dataType: 'json',
            url: '/StaffManagement/AddStaffEvaluationCategory?Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section + '&AcademicYear=' + AcademicYear + '&CategoryName=' + CategoryName + '&EvaluationDate=' + date,
            success: function (data) {
                if (data == "success") {
                    SucessMsg("Added Successfully");
                    $(grid_selector).trigger('reloadGrid');
                }
                else if (data == "notexist") {
                    ErrMsg("Please Configure the Survey in Survey Configuration Before Generating OTP..");
                    return false;
                }
                else {
                    ErrMsg("Already Exist");
                }
                //if (data.Message == "success") {
                //$('#calendar').fullCalendar('refetchEvents');
                //$("input[type=text], textarea, select").val("");
                //$('input[name=EventColor]').attr('checked', false);
                //$("#ddlStaffName").multiselect('refresh');
                //$("#ddlInstitution").multiselect('refresh');
                //}
                //if (data.Message == "failed") {
                //    ErrMsg("The Task is Already Exist");
                //}
            }
        });

    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
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
