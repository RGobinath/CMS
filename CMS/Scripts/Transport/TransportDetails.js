$(function () {
    //    debugger;

    $(".Fuel").attr("readonly", true).css("background-color", "#F5F5F5");
    $(".date-picker").attr("readonly", true).css("background-color", "#F5F5F5");
    var VehicleId = $("#hdnVehicleId").val();

    $("#btnBack").click(function () {

        window.location.href = $("#myUrl").val();
    });
    //------------Browse File-------------------------
    //$('#FCertificate').ace_file_input();
    //$('#ICertificate').ace_file_input();

    // FuelRefill
    var FuelRefillgrid_selector = "#FuelRefillListGrid";
    var FuelRefillpager_selector = "#FuelRefillListGridPager";
    // FinesandPenalities
    var FinesandPenalitiesgrid_selector = "#jqGridFinesAndPenalities";
    var FinesandPenalitiespager_selector = "#jqGridFinesAndPenalitiesPager";
    // FitnessCertificate
    var FitnessCertificategrid_selector = "#jqGridFitnessCertificate";
    var FitnessCertificatepager_selector = "#jqGridFitnessCertificatePager";
    // Insurance
    var Insurancegrid_selector = "#jqGridInsuranceJqGrid";
    var Insurancepager_selector = "#jqGridInsurancePager";
    // Permit
    var Permitgrid_selector = "#PermitJqGrid";
    var Permitpager_selector = "#PermitPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(FuelRefillgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(FinesandPenalitiesgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(FitnessCertificategrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(Insurancegrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(Permitgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var FuelRefill_column = $(FuelRefillgrid_selector).closest('[class*="col-"]');
    var FinesandPenalities_column = $(FinesandPenalitiesgrid_selector).closest('[class*="col-"]');
    var FitnessCertificate_column = $(FitnessCertificategrid_selector).closest('[class*="col-"]');
    var Insurance_column = $(Insurancegrid_selector).closest('[class*="col-"]');
    var Permit_column = $(Permitgrid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(FuelRefillgrid_selector).jqGrid('setGridWidth', FuelRefill_column.width());
                $(FinesandPenalitiesgrid_selector).jqGrid('setGridWidth', FinesandPenalities_column.width());
                $(FitnessCertificategrid_selector).jqGrid('setGridWidth', FitnessCertificate_column.width());
                $(Insurancegrid_selector).jqGrid('setGridWidth', Insurance_column.width());
                $(Permitgrid_selector).jqGrid('setGridWidth', Permit_column.width());
            }, 0);
        }
    })
    var Type = $("#Type").val();
    VehicleFuelReportChart(VehicleId);
    $("#btnNew").click(function () {
        window.location.href = '/Transport/FuelRefill';
    });
    //------------------------Fuel Refill---------------------------------
    jQuery(FuelRefillgrid_selector).jqGrid({
        url: '/Transport/FuelRefillListJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Fuel Type', 'Fuel Quantity', 'Litre Price', 'Total Price', 'Filled By', 'Filled Date', 'Bunk Name', 'Fuel Fill Type '
            , 'Last Milometer Reading', 'Current Milometer Reading', 'Mileage', 'Created Date', 'Created By', 'Status'],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo', width: 100 },
             { name: 'FuelType', index: 'FuelType', width: 100 },
             { name: 'FuelQuantity', index: 'FuelQuantity', width: 50 },
             { name: 'LitrePrice', index: 'LitrePrice', width: 50 },
             { name: 'TotalPrice', index: 'TotalPrice', width: 60 },
             { name: 'FilledBy', index: 'FilledBy', width: 100 },
             { name: 'FilledDate', index: 'FilledDate', width: 100 },
             { name: 'BunkName', index: 'BunkName', width: 100 },
             { name: 'FuelFillType', index: 'FuelFillType', width: 100 },
             { name: 'LastMilometerReading', index: 'LastMilometerReading', width: 100 },
             { name: 'CurrentMilometerReading', index: 'CurrentMilometerReading', width: 100 },
             { name: 'Mileage', index: 'Mileage' },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'Status', index: 'Status', hidden: true },
             ],
        pager: FuelRefillpager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 150,
        width:811,
        //autowidth: true,
        //shrinktofit: true,
        viewrecords: true,
        gridComplete: function () {
            RowList = $(FuelRefillgrid_selector).getDataIDs();
            if (RowList != null) {
                var rowData = jQuery(FuelRefillgrid_selector).jqGrid('getRowData', RowList[0]);
                if (rowData != null)
                    $("#LastMiloMeterReading").val(rowData.CurrentMilometerReading);
                else
                    $("#LastMiloMeterReading").val(0);
            }
        },
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            //$(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Fuel Refill List'
    });
    //navButtons
    jQuery(FuelRefillgrid_selector).jqGrid('navGrid', FuelRefillpager_selector,
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
                { url: '/Transport/DeleteFuelRefillId' }, {}).navButtonAdd(FuelRefillpager_selector, {
                    caption: '<i class="fa fa-file-excel-o"></i>&nbsp; Export Excel',
                    onClickButton: function () {
                        alert();
                        var VehicleNo = $("#gs_VehicleNo").val();
                        var FuelType = $("#gs_FuelType").val();
                        var FuelQuantity = $("#gs_FuelQuantity").val();
                        var FilledBy = $("#gs_FilledBy").val();
                        var FilledDate = $("#gs_FilledDate").val();
                        var BunkName = $("#gs_BunkName").val();
                        var FuelFillType = $("#gs_FuelFillType").val();
                        var LastMilometerReading = $("#gs_LastMilometerReading").val();
                        var CurrentMilometerReading = $("#gs_CurrentMilometerReading").val();
                        var Mileage = $("#gs_Mileage").val();
                        var CreatedDate = $("#gs_CreatedDate").val();
                        var CreatedBy = $("#gs_CreatedBy").val();
                        window.open("/Transport/FuelRefillListJqGrid" + '?ExportType=Excel'
                    + '&VehicleId=' + VehicleId
                    + '&VehicleNo=' + VehicleNo
                    + '&FuelQuantity=' + FuelQuantity
                    + '&FilledBy=' + FilledBy
                    + '&FilledDate=' + FilledDate
                    + '&BunkName=' + BunkName
                    + '&FuelFillType=' + FuelFillType
                    + '&LastMilometerReading=' + LastMilometerReading
                    + '&CurrentMilometerReading=' + CurrentMilometerReading
                    + '&Mileage=' + Mileage
                    + '&CreatedDate=' + CreatedDate
                    + '&CurrentMilometerReading=' + CreatedBy
                    + '&rows=9999');
                    }
                });

    $("#CurrentMiloMeterReading, #FuelQuantity").keyup(function () {

        CalculateMileage();
    });
    $("#FuelFillType").change(function () {

        CalculateMileage();
    });
    $("#FuelQuantity, #LitrePrice").keyup(function () {
        CalculateTotalPrice();
    });
    $("#FilledBy").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $(FuelRefillgrid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    function formateadorLink(cellvalue, options, rowObject) {

        return "<a href=/Transport/ShowFuelRefill?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
    //--------------------------FinesandPenalities-----------------------------------------
    jQuery(FinesandPenalitiesgrid_selector).jqGrid({
        url: '../../Transport/FinesAndPenalitiesjqgrid?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Penality Id', 'Vehicle Id', 'Vehicle No', 'Penality Date', 'Penality Area', 'Penality Reason', 'Penality Rupees', 'Penality Due Date', 'Penality Paid By', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 190 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 290 },
            { name: 'VehicleNo', index: 'VehicleNo', width: 90 },
            { name: 'PenalityDate', index: 'PenalityDate', width: 90 },
            { name: 'PenalityArea', index: 'PenalityArea', width: 90 },
            { name: 'PenalityReason', index: 'PenalityReason', width: 90 },
            { name: 'PenalityRupees', index: 'PenalityRupees', width: 90 },
            { name: 'PenalityDueDate', index: 'PenalityDueDate', width: 90 },
            { name: 'PenalityPaidBy', index: 'PenalityPaidBy', width: 90 },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true, width: 230 },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true, width: 230 }
            ],
        pager: FinesandPenalitiespager_selector,
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
        // width: 1225,
        // shrinktofit: true,
        //autowidth: true,
        height: 160,
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
        caption: 'Fines And Penalities'
    });
    //navButtons
    jQuery(FinesandPenalitiesgrid_selector).jqGrid('navGrid', FinesandPenalitiespager_selector,
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
            }, { url: '/Transport/EditeFines' }, {},
            { url: '/Transport/DeleteFinesAndPenalitiesById' }, {}).navButtonAdd(FinesandPenalitiespager_selector, {
                caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
                onClickButton: function () {
                    //var s=document.getElementById("gs_VehicleNo").value
                    //alert(s);
                    var VehiNo = $("#gs_VehicleNo").val();
                    var PenalityDate = $("#gs_PenalityDate").val();
                    var PenalityArea = $("#gs_PenalityArea").val();
                    var PenalityReason = $("#gs_PenalityReason").val();
                    var PenalityRupees = $("#gs_PenalityRupees").val();
                    var PenalityDueDate = $("#gs_PenalityDueDate").val();
                    var PenalityPaidBy = $("#gs_PenalityPaidBy").val();
                    var CreatedDate = $("#gs_CreatedDate").val();
                    var CreatedBy = $("#gs_CreatedBy").val();
                    window.open("/Transport/FinesAndPenalitiesjqgrid?ExportType=Excel"
                    + "&VehicleId=" + VehicleId
                    + "&VehicleNo=" + VehiNo
                    + '&PenalityDate=' + PenalityDate
                    + '&PenalityArea=' + PenalityArea
                    + '&PenalityReason=' + PenalityReason
                    + '&PenalityRupees=' + PenalityRupees
                    + '&PenalityDueDate=' + PenalityDueDate
                    + '&PenalityPaidBy=' + PenalityPaidBy
                    + '&CreatedDate=' + CreatedDate
                    + '&CreatedBy=' + CreatedBy
                    + '&rows=9999');
                }
            });
    $(FinesandPenalitiesgrid_selector).jqGrid('filterToolbar', { searchOnEnter: true });


    $("#PenalityDatePickerFine").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true
    });
    $("#PenalityDueDatePickerFine").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true
    });
    $("#PDriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                // alert(data);
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    //---------------------------FitnessCertificate------------------------------------
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
    jQuery(FitnessCertificategrid_selector).jqGrid({
        url: '/Transport/FitnessCertificateJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'FC Date', 'Next FC Date', 'FC Cost', 'Description', 'FC Work Carried At', 'RTO', 'Driver', 'Fitness Certificate'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 490 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 390 },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'FCDate', index: 'FCDate' },
            { name: 'NextFCDate', index: 'NextFCDate' },
            { name: 'FCCost', index: 'FCCost' },
            { name: 'Description', index: 'Description' },
            { name: 'FCWorkCarriedAt', index: 'FCWorkCarriedAt' },
            { name: 'RTO', index: 'RTO' },
            { name: 'Driver', index: 'Driver' },
            { name: 'FCertificate', index: 'FCertificate' }
            ],
        pager: FitnessCertificatepager_selector,
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1225,
        //        shrinktofit: true,
        height: 160,
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
    //navButtons
    jQuery(FitnessCertificategrid_selector).jqGrid('navGrid', FitnessCertificatepager_selector,
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
            { url: '/Transport/DeleteFitnessCertificateById' }, {}).navButtonAdd(FitnessCertificatepager_selector, {
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
            $(FitnessCertificategrid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
            $("#FCTaxValidUpto").datepicker({
                format: "dd/mm/yyyy",
                autoclose:true
            });
    

    //-------------------------------------Insurance------------------------------------
    jQuery(Insurancegrid_selector).jqGrid({
        url: '/Transport/InsuranceJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Insurance Date', 'Next Insurance Date', 'Insurance Provider', 'Insurance Consultant Name', 'Insurance Declared Value', 'Insurance Certificate'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 430 },
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
        pager: Insurancepager_selector,
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
        //        width: 1225,
        // shrinktofit: true,
        height: 150,
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
    //navButtons
    jQuery(Insurancegrid_selector).jqGrid('navGrid', Insurancepager_selector,
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
            { url: '/Transport/DeleteInsuranceById' }, {}).navButtonAdd(Insurancepager_selector, {
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
    $(Insurancegrid_selector).jqGrid('filterToolbar', { searchOnEnter: true });


    //---------------------------------------Permit-------------------------------------
    jQuery(Permitgrid_selector).jqGrid({
        url: '/Transport/PermitJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Permit No', 'Valid In', 'Valid From', 'Valid To', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 190 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 190 },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'PermitNo', index: 'PermitNo' },
            { name: 'ValidIn', index: 'ValidIn' },
            { name: 'ValidFrom', index: 'ValidFrom' },
            { name: 'ValidTo', index: 'ValidTo' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true, width: 190 },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true, width: 190 }
            ],
        pager: Permitpager_selector,
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 180,
        //shrinkToFit: true,
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
        caption: 'Permit'
    });
    //navButtons
    jQuery(Permitgrid_selector).jqGrid('navGrid', Permitpager_selector,
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
            { url: '/Transport/DeletePermitDetailsById' }, {}).navButtonAdd(Permitpager_selector, {
                caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
                onClickButton: function () {
                    window.open('/Transport/PermitJqGrid?ExportType=Excel'
                    + '&VehicleId=' + $("#gs_VehicleId").val()
                    + '&VehicleNo=' + $("#gs_VehicleNo").val()
                    + '&PermitNo=' + $("#gs_PermitNo").val()
                    + '&ValidIn=' + $("#gs_ValidIn").val()
                    + '&ValidFrom=' + $("#gs_ValidFrom").val()
                    + '&ValidTo=' + $("#gs_ValidTo").val()
                    + '&CreatedDate=' + $("#gs_CreatedDate").val()
                    + '&CreatedBy=' + $("#gs_CreatedBy").val()
                    + '&rows=9999');
                }
            });
    $(Permitgrid_selector).jqGrid('filterToolbar', { searchOnEnter: true });


});
//---------------------------For Fuel Refill-------------------------------------
function FuelRefillCreate() {
    debugger;
    var VehicleNo = $('#VehicleNo').val();
    var FuelType = $('#FuelType').val();
    var FuelQuantity = $('#FuelQuantity').val();
    var FilledDate = $('#FilledDate').val();
    var FilledBy = $('#FilledBy').val();
    var BunkName = $('#BunkName').val();
    var VehicleId = $("#hdnVehicleId").val();
    var Type = $("#Type").val();
    var FuelFillType = $("#FuelFillType").val();
    var LastMilometerReading = $("#LastMiloMeterReading").val();
    var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
    var LitrePrice = $("#LitrePrice").val();
    var TotalPrice = $("#TotalPrice").val();
    var Campus = $("#hdnCampus").val();

    if (VehicleNo == '' || FuelType == '' || FuelQuantity == '' || FilledDate == '' || FilledBy == '' || BunkName == '' || VehicleId == '' || Type == '' || FuelFillType == '' || LitrePrice == '' || TotalPrice == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    if (isNaN(FuelQuantity)) {
        ErrMsg('Numbers only allowed.');
        $('#FuelQuantity').focus();
        return false;
    }

    if (FuelFillType == "Full Tank") {
        var Mileage = (parseFloat(CurrentMilometerReading) - parseFloat(LastMilometerReading)) / parseFloat(FuelQuantity);
    }
    debugger;
    $.ajax({
        type: 'POST',
        url: "/Transport/AddFuelRefill",
        data: { VehicleId: VehicleId, Type: Type, VehicleNo: VehicleNo, FuelType: FuelType, FuelQuantity: FuelQuantity, FilledDate: FilledDate, FilledBy: FilledBy
            , BunkName: BunkName, FuelFillType: FuelFillType, LastMilometerReading: LastMilometerReading, CurrentMilometerReading: CurrentMilometerReading, Mileage: Mileage
            , LitrePrice: LitrePrice, TotalPrice: TotalPrice,Campus:Campus
        },
        success: function (data) {
            $("#FuelRefillListGrid").trigger('reloadGrid');
            //  $("input[type=text], textarea").val("");
            $('#FuelQuantity').val('');
            $('#FilledDate').val('');
            $('#FilledBy').val('');
            $('#BunkName').val('');
            $("#FuelFillType").val('');
            $("#CurrentMiloMeterReading").val('');
            $("#Distance").text('');
            $("#Mileage").text('');
            $("#LitrePrice").val('');
            $("#TotalPrice").val('');
        }
    });

    VehicleFuelReportChart(VehicleId)
}
function CalculateMileage() {
    var FuelFillType = $("#FuelFillType").val();
    if (FuelFillType == "Full Tank") {
        var FuelQuantity = $('#FuelQuantity').val();
        var LastMilometerReading = $("#LastMiloMeterReading").val();
        var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
        var Distance = (parseFloat(CurrentMilometerReading) - parseFloat(LastMilometerReading));
        var Mileage = Math.round(parseFloat(Distance) / parseFloat(FuelQuantity));
        $("#Distance").text(Distance);
        $("#Mileage").text(Mileage);
    }
    else {
        $("#Distance").text('');
        $("#Mileage").text('');
    }
}
function VehicleFuelReportChart(VehicleId) {
    $.ajax({
        type: 'Get',
        url: '/Transport/VehicleFuelReportChart/?VehicleId=' + VehicleId,
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_Column3D.swf", "productSales", "400", "220");
            chart.setDataXML(data);
            chart.render("VehicleFuelChart");
        },
        async: false,
        dataType: "text"
    });
}
function CalculateTotalPrice() {
    var FuelQuantity = $("#FuelQuantity").val();
    var LitrePrice = $("#LitrePrice").val();
    if (FuelQuantity != '' && LitrePrice != '')
        $("#TotalPrice").val(parseFloat(FuelQuantity) * parseFloat(LitrePrice));
    else
        $("#TotalPrice").val('');
}
//--------------------------FinesandPenalities-------------------------------------------------
function FineCreate() {
    var VehicleId = $("#hdnVehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var PenalityDatePicker = $('#PenalityDatePickerFine').val();
    var PenalityArea = $('#PenalityAreaFine').val();
    var PenalityReason = $('#PenalityReasonFine').val();
    var PenalityRupees = $('#PenalityRupeesFine').val();
    var PenalityDueDate = $('#PenalityDueDate').val();
    var PenalityPaidBy = $('#PenalityPaidByFine').val();
    var DriverName = $("#PDriverName").val();

    if (VehicleId == '' || VehicleNo == '' || PenalityDatePicker == '' || PenalityArea == '' || PenalityReason == '' || PenalityRupees == ''
         || PenalityDueDate == '' || PenalityPaidBy == '' || DriverName == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (!parseInt(PenalityRupees)) {
        ErrMsg('Numbers only allowed.');
        $('#PenalityRupeesFine').focus();
        return false;
    }

    $.ajax({
        type: 'POST',
        url: "/Transport/AddFinesAndPenalities",
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, PenalityDate: PenalityDatePicker, PenalityArea: PenalityArea, PenalityReason: PenalityReason, PenalityRupees: PenalityRupees, PenalityDueDate: PenalityDueDate, PenalityPaidBy: PenalityPaidBy, DriverName: DriverName },
        success: function (data) {
            $("#jqGridFinesAndPenalities").trigger('reloadGrid');
            $("input[type=text], textarea, select").val("");
        }
    });

    $('#PenalityDatePicker').val("");
    $('#PenalityArea').val("");
    $('#PenalityReason').val("");
    $('#PenalityRupees').val("");
    $('#PenalityDueDatePicker').val("");
    $('#PenalityPaidBy').val("");
}
//--------------------------------FitnessCertificate----------------------------
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
//---------------------------------Insurance----------------------------------------
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
//----------------------------------------Permit----------------------------------
function ValidatePermitDetails() {
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var PermitNo = $('#PermitNo').val();
    var ValidIn = $('#ValidIn').val();
    var ValidFrom = $('#ValidFrom').val();
    var ValidTo = $('#ValidTo').val();

    if (VehicleId == '' || VehicleNo == '' || PermitNo == '' || ValidIn == '' || ValidFrom == '' || ValidTo == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: "/Transport/AddPermitDetails",
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, PermitNo: PermitNo, ValidIn: ValidIn, ValidFrom: ValidFrom, ValidTo: ValidTo },
        success: function (data) {
            $("#PermitJqGrid").trigger('reloadGrid');
            $("input[type=text], textarea, select").val("");
        }
    });

    $('#PermitNo').val("");
    $('#ValidIn').val("");
    $('#ValidFrom').val("");
    $('#ValidTo').val("");
}


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