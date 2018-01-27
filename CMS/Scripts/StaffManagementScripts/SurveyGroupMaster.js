$(function () {
    var grid_selector = "#SurveyGroupJMasterqgrid";
    var pager_selector = "#SurveyGroupJMasterqgridPager";

    //resize to fit page size
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
        url: '/StaffManagement/SurveyGroupJMasterqgrid',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Survey Group Id', 'Survey Group', 'Is Active', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'SurveyGroupId', index: 'SurveyGroupId', key: true, hidden: true, editable: true },
                      { name: 'SurveyGroup', index: 'SurveyGroup', editable: true },
                      { name: 'IsActive', index: 'IsActive', editable: false, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        sortname: 'SurveyGroupId',
        sortorder: 'Desc',
        altRows: false,
        multiselect: true,
        multiboxonly: true,
        autowidth: true,
        //shrinkToFit:true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Student Survey Group",
    })
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size            
    //navButtons
    $(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
            {
                url: '/StaffManagement/AddOrEditSurveyGroupMaster', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                url: '/StaffManagement/AddOrEditSurveyGroupMaster', closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add

		{ url: '/StaffManagement/DeleteSurveyGroupMaster' },
               {},
                {})

    //$(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });

    $("#btnReset").click(function () {
        //$("input[type=text], textarea, select").val("");
        $("#txtStudentSurveyGroup").val('');


    });
    $("#SrchbtnReset").click(function () {
        $("#SrcStudentSurveyGroup").val('');
        $("#ddlIsActive").val('');

        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/SurveyGroupJMasterqgrid',
           postData: { SurveyGroup: "", IsActive: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        debugger;
        $(grid_selector).clearGridData();
        var SurveyGroup = $("#SrcStudentSurveyGroup").val();
        var IsActive = $("#ddlIsActive").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/SurveyGroupJMasterqgrid',
           postData: { SurveyGroup: SurveyGroup, IsActive: IsActive },
           page: 1
       }).trigger("reloadGrid");
    });
    //$('#btnSave').click(function () {
    //    debugger;
    //    var SurveyGroup = $("#txtStudentSurveyGroup").val();
    //    if (SurveyGroup == "" || SurveyGroup == null) {
    //        ErrMsg("Please Fill the Required Details");
    //        return false;
    //    }
    //    $.ajax({
    //        type: 'POST',
    //        datatype: 'json',
    //        url: '/StaffManagement/AddStudentSurveyGroup?SurveyGroup=' + SurveyGroup,
    //        success: function (data) {
    //            $(grid_selector).trigger('reloadGrid');
    //            $("input[type=text], textarea").val("");
    //            if (data == "success") {
    //                SucessMsg("Added Successfully");

    //            }
    //            else {
    //                ErrMsg("Already Exist");
    //            }

    //        }
    //    });

    //});
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});

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