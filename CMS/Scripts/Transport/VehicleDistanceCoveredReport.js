jQuery(function ($) {
    var grid_selector = "#vehicleDistanceReport";
    var pager_selector = "#vehicleDistanceReportPager";

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

    var Month = $("#Month").val();
    var Year = $("#Year").val();
    var Campus;
    var Type;
    var VehicleNo;
    $("#ddlyear").val(Year);
    $.getJSON("/Base/FillMonth",
             function (fillig) {
                 var mior = $("#ddlmonth");
                 mior.empty();
                 mior.append("<option value=' '> Select </option>");
                 $.each(fillig, function (index, itemdata) {
                     if (itemdata.Value != Month) {
                         mior.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                     }
                     else {
                         mior.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text,
                         selected: "Selected"

                     }));
                     }
                 });
             });

    jQuery(grid_selector).jqGrid({
        url: '/Transport/VehicleDistanceReportJqGrid?CurrMonth=' + Month + '&CurrYear=' + Year,
        datatype: 'json',
        height: 250,
        colNames: ['Id','Campus', 'Vehicle Name', 'Vehicle No', 'Month', 'Year', 'Distance Covered'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              {name:'Campus',index:'Campus'},
              { name: 'Type', index: 'Type', width: '250px' },
              { name: 'VehicleNo', index: 'VehicleNo' },
              { name: 'Month', index: 'Month', stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: "select:--select--;1:January;2:February;3:March;4:April;5:May;6:June;7:July;8:August;9:September;10:October;11:November;12:December"} },
              { name: 'Year', index: 'Year' },
              { name: 'DistanceCovered', index: 'DistanceCovered' },
              ],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-truck'></i>&nbsp;Vehicle Distance Covered Report"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });


    //navButtons
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
            }, {}, {}, {}, {})

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            Campus = $("#ddlcampus").val();
            Type = $("#txtType").val();
            VehicleNo = $("#txtVehicleNo").val();
            Year = $("#ddlyear").val();
            Month = $("#ddlmonth").val();
            window.open("VehicleDistanceReportJqGrid" +'?Campus='+Campus +'?Type=' + Type + '&VehicleNo=' + VehicleNo + '&CurrMonth=' + Month + '&CurrYear=' + Year + ' &rows=9999 ' + '&ExptXl=1');
        }
    });

    //For pager Icons
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
    $("#btnSearch").click(function () {
        Campus = $("#ddlcampus").val();
        Type = $("#txtType").val();
        VehicleNo = $("#txtVehicleNo").val();
        Year = $("#ddlyear").val();
        Month = $("#ddlmonth").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleDistanceReportJqGrid',
                    postData: { Campus:Campus,Type: Type, VehicleNo: VehicleNo, CurrMonth: Month, CurrYear: Year },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        Campus = $("#ddlcampus").val();
        Type = $("#txtType").val();
        VehicleNo = $("#txtVehicleNo").val();
        Year = $("#ddlyear").val();
        Month = $("#ddlmonth").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleDistanceReportJqGrid',
                    postData: {Campus:Campus, Type: Type, VehicleNo: VehicleNo, CurrMonth: Month, CurrYear: Year },
                    page: 1
                }).trigger("reloadGrid");
    });
});