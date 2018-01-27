jQuery(function ($) {
    $.getJSON("/StaffIssues/FillWorkFlowStatus",
     function (fillig) {

         var ddlwfs = $("#statussearchddl");
         ddlwfs.empty();
         ddlwfs.append($('<option/>',{value: "",text: "Select One"}));

         $.each(fillig, function (index, itemdata) {

             ddlwfs.append($('<option/>',{value: itemdata.Value,text: itemdata.Text}));
});

     });

    $.getJSON("/Base/FillBranchCode",
     function (fillig) {
         var ddlcam = $("#ddlCampus");
         ddlcam.empty();
         ddlcam.append($('<option/>',{value: "",text: "Select One"}));

         $.each(fillig, function (index, itemdata) {
             ddlcam.append($('<option/>',{value: itemdata.Value,text: itemdata.Text}));
         });
     });
    var column2 = "Region";
    var column3 = "Action";
    var StatusStr = ":All;1:LogIssue;2:ResolveIssue;3:ApproveIssue;4:Complete;5:ResolveIssueRejection;6:ApproveIssueRejection;7:Information";
    var stat = "";
    var ddlcampus = "";
    var grid_selector = "#grid-list4";
    var pager_selector = "#grid-pager";
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

            url: '/StaffIssues/StaffIssueStatusjqgrid',
            mtype: 'GET',
            postData: { txtSearch: stat, ddlcampus: ddlcampus },
            datatype: 'json',
            height: '230',
            colNames: ['Id', 'Support Number', 'Campus', 'Request Date', 'Issue Group', 'Issue Type', 'Created By', 'Status', 'History', 'SLA Status'],
            colModel: [
            // if any column added in future have to check rowObject for SLA status image.... 
                          {name: 'Id', index: 'Id', hidden: true },
                          { name: 'IssueNumber', index: 'IssueNumber', align: 'left', resizable: true, editable: false, search: true, sortable: true },
                          { name: 'BranchCode', index: 'BranchCode', align: 'left', resizable: true, editable: false, search: true, sortable: true },
                          { name: 'IssueDate', index: 'IssueDate', align: 'left', resizable: true, editable: false, search: false, sortable: true },
                          { name: 'IssueGroup', index: 'IssueGroup', align: 'left', resizable: true, editable: false, search: true, sortable: true },
                          { name: 'IssueType', index: 'IssueType', align: 'left', resizable: true, editable: false, search: true, sortable: true },
                          { name: 'UserInbox', index: 'UserInbox', align: 'left', resizable: true, editable: false, search: true, sortable: true },
                          { name: 'Status', index: 'Status', align: 'left', resizable: true, editable: false, search: true, stype: 'select', sortable: true },
                          { name: 'history', index: 'history', align: 'center', resizable: true, editable: false, search: false, sortable: false },
                          { name: 'colorstat', index: 'colorstat', align: 'center', formatter: statusimage, resizable: true, editable: false, search: false, sortable: false }
                          ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            sortname: 'Id',
            sortorder: "Desc",
            viewrecords: true,
            altRows: true,
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            },
            caption: 'Status Report'
        });
        $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
        //navButtons
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
            }, {}, {}, {}, {})

        function statusimage(cellvalue, options, rowObject) {
            var i;
            var cellValueInt = parseInt(cellvalue);
            var StsRpt = $(grid_selector).jqGrid();
            for (i = 0; i < StsRpt.length; i++) {

                if (cellValueInt <= 24) {
                    //alert('y');
                    return '<img src="../../Images/yellow.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
                }
                else if (cellValueInt > 24 && cellValueInt <= 48) {
                    //alert('o');
                    return '<img src="../../Images/orange.jpg" height="10px" width="10px"  alt=' + cellvalue + ' title=' + cellvalue + ' />'
                }
                else if (cellValueInt > 48) {
                    return '<img src="../../Images/redblink3.gif" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
                }
                else if (cellvalue == 'Completed') {
                    return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
                }

                else {
                    return null;
                }
            }
        }
    }

    window.onload = loadgrid;
    $("#statussearchddl").change(function () {
        $("#search").click();
    });
    $("#ddlCampus").change(function () {
        $("#search").click();
    });
    $("#search").click(function () {
        $(grid_selector).GridUnload();
        var e = document.getElementById("statussearchddl");
        stat = e.options[e.selectedIndex].value;
        ddlcampus = $("#ddlCampus").val();
        loadgrid();
    });

    $("#reset").click(function () {
        $(grid_selector).GridUnload();
        document.getElementById("statussearchddl").value = "";
        stat = "";
        loadgrid();
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});
function ShowComments(ActivityId) {

    modalid = $('#Activities');
    $('#ActivitiesList').clearGridData();
    ModifiedLoadPopupDynamicaly("/StaffIssues/LoadUserControl/Activities", modalid, function () {
        LoadSetGridParam($('#ActivitiesList'), "/StaffIssues/ActivitiesListJqGrid?Id=" + ActivityId)}, function () { },800,400,"History");
}