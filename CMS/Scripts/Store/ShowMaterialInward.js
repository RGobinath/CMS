$(function () {
    $('#btnBack').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });
    var Id = $("#Id").val();

    var grid_selector = "#MaterialSkuList";
    var pager_selector = "#MaterialSkuListPager";

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

    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialSkuListJqGrid?Id=' + Id + '&Store=' + $("#Store").val(),
        datatype: 'json',
        mtype: 'GET',
        shrinkToFit: true,
        colNames: ['SKU Id', 'Material Ref Id', 'Material', 'Material Group', 'Material Sub Group', 'Ord.Units', 'Rcvd.Units', 'Old Prices', 'Ord.Qty', 'Rcvd.Qty', 'Dmg.Qty', 'Unit Price', 'Tax in %', 'Discount in %', 'Total Price', 'Dmg Desc / Remarks'],
        colModel: [
              { name: 'SkuId', index: 'SkuId', hidden: true, key: true },
              { name: 'MaterialRefId', index: 'MaterialRefId', hidden: true },
              { name: 'Material', index: 'Material', sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[3] + ', Material Sub Group:' + rawObject[4] + '"' } },
              { name: 'MaterialGroup', index: 'MaterialGroup', sortable: true, hidden: true },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', sortable: true, hidden: true },
              { name: 'OrderedUnits', index: 'OrderedUnits', sortable: true },
              { name: 'ReceivedUnits', index: 'ReceivedUnits', sortable: true },
              { name: 'OldPrices', index: 'OldPrices' },
              { name: 'OrderQty', index: 'OrderQty', sortable: true },
              { name: 'ReceivedQty', index: 'ReceivedQty', sortable: true },
              { name: 'DamagedQty', index: 'DamagedQty', sortable: true },
              { name: 'UnitPrice', index: 'UnitPrice', sortable: true },
              { name: 'Tax', index: 'Tax', sortable: true },
              { name: 'Discount', index: 'Discount',sortable:true},
              { name: 'TotalPrice', index: 'TotalPrice', sortable: true },
              { name: 'DamageDescription', index: 'DamageDescription', sortable: true }
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'SkuId',
        sortorder: 'Desc',
        height: '220',
        autowidth: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list"></i>&nbsp;Material SKU List',
        forceFit: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
   
});

