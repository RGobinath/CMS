$(function () {
    var grid_selector = "#CharacterBehaviourList";
    var pager_selector = "#CharacterBehaviourPage";

    var lastsel3;
    //$("#btnStaffPopup").button({ icons: { primary: "ui-icon-search" },
    //    text: false
    //});

    $('#AssessmentNtv').attr("tabindex", 2);
    $('#AssessmentPtv').attr("tabindex", 3);

    $('#OtherMarks').hide();

    if ($('#loggedInUserType').val() == "Staff") {
        $('#Staff').val($('#loggedInUserName').val());
        $('#btnStaffPopup').hide();
    }

    $('#btnStaffPopup').click(function () {
        GetStaffPopup();
    });

    $('#AssessmentNtv, #AssessmentPtv').attr('checked', false);

    $("#AssessmentNtv").click(function () {
        if ($(this).is(":checked")) {
            $('#AssessmentPtv').attr("checked", false);
            $('#AssessmentNtv').attr("checked", true);
            $('#Assessment').val($(this).val());
            $('#hdnIsCredit').val($(this).val());
            GetIssueType('IssuesCredits', '1');
            $('#Marks').empty();
            $('#Marks').append("<option value=''> Select </option>");
        }
    });

    $("#AssessmentPtv").click(function () {
        if ($(this).is(":checked")) {
            $('#AssessmentNtv').attr("checked", false);
            $('#AssessmentPtv').attr("checked", true);
            $('#Assessment').val($(this).val());
            $('#hdnIsCredit').val($(this).val());
            GetIssueType('IssuesCredits', '1');
            $('#Marks').empty();
            $('#Marks').append("<option value=''> Select </option>");
        }
    });

    $('#IssuesCredits').change(function () {
        // 
        if ($('#IssuesCredits option:selected').text() == "Others") {
            $('#OtherMarks').show();
            $('#Marks').val('').hide();
        } else {
            $('#OtherMarks').val('').hide();
            var arrMarks = new Array();
            arrMarks = $('#IssuesCredits option:selected').attr('Marks').split(',');
            $('#Marks').empty(); $('#Marks').show();
            $('#Marks').append("<option value=''> Select </option>");
            for (var i = 0; i < arrMarks.length; i++) {
                $('#Marks').append("<option value='" + arrMarks[i] + "'>" + arrMarks[i] + "</option>");
            }
        }
    });

    function frmtrUpdate(cellvalue, options, rowdata) {
        var saveBtn = "";
        if (rowdata[12] == $('#loggedInUserId').val()) {
            saveBtn = "<span class='fa fa-pencil blue T1CompUpdate' id='T1btnUpdate_" + options.rowId + "' rowid='" + options.rowId + "' title='Update'></span>";
        }
        return saveBtn;
    }
    function frmtrDel(cellvalue, options, rowdata) {
        var delBtn = "";
        if (rowdata[12] == $('#loggedInUserId').val()) {
            delBtn = "<span id='T1btnDel_" + options.rowId + "'class='fa fa-trash-o red T1CompDel' rowid='" + options.rowId + "' title='Delete'></span>";
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
        'ace-icon fa fa-angle-double-left bigger-140'
    }


    //unlike navButtons icons, action icons in rows seem to be hard-coded
    //you can change them like this in here if you want
    function updateActionIcons(table) {
        'ace-icon fa fa-angle-double-left bigger-140'
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
        postData: { Assess360Id: $('#Id').val(), tab: 1 },
        datatype: 'json',
        height: '80',

        // shrinkToFit: true,
        colNames: ['Id', 'Assess360Id', 'AssessmentCompGroup', 'Assessment', 'Assessment Name', 'Subject', 'Staff', 'Incident Date', 'Mark Details', 'Assignment Name', 'Description', 'Created Date', 'Entered By', 'Semester', '', ''],
        colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'Assess360Id', index: 'Assess360Id', hidden: true },
                          { name: 'AssessCompGroup', index: 'AssessCompGroup', sortable: false, hidden: true },
                          { name: 'IsCredit', index: 'IsCredit', hidden: false },
                          { name: 'GroupName', index: 'GroupName', sortable: false },
                          { name: 'Subject', index: 'Subject', sortable: false, hidden: true },
                          { name: 'Staff', index: 'Staff', sortable: false },
                          { name: 'IncidentDate', index: 'IncidentDate', width: 90, sortable: false },
                          { name: 'MarkDetails', index: 'MarkDetails', sortable: false },
                          { name: 'AssignmentName', index: 'AssignmentName', sortable: false, hidden: true },
                          { name: 'Description', index: 'Description', sortable: false },
                          { name: 'DateCreated', index: 'DateCreated', sortable: false },
                          { name: 'EnteredBy', index: 'EnteredBy', sortable: false, hidden: true },
                          { name: 'Semester', index: 'Semester', sortable: false, hidden: true },
                          { name: 'Update', index: 'Update', width: 20, sortable: false, formatter: frmtrUpdate },
                          { name: 'Delete', index: 'Delete', width: 20, sortable: false, formatter: frmtrDel }
                          ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        viewrecords: true,
        sortname: 'Id',
        autowidth: true,
        sortorder: 'Desc',
        caption: '<i class="fa fa-list">&nbsp;</i>Character and Behaviour',
        loadComplete: function () {
            var table = this;
            var rdata = $(grid_selector).getRowData();

            if (rdata.length > 0) {
                $('.T1CompUpdate').click(function () { UpdateComponentDtls($(this).attr('rowid')); });
                $('.T1CompDel').click(function () { DeleteComponentDtls($(this).attr('rowid')); });
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
    $(window).triggerHandler('resize.jqGrid');
});

$('#btnReset').click(function () {
    $('#Assess360ComponentId').val('');
    $('#A360CompCreatedDate').val('');
    $('#CharacterBehaviour').click();
});

function DeleteComponentDtls(id) {
    debugger;
    if (confirm("Are you sure you want to delete this item?")) {
        DeleteComponentIds(

                '/Assess360/DeleteAssess360Component?Assess360ComponentIds=' + id, //delURL, 
                '/Assess360/GetAssess360ComponentListWithPagingAndCriteria?Assess360Id=' + $('#Id').val() + '&tab=1', //reloadURL, 
                $('#CharacterBehaviourList') //GridId, 
                );
    }
}

function UpdateComponentDtls(id) {
    $('#btnAddChrtr').val('Update');
    $('#btnReset').show();
    var rowData = $('#CharacterBehaviourList').getRowData(id);
    $('#Staff').val(rowData.Staff);

    if (rowData.IsCredit && rowData.IsCredit == 'Credit') {
        $('#AssessmentPtv').attr("checked", true);
        $('#AssessmentPtv').click();
    } else {
        $('#AssessmentNtv').attr("checked", true);
        $('#AssessmentNtv').click();
    }
    var cmps = document.getElementById("IssuesCredits");
    for (i = 0; i < cmps.options.length; i++) {
        if (cmps.options[i].text == rowData.GroupName) {
            cmps.selectedIndex = i;
            $('#IssuesCredits').change();
        }
    }

    if (rowData.GroupName == "Others") {
        $('#OtherMarks').show();
        $('#Marks').val('').hide();
        $('#OtherMarks').val(rowData.MarkDetails);
    } else {
        //$('#OtherMarks').val('').hide();
        //$('#Marks').empty().show();
        var mrks = document.getElementById("Marks");
        for (i = 0; i < mrks.options.length; i++) {
            if (parseFloat(mrks.options[i].value) == parseFloat(rowData.MarkDetails)) {
                mrks.selectedIndex = i;
            }
        }
    }

    $('#IncidentDate').val(rowData.IncidentDate);
    $('#Description').val(rowData.Description);
    $('#Assess360ComponentId').val(rowData.Id);
    $('#A360CompCreatedDate').val(rowData.DateCreated);
}

$('#btnAddChrtr').click(function () {
    SaveOrUpdateAssess360Component('CharacterBehaviour');
});


function InitStaffMasterGrid() {
    $("#StaffMasterList").jqGrid({
        datatype: 'local',
        colNames: ['Id', 'Date of Joined', 'Grade', 'Staff Name', 'Subject Teaching'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'DateJoined', index: 'DateJoined', hidden: true },
              { name: 'Grade', index: 'Grade', width: 150, hidden: true },
              { name: 'StaffName', index: 'StaffName', width: 150 },
              { name: 'SubjectTeaching', index: 'SubjectTeaching', hidden: true }
              ],
        pager: $("#StaffMasterPager"),
        rowNum: 10,
        rowList: [10, 15, 20, 50],
        sortname: 'StaffName',
        sortorder: 'asc',
        viewrecords: true,
        height: 'auto',
        shrinkToFit: true,
        caption: 'Staff List',
        autowidth: true, //$("#StaffMasterList").parent().width(), //430,
        onSelectRow: function (rowid, status) {
            debugger;
            rowData = $('#StaffMasterList').getRowData(rowid);
            $('#Staff').val(rowData.StaffName);
            if (clbPupGrdSel != undefined && clbPupGrdSel) { clbPupGrdSel(rowData); }
            clbPupGrdSel = null;
            $('#DivStaffMasterSearch').dialog('close');
        },
        loadComplete: function () {

            var table = this;
            setTimeout(function () {

                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
}