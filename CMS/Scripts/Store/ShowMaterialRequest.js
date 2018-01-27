var grid_selector = "#MaterialRequestList";
var pager_selector = "#MaterialRequestListPager";
var commentgrid_selector = "#CommentList";
var commentpager_selector = "#CommentListpager";
$(function () {
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
        $(commentgrid_selector).jqGrid('setGridWidth', $(".col-xs-8").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    var parent_column1 = $(commentgrid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                $(commentgrid_selector).jqGrid('setGridWidth', parent_column1.width());
            }, 0);
        }
    })
    var Id = $("#Id").val();
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialRequestJqGrid?Id=' + Id,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Req.Type', 'Grade', 'Section', 'Req.For', 'Material', 'Material Group', 'Material Sub Group', 'Units', 'Req.Date', 'Status', 'Req.Qty', 'App.Qty', 'Issued.Qty'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'RequestType', index: 'RequestType', width: 90, sortable: true },
              { name: 'RequiredForGrade', index: 'RequiredForGrade', width: 90, sortable: true },
              { name: 'Section', index: 'Section', width: 60, sortable: true },
              { name: 'RequiredFor', index: 'RequiredFor', width: 90, sortable: true },
              { name: 'Material', index: 'Material', width: 90, sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[6] + ', Material Sub Group:' + rawObject[7] + '"' } },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true, hidden: true },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, sortable: true, hidden: true },
              { name: 'Units', index: 'Units', width: 90, sortable: true, editable: true },
              { name: 'RequiredDate', index: 'RequiredDate', width: 90, sortable: true },
              { name: 'Status', index: 'Status', width: 90, sortable: true },
              { name: 'Quantity', index: 'Quantity', width: 90, sortable: true },
              { name: 'ApprovedQty', index: 'ApprovedQty', width: 90, sortable: true },
              { name: 'IssuedQty', index: 'IssuedQty', width: 90, sortable: true },
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '200',
        //width: 1225,
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Material Request List',
        forceFit: true,
        // multiselect: true,
        loadError: function (xhr, status, error) {
            $(grid_selector).clearGridData();
            ErrMsg($.parseJSON(xhr.responseText).Message);
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        subGrid: true,
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (MatIssueList, Id) {
            var MatIssueListTable, MatIssueListPager;
            MatIssueListTable = MatIssueList + "_t";
            MatIssueListPager = "p_" + MatIssueListTable;
            $("#" + MatIssueList).html("<table id='" + MatIssueListTable + "' ></table><div id='" + MatIssueListPager + "' ></div>");
            jQuery("#" + MatIssueListTable).jqGrid({
                url: '/Store/MaterialIssueListJqGrid?Id=' + Id,
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'MRL Id', 'Iss Note Number', 'Issue Date', 'Issued Qty', 'Issued By', 'Status'],
                colModel: [
                       { name: 'Id', index: 'Id', hidden: true },
                       { name: 'MRLId', index: 'MRLId', width: 90, sortable: true, hidden: true },
                       { name: 'IssNoteNumber', index: 'IssNoteNumber', width: 90, sortable: true },
                       { name: 'IssueDate', index: 'IssueDate', width: 90, sortable: true },
                       { name: 'IssueQty', index: 'IssueQty', width: 90, sortable: true },
                       { name: 'IssuedBy', index: 'IssuedBy', width: 90, sortable: true },
                       { name: 'Status', index: 'Status', width: 90, sortable: true },
                       ],
                pager: MatIssueListPager,
                rowNum: '5',
                rowList: [5, 10, 20, 50, 100],
                sortname: 'Id',
                sortorder: "Asc",
                height: '130',
                autowidth: true,
                shrinkToFit: true,
                viewrecords: true,
                forceFit: true,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                }
            });
            jQuery("#" + MatIssueListTable).jqGrid('navGrid', "#" + MatIssueListPager, { edit: false, add: false, del: false, search: false, searchicon: 'ace-icon fa fa-search orange', refresh: true, refreshicon: 'ace-icon fa fa-refresh green' })
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
    }, {}, {}, {}, {})

    $("#btnBack").click(function () {
        window.location.href = $('#BackUrl').val();
    });

    $(commentgrid_selector).jqGrid({
        url: '/Store/DescriptionForSelectedIdJqGrid?Id=' + $('#Id').val(),
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
        colModel: [
        // { name: 'Issue Number', index: 'EntityRefId', sortable: false },
              {name: 'CommentedBy', index: 'CommentedBy', sortable: false },
              { name: 'CommentedOn', index: 'CommentedOn', sortable: false },
              { name: 'RejectionComments', index: 'RejectionComments', sortable: false },
              { name: 'ResolutionComments', index: 'ResolutionComments', sortable: false }
             ],
        pager: commentpager_selector,
        rowNum: -1,
        //width: 800,
        autowidth: true,
        height: 80,
        sortname: 'EntityRefId',
        sortorder: "desc",
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-comments">&nbsp;</i>Discussion Forum'
    });
    jQuery(commentgrid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
    }, {}, {}, {}, {})
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

function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}