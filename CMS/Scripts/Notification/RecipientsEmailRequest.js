$(function () {
    $('#Compose').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });

    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    // var categoriesStr = ":All;1:True;2:False";


    var grid_selector = "#jqBulkEmailRequest";
    var pager_selector = "#jqGridBulkEmailRequest";

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

    //Pager icons

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
        url: "/Communication/JqGridRecipientsEmailRequest",
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'IdKeyValue', 'Compose Request Id', 'UserId', 'Campus', 'Grade', 'AcademicYear', 'Father', 'Mother', 'General', 'Subject', 'Attachment', 'Message', 'Status', 'Created By', 'Modified By', 'Created Date', 'Modified Date', 'Sent','Not Sent', 'In Progress', 'Invalid Mail', 'Total'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true },
            { name: 'IdKeyValue', index: 'IdKeyValue', width: 25, hidden: true },
            { name: 'BulkReqId', index: 'BulkReqId', width: 45 },
            { name: 'UserId', index: 'UserId', width: 60, hidden: true },
            { name: 'Campus', index: 'Campus', width: 60, search: false },
            { name: 'Grade', index: 'Grade', width: 60, search: false },
            { name: 'AcademicYear', index: 'AcademicYear', width: 60, hidden: true },
            { name: 'Father', index: 'Father', width: 30, hidden: true },
            { name: 'Mother', index: 'Mother', width: 30, hidden: true },
            { name: 'General', index: 'General', width: 30, hidden: true },
            { name: 'Subject', index: 'Subject', width: 100 },
            {
                name: 'Attachment', index: 'Attachment', width: 60, stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: ":All;1:True;0:False" }
            },
            { name: 'Message', index: 'Message', width: 90, search: false },
            {
                name: 'Status', index: 'Status', width: 80, stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: ":All;1:Email Composed;2:Recipients Added;3:CompletedWithErrors;4:SuccessfullyCompleted;5:Suspend" }
            },
            { name: 'CreatedBy', index: 'CreatedBy', width: 60, search: false, hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', width: 60, hidden: true, hidden: true },
            { name: 'CreatedDate', index: 'CreatedDate', width: 40, search: false },
            { name: 'ModifiedDate', index: 'ModifiedDate', width: 60, hidden: true },
            { name: 'Sent', index: 'Sent', width: 30, search: false, align: 'center', formatter: formatterlink },
            { name: 'NotSent', index: 'NotSent', width: 30, search: false, align: 'center', formatter: formatterlink },            
            { name: 'InProgress', index: 'InProgress', width: 30, search: false, align: 'center', formatter: formatterlink },
            { name: 'InvalidMail', index: 'InvalidMail', width: 30, search: false, align: 'center', formatter: formatterlink },
            { name: 'Total', index: 'Total', width: 30, search: false, align: 'center', formatter: formatterlink }
        ],

        pager: pager_selector,
        rowNum: 100,
        rowList: [100, 200, 250, 300],
        sortname: 'Id',
        sortorder: 'Asc',
        reloadAfterSubmit: true,
        autowidth: true,
        height: 215,
        viewrecords: true,
        caption: '<i class="fa fa-envelope"></i>&nbsp;Bulk Email Request',
        shrinkToFit: true,
        loadComplete: function () {
            debugger;
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            $("tr.jqgrow:odd").addClass('RowBackGroundColor');
            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.Status == "Email Composed") { $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#66CCFF" }); }
                else if (rowData.Status == "Recipients Added") { $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#FFFF99" }); }
                else if (rowData.Status == "SuccessfullyCompleted") { $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#99FF66" }); }
                else if (rowData.Status == "Suspended") { $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#FF9900" }); }
                else { $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#FF2400" }); }
            }
            var table = this;
            setTimeout(function () {
                debugger;
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }



    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, defaultSearch: "cn" });



});


function Search() {

    $(grid_selector).setGridParam({
        datatype: 'json',
        url: "/Communication/JqGridRecipientsEmailRequest",
        postData: { Subject: $('#txtSubject').val(), IsSent: $('#txtIsSent').val(), CreatedDate: $('#CreatedDate').val(), CreatedBy: $('#txtCreatedBy').val() },
        page: 1
    }).trigger('reloadGrid');
}

function Reset() {
    $("input[type=text], textarea, select").val("");
}

function ShowComments(Id, BulkReqId) {
    $.ajax({
        url: '/Communication/ShowMessage?Id=' + Id,
        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {
            if (data != 0) {
                $("#dialog").dialog("open");
                $('#showMessage').html(data);

            }
        }
    });
}
function ShowEmailRecipientsDetails(ComposeId, Status) {
    if (Status == "NotSent")
    {
        Status = "Not Sent";
    }
    if (Status == "InvalidMail")
    {
        Status = "InValid MailId";
    }
    ModifiedLoadPopupDynamicaly("/Communication/ShowEmailRecipientsDetails?ComposeId=" + ComposeId + '&Status=' + Status, $('#ShowEmailReceipientsDetails'),
               function () { }, function () { }, 1200, 500, "Email Recipients");
}
function formatterlink(cellvalue, options, rowObject) {
    //return cellvalue;
    var delBtn = "";
    delBtn = "<a style='cursor:pointer;text-decoration:underline;' onclick=ShowEmailRecipientsDetails('" + rowObject[0] + "','" + options.colModel.index + "');>" + cellvalue + "</a>";
    return delBtn;
}