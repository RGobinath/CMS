jQuery(function ($) {
    //SupplierNameDdl();
    SupplierDdl();
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
    jQuery("#VehicleVehicleServiceListJqGrid").jqGrid({
        url: '/Transport/VehicleVehicleServiceListJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Service Id', 'Vehicle Id', 'Vehicle No', 'Campus', 'Start Kms', 'End Kms', 'Vendor', 'Invoice Number', 'Description','EntryDate', 'Created By', 'Created Date', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
             { name: 'ServiceId', index: 'ServiceId', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo', width: 100 },
             { name: 'Campus', index: 'Campus', width: 60 },
             { name: 'StartKms', index: 'StartKms', width: 50 },
             { name: 'EndKms', index: 'EndKms', width: 50 },
             { name: 'Vendor', index: 'Vendor', width: 50 },
             { name: 'InvoiceNo', index: 'InvoiceNo', width: 60 },
             { name: 'Description', index: 'Description', width: 60 },
             { name: 'EntryDate', index: 'EntryDate', width: 60 },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
        ],
        pager: '#VehicleVehicleServiceListJqGridPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'ServiceId',
        sortorder: 'Asc',
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
        caption: 'Vehicle Service Entry List'
    });

    var Type = $("#Type").val();
    //VehicleFuelReportChart(VehicleId);
    // $('#VehicleBreakdownMaintenanceListJqGrid').jqGrid('filterToolbar', { searchOnEnter: true });


    //navButtons
    jQuery("#VehicleVehicleServiceListJqGrid").jqGrid('navGrid', '#VehicleVehicleServiceListJqGridPager',
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
    $("#EndKms").blur(function () {
        var startKms = $("#StartKms").val();
        var endKms = $("#EndKms").val();

        if (parseFloat(startKms) >= parseFloat(endKms)) {
            ErrMsg("End Kms should be greater than Start Kms");

            $("#EndKms").val("");
        }
    });

});
$("#StartKms").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});


$("#EndKms").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});
function SupplierDdl() {
    $.getJSON("/Transport/SupplierNameddl?SupplierType=General,Service,Fuel",
      function (fillbc) {
          var ddlbc = $("#Vendor2");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {              
              if ($("#hdnVendor2").val() == itemdata.Value)
              {                  
                  ddlbc.append("<option value='" + itemdata.Value + "'selected='selected'>" + itemdata.Text + "</option>");
              }
              else {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              }
          });
      });
}
function ServiceCreate() {
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $('#hdnCampus').val();
    var StartKms = $('#StartKms').val();
    var EndKms = $('#EndKms').val();
    var Vendor = $('#Vendor2').val();
    var InvoiceNo = $('#InvoiceNo2').val();
    var Description = $('#Description2').val();
    var Date = $("#EntryDate2").val();
    var ServiceId = $("#hdnServiceId").val();
    var VehicleCostId = $("#hdnVehicleCostId2").val();
    //var Type = $("#Type").val();
    if (StartKms == '' || EndKms == '' || Vendor == '' || InvoiceNo == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }    
    $.ajax({
        type: 'POST',
        url: "/Transport/SaveOrUpdateVehicleService",
        data: {
            VehicleId: VehicleId, Campus: Campus, VehicleNo: VehicleNo, StartKms: StartKms, EndKms: EndKms, Vendor: Vendor, InvoiceNo: InvoiceNo, Description: Description, EntryDate: Date, ServiceId: ServiceId, VehicleCostId: VehicleCostId
        },
        async:false,
        success: function (data) {
            if (data.statusval == "added") {
                $("#hdnServiceId").val(data.ServiceId);
                SucessMsg("Added Sucessfully");
                return true;
            }
            if (data.statusval == "updated") {
                $("#hdnServiceId").val(data.ServiceId);
                SucessMsg("Updated Successfully");
                return true;
            }
            //$("#VehicleVehicleServiceListJqGrid").trigger('reloadGrid');
            //$("input[type=text], textarea").val("");
            //$('#hdnVehicleId').val('');
            //$('#hdncampus').val('');
            //$("#StartKms").val('');
            //$("#EndKms").val('');
            //$("#InvoiceNo").val('');
            //$("#Vendor").val();
            //$("#Description").val();
            //SucessMsg("Added Sucessfully");
        }

    });
}