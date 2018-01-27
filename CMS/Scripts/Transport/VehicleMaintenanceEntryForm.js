jQuery(function ($) {
    SupplierNameDdl();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#FuelRefillListGrid").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var fuelparent_column = $("#FuelRefillListGrid").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#FuelRefillListGrid").jqGrid('setGridWidth', fuelparent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    jQuery("#VehicleMaintenanceEntryListJqGrid").jqGrid({
        url: '/Transport/VehicleMaintenanceEntryListJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Maintenance Id', 'Vehicle Id', 'Vehicle No', 'Campus', 'AC', 'Battery', 'Tyre', 'Supplier Name', 'Amount', 'Invoice Number', 'Description', 'Vehicle Travel Date', 'Created By', 'Created Date', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
             { name: 'MaintenanceId', index: 'MaintenanceId', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo', width: 100 },
             { name: 'Campus', index: 'Campus', width: 60 },
             { name: 'AC', index: 'AC', width: 50 },
             { name: 'Battery', index: 'Battery', width: 50 },
             { name: 'Tyre', index: 'Tyre', width: 50 },
             { name: 'SupplierName', index: 'SupplierName', width: 50 },
             { name: 'Amount', index: 'Amount', width: 50 },
             { name: 'InvoiceNo', index: 'InvoiceNo', width: 60 },
             { name: 'Description', index: 'Description', width: 60 },
             { name: 'VehicleTravelDate', index: 'VehicleTravelDate', width: 60 },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
             { name: 'MechanicalMaintenance', index: 'MechanicalMaintenance', width: 60 },
             { name: 'ElectricalMaintenance', index: 'ElectricalMaintenance', width: 60 },
             { name: 'BodyMaintenance', index: 'BodyMaintenance', width: 60 },
             
        ],
        pager: '#VehicleMaintenanceEntryListJqGridPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'MaintenanceId',
        sortorder: 'Desc',
        height: 130,
        width: 900,
        autowidth: false,
        shrinktofit: true,
        viewrecords: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Breakdown Maintenance  List'
    });

    var Type = $("#Type").val();
    //VehicleFuelReportChart(VehicleId);
    // $('#VehicleBreakdownMaintenanceListJqGrid').jqGrid('filterToolbar', { searchOnEnter: true });


    //navButtons
    jQuery("#VehicleMaintenanceEntryListJqGrid").jqGrid('navGrid', '#VehicleMaintenanceEntryListJqGridPager',
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
            }, {}, {},
            //{ url: '/Transport/DeleteFuelRefillId' }, 
            {})

    //$("#txtAC").change(function () {
    //    SupplierNameDdl();
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


    $(document).on('ajaxloadstart', function (e) {
        $("#FuelRefillListGrid").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });    
});
//$("#InvoiceNo").on("keyup", function () {
//    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
//        val = this.value;

//    if (!valid) {
//        console.log("Invalid input!");
//        this.value = val.substring(0, val.length - 1);
//    }
//});
function SupplierNameDdl() {    
    $.getJSON("/Transport/SupplierNameddl?SupplierType=General,Service,Fuel",
      function (fillbc) {
          var ddlbc = $("#SupplierName");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              if ($("#hdnSupplierName1").val() == itemdata.Value)
              {
                  ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
              }
              else {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              }
          });
      });
}
function MaintenanceCreate() {
    debugger;
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $('#hdnCampus').val();
    var AC = $('#AC').is(":checked");
    var Battery = $('#Battery').is(":checked");
    var Tyre = $('#Tyre').is(":checked");
    var SupplierName = $('#SupplierName').val();
    var InvoiceNumber = $('#InvoiceNo1').val();
    //var Amount = $('#Amount').val();
    var Description = $('#Description1').val();
    var Date = $("#VehicleTravelDate").val();
    var MaintenanceId = $("#hdnMaintenanceId").val();
    var VehicleCostId = $("#hdnVehicleCostId1").val();
    var MechanicalMaintenance = $("#MechanicalMaintenance").is(":checked");
    var ElectricalMaintenance = $("#ElectricalMaintenance").is(":checked");
    var BodyMaintenance = $("#BodyMaintenance").is(":checked");
    //var Type = $("#Type").val();
    if (SupplierName == '' || InvoiceNumber == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (AC == false && Tyre == false && Battery == false && MechanicalMaintenance == false && ElectricalMaintenance == false && BodyMaintenance == false)
    {
        ErrMsg("Please Select Atlease One Service.");
        return false;
    }
    //if (isNaN(Amount)) {
    //    ErrMsg('Numbers only allowed.');
    //    $('#Amount').focus();
    //    return false;
    //}    
    $.ajax({
        type: 'POST',
        url: "/Transport/SaveOrUpdateVehicleMaintenanceEntry",
        async: false,
        data: {
            VehicleId: VehicleId, Campus: Campus, VehicleNo: VehicleNo, AC: AC, Battery: Battery, Tyre: Tyre, SupplierName: SupplierName, InvoiceNo: InvoiceNumber, Description: Description, VehicleTravelDate: Date, MaintenanceId: MaintenanceId, VehicleCostId: VehicleCostId, MechanicalMaintenance: MechanicalMaintenance, ElectricalMaintenance: ElectricalMaintenance, BodyMaintenance: BodyMaintenance
        },
        success: function (data) {
            if (data.statusval == "added") {
                $("#hdnMaintenanceId").val(data.MaintenanceId);
                SucessMsg("Added Sucessfully");
                return true;
            }
            if (data.statusval == "updated") {
                $("#hdnMaintenanceId").val(data.MaintenanceId);
                SucessMsg("Updated Successfully");
                return true;
            }
            //$("#VehicleBreakdownMaintenanceListJqGrid").trigger('reloadGrid');
            //$("input[type=text], textarea").val("");
            //$('#hdnVehicleId').val('');
            //$('#hdncampus').val('');
            //$("#txtAC").val('');
            //$("#txtBattery").val('');
            //$("#txtTyre").val('');
            //$("#SupplierName").val('');
            //$("#InvoiceNo").val('');
            //$("#Amount").val('');
            //$("#Description").val('');            
        }

    });
}