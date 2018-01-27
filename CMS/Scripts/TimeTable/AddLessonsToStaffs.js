jQuery(function ($) {
    var grid_selector = "#AddLessons";
    var pager_selector = "#AddLessonsPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#TTStaff").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#TTStaff").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/TimeTable/StaffLessonsJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Staff Name', 'Grade', 'Section','Subject', 'TotalHours', 'ClassContinuity'],
        colModel: [
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'Campus', index: 'Campus' },
             { name: 'StaffName', index: 'StaffName', search: false, sortable: false },
             { name: 'Grade', index: 'Grade' },
             { name: 'Section', index: 'Section' },
             { name: 'Subject', index: 'Subject', search: false, sortable: false },
              { name: 'TotalHours', index: 'TotalHours', search: false, sortable: false },
                { name: 'ClassContinuity', index: 'ClassContinuity', search: false, sortable: false },
        ],
        pager: '#AddLessonsPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'desc',
        width: 1480,
        viewrecords: true,
        altRows: true,
        select: true,
        viewrecords: true,
        caption: 'Add Class Details'
    });

    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {}, {}, {}, {}, {})

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

    $.getJSON("/Base/FillBranchCode",
function (fillig) {
    var ddlcam = $("#LessCampus");
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
    $("#LessCampus").change(function () {
        debugger;
        gradeddl();
        staffddl();
        subjectddl();
    });

    function gradeddl() {

        var campus = $("#LessCampus").val();
        $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#LessGrade");
                select.empty();
                select.append($('<option/>',
            {
                value: "", text: "Select Grade"
            }));
                $.each(modelData, function (index, itemData) {

                    select.append($('<option/>',
            {
                value: itemData.gradcod,
                text: itemData.gradcod
            }));
                });
            });
    }

    function staffddl() {
        var campus = $("#LessCampus").val();
        $.getJSON("/TimeTable/FillStaffByCampus/", { campus: campus },
    function (modelData) {
        var select = $("#LessStaffName");
        select.empty();
        select.append($('<option/>'
    , {
        value: "",
        text: "Select Staff"
    }));
        $.each(modelData, function (index, itemData) {

            select.append($('<option/>',
    {
        value: itemData.Value,
        text: itemData.Text
    }));
        });
    });

    }


    function subjectddl() {
        var campus = $("#LessCampus").val();
        $.getJSON("/TimeTable/FillSubjectByCampus/", { campus: campus },
    function (modelData) {
        var select = $("#LessSubject");
        select.empty();
        select.append($('<option/>'
    , {
        value: "",
        text: "Select Subject"
    }));
        $.each(modelData, function (index, itemData) {

            select.append($('<option/>',
    {
        value: itemData.Value,
        text: itemData.Text
    }));
        });
    });

    }

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
});

function SaveSubjectDetails() {
    debugger;
    var Grade = $('#LessGrade').val();
    var Section = $('#LessSection').val();
    var Campus = $('#LessCampus').val();
    var StaffName = $('#LessStaffName').val();
    var Subject = $('#LessSubject').val();
    var Lssweek = $('#Lssweek').val();
    var LssTotalHours = $('#LssTotalHours').val();
    
    $.ajax({
        type: 'POST',
        url: "/TimeTable/AddStaffsandSubjects",
        data: { Class: Grade, Campus: Campus, StaffName: StaffName, Section: Section, Subject: Subject, LessonsPerWeek: Lssweek, ClassContinueity: LssTotalHours },
        success: function (data) {
            $("#AddLessons").trigger('reloadGrid');
            $('#LessGrade').val('');
            $('#LessSection').val('');
            $('#LessCampus').val('');
            $('#LessStaffName').val('');
            $('#Lssweek').val('');
            $('#LessSubject').val('');
            $('#LssTotalHours').val('');
            //  $("#AddLessons").hide();
        }
    });

}