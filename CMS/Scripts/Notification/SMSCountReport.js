function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    FillCampusDll();
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
        url: '/Communication/GetSMSCount_SPListJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Sent', 'Failed', 'Not valid', 'Not Delivered', 'DND Applied', 'Total'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true },
                      { name: 'Campus', index: 'Campus', width: '300' },
                      {
                          name: 'Sent', index: 'Sent', align: 'center'
                          //, formatter: formatterlink
                      },
                      {
                          name: 'Failed', index: 'Failed', align: 'center'
                          //, formatter: formatterlink
                      },
                      {
                          name: 'Notvalid', index: 'Notvalid', align: 'center'
                          //, formatter: formatterlink
                      },
                      {
                          name: 'NotDelivered', index: 'NotDelivered', align: 'center'
                          //, formatter: formatterlink
                      },
                      {
                          name: 'DNDApplied', index: 'DNDApplied', align: 'center'
                          //, formatter: formatterlink
                      },
                      {
                          name: 'Total', index: 'Total', align: 'center'
                          //, formatter: formatterlink
                      }
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
        //userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            debugger;
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            jQuery(grid_selector).footerData('set', { Campus: 'Total' });
            var colSent = $(grid_selector).jqGrid('getCol', 'Sent', false, 'sum');
            var colFailed = $(grid_selector).jqGrid('getCol', 'Failed', false, 'sum');
            var colNotvalid = $(grid_selector).jqGrid('getCol', 'Notvalid', false, 'sum');
            var colNotDelivered = $(grid_selector).jqGrid('getCol', 'NotDelivered', false, 'sum');
            var colDNDApplied = $(grid_selector).jqGrid('getCol', 'DNDApplied', false, 'sum');
            var colTotal = $(grid_selector).jqGrid('getCol', 'Total', false, 'sum');
            jQuery(grid_selector).footerData('Set', { Sent: colSent });
            jQuery(grid_selector).footerData('Set', { Failed: colFailed });
            jQuery(grid_selector).footerData('Set', { Notvalid: colNotvalid });
            jQuery(grid_selector).footerData('Set', { NotDelivered: colNotDelivered });
            jQuery(grid_selector).footerData('Set', { DNDApplied: colDNDApplied });
            jQuery(grid_selector).footerData('Set', { Total: colTotal });

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;SMS Count Report",
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
            }, //Edit
            {                
            }, //Add
              {                  
              },
               {},
                {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("GetSMSCount_SPListJqGrid" + '?rows=50' + '&ExptXl=1' + '&Campus=' + $("#ddlCampus").val() + '&FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val());
        }
    });
    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);
    $('#txtFromDate').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        //startDate: startDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#txtToDate').datepicker('setStartDate', startDate);
    });
    $('#txtToDate').datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#txtFromDate').datepicker('setEndDate', FromEndDate);
    });
    $("#txtToDate").focus(function () {
        if ($("#txtFromDate").val() == "") {
            return ErrMsg("Please Select FromDate");
        }
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Communication/GetSMSCount_SPListJqGrid',
           postData: { Campus: "", FromDate: "", ToDate: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#ddlCampus").val();
        var FromDate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        if (ToDate != "") {
            if (FromDate == "") {
                return ErrMsg("Please Select From Date");
            }
        }
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Communication/GetSMSCount_SPListJqGrid',
           postData: { Campus: Campus, FromDate: FromDate, ToDate: ToDate },
           page: 1
       }).trigger("reloadGrid");
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
    //$(document).on('ajaxloadstart', function (e) {
    //    $(grid_selector).jqGrid('GridUnload');
    //    $('.ui-jqdialog').remove();
    //});
});
//function ShowAssetDetails(AssetType, TransactionType) {
//    if (AssetType == "undefined") {
//        AssetType = "";
//    }
//    ModifiedLoadPopupDynamicaly("/Asset/ShowAssetDetails?AssetType=" + AssetType + '&TransactionType=' + TransactionType, $('#ShowAssetDetails'),
//               function () { }, function () { }, 1200, 500, "Asset Details");
//}
//function formatterlink(cellvalue, options, rowObject) {
//    //return cellvalue;
//    var delBtn = "";
//    delBtn = "<a onclick=ShowAssetDetails('" + rowObject[1] + "','" + options.colModel.index + "');>" + cellvalue + "</a>";
//    return delBtn;
//}