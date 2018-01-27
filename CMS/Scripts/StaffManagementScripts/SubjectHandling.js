$(function () {
    var StaffPreRegNumber = $('#StaffPreRegNumber').val();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery('#ShowCampusBasedStaffDetailsGrid').jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $('#ShowCampusBasedStaffDetailsGrid').closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery('#ShowCampusBasedStaffDetailsGrid').jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery('#ShowCampusBasedStaffDetailsGrid').jqGrid({
        url: '/StaffManagement/CampusBasedStaffDetailsJqGrid?StaffPreRegNumber=' + StaffPreRegNumber,
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Staff PreReg Number', 'Campus', 'Grade', 'Section', 'Subject'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true, editable: true, key: true },
                      { name: 'StaffPreRegNumber', index: 'StaffPreRegNumber', hidden: true, editable: true },
                      { name: 'Campus', index: 'Campus', hidden: false, editable: true },
                      { name: 'Grade', index: 'Grade', editable: true },
                      { name: 'Section', index: 'Section', editable: true },
                      { name: 'Subject', index: 'Subject', editable: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#ShowCampusBasedStaffDetailsGridPager',
        altRows: true,
        sortorder: "Asc",
        //autowidth: true,
        width: 1000,
        //shrinkToFit: false,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                //styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Subject Handled Details",
    })
    //navButtons
    $('#ShowCampusBasedStaffDetailsGrid').jqGrid('navGrid', '#ShowCampusBasedStaffDetailsGridPager',
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
            }, {}, {}, {}, {}, {});
});