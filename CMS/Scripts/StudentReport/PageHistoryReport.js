//jQuery(function ($) {
debugger;
    jQuery("#PageHistoryReportGrid").jqGrid({
        url: '/StudentsReport/GetPageHistoryReportListJqGrid',
        datatype: "json",
        colNames: ['PageHistoryReport_Id', 'Campus', 'ControllerName', 'ControllerHit', 'ActionName', 'ActionHit', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
            { name: 'PageHistoryReport_Id', index: 'PageHistoryReport_Id', hidden: true, key: true },
            { name: 'Campus', index: 'Campus' },
            { name: 'ControllerName', index: 'ControllerName' },
            { name: 'ControllerHit', index: 'ControllerHit', hidden: true },
            { name: 'ActionName', index: 'ActionName' },
            { name: 'ActionHit', index: 'ActionHit' },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#PageHistoryReportPager',
        sortname: 'PageHistoryReport_Id',
        //width: 1250,
        //height:750,
        autowidth: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                debugger;
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        viewrecords: true,
        sortorder: "Asc",

        caption: "Page History Report"
    });
    jQuery("#PageHistoryReportGrid").jqGrid('navGrid', '#PageHistoryReportPager',
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
        },
        {},{},{},{})

    $.getJSON("/Base/FillAllBranchCode",
         function (fillbc) {
             var ddlbc = $("#Campus");
             ddlbc.empty();
             ddlbc.append($('<option/>', { value: "", text: "---Select---" }));

             $.each(fillbc, function (index, itemdata) {
                 ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
             });
         });
    $("#btnSearch").click(function () {
        debugger;
        var Campus = $("#Campus").val();
        var ControllerName = $("#ControllerName").val();
        var ActionName = $("#ActionName").val();
        var ActionHit = $("#ActionHit").val();
        jQuery("#PageHistoryReportGrid").setGridParam(
       {
           datatype: "json",
           url: '/StudentsReport/GetPageHistoryReportListJqGrid/',
           postData: { Campus: Campus, ControllerName: ControllerName, ActionName: ActionName, ActionHit: ActionHit },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        var Campus = $("#Campus").val();
        var ControllerName = $("#ControllerName").val();
        var ActionName = $("#ActionName").val();
        var ActionHit = $("#ActionHit").val();
        jQuery("#PageHistoryReportGrid").setGridParam(
        {
            datatype: "json",
            url: '/StudentsReport/GetPageHistoryReportListJqGrid/',
            postData: { Campus: Campus, ControllerName: ControllerName, ActionName: ActionName, ActionHit: ActionHit },
            page: 1
        }).trigger("reloadGrid");

    });
//});
