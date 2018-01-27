var grid_selector = "#SMSLogGrid";
var pager_selector = "#SMSLogGridPager";

$(window).on('resize.jqGrid', function () {
    $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
})

//resize to fit page size

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
$(function () {

    $("#search").click(function () {
        var smssuccess = $('#txtSuccessNos').val();
        var smsfaild = $('#txtFaildNos').val();
        var message = $('#txtMessage').val();
        var date = $('#txtSmsDate').val();
        var ddlflag = $('#ddlflag').val();
        var name = $('#txtName').val();
        var newid = $('#txtNewId').val();

        $(grid_selector).setGridParam(
                             {
                                 datatype: "json",
                                 url: '@Url.Content("~/Admission/SMSLogGrid/")',
                                 type: 'POST',
                                 postData: { SuccessSMSNos: smssuccess, FailedSMSNos: smsfaild, Message: message, CreatedDate: date, Flag: ddlflag, StudName: name, NewId: newid },
                                 page: 1
                             }).trigger("reloadGrid");

    });
    //                         $(window).on('resize.jqGrid', function () {
    //                             $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    //                         })
                         $('#reset').click(function () {
                             var url = $('#BackUrl').val();
                             window.location.href = url;
                         });

    function loadgrid() {
        jQuery(grid_selector).jqGrid({
            mtype: 'GET',
            url: '/Admission/SMSLogGrid',
            datatype: 'json',
            colNames: ['Id', 'Student Name', 'New Id', 'SuccessSMSNos', 'FailedSMSNos', 'Message', 'SMSDateTime'],
            colModel: [
                { name: 'Id', index: 'Id', sortable: false, hidden: true },
                { name: 'StudName', index: 'StudName', width: 70, align: 'left', sortable: true },
                { name: 'NewId', index: 'NewId', width: 60, align: 'left', sortable: true },
                { name: 'SuccessSMSNos', index: 'SuccessSMSNos', width: 60, align: 'left', sortable: true },
                { name: 'FailedSMSNos', index: 'FailedSMSNos', width: 60, align: 'left', sortable: false },
                { name: 'Message', index: 'Message', width: 80, align: 'left', sortable: false },
                { name: 'CreatedDate', index: 'CreatedDate', width: 30, align: 'left', sortable: false }
                ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50],
            sortname: 'Id',
            sortorder: 'desc',
            viewrecords: true,
            multiselect: true,
            height: '220',
            autowidth: true,
            altRows: true,
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: '<i class="ace-icon fa fa-comment white"></i>&nbsp;&nbsp;SMS Log'
        });
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
            {}, {}, {});
    }
    window.onload = loadgrid();
});
//Pager icons
function styleCheckbox(table) {
}
function updateActionIcons(table) {
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