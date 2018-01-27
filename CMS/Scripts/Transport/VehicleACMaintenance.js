jQuery(function ($) {
    $('#AM_SparePartsUsedfile').ace_file_input();

    var VehicleId = $("#hdnVehicleId").val();
    $("#tdACBreakDownLocation").hide();
    $("#tdACDateofBreakDown").hide();
    $("#AcbtnSave2").hide();
    $("#ACMaintenanceType").change(function () {
        if ($(this).val() == "Breakdown") {
            $("#tdACBreakDownLocation").show();
            $("#tdACDateofBreakDown").show();
            $("#AcbtnSave1").hide();
            $("#AcbtnSave2").show();
        }
        else {
            $("#tdACBreakDownLocation").hide();
            $("#tdACDateofBreakDown").hide();
            $("#AcbtnSave1").show();
            $("#AcbtnSave2").hide();
        }
    });

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#VehicleACMaintenanceList").jqGrid('setGridWidth', $(".col-xs-12").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#VehicleACMaintenanceList").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#VehicleACMaintenanceList").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    $("#VehicleACMaintenanceList").jqGrid({
        url: '/Transport/VehicleACMaintenanceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'AC Model', 'Maintenance Type', 'Breakdown Date', 'Breakdown Location', 'Planned Date of Service', 'Actual Date of Service', 'Service Provider', 'Service Cost', 'Bill No', 'Spare Parts Used', 'Spare Parts', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'ACModel', index: 'ACModel' },
            { name: 'ACMaintenanceType', index: 'ACMaintenanceType' },
            { name: 'ACDateOfBreakdown', index: 'ACDateOfBreakdown' },
            { name: 'ACBreakdownLocation', index: 'ACBreakdownLocation' },
            { name: 'ACPlannedDateOfService', index: 'ACPlannedDateOfService' },
            { name: 'ACActualDateOfService', index: 'ACActualDateOfService' },
            { name: 'ACServiceProvider', index: 'ACServiceProvider' },
            { name: 'ACServiceCost', index: 'ACServiceCost' },
            { name: 'ACServiceBillNo', index: 'ACServiceBillNo' },
            { name: 'ACSparePartsUsed', index: 'ACSparePartsUsed' },
             { name: 'AM_SparePartsUsedfile', index: 'AM_SparePartsUsedfile' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true }
            ],
        pager: '#VehicleACMaintenanceListPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1225,
        //        shrinktofit: true,
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
        caption: 'AC Maintenance List'
    });
    $("#VehicleACMaintenanceList").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#VehicleACMaintenanceList").jqGrid('navGrid', '#VehicleACMaintenanceListPager',
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
            { url: '/Transport/DeleteVehicleACMaintenanceById' }, {})

    $("#VehicleACMaintenanceList").jqGrid('navButtonAdd', '#VehicleACMaintenanceListPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            var VehicleNo = $("#gs_VehicleNo").val();
            var ACModel = $("#gs_ACModel").val();
            var ACMaintenanceType = $("#gs_ACMaintenanceType").val();
            var ACDateOfBreakdown = $("#gs_ACDateOfBreakdown").val();
            var ACBreakdownLocation = $("#gs_ACBreakdownLocation").val();
            var ACPlannedDateOfService = $("#gs_ACPlannedDateOfService").val();
            var ACActualDateOfService = $("#gs_ACActualDateOfService").val();
            var ACServiceProvider = $("#gs_ACServiceProvider").val();
            var ACServiceCost = $("#gs_ACServiceCost").val();
            var ACServiceBillNo = $("#gs_ACServiceBillNo").val();
            var ACSparePartsUsed = $("#gs_ACSparePartsUsed").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();

            window.open("/Transport/VehicleACMaintenanceJqGrid" + '?ExportType=Excel'
                    + '&VehicleId=' + VehicleId
                    + '&VehicleNo=' + VehicleNo
                    + '&ACModel=' + ACModel
                    + '&ACMaintenanceType=' + ACMaintenanceType
                    + '&ACDateOfBreakdown=' + ACDateOfBreakdown
                    + '&ACBreakdownLocation=' + ACBreakdownLocation
                    + '&ACPlannedDateOfService=' + ACPlannedDateOfService
                    + '&ACActualDateOfService=' + ACActualDateOfService
                    + '&ACServiceProvider=' + ACServiceProvider
                    + '&ACServiceCost=' + ACServiceCost
                    + '&ACServiceBillNo=' + ACServiceBillNo
                    + '&ACSparePartsUsed=' + ACSparePartsUsed
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
        $("#VehicleACMaintenanceList").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});
function AddVehicleACMaintenance() {
    debugger;
    var VehicleId = $("#hdnVehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var ACMaintenanceType = $('#ACMaintenanceType').val();
    var ACModel = $('#ACModel').val();
    var ACPlannedDateOfService = $('#ACPlannedDateOfService').val();


    var ACActualDateOfService = $('#ACActualDateOfService').val();
    var ACServiceProvider = $('#ACServiceProvider').val();
    var ACServiceCost = $('#ACServiceCost').val();
    var ACMaintenanceDescription = $('#ACMaintenanceDescription').val();
    var ACServiceBillNo = $('#ACServiceBillNo').val();
    var ACBreakdownLocation = $('#ACBreakdownLocation').val();
    var ACDateofBreakDown = $('#ACDateofBreakDown').val();
    var ACSparePartsUsed = $("#ACSparePartsUsed").val();
    var AM_SparePartsUsedfile = $("#AM_SparePartsUsedfile").val();
    if (VehicleId == '' || VehicleNo == '' || ACMaintenanceType == '' || ACModel == '' || ACPlannedDateOfService == '' || ACActualDateOfService == ''
         || ACServiceProvider == '' || ACServiceCost == '' || ACMaintenanceDescription == '' || ACServiceBillNo == '' || ACSparePartsUsed == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    if (ACMaintenanceType == "Breakdown") {
        if (ACBreakdownLocation == '' || ACDateofBreakDown == '') {
            ErrMsg("Please fill Breakdown Location, Date Of Breakdown.");
            return false;
        }
    }

    $.ajax({
        type: 'POST',
        url: '/Transport/AddVehicleACMaintenance',
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, ACMaintenanceType: ACMaintenanceType, ACModel: ACModel,
            ACPlannedDateOfService: ACPlannedDateOfService, ACActualDateOfService: $('#ACActualDateOfService').val(), ACServiceProvider: ACServiceProvider,
            ACServiceCost: ACServiceCost, ACMaintenanceDescription: ACMaintenanceDescription, ACServiceBillNo: ACServiceBillNo, ACBreakdownLocation: ACBreakdownLocation,
            ACDateofBreakDown: ACDateofBreakDown, ACSparePartsUsed: ACSparePartsUsed, AM_SparePartsUsedfile: AM_SparePartsUsedfile
        },
        success: function (Id) {
            $("input[type=text], textarea, select").val("");
            $("#VehicleACMaintenanceList").trigger('reloadGrid');
            UploadAM_SparePartsUsedfile(Id);
        }
    });
}

function ajaxFileUploadACMaintenance(Id, AppName) {
    debugger;
    $.ajaxFileUpload({
        url: '/UploadDocuments',
        secureuri: false,
        fileElementId: 'AM_SparePartsUsedfile',
        dataType: 'json',
        data: { Id: Id, AppName: AppName },
        success: function (data) {
            alert(data);
            InfoMsg(data);
        }
    })
}
function UploadAM_SparePartsUsedfile(Id) {
    debugger;
    if ($("#AM_SparePartsUsedfile").val() != "") {
        ajaxFileUploadACMaintenance(Id, "AMSP");
        $("#AM_SparePartsUsedfile").val("");
    }
}
function ShowAMsparePartsUsed(Id, FileName) {
    debugger;
    var AppName = 'AMSP';
    window.location.href = "/Transport/uploaddisplay?Id=" + Id + '&FileName=' + FileName + '&AppName=' + AppName;
    // processBusy.dialog('close');
}