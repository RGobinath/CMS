$(function () {

    var grid_selector = "#ReportCardIBInboxList";
    var pager_selector = "#ReportCardIBInboxPage";

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

    $(grid_selector).jqGrid({
        mtype: 'GET',
        url: '/Assess360/GetFormativeMarkInbox?Name=' + $('#Name').val() + '&Campus=' + $('#Campus').val() + '&Section=' + $('#Section').val() + '&Grade=' + $('#Grade').val(),
        //postData: { Assess360Id: $('#Id').val() },
        datatype: 'json',
        height: '250',
        autowidth: true,
        colNames: ['Id', 'Name', 'Academic Year', 'Campus Name', 'Id No', 'Section', 'Grade', 'View FA Marks', 'FA PDF', 'View FAHW Marks', 'FAHW PDF', 'Created By', 'Date Created', ''],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
                   { name: 'Name', index: 'Name' },
                   { name: 'AcademicYear', index: 'AcademicYear' },
                   { name: 'Campus', index: 'Campus' },
                   { name: 'IdNo', index: 'IdNo' },
                   { name: 'Section', index: 'Section' },
                   { name: 'Grade', index: 'Grade' },
                   { name: 'ViewFAmarks', index: 'ViewFAmarks', align: 'center' },
                   { name: 'GetFAPdf', index: 'GetFAPdf', align: 'center' },
                   { name: 'ViewFAHWmarks', index: 'ViewFAHWmarks', align: 'center' },
                   { name: 'GetFAHWPdf', index: 'GetFAHWPdf', align: 'center' },
                   { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                   { name: 'DateCreated', index: 'DateCreated', hidden: true },
                   { name: 'CreatedBy', index: 'CreatedBy', hidden: true }
        ],
        pager: pager_selector,
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'asc',
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Formative Analysis',
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
            {}, {}, {})



    $.getJSON("/Base/FillBranchCode",
   function (fillig) {
       var ddlcam = $("#Campus");
       ddlcam.empty();
       ddlcam.append($('<option/>', { value: "", text: "Select One" }));
       $.each(fillig, function (index, itemdata) { ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text })); });
   });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $('#btnSearch').click(function () {
        if (isEmptyorNull($("#Campus").val())) {
            ErrMsg("Please select Campus"); return false;
        }
        else if (isEmptyorNull($("#Grade").val())) {
            ErrMsg("Please select Grade"); return false;
        }
        else {
            $(grid_selector).clearGridData();
            LoadSetGridParam($(grid_selector), "/Assess360/GetFormativeMarkInbox?Name=" + $('#Name').val() + '&Campus=' + $('#Campus option:selected').val() + '&Section=' + $('#Section option:selected').val() +
            '&Grade=' + $('#Grade option:selected').val());
        }
    });

    $('#btnReset').click(function () {
        $('#Assess360InboxList').clearGridData();
        $('#Name').val('');
        $('#Campus').val('');
        $('#Section').val('');
        $('#Grade').val('');
        jQuery(grid_selector).jqGrid('clearGridData').jqGrid('setGridParam', { data: data, page: 1 }).trigger('reloadGrid');
        //LoadSetGridParam($(grid_selector), "/Assess360/GetReportCardIBInbox?page=1");
    });

    $('#GenerateAll').click(function () {

        if (isEmptyorNull($("#Campus").val())) {
            ErrMsg("Please select Campus"); return false;
        }
        else if (isEmptyorNull($("#Grade").val())) {
            ErrMsg("Please select Grade"); return false;
        }
        else if (isEmptyorNull($("#Section").val())) { ErrMsg("Please select Section"); return false; }
        else{
            window.open('/Assess360/GenerateFAMarkAllPDF?Campus=' + $("#Campus").val() + '&Grade=' + $("#Grade").val() + '&Section=' + $("#Section").val());
        }
    });

    $('#GenerateComparitiveFA').click(function () {
        if (isEmptyorNull($("#Campus").val())) {
            ErrMsg("Please select Campus"); return false;
        }
        else if (isEmptyorNull($("#Grade").val())) {
            ErrMsg("Please select Grade"); return false;
        }
        else if (isEmptyorNull($("#Section").val())) { ErrMsg("Please select Section"); return false; }
        else {
            window.open('/Assess360/ComparitiveFAReport?Campus=' + $("#Campus").val() + '&Grade=' + $("#Grade").val() + '&Section=' + $("#Section").val());
        }
    });

    $('#GenerateComparitiveFAHW').click(function () {
        if (isEmptyorNull($("#Campus").val())) {
            ErrMsg("Please select Campus"); return false;
        }
        else if (isEmptyorNull($("#Grade").val())) {
            ErrMsg("Please select Grade"); return false;
        }
        else if (isEmptyorNull($("#Section").val())) { ErrMsg("Please select Section"); return false; }
        else {
            window.open('/Assess360/ComparitiveFAHWReport?Campus=' + $("#Campus").val() + '&Grade=' + $("#Grade").val() + '&Section=' + $("#Section").val());
        }
    });


    /*enter key press event*/
    function defaultFunc(e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#btnSearch').click();
            return false;
        }
        return true;
    };
});
function GetFAPdfMarkList(assessId) {
    window.open('/Assess360/GenerateFAMarkPDF?assessid=' + assessId+'&IsFAHW=false');
}
function GetFAHWPdfMarkList(assessId) {
    window.open('/Assess360/GenerateFAMarkPDF?assessid=' + assessId + '&IsFAHW=true');
}
function ViewFAMarksList(assId) {
    ModifiedLoadPopupDynamicaly("/Assess360/ViewFAMarks?Assess360Id=" + assId, $('#Viewmarks'),
            function () {
                LoadSetGridParam($('#ViewFAMarksList'), "/Assess360/ViewFAMarksListJqGrid?Id=" + assId)
            }, function () { }, 925, 410, "FA MarkList");
}

function ViewFAHWMarksList(assId) {
    ModifiedLoadPopupDynamicaly("/Assess360/ViewFAHWMarks?Assess360Id=" + assId, $('#Viewmarks'),
            function () {
                LoadSetGridParam($('#ViewFAHWMarksList'), "/Assess360/ViewFAHWMarksListJqGrid?Id=" + assId)
            }, function () { }, 1050, 410, "FAHW MarkList");
}
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
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