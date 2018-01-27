$(function () {
    var grid_selector = "#IssueNoteList";
    var pager_selector = "#IssueNoteListPager";
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    $.getJSON("/Base/FillBranchCode",
             function (fillig) {
                 var ddlcam = $("#ddlCampus");
                 ddlcam.empty();
                 ddlcam.append($('<option/>', { value: "", text: "Select One" }));

                 $.each(fillig, function (index, itemdata) {
                     ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                 });
             });

    $("#Search").click(function () {
        $(grid_selector).clearGridData();
        var RequiredForCampus = $("#ddlCampus").val();
        var IssNoteNumber = $("#IssueNoteNumber").val();
        var DeliveredThrough = $("#DeliveredThrough").val();
        var DeliveryDate = $("#DeliveryDate").val();
        $(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: '/Store/IssueNoteListJqGrid',
                            postData: { RequiredCampus: RequiredForCampus, IssNoteNo: IssNoteNumber, DeliverThrough: DeliveredThrough, DeliverDate: DeliveryDate },
                            page: 1
                        }).trigger("reloadGrid");
    });
    debugger;
    jQuery(grid_selector).jqGrid({
        url: '/Store/IssueNoteListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['IssNoteId', 'Issue Note Number', 'Processed By', 'Required For Campus', 'Required For Store', 'Required From Store', 'Requested Date', 'Issue Date', 'Issued By', 'Request Status', 'Delivered Through', 'Delivery Details', 'Delivery Date'],
        colModel: [
              { name: 'IssNoteId', index: 'IssNoteId', hidden: true, key: true },
              { name: 'IssNoteNumber', index: 'IssNoteNumber', formatter: formateadorLink },
              { name: 'ProcessedBy', index: 'ProcessedBy' },
              { name: 'RequiredForCampus', index: 'RequiredForCampus' },
              { name: 'RequiredForStore', index: 'RequiredForStore' },
              { name: 'RequiredFromStore', index: 'RequiredFromStore' },
              { name: 'RequestedDate', index: 'RequestedDate', width: 90, search: false, sortable: true, hidden: false },
              { name: 'IssueDate', index: 'IssueDate', width: 90, search: false, sortable: true, hidden: false },
              { name: 'IssuedBy', index: 'IssuedBy', width: 90, sortable: true },
              { name: 'RequestStatus', index: 'RequestStatus', width: 90, sortable: true },
              { name: 'DeliveredThrough', index: 'DeliveredThrough', width: 90, sortable: true },
              { name: 'DeliveryDetails', index: 'DeliveryDetails', width: 90, sortable: true },
              { name: 'DeliveryDate', index: 'DeliveryDate', width: 90, search: false, sortable: true }
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'IssNoteId',
        sortorder: 'Desc',
        height: '230',
        autowidth: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-th-list'></i>&nbsp;Issue Note List"
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
            {}, {}, {})
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });

});

function updatePagerIcons(table) {
    var replacement = {
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
function btnBack() {
    var url = $('#BackUrl').val();
    window.location.href = url;
}
$('#reset').click(function () {
    var url = $('#BackUrl1').val();
    window.location.href = url;
});
function formateadorLink(cellvalue, options, rowObject) {
    return "<a href=/Store/ShowMaterialIssueNote?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
}