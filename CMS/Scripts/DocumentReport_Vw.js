jQuery(function ($) {
    var grid_selector = "#jqGridDocumentsReport";
    var pager_selector = "#jqGridDocumentsReportPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#jqGridDocumentsReport").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#jqGridDocumentsReport").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    var ddlSearchby = $("#ddlSearchBy").val();
    var Campus = $("#Campus").val();
    var IdNum = $("#IdNum").val();
    var DocAvl = $("#DocAvl").val();
    jQuery(grid_selector).jqGrid({
        url: '/Common/DocumentReportJqGrid?ddlSearchBy=' + ddlSearchby + "&Campus=" + Campus + "&IdNum=" + IdNum + "&DocAvl=" + DocAvl,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'PreRegNum', 'Campus', 'Id Number', 'Name', 'Type','Is Document Available', 'Email Id', 'Phone No.', 'Address','PF No','ESI','Bank Details','Designation','DOB'],
        colModel: [
        //if any column added need to check formateadorLink
                { name: 'Id', index: 'Id', hidden: true, key: true },
                { name: 'PreRegNum', index: 'PreRegNum', hidden: true },
                { name: 'Campus', index: 'Campus' },
                { name: 'IdNum', index: 'IdNum'},
                { name: 'Name', index: 'Name', search: false,width:'300'},                
                { name: 'Type', index: 'Type', search: false, sortable: false },
                { name: 'IsDocumentAvailable', index: 'IsDocumentAvailable', search: false, align: 'center' },
                { name: 'EmailId', index: 'EmailId', align: 'center' },
                { name: 'PhoneNum', index: 'PhoneNum',  search: false, align: 'center' },
                { name: 'Address', index: 'Address', search: false, align: 'center' },
                { name: 'PFNo', index: 'PFNo', search: false, align: 'center' },
                { name: 'ESI', index: 'ESI', search: false, align: 'center' },
                { name: 'BankDetails', index: 'BankDetails', align: 'center'  },
                { name: 'Designation', index: 'Designation', search: false, align: 'center' },
                { name: 'DOB', index: 'DOb', search: false, align: 'center' },
        ],
        pager: '#jqGridDocumentsReportPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 340,
        autowidth: true,
        //shrinktofit: true,
        viewrecords: true,
        altRows: true,
        multiselect: true,
        viewrecords: true,
        loadComplete: function () {

            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Document Details'
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
        //
    }

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $("#btnSearch").click(function () {
        debugger;
        jQuery(grid_selector).clearGridData();
        var Campus = $("#Campus").val();
        var ddlSearchBy = $("#ddlSearchBy").val();
        var IdNum = $("#IdNum").val();
        var DocAvl = $("#DocAvl").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Common/DocumentReportJqGrid',
                    postData: { ddlSearchBy: ddlSearchBy, Campus: Campus, IdNum: IdNum, DocAvl: DocAvl },
                    page: 1
                }).trigger("reloadGrid");

    });

});

function ShowComments(ActivityId) {
    debugger;
    window.location.href = "/StaffManagement/uploaddisplay?id=" + ActivityId;
}