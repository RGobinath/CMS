function FillSurveyNameDll() {
    $.getJSON("/StaffManagement/GetSurveyNameddl",
      function (fillbc) {
          var ddlbc = $("#ddlSurveyNumber");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
          });
          //$('#ddlcampus').multiselect('rebuild');
      });
}
function SrchFillSurveyNameDll() {
    $.getJSON("/StaffManagement/GetSurveyNameddl",
      function (fillbc) {
          var ddlbc = $("#SrchddlSurveyNumber");
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
          $('#ddlcampus').multiselect('rebuild');
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
              $('#ddlgrade').multiselect('rebuild');
          });
    }
    if (Campus == "" || Campus == null) {
        ddlbc.empty();
        $('#ddlgrade').multiselect('rebuild');
    }
}
function FillddlGradeByCampus() {    
    var Campus = $("#Srchddlcampus").val();
    var ddlbc = $("#Srchddlgrade");
    if (Campus != "") {
        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });              
          });
    }
    else{
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
//jQuery(function ($) {
$(function () {
    FillCampusDll();
    FillAcademicYearDll();
    SrchFillSurveyNameDll();
    FillSurveyNameDll();
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
        url: '/StaffManagement/GetJqGridSurveyConfigurationList',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Survey Configuration Id', 'Academic Year', 'Campus', 'Grade', 'Section','Survey Number','Is Active'],
        colModel: [
            { name: 'SurveyConfigurationId', index: 'SurveyConfigurationId', key: true, width: 130, hidden: true, editable: true },
            { name: 'AcademicYear', index: 'AcademicYear', editable: false, search: true, sortable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/AcademicYearddl' } },
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
                      { name: 'Grade', index: 'Grade', editable: false, search: true, sortable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/GetGradeddl' } },
                      { name: 'Section', index: 'Section', editable: true, search: true, sortable: true },
                      { name: 'SurveyNumber', index: 'SurveyNumber', editable: false, search: true, sortable: true },
                      //{ name: 'SurveyGroupMaster.SurveyGroupId', index: 'SurveyGroupMaster.SurveyGroupId', editable: false, search: true, sortable: true,hidden:true },
                      { name: 'IsActive', index: 'IsActive', editable: false, width: 60, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
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
        multiselect: true,
        sortname: 'Id',
        sortorder: 'Desc',
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Survey Configuration List",
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
                url: '', closeAfterEdit: true, closeOnEscape: true
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
    $('#ddlgrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '165',
        numberDisplayed: 1,
        nonSelectedText: 'None Selected',
        includeSelectAllDivider: true
    });
    $('#ddlcampus').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '165',
        numberDisplayed: 1,
        nonSelectedText: 'None Selected',
        includeSelectAllDivider: true
    });
    $('#ddlSurveyGroup').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '165',
        numberDisplayed: 1,
        nonSelectedText: 'None Selected',
        includeSelectAllDivider: true
    });
    $("#btnReset").click(function () {
        //$("input[type=text], textarea, select").val("");
        $("#ddlcampus").val('');
        $("#ddlgrade").val('');
        $("#ddlcampus").multiselect('rebuild');
        FillGradeByCampus();
        $("#ddlacademicyear").val('');
        $("#ddlSurveyGroup").val('');
        $("#ddlSurveyNumber").val('');
        $('#ddlSurveyGroup').multiselect('rebuild');
    });
    $("#SrchbtnReset").click(function () {
        //$("input[type=text], textarea, select").val("");
        $("#Srchddlcampus").val('');
        $("#Srchddlgrade").val('');
        $("#Srchddlacademicyear").val('');
        //$("#SrchddlSurveyGroup").val('');
        $("#SrchddlSurveyNumber").val('');
        FillddlGradeByCampus();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetJqGridSurveyConfigurationList',
           postData: { Campus: "", Grade: "", AcademicYear: "", SurveyNumber: "", IsActive: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#Srchddlcampus").change(function () {
        FillddlGradeByCampus();
    });
    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        var Campus = $("#Srchddlcampus").val();
        var Grade = $("#Srchddlgrade").val();
        var AcademicYear = $("#Srchddlacademicyear").val();
        //var SurveyGroupId = $("#SrchddlSurveyGroup").val();
        //var IsActive = $("#ddlIsActive").val();
        var SurveyNumber = $("#SrchddlSurveyNumber").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetJqGridSurveyConfigurationList',
           //postData: { Campus: Campus, Grade: Grade, Section: Section, AcademicYear: AcademicYear, CategoryName: CategoryName },
           postData: { Campus: Campus, Grade: Grade, AcademicYear: AcademicYear,SurveyNumber: SurveyNumber },
           page: 1
       }).trigger("reloadGrid");
    });
    $('#btnSave').click(function () {
        var Campus = $("#ddlcampus").val();
        var Grade = $("#ddlgrade").val();
        var SurveyGroup = $("#ddlSurveyGroup").val();
        var AcademicYear = $("#ddlacademicyear").val();
        var SurveyNumber = $("#ddlSurveyNumber").val();
        if (Campus == "" || Grade == "" || SurveyGroup == "" || AcademicYear == "" || SurveyNumber == "") {
            ErrMsg("Please Fill the Required Details");
            return false;
        }
        $.ajax({
            Type: 'POST',
            dataType: 'json',
            url: '/StaffManagement/AddSurveyConfiguration?Campus=' + Campus + '&Grade=' + Grade + '&AcademicYear=' + AcademicYear + '&SurveyGroupId=' + SurveyGroup + '&SurveyNumber=' + SurveyNumber,
            success: function (JsonRetVal) {
                if (JsonRetVal != null && JsonRetVal != "") {
                    InfoMsg(JsonRetVal);
                    $(grid_selector).trigger('reloadGrid');
                }
            }
        });

    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $.getJSON("/StaffManagement/GetSurveyGroupddl/",
                    function (modelData) {
                        var select = $("#ddlSurveyGroup");
                        select.empty();
                        select.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
                        $('#ddlSurveyGroup').multiselect('rebuild');
                    });
    $.getJSON("/StaffManagement/GetSurveyGroupddl/",
                    function (modelData) {
                        var ddlsg = $("#SrchddlSurveyGroup");
                        ddlsg.empty();
                        ddlsg.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            ddlsg.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
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
