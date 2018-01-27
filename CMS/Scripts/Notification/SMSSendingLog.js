$(function () {
    $('#Send').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });



    var grid_selector = "#jqBulkSMSRequest";
    var pager_selector = "#jqBulkSMSRequestPager";

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

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    $(grid_selector).jqGrid({
        url:"/Communication/JqGridSMSRequest",
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'SMS Request Id', 'Campus', 'Message Template', 'Message Template Value', 'Message', 'Father', 'Mother', 'Status', 'CreatedBy', 'ModifiedBy', 'CreatedDate', 'ModifiedDate', 'Report'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true },
            { name: 'SMSReqId', index: 'SMSReqId', width: 45 },
            { name: 'Campus', index: 'Campus', width: 60 },
            { name: 'SMSTemplate', index: 'SMSTemplate', width: 100 },
            { name: 'SMSTemplateValue', index: 'SMSTemplateValue', width: 100, hidden: true },
            { name: 'Message', index: 'Message', width: 150, search: false },
            { name: 'Father', index: 'Father', width: 30, hidden: true },
            { name: 'Mother', index: 'Mother', width: 30, hidden: true },
            { name: 'Status', index: 'Status', width: 80, stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: ":All;SMS Composed:SMS Composed;Recipients Added:Recipients Added;Message Sending Failure:Message Sending Failure;Message Sent:Message Sent;Suspended:Suspended"} },
            { name: 'CreatedBy', index: 'CreatedBy', width: 60, search: false },
            { name: 'ModifiedBy', index: 'ModifiedBy', width: 60, search: false, hidden: true },
            { name: 'CreatedDate', index: 'CreatedDate', width: 60, search: false },
            { name: 'ModifiedDate', index: 'ModifiedDate', width: 60, search: false, hidden: true },
            { name: 'Report', index: 'Report', width: 40, search: false, formatter: SmsReportShow, align: 'center' }
            ],
        loadComplete: function () {
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            $("tr.jqgrow:odd").addClass('RowBackGroundColor');
            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.Status == "SMS Composed") {
                    $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#66CCFF" });
                }
                else if (rowData.Status == "Recipients Added") {
                    $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#FFFF99" });
                }
                else if (rowData.Status == "Message Sent") {
                    $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#99FF66" });
                }
                else if (rowData.Status == "Suspended") {
                    $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#FF9900" });
                }
                else {
                    $(grid_selector).setCell(ids[i], "Status", "", { "background-color": "#99FF66" });
                }
            }
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 500],
        sortname: 'Id',
        sortorder: 'Asc',
        reloadAfterSubmit: true,
        autowidth: true,
        height: 250,
        viewrecords: true,
        caption: 'Bulk SMS Request'
    });
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, defaultSearch: "cn" });



});

function SmsReportShow(cellvalue, options, rowObject) {
    
    if (rowObject[7] == 'Message Sent' || rowObject[7] == 'Sending Failure') {
        return "<a href=/Communication/BulkSMSRequestReport?ComposeId=" + rowObject[0] + ">Report</a>";
    } else {
        return "...";
    }
    return;
}