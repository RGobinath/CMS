$(function () {

    var grid_selector = "#TestsAssessmentsList";
    var pager_selector = "#TestsAssessmentsPage";

    $("#TASubject").change(function () {
        var cam = $("#hdnCampus").val();
        var gra = $("#Grade").val();
        var sub = $("#TASubject").val();
        var assgnname = $("#TAAssmntName").val();
        GetAssignmentNameByCampusGradeSubject('TAAssmntName', assgnname, cam, gra, encodeURIComponent(sub))
    });
    $("#TAAssmntName").change(function () {
        var cam = $("#hdnCampus").val();
        var gra = $("#Grade").val();
        var sub = $("#TASubject").val();
        var assgnname = $("#TAAssmntName").val();
        GetTermtestMarkWeightings(cam, gra, sub, assgnname);
    });
    //$("#btnStaffPopup").button({ icons: { primary: "ui-icon-search" },
    //    text: false
    //});
    $('#btnResetT3').hide();
    if ($('#loggedInUserType').val() == "Staff") {
        $('#Staff').val($('#loggedInUserName').val());
        $('#btnStaffPopup').hide();
    }

    $('#btnStaffPopup').click(function () {
        GetStaffPopup();
    });
    if ($("#hdnCampus").val() == "IB MAIN" || $('#hdnCampus').val() == "CHENNAI MAIN") {
        semhideshow("show");
    }
    else {
        semhideshow("hide");
    }
    $('#TAAssmntType').change(function () {
        $('#hdnIsCredit').val($('#TAAssmntType option:selected').attr('IsCredit'));
        if ($('#TAAssmntType option:selected').text() == "SLC Assessment") {
            $('#TASubject').attr('disabled', 'disabled');
            $('#btnStaffPopup').hide();
            $('#StaffId').val('');
            $('#Staff').val('');
            if ($("#hdnCampus").val() == "IB MAIN" || $('#hdnCampus').val() == "CHENNAI MAIN") {
                semhideshow("hide");
            }
            else {
                semhideshow("show");
            }
            $("#TAAssmntName").empty();
            $("#TAAssmntName").append("<option value=''> Select One </option>");
            for (var i = 1; i < 6; i++) {
                $("#TAAssmntName").append("<option value='SLC Asessment" + i + "'>SLC Asessment" + i + "</option>");
            }
        } else {
            $('#TASubject').removeAttr('disabled');
            $('#btnStaffPopup').show();
            if ($('#AdminRole').toString != "All") {
                $('#UserId').val($('#UserId').toString);
            }
            if ($("#hdnCampus").val() == "IB MAIN"||$("#hdnCampus").val() == "CHENNAI MAIN") {
                semhideshow("show");
                GetTermtestMarkWeightings();
            }
            else {
                semhideshow("hide");
            }
            //GetAssignmentName('TAAssmntName', null)
            var cam = $("#hdnCampus").val();
            var gra = $("#Grade").val();
            var sub = $("#TASubject").val();
            var assgnname = $("#TAAssmntName").val();
            GetTermtestMarkWeightings(cam, gra, sub, assgnname);
        }
    });

    $("#TADatofAssgn").datepicker({
        format: 'dd-M-yy',
        autoclose: true
    });

    //        $('.datepicker').datepicker('option', 'dateFormat', 'dd-M-yy');

    function frmtrUpdate(cellvalue, options, rowdata) {
        var saveBtn = "";
        if (rowdata[12] == $('#loggedInUserId').val()) {
            saveBtn = "<span id='T3btnUpdate_" + options.rowId + "'class='fa fa-pencil blue T3CompUpdate'  rowid='" + options.rowId + "' title='Update'></span>";
        }
        return saveBtn;
    }
    function frmtrDel(cellvalue, options, rowdata) {
        var delBtn = "";
        if (rowdata[12] == $('#loggedInUserId').val()) {
            delBtn = "<span id='T3btnDel_" + options.rowId + "'class='fa fa-trash-o red T3CompDel' rowid='" + options.rowId + "' title='Delete'></span>";
        }
        return delBtn;
    }



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

    function styleCheckbox(table) {
    }


    //unlike navButtons icons, action icons in rows seem to be hard-coded
    //you can change them like this in here if you want
    function updateActionIcons(table) {

    }
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
        mtype: 'GET',
        url: '/Assess360/GetAssess360ComponentListWithPagingAndCriteria',
        postData: { Assess360Id: $('#Id').val(), tab: 3 },
        datatype: 'json',
        height: '80',
        shrinkToFit: true,
        colNames: ['Id', 'Assess360Id', 'Type', 'IsCredit', 'Assessment Type', 'Subject', 'Staff', 'Date of Assessment', 'Mark Awarded', 'Assessment Name', 'Description', 'Created Date', 'Entered By', 'Semester', '', ''],
        colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'Assess360Id', index: 'Assess360Id', hidden: true },
                          { name: 'AssessCompGroup', index: 'AssessCompGroup', sortable: false, hidden: true },
                          { name: 'IsCredit', index: 'IsCredit', hidden: true },
                          { name: 'GroupName', index: 'GroupName', sortable: false },
                          { name: 'Subject', index: 'Subject', sortable: false },
                          { name: 'Staff', index: 'Staff', width: 80, sortable: false },
                          { name: 'IncidentDate', index: 'IncidentDate', width: 80, sortable: false },
                          { name: 'MarkDetails', index: 'MarkDetails', sortable: false },
                          { name: 'AssignmentName', index: 'AssignmentName', sortable: false },
                          { name: 'Description', index: 'Description', sortable: false, hidden: true },
                          { name: 'DateCreated', index: 'DateCreated', width: 80, sortable: false },
                          { name: 'EnteredBy', index: 'EnteredBy', sortable: false, hidden: true },
                          { name: 'Semester', index: 'Semester', sortable: false, hidden: true },
                          { name: 'Update', index: 'Update', width: 30, align: 'center', sortable: false, formatter: frmtrUpdate },
                          { name: 'Delete', index: 'Delete', width: 30, align: "center", sortable: false, formatter: frmtrDel }
                          ],
        pager: pager_selector,
        autowidth: true,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        viewrecords: true,
        sortname: 'Id',
        sortorder: 'Desc',
        caption: '<i class="fa fa-book"></i>&nbsp;Test / Assessments',
        loadComplete: function () {
            var rdata = $(grid_selector).getRowData();
            var table = this;
            if (rdata.length > 0) {
                $('.T3CompUpdate').click(function () { UpdateComponentDtlsT3($(this).attr('rowid')); });
                $('.T3CompDel').click(function () { DeleteComponentDtlsT3($(this).attr('rowid')); });

            }
            setTimeout(function () {

                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
});

$('#btnResetT3').click(function () {
    $('#Assess360ComponentId').val('');
    $('#A360CompCreatedDate').val('');
    $('#TestsAssessments').click();
});

function DeleteComponentDtlsT3(id) {
    if (confirm("Are you sure you want to delete this item?")) {
        // your deletion code
        DeleteComponentIds(
                '/Assess360/DeleteAssess360Component?Assess360ComponentIds=' + id, //delURL, 
                '/Assess360/GetAssess360ComponentListWithPagingAndCriteria?Assess360Id=' + $('#Id').val() + '&tab=3', //reloadURL, 
                $("#TestsAssessmentsList") //GridId, 
                );
    }
    return false;

}


function UpdateComponentDtlsT3(id) {
    $('#btnAddTA').val('Update');
    $('#btnResetT3').show();
    var rowData = $("#TestsAssessmentsList").getRowData(id);
    $('#Staff').val(rowData.Staff);

    var cmps = document.getElementById("TAAssmntType");

    for (i = 0; i < cmps.options.length; i++) {
        if (cmps.options[i].text == rowData.GroupName) {
            cmps.selectedIndex = i;
            $('#TAAssmntType').change();
        }
    }
    if ($("#hdnCampus").val() == "IB MAIN" || $('#hdnCampus').val() == "CHENNAI MAIN") {
        var semtype = document.getElementById("TASemester");
        for (i = 0; i < semtype.options.length; i++) {
            if (semtype.options[i].text == rowData.Semester) {
                semtype.selectedIndex = i;
            }
        }
    }
    var subj = document.getElementById("TASubject");
    for (i = 0; i < subj.options.length; i++) {
        if (subj.options[i].text == rowData.Subject) {
            subj.selectedIndex = i;
        }
    }

    $('#TAobtainedMarks').val(rowData.MarkDetails.substring(0, rowData.MarkDetails.lastIndexOf("/")));
    $('#TATotalMarks').val(rowData.MarkDetails.substring(rowData.MarkDetails.lastIndexOf("/") + 1, rowData.MarkDetails.length));

    $('#TADatofAssgn').val(rowData.IncidentDate);
    //$('#TAAssmntName').val(rowData.AssignmentName);
    /*
    This method added by Lee on 22-Jun-2013, 
    Earlier this is a free text, 
    now it has been turned to Master data.
    */
    GetAssignmentName('TAAssmntName', rowData.AssignmentName);
    $('#Assess360ComponentId').val(rowData.Id);
    $('#A360CompCreatedDate').val(rowData.DateCreated);
}
function GetTermtestMarkWeightings(cam, gra, sub, assmntname) {
    if (!isEmptyorNull(cam) && gra == "IX" && !isEmptyorNull(sub) && !isEmptyorNull(assmntname) && assmntname != 'Select' && $('#TAAssmntType option:selected').text() == "Term Assessment") {
        $.ajax({
            type: 'POST',
            async: false,
            dataType: "json",
            url: 'Assess360/GetMarkWeightingsbyCampusGrade?Campus=' + cam + '&Grade=' + gra + '&Subject=' + sub + '&AssignmntName=' + assmntname,
            success: function (data) {
                if (data != null) {
                    $("#TATotalMarks").val(data);
                    $('#TATotalMarks').attr('disabled', 'disabled');
                }
            }
        });
    }
    else { $("#TATotalMarks").val(''); $('#TATotalMarks').removeAttr('disabled'); }
}
$('#btnAddTA').click(function () {
    SaveOrUpdateAssess360Component('TestsAssessments');
});