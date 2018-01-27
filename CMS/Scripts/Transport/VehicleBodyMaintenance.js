jQuery(function ($) {
    $('#BM_SparePartsUsedfile').ace_file_input();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#VehicleBodyMaintenanceList").jqGrid('setGridWidth', $(".col-xs-12").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#VehicleBodyMaintenanceList").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#VehicleBodyMaintenanceList").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();

    $("#VehicleBodyMaintenanceList").jqGrid({
        url: '/Transport/VehicleBodyMaintenanceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Type Of Body', 'Date Of Repair', 'Type Of Repair', 'Parts Required', 'Service Provider', 'Service Cost', 'Bill No', 'Description', 'Spare Parts', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'BTypeOfBody', index: 'BTypeOfBody' },
            { name: 'BDateOfRepair', index: 'BDateOfRepair' },
            { name: 'BTypeOfRepair', index: 'BTypeOfRepair' },
            { name: 'BPartsRequired', index: 'BPartsRequired' },
            { name: 'BServiceProvider', index: 'BServiceProvider' },
            { name: 'BServiceCost', index: 'BServiceCost' },
            { name: 'BBillNo', index: 'BBillNo' },
            { name: 'BDescription', index: 'BDescription' },
            { name: 'BM_SparePartsUsedfile', index: 'BM_SparePartsUsedfile' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true }
            ],
        pager: '#VehicleBodyMaintenanceListPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        //            width: 1225,
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
        caption: 'Body Maintenance List'
    });
    $("#VehicleBodyMaintenanceList").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#VehicleBodyMaintenanceList").jqGrid('navGrid', '#VehicleBodyMaintenanceListPager',
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
            { url: '/Transport/DeleteVehicleBodyMaintenanceById' }, {})
    $("#VehicleBodyMaintenanceList").jqGrid('navButtonAdd', '#VehicleBodyMaintenanceListPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            var VehicleNo = $("#gs_VehicleNo").val();
            var BTypeOfBody = $("#gs_BTypeOfBody").val();
            var BDateOfRepair = $("#gs_BDateOfRepair").val();
            var BTypeOfRepair = $("#gs_BTypeOfRepair").val();
            var BPartsRequired = $("#gs_BPartsRequired").val();
            var BServiceProvider = $("#gs_BServiceProvider").val();
            var BServiceCost = $("#gs_BServiceCost").val();
            var BBillNo = $("#gs_BBillNo").val();
            var BDescription = $("#gs_BDescription").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            var BM_SparePartsUsedfile = $("#gs_BM_SparePartsUsedfile").val();
            window.open("/Transport/VehicleBodyMaintenanceJqGrid" + '?ExportType=Excel'
                    + '&VehicleNo=' + VehicleNo
                    + '&BTypeOfBody=' + BTypeOfBody
                    + '&BDateOfRepair=' + BDateOfRepair
                    + '&BTypeOfRepair=' + BTypeOfRepair
                    + '&BPartsRequired=' + BPartsRequired
                    + '&BServiceProvider=' + BServiceProvider
                    + '&BServiceCost=' + BServiceCost
                    + '&BBillNo=' + BBillNo
                    + '&BDescription=' + BDescription
                    + '&CreatedDate=' + CreatedDate
                    + '&CreatedBy=' + CreatedBy
                    + '&BM_SparePartsUsedfile=' + BM_SparePartsUsedfile
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
        $("#VehicleBodyMaintenanceList").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});

function AddVehicleBodyMaintenance() {

    var VehicleId = $("#hdnVehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var BTypeOfBody = $('#BTypeOfBody').val();
    var BDateOfRepair = $('#BDateOfRepair').val();
    var BTypeOfRepair = $('#BTypeOfRepair').val();
    var BPartsRequired = $('#BPartsRequired').val();
    var BServiceProvider = $('#BServiceProvider').val();
    var BServiceCost = $('#BServiceCost').val();
    var BBillNo = $("#BBillNo").val();
    var BDescription = $("#BDescription").val();
    var BM_SparePartsUsedfile = $("#BM_SparePartsUsedfile").val();
    if (VehicleId == '' || VehicleNo == '' || BTypeOfBody == '' || BDateOfRepair == '' || BTypeOfRepair == '' || BPartsRequired == '' || BServiceProvider == ''
          || BServiceCost == '' || BBillNo == '' || BDescription == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/Transport/AddVehicleBodyMaintenance',
        data: {
            VehicleId: VehicleId, VehicleNo: VehicleNo, BTypeOfBody: BTypeOfBody, BDateOfRepair: BDateOfRepair, BTypeOfRepair: BTypeOfRepair,
            BPartsRequired: BPartsRequired, BServiceProvider: BServiceProvider, BServiceCost: BServiceCost, BBillNo: BBillNo, BDescription: BDescription, BM_SparePartsUsedfile: BM_SparePartsUsedfile
        },
        success: function (Id) {
            $("input[type=text], textarea, select").val("");
            $("#VehicleBodyMaintenanceList").trigger('reloadGrid');
            UploadBM_SparePartsUsedfile(Id);
        }
    });
}

function UploadBM_SparePartsUsedfile(Id) {
    if ($("#BM_SparePartsUsedfile").val() != "") {
        ajaxFileUploadBodyMaintenance(Id, "BMSP");
        $("#BM_SparePartsUsedfile").val("");
    }
}
function ajaxFileUploadBodyMaintenance(Id, AppName) {
    $.ajaxFileUpload({
        url: '/UploadDocuments/',
        secureuri: false,
        fileElementId: 'BM_SparePartsUsedfile',
        dataType: 'json',
        data: { Id: Id, AppName: AppName },
        success: function (data) {
            alert(data);
            InfoMsg(data);
        }
    })
}

function ShowBMsparePartsUsed(Id, FileName) {
    var AppName = 'BMSP';
    window.location.href = "/Transport/uploaddisplay?Id=" + Id + '&FileName=' + FileName + '&AppName=' + AppName;
    // processBusy.dialog('close');
}
