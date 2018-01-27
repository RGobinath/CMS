jQuery(function ($) {
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
    jQuery("#VehicleVehicleFCEntryListJqGrid").jqGrid({
        url: '/Transport/VehicleVehicleFCEntryListJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['FC Id', 'Vehicle Id', 'Vehicle No', 'Campus', 'Vendor', 'Invoice No', 'Description', 'EntryDate', 'Created By', 'Created Date', 'ModifiedBy', 'ModifiedDate'],
        colModel: [
             { name: 'OthersId', index: 'OthersId', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo', width: 100 },
             { name: 'Campus', index: 'Campus', width: 60 },
             { name: 'Vendor', index: 'Vendor', width: 60 },
             { name: 'InvoiceNo', index: 'InvoiceNo', width: 60 },
             { name: 'Description', index: 'Description', width: 60 },
             { name: 'EntryDate', index: 'EntryDate', width: 60 },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
        ],
        pager: '#VehicleVehicleFCEntryListJqGridPager',
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
    jQuery("#VehicleVehicleFCEntryListJqGrid").jqGrid('navGrid', '#VehicleVehicleFCEntryListJqGridPager',
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

});

//$("#InvoiceNo").on("keyup", function () {
//    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
//        val = this.value;

//    if (!valid) {
//        console.log("Invalid input!");
//        this.value = val.substring(0, val.length - 1);
//    }
//});
function FitnessCertificateDetailsCreate() {
    debugger;
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $('#hdnCampus').val();
    var Vendor = $('#Vendor3').val();
    var InvoiceNo = $('#InvoiceNo3').val();
    var Description = $('#Description3').val();
    var Date = $("#EntryDate3").val();
    var FCId = $("#hdnFCId").val();
    var VehicleCostId = $("#hdnVehicleCostId3").val();
    //var Type = $("#Type").val();
    if (Description == '') {
        ErrMsg("Please fill the mandatory fields.");
        return false;
    }    
    $.ajax({
        type: 'POST',
        url: "/Transport/SaveOrUpdateVehicleFCEntry",
        async:false,
        data: {
            VehicleId: VehicleId, Campus: Campus, VehicleNo: VehicleNo, Vendor: Vendor, InvoiceNo: InvoiceNo, Description: Description, EntryDate: Date, FCId: FCId,VehicleCostId:VehicleCostId
        },
        success: function (data) {
            if (data.statusval == "added") {
                $("#hdnFCId").val(data.FCId);
                SucessMsg("Added Sucessfully");
                return true;
            }
            if (data.statusval == "updated") {
                $("#hdnFCId").val(data.FCId);
                SucessMsg("Updated Successfully");
                return true;
            }
            //$("#VehicleVehicleFCEntryListJqGrid").trigger('reloadGrid');
            //$("input[type=text], textarea").val("");
            //$('#hdnVehicleId').val('');
            //$('#hdncampus').val('');
            //$("#InvoiceNo").val('');
            //$("#Description").val('');
            //SucessMsg("Added Sucessfully");
        }

    });
}