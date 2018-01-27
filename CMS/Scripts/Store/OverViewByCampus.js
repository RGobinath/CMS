var grid_selector = "#jqGridIssueReportDetailsList";
var Pager_selector = "#jqGridIssueReportDetailsListPager";

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
    $(grid_selector).jqGrid({
        url: '/Store/GetMaterialDistributionListReportByCampus',
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['Id','Academic Year', 'Campus', 'Material', 'IssuedTotal'],
        colModel: [
                    { name: 'Id', index: 'Id', key: true, hidden: true, editable: false },
                    { name: 'AcademicYear', index: 'AcademicYear', editable: false },
                    {
                        name: 'Campus', index: 'Campus', sortable: true, edittype: 'select',
                        stype: 'select', sortable: true,
                        searchoptions: {
                            dataUrl: '/Assess360/GetCampusddl',
                            style: "width: 265px; height: 25px; font-size: 0.9em"
                        }
                    },
                    
                    { name: 'Material', index: 'Material', sortable: false, editable: false },
                     { name: 'IssuedTotal', index: 'IssuedTotal', sortable: false, editable: false, stype: false }, 
        ],

        viewrecords: true,
        altRows: true,
        autowidth: true,
        //multiselect: true,
        // multiboxonly: true,
        height: '220',
        rowNum: 1000,
        rowList: [5, 10, 20],
        sortName: 'Id',
        sortOrder: 'Asc',
        //footerrow: true,
        pager: Pager_selector,

        loadComplete: function () {
            var table = this;
            //$(grid_selector).footerData('set', { Material: 'Total Qtys' });
            //var colTrip = $(grid_selector).jqGrid('getCol', 'IssuedTotal', false, 'sum');
            ////alert(colTrip);
            //$(grid_selector).footerData('set', { IssuedTotal: colTrip });

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },

        caption: '<i class="fa fa-th-list"></i>&nbsp; Meterial Distribution Report',
        serializeRowData: function (postdata) {

            if (postdata.IssuedTotal != 0) {
                return postdata;
            } else {
                return null;
            }
        }

    });
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $("#btnBack").click(function () {
        window.location.href = "/Store/StudentMaterialDistribution";
    });

    jQuery(grid_selector).jqGrid('navGrid', Pager_selector,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {})
    jQuery(grid_selector).jqGrid('navButtonAdd', Pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            var Campus = $('#gs_Campus').val();
            var Grade = $('#gs_AcademicYear').val();
            var Material = $('#gs_Material').val();
            window.open("ExportToExcelMaterialDistributionOverviewReport" + '?rows=9999' + '&Campus=' + Campus + '&Material=' + Material);
            //window.open("ExportToExcelStudentMaterialDistribution");
            //window.open("ExportToExcel");
        }
    });

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
function CalculateTotalQty(rowId) {
    debugger;
    if ($("#" + rowId + "_IssuedTotal").val() != "") {
        $("#" + rowId + "_IssuedTotal").val(parseInt($("#" + rowId + "_IssuedTotal").val()));
    }
    else {
        $("#" + rowId + "_IssuedTotal").val('');
    }
}

