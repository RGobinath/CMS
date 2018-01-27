jQuery(function ($) {
    debugger;
    VehicleTypeddl();
    PurposeNameddl();
    var grid_selector = "#DailyUsageVehicleMasterListJqGrid";
    var pager_selector = "#DailyUsageVehicleMasterListJqGridPager";

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
        url: '/Transport/DailyUsageVehicleMasterListJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['ViewId','Id', 'Campus', 'Vehicle Type', 'Type', 'Vehicle No', 'Fuel Type', 'Last Upadeted Date'],
        colModel: [
                { name: 'ViewId', index: 'ViewId', width: 50, align: 'left', hidden: true, key: true },
                { name: 'Id', index: 'Id', width: 50, align: 'left', hidden: true},
                { name: 'Campus', index: 'Campus', width: 150, align: 'left' },
                { name: 'VehicleType', index: 'VehicleType', width: 150, align: 'left',sortable:false },
                { name: 'Type', index: 'Type', width: 150, align: 'left',hidden:true },
                { name: 'VehicleNo', index: 'VehicleNo', formatter: formateadorLink },
                { name: 'FuelType', index: 'FuelType' },
                { name: 'VehicleTravelDate', index: 'VehicleTravelDate', width: 150, align: 'left' }
        ],

        viewrecords: true,
        rowNum: 10,
        rowTotal: 2000,
        rowList: [10, 20, 30],
        pager: pager_selector,
        sortname: 'ViewId',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-truck'></i>&nbsp;Vehicle Details"
    });


   // $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size


    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
        }, 0);
    }
    function formateadorLink(cellvalue, options, rowObject) {
        

        return "<a style='color:#034af3;text-decoration:underline' href=/Transport/VehicleDetailsAdd?VehicleId=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
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
            { width: 'auto', url: '/Transport/AddVehicleType?test=edit' }, { width: 'auto', url: '/Transport/AddVehicleType' }, {}, {}, {})

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
    $("#gs_VehicleNo").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetVehicleNo?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $("#btnAddSubTyp").click(function () {
        window.location.href = "/Transport/AddVehicleDetails";
    });

    $("#btnSearch").click(function () {        
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/DailyUsageVehicleMasterListJqGrid',
                    postData: { VehicleType: $("#VehicleTypeDdl").val(), VehicleNo: $("#VehicleNo").val(), FuelType: $("#FuelType").val(), Campus: $("#branchcodeddl").val(), Purpose: $("#ddlPurposeName").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/DailyUsageVehicleMasterListJqGrid',
                    postData: { VehicleType: $("#VehicleTypeDdl").val(), Type: $("#Type").val(), VehicleNo: $("#VehicleNo").val(), FuelType: $("#FuelType").val(), Campus: $("#branchcodeddl").val(), Purpose: $("#ddlPurposeName").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReport").click(function () {
        ModifiedLoadPopupDynamicaly("/Transport/VehicleOverviewReport", $('#DivVehicleOverviewReport'),
               function () { }, function () { }, 1020, 550, "Vehicle Overview Report");
    });    
});
$.getJSON("/Base/FillAllBranchCode",
     function (fillcampus) {
         var ddlcam = $("#branchcodeddl");
         ddlcam.empty();
         ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select"

        }));

         $.each(fillcampus, function (index, itemdata) {
             ddlcam.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
     });



//function NewVehicleTypeddl() {
//    $.getJSON("/Transport/NewVehicleTypeddl",
//      function (fillbc) {
//          var ddlbc = $("#VehicleTypeDdl");
//          ddlbc.empty();
//          ddlbc.append($('<option/>', { value: "", text: "Select" }));

//          $.each(fillbc, function (index, itemdata) {
//              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//          });
//      });
//}
function VehicleTypeddl() {
    $.getJSON("/Base/GetVehicleType",
      function (fillbc) {
          var ddlbc = $("#VehicleTypeDdl");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function PurposeNameddl() {
    $.getJSON("/Transport/Purposeddl",
      function (fillbc) {
          var ddlbc = $("#ddlPurposeName");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
          });
      });
}
