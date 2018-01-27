$(function () {


    $('#btnBack').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });

    var grid_selector = "#MaterialReturnList";
    var pager_selector = "#MaterialReturnListPager";

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

    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialReturnListJqGrid?MatRetId=' + $("#MatRetId").val(),
        datatype: 'json',
        mtype: 'POST',
        height: '220',

        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Return Qty'],
        colModel: [
                                { name: 'Id', index: 'Id', width: 100, align: 'left', editable: true, hidden: true, edittype: 'text', key: true },
                                { name: "MaterialGroup", width: 150, index: "MaterialGroup", align: "left" },
                                { name: 'MaterialSubGroup', width: 150, index: 'MaterialSubGroup' },
                                { name: 'Material', width: 150,index: 'Material' },
                                { name: 'Units', index: 'Units', width: 50 },
                                { name: 'ReturnQty', index: 'ReturnQty', width: 50 }

                                ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100],
        sortname: 'Id',
        sortorder: "asc",
        shrinkToFit: true,
        viewrecords: true,
        autowidth: true,
        caption: 'Materials Return List',
        
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        gridview: true
    });

    $("#Complete").click(function () {
        if (confirm("Are you sure to Complete?")) {
            var MaterialReturn = {
                MatRetId: $("#MatRetId").val()
            };
            $.ajax({
                url: '/Store/AcceptReturnedMaterialsAndUpdateStock',
                type: 'POST',
                dataType: 'json',
                data: MaterialReturn,
                traditional: true,
                success: function (data) {
                    if (data != "") {
                        InfoMsg(data + " is completed successfully", function () { window.location.href = '/Store/ShowMaterialReturnList?MatRetId=' + $("#MatRetId").val(); });
                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        }
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