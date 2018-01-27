jQuery(function ($) {

    $("#Driver").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $('#FCertificate').ace_file_input();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#jqGridFitnessCertificate").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#jqGridFitnessCertificate").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#jqGridFitnessCertificate").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    $("#jqGridFitnessCertificate").jqGrid({
        url: '/Transport/FitnessCertificateJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'FC Date', 'Next FC Date', 'FC Cost', 'Description', 'FC Work Carried At', 'RTO', 'Driver', 'Fitness Certificate'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 490 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 390 },
            { name: 'VehicleNo',  index: 'VehicleNo' },
            { name: 'FCDate',  index: 'FCDate' },
            { name: 'NextFCDate',  index: 'NextFCDate' },
            { name: 'FCCost', index: 'FCCost' },
            { name: 'Description',  index: 'Description' },
            { name: 'FCWorkCarriedAt',  index: 'FCWorkCarriedAt' },
            { name: 'RTO',  index: 'RTO' },
            { name: 'Driver',  index: 'Driver' },
            { name: 'FCertificate',  index: 'FCertificate' }
            ],
        pager: '#jqGridFitnessCertificatePager',
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
//        width: 1225,
//        shrinktofit: true,
        height: 130,
        viewrecords: true,
        multiselect: true,
        altRows: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Fitness Certificate'
    });
    $("#jqGridFitnessCertificate").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#jqGridFitnessCertificate").jqGrid('navGrid', '#jqGridFitnessCertificatePager',
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
            { url: '/Transport/DeleteFitnessCertificateById' }, {})
    $("#jqGridFitnessCertificate").jqGrid('navButtonAdd', '#jqGridFitnessCertificatePager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            window.open('/Transport/FitnessCertificateJqGrid?ExportType=Excel'
                    + '&VehicleNo=' + $("#gs_VehicleNo").val()
                    + '&FCDate=' + $("#gs_FCDate").val()
                    + '&FCCost=' + $("#gs_FCCost").val()
                    + '&Description=' + $("#gs_Description").val()
                    + '&FCWorkCarriedAt=' + $("#gs_FCWorkCarriedAt").val()
                    + '&RTO=' + $("#gs_RTO").val()
                    + '&Driver=' + $("#gs_Driver").val()
                    + '&FCertificate=' + $("#gs_FCertificate").val()
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
        $("#jqGridFitnessCertificate").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function FitnessCertificateDetailsCreate() {

    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var FCDate = $('#FC_FCDate').val();
    var NextFCDate = $('#FC_NextFCDate').val();
    var FCCost = $('#FC_FCCost').val();
    var Description = $('#FC_Description').val();
    var FCWorkCarriedAt = $('#FC_FCWorkCarriedAt').val();
    var RTO = $('#FC_RTO').val();
    // var FCGivenBy = $('#FC_FCGivenBy').val();
    var Driver = $('#Driver').val();
    var FCertificate = $('#FCertificate').val();
    var FCTaxValidUpto = $("#FCTaxValidUpto").val();
    if (VehicleId == '' || VehicleNo == '' || FCDate == '' || NextFCDate == '' || FCCost == '' || Description == '' || FCWorkCarriedAt == ''
          || FCTaxValidUpto == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (!parseInt(FCCost)) {
        ErrMsg('Numbers only allowed.');
        $('#FC_FCCost').focus();
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/Transport/AddFitnessCertificateDetails",
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, FCDate: FCDate, NextFCDate: NextFCDate, FCCost: FCCost, Description: Description, FCWorkCarriedAt: FCWorkCarriedAt, RTO: RTO, Driver: Driver, FCertificate: FCertificate, FCTaxValidUpto: FCTaxValidUpto },
        success: function (data) {
            ajaxUploadDocs(data);
            $("#jqGridFitnessCertificate").trigger('reloadGrid');
            $("input[type=text], textarea, select").val("");
        }
    });

    $('#FC_FCDate').val("");
    $('#FC_NextFCDate').val("");
    $('#FC_FCCost').val("");
    $('#FC_FCWorkCarriedAt').val("");
    $('#FC_Description').val("");
    $('#FC_RTO').val("");
    // $('#FC_FCGivenBy').val("");
    $('#Driver').val("");

}

function ajaxUploadDocs(Id) {
    var AppName1 = 'FIT';
    $.ajaxFileUpload({
        url: '/Transport/UploadDocuments?Id=' + Id + '&AppName=' + AppName1,
        secureuri: false,
        fileElementId: 'FCertificate',
        dataType: 'json',
        success: function (data, status) {
            jQuery("#jqGridFitnessCertificate").setGridParam({ url: 'Transport/FitnessCertificateJqGrid' }).trigger("reloadGrid");
        },
        error: function (data, status, e) {
        }
    });
}

function uploaddat1(id1, FileName) {

    var AppName1 = 'FIT';
    window.location.href = "/Transport/uploaddisplay?Id=" + id1 + '&FileName=' + FileName + '&AppName=' + AppName1;
    //processBusy.dialog('close');
}