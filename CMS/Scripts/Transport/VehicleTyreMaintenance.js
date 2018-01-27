jQuery(function ($) {
    var VehicleId = $("#hdnVehicleId").val();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#VehicleTyreMaintenanceList").jqGrid('setGridWidth', $(".col-xs-12").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#VehicleTyreMaintenanceList").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#VehicleTyreMaintenanceList").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    $("#VehicleTyreMaintenanceList").jqGrid({
        url: '/Transport/VehicleTyreMaintenanceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Maintenance Type', 'Location', 'Tyre Type', 'Make', 'Model', 'Size', 'Assigned Date', 'Milometer Reading', 'Reason For Removing',
             'Cost', 'Bill No', 'Date Of Alignment', 'Date Of Rotation', 'Date Of WheelService', 'Service Cost', 'Maintenance Bill No', 'Tyre Date Of Service', 'Tyre Service Provider', 'Cost Of Service', 'Tyre Serviced By', 'Tyre Service BillNo', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'TyreMaintenanceType', index: 'TyreMaintenanceType' },
            { name: 'TyreLocation', index: 'TyreLocation' },
            { name: 'TypeOfTyre', index: 'TypeOfTyre' },
            { name: 'TyreMake', index: 'TyreMake' },
            { name: 'TyreModel', index: 'TyreModel' },
            { name: 'TyreSize', index: 'TyreSize' },
            { name: 'TyreDateOfEntry', index: 'TyreDateOfEntry' },
            { name: 'TyreMilometerReading', index: 'TyreMilometerReading' },
            { name: 'TyreReasonForRemoving', index: 'TyreReasonForRemoving' },
            { name: 'TyreCost', index: 'TyreCost' },
            { name: 'TyreBillNo', index: 'TyreBillNo' },
            { name: 'TyreDateOfAlignment', index: 'TyreDateOfAlignment' },
            { name: 'TyreDateOfRotation', index: 'TyreDateOfRotation' },
            { name: 'TyreDateOfWheelService', index: 'TyreDateOfWheelService' },
            { name: 'TyreServiceCost', index: 'TyreServiceCost' },
            { name: 'TyreMaintenanceBillNo', index: 'TyreMaintenanceBillNo' },
            { name: 'TyreDateOfService', index: 'TyreDateOfService' },
            { name: 'TyreServiceProvider', index: 'TyreServiceProvider' },
            { name: 'CostOfService', index: 'CostOfService' },
            { name: 'TyreServicedBy', index: 'TyreServicedBy' },
            { name: 'TyreServiceBillNo', index: 'TyreServiceBillNo' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
            ],
        pager: '#VehicleTyreMaintenanceListPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1225,
        //shrinktofit: true,
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
        caption: 'Tyre Maintenance List'
    });
    $("#VehicleTyreMaintenanceList").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#VehicleTyreMaintenanceList").jqGrid('navGrid', '#VehicleTyreMaintenanceListPager',
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
            { url: '/Transport/DeleteVehicleTyreMaintenanceById' }, {})

    $("#VehicleTyreMaintenanceList").jqGrid('navButtonAdd', '#VehicleTyreMaintenanceListPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            window.open('/Transport/VehicleTyreMaintenanceJqGrid?ExportType=Excel'
                     + '&VehicleId=' + VehicleId
                    + '&VehicleNo=' + $("#gs_VehicleNo").val()
                    + '&TyreMaintenanceType=' + $("#gs_TyreMaintenanceType").val()
                    + '&TyreLocation=' + $("#gs_TyreLocation").val()
                    + '&TypeOfTyre=' + $("#gs_TypeOfTyre").val()
                    + '&TyreMake=' + $("#gs_TyreMake").val()
                    + '&TyreModel=' + $("#gs_TyreModel").val()
                    + '&TyreSize=' + $("#gs_TyreSize").val()
                    + '&TyreDateOfEntry=' + $("#gs_TyreDateOfEntry").val()
                    + '&TyreMilometerReading=' + $("#gs_TyreMilometerReading").val()
                    + '&TyreCost=' + $("#gs_TyreCost").val()
                    + '&TyreBillNo=' + $("#gs_TyreBillNo").val()
                    + '&TyreDateOfAlignment=' + $("#gs_TyreDateOfAlignment").val()
                    + '&TyreDateOfRotation=' + $("#gs_TyreDateOfRotation").val()
                    + '&TyreDateOfWheelService=' + $("#gs_TyreDateOfWheelService").val()
                    + '&TyreServiceCost=' + $("#gs_TyreServiceCost").val()
                    + '&TyreMaintenanceBillNo=' + $("#gs_TyreMaintenanceBillNo").val()
                    + '&CreatedDate=' + $("#gs_CreatedDate").val()
                    + '&CreatedBy=' + $("#gs_CreatedBy").val()
                    + '&rows=9999');
        }
    });

    $(".New").hide();
    $(".Maintenance").hide();
    $(".Service").hide();
    $("#TyreSearchTD").hide();
    $("#TyreMaintenanceType").change(function () {
        debugger;
        if ($(this).val() == "New") {
            $(".New").show();
            $(".Maintenance").hide();
            $(".Service").hide();
            $('.Maintenance, .Service').find($("input[type=text], textarea, select")).val("");
            $("#TyreSearchTD").show();
        }
        else if ($(this).val() == "Maintenance") {
            $("#TyreSearchTD").hide();
            $(".Maintenance").show();
            $(".New").hide();
            $(".Service").hide();
            $('.New, .Service').find($("input[type=text], textarea, select")).val("");
        }
        else if ($(this).val() == "Service") {
            $("#TyreSearchTD").hide();
            $(".Service").show();
            $(".New").hide();
            $(".Maintenance").hide();
            $('.New, .Maintenance').find($("input[type=text], textarea, select")).val("");
        }
        else {
            $("#TyreSearchTD").hide();
            $(".New").hide();
            $(".Maintenance").hide();
            $(".Service").hide();
            $('.Maintenance').find($("input[type=text], textarea, select")).val("");
            $('.New').find($("input[type=text], textarea, select")).val("");
            $('.Service').find($("input[type=text], textarea, select")).val("");
        }
    });

    $("#TyreSearch").click(function () {
        debugger;
        LoadPopupDynamicaly("/Transport/TyreSearch", $('#DivTyreSearch'),
            function () {
                LoadSetGridParam($('#TyreDetailsList'), "/Transport/GetTyresFromStock")
            }, "", 1225);
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
        $("#VehicleTyreMaintenanceList").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});
function AddVehicleTyreMaintenance() {
    var VehicleId = $("#hdnVehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var TyreMaintenanceType = $('#TyreMaintenanceType').val();
    var TyreLocation = $('#TyreLocation').val();
    var TypeOfTyre = $('#TypeOfTyre').val();
    var TyreMake = $('#TyreMake').val();
    var TyreModel = $('#TyreModel').val();
    var TyreSize = $('#TyreSize').val();
    //  var TyreDateOfEntry = $("#TyreDateOfEntry").val();
    var TyreMilometerReading = $("#TyreMilometerReading").val();
    var TyreCost = $('#TyreCost').val();
    var TyreNo = $("#TyreNo").val();
    var TyreBillNo = $('#TyreBillNo').val();

    var TyreDateOfAlignment = $('#TyreDateOfAlignment').val();
    var TyreDateOfRotation = $('#TyreDateOfRotation').val();
    var TyreDateOfWheelService = $("#TyreDateOfWheelService").val();
    var TyreServiceCost = $("#TyreServiceCost").val();
    var TyreMaintenanceBillNo = $("#TyreMaintenanceBillNo").val();

    var TyreDateOfService = $("#TyreDateOfService").val();
    var TyreServiceProvider = $("#TyreServiceProvider").val();
    var CostOfService = $("#CostOfService").val();
    var TyreServicedBy = $("#TyreServicedBy").val();
    var TyreServiceBillNo = $("#TyreServiceBillNo").val();
    var TyreReasonForRemoving = $("#TyreReasonForRemoving").val();
    var TyreAssignedDate = $("#TyreAssignedDate").val();
    if (TyreMaintenanceType == "New") {
        if (VehicleId == '' || VehicleNo == '' || TyreMaintenanceType == '' || TyreLocation == '' || TypeOfTyre == '' || TyreMake == '' || TyreModel == ''
                 || TyreSize == '' || TyreAssignedDate == '' || TyreMilometerReading == '' || TyreCost == '' || TyreNo == '' || TyreBillNo == '' || TyreReasonForRemoving == '') {
            ErrMsg("Please fill all the mandatory fields.");
            return false;
        }
    }

    if (TyreMaintenanceType == "Maintenance") {
        if (VehicleId == '' || VehicleNo == '' || TyreDateOfAlignment == '' || TyreDateOfRotation == '' || TyreDateOfWheelService == '' || TyreServiceCost == ''
                 || TyreMaintenanceBillNo == '') {
            ErrMsg("Please fill all the mandatory fields.");
            return false;
        }
    }

    if (TyreMaintenanceType == "Service") {
        if (VehicleId == '' || VehicleNo == '' || TyreDateOfService == '' || TyreServiceProvider == '' || CostOfService == '' || TyreServicedBy == '' || TyreServiceBillNo == '') {
            ErrMsg("Please fill all the mandatory fields.");
            return false;
        }
    }

    $.ajax({
        type: 'POST',
        url: '/Transport/AddVehicleTyreMaintenance',
        data: {
            VehicleId: VehicleId, VehicleNo: VehicleNo, TyreMaintenanceType: TyreMaintenanceType, TyreLocation: TyreLocation, TypeOfTyre: TypeOfTyre,
            TyreMake: TyreMake, TyreModel: TyreModel, TyreSize: TyreSize, TyreAssignedDate: TyreAssignedDate, TyreMilometerReading: TyreMilometerReading, TyreReasonForRemoving: TyreReasonForRemoving,
            TyreCost: TyreCost, TyreNo: TyreNo, TyreBillNo: TyreBillNo, TyreDateOfAlignment: TyreDateOfAlignment, TyreDateOfRotation: TyreDateOfRotation, TyreDateOfWheelService: TyreDateOfWheelService,
            TyreServiceCost: TyreServiceCost, TyreMaintenanceBillNo: TyreMaintenanceBillNo, TyreDateOfService: TyreDateOfService, TyreServiceProvider: TyreServiceProvider,
            CostOfService: CostOfService, TyreServicedBy: TyreServicedBy, TyreServiceBillNo: TyreServiceBillNo
        },
        success: function (data) {
            $("input[type=text], textarea, select").val("");
            $("#VehicleTyreMaintenanceList").trigger('reloadGrid');
        }
    });


}
