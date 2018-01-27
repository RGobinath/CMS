jQuery(function ($) {
    SupplierNameDDl();
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
    jQuery("#VehicleFuelRefillEntryListJqGrid").jqGrid({
        url: '/Transport/VehicleFuelRefillEntryListJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['FuelRefillId', 'Vehicle Id', 'VehicleCostId', 'Vehicle No', 'Campus', 'Vendor', 'IndentNumber', 'InvoiceNumber', 'Description', 'EntryDate', 'Created By', 'Created Date', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
             { name: 'FuelRefillId', index: 'FuelRefillId', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'VehicleCostId', index: 'VehicleCostId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo', width: 100 },
             { name: 'Campus', index: 'Campus', width: 60 },
             { name: 'NoOfLitres', index: 'NoOfLitres', width: 50 },
             { name: 'Vendor', index: 'Vendor', width: 50 },
             { name: 'IndentNumber', index: 'IndentNumber', width: 100 },
             { name: 'InvoiceNumber', index: 'InvoiceNumber', width: 100 },
             { name: 'Description', index: 'Description', width: 100 },
             { name: 'EntryDate', index: 'EntryDate', width: 100 },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
        ],
        pager: '#VehicleFuelRefillEntryListJqGridPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'FuelRefillId',
        sortorder: 'Asc',
        height: 130,
        //autowidth:true,   
        //shrinktofit: true,
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
        caption: 'Fuel  List'
    });

    var Type = $("#Type").val();
    //VehicleFuelReportChart(VehicleId);
    $('#VehicleFuelRefillEntryListJqGrid').jqGrid('filterToolbar', { searchOnEnter: true });


    //navButtons
    jQuery("#VehicleFuelRefillEntryListJqGrid").jqGrid('navGrid', '#VehicleFuelRefillEntryListJqGridPager',
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
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
//$("#IndentNumber").on("keyup", function () {
//    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
//        val = this.value;

//    if (!valid) {
//        console.log("Invalid input!");
//        this.value = val.substring(0, val.length - 1);
//    }
//});
//$("#InvoiceNumber").on("keyup", function () {
//    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
//        val = this.value;

//    if (!valid) {
//        console.log("Invalid input!");
//        this.value = val.substring(0, val.length - 1);
//    }
//});
function SupplierNameDDl() {
    $.getJSON("/Transport/SupplierNameddl?SupplierType=General,Service,Fuel",
      function (fillbc) {
          var ddlbc = $("#Vendor");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              if ($("#hdnVendor").val() == itemdata.Value)
              {
                  ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
              }
              else
              {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              }
          });
      });
}
function FuelEntryCreate() {
    debugger;
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $('#hdnCampus').val();
    var Vendor = $('#Vendor').val();
    var IndentNumber = $('#IndentNumber').val();
    var InvoiceNumber = $('#InvoiceNumber').val();
    var Description = $("#Description").val();
    //var Date = $("#VehicleTravelDate").val();
    var VehicleCostId = $("#hdnVehicleCostId").val();
    var FuelRefillId = $("#hdnFuelRefillId").val();
    //var Type = $("#Type").val();
    if ( Vendor == '' || IndentNumber == '' || InvoiceNumber == '' ) {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (isNaN(IndentNumber)) {
        ErrMsg('Numbers only allowed.');
        $('#IndentNumber').focus();
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/Transport/SaveOrUpdateVehicleFuelEntry",
        data: {
            VehicleId: VehicleId, Campus: Campus, VehicleNo: VehicleNo, Vendor: Vendor, IndentNumber: IndentNumber, InvoiceNumber: InvoiceNumber, Description: Description, VehicleCostId: VehicleCostId, FuelRefillId: FuelRefillId,
        },
        async:false,
        success: function (data) {           
            if (data.statusval == "added") {
                $("#hdnFuelRefillId").val(data.FuelRefillId);
                SucessMsg("Added Sucessfully");
                return true;
            }
            if (data.statusval == "updated")
            {
                $("#hdnFuelRefillId").val(data.FuelRefillId);
                SucessMsg("Updated Successfully");
                return true;
            }
        }

    });
}