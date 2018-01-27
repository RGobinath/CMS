$(function () {
    var grid_selector = "#MaterialReturn";
    var pager_selector = "#MaterialReturnPager";


    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialReturnJqGrid?ReturnStatus=' + $("#ReturnStatus").val(),
        datatype: 'json',
        mtype: 'POST',
        height: '220',
        width: '1000',
        colNames: ['MatRetId', 'Return RefNum', 'From Store', 'To Store', 'DC Number', 'Created By', 'Created Date', 'Return Status', 'Accepted Status'],
        colModel: [
                                { name: 'MatRetId', index: 'MatRetId', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true },
                                { name: "ReturnRefNum", index: "ReturnRefNum", width: 100, align: "left", formatter: formateadorLink },
                                { name: 'FromStore', index: 'FromStore', width: 100 },
                                { name: 'ToStore', index: 'ToStore', width: 100 },
                                { name: 'DCNumber', index: 'DCNumber', width: 100 },
                                { name: 'CreatedBy', index: 'CreatedBy' },
                                { name: "CreatedDate", index: "CreatedDate", width: 100 },
                                { name: 'ReturnStatus', index: 'ReturnStatus', width: 100 },
                                { name: 'AcceptedStatus', index: 'AcceptedStatus', width: 100 }
                                ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100],
        sortname: 'MatRetId',
        sortorder: "Desc",
        viewrecords: true,
        autowidth: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-th-list">&nbsp;</i>Materials Return',
        forceFit: true,
        gridview: true
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

    $("#ReturnStatus").change(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialReturnJqGrid',
                    postData: { ReturnStatus: $("#ReturnStatus").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnNew").click(function () {
        var url = $('#NewUrl').val();
        window.location.href = url;
    });
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

function formateadorLink(cellvalue, options, rowObject) {
    if (rowObject[7] == "Open") {
        return "<a href=/Store/MaterialReturn?MatRetId=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
    else {
        return "<a href=/Store/ShowMaterialReturnList?MatRetId=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
}