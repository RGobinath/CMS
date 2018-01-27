$(function () {
    var grid_selector = "#EventMasterApproveJqgrid";
    var pager_selector = "#EventMasterApproveJqgridPager";
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                var page_width = $(".tab-content").width();
                $(grid_selector).jqGrid('setGridWidth', page_width);
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/Asset/EventListDetailsListJqGrid?PageName=Approve',
        datatype: 'json',
        height: 265,
        autowidth: true,
        colNames: ['Id', 'Request No', 'Campus', 'Hall', 'Booked on', 'Start Time', 'End Time', 'Event Description', 'Status', 'Created Date', 'Created By', 'CreatedBy'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
        { name: 'RequestNo', index: 'RequestNo', search: false },
             { name: 'Campus', index: 'Campus' },
             { name: 'AssetName', index: 'AssetName' },
             { name: 'Date', index: 'Date', search: false },
             { name: 'StartTimeString', index: 'StartTimeString', search: false },
             { name: 'EndTimeString', index: 'EndTimeString', search: false },
             { name: 'ReasonForBooking', index: 'ReasonForBooking' },
             { name: 'status', index: 'status' },
             { name: 'CreatedDate', index: 'CreatedDate' },
             { name: 'CreatedName', index: 'CreatedName' },
             { name: 'CreatedBy', index: 'CreatedBy',hidden:true },
        ],
        viewrecords: true,
        rowNum: 8,
        //width:500,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Event History"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    //navButtons
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
            }, {},
             {}, {}, {})

    $("#srhDate").datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true
    });
    $('#btncnfmSearch').click(function () {
        var srhCampus = $("#srhCampusddl").val();
        var srhHall = $("#srhAssetddl").val();
        var srhdate = $("#srhDate").val();
        var sts = $('#srhstatus').val();
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: "/Asset/EventListDetailsListJqGrid?PageName=Approve",
                postData: { Campus: srhCampus, Status: sts, AssetName: srhHall, Date: srhdate },
                page: 1
            }).trigger("reloadGrid");
        //loadgrid();
    });
    $('#btncnfmReset').click(function () {
        $("input[type=text], textarea, select").val(""); $(grid_selector).clearGridData();
        var srhCampus = $("#srhCampusddl").val();
        var srhHall = $("#srhAssetddl").val();
        var srhdate = $("#srhDate").val();
        var sts = $('#srhstatus').val();
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: "/Asset/EventListDetailsListJqGrid?PageName=Approve",
                postData: { Campus: srhCampus, Status: sts, AssetName: srhHall, Date: srhdate },
                page: 1
            }).trigger("reloadGrid");
    });
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
//For pager Icons
function styleCheckbox(table) {
}
function updateActionIcons(table) {
}
//Nav Grid Icons Tool Tip
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}
function getAssetdata(id) {
    ModifiedLoadPopupDynamicaly("/Asset/ApproveAssetEvent?Id=" + id, $('#ApproveDiv'), function () { LoadSetGridParam($('#EventMasterApproveJqgrid'), "/Asset/EventListDetailsListJqGrid?PageName=Approve") }, function () { }, 800, 250, "Approve Booking Request");
}


//Create new event

//function NewEvent() {
//    debugger;
//    var Campus = $('#campusddl').val();
//    var AssetName = $('#Assetddl').val();
//    var Date = $('#txtDate').val();
//    //var txtDate = DateChangeDetails($("#txtDate"));
//    //alert(Date);
//    var FromTime = $('#FrmTime').val();
//    var ToTime = $('#ToTime').val();
//    var ReasonForBooking = $('#EventDescrip').val();
//    if (Campus == '' || AssetName == '' || Date == '' || FromTime == '' || ToTime == '' || ReasonForBooking == '') {
//        ErrMsg("Please fill all the mandatory fields.");
//        return false;
//    }
//    function DateChangeDetails(cellvalue) {
//        //debugger;
//        var Date = cellvalue.split(" ");
//        var dd = Date[0];
//        var time = Date[1];
//        var value = Date[0].split("/");
//        var dd = value[0];
//        var mm = value[1];
//        var yy = value[2];
//        var resultField = mm + "/" + dd + "/" + yy + "";
//        return resultField;

//    }

//    $.ajax({
//        type: 'POST',
//        url: "/Asset/AddEventList",
//        data: { Campus: Campus, AssetName: AssetName, Date: Date, FromTime: FromTime, ToTime: ToTime, ReasonForBooking: ReasonForBooking },
//        success: function (data) {
//            if (data = "Success") {
//                $("#EventMasterJqgrid").trigger('reloadGrid');
//                $("input[type=text], textarea, select").val("");
//                InfoMsg("Your Process has been Initiated Successfully!...");
//                return true;
//            }
//        }
//    });
//}

