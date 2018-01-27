$(function () {

    var grid_selector = "#WeightingMasterjqGrid";
    var pager_selector = "#WeightingMasterPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand 
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $(grid_selector).jqGrid({
        url: "/Assess360/WeightingMasterJqGrid",
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Academic Year', 'Subject', 'AssignmentName', 'Weightings', 'TotalMarks', ],
        colModel: [
          { name: 'Id', index: 'Id', hidden: true, key: true },
          { name: 'Campus', index: 'Campus', width: 80, editable: true, edittype: 'select', editoptions: { dataUrl: '/Home/BranchCodeddl' }, editrules: { custom: true, custom_func: checkvalid } },
          { name: 'Grade', index: 'Grade', width: 40, editable: true, edittype: 'select', editoptions: { dataUrl: '/Admission/Gradeddl' }, editrules: { custom: true, custom_func: checkvalid } },
          { name: 'AcademicYear', index: 'AcademicYear', width: 80, editable: true, edittype: 'select', editoptions: { dataUrl: '/Assess360/AcademicYearddl' }, editrules: { custom: true, custom_func: checkvalid } },
          {name: 'Subject', index: 'Subject', width: 200, editable: true, edittype: 'select', editoptions: {
              dataUrl: '/Assess360/GetSubjects', buildSelect: function (data) {
                  Subject = jQuery.parseJSON(data);
                  var s = '<select>';
                  if (Subject && Subject.length) {
                      for (var i = 0, l = Subject.length; i < l; i++) {
                          var mg = Subject[i];
                          s += '<option value="' + mg + '">' + mg + '</option>';
                      }
                  }
                  return s + "</select>";
              }
          }, editrules: { custom: true, custom_func: checkvalid }
          },
          {name: 'ComponentTitle', index: 'ComponentTitle', width: 200, editable: true, editoptions: {
              dataInit: function (element) {
                  $(element).autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: "/Assess360/GetAssignmentNameByCampusGradeSubject",
                              type: "POST",
                              dataType: "json",
                              data: {cam:$('#Campus').val(),gra:$('#Grade').val(),sub:$('#Subject').val(), term: request.term },
                              success: function (data) {
                                  response($.map(data, function (item) {
                                      return { label: item.text, value: item.Value };
                                  }))
                              }
                          })
                      },
                      messages: {
                          noResults: "", results: ""
                      }
                  });
                        
              }
          }},
          { name: 'Weightings', index: 'Weightings', width: 50, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid } },
          { name: 'TotalMarks', index: 'TotalMarks', width: 50, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid } },
        ],

        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '250',
        closeAfterEdit: true,
        closeAfterAdd: true,
        autowidth: true,
        // shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>Assignment Name Master',
        forceFit: true,
        multiselect: true,
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
        { 	//navbar options
            edit: false,
            editicon: 'ace-icon fa fa-pencil blue',
            add: true,
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
     { width: 'auto', url: '/Assess360/AddRptCardMarkWeightingMaster?test=edit', left: '10%', top: '10%', height: '50%', width: 400 },
     { width: 'auto', url: '/Assess360/AddRptCardMarkWeightingMaster', left: '10%', top: '10%', height: '50%', width: 400 }
    //{ width: 'auto', url: '/Assess360/AddAssignmentName?test=del', left: '10%', top: '10%', height: '50%', width: 400, beforeShowForm: function (params) { var gsr = $("#AssignmentNameList").getGridParam('selrow'); var sdata = $("#AssignmentNameList").getRowData(gsr); return { Id: sdata.Id} } }
     );
    $("#ComponentTitle").autocomplete({
        source: function (request, response) {
            $.getJSON('/Assess360/GetAssignmentNameByCampusGradeSubject?cam='+$("#ddlCampus").val()+'&gra='+$("#ddlGrade").val()+'&sub='+$("#ddlSubject").val()+'&term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $("#Search").click(function () {

        $(grid_selector).clearGridData();
        var cam = $("#ddlCampus").val();
        var gra = $("#ddlGrade").val();
        var sub = $("#ddlSubject").val();
        var acyear = $("#ddlyear").val();
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: "/Assess360/WeightingMasterJqGrid/",
                postData: { Campus: cam, Grade: gra, AcademicYear: acyear, Subject: sub },
                page: 1
            }).trigger("reloadGrid");
    });

    $("#ddlGrade").change(function () {
        GetSubjectsByGrade($(this).val(), $('#ddlSubject'));
    });
    $("#ddlCampus").change(function () {
        gradeddl($("#ddlCampus").val(), $('#ddlGrade'));
    });

    // ------------------------added by anto------------
    $("#Campusddl").change(function () {
        gradeddl($("#Campusddl").val(),$('#Gradeddl'));
    });
    $("#Gradeddl").change(function () {
        GetSubjectsByGrade($(this).val(), $('#Subjectddl'));
    });

    $("#resetAdd").click(function () {
        $("#Campusddl").val('');
        $("#Gradeddl").val('');
        $("#ddlSubject").val('');
        $("#ddlyear").val('');
        $("#AssignmentName").val('');
    });
});



function gradeddl(campus, dllId) {
    debugger;
    //var campus = $("#ddlCampus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
        function (modelData) {
            var select = dllId;
            select.empty();
            select.append($('<option/>', { value: "", text: "Select Grade" }));
            $.each(modelData, function (index, itemData) {
                select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
            });
        });
}

function GetSubjectsByGrade(grade,dllId) {
    var ddlsub = dllId;
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetSubjectsByGrade?Grade=' + grade,
        success: function (data) {
            ddlsub.empty();
            ddlsub.append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                ddlsub.append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}

function AddWeightingMaster() {
    var cam = $("#Campusddl").val();
    var gra = $("#Gradeddl").val();
    var sub = $("#Subjectddl").val();
    var Acayr = $("#Yearddl").val();
    var Assnme = $("#ComponentTitle").val();
    var tot = $("#TotalMarks").val();
    var weight = $("#Weightings").val();
    if (cam == '' || gra == '' || sub == '' || Assnme == '' || tot=='' || weight=='') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/Assess360/AddRptCardMarkWeightingMaster?Campus=" + cam + '&Grade=' + gra + '&Subject=' + sub + '&Academicyear=' + Acayr + '&ComponentTitle=' + Assnme + '&TotalMarks=' + tot + '&Weightings=' + weight,
        success: function (data) {
            $("#WeightingMasterjqGrid").trigger('reloadGrid');
            $("#ComponentTitle").val('');
            $("#TotalMarks").val('');
            $("#Weightings").val('');
        }
    });
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
            return [true];
        }
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