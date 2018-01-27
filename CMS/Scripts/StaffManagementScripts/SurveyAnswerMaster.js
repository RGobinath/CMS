$(function () {
    SurveyQuestionddl();
    var grid_selector = "#GetJqGridsurveyanswerList";
    var pager_selector = "#GetJqGridsurveyanswerListPager";

    //resize to fit page size
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
        url: '/StaffManagement/GetJqGridsurveyanswerList',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Survey Answer Id', 'Survey Question', 'Survey Answer', 'Survey Mark', 'Is Positive', 'Is Active', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'SurveyAnswerId', index: 'SurveyAnswerId', key: true, hidden: true, editable: true },
                      {
                          name: 'SurveyQuestionMaster.SurveyQuestionId', index: 'SurveyQuestionMaster.SurveyQuestionId', editable: true, search: true, edittype: 'select', editoptions: {
                              dataUrl: '/StaffManagement/GetSurveyQuestionddl',
                              buildSelect: function (data) {
                                  var SurveyQuestion = jQuery.parseJSON(data);
                                  var s = '<select>';
                                  s += '<option value="">----Select----</option>';
                                  if (SurveyQuestion && SurveyQuestion.length) {

                                      for (var i = 0, l = SurveyQuestion.length; i < l; i++) {

                                          s += '<option value="' + SurveyQuestion[i].Value + '">' + SurveyQuestion[i].Text + '</option>';
                                      }
                                  }
                                  return s + "</select>";
                              },
                              style: "width: 140px;"
                          }, editrules: { required: true }, sortable: true
                      },
                      { name: 'SurveyAnswer', index: 'SurveyAnswer', editable: true, editrules: { required: true }, },
                      { name: 'SurveyMark', index: 'SurveyMark', editable: true,editrules: { required: true }, sorttype: "float" ,template: "number" ,edittype:"text", editoptions:{
                          size: 20, maxlengh: 10,
                          dataInit: function(element) {
                              $(element).keypress(function(e){
                                  if (e.which != 8 && e.which != 0 && (e.which < 47 || e.which > 57)) {
                                      return false;
                                  }
                              });
                          }
                      }
                      },
                      { name: 'IsPositive', index: 'IsPositive', editable: true,editrules: { required: true }, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 140px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      { name: 'IsActive', index: 'IsActive', editable: false, editrules: { required: true }, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        sortname: 'SurveyAnswerId',
        sortorder: 'Asc',
        altRows: false,
        multiselect: true,
        multiboxonly: true,
        autowidth: true,
        //shrinkToFit:true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Survey Answer Master",
    })
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size            
    //navButtons
    $(grid_selector).jqGrid('navGrid', pager_selector,
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
            {
                url: '/StaffManagement/SaveOrUpdateSurveyAnswerMaster', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                url: '/StaffManagement/SaveOrUpdateSurveyAnswerMaster', closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add

		//{ url: '/StaffManagement/DeleteSurveyGroupMaster' },
               {},
                {})

    //$(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });
    // For Pager Icons
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }
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
    $("#btnReset").click(function () {
        //$("input[type=text], textarea, select").val("");
        $("#txtStudentSurveyGroup").val('');
    });
    $("#btnReset").click(function () {
        $("#ddlSurveyQuestion").val('');
        $("#txtSurveyAnswer").val('');
        $("#txtSurveyMark").val('');
        $("#ddlIsPositive").val('');
        $("#ddlIsActive").val('');

        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetJqGridsurveyanswerList',
           postData: { SurveyQuestionId: "", SurveyAnswer: "", SurveyMark: "", IsPositive: "", IsActive: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        //debugger;
        $(grid_selector).clearGridData();
        var SurveyQuestionId = $("#ddlSurveyQuestion").val();
        var SurveyAnswer = $("#txtSurveyAnswer").val();
        var SurveyMark = $("#txtSurveyMark").val();
        var IsPositive = $("#ddlIsPositive").val();
        var IsActive = $("#ddlIsActive").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/GetJqGridsurveyanswerList',
           postData: { SurveyQuestionId: SurveyQuestionId, SurveyAnswer: SurveyAnswer, SurveyMark: SurveyMark, IsPositive: IsPositive, IsActive: IsActive },
           page: 1
       }).trigger("reloadGrid");
    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function checkvalid(value, column) {
    if (value == 'nil') {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}
function SurveyQuestionddl() {
    var ddlcam = $("#ddlSurveyQuestion");
    $.ajax({
        type: 'POST',
        async: true,
        dataType: "json",
        url: "/StaffManagement/GetSurveyQuestionddl",
        success: function (data) {
            ddlcam.empty();
            ddlcam.append("<option value=''> Select </option>");
            for (var i = 0; i < data.length; i++) {
                ddlcam.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}