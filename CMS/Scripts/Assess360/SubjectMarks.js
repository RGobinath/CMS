$(function () {

    $('#reset').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });


    var lastsel2 = "";

    $('#idFinalResult').click(function () {
        var PreRegNum = "";
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "") { ErrMsg("Please fill the Section"); return false; }
        if ($('#Semester').val() == "") { ErrMsg("Please fill the Semester"); return false; }
        if ($('#AcademicYear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Assess360/SemesterResultVaild?&PreRegNum=' + PreRegNum + '&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val(),
            success: function (data) {
                if (data == "Success") {
                    return window.location.href = '/Assess360/FinalResultZipPdfGen?&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val();
                }
                else {
                    ErrMsg("Semester marks are not available");
                    return false;
                }
            }
        });
        // return window.location.href = '/Assess360/FinalResultZipPdfGen?&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val();
    });
    $('#overAllZipFile').click(function () {
        var PreRegNum = "";
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "") { ErrMsg("Please fill the Section"); return false; }
        if ($('#Semester').val() == "") { ErrMsg("Please fill the Semester"); return false; }
        if ($('#AcademicYear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }

        $.ajax({
            type: 'POST',
            async: false,
            url: '/Assess360/OverAllGradeMarksValidation?&PreRegNum=' + PreRegNum + '&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val(),
            success: function (data) {
                if (data == "Success") {
                    return window.location.href = '/Assess360/OverAllGradeZipPdfGen?&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val();
                }
                else {
                    ErrMsg("Subject marks not available");
                    return false;
                }
            }
        });
        //return window.location.href = '/Assess360/OverAllGradeZipPdfGen?&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val();
    });


    var grid_selector = "#SubjectGrid";
    var pager_selector = "#SubjectPager";
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

        url: "/Assess360/GetSubjectMarksJqGrid/",
        datatype: 'json',
        type: 'POST',
        colNames: ['', 'PreRegNum', 'Student Name', 'Test I (10)', 'Test II (10)', 'Test III (10)', 'Project (20)', 'Total (50)', 'Total (50)', 'OutOf (100)', 'FA + SA', 'Grade', '', '', '', '', '', 'Academic Year', 'Final Result', 'OverAllResult', ''],
        colModel: [
            { name: 'MarksId', index: 'MarksId', editable: true, hidden: true },
            { name: 'PreRegNum', index: 'PreRegNum', editable: true, hidden: true, key: true },
            { name: 'Name', index: 'Name', width: 40, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
            { name: 'Test1', index: 'Test1', align: 'center', width: 15, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid }, sortable: false },
            { name: 'Test2', index: 'Test2', align: 'center', width: 15, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid }, sortable: false },
            { name: 'Test3', index: 'Test3', align: 'center', width: 15, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid }, sortable: false },
            { name: 'Project', index: 'Project', align: 'center', width: 15, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid1 }, sortable: false },
            { name: 'FATotal', index: 'FATotal', align: 'center', width: 15, sortable: false },
            { name: 'SATotal', index: 'SATotal', align: 'center', width: 15, editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid2 }, sortable: false },
            { name: 'Outof', index: 'Outof', align: 'center', width: 15, sortable: false },
            { name: 'Fa1andFa2', index: 'Fa1andFa2', align: 'center', width: 15, sortable: false },
            { name: 'SEMGrade', index: 'SEMGrade', align: 'center', width: 15, sortable: false },
            { name: 'Subject', index: 'Subject', editable: true, hidden: true, sortable: false },
            { name: 'Semester', index: 'Semester', editable: true, hidden: true, sortable: false },
            { name: 'Campus', index: 'Campus', align: 'center', width: 20, editable: true, hidden: true, sortable: false },
            { name: 'Grade', index: 'Grade', align: 'center', width: 10, editable: true, hidden: true, sortable: false },
            { name: 'Section', index: 'Section', align: 'center', width: 10, editable: true, hidden: true, sortable: false },
            { name: 'AcademicYear', index: 'AcademicYear', width: 60, editable: true, hidden: true, sortable: false },
            { name: 'History', index: 'History', width: 10, align: 'center', sortable: false, sortable: false },
            { name: 'History', index: 'History', width: 10, align: 'center', sortable: false, sortable: false },
            { name: 'CreatedBy', index: 'CreatedBy', width: 10, align: 'center', hidden: false },

        ],
        onSelectRow: function (id) {
            var userid = jQuery(grid_selector).jqGrid('getCell', id, 'CreatedBy');
            if (id && id !== lastsel2) {
                jQuery(grid_selector).restoreRow(lastsel2);
                // jQuery('#SubjectGrid').editRow(id, true);
                if (('@Session["UserId"].ToString()' == userid)) {

                    jQuery(grid_selector).jqGrid("editRow", id, true, '', '', '', '', function reload(rowid, result) {
                        $(grid_selector).trigger("reloadGrid");
                    });
                }
                lastsel2 = id;
            }
        },
        editurl: "/Assess360/GetSubjectEditRowList",
        rowNum: 50,
        rowList: [10, 50, 100],
        autowidth: true,
        height: 195,
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
        viewrecords: true,
        caption: 'Subject Marks List',
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    jQuery(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green', search: false });
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "&nbsp;&nbsp;<i class='fa fa-file-excel-o'>&nbsp;</i><u>Export To Excel</u>",
        onClickButton: function () {
            window.open("GetSubjectMarksJqGrid" + '?rows=9999' +
                         '&page=1' +
                         '&sidx=Id' +
                         '&sord=Asc' +
                         '&campus=' + $('#Campus').val() +
                         '&grade=' + $('#Grade').val() +
                         '&section=' + $('#Section').val() +
                         '&subject=' + $('#Subject').val() +
                         '&semester=' + $('#Semester').val() +
                         '&academicyear=' + $('#AcademicYear').val() +
                         '&ExportType=Excel'
                         );
        }
    });
    jQuery(grid_selector).jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
         { startColumnName: 'Test1', numberOfColumns: 5, titleText: '<em>Formative Assessment (FA)</em>' },
         { startColumnName: 'SATotal', numberOfColumns: 2, titleText: '<em>Summative Assessment (SA)</em>' },
         { startColumnName: 'Fa1andFa2', numberOfColumns: 2, titleText: '<em>SEM</em>' },
        ]
    });


    function checkvalid(value, column) {
        if (value == "") {
            return [true];
        }
        if (!$.isNumeric(value)) {
            //  ErrMsg(column + 'Should be numeric');
            return [false, column + 'Should be numeric'];
        }
        else if (parseInt(value) > parseInt(10)) {
            //  ErrMsg(column + 'Should be numeric');
            return [false, column + ' Mark Should not be greater than 10'];
        }
        else {
            return [true];
        }
    }

    function checkvalid1(value, column) {
        if (value == "") { return [true]; }
        if (!$.isNumeric(value)) { return [false, column + 'Should be numeric']; }
        else if (parseInt(value) > parseInt(20)) { return [false, column + ' Mark Should not be greater than 20']; }
        else { return [true]; }
    }
    function checkvalid2(value, column) {
        if (value == "") { return [true]; }
        if (!$.isNumeric(value)) { return [false, column + 'Should be numeric']; }
        else if (parseInt(value) > parseInt(50)) { return [false, column + ' Mark Should not be greater than 50']; }
        else { return [true]; }
    }

    $("#search").click(function () {
        var campus = $('#Campus').val();
        var grade = $('#Grade').val();
        var section = $('#Section').val();
        var subject = $('#Subject').val();
        var semester = $('#Semester').val();

        if (campus == "") {
            ErrMsg("Please fill the Campus");
            return false;
        }
        if (grade == "") {
            ErrMsg("Please fill the Grade");
            return false;
        }
        if (section == "") {
            ErrMsg("Please fill the Section");
            return false;
        }
        if (subject == "") {
            ErrMsg("Please fill the Subject");
            return false;
        }
        if (semester == "") {
            ErrMsg("Please fill the Semester");
            return false;
        }
        if ($('#AcademicYear').val() == "") {
            ErrMsg("Please fill the Academic Year");
            return false;
        }
        $(grid_selector).setGridParam(
             {
                 datatype: "json",
                 url: "/Assess360/GetSubjectMarksJqGrid",
                 type: 'POST',
                 postData: { campus: campus, grade: grade, section: section, subject: subject, semester: semester, academicyear: $('#AcademicYear').val() },
                 page: 1
             }).trigger("reloadGrid");

    });

    //    $("#reset").click(function () {
    //        window.location.href = '@Url.Action("SubjectMarks", "Assess360")';
    //    });




    // fill the branch code or campus from the base controller....

    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#Campus");
        ddlcam.empty();
        ddlcam.append($('<option/>',
    {
        value: "",
        text: "Select One"

    }));

        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>',
    {
        value: itemdata.Value,
        text: itemdata.Text
    }));
        });
    });

    $("#Campus").change(function () {
        gradeddl();
    });




});




function gradeddl() {

    var campus = $("#Campus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                    function (modelData) {
                        var select = $("#Grade");
                        select.empty();
                        select.append($('<option/>'
            , {
                value: "",
                text: "Select Grade"
            }));
                        $.each(modelData, function (index, itemData) {

                            select.append($('<option/>',
            {
                value: itemData.gradcod,
                text: itemData.gradcod
            }));
                        });
                    });
}



function ShowComments(PreRegNum, campus, grade, section) {
    $.ajax({
        type: 'POST',
        async: false,
        url: '/Assess360/SemesterResultVaild?&PreRegNum=' + PreRegNum + '&campus=' + campus + '&grade=' + grade + '&section=' + section + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val(),
        success: function (data) {
            if (data == "Success") {
                return window.location.href = '/Assess360/FinalResultPDF?&PreRegNum=' + PreRegNum + '&campus=' + campus + '&grade=' + grade + '&section=' + section + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val();
            }
            else {
                ErrMsg("Semester marks are not available");
                return false;
            }
        }
    });
}



function ShowComments1(PreRegNum, campus, grade, section) {
    $.ajax({
        type: 'POST',
        async: false,
        url: '/Assess360/OverAllGradeMarksValidation?&PreRegNum=' + PreRegNum + '&campus=' + campus + '&grade=' + grade + '&section=' + section + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val(),
        success: function (data) {
            if (data == "Success") {
                return window.location.href = '/Assess360/OverAllGrade?&PreRegNum=' + PreRegNum + '&campus=' + campus + '&grade=' + grade + '&section=' + section + '&semester=' + $('#Semester').val() + '&academicyear=' + $('#AcademicYear').val();
            }
            else {
                ErrMsg("Subject marks not available");
                return false;
            }
        }
    });

}



var clbPupGrdSel = null;
function LoadPopupDynamicaly(dynURL, ModalId, loadCalBack, onSelcalbck, width) {


    if (width == undefined) { width = 1110; }
    //  if (ModalId.html().length == 0) {
    $.ajax({
        url: dynURL,
        type: 'GET',
        async: false,
        dataType: 'html', // <-- to expect an html response
        success: function (data) {
            ModalId.dialog({
                height: 340,
                width: width,
                modal: true,
                title: 'FinalResult Marks List'
                //  buttons: {}
            });
            ModalId.html(data);
        }
    });
    // }
    clbPupGrdSel = onSelcalbck;
    ModalId.dialog('open');
    CallBackFunction(loadCalBack);
}
function LoadSetGridParam(GridId, brUrl) {

    GridId.setGridParam({
        url: brUrl,
        datatype: 'json',
        mtype: 'GET'
    }).trigger("reloadGrid");
}