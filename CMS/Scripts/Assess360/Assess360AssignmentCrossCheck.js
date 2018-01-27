$(function () {

    var grid_selector = "#Assess360AssignmentList";
    var pager_selector = "#Assess360AssignmentListPager";

    $.getJSON("/Base/FillBranchCode", function (fillcampus) {
        var ddlcam = $("#Campus");
        ddlcam.empty();
        ddlcam.append($('<option/>', { value: "", text: "Select One" }));

        $.each(fillcampus, function (index, itemdata) {
            ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        });
    });
    $("#Campus").change(function () {
        var cam = $(this).val();
        $.getJSON("/Base/FillStaffNameByUserRole?cam=" + cam,
 function (fillbc) {
     var ddlstaff = $("#Staff");
     ddlstaff.empty();
     ddlstaff.append($('<option/>', { value: "", text: "Select One" }));
     $.each(fillbc, function (index, itemdata) {
         ddlstaff.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
     });
 });
    });

    var cam = $("#Campus").val();
    var grade = $("#Grade").val();
    var section = $("#Section").val();
    var assmntType = $("#AssessGroup").val();
    var sub = $("#Subject").val();
    var Staff = $("#Staff").val();
    var sem = $("#Semester").val();
    GetSubjectsByGrade($("#Grade").val());
    InitAssess360AssignmentCrossCheck(cam, grade, section, assmntType, sub, Staff, sem);
    $("#Search").click(function () {
        $(grid_selector).clearGridData();
        cam = $("#Campus").val();
        grade = $("#Grade").val();
        section = $("#Section").val();
        Staff = $("#Staff").val();
        sub = $("#Subject").val();
        assmntType = $("#AssessGroup").val();
        sem = $("#Semester").val();
        //            if (cam == "" || cam == "Select") {
        //                ErrMsg("Please select Campus");
        //                return false;
        //            }
        //            if (grade == "" || grade == "Select") {
        //                ErrMsg("Please select Grade");
        //                return false;
        //            }

        //            if (section == "" || section == "Select") {
        //                ErrMsg("Please select Section");
        //                return false;
        //            }

        //            if (Staff == "" || Staff == "Select") {
        //                ErrMsg("Please select Staff");
        //                return false;
        //            }
        //            if (sub == "" || sub == "Select") {
        //                ErrMsg("Please select Subject");
        //                return false;
        //            }
        //            if (assmntType == "" || assmntType == "Select") {
        //                ErrMsg("Please select Assessment Type");
        //                return false;
        //            }

        $(grid_selector).GridUnload();
        InitAssess360AssignmentCrossCheck(cam, grade, section, assmntType, sub, Staff, sem);
        //            $("#Assess360AssignmentList").setGridParam(
        //                {
        //                    datatype: "json",
        //                    url: '@Url.Content("~/Assess360/Assess360AssignmentListJqGrid/")',
        //                    postData: { cam: cam, grade: grade, section: section, sub: sub, assmntType: assmntType, Staff: Staff },
        //                    page: 1
        //                }).trigger("reloadGrid");
    });

    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).jqGrid('clearGridData')
                     .jqGrid('setGridParam', { data: data, page: 1 })
                     .trigger('reloadGrid');
    });

    $("#Subject").click(function () {
        if ($(this).val() == "" && $("#Grade").val() == "") {
            ErrMsg("Please select Grade.");
            return false;
        }
    });
    $("#Grade").change(function () {
        GetSubjectsByGrade($(this).val());
    });

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


    function InitAssess360AssignmentCrossCheck(cam, grade, section, assmntType, sub, Staff, sem) {
        $(grid_selector).jqGrid({
            url: "/Assess360/Assess360AssignmentListJqGrid",
            postData: { cam: cam, grade: grade, section: section, sub: sub, assmntType: assmntType, Staff: Staff, Semester: sem },
            datatype: 'json',
            type: 'GET',
            colNames: ['Campus', 'Grade', 'Section', 'Semester', 'Subject', 'Assignment Name', 'Staff', 'Entered', 'Not Entered', 'Total No of Students'],
            colModel: [
                  { name: 'Campus', index: 'Campus', width: 80 },
                  { name: 'Grade', index: 'Grade', width: 40 },
                  { name: 'Section', index: 'Section', width: 40, sortable: true },
                  { name: 'Semester', index: 'Semester', width: 60 },
                  { name: 'Subject', index: 'Subject', width: 80 },
                  { name: 'AssignmentName', index: 'AssignmentName', width: 200 },
                  { name: 'Staff', index: 'Staff', width: 40, sortable: true },
                  { name: 'Entered', index: 'Entered', width: 40, sortable: true, title: false },
                  { name: 'NotEntered', index: 'NotEntered', width: 40, sortable: true, title: false },
                  { name: 'TotalStudents', index: 'TotalStudents', width: 40, sortable: true },
            ],
            pager: pager_selector,
            rowNum: '50',
            rowList: [5, 10, 20, 50, 100],
            sortname: 'Id',
            sortorder: 'Desc',
            autowidth: true,
            height: 250,
            viewrecords: true,
            caption: '<i class="fa fa-th-list">&nbsp;</i>Assignment List Report',
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            }
        });

        $(grid_selector).navGrid(pager_selector, {
            add: false, edit: false, del: false, search: false, refresh: true,
            refreshicon: 'ace-icon fa fa-refresh green'
        });

        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
            onClickButton: function () {
                window.open("Assess360AssignmentListJqGrid" + '?cam=' + cam + '&grade=' + grade + '&section=' + section + '&sub=' + sub + '&assmntType=' + assmntType + '&Staff=' + Staff + '&Semester=' + sem + '&rows=9999' + '&ExptXl=1');
            }
        });
    }
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

function GetSubjectsByGrade(grade) {

    var sub = $("#Subject");
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetSubjectsByGrade?Grade=' + grade,
        success: function (data) {
            sub.empty();
            sub.append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {

                sub.append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}


function ShowMarksEntered(Cam, Gra, Sect, AssCompGrp, Sub, AssignmentName, Sem) {
    modalid = $('#EnteredListDiv');
    $('#EnteredList').clearGridData();
    //        LoadPopupDynamicaly("/Assess360/MarksEntered", $('#EnteredListDiv'), function () {
    //            LoadSetGridParam($('#EnteredList'), "/Assess360/MarksEnteredListJqGrid?cam=" + Cam + '&gra=' + Gra + '&sect=' + Sect + '&sub=' + encodeURIComponent(Sub) + '&AssCompGrp=' + AssCompGrp + '&AssignmentName=' + encodeURIComponent(AssignmentName));
    //        });
    ModifiedLoadPopupDynamicaly("/Assess360/MarksEntered", $('#EnteredListDiv'), function () {
        LoadSetGridParam($('#EnteredList'), "/Assess360/MarksEnteredListJqGrid?cam=" + Cam + '&gra=' + Gra + '&sect=' + Sect + '&sem=' + Sem + '&sub=' + encodeURIComponent(Sub) + '&AssCompGrp=' + AssCompGrp + '&AssignmentName=' + encodeURIComponent(AssignmentName));
    }, function () { }, 818, 487, "Entered List Report");
}

function ShowMarksNotEntered(Cam, Gra, Sect, AssCompGrp, Sub, AssignmentName, Sem) {

    modalid = $('#NotEnteredListDiv');
    $('#NotEnteredList').clearGridData();
    //        LoadPopupDynamicaly("/Assess360/NotEntered", $('#NotEnteredListDiv'), function () {
    //            LoadSetGridParam($('#NotEnteredList'), "/Assess360/NotEnteredListJQGrid?cam=" + Cam + '&gra=' + Gra + '&sect=' + Sect + '&sub=' + encodeURIComponent(Sub) + '&AssCompGrp=' + AssCompGrp + '&AssignmentName=' + encodeURIComponent(AssignmentName));
    //        });

    ModifiedLoadPopupDynamicaly("/Assess360/NotEntered", $('#NotEnteredListDiv'), function () {
        LoadSetGridParam($('#NotEnteredList'), "/Assess360/NotEnteredListJQGrid?cam=" + Cam + '&gra=' + Gra + '&sect=' + Sect + '&sem=' + Sem + '&sub=' + encodeURIComponent(Sub) + '&AssCompGrp=' + AssCompGrp + '&AssignmentName=' + encodeURIComponent(AssignmentName));
    }, function () { }, 830, 418, "Not Entered List Report");
}
function LoadSetGridParam(GridId, brUrl) {
    GridId.setGridParam({
        url: brUrl,
        datatype: 'json',
        mtype: 'GET',
        page: 1
    }).trigger("reloadGrid");
}
var clbPupGrdSel = null;
function LoadPopupDynamicaly(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
    if (width == undefined) { width = 400; }
    if (ModalId.html().length == 0) {
        $.ajax({
            url: dynURL,
            type: 'GET',
            async: false,
            dataType: 'html', // <-- to expect an html response
            success: function (data) {
                ModalId.dialog({
                    height: 'auto',
                    width: width,
                    modal: true,
                    //  title: 'Marks Entered',
                    buttons: {}
                });
                ModalId.html(data);
            }
        });
    }
    clbPupGrdSel = onSelcalbck;
    ModalId.dialog('open');
    CallBackFunction(loadCalBack);
}