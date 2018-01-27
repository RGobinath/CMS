//for adding Staff

jQuery(function ($) {
    var grid_selector = "#TTStaff";
    var pager_selector = "#TTStaffPager";
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
        url: '/TimeTable/StaffDetailsJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Staff Name'],
        colModel: [
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'Campus', index: 'Campus' },
            // { name: 'Grade', index: 'Grade' },
             { name: 'StaffName', index: 'StaffName', search: false, sortable: false },
           //  { name: 'StaffDetails', index: 'StaffDetails', search: false, sortable: false},
        ],
        pager: '#TTStaffPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'desc',
        width: 840,
        viewrecords: true,
        altRows: true,
        select: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: 'Add Staff'
    });
    $.getJSON("/Base/FillBranchCode",
function (fillig) {
    var ddlcam = $("#dllCampus");
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
 

   


    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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
    function styleCheckbox(table) { }

    function updateActionIcons(table) { }
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
});

function AddStaff() {
    debugger;
    var Campus = $('#dllCampus').val();
    var StaffName = $('#TTStaffName').val();
    if (Campus == '' || StaffName == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (StaffName == 'English' || StaffName == 'Maths' || StaffName == 'Biology' || StaffName == 'Chemistry' || StaffName == 'FA' || StaffName == 'Physics') {
        ErrMsg("Please Enter Staff Name insted of Subject Name.");
        $('#TTStaffName').val('');
        return false;
    }

    $.ajax({
        type: 'POST',
        url: "/TimeTable/AddStaffs",
        data: { Campus: Campus, StaffName: StaffName },
        success: function (data) {
            $("#TTStaff").trigger('reloadGrid');
            $('#dllCampus').val('');
            $('#TTStaffName').val('');
        }
    });
}


function Activities() {
    ModifiedLoadPopupDynamicaly("/TimeTable/StaffSubjectDetails", $('#Activities1'),
            function () {
                LoadSetGridParam($('#ActivitiesList'), "/TimeTable/StaffLessonsJqGrid")
            }, function () { }, 925, 410, "ActivitiesList");
}


$("#btnAddSub").click(function () {
    ModifiedLoadPopupDynamicaly("/TimeTable/AddLessonsToStaffs", $('#AddLessons'),
        function () {
            LoadSetGridParam($('#AddLessons'))
        }, function () { }, 850, 335, "Add Lessons");

});

$("#btnAddLess").click(function () {
    ModifiedLoadPopupDynamicaly("/TimeTable/AddLessonsToStaffs", $('#SubjLessons'),
        function () {
            LoadSetGridParam($('#AddLessons'))
        }, function () { }, 850, 335, "Add Lessons");

});



