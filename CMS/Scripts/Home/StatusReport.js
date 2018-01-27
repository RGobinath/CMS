
var column2 = "Region";
var column3 = "Action";
var StatusStr = ":All;1:LogIssue;2:ResolveIssue;3:ApproveIssue;4:Complete;5:ResolveIssueRejection;6:ApproveIssueRejection;7:Information";
var stat = "";
var ddlcampus = "";
$(function () {
    var grid_selector = "#list4";
    var pager_selector = "#gridpager";

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
 
    function loadgrid() {
        jQuery(grid_selector).jqGrid({
            mtype: 'GET',
            url: "/Home/Statusjqgrid",
            postData: { txtSearch: stat, ddlcampus: ddlcampus, Name: Name, ddlgrade: ddlgrade, SupNum: SupNo },
            datatype: 'json',
            height: '230',
            autowidth: true,
            colNames: ['Id', 'Support Number', 'Campus', 'Student Name', 'Grade', 'Request Date', 'Information For', 'Leave Type', 'Issue Group', 'Issue Type', 'Created By', 'Status', 'History', 'SLA Status'],
            colModel: [
            // if any column added in future have to check rowObject for SLA status image.... 
                          {name: 'Id', index: 'Id', hidden: true },
                          { name: 'IssueNumber', index: 'IssueNumber', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                           { name: 'School', index: 'School', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'StudentName', index: 'StudentName', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'Grade', index: 'Grade', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'IssueDate', index: 'IssueDate', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'InformationFor', index: 'InformationFor', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'LeaveType', index: 'LeaveType', sortable: true },
                          { name: 'IssueGroup', index: 'IssueGroup', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'IssueType', index: 'IssueType', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'UserInbox', index: 'UserInbox', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'Status', index: 'Status', resizable: true, editable: false, search: true, stype: 'select', searchoptions: { sopt: ['eq', 'ne'], value: StatusStr }, sortable: true },
                          { name: 'history', index: 'history', align: 'center', resizable: true, editable: false, search: false, sortable: false },
                          { name: 'colorstat', index: 'colorstat', align: 'center', formatter: statusimage, resizable: true, editable: false, search: false, sortable: false }
                          ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            sortname: 'Id',
            sortorder: "Desc",
            viewrecords: true,
            caption: '<i class="ace-icon fa fa-list"></i>&nbsp;&nbsp;Status Report',
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(grid_selector).jqGrid('setGridWidth');
            }
        });
        $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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
            {}, {}, { caption: "Search...", top: '10%', height: '50%', width: 400, searchOnEnter: true })
        //            jQuery(grid_selector).navGrid(pager_selector, { search: true, add: false, edit: false, del: false },
        //        {},
        //        {},
        //        {},
        //        { caption: "Search...", top: '10%', height: '50%', width: 400, searchOnEnter: true }
        //             );

        function statusimage(cellvalue, options, rowObject) {
            var i;
            var cellValueInt = parseInt(cellvalue);
            var StsRpt = $(grid_selector).jqGrid();
            for (i = 0; i < StsRpt.length; i++) {
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
                else {
                    return null;
                }
            }
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

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    window.onload = loadgrid;

    $("#search").click(function () {
        $(grid_selector).GridUnload();
        var e = document.getElementById("statussearchddl");
        stat = e.options[e.selectedIndex].value;
        ddlcampus = $("#ddlcampus").val();
        Name = $("#Name").val();
        ddlgrade = $("#ddlgrade").val();
        SupNo = $("#SupNo").val();
        loadgrid();
    });

    $("#reset").click(function () {
        $("#ddlcampus").val("");
        $('#statussearchddl').val("");
        stat = "";
        ddlcampus = "";
        jQuery(grid_selector).jqGrid('clearGridData')
                .jqGrid('setGridParam', { data: data, page: 1 }).trigger('reloadGrid');
    });
});
$("#ddlcampus").change(function () {
    gradeddl($("#ddlcampus").val());
});
$(function () {
    $.getJSON("/Home/FillWorkFlowStatus",
     function (fillig) {

         var ddlwfs = $("#statussearchddl");
         ddlwfs.empty();
         ddlwfs.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

         $.each(fillig, function (index, itemdata) {

             ddlwfs.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));

         });

     });
});
$.getJSON("/Base/FillBranchCode",
     function (fillig) {
         var ddlcam = $("#ddlcampus");
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
function ShowComments(ActivityId) {
    

    ModifiedLoadPopupDynamicaly("/Home/Activities?Id=" + ActivityId, $('#Activities'),
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

function gradeddl(campus) {
    //var campus = $("#ddlCampus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
        function (modelData) {
            var grd = $("#ddlgrade");
            var select = $("#ddlGrade");
            grd.empty();
            select.empty();
            select.append($('<option/>', { value: "", text: "Select Grade" }));
            grd.append($('<option/>', { value: "", text: "Select Grade" }));
            $.each(modelData, function (index, itemData) {
                grd.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
            });
        });
}