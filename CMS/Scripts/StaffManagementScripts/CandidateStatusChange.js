//--Grid Loading
jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/CandidateStatusJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'PreRegNum', 'Name', 'Gender', 'Age','Campus' ,'Applied For', 'Qualification', 'Tot.Yrs Experience', 'Tot.Yrs Teaching Experience', 'Subjects Taught', 'Grades Taught', 'Last CTC', 'Expected CTC', 'Status',' '],
        colModel: [
                                  { name: 'Id', index: 'Id', key: true, hidden: true },
                                  { name: 'PreRegNum', index: 'NewId', sortable: true, hidden: true },
                                  //{ name: 'Name', index: 'Name', sortable: true, width: 300},
                                  { name: 'Name', index: 'Name', sortable: true, width: 250, formatter: Candidateformatterlink},
                                  { name: 'Gender', index: 'Gender', sortable: true, width: 80 },
                                  { name: 'Age', index: 'Age', sortable: true, width: 60, align: 'Right' },
                                  { name: 'Campus', index: 'Campus', sortable: true, hidden: true },
                                  { name: 'AppliedFor', index: 'AppliedFor', sortable: true },
                                  { name: 'Qualification', index: 'Qualification', sortable: true },
                                  { name: 'TotalYearsOfExp', index: 'TotalYearsOfExp', sortable: true },
                                  { name: 'TotalYearsOfTeachingExp', index: 'TotalYearsOfTeachingExp', sortable: true },
                                  { name: 'SubjectsTaught', index: 'SubjectsTaught', sortable: true },
                                  { name: 'GradesTaught', index: 'GradesTaught', sortable: true, hidden: true },
                                  { name: 'LastDrawnGrossSalary', index: 'LastDrawnGrossSalary', sortable: true, align: 'Right' },
                                  { name: 'ExpectedSalary', index: 'ExpectedSalary', sortable: true, align: 'Right' },
                                  { name: 'Status', index: 'Status', sortable: true },
                                  { name: 'Action', index: 'Action', formatter: StatusChangeformatterlink },
        ],

        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Candidate Details",
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
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
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {});
    //$("#grid-table").navButtonAdd('grid-pager', {
    //    caption: "Update",
    //    title: "Click here to Change Candidate Status",
    //    //buttonicon: "ace-icon fa fa-plus-circle purple",
    //    //buttonicon: "ace-icon fa fa-search white",
    //    onClickButton: function () {
    //    },
    //    position: "first"
    //});
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
    $("#btnSearch").click(function () {
        var lCampus = $("#ddlCampus").val();
        var lInterviewDate = $("txtDate").val();
        var lStatus = $("#ddlStatus").val();
        $(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/CandidateStatusJqGrid',
           postData: { InterviewCampus: lCampus, InterviewDate: lInterviewDate, Status: lStatus },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        var lCampus = $("#ddlCampus").val();
        var lInterviewDate = $("txtDate").val();
        var lStatus = $("#ddlStatus").val();
        $(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/CandidateStatusJqGrid',
           postData: { InterviewCampus: lCampus, InterviewDate: lInterviewDate, Status: lStatus },
           page: 1
       }).trigger("reloadGrid");
    });
});

function Candidateformatterlink(cellvalue, options, rowObject) {
        return "<a style='text-decoration:underline;cursor:pointer;' onclick=ShowCandidatePopup('" + rowObject[0] + "','" + options.colModel.index + "');>" + cellvalue + "</a>";
}
function ShowCandidatePopup(Id) {
    ModifiedLoadPopupDynamicaly("/StaffManagement/CandidateDetailsProfileView?Id=" + Id, $('#CandidateDtls'),
               function () { }, function () { }, 920, 1200, "Candidate Details - View");
}
function StatusChangeformatterlink(cellvalue, options, rowObject)
{
    if (rowObject[14] != "On Board")
        return "<button class='btn btn-success btn-info btn-block btn-sm' id='btnRegister' , disabled = 'disabled'>Register</button>";
    else
        return "<button class='btn btn-success btn-info btn-block btn-sm' id='btnRegister' onclick=StatusOnboardChange('" + rowObject[0] + "');><i class='fa fa-check-square-o'></i> Register</button>";
}

function StatusOnboardChange(Id)
{
    ModifiedLoadPopupDynamicaly("/StaffManagement/CandidateRegistration?Id="+Id, $('#CandidateOnboardDtls'), function () { }, function () { }, 400, 480, "Candidate for Staff Registration");

}

