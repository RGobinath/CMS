jQuery(function ($) {

    //$('#EM_SparePartsUsedfile').ace_file_input();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#VehicleElectricalMaintenanceList").jqGrid('setGridWidth', ($(".col-xs-12").width())-40);
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#VehicleElectricalMaintenanceList").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#VehicleElectricalMaintenanceList").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    var Campus;
    $("#VehicleElectricalMaintenanceList").jqGrid({
        url: '/Transport/VehicleElectricalMaintenanceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id','Campus', 'Vehicle No', 'Date Of Service', 'Service Provider', 'Service Cost', 'Bill No', 'SpareParts Used', 'Description', 'Spare Parts', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
            {name:'Campus',index:'Campus'},
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'EDateOfService', index: 'EDateOfService' },
            { name: 'EServiceProvider', index: 'EServiceProvider' },
            { name: 'EServiceCost', index: 'EServiceCost' },
            { name: 'EBillNo', index: 'EBillNo' },
            { name: 'ESparePartsUsed', index: 'ESparePartsUsed' },
            { name: 'EDescription', index: 'EDescription' },
            { name: 'EM_SparePartsUsedfile', index: 'EM_SparePartsUsedfile' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true }
            ],
        pager: '#VehicleElectricalMaintenanceListPager',
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
        caption: 'Electrical Maintenance List'
    });
    $("#VehicleElectricalMaintenanceList").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#VehicleElectricalMaintenanceList").jqGrid('navGrid', '#VehicleElectricalMaintenanceListPager',
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
            { url: '/Transport/DeleteVehicleElectricalMaintenanceById' }, {})
    $("#VehicleElectricalMaintenanceList").jqGrid('navButtonAdd', '#VehicleElectricalMaintenanceListPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            var Campus = $("#ddlcampus").val();
            var VehicleNo = $("#gs_VehicleNo").val();
            var EDateOfService = $("#gs_EDateOfService").val();
            var EServiceProvider = $("#gs_EServiceProvider").val();
            var ServiceCost = $("#gs_EServiceCost").val();
            var EBillNo = $("#gs_EBillNo").val();
            var ESparePartsUsed = $("#gs_ESparePartsUsed").val();
            var EDescription = $("#gs_EDescription").val();
            var EM_SparePartsUsedfile = $("#gs_EM_SparePartsUsedfile").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/VehicleElectricalMaintenanceJqGrid" + '?ExportType=Excel'
                +'&Campus='+Campus
                    + '&VehicleId=' + VehicleId
                    + '&VehicleNo=' + VehicleNo
                    + '&EDateOfService=' + EDateOfService
                    + '&EServiceProvider=' + EServiceProvider
                    + '&EServiceCost=' + ServiceCost
                    + '&EBillNo=' + EBillNo
                    + '&ESparePartsUsed=' + ESparePartsUsed
                    + '&EDescription=' + EDescription
                    + '&EM_SparePartsUsedfile=' + EM_SparePartsUsedfile
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
        $("#VehicleElectricalMaintenanceList").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});

function AddVehicleElectricalMaintenance() {
    var VehicleId = $("#hdnVehicleId").val();
    var Campus = $("#ddlcampus").val();
    var VehicleNo = $('#VehicleNo').val();
    var EDateOfService = $('#EDateOfService').val();
    var EServiceProvider = $('#EServiceProvider').val();
    var EServiceCost = $('#EServiceCost').val();
    var EBillNo = $('#EBillNo').val();
    var ESparePartsUsed = $('#ESparePartsUsed').val();
    var EDescription = $('#EDescription').val();
    var EM_SparePartsUsedfile = $("#EM_SparePartsUsedfile").val();
    if (Campus==''|| VehicleId == '' || VehicleNo == '' || EDateOfService == '' || EServiceProvider == '' || EServiceCost == '' || EBillNo == '' || ESparePartsUsed == '' || EDescription == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/Transport/AddVehicleElectricalMaintenance',
        data: { VehicleId: VehicleId,Campus:Campus, VehicleNo: VehicleNo, EDateOfService: EDateOfService, EServiceProvider: EServiceProvider, EServiceCost: EServiceCost,
            EBillNo: EBillNo, ESparePartsUsed: ESparePartsUsed, EDescription: EDescription, EM_SparePartsUsedfile: EM_SparePartsUsedfile
        },
        success: function (Id) {
            $("input[type=text], textarea, select").val("");
            $("#VehicleElectricalMaintenanceList").trigger('reloadGrid');
            UploadEM_SparePartsUsedfile(Id);
        }
    });
}

function UploadEM_SparePartsUsedfile(Id) {
    if ($("#EM_SparePartsUsedfile").val() != "") {
        ajaxFileUploadElectricalMaintenance(Id, "EMSP");
        $("#EM_SparePartsUsedfile").val("");
    }
}
function ajaxFileUploadElectricalMaintenance(Id, AppName) {
    $.ajaxFileUpload({
        url: '/UploadDocuments/',
        secureuri: false,
        fileElementId: 'EM_SparePartsUsedfile',
        dataType: 'json',
        data: { Id: Id, AppName: AppName },
        success: function (data) {
            alert(data);
            InfoMsg(data);
        }
    })
}

function ShowEMsparePartsUsed(Id, FileName) {
    var AppName = 'EMSP';
    window.location.href = "/Transport/uploaddisplay?Id=" + Id + '&FileName=' + FileName + '&AppName=' + AppName;
    // processBusy.dialog('close');
}

