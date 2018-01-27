jQuery(function ($) {
    $('#ICertificate').ace_file_input();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#jqGridInsuranceJqGrid").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#jqGridInsuranceJqGrid").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#jqGridInsuranceJqGrid").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    $("#jqGridInsuranceJqGrid").jqGrid({
        url: '/Transport/InsuranceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Insurance Date', 'Next Insurance Date', 'Insurance Provider', 'Insurance Consultant Name', 'Insurance Declared Value', 'Insurance Certificate'],
        colModel: [
            { name: 'Id',  index: 'Id', key: true, hidden: true,width:430 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 460 },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'InsuranceDate', index: 'InsuranceDate' },
            { name: 'NextInsuranceDate', index: 'NextInsuranceDate' },
            { name: 'InsuranceProvider', index: 'InsuranceProvider' },
            { name: 'InsuranceConsultantName', index: 'InsuranceConsultantName' },
            { name: 'InsuranceDeclaredValue', index: 'InsuranceDeclaredValue' },
        //{ name: 'ValidityFromDate', width: 40, index: 'ValidityFromDate' },
        //{ name: 'ValidityToDate', width: 40, index: 'ValidityToDate' },
            {name: 'ICertificate', index: 'ICertificate' }
            ],
        pager: '#jqGridInsurancePager',
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1225,
        // shrinktofit: true,
        height: 105,
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
        caption: 'Insurance'
    });
    //$(window).triggerHandler('resize.jqGrid');
    $("#jqGridInsuranceJqGrid").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#jqGridInsuranceJqGrid").jqGrid('navGrid', '#jqGridInsurancePager',
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
            { url: '/Transport/DeleteInsuranceById' }, {})
    $("#jqGridInsuranceJqGrid").jqGrid('navButtonAdd', '#jqGridInsurancePager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            window.open('/Transport/InsuranceJqGrid?ExportType=Excel'
                    + '&VehicleNo=' + $("#gs_VehicleNo").val()
                    + '&InsuranceDate=' + $("#gs_InsuranceDate").val()
                    + '&NextInsuranceDate=' + $("#gs_NextInsuranceDate").val()
                    + '&InsuranceProvider=' + $("#gs_InsuranceProvider").val()
                    + '&InsuranceConsultantName=' + $("#gs_InsuranceConsultantName").val()
                    + '&InsuranceDeclaredValue=' + $("#gs_InsuranceDeclaredValue").val()
                    + '&ValidityFromDate=' + $("#gs_ValidityFromDate").val()
                    + '&ValidityToDate=' + $("#gs_ValidityToDate").val()
                    + '&ICertificate=' + $("#gs_ICertificate").val()
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
        $("#jqGridInsuranceJqGrid").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});

function InsuranceDetailsCreate() {
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var InsuranceDate = $('#Insurance_InsuranceDate').val();
    var NextInsuranceDate = $('#Insurance_NextInsuranceDate').val();
    var InsuranceProvider = $('#Insurance_InsuranceProvider').val();
    var InsuranceDeclaredValue = $('#Insurance_InsuranceDeclaredValue').val();
    //        var ValidityFromDate = $('#Insurance_ValidityFromDate').val();
    //        var ValidityToDate = $('#Insurance_ValidityToDate').val();
    var InsuranceConsultantName = $('#InsuranceConsultantName').val();
    var ICertificate = $('#ICertificate').val();
    //  InsTaxValidUpto = $("#InsTaxValidUpto").val();

    if (VehicleId == '' || VehicleNo == '' || InsuranceDate == '' || NextInsuranceDate == '' || InsuranceProvider == '' || InsuranceDeclaredValue == '' || InsuranceConsultantName == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/Transport/AddInsuranceDetails",
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, InsuranceDate: InsuranceDate, NextInsuranceDate: NextInsuranceDate, InsuranceProvider: InsuranceProvider, InsuranceDeclaredValue: InsuranceDeclaredValue, InsuranceConsultantName: InsuranceConsultantName, ICertificate: ICertificate },
        success: function (data) {
            ajaxUploadDocs(data);
            $("#jqGridInsuranceJqGrid").trigger('reloadGrid');
            $("input[type=text], textarea, select").val("");
        }
    });

    function ajaxUploadDocs(Id) {
        var AppName = 'INS';
        $.ajaxFileUpload({
            url: '/Transport/UploadDocuments?Id=' + Id + '&AppName=' + AppName,
            secureuri: false,
            fileElementId: 'ICertificate',
            dataType: 'json',
            success: function (data, status) {
                jQuery("#jqGridInsuranceJqGrid").setGridParam({ url: 'Transport/InsuranceJqGrid' }).trigger("reloadGrid");
            },
            error: function (data, status, e) {
            }
        });
    }

    $('#Insurance_Id').val("");
    $('#Insurance_InsuranceDate').val("");
    $('#Insurance_NextInsuranceDate').val("");
    $('#Insurance_InsuranceProvider').val("");
    $('#Insurance_InsuranceDeclaredValue').val("");
    $('#Insurance_ValidityFromDate').val("");
    $('#Insurance_ValidityToDate').val("");
    $('#InsuranceConsultantName').val("");
}

function uploaddat(id1, FileName) {
    var AppName = 'INS';
    window.location.href = "/Transport/uploaddisplay?Id=" + id1 + '&FileName=' + FileName + '&AppName=' + AppName;
}