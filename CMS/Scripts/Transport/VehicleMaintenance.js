jQuery(function ($) {
    //$('#VM_SparePartsUsedfile').ace_file_input();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#VehicleMaintenanceList").jqGrid('setGridWidth', ($(".col-xs-12").width())-40);
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#VehicleMaintenanceList").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#VehicleMaintenanceList").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    $("#tdVehicleBreakDownLocation").hide();
    $("#tdVehicleDateofBreakDown").hide();
    $("#btnSave2").hide();
    $("#VehicleMaintenanceType").change(function () {
        if ($(this).val() == "Breakdown") {
            $("#tdVehicleBreakDownLocation").show();
            $("#tdVehicleDateofBreakDown").show();
            $("#btnSave2").show();
            $("#btnSave1").hide();
        }
        else {
            $("#tdVehicleBreakDownLocation").hide();
            $("#tdVehicleDateofBreakDown").hide();
            $("#btnSave2").hide();
            $("#btnSave1").show();
        }
    });

    $("#VehicleMaintenanceList").jqGrid({
        url: '/Transport/VehicleMaintenanceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Maintenance Type', 'Breakdown Date', 'Breakdown Location', 'Planned Date of Service', 'Actual Date of Service', 'Service Provider', 'Service Cost', 'Bill No', 'Spare Parts Used', 'Spare Parts', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 90 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 90 },
            { name: 'VehicleNo', index: 'VehicleNo', width: 90 },
            { name: 'VehicleMaintenanceType', index: 'VehicleMaintenanceType', width: 90 },
            { name: 'VehicleDateOfBreakdown', index: 'VehicleDateOfBreakdown', width: 90 },
            { name: 'VehicleBreakdownLocation', index: 'VehicleBreakdownLocation', width: 90 },
            { name: 'VehiclePlannedDateOfService', index: 'VehiclePlannedDateOfService', width: 90 },
            { name: 'VehicleActualDateOfService', index: 'VehicleActualDateOfService', width: 90 },
            { name: 'VehicleServiceProvider', index: 'VehicleServiceProvider', width: 90 },
            { name: 'VehicleSeviceCost', index: 'VehicleSeviceCost', width: 90 },
            { name: 'VehicleServiceBillNo', index: 'VehicleServiceBillNo', width: 90 },
            { name: 'VehicleSparePartsUsed', index: 'VehicleSparePartsUsed', width: 90 },
            { name: 'VM_SparePartsUsedfile', index: 'VM_SparePartsUsedfile', width: 90 },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true, width: 90 },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true, width: 90 }
            ],
        pager: '#VehicleMaintenanceListPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
       // autoWidth: true,
        //            width: 1225,
        // shrinktofit: true,
        height: 130,
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
        caption: 'Mechanical Maintenance List'
    });
//    $(window).triggerHandler('resize.jqGrid');
    $("#VehicleMaintenanceList").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#VehicleMaintenanceList").jqGrid('navGrid', '#VehicleMaintenanceListPager',
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
            { url: '/Transport/DeleteVehicleMaintenanceById' }, {})

    $("#VehicleMaintenanceList").jqGrid('navButtonAdd', '#VehicleMaintenanceListPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            var VehicleNo = $("#gs_VehicleNo").val();
            var VehicleMaintenanceType = $("#gs_VehicleMaintenanceType").val();
            var VehicleDateOfBreakdown = $("#gs_VehicleDateOfBreakdown").val();
            var VehicleBreakdownLocation = $("#gs_VehicleBreakdownLocation").val();
            var VehiclePlannedDateOfService = $("#gs_VehiclePlannedDateOfService").val();
            var VehicleActualDateOfService = $("#gs_VehicleActualDateOfService").val();
            var VehicleServiceProvider = $("#gs_VehicleServiceProvider").val();
            var VehicleSeviceCost = $("#gs_VehicleSeviceCost").val();
            var VehicleServiceBillNo = $("#gs_VehicleServiceBillNo").val();
            var VehicleSparePartsUsed = $("#gs_VehicleSparePartsUsed").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/VehicleMaintenanceJqGrid" + '?ExportType=Excel'
                    + '&VehicleId=' + VehicleId
                    + '&VehicleNo=' + VehicleNo
                    + '&VehicleMaintenanceType=' + VehicleMaintenanceType
                    + '&VehicleDateOfBreakdown=' + VehicleDateOfBreakdown
                    + '&VehicleBreakdownLocation=' + VehicleBreakdownLocation
                    + '&VehiclePlannedDateOfService=' + VehiclePlannedDateOfService
                    + '&VehicleActualDateOfService=' + VehicleActualDateOfService
                    + '&VehicleServiceProvider=' + VehicleServiceProvider
                    + '&VehicleSeviceCost=' + VehicleSeviceCost
                    + '&VehicleServiceBillNo=' + VehicleServiceBillNo
                    + '&VehicleSparePartsUsed=' + VehicleSparePartsUsed
                    + '&CreatedDate=' + CreatedDate
                    + '&CreatedBy=' + CreatedBy
                    + '&rows=9999');
        }
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


    $(document).on('ajaxloadstart', function (e) {
        $("#VehicleMaintenanceList").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});

function AddVehicleMaintenance() {

    var VehicleId = $("#hdnVehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var VehicleMaintenanceType = $('#VehicleMaintenanceType').val();
    var VehiclePlannedDateOfService = $('#VehiclePlannedDateOfService').val();
    var VehicleActualDateOfService = $('#VehicleActualDateOfService').val();
    var VehicleServiceProvider = $('#VehicleServiceProvider').val();
    var VehicleSeviceCost = $('#VehicleSeviceCost').val();
    var VehicleMaintenanceDescription = $('#VehicleMaintenanceDescription').val();
    var VehicleServiceBillNo = $('#VehicleServiceBillNo').val();
    var VehicleBreakdownLocation = $('#VehicleBreakdownLocation').val();
    var VehicleDateOfBreakdown = $('#VehicleDateOfBreakdown').val();
    var VehicleSparePartsUsed = $("#VehicleSparePartsUsed").val();
    var VM_SparePartsUsedfile = $("#VM_SparePartsUsedfile").val();
    if (VehicleId == '' || VehicleNo == '' || VehicleMaintenanceType == '' || VehiclePlannedDateOfService == '' || VehicleActualDateOfService == '' || VehicleServiceProvider == ''
          || VehicleSeviceCost == '' || VehicleMaintenanceDescription == '' || VehicleServiceBillNo == '' || VehicleSparePartsUsed == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (VehicleMaintenanceType == "Breakdown") {
        if (VehicleBreakdownLocation == '' || VehicleDateOfBreakdown == '') {
            ErrMsg("Please fill Breakdown Location, Date Of Breakdown.");
            return false;
        }
    }

    $.ajax({
        type: 'POST',
        url: '/Transport/AddVehicleMaintenance',
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, VehicleMaintenanceType: VehicleMaintenanceType, VehiclePlannedDateOfService: VehiclePlannedDateOfService, VehicleActualDateOfService: VehicleActualDateOfService,
            VehicleServiceProvider: VehicleServiceProvider, VehicleSeviceCost: VehicleSeviceCost, VehicleMaintenanceDescription: VehicleMaintenanceDescription,
            VehicleServiceBillNo: VehicleServiceBillNo, VehicleBreakdownLocation: VehicleBreakdownLocation, VehicleDateOfBreakdown: VehicleDateOfBreakdown, VehicleSparePartsUsed: VehicleSparePartsUsed, VM_SparePartsUsedfile: VM_SparePartsUsedfile
        },
        success: function (Id) {
            $("input[type=text], textarea, select").val("");
            $("#VehicleMaintenanceList").trigger('reloadGrid');
            UploadImage(Id);
        }
    });
}

function UploadImage(Id) {
    debugger;
    if ($("#VM_SparePartsUsedfile").val() != "") {
        ajaxFileUpload(Id, "VMSP");
        $("#VM_SparePartsUsedfile").val("");
    }
}
function ajaxFileUpload(Id, AppName) {
    debugger;
    $.ajaxFileUpload({
        url: '@Url.Action("UploadDocuments")',
        secureuri: false,
        fileElementId: 'VM_SparePartsUsedfile',
        dataType: 'json',
        data: { Id: Id, AppName: AppName },
        success: function (data) {
            alert(data);
            InfoMsg(data);
        }
    })
}

function ShowVMsparePartsUsed(Id, FileName) {
    var AppName = 'VMSP';
    window.location.href = "/Transport/uploaddisplay?Id=" + Id + '&FileName=' + FileName + '&AppName=' + AppName;
    // processBusy.dialog('close');
}

