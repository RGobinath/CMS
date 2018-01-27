$(function () {

    var grid_selector = "#TicketSystemList";
    var pager_selector = "#TicketSystemPage";

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
    function toProperCase(str) {
        return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
    }

    if ($.trim($("#AlrtDskMsg").val()) != "") {
        InfoMsg($("#AlrtDskMsg").val());
    }

    if ($('#isCreator').val() == true || $('#isCreator').val() == 'True') {
        $('#btnNewTicket').show();
    } else {
        $('#btnNewTicket').hide();
    }

    function buildDataforDrpDwn(VwData) {
        //
        //var objVwData=;
        var SrlzdData = VwData;
        var SlctData = ":All;";
        for (var i = 0, l = SrlzdData.length; i < l; i++) {
            SlctData += SrlzdData[i].Text + ":" + toProperCase(SrlzdData[i].Text) + ";";
        }
        return SlctData.substring(0, SlctData.length - 1);
    }

    //activityName,status,ActivityFullName
    function formateadorLink(cellvalue, options, rowObject) {
        return "<a href=/TicketingSystem/TicketingSystem?Id=" + rowObject[1] + "&ActivityId=" + rowObject[0] + "&activityName=" + rowObject[3] + "&status=" + rowObject[13] + "&ActivityFullName=" + rowObject[4] + ">" + cellvalue + "</a>";
    }

    //view-source:http://www.ok-soft-gmbh.com/jqGrid/ToolbarSearchValidation.htm
    $(grid_selector).jqGrid({
        mtype: 'GET',
        url: "/TicketingSystem/GetTicketingSystemInbox",
        datatype: 'json',
        height: '250',
        autowidth: true,
        //shrinkToFit: false,
        colNames: ['Id', 'TSId', 'Ticket No', 'Activity Name', 'Activity Name', 'Module', 'Ticket Type', 'Severity', 'Priority', 'Ticket Status',
				'Reporter', 'Created Date', 'Assigned To', 'Status',
				'History',
				'SLA', 'Description'],
        colModel: [
				{ name: 'Id', index: 'Id', hidden: true, key: true }, //0
				{name: 'TSId', index: 'TSId', hidden: true }, //1
				{name: 'TicketNo', index: 'TicketNo', formatter: formateadorLink, cellattr: function (rowId, val, rawObject) { return 'title="' + rawObject[16] + '"' } }, //2
				{name: 'ActivityName', index: 'ActivityName', hidden: true }, //3
				{name: 'ActivityFullName', index: 'ActivityFullName' }, //4
                {name: 'Module', index: 'Module', editable: true, stype: 'select', searchoptions: { dataUrl: '/TicketingSystem/FillModule'} },
				{ name: 'TicketType', index: 'TicketType', editable: true, stype: 'select', searchoptions: { dataUrl: '/TicketingSystem/FillTicketType'} },
                { name: 'Severity', index: 'Severity', editable: true, stype: 'select', searchoptions: { dataUrl: '/TicketingSystem/FillSeverity' }, hidden: true },
				{ name: 'Priority', index: 'Priority', editable: true, stype: 'select', searchoptions: { dataUrl: '/TicketingSystem/FillPriority'} },
				{ name: 'TicketStatus', index: 'TicketStatus', editable: true, stype: 'select', searchoptions: { dataUrl: '/TicketingSystem/FillTicketStatus'} },
				{ name: 'Reporter', index: 'Reporter' }, //10
                {name: 'CreatedDate', index: 'CreatedDate' },
				{ name: 'AssignedTo', index: 'AssignedTo', hidden: true }, //12
                //Message Sending Failure:Message Sending Failure;Message Sent:Message Sent;Suspended:Suspended"} },
				{name: 'Status', index: 'Status', stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: "Available:Available;Assigned:Assigned;Sent:Sent;Completed:Completed"} },
				{ name: 'History', index: 'History', align: 'center', sortable: false }, //14
				{name: 'SLA', index: 'SLA', align: 'center', sortable: false, formatter: statusimage }, //15
				{name: 'Description', index: 'Description', hidden: true}//16
				],
        pager: pager_selector,
        rowNum: '10',
        sortname: 'Id',
        sortorder: 'Desc',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        multiselect: true,
        viewrecords: true,
        caption: "<i class='ace-icon fa fa-ticket'></i>&nbsp;&nbsp;Ticketing Details",
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });

    function statusimage(cellvalue, options, rowObject) {
        //
        var i;
        var cellValueInt = parseInt(cellvalue);
        var cml = $(grid_selector).jqGrid();
        for (i = 0; i < cml.length; i++) {
            if (rowObject[7] != "") {
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
        }
    }

    $("#btnNewTicket").click(function () {
        window.location.href = "/TicketingSystem/TicketingSystem/";
    });
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
            {}, {}, {})

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
    //$(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: false });
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "Export To Excel",
        onClickButton: function () {
            window.open("GetTicketingSystemInbox" + '?rows=9999' +
				'&page=1' +
                '&sidx=TicketNo' +
                '&sord=desc' +
                '&TicketStatus=' + $('#gs_TicketStatus').val() +
                '&fromDat=' + $('#gs_CreatedDate').val() +
                '&TicketNo=' + $('#gs_TicketNo').val() +
                '&Module=' + $('#gs_Module').val() +
                '&TicketType=' + $('#gs_TicketType').val() +
                '&Severity=' + $('#gs_Severity').val() +
                '&Priority=' + $('#gs_Priority').val() +
                '&Reporter=' + $('#gs_Reporter').val() +
                '&AssignedTo=' + $('#gs_AssignedTo').val() +
                '&ActivityFullName=' + $('#gs_ActivityFullName').val() +
                '&Status=' + $('#gs_Status').val() +
                '&ExportToXL=true'
                );
        }
    });
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(grid_selector).clearGridData();
        return false;
    }
    });
});
    function ShowComments(ActivityId) {
        modalid = $('#Activities');
        //        LoadSetGridParam($('#ActivitiesHstryList'), "/TicketingSystem/ActivitiesListJqGrid?Id=" + ActivityId);
        ModifiedLoadPopupDynamicaly("/TicketingSystem/LoadUserControl/Activities", modalid, function () {
            LoadSetGridParam($('#ActivitiesHstryList'), "/TicketingSystem/ActivitiesListJqGrid?Id=" + ActivityId);
        }, function () { }, 700, 474, "Activity History");
    }

