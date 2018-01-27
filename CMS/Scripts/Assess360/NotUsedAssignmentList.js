$(function () {
    var grid_selector = "#NotUsedAssignmentList";
    var pager_selector = "#NotUsedAssignmentListPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    });
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
    $(grid_selector).jqGrid({
        url: "/Assess360/NotUsedAssignmentListJqGrid",
        datatype: 'Json',
        type: 'GET',
        autowidth: true,
        height: 230,
        colNames: ['Id', 'Campus', 'Grade', 'Subject', 'Assignment Name'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'Campus', index: 'Campus' },
              { name: 'Grade', index: 'Grade' },
              { name: 'Subject', index: 'Subject' },
              { name: 'AssignmentName', index: 'AssignmentName' }
              ],
        rowNum: 10,
        rowList: [10, 50, 100],
        sortname: 'Id',
        sortorder: 'Asc',
        viewrecords: true,
        pager: pager_selector,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Not Used Assignments',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });
    //navButtons Add, edit, delete
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
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
            {}, {},
            {})
    function updatePagerIcons(table) {
        var replacement = {
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
    //Export Excel
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, { caption: "Export Excel", buttonicon: "ui-icon-print",
        onClickButton: function () {
            var ExportType = "Excel";
            window.open("/Assess360/NotUsedAssignmentListJqGrid" + '?ExportType=' + ExportType +
                '&Campus=' + $("#gs_Campus").val() +
                '&Grade=' + $("#gs_Grade").val() +
                '&Subject=' + $("#gs_Subject").val() +
                '&AssignmentName=' + $("#gs_AssignmentName").val() +
                '&rows=9999');
        }
    });
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(grid_selector).clearGridData();
        return false;
    }
    });
    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#ddlCampus");
        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        });
        $('#ddlCampus').multiselect('rebuild');
    });

    $("#ddlCampus").change(function () {
        GetGrade();
    });
    $('#ddlCampus').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 2,
        includeSelectAllDivider: true
    });
    $('#ddlGrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 2,
        includeSelectAllDivider: true
    });

    $("#ddlGrade").change(function () {
        GetSubjectsByGrade('ddlSubject');
    });

    $("#ddlGrade").click(function () {
        var SDate = $("#ddlCampus").val();
        if (SDate == "") {
            ErrMsg("Please select Campus");
            return false;
        }
    });
    $("#ddlSubject").change(function () {
        GetAssignmentNameByCampusGradeSubject();
    });
    $("#btnSearch").click(function () {
        debugger;
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlGrade").val();
        jQuery(grid_selector).clearGridData();
        if (Campus != "" && Campus != null); {
            Campus = Campus.toString();
        }
        if (Grade != "" && Grade != null) {
            Grade = Grade.toString();
        }

        var Subject = $("#ddlSubject").val();
        var AssignmentName = $("#ddlAssmntName").val();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: "/Assess360/NotUsedAssignmentListJqGrid",
                        postData: { Campus: Campus, Grade: Grade, Subject: Subject, AssignmentName: AssignmentName },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        debugger;
        //    $("#ddlSrchCampus").multiselect('destroy');
        //    $("#ddlSrchCampus").multiselect('rebuild');
        //    $("#ddlSrchCampus").multiselect('None selected');
        //$('#ddlSrchCampus').dropdownchecklist('destroy');
        var Campus = $("#ddlCampus").val("");
        var Grade = $("#ddlGrade").val("");
        $('#ddlCampus').multiselect('rebuild');
        $('#ddlGrade').multiselect('rebuild');
        var Subject = $("#ddlSubject").val("");
        var AssignmentName = $("#ddlAssmntName").val("");
        $(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: "/Assess360/NotUsedAssignmentListJqGrid",
                            postData: { Campus: "", Grade: "", Subject: "", AssignmentName: "" },
                            page: 1
                        }).trigger("reloadGrid");
    });

});
function GetGrade() {
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Communication/FillGradesWithArrayCriteria?campus=' + $("#ddlCampus").val(),
        success: function (data) {
            $("#ddlGrade").empty();
            if (data != null && data != "") {
                for (var k = 0; k < data.length; k++) {
                    $("#ddlGrade").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
                }
            }
            $('#ddlGrade').multiselect('rebuild');
        }
    });
}
function GetSubjectsByGrade(drpdwnId) {
    debugger;
    var campus = $('#ddlCampus').val();
    var grade = $('#ddlGrade').val();

    //if (!isEmptyorNull(campus) && !isEmptyorNull(grade)) {
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetSubjectsByGrade?Campus=' + campus + '&Grade=' + grade,
        success: function (data) {
            $('#' + drpdwnId).empty();
            $('#' + drpdwnId).append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg("Could not get a value");
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
    //}
}

function GetAssignmentNameByCampusGradeSubject() {
    debugger;
    var cam = $("#ddlCampus").val();
    var gra = $("#ddlGrade").val();
    var sub = $("#ddlSubject").val();
    $.ajax({
        type: 'POST',
        async: false,
        url: "/Assess360/GetAssignmentNameByCampusGradeSubject?cam=" + cam + '&gra=' + gra + '&sub=' + encodeURIComponent(sub),
        success: function (data) {

            $("#ddlAssmntName").empty();
            $("#ddlAssmntName").append("<option value=''> Select One </option>");

            for (var i = 0; i < data.rows.length; i++) {
                $("#ddlAssmntName").append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
            }
        },
        dataType: "json"
    });
}

