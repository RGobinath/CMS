var grid_selector = "#MaterialInwardMonthList";
var pager_selector = "#MaterialInwardMonthListPager";


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


$(function () {
    var currmonth = $('#curmonth').val();
    $.getJSON("/Store/FillMonth",
             function (fillig) {
                 var mon = $("#ddlmonth");
                 mon.empty();
                 mon.append("<option value=' '> Select </option>");
                 $.each(fillig, function (index, itemdata) {
                     if (itemdata.Value != currmonth) {
                         mon.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                     }
                     else {
                         mon.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text,
                         selected: "Selected"

                     }));
                     }
                 });
             });
    $("#txtMaterial").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $("#btnSearch").click(function () {
       
        $(grid_selector).clearGridData();
        StoreCampus = $("#ddlcampus").val();
        Material = $("#txtMaterial").val();
        MaterialGroup = $("#txtMaterialGroup").val();
        MaterialSubGroup = $("#txtMaterialSubGroup").val();
        Month = $("#ddlmonth").val();
        Year = $("#ddlyear").val();
        ReceivedQty = $("#ReceivedQty").val();
        DamagedQty = $("#DamagedQty").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialInwardMonthlyReportJqGrid/',
                    postData: { StoreCampus: StoreCampus, Material: Material, MaterialGroup: MaterialGroup, MaterialSubGroup: MaterialSubGroup, Month: Month, Year: Year, ReceivedQty: ReceivedQty, DamagedQty: DamagedQty },
                    page: 1
                }).trigger("reloadGrid");
    });
            $('#btnReset').click(function () {
                var url = $('#BackUrl').val();
                window.location.href = url;
            });
    MaterialGroup = $("#txtMaterialGroup").val();
    MaterialSubGroup = $("#txtMaterialSubGroup").val();
    Material = $("#txtMaterial").val();
    Month = $("#ddlmonth").val();
    Year = $("#ddlyear").val();
    ReceivedQty = $("#ReceivedQty").val();
    DamagedQty = $("#DamagedQty").val();
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialInwardMonthlyReportJqGrid?Material=' + Material + '&MaterialGroup=' + MaterialGroup + '&MaterialSubGroup=' + MaterialSubGroup + '&Month=' + currmonth + '&Year=' + Year + '&ReceivedQty=' + ReceivedQty + '&DamagedQty=' + DamagedQty,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Material Group', 'Material SubGroup', 'Material', 'Store', 'ReceivedQty', 'DamagedQty', 'Total Price', 'Month', 'Year'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'Campus', index: 'Campus', width: 90 },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90 },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90 },
              { name: 'Material', index: 'Material', width: 90 },
              { name: 'Store', index: 'Store', width: 90 },
              { name: 'ReceivedQty', index: 'ReceivedQty', width: 60 },
              { name: 'DamagedQty', index: 'DamagedQty', width: 60 },
              { name: 'TotalPrice', index: 'TotalPrice', width: 70 },
              { name: 'Month', index: 'Month', width: 90, formatter: MonthName },
              { name: 'Year', index: 'Year', width: 90 },
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '260',
        //width: 1225,
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-sort-amount-asc"></i> Material Inward Monthly Report',
        forceFit: true
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: true })


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
            {}, {}, {});
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            Material = $("#txtMaterial").val();
            MaterialGroup = $("#txtMaterialGroup").val();
            MaterialSubGroup = $("#txtMaterialSubGroup").val();
            Month = $("#ddlmonth").val();
            Year = $("#ddlyear").val();
            ReceivedQty = $("#ReceivedQty").val();
            DamagedQty = $("#DamagedQty").val();
            window.open("MaterialInwardMonthlyReportJqGrid" + '?Material=' + Material +
                '&MaterialGroup=' + MaterialGroup +
                '&MaterialSubGroup=' + MaterialSubGroup +
                '&Month=' + Month + '&Year=' + Year + '&ReceivedQty=' + ReceivedQty + '&DamagedQty=' + DamagedQty + '&rows=9999' + '&ExptXl=1');
        }
    });
});

function MonthName(cellvalue, options, rowObject) {

    switch (cellvalue) {
        case '1': return "January";
        case '2': return "February";
        case '3': return 'March';
        case '4': return 'April';
        case '5': return 'May';
        case '6': return 'June';
        case '7': return 'July';
        case '8': return 'August';
        case '9': return 'September';
        case '10': return 'October';
        case '11': return 'November';
        case '12': return 'December';
        default: return "";
    }
}
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