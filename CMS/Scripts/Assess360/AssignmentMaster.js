$(function () {
    $('#BackToBulkEntry').click(function () {
        var url = $("#BackTo").val();
        window.location.href = url;
    });
   // GetSubjectsByGrade($("#ddlGrade").val());

    var grid_selector = "#AssignmentNameList";
    var pager_selector = "#AssignmentNameListPager";
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
    //pager icon
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $(grid_selector).jqGrid({
        url: "/Assess360/AssignmentNameJqGrid",
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Academic Year', 'AssignmentType', 'Subject', 'AssignmentName'],
        colModel: [
          { name: 'Id', index: 'Id', hidden: true, key: true },
          { name: 'Campus', index: 'Campus', width: 80, editable: true, edittype: 'select', editoptions: { dataUrl: '/Home/BranchCodeddl' }, editrules: { custom: true, custom_func: checkvalid } },
          { name: 'Grade', index: 'Grade', width: 40, editable: true, edittype: 'select', editoptions: { dataUrl: '/Admission/Gradeddl' }, editrules: { custom: true, custom_func: checkvalid } },
          { name: 'AcademicYear', index: 'AcademicYear', width: 80, editable: true, edittype: 'select', editoptions: { dataUrl: '/Assess360/AcademicYearddl' }, editrules: { custom: true, custom_func: checkvalid } },
           {
               name: 'AssignmentType', index: 'AssignmentType', width: 80, editable: true, edittype: 'select', editoptions: {
                   dataUrl: '/Assess360/GetAssess360CompAssessmentMasterListByName?tab=' + "23" + '&IssueCredits=' + "true"
               }, editrules: { custom: true, custom_func: checkvalid }
           },
           {
               name: 'Subject', index: 'Subject', width: 80, editable: true, edittype: 'select', editoptions: {
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
          { name: 'AssignmentName', index: 'AssignmentName', width: 200, editable: true, edittype: 'text', editrules: { required: true } },
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
     { width: 'auto', url: '/Assess360/AddAssignmentName?test=edit', left: '10%', top: '10%', height: '50%', width: 400 },
     { width: 'auto', url: '/Assess360/AddAssignmentName', left: '10%', top: '10%', height: '50%', width: 400 }
    //{ width: 'auto', url: '/Assess360/AddAssignmentName?test=del', left: '10%', top: '10%', height: '50%', width: 400, beforeShowForm: function (params) { var gsr = $("#AssignmentNameList").getGridParam('selrow'); var sdata = $("#AssignmentNameList").getRowData(gsr); return { Id: sdata.Id} } }
     );

    $("#ddlCampus").change(function () {
        gradeddl($("#ddlCampus").val());
    });

    $.getJSON("/Base/FillBranchCode",
         function (fillig) {
             var srchddlcam = $("#ddlCampus");
             var ddlcam = $("#Campusddl");
             ddlcam.empty();
             srchddlcam.empty();
             ddlcam.append($('<option/>',{value: "",text: "Select One"}));
             srchddlcam.append($('<option/>', { value: "", text: "Select One" }));
             $.each(fillig, function (index, itemdata) {
                 ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                 srchddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
             });
         });

    $("#Search").click(function () {

        $(grid_selector).clearGridData();
        var cam = $("#ddlCampus").val();
        var gra = $("#ddlGrade").val();
        var sub = $("#AssSubject").val();

        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: "/Assess360/AssignmentNameJqGrid/",
                postData: { cam: cam, gra: gra, sub: sub },
                page: 1
            }).trigger("reloadGrid");
    });

    $("#ddlGrade").change(function () {
        GetSubjectsByGrade($(this).val());
    });


    // ------------------------added by anto------------
    $("#Campusddl").change(function () {
        gradeddl($("#Campusddl").val());
    });
    $("#Gradeddl").change(function () {
        GetSubjectsByGrade($(this).val());
    });

    $("#resetAdd").click(function () {
        $("#Campusddl").val('');
        $("#Gradeddl").val('');
        $("#AssSubject").val('');
        $("#ddlyear").val('');
        $("#AssignmentName").val('');
    });
});



function gradeddl(campus) {
    //var campus = $("#ddlCampus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
        function (modelData) {
            var grd = $("#Gradeddl");
            var select = $("#ddlGrade");
            grd.empty();
            select.empty();
            select.append($('<option/>', { value: "", text: "Select Grade" }));
            grd.append($('<option/>', { value: "", text: "Select Grade" }));
            $.each(modelData, function (index, itemData) {
                grd.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
            });
        });
}



function checkvalid(value, column) {

    if (value == 'nil') {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}



function GetSubjectsByGrade(grade) {
    var srchsubddl = $("#Subject");
    var subddl = $("#AssSubject");
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetSubjectsByGrade?Grade=' + grade,
        success: function (data) {
            srchsubddl.empty();
            subddl.empty();
            srchsubddl.append("<option value=''> Select </option>");
            subddl.append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                srchsubddl.append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                subddl.append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}

function AddAssignmentName() {
    var cam = $("#Campusddl").val();
    var gra = $("#Gradeddl").val();
    var sub = $("#AssSubject").val();
    var Acayr = $("#ddlyear").val();
    var Assnme = $("#AssignmentName").val();
    var AssnTyp = $("#ddlAssTyp").val();
    if (cam == '' || gra == '' || sub == '' || Assnme == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/Assess360/AddAssignmentName?Campus=" + cam + '&Grade=' + gra + '&Subject=' + sub + '&Academicyear=' + Acayr + '&AssignmentName=' + Assnme + '&AssignmentType=' + AssnTyp,
        success: function (data) {
            $("#AssignmentNameList").trigger('reloadGrid');
            $("#AssignmentName").val('');
        }
    });
}


