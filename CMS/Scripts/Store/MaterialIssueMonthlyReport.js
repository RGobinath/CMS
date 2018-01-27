var grid_selector = "#materialIssueReport";
var pager_selector = "#MaterialsIssueJqGridPager";
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
//function FillMaterialSubGroup()
//{
//    var ddlbc = $("#ddlMaterialSubGroup");
//    if ($("#ddlMaterialGroup").val() != "") {
//        $.getJSON("/Store/FillMaterialSubGroup?MaterialGroupId=" + $("#ddlMaterialGroup").val(),
//          function (fillbc) {
//              ddlbc.empty();
//              ddlbc.append($('<option/>', { value: "", text: "Select" }));
//              $.each(fillbc, function (index, itemdata) {
//                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//              });
//          });
//    }
//    else
//    {
//        ddlbc.empty();
//        ddlbc.append($('<option/>', { value: "", text: "Select" }));
//    }
//}
$(function () {

    var currmonth = $('#curmonth').val();
    $.getJSON("/Store/FillMonth",
             function (fillig) {
                 var ddlmon = $("#ddlmonth");
                 ddlmon.empty();
                 ddlmon.append("<option value=' '> Select </option>");
                 $.each(fillig, function (index, itemdata) {
                     if (itemdata.Value != currmonth) {
                         ddlmon.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                     }
                     else {
                         ddlmon.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text,
                         selected: "Selected"

                     }));
                     }
                 });
             });
    $.getJSON("/Base/FillAllBranchCode",
             function (fillig) {
                 var ddlcam = $("#ddlCampus");
                 ddlcam.empty();
                 ddlcam.append("<option value=' '> Select </option>");
                 $.each(fillig, function (index, itemdata) {
                     if (itemdata.Value != currmonth) {
                         ddlcam.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                     }
                     else {
                         ddlcam.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text,
                         selected: "Selected"

                     }));
                     }
                 });
             });
    //$.getJSON("/Store/FillMaterialGroup",
    //  function (fillbc) {
    //      var ddlbc = $("#ddlMaterialGroup");
    //      ddlbc.empty();
    //      ddlbc.append($('<option/>', { value: "", text: "Select" }));

    //      $.each(fillbc, function (index, itemdata) {
    //          ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    //      });
    //  });

    $("#txtMaterial").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    //$("#ddlMaterialGroup").change(function () {
    //    alert($("#ddlMaterialGroup"));
    //    FillMaterialSubGroup();
    //});
    $("#btnSearch").click(function () {

        $(grid_selector).clearGridData();
        RequiredForCampus = $("#ddlCampus").val();
        Material = $("#txtMaterial").val();
        MaterialGroup = $("#txtMaterialGroup").val();
        MaterialSubGroup = $("#txtMaterialSubGroup").val();
        Month = $("#ddlmonth").val();
        Year = $("#ddlyear").val();
        Quantity = $("#txtQuantity").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialsIssueReportJqGrid/',
                    postData: { RequiredForCampus: RequiredForCampus, MaterialGroup: MaterialGroup, MaterialSubGroup: MaterialSubGroup, Material: Material, IssuedMonth: Month, IssuedYear: Year, IssuedQty: Quantity },
                    page: 1
                }).trigger("reloadGrid");
    });


    $('#btnReset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });

    var RequiredForCampus = $("#ddlCampus").val();
    var Material = $("#txtMaterial").val();
    var Year = $("#ddlyear").val();
    var Quantity = $("#txtQuantity").val();
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialsIssueReportJqGrid/?RequiredForCampus=' + RequiredForCampus + '&Material=' + Material + '&IssuedMonth=' + currmonth + '&IssuedYear=' + Year + '&IssuedQty=' + Quantity,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'From Store', 'To Store', 'Required For Campus', 'Material Group', 'Material SubGroup', 'Material', 'Issued Month', 'Issued Year', 'Issued Quantity', 'Total Price'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'RequiredFromStore', index: 'RequiredFromStore', width: 90 },
              { name: 'RequiredForStore', index: 'RequiredForStore', width: 90 },
              { name: 'RequiredForCampus', index: 'RequiredForCampus', width: 90 },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90 },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90 },
              { name: 'Material', index: 'Material', width: 90 },
              { name: 'IssuedMonth', index: 'IssuedMonth', width: 90, formatter: MonthName },
              { name: 'IssuedYear', index: 'IssuedYear' },
              { name: 'IssuedQty', index: 'IssuedQty', width: 90 },
              { name: 'TotalPrice', index: 'TotalPrice', width: 90 },
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '230',
        width: 1225,
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
        caption: '<i class="fa fa-shopping-cart"></i> Material Issued Report List',
        forceFit: true
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
            {}, {}, {});

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            RequiredForCampus = $("#ddlCampus").val();
            MaterialGroup = $("#txtMaterialGroup").val();
            MaterialSubGroup = $("#txtMaterialSubGroup").val();
            Material = $("#txtMaterial").val();
            Month = $("#ddlmonth").val();
            Year = $("#ddlYear").val();
            OpeningBalance = $("#txtOpeningBalance").val();
            Inward = $("#txtInward").val();
            Outward = $("#txtOutward").val();
            ClosingBalance = $("#txtClosingBalance").val();
            window.open("MaterialsIssueReportJqGrid" + '?RequiredForCampus=' + RequiredForCampus +
                '&MaterialGroup=' + MaterialGroup +
                '&MaterialSubGroup=' + MaterialSubGroup +
                '&Material=' + Material + '&IssuedMonth=' + Month + '&IssuedYear=' + Year + '&IssuedQty=' + Quantity + '&rows=9999' + '&ExptXl=1');
        }
    });
    $(grid_selector).jqGrid('filterToolbar', {
        searchOnEnter: true, beforeSearch: function () {
            $(grid_selector).clearGridData();
            return false;
        }
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
});
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
