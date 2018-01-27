$(function () {


    var grid_selector = "#HomeWorkRelatedList";
    var pager_selector = "#HomeWorkRelatedPage";

    $('.obtotalMarks').hide();
    $("#btnStaffPopup").button({ icons: { primary: "ui-icon-search" },
        text: false
    });
    $('#btnResetT2').hide();
    if ($('#loggedInUserType').val() == "Staff") {
        $('#Staff').val($('#loggedInUserName').val());
        $('#btnStaffPopup').hide();
    }

    $('#btnStaffPopup').click(function () {
        GetStaffPopup();
    });

    $("#HWRDatofAssgn").datepicker({
        format: 'dd-M-yy',
        autoclose: true
    });
    if ($('#hdnCampus').val() == "IB MAIN" || $('#hdnCampus').val() == "CHENNAI MAIN") {
        semhideshow("show");
    }
    else {
        semhideshow("hide");
    }
    //   $('.datepicker').datepicker('option', 'dateFormat', 'dd-M-yy');
    $('#HWRAssmntType').change(function () {
        // 
        $('#hdnIsCredit').val($('#HWRAssmntType option:selected').attr('IsCredit'));
        if ($('#HWRAssmntType option:selected').text() == "Homework score") {
            $('.snglMarks').hide();
            $('.obtotalMarks').show();
            $('#HWRMarks').val('');
        } else {
            $('.snglMarks').show();
            $('.obtotalMarks').hide();
            $('#HWRTotalMarks').val('');
            $('#HWRobtainedMarks').val('');

            var arrMarks = new Array();
            arrMarks = $('#HWRAssmntType option:selected').attr('Marks').split(',');

            $('#HWRMarks').empty();
            $('#HWRMarks').append("<option value=''> Select </option>");
            for (var i = 0; i < arrMarks.length; i++) {
                $('#HWRMarks').append("<option value='" + arrMarks[i] + "'>" + arrMarks[i] + "</option>");
            }
        }
    });

    function frmtrUpdate(cellvalue, options, rowdata) {
        var saveBtn = "";
        if (rowdata[12] == $('#loggedInUserId').val()) {
            saveBtn = "<span id='T2btnUpdate_" + options.rowId + "'class='fa fa-pencil blue T2CompUpdate'  rowid='" + options.rowId + "' title='Update'></span>";
        }
        return saveBtn;
    }
    function frmtrDel(cellvalue, options, rowdata) {
        var delBtn = "";
        if (rowdata[12] == $('#loggedInUserId').val()) {
            delBtn = "<span id='T2btnDel_" + options.rowId + "'class='fa fa-trash-o red T2CompDel' rowid='" + options.rowId + "' title='Delete'></span>";
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
        postData: { Assess360Id: $('#Id').val(), tab: 2 },
        datatype: 'json',
        height: '80',

        //shrinkToFit: true,
        colNames: ['Id', 'Assess360Id', 'AssessCompGroup', 'IsCredit', 'Type', 'Subject', 'Staff', 'Date of Assigning', 'Mark Awarded', 'Assignment/Homework Name', 'Description', 'Created Date', 'Entered By', 'Semester', '', ''],
        colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'Assess360Id', index: 'Assess360Id', hidden: true },
                          { name: 'AssessCompGroup', index: 'AssessCompGroup', hidden: true },
                          { name: 'IsCredit', index: 'IsCredit', hidden: true },
                          { name: 'GroupName', index: 'GroupName', sortable: false },
                          { name: 'Subject', index: 'Subject', sortable: false },
                          { name: 'Staff', index: 'Staff', width: 100, sortable: false },
                          { name: 'IncidentDate', index: 'IncidentDate', width: 90, sortable: false },
                          { name: 'MarkDetails', index: 'MarkDetails', width: 80, sortable: false },
                          { name: 'AssignmentName', index: 'AssignmentName', width: 180, sortable: false },
                          { name: 'Description', index: 'Description', sortable: false, hidden: true },
                          { name: 'DateCreated', index: 'DateCreated', width: 80, sortable: false },
                          { name: 'EnteredBy', index: 'EnteredBy', sortable: false, hidden: true },
                          { name: 'Semester', index: 'Semester', sortable: false, hidden: true },
                          { name: 'Update', index: 'Update', width: 18, sortable: false, formatter: frmtrUpdate },
                          { name: 'Delete', index: 'Delete', width: 18, sortable: false, formatter: frmtrDel }
                          ],
        pager: pager_selector,
        rowNum: '10',
        autowidth: true,
        rowList: [5, 10, 20, 50, 100, 150, 200],
        viewrecords: true,
        sortname: 'Id',
        sortorder: 'Desc',
        caption: '<i class="fa fa-home"></i>&nbsp;Home Work Related',
        loadComplete: function () {

            var rdata = $(grid_selector).getRowData();
            var table = this;
            if (rdata.length > 0) {
                $('.T2CompUpdate').click(function () { UpdateComponentDtlsT2($(this).attr('rowid')); });
                $('.T2CompDel').click(function () { DeleteComponentDtlsT2($(this).attr('rowid')); });
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

$('#btnResetT2').click(function () {
    $('#Assess360ComponentId').val('');
    $('#A360CompCreatedDate').val('');
    $('#HomeWorkRelated').click();
});

function DeleteComponentDtlsT2(id) {
    if (confirm("Are you sure you want to delete this item?")) {
        DeleteComponentIds(
                '/Assess360/DeleteAssess360Component?Assess360ComponentIds=' + id, //delURL, 
                '/Assess360/GetAssess360ComponentListWithPagingAndCriteria?Assess360Id=' + $('#Id').val() + '&tab=2', //reloadURL, 
                $('#HomeWorkRelatedList') //GridId, 
                );
    }
    return false;
}

function UpdateComponentDtlsT2(id) {
    $('#btnAddHWR').val('Update');
    $('#btnResetT2').show();
    var rowData = $('#HomeWorkRelatedList').getRowData(id);
    $('#Staff').val(rowData.Staff);

    var cmps = document.getElementById("HWRAssmntType");
    for (i = 0; i < cmps.options.length; i++) {
        if (cmps.options[i].text == rowData.GroupName) {
            cmps.selectedIndex = i;
            $('#HWRAssmntType').change();
        }
    }
    if ($("#hdnCampus").val() == "IB MAIN" || $('#hdnCampus').val() == "CHENNAI MAIN") {
        var semtype = document.getElementById("HWSemester");
        for (i = 0; i < semtype.options.length; i++) {
            if (semtype.options[i].text == rowData.Semester) {
                semtype.selectedIndex = i;
            }
        }
    }
    var subj = document.getElementById("HWRSubject");
    for (i = 0; i < subj.options.length; i++) {
        if (subj.options[i].text == rowData.Subject) {
            subj.selectedIndex = i;
        }
    }

    if (rowData.GroupName == 'Homework score') {
        $('#HWRobtainedMarks').val(rowData.MarkDetails.substring(0, rowData.MarkDetails.lastIndexOf("/")));
        $('#HWRTotalMarks').val(rowData.MarkDetails.substring(rowData.MarkDetails.lastIndexOf("/") + 1, rowData.MarkDetails.length));
    } else {
        var mrks = document.getElementById("HWRMarks");
        for (i = 0; i < mrks.options.length; i++) {
            if (parseFloat(mrks.options[i].value) == parseFloat(rowData.MarkDetails)) {
                mrks.selectedIndex = i;
            }
        }
    }

    $('#HWRDatofAssgn').val(rowData.IncidentDate);
    //$('#HWRAssmntName').val(rowData.AssignmentName);
    /*
    This method added by Lee on 22-Jun-2013, 
    Earlier this is a free text, 
    now it has been turned to Master data.
    */
    GetAssignmentName('HWRAssmntName', rowData.AssignmentName);

    $('#Assess360ComponentId').val(rowData.Id);
    $('#A360CompCreatedDate').val(rowData.DateCreated);
}

$('#btnAddHWR').click(function () {
    SaveOrUpdateAssess360Component('HomeWorkRelated');
});

$("#HWRSubject").change(function () {
    var cam = $("#hdnCampus").val();
    var gra = $("#Grade").val();
    var sub = $("#HWRSubject").val();
    var assgnname = $("#HWRAssmntName").val();
    GetAssignmentNameByCampusGradeSubject('HWRAssmntName', assgnname, cam, gra, encodeURIComponent(sub))
});
       