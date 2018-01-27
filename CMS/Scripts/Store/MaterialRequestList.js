var grid_selector = "#MaterialRequestList";
var pager_selector = "#MaterialRequestListPager";
$(function () {
    $("#NewRequest").hide();
    $("#btnCreateIssueNote").hide();
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
    $('#RequestedDate').datepicker({
        dateFormat: 'dd/mm/yy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true
    });
    var flag = $('#flag').val() ;
    if (flag == "MRC") {
        $("#NewRequest").show();
    }
    if (flag == "INC" && ($("#ddlStatus").val() == "Available" || $("#ddlStatus").val() == "Assigned")) {
        $("#btnCreateIssueNote").show();
    }
    var status = $("#ddlStatus").val();
    $("#ddlStatus").change(function () {
        var status = $("#ddlStatus").val();
        $("#Search").click();
        if ($(this).val() == "Sent" || $(this).val() == "Completed") {
            $("#btnCreateIssueNote").hide();
        }
        else {
            $("#btnCreateIssueNote").show();
        }
    });

    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialRequestListJqGrid?status=' + status,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Request Number', 'Campus', 'Processed By', 'Requested Date', 'Request Status', 'SLA Status', '', '', ''],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
                       { name: 'RequestNumber', index: 'RequestNumber', width: 90, sortable: true, formatter: formateadorLink },
                       { name: 'Campus', index: 'Campus', width: 90, sortable: true },
                       { name: 'ProcessedBy', index: 'ProcessedBy', width: 90, sortable: true },
                       { name: 'RequestedDate', index: 'RequestedDate', width: 90, sortable: true },
                       { name: 'RequestStatus', index: 'RequestStatus', width: 90, sortable: true },
                       { name: 'Stat', index: 'Stat', width: 30, align: 'center', formatter: statusimage, resizable: true, sortable: false },
                       { name: 'ActivityId', index: 'ActivityId', hidden: true },
                       { name: 'ActivityName', index: 'ActivityName', hidden: true },
                       { name: 'ActivityFullName', index: 'ActivityFullName', hidden: true}],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '250',
        autowidth: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Material Request List',
        multiselect: true,
        subGrid: true,
        loaderror: function (xhr, status, error) {
            $(grid_selector).cleargriddata();
            errmsg($.parsejson(xhr.responsetext).message);
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (MatReqList, Id) {
            var MatReqListTable, MatReqListPager;
            MatReqListTable = MatReqList + "_t";
            MatReqListPager = "p_" + MatReqListTable;
            $("#" + MatReqList).html("<table id='" + MatReqListTable + "' ></table><div id='" + MatReqListPager + "' ></div>");
            jQuery("#" + MatReqListTable).jqGrid({
                url: '/Store/MaterialRequestJqGrid?Id=' + Id,
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'Req.Type', 'Grade', 'Section', 'Req.For', 'Material', 'Material Group', 'Material Sub Group', 'Units', 'Req.Date', 'Status', 'Req.Qty', 'App.Qty', 'Issued.Qty'],
                colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
                               { name: 'RequestType', index: 'RequestType', width: 90, sortable: true, editable: false },
                               { name: 'RequiredForGrade', index: 'RequiredForGrade', width: 90, sortable: true },
                               { name: 'Section', index: 'Section', width: 60, sortable: true },
                               { name: 'RequiredFor', index: 'RequiredFor', width: 90, sortable: true },
                               { name: 'Material', index: 'Material', width: 90, sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[6] + ', Material Sub Group:' + rawObject[7] + '"' } },
                               { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true, hidden: true },
                               { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 150, sortable: true, hidden: true },
                               { name: 'Units', index: 'Units', width: 90, sortable: true },
                               { name: 'RequiredDate', index: 'RequiredDate', width: 90, sortable: true },
                               { name: 'Status', index: 'Status', width: 90, sortable: true },
                               { name: 'Quantity', index: 'Quantity', width: 90, sortable: true },
                               { name: 'ApprovedQty', index: 'ApprovedQty', width: 90, sortable: true },
                               { name: 'IssuedQty', index: 'IssuedQty', width: 90, sortable: true}],
                pager: MatReqListPager,
                rowNum: '5',
                rowList: [5, 10, 20, 50],
                sortname: 'Id',
                sortorder: 'Desc',
                height: '130',
                autowidth: true,
                viewrecords: true,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                },
                subGrid: true,
                subGridOptions: {
                    plusicon: "ace-icon fa fa-plus center bigger-110 blue",
                    minusicon: "ace-icon fa fa-minus center bigger-110 blue",
                    openicon: "ace-icon fa fa-chevron-right center orange",
                    // load the subgrid data only once // and the just show/hide 
                    "reloadOnExpand": false,
                    // select the row when the expand column is clicked 
                    "selectOnExpand": true
                },
                subGridRowExpanded: function (MatIssueList, Id) {
                    var MatIssueListTable, MatIssueListPager;
                    MatIssueListTable = MatIssueList + "_t";
                    MatIssueListPager = "p_" + MatIssueListTable;
                    $("#" + MatIssueList).html("<table id='" + MatIssueListTable + "' ></table><div id='" + MatIssueListPager + "' ></div>");
                    jQuery("#" + MatIssueListTable).jqGrid({
                        url: '/Store/MaterialIssueListJqGrid?Id=' + Id,
                        datatype: 'json',
                        mtype: 'GET',
                        colNames: ['Id', 'MRL Id', 'Iss Note Number', 'Issue Date', 'Issued Qty', 'Issued By', 'Status', ''],
                        colModel: [{ name: 'Id', index: 'Id', hidden: true },
                                       { name: 'MRLId', index: 'MRLId', width: 90, sortable: true, hidden: true },
                                       { name: 'IssNoteNumber', index: 'IssNoteNumber', width: 90, sortable: true, formatter: ShowIssueNote },
                                       { name: 'IssueDate', index: 'IssueDate', width: 90, sortable: true },
                                       { name: 'IssueQty', index: 'IssueQty', width: 90, sortable: true },
                                       { name: 'IssuedBy', index: 'IssuedBy', width: 90, sortable: true },
                                       { name: 'Status', index: 'Status', width: 90, sortable: true },
                                       { name: 'IssNoteId', index: 'IssNoteId', hidden: true}],
                        pager: MatIssueListPager,
                        rowNum: '5',
                        rowList: [5, 10, 20, 50],
                        sortname: 'Id',
                        sortorder: "Asc",
                        height: '130',
                        loadComplete: function () {
                            var table = this;
                            setTimeout(function () {
                                updatePagerIcons(table);
                                enableTooltips(table);
                            }, 0);
                        },
                        autowidth: true,
                        viewrecords: true
                    });
                    jQuery("#" + MatIssueListTable).jqGrid('navGrid', "#" + MatIssueListPager, { edit: false, add: false, del: false, search: false, searchicon: 'ace-icon fa fa-search orange', refresh: true, refreshicon: 'ace-icon fa fa-refresh green' })
                }
            });
            jQuery("#" + MatReqListTable).jqGrid('navGrid', "#" + MatReqListPager, { edit: false, add: false, del: false, search: false, searchicon: 'ace-icon fa fa-search orange', refresh: true, refreshicon: 'ace-icon fa fa-refresh green' })
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
    }, {}, {}, {}, {})

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#Search").click(function () {
        $(grid_selector).clearGridData();
        var ReqNum = $("#RequestNumber").val();
        var ReqstDate = $("#RequestedDate").val();
        var cam = $("#Campus").val();
        var ReqFor = $("#RequiredFor").val();
        var Mat = $("#Material").val();
        var ReqrdDate = $("#RequiredDate").val();
        var status = $("#ddlStatus").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialRequestListJqGrid',
                    postData: { ReqNum: ReqNum, ReqstDate: ReqstDate, cam: cam, ReqFor: ReqFor, Mat: Mat, ReqrdDate: ReqrdDate, status: status },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#reset").click(function () {
        url = $('#BackUrl').val();
        window.location.href = url;
    });

    $("#NewRequest").click(function () {
        //window.location.href = '@Url.Action("MaterialRequest", "Store")';
        url = $('#NewMRQUrl').val();
        window.location.href = url;
    });

    $("#btnCreateIssueNote").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (GridIdList != '') {
            window.location.href = '/Store/MaterialIssueNote?Id=' + GridIdList;
        }
        else {
            ErrMsg("Please select Request Number.");
            return false;
        }
    });

});


function formateadorLink(cellvalue, options, rowObject) {
    var status = $("#ddlStatus").val();
    if (status == "Available" && rowObject[5] == "CreateMatRequest") {
        return "<a href=/Store/MaterialRequest?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
    else if ((status == "Available" || status == "Assigned") && rowObject[5] == "IssueMatRequest") {
        return "<a href=/Store/MaterialIssueNote?Id=" + rowObject[0] + "&activityId=" + rowObject[7] + ">" + cellvalue + "</a>";
    }
    else if (status == "Available" || status == "Assigned") {
        return "<a href=/Store/ActOnMaterialRequest?id=" + rowObject[0] + "&activityId=" + rowObject[7] + "&activityName=" + rowObject[8] + "&activityFullName=" + rowObject[9] + ">" + cellvalue + "</a>";
    }
    else if (status == "Sent" || status == "Completed") {
        return "<a href=/Store/ShowMaterialRequest?id=" + rowObject[0] + "&activityId=" + rowObject[7] + "&activityName=" + rowObject[8] + "&activityFullName=" + rowObject[9] + ">" + cellvalue + "</a>";
    }
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

function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}
function statusimage(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseInt(cellvalue);
    var mrl = $(grid_selector).jqGrid();
    for (i = 0; i < mrl.length; i++) {

        if (rowObject[6] != "") {
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

function ShowIssueNote(cellvalue, options, rowObject) {
    return "<a href=/Store/ShowMaterialIssueNote?Id=" + rowObject[7] + ">" + cellvalue + "</a>";
}

