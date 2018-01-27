$(function () {
    $('#btnReset').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });
    var campus = ""; var grade = ""; var section = ""; var month = ""; var model = "";
    var grid_selector = "#Grid";
    var pager_selector = "#Pager";
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
        //url: "/Assess360/SummativeAssessmentJqGridNew",
        datatype: 'json',
        type: 'GET',
        autowidth: true,
        height: 270,
        colNames: [],
        colModel: [],
        rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Total',
        sortroder: 'Desc',
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>Summative Test Report'
    });

    $("#btnSearch").click(function () {
        campus = $('#Campus').val();
        grade = $('#Grade').val();
        section = $('#Section').val();
        //month = $('#modelExam option:selected').attr('Month');
        model = $('#modelExam option:selected').val();
        if (!isEmptyorNull(campus) && !isEmptyorNull(grade) && !isEmptyorNull(section) && !isEmptyorNull(model)) {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Assess360/CheckSummativeAssessmentReport?Campus=' + campus + '&Grade=' + grade + '&Section=' + section,
                success: function (data) {
                    if (data == "Success") {
                        $('#IsCombinedSci').val(true);
                        $(grid_selector).GridUnload();
                        window.onload = loadgrid(campus, grade, section, model, true);
                    }
                    else {
                        $('#IsCombinedSci').val(false);
                        $(grid_selector).GridUnload();
                        window.onload = loadgrid(campus, grade, section, model, false);
                    }
                }
            });

        }
        else {
            ErrMsg('Please enter all the values.');
            return false;
        }
    });

    
    function loadgrid(campus, grade, section, model, isCombinedSci) {
        if ((grade == 'IX' || grade == 'X') && isCombinedSci == true) {
            colnames = "['RptId', 'Student Id', 'Name', 'Campus', 'Grade', 'Section', 'AcademicYear','Semester', 'English','Language','Combined Science', 'Mathematics','Economics', 'Physical Education', 'ICT', 'Total',  'Average','Grade', 'Pdf']";
            colmodel = "[{ name: 'RptId', index: 'RptId', editable: true, hidden: true, key: true },{ name: 'NewId', index: 'NewId', align: 'center', sortable: false },{ name: 'Name', index: 'Name' },{ name: 'Campus', index: 'Campus', align: 'center', sortable: false },{ name: 'Grade', index: 'Grade', align: 'center', sortable: false },{ name: 'Section', index: 'Section', align: 'center', sortable: false },{ name: 'AcademicYear', index: 'AcademicYear', align: 'center', sortable: false },{ name: 'Semester', index: 'Semester', align: 'center', sortable: false, hidden:true },{ name: 'English', index: 'English', align: 'center',sortable: false },{ name: 'Language', index: 'Language', align: 'center', sortable: false },{ name: 'Combined Science', index: 'Combined Science', align: 'center', sortable: false },{ name: 'Mathematics', index: 'Mathematics', align: 'center', sortable: false },{ name: 'Economics', index: 'Economics', align: 'center', sortable: false },{ name: 'PhysicalEducation', index: 'PhysicalEducation', align: 'center', sortable: false },{ name: 'ICT', index: 'ICT', align: 'center', sortable: false },{ name: 'Total', index: 'Total', align: 'center', sortable: false },{ name: 'Average', index: 'Average', align: 'center', sortable: false },{ name: 'TestGrade', index: 'TestGrade', align: 'center', sortable: false },{ name: 'GetPdf', index: 'GetPdf', align: 'center' }]";
        }
        else if ((grade == 'IX' || grade == 'X') && isCombinedSci != true) {
            colnames = "['RptId', 'Student Id', 'Name', 'Campus', 'Grade', 'Section', 'AcademicYear','Semester', 'English','Language', 'Mathematics', 'Chemistry', 'Physics', 'Biology','Economics', 'ICT', 'Total',  'Average', 'Grade','Pdf']";
            colmodel = "[{ name: 'RptId', index: 'RptId', editable: true, hidden: true, key: true },{ name: 'NewId', index: 'NewId', align: 'center', sortable: false },{ name: 'Name', index: 'Name' },{ name: 'Campus', index: 'Campus', align: 'center', sortable: false },{ name: 'Grade', index: 'Grade', align: 'center', sortable: false },{ name: 'Section', index: 'Section', align: 'center', sortable: false },{ name: 'AcademicYear', index: 'AcademicYear', align: 'center', sortable: false },{ name: 'Semester', index: 'Semester', align: 'center', sortable: false, hidden:true },{ name: 'English', index: 'English', align: 'center', sortable: false },{ name: 'Language', index: 'Language', align: 'center', sortable: false },{ name: 'Mathematics', index: 'Mathematics', align: 'center', sortable: false },{ name: 'Chemistry', index: 'Chemistry', align: 'center', sortable: false },{ name: 'Physics', index: 'Physics', align: 'center',sortable: false },{ name: 'Biology', index: 'Biology', align: 'center', sortable: false },{ name: 'Economics', index: 'Economics', align: 'center', sortable: false },{ name: 'ICT', index: 'ICT', align: 'center', sortable: false },{ name: 'Total', index: 'Total', align: 'center',sortable: false },{ name: 'Average', index: 'Average', align: 'center', sortable: false },{ name: 'TestGrade', index: 'TestGrade', align: 'center', sortable: false },{ name: 'GetPdf', index: 'GetPdf', align: 'center' }]";
        }
        else {
            colnames = "['RptId', 'Student Id', 'Name', 'Campus', 'Grade', 'Section', 'AcademicYear','Semester', 'English','Language', 'Mathematics', 'Chemistry', 'Physics', 'Biology','HistoryGeography', 'ICT', 'Total',  'Average', 'Grade']";
            colmodel = "[{ name: 'RptId', index: 'RptId', editable: true, hidden: true, key: true },{ name: 'NewId', index: 'NewId', align: 'center', sortable: false },{ name: 'Name', index: 'Name' },{ name: 'Campus', index: 'Campus', align: 'center', sortable: false },{ name: 'Grade', index: 'Grade', align: 'center', sortable: false },{ name: 'Section', index: 'Section', align: 'center', sortable: false },{ name: 'AcademicYear', index: 'AcademicYear', align: 'center', sortable: false },{ name: 'Semester', index: 'Semester', align: 'center', sortable: false, hidden:true },{ name: 'English', index: 'English', align: 'center', sortable: false },{ name: 'Language', index: 'Language', align: 'center', sortable: false },{ name: 'Mathematics', index: 'Mathematics', align: 'center', sortable: false },{ name: 'Chemistry', index: 'Chemistry', align: 'center', sortable: false },{ name: 'Physics', index: 'Physics', align: 'center',sortable: false },{ name: 'Biology', index: 'Biology', align: 'center', sortable: false },{ name: 'HistoryGeography', index: 'HistoryGeography', align: 'center', sortable: false },{ name: 'ICT', index: 'ICT', align: 'center', sortable: false },{ name: 'Total', index: 'Total', align: 'center',sortable: false },{ name: 'Average', index: 'Average', align: 'center', sortable: false },{ name: 'TestGrade', index: 'TestGrade', align: 'center', sortable: false }]";
        }
        columnName = eval(colnames);
        columnModel = eval(colmodel);

        $(grid_selector).jqGrid({
            url: "/Assess360/SummativeAssessmentJqGridNew",
            postData: { Campus: campus, Grade: grade, Section: section, model: model, IsCombinedSci: isCombinedSci },
            datatype: 'json',
            type: 'GET',
            autowidth: true,
            height: 250,
            colNames: columnName,
            colModel: columnModel,
            rowNum: 50,
            shrinkToFit: false,
            rowList: [50, 100, 150, 200],
            viewrecords: true,
            sortname: 'Total',
            sortorder: 'Desc',
            pager: 'Pager',
            caption: '<i class="fa fa-th-list">&nbsp;</i>Summative Test Report',
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
                window.open("SummativeAssessmentJqGridNew" + '?Campus=' + $('#Campus').val() + '&Grade=' + $('#Grade').val() + '&Section=' + $('#Section').val() + '&model=' + model + '&IsCombinedSci=' + $('#IsCombinedSci').val() + '&rows=9999&sidx=Total&sord=Desc' + '&ExptXl=1');
            }
        });
    }
    //replace icons with FontAwesome icons like above
    $("#Campus").change(function () {
        getSemdll();
    });
    $("#Grade").change(function () {
        getSemdll();
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

function getSemdll() {
    if (Campus != "" && Grade != "") {
        var Campus = $("#Campus").val();
        var Grade = $("#Grade").val();
        $.getJSON("/Assess360/GetSemester?Campus=" + Campus + "&Grade=" + Grade,
              function (fillig) {
                  $("#modelExam").empty();
                  $("#modelExam").append($('<option/>', { value: "", text: "Select One" }));
                  $("#modelExam").append($('<option/>', { value: "Model I", Month: "6", text: "Model I" }));
                  $("#modelExam").append($('<option/>', { value: "Model II", Month: "6", text: "Model II" }));
                  $("#modelExam").append($('<option/>', { value: "Model III", Month: "6", text: "Model III" }));
                  $("#modelExam").append($('<option/>', { value: "Model IV", Month: "6", text: "Model IV" }));
                  $.each(fillig, function (index, itemdata) {
                      $("#modelExam").append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  });
              });
    }
}

/*enter key press event*/
function defaultFunc(e) {
    if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
        $('#btnSearch').click();
        return false;
    }
    return true;
};
function GenerateSAPdf(idno, AcademicYear, model) {
    $.ajax({
        type: 'POST',
        async: false,
        url: '/Assess360/SummativeTestValidation?IdNo=' + idno + '&AcYear=' + AcademicYear + '&model=' + model,
        success: function (data) {
            if (data == "Success") {
                window.open('/Assess360/GenerateSAPDF?IdNo=' + idno + '&AcYear=' + AcademicYear + '&model=' + model + '&IsCombinedSci=' + $('#IsCombinedSci').val());
            }
            else {
                ErrMsg("Marks are not available");
                return false;
            }
        }
    });
}