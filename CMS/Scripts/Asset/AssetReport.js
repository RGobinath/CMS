
$(document).ready(function () {
    var grid_selector = "#AssetReportJqgrid";
    var pager_selector = "#AssetReportJqgridPager";
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                var page_width = $(".tab-content").width();
                $(grid_selector).jqGrid('setGridWidth', page_width);
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/Asset/AssetReportListJqGrid',
        datatype: 'json',
        height: 265,
        autowidth: true,
        colNames: ['Id', 'Campus', 'Hall', 'Booked on', 'Start Time', 'End Time', 'Event Description'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'Campus', index: 'Campus' },
             { name: 'AssetName', index: 'AssetName' },
             { name: 'Date', index: 'Date', search: false },
             { name: 'StartTimeString', index: 'StartTimeString', search: false },
             { name: 'EndTimeString', index: 'EndTimeString', search: false },
             { name: 'ReasonForBooking', index: 'ReasonForBooking' },
        ],
        viewrecords: true,
        rowNum: 50,
        //width:500,
        rowList: [50,100,200],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
               // styleCheckbox(table);
               // updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Event History"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

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
    //expot to excel
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            var Campus = $("#ddlcampus").val();
            var Hall = $("#ddlAsset").val();
            var frmdate = $('#txtFromDate').val();
            var todate = $('#txtToDate').val();
            window.open("/Asset/AssetReportListJqGrid?Campus=" + Campus + '&AssetName=' + Hall + '&frmdate=' + frmdate + '&todate=' + todate + '&ExportType=Excel&rows=9999');
        }
    });


    $('#btnSearch').click(function () {
        debugger;
        var Campus = $("#ddlcampus").val();
        var Hall = $("#ddlAsset").val();
        var frmdate = $('#txtFromDate').val();
        var todate = $('#txtToDate').val();
        //if (frmdate != "" && todate == "");
        //{
        //    ErrMsg("Please select To date!...");
        //    return false;
        //}
        //if (todate != "" && frmdate == "");
        //{
        //    ErrMsg("Please select From date!...");
        //    return false;
        //}
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam({
            datatype: "json",
            url: "/Asset/AssetReportListJqGrid",
            postData: { Campus: Campus, AssetName: Hall, frmdate: frmdate, todate: todate },
            page: 1
        }).trigger("reloadGrid");
        loadgrid();
    });
    $('#btnReset').click(function () { $("input[type=text], textarea, select").val(""); $(grid_selector).trigger('reloadGrid'); });
    $("#ddlcampus").change(function () {
        getAssetdll($("#ddlcampus").val());
    });

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

//Nav Grid Icons Tool Tip
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}
function getAssetdll(campus) {
    $.getJSON("/Asset/GetAssetByCampus?Campus=" + campus,
          function (fillig) {
              $("#ddlAsset").empty();
              $("#ddlAsset").append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillig, function (index, itemdata) {
                  $("#ddlAsset").append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
}
