jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/Asset/AssetDetailsReportJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Asset Type', 'Using', 'Scrap', 'Service', 'Stock', 'Total'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true },
                      { name: 'AssetType', index: 'AssetType', editable: true, width: '300' },
                      { name: 'Using', index: 'Using', align: 'center', formatter: formatterlink },
                      { name: 'Scrap', index: 'Scrap', align: 'center', formatter: formatterlink },
                      { name: 'Service', index: 'Service', align: 'center', formatter: formatterlink },
                      { name: 'Stock', index: 'Stock', align: 'center', formatter: formatterlink },
                      { name: 'TotalAsset', index: 'TotalAsset', align: 'center', formatter: formatterlink }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        footerrow: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            debugger;
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            //jQuery(grid_selector).footerData('set', { AssetType: 'Total' });

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Asset Details Report",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
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
            {
                //width: 'auto', url: '/Asset/EditAssetBrandMaster', modal: false, closeAfterEdit: true
                //url: '/Common/AddAcademicMaster/?test=edit', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                //width: 'auto', url: '/Asset/AddAssetBrandMaster', modal: false, closeAfterAdd: true
                //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
              {
                  //width: 'auto', url: '/Asset/DeleteAssetBrandMaster', beforeShowForm: function (params) {
                  //    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                  //    return { Id: selectedrows }
                  //}
              },
               {},
                {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("AssetDetailsReportJqGrid" + '?rows=9999999' + '&ExptXl=1');
        }
    });
    //$("#btnReset").click(function () {
    //    $("input[type=text], textarea, select").val("");
    //    //FillBlockByCampus();
    //    jQuery(grid_selector).setGridParam(
    //   {
    //       datatype: "json",
    //       url: '/Asset/AssetDetailsReportJqGrid',
    //       postData: { Brand: "", IsActive: "" },
    //       page: 1
    //   }).trigger("reloadGrid");
    //});
    //$("#btnSearch").click(function () {
    //    //var Campus = $("#ddlCampus").val();
    //    var Brand = $("#txtBrandName").val();
    //    var IsActive = $("#ddlIsActive").val();
    //    jQuery(grid_selector).setGridParam(
    //   {
    //       datatype: "json",
    //       url: '/Asset/AssetDetailsReportJqGrid',
    //       postData: { Brand: Brand, IsActive: IsActive },
    //       page: 1
    //   }).trigger("reloadGrid");
    //});
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
    //$(document).on('ajaxloadstart', function (e) {
    //    $(grid_selector).jqGrid('GridUnload');
    //    $('.ui-jqdialog').remove();
    //});
});
function ShowAssetDetails(AssetType, TransactionType) {
    if (AssetType == "undefined")
    {
        AssetType = "";
    }
    ModifiedLoadPopupDynamicaly("/Asset/ShowAssetDetails?AssetType=" + AssetType + '&TransactionType=' + TransactionType, $('#ShowAssetDetails'),
               function () { }, function () { }, 1200, 500, "Asset Details");
}
function formatterlink(cellvalue, options, rowObject) {
    //return cellvalue;
    var delBtn = "";
    delBtn = "<a onclick=ShowAssetDetails('" + rowObject[1] + "','" + options.colModel.index + "');>" + cellvalue + "</a>";
    return delBtn;
}