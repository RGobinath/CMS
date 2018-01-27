$(function () {
    $('#cssmenu > ul > li > a').click(function () {
        $('#cssmenu li').removeClass('active');
        $(this).closest('li').addClass('active');
    });
    $("#ddlSearchBy").change(function () {
        if ($("#ddlSearchBy").val() == "") {
            $("#txtSearch").val("");
            $("#txtSearch").attr("disabled", true);
        }
        else {
            $("#txtSearch").attr("disabled", false);
        }
    });

    //    $('.CallMgmtdatepicker').datepicker({
    //        format: "dd/mm/yy",
    //        buttonImageOnly: true,
    //        changeMonth: true,
    //        timeFormat: 'hh:mm:ss',
    //        autowidth: true,
    //        changeYear: true
    //    });
    var grid_selector = "#CallManagementList";
    var pager_selector = "#CallManagementListPager";

    var Status = "Available";
    //ViewBag Object id declaration
    $("#ddlSearchBy").val($("#SearchKey").val());
    $("#txtSearch").val($("#SearchVal").val());
    $("#ddlStatus").val($("#CMGTStatus").val());
    //    $("#ddlSearchBy").val('@ViewBag.SearchKey');
    //    $("#txtSearch").val('@ViewBag.SearchValue');
    //    //$("#ddlStatus").val('@ViewBag.CMGTStatus');
    //    if ('@ViewBag.CMGTStatus' != null && '@ViewBag.CMGTStatus' != '') {
    //        Status = '@ViewBag.CMGTStatus';
    //        $('#' + Status).removeClass('active').addClass('active');
    //    }
    //    var srchitems = '@ViewBag.CMGTSearched';
    //    var srchitemsArr = srchitems.split(',');
    //    $("#txtFromDate").val(srchitemsArr[0]);
    //    var vGrade;
    //    var count = "@ViewBag.count";
    //    var count1 = "@ViewBag.count1";
    if ($("#CMGTStatus").val() != null && $("#CMGTStatus").val() != '') {
        Status = $("#CMGTStatus").val();
        $('#' + Status).removeClass('active').addClass('active');
    }
    var srchitems = $("#CMGTSearched").val();
    var srchitemsArr = srchitems.split(',');
    $("#txtFromDate").val(srchitemsArr[0]);
    var vGrade;
    var count = $("#Count").val();
    var count1 = $("#Count1").val();
    if (count == "0") {
        $("#New").hide();
        $("#btnBulkComplete").hide();
    }

    if (count1 == "0") {
        $("#btnBulkInfoComplete").hide();
    }
    else {
        $("#btnBulkInfoComplete").show();
    }

    $("#Search").click(function (Status) {

        $(grid_selector).clearGridData();
        var txtSearch = $("#txtSearch").val();
        var ddlSearchBy = $("#ddlSearchBy").val();
        var fromDate = $("#txtFromDate").val();
        var status = "";
        var Grades = $("#ddlGrades").val();
        if (Grades != '' && Grades != null) {
            Grades = Grades.toString();
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: "/Home/CallManagementListJqGrid/",
                    postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, Grades: Grades, status: status },
                    page: 1
                }).trigger("reloadGrid");
    });

    if ($("#SubmitSuccessMsg").val() != null & $("#SubmitSuccessMsg").val() != "") {
        InfoMsg("Issue created Successfully.\n Issue Number is:" + $("#SubmitSuccessMsg").val(), function () { $("#SubmitSuccessMsg").val(""); });
    }

    $("#New").click(function () {
        window.location.href = "/Home/CallRegister";
    });

    $("#btnBulkComplete").click(function () {

        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var Template = "";
        var userId = "";
        var rowData = [];
        var rowData1 = [];
        var activityName = [];
        var findString = "";
        var recepient = [];
        var Resolution = [];
        var IssueId = [];
        var BranchCode = [];
        var i = 0;
        if (GridIdList.length > 0) {

            for (i; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].Id;
                recepient[i] = rowData[i].Email;
                IssueId[i] = rowData[i].IssueId;
                Resolution[i] = rowData[i].Resolution;
                activityName[i] = rowData[i].Status;
                BranchCode[i] = rowData[i].BranchCode;
                if (activityName[i] != "Complete") {
                    ErrMsg("Please select complete items only to complete");
                    return false;
                }
            }
            $.ajax({
                url: "/Home/BulkComplete/",
                type: 'POST',
                dataType: 'json',
                data: { ActivityId: rowData1, Template: Template, userId: userId, recepient: recepient, IssueId: IssueId, ResolutionComments: Resolution, BranchCode: BranchCode },
                traditional: true,
                // page: 1,
                success: function (data) {

                    if (data == true) {
                        $(grid_selector).clearGridData();
                        var txtSearch = $("#txtSearch").val();
                        var ddlSearchBy = $("#ddlSearchBy").val();
                        var fromDate = $("#txtFromDate").val();
                        var status = $("#ddlStatus").val();
                        $(grid_selector).setGridParam(
                             {
                                 datatype: "json",
                                 url: "/Home/CallManagementListJqGrid/",
                                 type: 'POST',
                                 postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, status: status },
                                 page: 1
                             }).trigger("reloadGrid");
                    }
                    InfoMsg("Issue completed Successfully");
                }
            });
        }
        if (rowData1 == "") {
            ErrMsg("Please select atleast one row to complete.");
            return false;
        }
    });

    $("#btnBulkInfoComplete").click(function () {

        $.ajax({
            url: "/Home/BulkInfoCompleteNew/",
            type: 'POST',
            dataType: 'json',
            data: {},
            traditional: true,
            // page: 1,
            success: function (data) {
                if (data == true) {
                    $(grid_selector).clearGridData();
                    var txtSearch = $("#txtSearch").val();
                    var ddlSearchBy = $("#ddlSearchBy").val();
                    var fromDate = $("#txtFromDate").val();
                    var status = $("#ddlStatus").val();
                    $(grid_selector).setGridParam(
                                     {
                                         datatype: "json",
                                         url: "/Home/CallManagementListJqGrid/",
                                         type: 'POST',
                                         postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, status: status },
                                         page: 1
                                     }).trigger("reloadGrid");
                }
                InfoMsg("Information completed Successfully");
            }
        });
    });

    $("#reset").click(function () {
        window.location.href = $("#reseturl").val();
    });
    //    $("#ExportExcel").click(function () {
    //        window.location.href = "/Home/ExportExcel";
    //    });
    //    $("#ExportToExcel").click(function () {
    //        window.open('/Home/CallManagementListNewJqGrid');
    //    });



    var txtSearch = $("#txtSearch").val();
    var SearchBy = $("#ddlSearchBy").val();
    var fromDate = $("#txtFromDate").val();
    var Grades = $("#ddlGrades").val();
    if (Grades != '' && Grades != null) {
        Grades = Grades.toString();
    }

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-10").width());
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
    jQuery(grid_selector).jqGrid({
        url: "/Home/CallManagementListJqGrid?status=" + Status + "&ddlSearchBy=" + SearchBy + "&txtSearch=" + txtSearch + "&fromDate=" + fromDate + "&Grades=" + Grades,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Ticket Number', 'Campus', 'Student Name', 'Grade', 'Request Date', 'Information For', 'Leave Type', 'Issue Group', 'Issue Type', 'Activity Name', 'Action Date', 'User Type', 'History', 'SLA Status', 'Email', 'Resolution', 'IssueId', ''],
        colModel: [
        // if any column added in future have to check rowObject for SLA status image.... 
              {name: 'Id', index: 'Id', hidden: true },
        //{ name: 'IssueNumber', index: 'IssueNumber', width: 105, hidden:false, sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + rawObject[17] + '"' } },
        // { name: 'IssueNumber', index: 'IssueNumber', width: 90, sortable: true, cellattr: function (rowId, val, rawObject) { return '"'+ rawObject[17] +'"' } },
              {name: 'IssueNumber', index: 'IssueNumber' },
              { name: 'BranchCode', index: 'BranchCode', sortable: true, search: true },
              { name: 'StudentName', index: 'StudentName', sortable: true },
              { name: 'Grade', index: 'Grade', sortable: true },
              { name: 'IssueDate', index: 'IssueDate', sortable: true, width: 120, search: false },
              { name: 'InformationFor', index: 'InformationFor', sortable: true },
              { name: 'LeaveType', index: 'LeaveType', sortable: true, hidden: true },
              { name: 'IssueGroup', index: 'IssueGroup', sortable: true },
              { name: 'IssueType', index: 'IssueType', sortable: true },
              { name: 'Status', index: 'Status', sortable: true, search: false },
              { name: 'ActionDate', index: 'ActionDate', sortable: true,width:120, hidden: false, search: false },
              { name: 'UserType', index: 'UserType', search: false,width:100, formatter: SourceFrom },
              { name: 'History', index: 'History', align: 'center', width:50, sortable: false, search: false },
              { name: 'Stat', index: 'Stat', align: 'center',width:50, formatter: statusimage, resizable: true, sortable: false, search: false },
              { name: 'Email', index: 'Email', hidden: true },
              { name: 'Resolution', index: 'Resolution', hidden: true },
              { name: 'IssueId', index: 'IssueId', hidden: true },
              { name: 'Description', index: 'Description', sortable: false, hidden: true }
            ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 238,
        autowidth: true,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;Call Management Issue List',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            //$(grid_selector).jqGrid('setGridWidth');
        },
        loadError: function (xhr, status, error) {
            $(grid_selector).clearGridData();
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
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

});

function SourceFrom(cellvalue, options, rowObject) {
    if (cellvalue == 'Parent') { return 'Parent' }
    else if (cellvalue == 'Student') { return 'Student' }
    else { return 'School' }
}
function Status(Status) {
    $("#CallManagementList").clearGridData();
    var txtSearch = $("#txtSearch").val();
    var ddlSearchBy = $("#ddlSearchBy").val();
    var fromDate = $("#txtFromDate").val();
    var fromIssueNum = $("#txtFromIssueNum").val();
    var toIssueNum = $("#txtToIssueNum").val();
    var status = Status;
    var Grades = $("#ddlGrades").val();
    if (Grades != '' && Grades != null) {
        Grades = Grades.toString();
    }
    $("#CallManagementList").setGridParam(
                {
                    datatype: "json",
                    url: "/Home/CallManagementListJqGrid/",
                    postData: { txtSearch: txtSearch, ddlSearchBy: ddlSearchBy, fromDate: fromDate, fromIssueNum: fromIssueNum, toIssueNum: toIssueNum, Grades: Grades, status: status },
                    page: 1
                }).trigger("reloadGrid");
}
function statusimage(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseInt(cellvalue);
    var cml = $("#CallManagementList").jqGrid();
    for (i = 0; i < cml.length; i++) {
        if ((rowObject[6] != "" || rowObject[6] != null) && rowObject[8] == "") {
            return '<img src="../../Images/blue.jpg" height="12px" width="12px" />'
        }
        else if (rowObject[8] != "") {
            if (cellValueInt <= 24) {
                return '<img src="../../Images/yellow.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellValueInt > 24 && cellValueInt <= 48) {
                return '<img src="../../Images/orange.jpg" height="10px" width="10px"  alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellValueInt > 48) {
                return '<img src="../../Images/redblink3.gif" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellvalue == 'Completed') {
                return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
        }
    }
}


function ShowComments(ActivityId, UserType) {
    ModifiedLoadPopupDynamicaly("/Home/Activities?Id=" + ActivityId + "&UserType=" + UserType, $('#Activities'),
            function () {
                LoadSetGridParam($('#ActivitiesList'), "/Home/ActivitiesListJqGrid?Id=" + ActivityId)
            }, function () { }, 925, 410, "ActivitiesList");
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
function LoadPopupDynamicaly1(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
    if (width == undefined) { width = 800; }
    //if (ModalId.html().length == 0) {
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
                title: '<i class="fa fa-list"></i>&nbsp;<label>History</label>',
                buttons: {}
            });
            ModalId.html(data);
        }
    });
    clbPupGrdSel = onSelcalbck;
    ModalId.dialog('open');
    CallBackFunction(loadCalBack);
}