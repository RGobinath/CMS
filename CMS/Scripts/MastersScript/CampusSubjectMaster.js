function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillGradeByCampus() {
    var Campus = $("#ddlCampus").val();
    $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
      function (fillbc) {
          debugger;
          var ddlbc = $("#ddlGrade");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {                            
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));              
          });
      });
}
function FillSubjectByCampusandGrade() {
    var Campus = $("#ddlCampus").val();
    var Grade = $("#ddlGrade").val();
    var ddlbc = $("#ddlSubject");
    if (Campus != "" && Grade != "") {
        $.getJSON("/Base/GetSubjectsByCampusGrade?Campus=" + Campus + '&Grade=' + Grade,
          function (fillbc) {              
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  debugger;
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
function FillAcademicYearDll() {
    $.getJSON("/Base/GetJsonAcademicYear",
      function (fillbc) {
          var ddlbc = $("#ddlAcademicYear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
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
    var jqGridstartDate = new Date();
    var jqGridstartDate = 01 + '/' + 04 + '/' + 1947;
    jQuery(grid_selector).jqGrid({
        url: '/Common/CampusSubjectMasterJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Section', 'Subject', 'Description', 'Academic Year', 'Is Academic Subject', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'Id', index: 'Id', key: true, width: 130, hidden: true, editable: true },
                      {
                          name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true }, editoptions: {
                              dataUrl: '/Assess360/GetCampusddl',
                              dataEvents: [{
                                  type: 'change',
                                  fn: function (e) {
                                      var Campus = $(e.target).val();
                                      if (Campus != '') {
                                          $.getJSON('@Url.Action("GetGradeByCampus", "Assess360")',
                                          { Campus: Campus },
                                          function (recipes) {
                                              var selectHtml = "";
                                              selectHtml += '<option value=" ">--select--</option>';
                                              $.each(recipes, function (jdIndex, jdData) {
                                                  selectHtml = selectHtml + "<option value='" + jdData.Value + "'>" + jdData.Text + "</option>";
                                              });
                                              if ($(e.target).is('.FormElement')) {
                                                  var form = $(e.target).closest('form.FormGrid');
                                                  $("select#Grade.FormElement", form[0]).html(selectHtml);
                                              }
                                          });
                                      }
                                  }
                              }], style: "width: 265px; height: 25px; font-size: 0.9em"
                          },
                          stype: 'select', sortable: true,
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
                           name: 'Grade', index: 'Grade', editable: true, search: true, edittype: 'select',
                           editoptions: { dataUrl: '/Assess360/GetGradeddl', style: "width: 265px; height: 25px; font-size: 0.9em" },
                           editrules: { required: true, custom: false }, sortable: true, stype: 'select',
                           searchoptions: { dataUrl: '/Assess360/GetGradeddl' }
                       },
                       { name: 'Section', index: 'Section', editable: true, edittype: 'select', editoptions: { sopt: ['eq'], value: { '': '--Select--', 'A': 'A', 'B': 'B', 'C': 'C', 'D': 'D', 'E': 'E', 'F': 'F', }, style: "width: 265px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, stype: 'select', search: true },
                       {
                           name: 'SubjectName', index: 'SubjectName', editable: true, search: true, edittype: 'select',
                           editoptions: { dataUrl: '/Base/GetSubjectsByGradeddl', style: "width: 265px; height: 25px; font-size: 0.9em" },
                           editrules: { required: true, custom: false }, sortable: true, stype: 'select',
                           searchoptions: { dataUrl: '/Base/GetSubjectsByGradeddl' }
                       },
                       { name: 'Description', index: 'Description', editable: false },
                       { name: 'AcademicYear', index: 'AcademicYear', editable: true, search: true, edittype: 'select', editoptions: { dataUrl: '/Assess360/AcademicYearddl', style: "width: 265px; height: 25px; font-size: 0.9em" }, editrules: { required: true, custom: false }, sortable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/AcademicYearddl' } },
                       { name: 'IsAcademicSubject', index: 'IsAcademicSubject', editable: true, edittype: 'select', editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 265px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, stype: 'select' },
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
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {                
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Campus Subject Master",
    })    
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {
                width: 'auto', url: '/Common/EditCampusSubjectMaster', modal: false,
                closeAfterEdit: true

            }, //Edit
            {
                width: 'auto', url: '/Common/AddCampusSubjectMaster', modal: false,                

            }, //Add
              {
                  width: 'auto', url: '/Common/DeleteCampusSubjectMaster', beforeShowForm: function (params) {
                      selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                      return { Id: selectedrows }
                  }
              },
               {},
                {})
    //$(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });
    $("#ddlCampus").change(function () {
        FillGradeByCampus();
    });
    $("#ddlCampus,#ddlGrade").change(function () {
        FillSubjectByCampusandGrade();
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Common/CampusSubjectMasterJqGrid',
           postData: { Campus: "", Grade: "", Section: "", AcademicYear: "", Description: "", IsAcademicSubject: "", SubjectName: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {        
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlGrade").val();
        var Section = $("#ddlSection").val();
        var AcademicYear = $("#ddlAcademicYear").val();
        var SubjectName = $("#ddlSubject").val();
        var IsAcademicSubject = $("#ddlIsAcademicSubject").val();        
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Common/CampusSubjectMasterJqGrid',
           postData: { Campus: Campus, Grade: Grade, Section:Section,AcademicYear: AcademicYear,IsAcademicSubject:IsAcademicSubject,SubjectName:SubjectName },
           page: 1
       }).trigger("reloadGrid");
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